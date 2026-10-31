using ElkollegeAdmissionsBot.Extensions;
using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Services;

public partial class BotHandler
{
    public async Task OnCallbackQueryAsync(BotCallbackQuery callbackQuery,
        CancellationToken cancellationToken = default)
    {
        logger.LogBotCallbackQuery(callbackQuery);

        // TODO
        //  switch (callbackQuery.Data)
        //  {
        //  }

        await client.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            cancellationToken: cancellationToken);
    }
}
