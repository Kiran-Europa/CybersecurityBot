using System.Collections.Generic;

namespace CybersecurityBotGUI
{
    /// <summary>
    /// Detects user mood (worried, curious, frustrated, neutral) and returns empathy text.
    /// </summary>
    static class SentimentDetector
    {
        static readonly List<string> WorriedWords = new()
        {
            "worried", "scared", "afraid", "nervous", "anxious",
            "concern", "unsafe", "danger", "threat", "hack"
        };

        static readonly List<string> CuriousWords = new()
        {
            "curious", "interested", "wonder", "how does", "what is",
            "tell me", "explain", "learn", "want to know"
        };

        static readonly List<string> FrustratedWords = new()
        {
            "frustrated", "annoyed", "angry", "confused", "dont understand",
            "don't understand", "this is hard", "complicated", "difficult"
        };

        /// <summary>
        /// Returns "worried", "frustrated", "curious", or "neutral".
        /// </summary>
        public static string Detect(string input)
        {
            string lower = input.ToLowerInvariant();

            foreach (string word in WorriedWords)
                if (lower.Contains(word)) return "worried";

            foreach (string word in FrustratedWords)
                if (lower.Contains(word)) return "frustrated";

            foreach (string word in CuriousWords)
                if (lower.Contains(word)) return "curious";

            return "neutral";
        }

        /// <summary>
        /// Returns a supportive sentence for worried or frustrated users.
        /// </summary>
        public static string GetEmpathyLine(string sentiment)
        {
            return sentiment switch
            {
                "worried" => "It's completely understandable to feel that way. Let me help put your mind at ease.",
                "frustrated" => "I hear you — cybersecurity can feel overwhelming. Let's break it down simply.",
                _ => ""
            };
        }
    }
}