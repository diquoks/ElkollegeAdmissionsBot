using System.Diagnostics;
using ElkollegeAdmissionsBot.Helpers;
using ElkollegeAdmissionsBot.Models;
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
            DatabaseMessage.FromForwardedMessage(originalMessage, forwardedMessage),
            cancellationToken);
    }

    public Task SendAnswerFromAdminGroupAsync(DatabaseMessage databaseMessage, BotMenu menu,
        CancellationToken cancellationToken = default)
    {
        IBotClient client = databaseMessage.MessengerType switch
        {
            MessengerType.Max => maxClient,
            MessengerType.Telegram => telegramClient,
            _ => throw new UnreachableException()
        };

        return client.SendReplyAsync(
            chatId: databaseMessage.OriginalChatId,
            messageId: databaseMessage.OriginalId,
            menu: menu,
            cancellationToken: cancellationToken);
    }

    public bool IsInAdminGroup(BotMessage message) =>
        message.MessengerType is MessengerType.Max && message.ChatId == maxClient.AdminGroupId;
}
