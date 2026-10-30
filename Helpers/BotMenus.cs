using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Helpers;

public static class BotMenus
{
    public static BotMenu Test(string botName) => new(
        $"""
         <b>Привет, я {botName}!</b>

         Здесь ты сможешь найти ответы на часто задаваемые вопросы, а также отправить свой вопрос приёмной комиссии.
         """,
        new BotKeyboardBuilder()
            .Append(BotButtons.TestCallback)
            .AppendRow()
            .Append(BotButtons.TestLink)
            .Build());
}
