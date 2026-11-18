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
            menu: BotMenus.ForwardToAdminGroup(
                message: originalMessage),
            cancellationToken: cancellationToken);

        await databaseManager.InsertMessageAsync(
            message: DatabaseMessage.FromForwardedMessage(originalMessage, forwardedMessage),
            cancellationToken: cancellationToken);
    }

    public async Task SendAnswerFromAdminGroupAsync(DatabaseMessage databaseMessage, BotMenu menu,
        CancellationToken cancellationToken = default)
    {
        IBotClient client = databaseMessage.OriginalMessengerType switch
        {
            BotMessengerType.Max => maxClient,
            BotMessengerType.Telegram => telegramClient,
            _ => throw new ArgumentOutOfRangeException()
        };

        await client.SendReplyAsync(
            chatId: databaseMessage.OriginalChatId,
            messageId: databaseMessage.OriginalId,
            menu: menu,
            cancellationToken: cancellationToken);
    }

    public bool IsInAdminGroup(BotUpdate update) =>
        update.MessengerType is BotMessengerType.Max && update.ChatId == maxClient.AdminGroupId;
}
