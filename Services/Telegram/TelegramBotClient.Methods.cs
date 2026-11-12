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

    public async Task SetMyCommandsAsync(BotCommand[] commands, CancellationToken cancellationToken = default) =>
        await Implementation.SetMyCommandsAsync(
            commands: commands.Select(command => command.ToTelegram()),
            scope: new BotCommandScopeAllPrivateChats(),
            cancellationToken: cancellationToken);

    public async Task<BotMessage> SendMessageAsync(long chatId, BotMenu menu,
        CancellationToken cancellationToken = default) =>
        await InternalSendMessageAsync(
            chatId: chatId,
            menu: menu,
            cancellationToken: cancellationToken);

    public async Task SendReplyAsync(long chatId, string messageId, BotMenu menu,
        CancellationToken cancellationToken = default) =>
        await InternalSendMessageAsync(
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

    public async Task EditMessageTextAsync(long chatId, string messageId, BotMenu menu,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await Implementation.EditMessageTextAsync(
                chatId: chatId,
                messageId: int.Parse(messageId),
                text: menu.Text,
                parseMode: FormatStyles.HTML,
                replyMarkup: menu.Keyboard?.ToTelegram(),
                cancellationToken: cancellationToken);
        }
        catch
        {
            await SendMessageAsync(
                chatId: chatId,
                menu: menu,
                cancellationToken: cancellationToken);
        }
    }

    public async Task AnswerCallbackQueryAsync(string callbackQueryId, CancellationToken cancellationToken = default) =>
        await Implementation.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQueryId,
            cancellationToken: cancellationToken);
}
