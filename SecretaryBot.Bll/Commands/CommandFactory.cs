using Microsoft.Extensions.DependencyInjection;
using SecretaryBot.Bll.Commands.Category;
using SecretaryBot.Bll.Commands.Purchase;
using SecretaryBot.Bll.Commands.Report;
using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Exceptions;

namespace SecretaryBot.Bll.Commands
{
    public class CommandFactory(IServiceProvider serviceProvider)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public ICommand CreateCommand(string commandText)
        {
            var baseCommand = commandText.Split('\\')[0];
            return baseCommand switch
            {
                "/start" => _serviceProvider.GetRequiredService<StartCommand>(),
                "/help" => _serviceProvider.GetRequiredService<HelpCommand>(),
                "/category" => _serviceProvider.GetRequiredService<CategoryScopeCommand>(),
                "/purchase" => _serviceProvider.GetRequiredService<PurchaseScopeCommand>(),
                "/report" => _serviceProvider.GetRequiredService<ReportScopeCommand>(),

                "/addcategory" => _serviceProvider.GetRequiredService<AddCategoryCommand>(),
                "/adddefaultcategories" => _serviceProvider.GetRequiredService<AddDefaultCategoriesCommand>(),
                "/getcategories" => _serviceProvider.GetRequiredService<GetCategoriesCommand>(),
                "/deletecategory" => _serviceProvider.GetRequiredService<DeleteCategoryCommand>(),
                "/addpurchase" => _serviceProvider.GetRequiredService<AddPurchaseCommand>(),
                "/deletelastpurchase" => _serviceProvider.GetRequiredService<DeleteLastPurchaseCommand>(),
                "/currentmonthpurchasereport" => _serviceProvider.GetRequiredService<CurrentMonthPurchaseReportCommand>(),
                "/lastmonthpurchasereport" => _serviceProvider.GetRequiredService<LastMonthPurchaseReportCommand>(),
                "/lastmonthcategoryreport" => _serviceProvider.GetRequiredService<LastMonthCategoryReportCommand>(),
                "/currentmonthcategoryreport" => _serviceProvider.GetRequiredService<CurrentMonthCategoryReportCommand>(),
                _ => throw new BadCommandRequestException("Команда не найдена")
            };
        }
    }
}
