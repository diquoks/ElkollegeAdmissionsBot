using ElkollegeAdmissionsBot.Models.Bot;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.UpdatingMessages;
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

    public Task SendReplyAsync(long chatId, string messageId, BotMenu menu,
        CancellationToken cancellationToken = default) =>
        InternalSendMessageAsync(
            chatId: chatId,
            menu: menu,
            messageId: int.Parse(messageId),
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

    public Task EditMessageTextAsync(long chatId, string messageId, BotMenu menu,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return Implementation.EditMessageTextAsync(
                chatId: chatId,
                messageId: int.Parse(messageId),
                text: menu.Text,
                parseMode: FormatStyles.HTML,
                replyMarkup: menu.Keyboard?.ToTelegram(),
                cancellationToken: cancellationToken);
        }
        catch
        {
            return SendMessageAsync(
                chatId: chatId,
                menu: menu,
                cancellationToken: cancellationToken);
        }
    }

    public Task AnswerCallbackQueryAsync(string callbackQueryId, CancellationToken cancellationToken = default) =>
        Implementation.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQueryId,
            cancellationToken: cancellationToken);
}
