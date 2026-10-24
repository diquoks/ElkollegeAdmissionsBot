using Max.Bot.Polling;

namespace ElkollegeAdmissionsBot.Services.Max;

public class MaxBot(MaxHandler handler) : UpdateHandlerBase, IBot<MaxHandler>
{
    public MaxHandler Handler { get; } = handler;
}
