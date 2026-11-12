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

        if (callbackQuery.Data is CallbackStrings.JustAnswer)
        {
            await client.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                cancellationToken: cancellationToken);
            return;
        }

        var state = await databaseManager.GetStateAsync(
            userId: callbackQuery.From.Id,
            messengerType: callbackQuery.MessengerType,
            cancellationToken: cancellationToken);

        switch (callbackQuery.Data.Split())
        {
            case [CallbackStrings.Start]:
            {
                state.Clear();

                await client.EditMessageTextAsync(
                    chatId: callbackQuery.ChatId,
                    messageId: callbackQuery.MessageId,
                    menu: BotMenus.Start(
                        botName: _botUser.FullName,
                        addCustomQuestionButton: !interconnectionService.IsInAdminGroup(callbackQuery),
                        faqTopics: faqProvider.Faq.Topics),
                    cancellationToken: cancellationToken);
                break;
            }
            case [CallbackStrings.Topic, var key]:
            {
                var faqTopic = faqProvider.Faq.Topics.GetValueOrDefault(key);

                if (faqTopic is null)
                    break;

                await client.EditMessageTextAsync(
                    chatId: callbackQuery.ChatId,
                    messageId: callbackQuery.MessageId,
                    menu: BotMenus.Topic(
                        faqTopic: faqTopic,
                        faqTopicKey: key,
                        faqSections: faqProvider.Faq.Sections
                            .Where(pair => faqTopic.Sections.Contains(pair.Key))
                            .ToDictionary()),
                    cancellationToken: cancellationToken);
                break;
            }
            case [CallbackStrings.Section, var topicKey, var key]:
            {
                var faqSection = faqProvider.Faq.Sections.GetValueOrDefault(key);

                if (faqSection is null)
                    break;

                await client.EditMessageTextAsync(
                    chatId: callbackQuery.ChatId,
                    messageId: callbackQuery.MessageId,
                    menu: BotMenus.Section(
                        faqSection: faqSection,
                        faqTopicKey: topicKey),
                    cancellationToken: cancellationToken);
                break;
            }
            case [CallbackStrings.AskCustomQuestion]:
            {
                state.Type = StateType.CustomQuestion;

                await client.SendMessageAsync(
                    chatId: callbackQuery.ChatId,
                    menu: BotMenus.AskCustomQuestion,
                    cancellationToken: cancellationToken);
                break;
            }
        }

        await client.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            cancellationToken: cancellationToken);

        await databaseManager.UpsertStateAsync(
            state: state,
            cancellationToken: cancellationToken);
    }
}
