using ElkollegeAdmissionsBot.Services;
using ElkollegeAdmissionsBot.Services.Max;
using ElkollegeAdmissionsBot.Services.Telegram;

namespace ElkollegeAdmissionsBot;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services
            .Configure<MaxBotClientOptions>(builder.Configuration.GetSection(MaxBotClientOptions.Section))
            .Configure<TelegramBotClientOptions>(builder.Configuration.GetSection(TelegramBotClientOptions.Section));
        builder.Services
            .AddSingleton<DatabaseManager>()
            .AddSingleton<MaxBotClient>()
            .AddSingleton<TelegramBotClient>()
            .AddSingleton<MaxBot>()
            .AddSingleton<TelegramBot>();
        builder.Services
            .AddHostedService<MaxBotWorker>()
            .AddHostedService<TelegramBotWorker>();

        using var host = builder.Build();
        await host.RunAsync();
    }
}
