using System.Collections.Generic;
using System.Linq;

namespace CybersecurityBotGUI
{
    static class NlpHelper
    {
        static readonly Dictionary<string, string[]> SynonymGroups = new()
        {
            ["task"] = new[] { "task", "todo", "to-do", "to do", "reminder", "remind" },
            ["quiz"] = new[] { "quiz", "test my knowledge", "test me", "play a game", "mini game", "minigame" },
            ["password"] = new[] { "password", "pw", "passwd", "pass word" },
            ["phishing"] = new[] { "phishing", "phish", "fake email", "scam email", "spoofed email" },
            ["privacy"] = new[] { "privacy", "private data", "personal info", "personal information" },
            ["help"] = new[] { "help", "topics", "menu", "options", "what can you do" },
            ["log"] = new[] { "activity log", "show log", "log", "what have you done", "what did you do", "history" },
        };

        public static bool MatchesIntent(string input, string intent)
        {
            if (!SynonymGroups.ContainsKey(intent)) return false;

            string lower = input.ToLowerInvariant();
            return SynonymGroups[intent].Any(phrase => lower.Contains(phrase));
        }

        public static string? DetectIntent(string input)
        {
            string lower = input.ToLowerInvariant();

            foreach (var group in SynonymGroups)
            {
                if (group.Value.Any(phrase => lower.Contains(phrase)))
                    return group.Key;
            }

            return null;
        }

        public static string Normalise(string input)
        {
            string result = input;

            var replacements = new Dictionary<string, string>
            {
                { " u ", " you " },
                { " r ", " are " },
                { " pls ", " please " },
                { " plz ", " please " },
                { " 2fa ", " two factor authentication " },
            };

            string padded = " " + result.ToLowerInvariant() + " ";
            foreach (var pair in replacements)
                padded = padded.Replace(pair.Key, pair.Value);

            return padded.Trim();
        }
    }
}