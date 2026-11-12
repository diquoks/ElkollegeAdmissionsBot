using ElkollegeAdmissionsBot.Extensions;
using ElkollegeAdmissionsBot.Helpers;
using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Services;

public partial class BotHandler
{
    public async Task OnCommandAsync(BotMessage message, string commandName,
        CancellationToken cancellationToken = default)
    {
        logger.LogBotMessage(message);

        switch (commandName)
        {
            case BotCommands.StartCommand:
            {
                await client.SendMessageAsync(
                    chatId: message.ChatId,
                    menu: BotMenus.Start(
                        botName: _botUser.FullName,
                        addCustomQuestionButton: !interconnectionService.IsInAdminGroup(message),
                        faqTopics: faqProvider.Faq.Topics),
                    cancellationToken: cancellationToken);
                break;
            }
        }
    }
}
