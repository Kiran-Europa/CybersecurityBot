using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace CybersecurityBotGUI
{
    public partial class MainWindow : Window
    {
        private string _userName = "";          // User's name (remembered)
        private bool _awaitingName = true;      // First message asks for name
        private string _rememberedTopic = "";   // Favourite topic (memory)
        private string _lastTopic = "";         // Last discussed topic (for follow‑ups)

        public MainWindow()
        {
            InitializeComponent();
        }

        // Called when window loads – plays greeting sound and asks for name
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AudioPlayer.PlayGreeting();
            AppendMessage("BOT", "Hello! Before we begin, what is your name?", "#00ffcc");
        }

        private void SendButton_Click(object sender, RoutedEventArgs e) => ProcessInput();
        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) ProcessInput();
        }

        // Main conversation handler
        private void ProcessInput()
        {
            string input = InputBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;

            InputBox.Clear();

            // First input is always the name
            if (_awaitingName)
            {
                _userName = input;
                _awaitingName = false;
                AppendMessage(_userName, input, "#ffff00");
                AppendMessage("BOT", $"Welcome, {_userName}! I'm the Cybersecurity Awareness Bot. Type 'help' to see what I can do.", "#00ffcc");
                return;
            }

            // Show user message
            AppendMessage(_userName, input, "#ffff00");

            // Detect sentiment and update status bar
            string sentiment = SentimentDetector.Detect(input);
            UpdateStatus(sentiment);

            // Store favourite topic if mentioned
            MemoryEngine.CheckAndStore(input, ref _rememberedTopic);

            // Get bot response (handles keywords, follow‑ups, random tips)
            string response = ResponseEngine.GetResponse(input, _rememberedTopic, _lastTopic, out string newTopic);
            _lastTopic = newTopic;

            // Add empathy line if user is worried or frustrated
            if (sentiment == "worried" || sentiment == "frustrated")
            {
                AppendMessage("BOT", SentimentDetector.GetEmpathyLine(sentiment), "#ff9944");
            }

            // Show main response
            AppendMessage("BOT", response, "#00ffcc");

            // Auto‑scroll to bottom
            ChatScroller.ScrollToBottom();
        }

        // Add a formatted message to chat (sender label in colour, message in white)
        private void AppendMessage(string sender, string message, string hexColour)
        {
            var paragraph = new Paragraph { Margin = new Thickness(0, 4, 0, 4) };
            var senderRun = new Run($"[{sender}] ")
            {
                Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(hexColour),
                FontWeight = FontWeights.Bold
            };
            var messageRun = new Run(message) { Foreground = Brushes.White };
            paragraph.Inlines.Add(senderRun);
            paragraph.Inlines.Add(messageRun);
            ChatBox.Document.Blocks.Add(paragraph);
        }

        // Update the top status label based on detected sentiment
        private void UpdateStatus(string sentiment)
        {
            StatusLabel.Text = sentiment switch
            {
                "worried" => "Mood detected: Worried — I'll be extra supportive!",
                "curious" => "Mood detected: Curious — great, let's explore!",
                "frustrated" => "Mood detected: Frustrated — I'll keep it simple.",
                _ => "Ready"
            };

            StatusLabel.Foreground = sentiment switch
            {
                "worried" => new SolidColorBrush(Color.FromRgb(255, 153, 68)),
                "curious" => new SolidColorBrush(Color.FromRgb(0, 255, 204)),
                "frustrated" => new SolidColorBrush(Color.FromRgb(255, 80, 80)),
                _ => new SolidColorBrush(Color.FromRgb(136, 136, 136))
            };
        }
    }
}