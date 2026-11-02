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

        var state = await databaseManager.GetStateAsync(callbackQuery.From.Id, cancellationToken);

        switch (callbackQuery.Data)
        {
            case CallbackStrings.Start:
            {
                state.Clear();

                await client.EditMessageTextAsync(
                    chatId: callbackQuery.ChatId,
                    messageId: callbackQuery.MessageId,
                    menu: BotMenus.Start(_botUser!.FullName),
                    cancellationToken: cancellationToken);
                break;
            }
            case CallbackStrings.AskCustomQuestion:
            {
                state.Type = StateType.CustomQuestion;

                await client.EditMessageTextAsync(
                    chatId: callbackQuery.ChatId,
                    messageId: callbackQuery.MessageId,
                    menu: BotMenus.AskCustomQuestion,
                    cancellationToken: cancellationToken);
                break;
            }
            default:
            {
                await client.AnswerCallbackQueryAsync(
                    callbackQueryId: callbackQuery.Id,
                    cancellationToken: cancellationToken);
                return;
            }
        }

        await databaseManager.UpsertStateAsync(state, cancellationToken);
    }
}
