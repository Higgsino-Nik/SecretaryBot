using Microsoft.Extensions.Configuration;
using SecretaryBot.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace SecretaryBot
{
    public class SecretaryBot : ISecretaryBot
    {
        private readonly ITelegramBotClient _bot;
        private readonly CommandResponseFactory _commandService;
        private readonly IRepository _repository;

        public SecretaryBot(string botToken, string dbConnectionString)
        {
            _repository = new Repository(dbConnectionString);
            _commandService = new CommandResponseFactory(_repository);

            _bot = new TelegramBotClient(botToken);
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

            _bot.StartReceiving(
                    HandleUpdateAsync,
                    HandleErrorAsync,
                    receiverOptions,
                    new CancellationTokenSource().Token
                );
        }

        public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            try
            {
                switch (update.Type)
                {
                    case Telegram.Bot.Types.Enums.UpdateType.Message:

                        var a = update.Message;
                        if (update.Message is not null) AnswerToMessage(update.Message);
                        break;
                }
            }
            catch (Exception ex)
            {
                await _repository.WriteLog("Error while handling update message: " + ex.Message);
            }
        }

        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия

            await _repository.WriteLog("Error received: " + exception.Message);
            Console.WriteLine(JsonSerializer.Serialize(exception));
            return;
        }

        private void AnswerToMessage(Message message)
        {
            if (message.Text.StartsWith("/"))
            {
                _commandService.ResponseCommand(message);
            }
        }
    }
}
