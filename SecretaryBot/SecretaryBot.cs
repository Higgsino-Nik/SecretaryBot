using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SecretaryBot.Domain;
using SecretaryBot.Domain.Abstractions;
using SecretaryBot.Domain.Exceptions;
using SecretaryBot.Domain.Models;
using SecretaryBot.Domain.Texts;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SecretaryBot
{
    public class SecretaryBot : BackgroundService
    {
        private readonly ITelegramBotClient _bot;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SecretaryBot(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;

            var botToken = configuration["SecretaryToken"];
            if (string.IsNullOrEmpty(botToken))
            {
                throw new ArgumentException("SecretaryToken was not configured");
            }
            
            _bot = new TelegramBotClient(botToken);
        }
        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _bot.StartReceiving(HandleUpdateAsync, HandleErrorAsync, cancellationToken: stoppingToken);
            return Task.CompletedTask;
        }

        private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            try
            {
                var message = update.Type switch
                {
                    Telegram.Bot.Types.Enums.UpdateType.Message => new TelegramMessage(update.Message.From.Id, update.Message.Chat.Id, update.Message.Text),
                    Telegram.Bot.Types.Enums.UpdateType.CallbackQuery => new TelegramMessage(update.CallbackQuery.From.Id, update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Data),
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
                await WriteLogAsync(LogLevel.Error, "Error while handling update message: " + ex.Message);
                await bot.SendTextMessageAsync(update.Message?.Chat ?? update.CallbackQuery?.Message?.Chat, BotMessages.ErrorMessage);
            }
        }

        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            await WriteLogAsync(LogLevel.Error, "Error received: " + exception.Message);
            Environment.Exit(-1);
        }

        private async Task AnswerToMessageAsync(TelegramMessage message)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var commandController = scope.ServiceProvider.GetRequiredService<CommandsController>();
            var response = await commandController.ResponseCommandAsync(message);

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

        private async Task WriteLogAsync(LogLevel level, string message)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ICustomLogger>();
            await logger.WriteLogAsync(level, message);
        }
    }
}
