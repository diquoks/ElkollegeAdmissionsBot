using ElkollegeAdmissionsBot.Extensions;
using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Services;

public partial class BotHandler
{
    public async Task OnMessageAsync(BotMessage message, CancellationToken cancellationToken = default)
    {
        logger.LogBotMessage(message);

        var state = await databaseManager.GetStateAsync(message.From.Id, cancellationToken);

        // TODO
        //  switch (state.Type)
        //  {
        //      default:
        //          return;
        //  }

        await databaseManager.UpsertStateAsync(state, cancellationToken);
    }
}
