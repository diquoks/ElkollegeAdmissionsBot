using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Extensions;

public static class LoggerExtensions
{
    extension(ILogger logger)
    {
        public void LogBotMessage(BotMessage message) =>
            logger.InternalLogBotInteraction(message.From, $"\"{message.Text}\"", new { message.ChatId });

        public void LogBotCallbackQuery(BotCallbackQuery callbackQuery) =>
            logger.InternalLogBotInteraction(callbackQuery.From, callbackQuery.Data, new { callbackQuery.ChatId });

        public void LogBotInteraction(BotUser user, string interaction, long chatId) =>
            logger.InternalLogBotInteraction(user, interaction, new { ChatId = chatId });

        private void InternalLogBotInteraction(BotUser user, string interaction, object? properties = null)
        {
            if (!logger.IsEnabled(LogLevel.Information))
                return;

            logger.LogInformation(
                "{UserInfo} ({UserId}) — {Interaction} | {@Properties}",
                !string.IsNullOrWhiteSpace(user.Username) ? $"{user.FullName} | @{user.Username}" : user.FullName,
                user.Id,
                interaction,
                properties);
        }
    }
}
