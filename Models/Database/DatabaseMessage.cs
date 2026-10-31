namespace ElkollegeAdmissionsBot.Models.Database;

public record DatabaseMessage(string Id, DatabaseMessageType Type, string OriginalId, long OriginalChatId);

public enum DatabaseMessageType
{
    Max = 1,
    Telegram = 2
}
