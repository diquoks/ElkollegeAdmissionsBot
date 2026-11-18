using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Models.Database;

public record DatabaseState
{
    public required long UserId { get; init; }

    public required BotMessengerType MessengerType { get; init; }

    public StateType Type { get; set; } = StateType.Empty;

    public bool IsBlocked { get; private set; }

    public void ResetType() => Type = StateType.Empty;

    public void BlockUser() => IsBlocked = true;
}

public enum StateType
{
    Empty,
    CustomQuestion
}
