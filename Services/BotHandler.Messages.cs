using ElkollegeAdmissionsBot.Extensions;
using ElkollegeAdmissionsBot.Helpers;
using ElkollegeAdmissionsBot.Models.Bot;
using ElkollegeAdmissionsBot.Models.Database;

namespace ElkollegeAdmissionsBot.Services;

public partial class BotHandler
{
    public async Task OnMessageAsync(BotMessage message, CancellationToken cancellationToken = default)
    {
        logger.LogBotMessage(message);

        var state = await databaseManager.GetStateAsync(message.From.Id, cancellationToken);

        switch (state.Type)
        {
            case StateType.CustomQuestion:
            {
                await interconnectionService.ForwardToAdminGroupAsync(
                    originalMessage: message,
                    cancellationToken: cancellationToken);

                state.Clear();

                await client.SendReplyAsync(
                    chatId: message.ChatId,
                    messageId: message.Id,
                    menu: BotMenus.CustomQuestionAsked,
                    cancellationToken: cancellationToken);
                break;
            }
            default:
            {
                if (!interconnectionService.IsInAdminGroup(message) || message.LinkedMessageId is null)
                    return;

                var databaseMessage = await databaseManager.GetMessageAsync(message.LinkedMessageId, cancellationToken);

                if (databaseMessage is null)
                    return;

                await interconnectionService.SendAnswerFromAdminGroupAsync(
                    databaseMessage: databaseMessage,
                    menu: BotMenus.AnswerFromAdminGroup(message.Text),
                    cancellationToken: cancellationToken);
                return;
            }
        }

        await databaseManager.UpsertStateAsync(state, cancellationToken);
    }
}
