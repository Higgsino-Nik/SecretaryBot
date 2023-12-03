using SecretaryBot.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace SecretaryBot
{
    public class CommandResponseFactory
    {
        private readonly IRepository _repository;

        public CommandResponseFactory(IRepository repository)
        {
            _repository = repository;
        }


        // Подумать как хранить ряд сообщений от одного пользователя - оперативка, бд или еще че. Чтоб команда, затем к ней написал что-то

        public string ResponseCommand(Message request)
        {
            return request.Text switch
            {
                "/start" => Start(request),
                "/help" => GetCommandsDocumentation(request),
                "/addCategory" => AddCategory(request),
                "/addDefaultCategories" => AddDefaultCategories(request),
                "/deleteCategory" => DeleteCategory(request),
                "/addExpense" => AddExpense(request),
                "/deleteLastExpense" => DeleteLastExpense(request),
                "/currentMonthExpenseReport" => CurrentMonthExpenseReport(request),
                "/lastMonthExpenseReport" => LastMonthExpenseReport(request),
                "/baseExpsenseReport" => BaseExpsenseReport(request),
                "/yearExpenseReportExcel" => YearExpenseReportExcel(request),
                "/addExpectedCategoryExpense" => AddExpectedCategoryExpense(request),
                "/getExpectedExpensies" => GetExpectedExpensies(request),
                _ => "Команда не найдена"
            };
        }

        private string Start(Message message)
        {
            //_repository.WriteLog("Received start" message.From)
            return "Вас приветствует HiggsinoSecretaryBot - персональный автоматизированный бот-секретарь.";
        }

        private string GetCommandsDocumentation(Message message)
        {
            throw new NotImplementedException();
        }

        private string AddCategory(Message message)
        {
            throw new NotImplementedException();
        }

        private string AddDefaultCategories(Message message)
        {
            throw new NotImplementedException();
        }

        private string DeleteCategory(Message message)
        {
            throw new NotImplementedException();
        }

        private string AddExpense(Message message)
        {
            throw new NotImplementedException();
        }

        private string DeleteLastExpense(Message message)
        {
            throw new NotImplementedException();
        }

        private string CurrentMonthExpenseReport(Message message)
        {
            throw new NotImplementedException();
        }

        private string LastMonthExpenseReport(Message message)
        {
            throw new NotImplementedException();
        }

        private string BaseExpsenseReport(Message message)
        {
            throw new NotImplementedException();
        }

        private string YearExpenseReportExcel(Message message)
        {
            throw new NotImplementedException();
        }

        private string AddExpectedCategoryExpense(Message message)
        {
            throw new NotImplementedException();
        }

        private string GetExpectedExpensies(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
