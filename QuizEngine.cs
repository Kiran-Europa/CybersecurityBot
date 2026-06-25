using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityBotGUI
{
    public class QuizQuestion
    {
        public string Question { get; set; } = "";
        public List<string> Options { get; set; } = new();
        public int CorrectIndex { get; set; }
        public string Explanation { get; set; } = "";
    }

    class QuizEngine
    {
        public bool IsActive { get; private set; }
        public int Score { get; private set; }
        public int QuestionsAnswered { get; private set; }

        List<QuizQuestion> _questions = new();
        int _currentIndex;

        static readonly Random Rng = new Random();

        static readonly List<QuizQuestion> QuestionBank = new()
        {
            new QuizQuestion
            {
                Question = "What should you do if you receive an email asking for your password?",
                Options = new() { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                CorrectIndex = 2,
                Explanation = "Reporting phishing emails helps prevent scams and protects others too."
            },
            new QuizQuestion
            {
                Question = "True or False: It's safe to reuse the same password across multiple websites.",
                Options = new() { "True", "False" },
                CorrectIndex = 1,
                Explanation = "Reusing passwords means one breach can compromise all your accounts."
            },
            new QuizQuestion
            {
                Question = "Which of these is a sign of a phishing email?",
                Options = new() { "Personalised greeting with your full name", "Urgent language demanding immediate action", "Sent from a known colleague", "No links included" },
                CorrectIndex = 1,
                Explanation = "Urgency is a common manipulation tactic used in phishing to make you act without thinking."
            },
            new QuizQuestion
            {
                Question = "True or False: Public Wi-Fi is always safe for online banking.",
                Options = new() { "True", "False" },
                CorrectIndex = 1,
                Explanation = "Public Wi-Fi can be intercepted by attackers; use a VPN or mobile data for sensitive tasks."
            },
            new QuizQuestion
            {
                Question = "What does '2FA' stand for?",
                Options = new() { "Two-Factor Authentication", "Two-File Access", "Final Account Authorisation", "Two-Friend Approval" },
                CorrectIndex = 0,
                Explanation = "2FA adds a second verification step beyond just your password."
            },
            new QuizQuestion
            {
                Question = "Social engineering attacks primarily exploit:",
                Options = new() { "Software bugs", "Human psychology and trust", "Network hardware", "Weak encryption" },
                CorrectIndex = 1,
                Explanation = "Social engineering manipulates people rather than exploiting technical flaws."
            },
            new QuizQuestion
            {
                Question = "True or False: A padlock icon in the browser address bar means a website is completely safe.",
                Options = new() { "True", "False" },
                CorrectIndex = 1,
                Explanation = "It only means the connection is encrypted (HTTPS) — scam sites can have padlocks too."
            },
            new QuizQuestion
            {
                Question = "What's the safest way to verify a suspicious request from your 'bank'?",
                Options = new() { "Click the link in the email", "Reply asking for confirmation", "Call the bank directly using a known number", "Forward it to friends" },
                CorrectIndex = 2,
                Explanation = "Always verify through an independent, trusted channel — never the contact info in the suspicious message."
            },
            new QuizQuestion
            {
                Question = "Which password is strongest?",
                Options = new() { "password123", "MyDog2020", "Tr$7!qLp9#zR", "qwerty" },
                CorrectIndex = 2,
                Explanation = "Long, random combinations of characters are far harder to crack than common words or patterns."
            },
            new QuizQuestion
            {
                Question = "True or False: Pretexting is a type of social engineering where an attacker creates a fake scenario to gain trust.",
                Options = new() { "True", "False" },
                CorrectIndex = 0,
                Explanation = "Pretexting involves inventing a false context (e.g. posing as IT support) to extract information."
            },
            new QuizQuestion
            {
                Question = "What should you do before clicking a link in an unexpected email?",
                Options = new() { "Click it immediately", "Hover over it to check the real URL", "Forward it to a friend first", "Reply to ask if it's safe" },
                CorrectIndex = 1,
                Explanation = "Hovering reveals the actual destination URL, helping you spot fake or malicious links."
            },
            new QuizQuestion
            {
                Question = "True or False: Software updates often include important security patches.",
                Options = new() { "True", "False" },
                CorrectIndex = 0,
                Explanation = "Updates frequently fix vulnerabilities that attackers could otherwise exploit."
            },
        };

        public string StartQuiz()
        {
            _questions = QuestionBank.OrderBy(q => Rng.Next()).ToList();
            _currentIndex = 0;
            Score = 0;
            QuestionsAnswered = 0;
            IsActive = true;

            return FormatCurrentQuestion();
        }

        string FormatCurrentQuestion()
        {
            var q = _questions[_currentIndex];
            string options = "";
            for (int i = 0; i < q.Options.Count; i++)
                options += $"\n  {(char)('A' + i)}) {q.Options[i]}";

            return $"Question {_currentIndex + 1}/{_questions.Count}:\n{q.Question}{options}\n\nType the letter of your answer (e.g. 'A').";
        }

        public bool LooksLikeAnswer(string input)
        {
            string trimmed = input.Trim();
            return trimmed.Length == 1 && char.IsLetter(trimmed[0]);
        }

        public string SubmitAnswer(string input, out bool quizFinished)
        {
            quizFinished = false;
            var q = _questions[_currentIndex];

            int chosenIndex = char.ToUpper(input.Trim()[0]) - 'A';

            string feedback;
            if (chosenIndex == q.CorrectIndex)
            {
                Score++;
                feedback = $"Correct! {q.Explanation}";
            }
            else
            {
                string correctLetter = ((char)('A' + q.CorrectIndex)).ToString();
                feedback = $"Not quite. The correct answer was {correctLetter}) {q.Options[q.CorrectIndex]}. {q.Explanation}";
            }

            QuestionsAnswered++;
            _currentIndex++;

            if (_currentIndex >= _questions.Count)
            {
                IsActive = false;
                quizFinished = true;
                return feedback + "\n\n" + FormatFinalScore();
            }

            return feedback + "\n\n" + FormatCurrentQuestion();
        }

        string FormatFinalScore()
        {
            double percent = (double)Score / _questions.Count * 100;
            string encouragement = percent >= 80
                ? "Great job! You're a cybersecurity pro!"
                : percent >= 50
                    ? "Good effort! Keep learning to stay safe online."
                    : "Keep learning to stay safe online — practice makes perfect!";

            return $"Quiz complete! You scored {Score}/{_questions.Count} ({percent:0}%).\n{encouragement}";
        }
    }
}