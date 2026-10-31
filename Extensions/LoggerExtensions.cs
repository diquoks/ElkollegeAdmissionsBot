using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Extensions;

public static class LoggerExtensions
{
    extension(ILogger logger)
    {
        public void LogBotMessage(BotMessage message, object? properties = null) =>
            logger.LogBotInteraction(
                message.From,
                $"\"{message.Text}\"",
                new { message.ChatId, Properties = properties });

        public void LogBotCallbackQuery(BotCallbackQuery callbackQuery, object? properties = null) =>
            logger.LogBotInteraction(
                callbackQuery.From,
                callbackQuery.Data,
                new { callbackQuery.ChatId, Properties = properties });

        private void LogBotInteraction(BotUser user, string interaction, object? properties = null)
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
