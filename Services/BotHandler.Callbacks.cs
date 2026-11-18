using ElkollegeAdmissionsBot.Extensions;
using ElkollegeAdmissionsBot.Helpers;
using ElkollegeAdmissionsBot.Models.Bot;
using ElkollegeAdmissionsBot.Models.Database;

namespace ElkollegeAdmissionsBot.Services;

public partial class BotHandler
{
    public async Task OnCallbackQueryAsync(BotCallbackQuery callbackQuery,
        CancellationToken cancellationToken = default)
    {
        logger.LogBotCallbackQuery(callbackQuery);

        switch (callbackQuery.Data.Split(CallbackStrings.CallbackSeparator))
        {
            case [CallbackStrings.Start]:
            {
                await HandleStartCallbackAsync(callbackQuery, cancellationToken);
                break;
            }
            case [CallbackStrings.Topic, var faqTopicKey]:
            {
                await HandleTopicCallbackAsync(callbackQuery, faqTopicKey, cancellationToken);
                break;
            }
            case [CallbackStrings.Section, var faqTopicKey, var faqSectionKey]:
            {
                await HandleSectionCallbackAsync(callbackQuery, faqTopicKey, faqSectionKey, cancellationToken);
                break;
            }
            case [CallbackStrings.AskCustomQuestion]:
            {
                await HandleAskCustomQuestionCallbackAsync(callbackQuery, cancellationToken);
                break;
            }
            case [CallbackStrings.BlockUser, var targetUserIdString, var targetMessengerTypeString]
                when long.TryParse(targetUserIdString, out var targetUserId) &&
                     Enum.TryParse<BotMessengerType>(targetMessengerTypeString, out var targetMessengerType):
            {
                await HandleBlockUserCallbackAsync(callbackQuery, targetUserId, targetMessengerType, cancellationToken);
                break;
            }
            case [CallbackStrings.JustAnswer]:
                break; // special handling isn't needed for this callback.
        }

        await client.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            cancellationToken: cancellationToken);
    }

    private async Task HandleStartCallbackAsync(BotCallbackQuery callbackQuery,
        CancellationToken cancellationToken = default)
    {
        var state = await databaseManager.GetStateAsync(
            userId: callbackQuery.From.Id,
            messengerType: callbackQuery.MessengerType,
            cancellationToken: cancellationToken);

        state.ResetType();

        await client.EditMessageTextAsync(
            chatId: callbackQuery.ChatId,
            messageId: callbackQuery.MessageId,
            menu: BotMenus.Start(
                botName: _botUser.FullName,
                faqTopics: faqProvider.Faq.Topics,
                withCustomQuestionButton: !(interconnectionService.IsInAdminGroup(callbackQuery) || state.IsBlocked)),
            cancellationToken: cancellationToken);

        await databaseManager.UpsertStateAsync(
            state: state,
            cancellationToken: cancellationToken);
    }

    private async Task HandleTopicCallbackAsync(BotCallbackQuery callbackQuery, string faqTopicKey,
        CancellationToken cancellationToken = default)
    {
        var faqTopic = faqProvider.Faq.Topics.GetValueOrDefault(faqTopicKey);

        if (faqTopic is null)
            return;

        var state = await databaseManager.GetStateAsync(
            userId: callbackQuery.From.Id,
            messengerType: callbackQuery.MessengerType,
            cancellationToken: cancellationToken);

        await client.EditMessageTextAsync(
            chatId: callbackQuery.ChatId,
            messageId: callbackQuery.MessageId,
            menu: BotMenus.Topic(
                faqTopic: faqTopic,
                faqTopicKey: faqTopicKey,
                faqSections: faqProvider.Faq.Sections
                    .Where(pair => faqTopic.Sections.Contains(pair.Key))
                    .ToDictionary(),
                withCustomQuestionButton: !(interconnectionService.IsInAdminGroup(callbackQuery) || state.IsBlocked)),
            cancellationToken: cancellationToken);
    }

    private async Task HandleSectionCallbackAsync(BotCallbackQuery callbackQuery, string faqTopicKey,
        string faqSectionKey, CancellationToken cancellationToken = default)
    {
        var faqSection = faqProvider.Faq.Sections.GetValueOrDefault(faqSectionKey);

        if (faqSection is null)
            return;

        var state = await databaseManager.GetStateAsync(
            userId: callbackQuery.From.Id,
            messengerType: callbackQuery.MessengerType,
            cancellationToken: cancellationToken);

        await client.EditMessageTextAsync(
            chatId: callbackQuery.ChatId,
            messageId: callbackQuery.MessageId,
            menu: BotMenus.Section(
                faqSection: faqSection,
                faqTopicKey: faqTopicKey,
                withCustomQuestionButton: !(interconnectionService.IsInAdminGroup(callbackQuery) || state.IsBlocked)),
            cancellationToken: cancellationToken);
    }

    private async Task HandleAskCustomQuestionCallbackAsync(BotCallbackQuery callbackQuery,
        CancellationToken cancellationToken = default)
    {
        var state = await databaseManager.GetStateAsync(
            userId: callbackQuery.From.Id,
            messengerType: callbackQuery.MessengerType,
            cancellationToken: cancellationToken);

        if (state.IsBlocked)
            return;

        state.Type = StateType.CustomQuestion;

        await client.SendMessageAsync(
            chatId: callbackQuery.ChatId,
            menu: BotMenus.AskCustomQuestion,
            cancellationToken: cancellationToken);

        await databaseManager.UpsertStateAsync(
            state: state,
            cancellationToken: cancellationToken);
    }

    private async Task HandleBlockUserCallbackAsync(BotCallbackQuery callbackQuery, long targetUserId,
        BotMessengerType targetMessengerType, CancellationToken cancellationToken = default)
    {
        var targetState = await databaseManager.GetStateAsync(
            userId: targetUserId,
            messengerType: targetMessengerType,
            cancellationToken: cancellationToken);

        if (targetState.IsBlocked)
        {
            await client.SendReplyAsync(
                chatId: callbackQuery.ChatId,
                messageId: callbackQuery.MessageId,
                menu: BotMenus.UserAlreadyBlocked(callbackQuery.From),
                cancellationToken: cancellationToken);
            return;
        }

        targetState.BlockUser();

        await client.SendReplyAsync(
            chatId: callbackQuery.ChatId,
            messageId: callbackQuery.MessageId,
            menu: BotMenus.UserBlockedSuccessfully(callbackQuery.From),
            cancellationToken: cancellationToken);

        await databaseManager.UpsertStateAsync(
            state: targetState,
            cancellationToken: cancellationToken);
    }
}
