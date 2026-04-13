// Program.cs — entry point
using CybersecurityBot;

AudioPlayer.PlayGreeting();
Display.ShowLogo();
Display.ShowDivider();

string userName = UserInteraction.GetUserName();
Display.ShowWelcomeBanner(userName);

ChatBot bot = new ChatBot(userName);
bot.Run();