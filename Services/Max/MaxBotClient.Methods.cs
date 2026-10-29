using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Services.Max;

public partial class MaxBotClient
{
    public async Task<BotUser> GetMeAsync(CancellationToken cancellationToken = default) =>
        BotUser.FromMax(await Implementation.Bot.GetMeAsync(cancellationToken));

    public Task SetMyCommandsAsync(BotCommand[] commands, CancellationToken cancellationToken = default) =>
        Implementation.Bot.SetCommandsAsync(
            commands: commands.Select(command => command.ToMax()),
            cancellationToken: cancellationToken);

    public Task SendMessageToUserAsync(long userId, BotMenu menu, CancellationToken cancellationToken = default) =>
        Implementation.Messages.SendMessageToUserAsync(
            userId: userId,
            text: menu.Text,
            cancellationToken: cancellationToken); // TODO: add keyboard support

    public Task SendMessageToChatAsync(long chatId, BotMenu menu, CancellationToken cancellationToken = default) =>
        Implementation.Messages.SendMessageAsync(
            chatId: chatId,
            text: menu.Text,
            cancellationToken: cancellationToken); // TODO: add keyboard support
}
