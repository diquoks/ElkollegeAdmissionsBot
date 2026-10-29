using ElkollegeAdmissionsBot.Models.Bot;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using BotCommand = ElkollegeAdmissionsBot.Models.Bot.BotCommand;

namespace ElkollegeAdmissionsBot.Services.Telegram;

public partial class TelegramBotClient
{
    public async Task<BotUser> GetMeAsync(CancellationToken cancellationToken = default) =>
        BotUser.FromTelegram(await Implementation.GetMeAsync(cancellationToken));

    public Task SetMyCommandsAsync(BotCommand[] commands, CancellationToken cancellationToken = default) =>
        Implementation.SetMyCommandsAsync(
            commands: commands.Select(command => command.ToTelegram()),
            scope: new BotCommandScopeAllPrivateChats(),
            cancellationToken: cancellationToken);

    public Task SendMessageToUserAsync(long userId, BotMenu menu, CancellationToken cancellationToken = default) =>
        SendMessageToChatAsync(userId, menu, cancellationToken);

    public Task SendMessageToChatAsync(long chatId, BotMenu menu, CancellationToken cancellationToken = default) =>
        Implementation.SendMessageAsync(
            chatId: chatId,
            text: menu.Text,
            cancellationToken: cancellationToken); // TODO: add keyboard support
}
