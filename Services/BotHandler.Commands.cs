using ElkollegeAdmissionsBot.Extensions;
using ElkollegeAdmissionsBot.Helpers;
using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Services;

public partial class BotHandler
{
    public async Task OnCommandAsync(BotMessage message, string commandName,
        CancellationToken cancellationToken = default)
    {
        logger.LogBotMessage(message);

        switch (commandName)
        {
            case BotCommands.StartCommand:
            {
                await HandleStartCommandAsync(message, cancellationToken);
                break;
            }
        }
    }

    public async Task HandleStartCommandAsync(BotUpdate update, CancellationToken cancellationToken = default)
    {
        var state = await databaseManager.GetStateAsync(
            userId: update.From.Id,
            messengerType: update.MessengerType,
            cancellationToken: cancellationToken);

        await client.SendMessageAsync(
            chatId: update.ChatId,
            menu: BotMenus.Start(
                botName: _botUser.FullName,
                faqTopics: faqProvider.Faq.Topics,
                withCustomQuestionButton: !(interconnectionService.IsInAdminGroup(update) || state.IsBlocked)),
            cancellationToken: cancellationToken);
    }
}
