using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Extensions;

public static class LoggerExtensions
{
    extension(ILogger logger)
    {
        public void LogBotMessage(BotMessage message, object? properties = null)
        {
            if (message.From is null || string.IsNullOrWhiteSpace(message.Text))
                return;

            logger.LogBotInteraction(
                message.From,
                $"\"{message.Text}\"",
                new { message.ChatId, Properties = properties });
        }

        private void LogBotInteraction(BotUser user, string interaction, object? properties = null)
        {
            if (!logger.IsEnabled(LogLevel.Information))
                return;

            logger.LogInformation(
                "{UserInfo} ({UserId}) — {Interaction} | {@Properties}",
                user.ReadableInfo,
                user.Id,
                interaction,
                properties);
        }
    }
}
