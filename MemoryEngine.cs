using System.Collections.Generic;

namespace CybersecurityBotGUI
{
    /// <summary>
    /// Remembers user interests (e.g., favourite cybersecurity topic).
    /// </summary>
    static class MemoryEngine
    {
        static readonly List<string> KnownTopics = new()
        {
            "privacy", "password", "phishing", "browsing",
            "scam", "malware", "2fa", "security"
        };

        /// <summary>
        /// If user says "I'm interested in X", stores X as rememberedTopic.
        /// </summary>
        public static void CheckAndStore(string input, ref string rememberedTopic)
        {
            string lower = input.ToLowerInvariant();

            bool expressedInterest =
                lower.Contains("interested in") ||
                lower.Contains("i care about") ||
                lower.Contains("i want to learn about") ||
                lower.Contains("i'm worried about") ||
                lower.Contains("im worried about");

            if (!expressedInterest) return;

            foreach (string topic in KnownTopics)
            {
                if (lower.Contains(topic))
                {
                    rememberedTopic = topic;
                    return;
                }
            }
        }
    }
}