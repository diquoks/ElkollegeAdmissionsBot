namespace ElkollegeAdmissionsBot.Models.Database;

public record DatabaseState
{
    public required long UserId { get; init; }

    public StateType Type { get; set; } = StateType.Empty;
}

public enum StateType
{
    Empty
}
