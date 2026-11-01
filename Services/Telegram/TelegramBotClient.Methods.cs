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

    public Task<BotMessage> SendMessageAsync(long chatId, BotMenu menu,
        CancellationToken cancellationToken = default) =>
        InternalSendMessageAsync(
            chatId: chatId,
            menu: menu,
            cancellationToken: cancellationToken);

    public Task SendReplyAsync(string messageId, long chatId, BotMenu menu,
        CancellationToken cancellationToken = default) =>
        InternalSendMessageAsync(
            chatId: chatId,
            menu: menu,
            messageId: int.TryParse(messageId, out var result) ? result : null,
            cancellationToken: cancellationToken);

    private async Task<BotMessage> InternalSendMessageAsync(long chatId, BotMenu menu, int? messageId = null,
        CancellationToken cancellationToken = default) =>
        BotMessage.FromTelegram(
            await Implementation.SendMessageAsync(
                chatId: chatId,
                text: menu.Text,
                parseMode: FormatStyles.HTML,
                replyParameters: messageId is not null ? new ReplyParameters { MessageId = messageId } : null,
                replyMarkup: menu.Keyboard?.ToTelegram(),
                cancellationToken: cancellationToken));

    public Task AnswerCallbackQueryAsync(string callbackQueryId, CancellationToken cancellationToken = default) =>
        Implementation.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQueryId,
            cancellationToken: cancellationToken);
}
