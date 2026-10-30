using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Helpers;

public static class BotButtons
{
    public static IBotButton TestCallback =>
        new BotCallbackButton(nameof(BotCallbackButton), CallbackStrings.Test);

    public static IBotButton TestLink =>
        new BotLinkButton(nameof(BotLinkButton), "https://diquoks.github.io");
}
