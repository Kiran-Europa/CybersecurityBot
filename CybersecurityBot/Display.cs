using System;
using System.Threading;

namespace CybersecurityBot
{
    static class Display
    {
        // writes text in a chosen colour without moving to a new line
        public static void Write(string text, ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
            Console.Write(text);
            Console.ResetColor();
        }

        // writes text in a chosen colour and moves to the next line
        public static void WriteLine(string text, ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        // prints text one character at a time to look like typing
        public static void TypeWrite(string text, ConsoleColor colour = ConsoleColor.White, int delayMs = 18)
        {
            Console.ForegroundColor = colour;
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delayMs); // small delay between each character
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        // prints a thick divider line across the console
        public static void ShowDivider()
        {
            WriteLine(new string('═', 60), ConsoleColor.DarkCyan);
        }

        // prints a thin divider line across the console
        public static void ShowThinDivider()
        {
            WriteLine(new string('─', 60), ConsoleColor.DarkGray);
        }

        // clears the screen and prints the ASCII art logo
        public static void ShowLogo()
        {
            Console.Clear();
            Console.WriteLine();
            string[] logo = new[]
            {
                @"  ==============================================",
                @"  ||   CYBERSECURITY  AWARENESS  BOT          ||",
                @"  ||              /                           ||",
                @"  ||             /                            ||",
                @"  ||            / ##                          ||",
                @"  ||           /  ##                          ||",
                @"  ||          /########                       ||",
                @"  ||          ________/                       ||",
                @"  ||         STAY SAFE ONLINE                  ||",
                @"  ==============================================",
            };
            foreach (string line in logo)
                WriteLine(line, ConsoleColor.Cyan);
            Console.WriteLine();
        }

        // shows a personalised welcome message after the user enters their name
        public static void ShowWelcomeBanner(string userName)
        {
            ShowDivider();
            TypeWrite($"  Welcome, {userName}! I am the Cybersecurity Awareness Bot.", ConsoleColor.Green);
            TypeWrite("  I'm here to help you stay safe online.", ConsoleColor.Green);
            ShowDivider();
            Console.WriteLine();
            WriteLine("  Type your question, or:", ConsoleColor.Gray);
            WriteLine("    'help'  - list topics", ConsoleColor.DarkGray);
            WriteLine("    'quit'  - exit", ConsoleColor.DarkGray);
            Console.WriteLine();
        }

        // prints the list of topics the bot can answer questions about
        public static void ShowHelp()
        {
            ShowDivider();
            WriteLine("  TOPICS I CAN HELP WITH:", ConsoleColor.Cyan);
            ShowThinDivider();
            WriteLine("  password   - Password safety tips", ConsoleColor.White);
            WriteLine("  phishing   - How to spot phishing attacks", ConsoleColor.White);
            WriteLine("  browsing   - Safe browsing practices", ConsoleColor.White);
            WriteLine("  purpose    - What this bot does", ConsoleColor.White);
            ShowDivider();
            Console.WriteLine();
        }

        // prints the [BOT] label before a response
        public static void PrintBotLabel()
        {
            Write("  [BOT] ", ConsoleColor.Cyan);
        }

        // prints the users name as the input prompt
        public static void PrintUserPrompt(string userName)
        {
            Console.WriteLine();
            Write($"  [{userName}] > ", ConsoleColor.Yellow);
        }
    }
}