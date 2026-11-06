using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Models.Database;

public record DatabaseState
{
    public required long UserId { get; init; }

    public required BotMessengerType MessengerType { get; init; }

    public StateType Type { get; set; } = StateType.Empty;

    public void Clear() =>
        Type = StateType.Empty;
}

public enum StateType
{
    Empty,
    CustomQuestion
}
