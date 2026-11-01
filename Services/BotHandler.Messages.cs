using ElkollegeAdmissionsBot.Extensions;
using ElkollegeAdmissionsBot.Helpers;
using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Services;

public partial class BotHandler
{
    public async Task OnMessageAsync(BotMessage message, CancellationToken cancellationToken = default)
    {
        logger.LogBotMessage(message);

        var state = await databaseManager.GetStateAsync(message.From.Id, cancellationToken);

        if (message.LinkedMessageId is not null)
        {
            var databaseMessage = await databaseManager.GetMessageAsync(message.LinkedMessageId, cancellationToken);

            if (databaseMessage is null)
                return;

            await interconnectionService.SendAnswerFromAdminGroupAsync(
                databaseMessage: databaseMessage,
                menu: BotMenus.AnswerFromAdminGroup(message.Text),
                cancellationToken: cancellationToken);
        }
        else
        {
            await interconnectionService.ForwardToAdminGroupAsync(
                originalMessage: message,
                cancellationToken: cancellationToken);
            return;
        }

        // TODO
        //  switch (state.Type)
        //  {
        //      default:
        //          return;
        //  }

        await databaseManager.UpsertStateAsync(state, cancellationToken);
    }
}
