using System;

namespace CybersecurityBot
{
    static class UserInteraction
    {
        public static string GetUserName()
        {
            // show a divider then ask for the users name
            Display.ShowDivider();
            Display.TypeWrite("  Hello! Before we begin, what is your name?", ConsoleColor.Green);
            Display.Write("  > ", ConsoleColor.Yellow);

            string name = Console.ReadLine()?.Trim() ?? string.Empty;

            // keep asking until they actually type something
            while (string.IsNullOrWhiteSpace(name))
            {
                Display.WriteLine("  Please enter your name:", ConsoleColor.DarkYellow);
                Display.Write("  > ", ConsoleColor.Yellow);
                name = Console.ReadLine()?.Trim() ?? string.Empty;
            }

            return name;
        }
    }
}