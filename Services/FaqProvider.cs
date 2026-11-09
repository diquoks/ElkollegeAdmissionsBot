using ElkollegeAdmissionsBot.Models.FAQ;
using Microsoft.Extensions.Options;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ElkollegeAdmissionsBot.Services;

public class FaqProvider
{
    public FrequentlyAskedQuestions Faq { get; }

    public FaqProvider(IOptions<AssetsOptions> options)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

        Faq = deserializer.Deserialize<FrequentlyAskedQuestions>(
            File.ReadAllText(options.Value.FrequentlyAskedQuestionsPath));
    }
}
