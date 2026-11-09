using JetBrains.Annotations;

namespace ElkollegeAdmissionsBot.Models.FAQ;

[UsedImplicitly]
public record FrequentlyAskedQuestions
{
    // ReSharper disable once CollectionNeverUpdated.Global
    public required Dictionary<string, FrequentlyAskedTopic> Topics { get; init; }

    // ReSharper disable once CollectionNeverUpdated.Global
    public required Dictionary<string, FrequentlyAskedSection> Sections { get; init; }
}
