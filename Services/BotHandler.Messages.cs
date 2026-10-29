using ElkollegeAdmissionsBot.Extensions;
using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Services;

public partial class BotHandler
{
    public async Task OnMessageAsync(BotMessage message, CancellationToken cancellationToken = default)
    {
        if (message.From is null || string.IsNullOrWhiteSpace(message.Text))
            return;

        logger.LogBotInteraction(
            user: message.From,
            interaction: $"\"{message.Text}\"");

        throw new NotImplementedException();
    }
}
