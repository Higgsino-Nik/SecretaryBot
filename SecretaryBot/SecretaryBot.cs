using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SecretaryBot.Domain;
using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Exceptions;
using SecretaryBot.Domain.Models;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SecretaryBot
{
    public class SecretaryBot : IHostedService
    {
        private readonly ITelegramBotClient _bot;
        private readonly CommandsController _commandController;
        private readonly ICustomLogger _logger;

        public SecretaryBot(IConfiguration configuration, CommandsController commandsFactory, Domain.Abstractions.ICustomLogger logger)
        {
            _commandController = commandsFactory;
            _logger = logger;

            var botToken = configuration["SecretaryToken"];
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
                var message = update.Type switch
                {
                    Telegram.Bot.Types.Enums.UpdateType.Message => new TelegramMessage(update.Message.From.Id, update.Message.Chat.Id, update.Message.Text),
                    Telegram.Bot.Types.Enums.UpdateType.CallbackQuery => new TelegramMessage(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Data),
                    _ => throw new Exception("Unknown UpdateType")
                };
                await AnswerToMessageAsync(message);
            }
            catch (BadCommandRequestException ex)
            {
                await bot.SendTextMessageAsync(update.Message?.Chat ?? update.CallbackQuery.Message.Chat, ex.Message);
            }
            catch (Exception ex)
            {
                await _logger.WriteLogAsync(LogLevel.Error, "Error while handling update message: " + ex.Message);
                await bot.SendTextMessageAsync(update.Message?.Chat ?? update.CallbackQuery.Message.Chat, Constants.ErrorMessageResponse);
            }
        }

        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            await _logger.WriteLogAsync(LogLevel.Error, "Error received: " + exception.Message);
            Environment.Exit(-1);
        }

        private async Task AnswerToMessageAsync(TelegramMessage message)
        {
            var response = await _commandController.ResponseCommandAsync(message);

            if (response.Buttons.Count > 0)
            {
                var responseButtons = response.Buttons.Select(button => new[] { InlineKeyboardButton.WithCallbackData(button.Text, button.CallBackMessage) });
                var markup = new InlineKeyboardMarkup(responseButtons);
                await _bot.SendTextMessageAsync(message.ChatId, response.ResponseText, replyMarkup: markup);
            }
            else
            {
                await _bot.SendTextMessageAsync(message.ChatId, response.ResponseText);
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
