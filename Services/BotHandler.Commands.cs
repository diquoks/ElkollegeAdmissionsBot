using ElkollegeAdmissionsBot.Extensions;
using ElkollegeAdmissionsBot.Helpers;
using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Services;

public partial class BotHandler
{
    public Task OnCommandAsync(BotMessage message, string commandName,
        CancellationToken cancellationToken = default)
    {
        logger.LogBotMessage(message);

        switch (commandName)
        {
            case BotCommands.StartCommand:
                return client.SendMessageAsync(
                    chatId: message.ChatId,
                    menu: BotMenus.Start(_botUser!.FullName),
                    cancellationToken: cancellationToken);
            default:
                return Task.CompletedTask;
        }
    }
}
