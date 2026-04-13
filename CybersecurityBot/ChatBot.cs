using System;

namespace CybersecurityBot
{
    class ChatBot
    {
        // stores the users name so we can use it throughout the conversation
        private readonly string _userName;

        // constructor that receives the users name when the chatbot is created
        public ChatBot(string userName)
        {
            _userName = userName;
        }

        public void Run()
        {
            // keeps the chatbot running until the user types quit/exit/bye
            while (true)
            {
                // show the input prompt with the users name
                Display.PrintUserPrompt(_userName);
                string input = Console.ReadLine() ?? string.Empty;

                // check if the user wants to exit
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

                // send input to the response engine to get a reply
                string? response = ResponseEngine.GetResponse(input, out bool isHelp);

                Console.WriteLine();

                // if the user asked for help, show the help menu
                if (isHelp)
                {
                    Display.ShowHelp();
                }
                // otherwise print the bots response
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