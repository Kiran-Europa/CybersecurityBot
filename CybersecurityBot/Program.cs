// entry point - this runs first when the program starts
using CybersecurityBot;

// play the voice greeting wav file
AudioPlayer.PlayGreeting();

// show the ASCII art logo and a divider line
Display.ShowLogo();
Display.ShowDivider();

// ask the user for their name
string userName = UserInteraction.GetUserName();

// show the welcome message using their name
Display.ShowWelcomeBanner(userName);

// create the chatbot and start the conversation loop
ChatBot bot = new ChatBot(userName);
bot.Run();