using ElkollegeAdmissionsBot.Models.Bot;
using Telegram.BotAPI;
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

    public Task SendMessageAsync(long chatId, BotMenu menu, CancellationToken cancellationToken = default) =>
        Implementation.SendMessageAsync(
            chatId: chatId,
            text: menu.Text,
            parseMode: FormatStyles.HTML,
            replyMarkup: menu.Keyboard.ToTelegram(),
            cancellationToken: cancellationToken);
}
