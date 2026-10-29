using ElkollegeAdmissionsBot.Models.Bot;
using Max.Bot.Types.Enums;

namespace ElkollegeAdmissionsBot.Services.Max;

public partial class MaxBotClient
{
    public async Task<BotUser> GetMeAsync(CancellationToken cancellationToken = default) =>
        BotUser.FromMax(await Implementation.Bot.GetMeAsync(cancellationToken));

    public Task SetMyCommandsAsync(BotCommand[] commands, CancellationToken cancellationToken = default) =>
        Implementation.Bot.SetCommandsAsync(
            commands: commands.Select(command => command.ToMax()),
            cancellationToken: cancellationToken);

    public Task SendMessageAsync(long chatId, BotMenu menu, CancellationToken cancellationToken = default) =>
        Implementation.Messages.SendMessageAsync(
            chatId: chatId,
            text: menu.Text,
            keyboard: menu.Keyboard.ToMax(),
            format: TextFormat.Html,
            cancellationToken: cancellationToken);
}
