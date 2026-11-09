using JetBrains.Annotations;

namespace ElkollegeAdmissionsBot.Models.FAQ;

[UsedImplicitly]
public record FrequentlyAskedTopic
{
    public required string Title { get; init; }

    public required string[] Sections { get; init; }
}
