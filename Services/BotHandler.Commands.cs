using ElkollegeAdmissionsBot.Extensions;
using ElkollegeAdmissionsBot.Helpers;
using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Services;

public partial class BotHandler
{
    public async Task OnCommandAsync(BotMessage message, string commandName,
        CancellationToken cancellationToken = default)
    {
        if (message.From is null || string.IsNullOrWhiteSpace(message.Text))
            return;

        logger.LogBotMessage(message);

        switch (commandName)
        {
            case BotCommands.StartCommand:
            {
                await client.SendMessageAsync(
                    chatId: message.ChatId,
                    menu: BotMenus.Test(_botUser?.FullName ?? string.Empty),
                    cancellationToken: cancellationToken);
                break;
            }
        }
    }
}
