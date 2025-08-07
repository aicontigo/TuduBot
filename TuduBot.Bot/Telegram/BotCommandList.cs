using Telegram.Bot.Types;

namespace TuduBot.Bot.Telegram;

public static class BotCommandList
{
    public static IReadOnlyList<BotCommand> Commands => new List<BotCommand>
    {
        new() { Command = "start", Description = "Start working with the bot" },
        new() { Command = "menu", Description = "Open the main menu" },
        //new() { Command = "setkey", Description = "Set Todoist API key" },
        //new() { Command = "deletekey", Description = "Delete API key" },
        //new() { Command = "projects", Description = "List of projects" },
        //new() { Command = "setproject", Description = "Set default project" },
        //new() { Command = "add", Description = "Add a task" }
    };
}

