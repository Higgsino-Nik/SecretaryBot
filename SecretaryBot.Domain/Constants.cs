using SecretaryBot.Domain.Models;

namespace SecretaryBot.Domain
{
    public static class Constants
    {
        public static List<Category> GetDefaultCategories() => DefaultCategories;

        public const string StartMessageResponse = "Вас приветствует HiggsinoSecretaryBot - персональный автоматизированный бот-секретарь.\n" +
            "Мой основной функционал - вести подсчет ваших затрат для вашего понимания, сколько и куда вы тратите свой бюджет.\n" +
            "Для получения справочной информации по доступным командам отправьте команду /help\n" +
            "Обращаю ваше внимание, что для использования функционала бота необходимо получить доступ. Для этого обратитесь к моему создателю @HiGGsino и предоставьте ему свой id: ";

        public const string ErrorMessageResponse = "Произошла непредвиденная ошибка во время выполнения команды. Пожалуйста, повторите попытку позже. " +
            "Если проблема сохраняется, пожалуйста, обратитесь к моему создателю @HiGGsino";
        public const string PermissionDeniedResponse = "У вас нет доступа к использованию функционала бота. Для получения доступа обратитесь к моему создателю @HiGGsino и предоставьте ему свой id: ";


        private static readonly List<Category> DefaultCategories =
        [
            new() { IsActive = true, Name = "Обычная еда/продукты" },
            new() { IsActive = true, Name = "Кафе/рестораны" },
            new() { IsActive = true, Name = "Одежда" },
            new() { IsActive = true, Name = "Тусы" },
            new() { IsActive = true, Name = "Транспорт" },
            new() { IsActive = true, Name = "ЖКХ, интернет, связь" },
            new() { IsActive = true, Name = "Здоровье/аптека" },
            new() { IsActive = true, Name = "Подписки" },
            new() { IsActive = true, Name = "Прочее" }
        ];

        

        #region Documentation
        private const string StartDocumentation = "/start - Получить приветственное сообщение";
        private const string HelpDocumentation = "/help - Получить справочную информацию по всем командам";
        private const string AddCategoryDocumentation = "/addcategory - Добавить новую категорию затрат. Следующим сообщением необходимо будет ввести название категории";
        private static readonly string AddDefaultCategoriesDocumentation = "/adddefaultcategories - Добавить категории, которые предлагаюся по умолчанию. " +
            "Доступно только если на данный момент нет никаких категорий\nСписок активных категорий:\n"
            + string.Join("\n",GetDefaultCategories().Select(x => x.Name));
        private const string GetCategoriesDocumentation = "/getcategories - Получить список ваших активных категорий";
        private const string DeleteCategoryDocumentation = "/deletecategory - Удалить категорию. После ввода команды нужно будет нажать на кнопку с соответствующей категорией. " +
            "Кнопки отобрязятся после ввода команды";
        private const string AddPurchaseDocumentation = "/addpurchase - Зафиксировать новую трату. После ввода команды нужно будет нажать на кнопку с категорией, " +
            "к которой вы хотите привязать трату. После выбора категории необходимо ввести целое число - сумма траты";
        private const string DeleteLastPurchaseDocumentation = "/deletelastpurchase - удалить последнюю добавленную трату. Полезно если, например, вы ошиблись при добавлении траты";
        private const string CurrentMonthPurchaseReportDocumentation = "/currentmonthpurchasereport - Краткий отчет по тратам за текущий месяц";
        private const string LastMonthPurchaseReportDocumentation = "/lastmonthpurchasereport - Краткий отчет по тратам за прошлый месяц";
        private const string LastMonthCategoryReportDocumentation = "/lastmonthcategoryreport - Отчет по выбранной категории за прошлый месяц. После ввода команды нужно " +
            "будет нажать на кнопку с категорией, по которой вы хотите посмотреть отчет";
        private const string CurrentMonthCategoryReportDocumentation = "/currentmonthcategoryreport - Отчет по выбранной категории за текущий месяц. После ввода команды нужно " +
            "будет нажать на кнопку с категорией, по которой вы хотите посмотреть отчет";
        #endregion

        private static readonly List<string> CommandsDescriptions =
        [
            StartDocumentation,
            HelpDocumentation,
            AddCategoryDocumentation,
            AddDefaultCategoriesDocumentation,
            GetCategoriesDocumentation,
            DeleteCategoryDocumentation,
            AddPurchaseDocumentation,
            DeleteLastPurchaseDocumentation,
            CurrentMonthPurchaseReportDocumentation,
            LastMonthPurchaseReportDocumentation,
            CurrentMonthCategoryReportDocumentation,
            LastMonthCategoryReportDocumentation
        ];

        public static readonly string CommandsDocumentation = "Помните, что для всех команд, кроме /start и /help, необходимо получить доступ у моего создателя - @HiGGsino\n\n"
            + string.Join("\n\n", CommandsDescriptions);
    }
}
