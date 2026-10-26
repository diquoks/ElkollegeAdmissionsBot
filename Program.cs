using ElkollegeAdmissionsBot.Services.Max;
using ElkollegeAdmissionsBot.Services.Telegram;

namespace ElkollegeAdmissionsBot;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services
            .Configure<MaxBotOptions>(builder.Configuration.GetSection(MaxBotOptions.Section))
            .Configure<TelegramBotOptions>(builder.Configuration.GetSection(TelegramBotOptions.Section));
        builder.Services
            .AddSingleton<MaxBotClient>()
            .AddSingleton<MaxBotHandler>()
            .AddSingleton<MaxBot>()
            .AddSingleton<TelegramBotClient>()
            .AddSingleton<TelegramBotHandler>()
            .AddSingleton<TelegramBot>();
        builder.Services
            .AddHostedService<MaxBotWorker>()
            .AddHostedService<TelegramBotWorker>();

        using var host = builder.Build();
        await host.RunAsync();
    }
}
