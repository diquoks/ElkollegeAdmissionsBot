using JetBrains.Annotations;

namespace ElkollegeAdmissionsBot.Models.FAQ;

[UsedImplicitly]
public record FrequentlyAskedSection
{
    public required string Title { get; init; }

    public required string Text { get; init; }

    public required FrequentlyAskedSectionLink[]? Links { get; init; }
}

[UsedImplicitly]
public record FrequentlyAskedSectionLink
{
    public required string Title { get; init; }

    public required string Url { get; init; }
}
