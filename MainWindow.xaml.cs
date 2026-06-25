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
        private string _userName = "";          // user's name once entered
        private bool _awaitingName = true;      // first message is always asking for the name
        private string _rememberedTopic = "";   // topic the user said they're interested in
        private string _lastTopic = "";         // last topic discussed, used for follow-ups
        private QuizEngine _quiz = new QuizEngine();

        public MainWindow()
        {
            InitializeComponent();
        }

        // runs on startup - plays the greeting and asks for the user's name
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

        private void ProcessInput()
        {
            string input = InputBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;

            InputBox.Clear();

            // first input is always the name
            if (_awaitingName)
            {
                _userName = input;
                _awaitingName = false;
                AppendMessage(_userName, input, "#ffff00");
                AppendMessage("BOT", $"Welcome, {_userName}! I'm the Cybersecurity Awareness Bot. Type 'help' to see what I can do.", "#00ffcc");
                return;
            }

            // show the user's message
            AppendMessage(_userName, input, "#ffff00");

            // if a quiz is active, treat input as an answer instead of a normal message
            if (_quiz.IsActive)
            {
                if (_quiz.LooksLikeAnswer(input))
                {
                    string quizResponse = _quiz.SubmitAnswer(input, out bool finished);
                    AppendMessage("BOT", quizResponse, finished ? "#00ffcc" : "#88ccff");
                    ActivityLog.Add(finished
                        ? $"Quiz completed - scored {_quiz.Score}/{_quiz.QuestionsAnswered}."
                        : $"Quiz question {_quiz.QuestionsAnswered} answered.");
                    ChatScroller.ScrollToBottom();
                }
                else
                {
                    AppendMessage("BOT", "Please answer with a letter (e.g. 'A', 'B', 'C'...) to continue the quiz.", "#ff9944");
                }
                return;
            }

            // check if the user wants to start the quiz
            if (NlpHelper.MatchesIntent(input, "quiz"))
            {
                string firstQuestion = _quiz.StartQuiz();
                AppendMessage("BOT", "Let's test your cybersecurity knowledge! I'll ask you 12 questions.", "#00ffcc");
                AppendMessage("BOT", firstQuestion, "#88ccff");
                ActivityLog.Add("Quiz started.");
                ChatScroller.ScrollToBottom();
                return;
            }

            // check if the user wants to view the activity log
            if (ActivityLog.IsViewLogCommand(input))
            {
                bool showAll = ActivityLog.IsShowMoreCommand(input);
                AppendMessage("BOT", ActivityLog.FormatLog(showAll), "#88ccff");
                ChatScroller.ScrollToBottom();
                return;
            }

            // detect sentiment and update the status bar
            string sentiment = SentimentDetector.Detect(input);
            UpdateStatus(sentiment);

            // store favourite topic if mentioned
            MemoryEngine.CheckAndStore(input, ref _rememberedTopic);

            // check for task assistant commands first
            string response;
            string newTopic = _lastTopic;

            if (TaskAssistant.IsViewTasksCommand(input))
            {
                response = TaskAssistant.FormatTaskList();
            }
            else if (TaskAssistant.IsCompleteTaskCommand(input))
            {
                int? id = TaskAssistant.ExtractTaskId(input);
                if (id.HasValue && DatabaseHelper.CompleteTask(id.Value))
                {
                    response = $"Marked task #{id} as completed. Great job staying on top of your security!";
                    ActivityLog.Add($"Task #{id} marked as completed.");
                }
                else
                    response = "I couldn't find that task. Try 'show tasks' to see the list with IDs.";
            }
            else if (TaskAssistant.IsDeleteTaskCommand(input))
            {
                int? id = TaskAssistant.ExtractTaskId(input);
                if (id.HasValue && DatabaseHelper.DeleteTask(id.Value))
                {
                    response = $"Deleted task #{id}.";
                    ActivityLog.Add($"Task #{id} deleted.");
                }
                else
                    response = "I couldn't find that task. Try 'show tasks' to see the list with IDs.";
            }
            else if (TaskAssistant.IsAddTaskCommand(input))
            {
                string title = TaskAssistant.ExtractTaskTitle(input);
                DateTime? reminder = TaskAssistant.ExtractReminderDate(input);
                bool ok = DatabaseHelper.AddTask(title, title, reminder);

                if (ok)
                {
                    response = reminder.HasValue
                        ? $"Task added: '{title}'. Reminder set for {reminder.Value:dd MMM yyyy}."
                        : $"Task added: '{title}'. Would you like to set a reminder for this task?";
                    ActivityLog.Add(reminder.HasValue
                        ? $"Task added: '{title}' (Reminder set for {reminder.Value:dd MMM yyyy})."
                        : $"Task added: '{title}' (no reminder set).");
                }
                else
                {
                    response = "I couldn't save that task - please check the database connection.";
                }
            }
            else
            {
                // fall back to the normal response engine
                response = ResponseEngine.GetResponse(input, _rememberedTopic, _lastTopic, out newTopic);
            }

            _lastTopic = newTopic;

            // add empathy line if the user seems worried or frustrated
            if (sentiment == "worried" || sentiment == "frustrated")
            {
                AppendMessage("BOT", SentimentDetector.GetEmpathyLine(sentiment), "#ff9944");
            }

            // show the main response
            AppendMessage("BOT", response, "#00ffcc");

            // scroll to the bottom
            ChatScroller.ScrollToBottom();
        }

        // adds a message to the chat, sender name in colour, message in white
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

        // changes the status text and colour based on detected mood
        private void UpdateStatus(string sentiment)
        {
            StatusLabel.Text = sentiment switch
            {
                "worried" => "Mood detected: Worried - I'll be extra supportive!",
                "curious" => "Mood detected: Curious - great, let's explore!",
                "frustrated" => "Mood detected: Frustrated - I'll keep it simple.",
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