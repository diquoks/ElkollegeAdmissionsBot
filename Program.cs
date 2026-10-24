using ElkollegeAdmissionsBot.Services.Max;
using ElkollegeAdmissionsBot.Services.Telegram;

namespace ElkollegeAdmissionsBot;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services
            .Configure<MaxHandlerOptions>(builder.Configuration.GetSection(MaxHandler.Section))
            .Configure<TelegramHandlerOptions>(builder.Configuration.GetSection(TelegramHandler.Section));
        builder.Services
            .AddSingleton<MaxHandler>()
            .AddSingleton<MaxBot>()
            .AddSingleton<TelegramHandler>()
            .AddSingleton<TelegramBot>()
            .AddHostedService<MaxWorker>()
            .AddHostedService<TelegramWorker>();

        using var host = builder.Build();
        await host.RunAsync();
    }
}
