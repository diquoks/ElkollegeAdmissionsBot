using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Services;

public interface IBotClient
{
    public Task<BotUser> GetMeAsync(CancellationToken cancellationToken = default);

    public Task SetMyCommandsAsync(BotCommand[] commands, CancellationToken cancellationToken = default);

    public Task SendMessageToUserAsync(long userId, BotMenu menu, CancellationToken cancellationToken = default);

    public Task SendMessageToChatAsync(long chatId, BotMenu menu, CancellationToken cancellationToken = default);
}
