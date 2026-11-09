using ElkollegeAdmissionsBot.Extensions;
using ElkollegeAdmissionsBot.Helpers;
using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Services;

public partial class BotHandler
{
    public Task OnCommandAsync(BotMessage message, string commandName,
        CancellationToken cancellationToken = default)
    {
        logger.LogBotMessage(message);

        switch (commandName)
        {
            case BotCommands.StartCommand:
                return client.SendMessageAsync(
                    chatId: message.ChatId,
                    menu: BotMenus.Start(
                        botName: _botUser.FullName,
                        faqTopics: faqProvider.Faq.Topics,
                        addCustomQuestionButton: !interconnectionService.IsInAdminGroup(message)),
                    cancellationToken: cancellationToken);
            default:
                return Task.CompletedTask;
        }
    }
}
