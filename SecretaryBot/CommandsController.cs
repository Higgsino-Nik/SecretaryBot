using Microsoft.Extensions.Configuration;
using SecretaryBot.Domain;
using SecretaryBot.Domain.Abstractions.Repositories;
using SecretaryBot.Domain.Abstractions.Services;
using SecretaryBot.Domain.Models;

namespace SecretaryBot
{
    public class CommandsController(IDbLogger logger, ICategoryService categoryService, IPurchaseService purchaseService, ICacheService cacheService, 
        IUserService userService, IReportService reportService, IConfiguration configuration)
    {
        private readonly IDbLogger _logger = logger;
        private readonly ICacheService _cacheService = cacheService;
        private readonly ICategoryService _categoryService = categoryService;
        private readonly IPurchaseService _purchaseService = purchaseService;
        private readonly IUserService _userService = userService;
        private readonly IReportService _reportService = reportService;

        private readonly long ADMIN_ID = long.Parse(configuration["AdminId"]);

        public async Task<CommandResult> SendParameterToLastCommandAsync(TelegramMessage request)
        {
            if (request.Text.StartsWith("changeaccess", StringComparison.CurrentCultureIgnoreCase) && request.UserId == ADMIN_ID)
            {
                var id = request.Text.Split(' ')[1];
                await _userService.ChangeAccessAsync(long.Parse(id));
                return new CommandResult("Доступ успешно выдан");
            }

            var lastCommand = _cacheService.GetLastCommand(request.UserId);
            await _logger.Info($"Received parameter for command {lastCommand}. User id: {request.UserId}");

            string responseMessage;
            if (!string.IsNullOrEmpty(lastCommand) && lastCommand.StartsWith("/addpurchase ", StringComparison.CurrentCultureIgnoreCase))
            {
                if (!int.TryParse(request.Text, out var amount))
                    return new CommandResult("Возникла ошибка при фиксации новой траты. Пожалуйста, в качестве суммы траты введите целое число");
                var categoryId = int.Parse(lastCommand[13..]);
                var category = await _categoryService.GetCategory(categoryId);
                if (category.UserTelegramId != request.UserId)
                    return new CommandResult("Возникла ошибка. Выбранная категория не найдена либо не принадлежит вам");
                
                responseMessage = await _purchaseService.AddPurchaseAsync(categoryId, amount);
                return new CommandResult(responseMessage);
            }

            var currentMonthFirstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var nextMonthFirstDay = currentMonthFirstDay.AddMonths(1);
            var previousMonthFirstDay = currentMonthFirstDay.AddMonths(-1);

            try
            {
                responseMessage = lastCommand switch
                {
                    "/addcategory" => await _categoryService.AddCategoryAsync(request.UserId, request.Text),
                    "/deletecategory" => await _categoryService.DeleteCategoryAsync(request.UserId, int.Parse(request.Text)),
                    "/addpurchase" => await StartAddCurrentPurchaseCategoryAsync(request),
                    "/currentmonthcategoryreport" => await _reportService.GetCategoryReportAsync(int.Parse(request.Text), currentMonthFirstDay, nextMonthFirstDay),
                    "/lastmonthcategoryreport" => await _reportService.GetCategoryReportAsync(int.Parse(request.Text), previousMonthFirstDay, currentMonthFirstDay),
                    _ => "Не найдена последняя команда либо истекло время ожидания ввода. Пожалуйста, введите команду повторно"
                };
            }
            catch (FormatException)
            {
                return new CommandResult("Произошла ошибка. Необходимо выбрать категорию из списка выше (нажатием на соответствующую кнопку)");
            }

            return new CommandResult(responseMessage);
        }

        public async Task<CommandResult> ResponseCommandAsync(TelegramMessage request)
        {
            await _logger.Info($"Received start. UserId: {request.UserId}");

            var hasAccess = await _userService.HasAccessAsync(request.UserId);
            if (request.Text != "/start" && request.Text != "/help" && !hasAccess)
                return new CommandResult(Constants.PermissionDeniedResponse + request.UserId);

            var responseMessage = request.Text switch
            {
                "/start" => await StartAsync(request),
                "/help" => await GetCommandsDocumentationAsync(request),
                "/addcategory" => await StartAddCategoryAsync(request),
                "/adddefaultcategories" => await AddDefaultCategoriesAsync(request),
                "/getcategories" => await GetCategoriesAsync(request),
                "/deletecategory" => await StartDeleteCategoryAsync(request),
                "/addpurchase" => await StartAddPurchaseAsync(request),
                "/deletelastpurchase" => await DeleteLastPurchaseAsync(request),
                "/currentmonthpurchasereport" => await CurrentMonthPurchaseReportAsync(request),
                "/lastmonthpurchasereport" => await LastMonthPurchaseReportAsync(request),
                "/lastmonthcategoryreport" => await LastMonthCategoryReportAsync(request),
                "/currentmonthcategoryreport" => await CurrentMonthCategoryReportAsync(request),
                "/baseexpsensereport" => await BasePurchaseReportAsync(request),
                "/yearpurchasereportexcel" => await YearPurchaseReportExcelAsync(request),
                "/addexpectedcategorypurchase" => await AddExpectedCategoryPurchaseAsync(request),
                "/getexpectedpurchases" => await GetExpectedExpensiesAsync(request),
                _ => "Команда не найдена"
            };

            var buttons = request.Text switch
            {
                "/deletecategory" or "/addpurchase" or "/lastmonthcategoryreport" or "/currentmonthcategoryreport" =>
                    (await _categoryService.GetCategoriesListAsync(request.UserId))
                    .Select(x => new KeyboardButton { Text = x.Name, CallBackMessage = x.Id.ToString() }),
                _ => []
            };

            return new CommandResult(responseMessage, buttons);
        }

        private async Task<string> StartAsync(TelegramMessage message)
        {
            await _logger.Info($"Received start. UserId: {message.UserId}");
            await _userService.AddUserAsync(message.UserId, message.ChatId);
            return Constants.StartMessageResponse + message.UserId;
        }

        private async Task<string> GetCommandsDocumentationAsync(TelegramMessage message)
        {
            await _logger.Info($"Received help. UserId: {message.UserId}");
            return Constants.CommandsDocumentation;
        }

        private async Task<string> StartAddCategoryAsync(TelegramMessage message)
        {
            await _logger.Info($"Received AddCategory. UserId: {message.UserId}");
            _cacheService.AddLastCommand(message.UserId, message.Text);
            return "Введите название новой категории";
        }

        private async Task<string> AddDefaultCategoriesAsync(TelegramMessage message)
        {
            await _logger.Info($"Received AddDefaultCategoriesAsync. UserId: {message.UserId}");
            var result = await _categoryService.AddDefaultCategoriesAsync(message.UserId);
            return result is null
                ? "Добавление категорий по умолчанию возможно только если на данный момент нет никаких категорий"
                : "Были добавлены дефолтные категории:\n\n" + result;                
        }

        private async Task<string> GetCategoriesAsync(TelegramMessage message)
        {
            await _logger.Info($"Received GetCategoriesAsync. UserId: {message.UserId}");
            return await _categoryService.GetCategoriesAsync(message.UserId);
        }

        private async Task<string> StartDeleteCategoryAsync(TelegramMessage message)
        {
            await _logger.Info($"Received DeleteCategoryAsync. UserId: {message.UserId}");
            _cacheService.AddLastCommand(message.UserId, message.Text);
            return "Выберите категорию, которую хотите удалить";
        }

        private async Task<string> StartAddPurchaseAsync(TelegramMessage message)
        {
            await _logger.Info($"Received AddPurchaseAsync. UserId: {message.UserId}");
            _cacheService.AddLastCommand(message.UserId, message.Text);
            return "Выберите категорию";
        }

        private async Task<string> StartAddCurrentPurchaseCategoryAsync(TelegramMessage message)
        {
            await _logger.Info($"Received AddPurchaseAsync. UserId: {message.UserId}");
            if (!int.TryParse(message.Text, out var categoryId))
            {
                return "Возникла ошибка при выборе категории. Пожалуйста, выберите категорию из списка выше (нажатием соответствующей кнопки)";
            }

            _cacheService.AddLastCommand(message.UserId, "/addpurchase " + categoryId);
            return "Введите ЦЕЛОЕ ЧИСЛО - сумма совершенной траты";
        }

        private async Task<string> DeleteLastPurchaseAsync(TelegramMessage message)
        {
            await _logger.Info($"Received DeleteLastPurchaseAsync. UserId: {message.UserId}");
            return await _purchaseService.DeleteLastPurchaseAsync(message.UserId);
        }

        private async Task<string> CurrentMonthPurchaseReportAsync(TelegramMessage message)
        {
            await _logger.Info($"Received CurrentMonthPurchaseReportAsync. UserId: {message.UserId}");
            var start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var end = start.AddMonths(1);
            return await _reportService.GetPurchaseReportAsync(message.UserId, start, end);
        }

        private async Task<string> LastMonthPurchaseReportAsync(TelegramMessage message)
        {
            await _logger.Info($"Received LastMonthPurchaseReportAsync. UserId: {message.UserId}");
            var end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var start = end.AddMonths(-1);
            return await _reportService.GetPurchaseReportAsync(message.UserId, start, end);
        }

        private async Task<string> LastMonthCategoryReportAsync(TelegramMessage message)
        {
            await _logger.Info($"Received LastMonthCategoryReportAsync. UserId: {message.UserId}");
            _cacheService.AddLastCommand(message.UserId, message.Text);
            return "Выберите категорию";
        }

        private async Task<string> CurrentMonthCategoryReportAsync(TelegramMessage message)
        {
            await _logger.Info($"Received CurrentMonthCategoryReportAsync. UserId: {message.UserId}");
            _cacheService.AddLastCommand(message.UserId, message.Text);
            return "Выберите категорию";
        }

        private async Task<string> BasePurchaseReportAsync(TelegramMessage message)
        {
            throw new NotImplementedException();
        }

        private async Task<string> YearPurchaseReportExcelAsync(TelegramMessage message)
        {
            throw new NotImplementedException();
        }

        private async Task<string> AddExpectedCategoryPurchaseAsync(TelegramMessage message)
        {
            throw new NotImplementedException();
        }

        private async Task<string> GetExpectedExpensiesAsync(TelegramMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
