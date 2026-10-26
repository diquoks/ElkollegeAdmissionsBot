using Max.Bot.Polling;

namespace ElkollegeAdmissionsBot.Services.Max;

public class MaxBot(MaxBotHandler handler) : UpdateHandlerBase, IBot<MaxBotHandler>
{
    public MaxBotHandler Handler { get; } = handler;
}
