using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Services;

public interface IBotClient
{
    public Task<BotUser> GetMeAsync(CancellationToken cancellationToken = default);

    public Task SetMyCommandsAsync(BotCommand[] commands, CancellationToken cancellationToken = default);

    public Task<BotMessage> SendMessageAsync(long chatId, BotMenu menu, CancellationToken cancellationToken = default);

    public Task SendReplyAsync(string messageId, long chatId, BotMenu menu,
        CancellationToken cancellationToken = default);

    public Task AnswerCallbackQueryAsync(string callbackQueryId, CancellationToken cancellationToken = default);
}
