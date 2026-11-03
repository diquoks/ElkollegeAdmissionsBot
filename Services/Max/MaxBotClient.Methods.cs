using ElkollegeAdmissionsBot.Models.Bot;
using Max.Bot.Types.Enums;
using Max.Bot.Types.Requests;

namespace ElkollegeAdmissionsBot.Services.Max;

public partial class MaxBotClient
{
    public async Task<BotUser> GetMeAsync(CancellationToken cancellationToken = default) =>
        BotUser.FromMax(await Implementation.Bot.GetMeAsync(cancellationToken));

    public Task SetMyCommandsAsync(BotCommand[] commands, CancellationToken cancellationToken = default) =>
        Implementation.Bot.SetCommandsAsync(
            commands: commands.Select(command => command.ToMax()),
            cancellationToken: cancellationToken);

    public Task<BotMessage> SendMessageAsync(long chatId, BotMenu menu,
        CancellationToken cancellationToken = default) =>
        InternalSendMessageAsync(
            chatId: chatId,
            menu: menu,
            cancellationToken: cancellationToken);

    public Task SendReplyAsync(long chatId, string messageId, BotMenu menu,
        CancellationToken cancellationToken = default) =>
        InternalSendMessageAsync(
            chatId: chatId,
            menu: menu,
            messageId: messageId,
            cancellationToken: cancellationToken);

    private async Task<BotMessage> InternalSendMessageAsync(long chatId, BotMenu menu, string? messageId = null,
        CancellationToken cancellationToken = default) =>
        BotMessage.FromMax(
            await Implementation.Messages.SendMessageAsync(
                request: new SendMessageRequest
                {
                    Text = menu.Text,
                    Attachments = menu.Keyboard is not null ? [AttachmentRequest.Keyboard(menu.Keyboard.ToMax())] : [],
                    Link = messageId is not null
                        ? new NewMessageLink { Type = MessageLinkType.Reply, Id = messageId }
                        : null,
                    Format = TextFormat.Html
                },
                chatId: chatId,
                cancellationToken: cancellationToken));

    public Task EditMessageTextAsync(long chatId, string messageId, BotMenu menu,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return Implementation.Messages.EditMessageAsync(
                messageId: messageId,
                request: new EditMessageRequest
                {
                    Text = menu.Text,
                    Attachments = menu.Keyboard is not null ? [AttachmentRequest.Keyboard(menu.Keyboard.ToMax())] : [],
                    Format = TextFormat.Html
                },
                cancellationToken: cancellationToken);
        }
        catch
        {
            return SendMessageAsync(
                chatId: chatId,
                menu: menu,
                cancellationToken: cancellationToken);
        }
    }

    public Task AnswerCallbackQueryAsync(string callbackQueryId, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;
}
