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

        var state = await databaseManager.GetStateAsync(
            userId: message.From.Id,
            messengerType: message.MessengerType,
            cancellationToken: cancellationToken);

        switch (state.Type)
        {
            case StateType.CustomQuestion:
            {
                await HandleCustomQuestionMessageAsync(message, state, cancellationToken);
                break;
            }
            default:
            {
                await HandleDefaultMessageAsync(message, cancellationToken);
                break;
            }
        }
    }

    private async Task HandleCustomQuestionMessageAsync(BotMessage message, DatabaseState state,
        CancellationToken cancellationToken = default)
    {
        if (state.IsBlocked)
        {
            state.ResetType();
            return;
        }

        await interconnectionService.ForwardToAdminGroupAsync(
            originalMessage: message,
            cancellationToken: cancellationToken);

        state.ResetType();

        await client.SendReplyAsync(
            chatId: message.ChatId,
            messageId: message.Id,
            menu: BotMenus.CustomQuestionAsked,
            cancellationToken: cancellationToken);

        await databaseManager.UpsertStateAsync(
            state: state,
            cancellationToken: cancellationToken);
    }

    private async Task HandleDefaultMessageAsync(BotMessage message,
        CancellationToken cancellationToken = default)
    {
        if (!interconnectionService.IsInAdminGroup(message) || message.LinkedMessageId is null)
        {
            await client.SendReplyAsync(
                chatId: message.ChatId,
                messageId: message.Id,
                menu: BotMenus.Default,
                cancellationToken: cancellationToken);
            return;
        }

        var databaseMessage = await databaseManager.GetMessageAsync(
            id: message.LinkedMessageId,
            cancellationToken: cancellationToken);

        if (databaseMessage is null)
            return;

        var targetState = await databaseManager.GetStateAsync(
            userId: databaseMessage.OriginalFromId,
            messengerType: databaseMessage.OriginalMessengerType,
            cancellationToken: cancellationToken);

        await interconnectionService.SendAnswerFromAdminGroupAsync(
            databaseMessage: databaseMessage,
            menu: BotMenus.AnswerFromAdminGroup(
                text: message.Text,
                withCustomQuestionButton: !(interconnectionService.IsInAdminGroup(message) || targetState.IsBlocked)),
            cancellationToken: cancellationToken);
    }
}
