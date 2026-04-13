using System;

namespace CybersecurityBot
{
    class ChatBot
    {
        private readonly string _userName;

        public ChatBot(string userName)
        {
            _userName = userName;
        }

        public void Run()
        {
            while (true)
            {
                Display.PrintUserPrompt(_userName);
                string input = Console.ReadLine() ?? string.Empty;

                string trimmed = input.Trim().ToLowerInvariant();
                if (trimmed is "quit" or "exit" or "bye" or "goodbye")
                {
                    Console.WriteLine();
                    Display.ShowDivider();
                    Display.TypeWrite($"  Stay safe online, {_userName}. Goodbye!", ConsoleColor.Green);
                    Display.ShowDivider();
                    Console.WriteLine();
                    break;
                }

                string? response = ResponseEngine.GetResponse(input, out bool isHelp);

                Console.WriteLine();

                if (isHelp)
                {
                    Display.ShowHelp();
                }
                else if (!string.IsNullOrEmpty(response))
                {
                    Display.PrintBotLabel();
                    Display.TypeWrite(response, ConsoleColor.White);
                    Display.ShowThinDivider();
                }
            }
        }
    }
}