using System;

namespace CybersecurityBot
{
    static class UserInteraction
    {
        public static string GetUserName()
        {
            Display.ShowDivider();
            Display.TypeWrite("  Hello! Before we begin, what is your name?", ConsoleColor.Green);
            Display.Write("  > ", ConsoleColor.Yellow);

            string name = Console.ReadLine()?.Trim() ?? string.Empty;

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