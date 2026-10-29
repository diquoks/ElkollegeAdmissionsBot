using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Extensions;

public static class LoggerExtensions
{
    extension(ILogger logger)
    {
        public void LogBotInteraction(BotUser user, string interaction, object? properties = null)
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
