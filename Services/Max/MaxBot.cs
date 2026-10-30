using System.Text.Json;
using System.Text.Json.Serialization;
using ElkollegeAdmissionsBot.Models.Bot;
using Max.Bot.Polling;
using Max.Bot.Types.Enums;
using Telegram.BotAPI.Extensions.Commands;

namespace ElkollegeAdmissionsBot.Services.Max;

public class MaxBot(ILogger<MaxBot> logger, MaxBotClient client) : UpdateHandlerBase, IBot
{
    private static readonly JsonSerializerOptions JsonEnumOptions = new()
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private BotUser? _botUser;

    public BotHandler Handler { get; } = new(logger, client);

    public async Task PrepareAsync(CancellationToken cancellationToken = default) =>
        _botUser = await Handler.PrepareAsync(cancellationToken);

    public override Task HandleMessageAsync(UpdateContext context, CancellationToken cancellationToken)
    {
        var message = context.Update.Message;

        if (message?.Text is null)
            return Task.CompletedTask;

        var chatType = JsonSerializer
            .SerializeToElement(message.Recipient!.ChatType!)
            .Deserialize<ChatType>(JsonEnumOptions);

        if (BotCommandParser.TryParse(message.Text, out var result))
        {
            if (chatType is ChatType.Dialog)
                return Handler.OnCommandAsync(BotMessage.FromMax(message), result.commandName, cancellationToken);

            if (string.IsNullOrWhiteSpace(result.username) || result.username != _botUser?.Username)
                return Task.CompletedTask;

            return Handler.OnCommandAsync(BotMessage.FromMax(message), result.commandName, cancellationToken);
        }

        var messageLinkType = JsonSerializer
            .SerializeToElement(message.Link?.Type)
            .Deserialize<MessageLinkType?>(JsonEnumOptions);

        if (chatType is ChatType.Dialog)
            return Handler.OnMessageAsync(BotMessage.FromMax(message), cancellationToken);

        if (messageLinkType is not MessageLinkType.Reply ||
            message.Link!.Sender?.Id is not { } senderId ||
            senderId != _botUser?.Id)
            return Task.CompletedTask;

        return Handler.OnMessageAsync(BotMessage.FromMax(message), cancellationToken);
    }
}
