using System.Diagnostics;
using ElkollegeAdmissionsBot.Helpers;
using ElkollegeAdmissionsBot.Models.Bot;
using ElkollegeAdmissionsBot.Models.Database;
using ElkollegeAdmissionsBot.Services.Max;
using ElkollegeAdmissionsBot.Services.Telegram;

namespace ElkollegeAdmissionsBot.Services;

public class InterconnectionService(
    DatabaseManager databaseManager,
    MaxBotClient maxClient,
    TelegramBotClient telegramClient)
{
    public async Task ForwardToAdminGroupAsync(BotMessage originalMessage,
        CancellationToken cancellationToken = default)
    {
        var forwardedMessage = await maxClient.SendMessageAsync(
            chatId: maxClient.AdminGroupId,
            menu: BotMenus.ForwardToAdminGroup(originalMessage),
            cancellationToken: cancellationToken);

        await databaseManager.InsertMessageAsync(
            message: DatabaseMessage.FromForwardedMessage(originalMessage, forwardedMessage),
            cancellationToken: cancellationToken);
    }

    public async Task SendAnswerFromAdminGroupAsync(DatabaseMessage databaseMessage, BotMenu menu,
        CancellationToken cancellationToken = default)
    {
        IBotClient client = databaseMessage.MessengerType switch
        {
            BotMessengerType.Max => maxClient,
            BotMessengerType.Telegram => telegramClient,
            _ => throw new UnreachableException()
        };

        await client.SendReplyAsync(
            chatId: databaseMessage.OriginalChatId,
            messageId: databaseMessage.OriginalId,
            menu: menu,
            cancellationToken: cancellationToken);
    }

    public bool IsInAdminGroup(BotMessage message) =>
        IsInAdminGroup(message.MessengerType, message.ChatId);

    public bool IsInAdminGroup(BotCallbackQuery callbackQuery) =>
        IsInAdminGroup(callbackQuery.MessengerType, callbackQuery.ChatId);

    public bool IsInAdminGroup(BotMessengerType messengerType, long chatId) =>
        messengerType is BotMessengerType.Max && chatId == maxClient.AdminGroupId;
}
