using System;
using System.Collections.Generic;

namespace CybersecurityBot
{
    static class ResponseEngine
    {
        // list of rules - each rule has a set of keywords and a matching response
        private static readonly List<(string[] keywords, string response)> Rules = new()
        {
            (
                new[] { "how are you", "how r you", "you ok" },
                "I'm running at full capacity and my firewall is up! How are YOU keeping your data safe today?"
            ),
            (
                new[] { "purpose", "what do you do", "who are you" },
                "I'm the Cybersecurity Awareness Bot. I educate you on staying safe online — passwords, phishing, and safe browsing."
            ),
            (
                new[] { "password" },
                "Password Safety Tips:\n" +
                "  - Use at least 12 characters with letters, numbers, and symbols.\n" +
                "  - Never reuse passwords across different websites.\n" +
                "  - Use a password manager like Bitwarden or 1Password.\n" +
                "  - Enable two-factor authentication (2FA) wherever possible."
            ),
            (
                new[] { "phishing", "scam email", "fake email" },
                "Spotting Phishing Attacks:\n" +
                "  - Check the sender's real email address carefully.\n" +
                "  - Hover over links before clicking to see the real URL.\n" +
                "  - Legit companies never ask for passwords via email.\n" +
                "  - Watch for urgent language like 'Your account will be closed!'"
            ),
            (
                new[] { "browsing", "safe browsing", "internet safety" },
                "Safe Browsing Practices:\n" +
                "  - Only use websites with HTTPS (padlock in address bar).\n" +
                "  - Avoid downloading software from untrusted sources.\n" +
                "  - Keep your browser and extensions up to date.\n" +
                "  - Avoid sensitive accounts on public Wi-Fi without a VPN."
            ),
            // null response means the help menu should be shown instead
            (
                new[] { "help", "topics", "menu" },
                null
            ),
        };

        public static string? GetResponse(string input, out bool isHelp)
        {
            isHelp = false;

            // handle empty input
            if (string.IsNullOrWhiteSpace(input))
                return "I didn't quite understand that. Could you rephrase?";

            // convert to lowercase so matching works regardless of how user typed it
            string normalised = input.Trim().ToLowerInvariant();

            // loop through each rule and check if the input contains any of its keywords
            foreach (var (keywords, response) in Rules)
            {
                foreach (string keyword in keywords)
                {
                    if (normalised.Contains(keyword))
                    {
                        // if response is null it means show the help menu
                        if (response == null) { isHelp = true; return null; }
                        return response;
                    }
                }
            }

            // nothing matched so return a default message
            return $"I didn't understand '{input.Trim()}'. Type 'help' to see what I can answer.";
        }
    }
}