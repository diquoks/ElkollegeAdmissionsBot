using ElkollegeAdmissionsBot.Extensions;
using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Services;

public partial class BotHandler
{
    public async Task OnMessageAsync(BotMessage message, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(message.Text))
            return;

        logger.LogBotMessage(message);

        throw new NotImplementedException();
    }
}
