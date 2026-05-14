namespace SecretaryBot.Domain.Texts
{
    public static class CommandDocumentation
    {
        private const string StartDocumentation = "/start - Получить приветственное сообщение";
        private const string HelpDocumentation = "/help - Получить справочную информацию по всем командам";
        private const string AddCategoryDocumentation = "/addcategory - Добавить новую категорию затрат. Следующим сообщением необходимо будет ввести название категории";
        private static readonly string AddDefaultCategoriesDocumentation = "/adddefaultcategories - Добавить категории, которые предлагаюся по умолчанию. " +
            "Доступно только если на данный момент нет никаких категорий\nСписок активных категорий:\n"
            + string.Join("\n", DefaultCategoryNames.All);
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

        public static readonly string Commands = "Помните, что для всех команд, кроме /start и /help, необходимо получить доступ у моего создателя - @HiGGsino\n\n"
            + string.Join("\n\n", CommandsDescriptions);
    }
}
