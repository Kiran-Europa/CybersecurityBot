using System;
using System.Collections.Generic;

namespace CybersecurityBotGUI
{
    static class ResponseEngine
    {
        static readonly Random Rng = new Random();

        static readonly Dictionary<string[], string> SingleResponses = new()
        {
            { new[] { "how are you", "you ok", "how r you" },
                "I'm fully operational and my firewall is up! How are you keeping safe online?" },
            { new[] { "purpose", "what do you do", "who are you" },
                "I'm the Cybersecurity Awareness Bot. I help you stay safe online - ask me about passwords, phishing, browsing, privacy or scams." },
            { new[] { "password", "pw", "passwd" },
                "Password tips:\n  - Use 12+ characters with letters, numbers and symbols.\n  - Never reuse passwords across sites.\n  - Use a password manager like Bitwarden.\n  - Enable two-factor authentication (2FA)." },
            { new[] { "privacy" },
                "Privacy tips:\n  - Review app permissions regularly.\n  - Use a VPN on public Wi-Fi.\n  - Check what data websites collect about you.\n  - Use private browsing when needed." },
            { new[] { "scam", "fraud" },
                "Scam awareness:\n  - If something seems too good to be true, it usually is.\n  - Never send money to someone you haven't met in person.\n  - Verify requests by calling the organisation directly.\n  - Report scams to your local consumer protection agency." },
            { new[] { "browsing", "safe browsing", "internet safety" },
                "Safe browsing:\n  - Only use HTTPS websites (padlock in address bar).\n  - Avoid downloading from untrusted sources.\n  - Keep your browser and extensions updated.\n  - Use an ad blocker to reduce malicious ads." },
            { new[] { "help", "topics", "menu" },
                "Topics I can help with:\n  - password\n  - phishing\n  - browsing\n  - privacy\n  - scam\n  - 2fa\n  - malware\n\nYou can also say 'tell me more' or 'give me another tip'." },
            { new[] { "2fa", "two factor", "two-factor", "authentication" },
                "Two-Factor Authentication (2FA):\n  - Adds a second layer beyond just your password.\n  - Use an authenticator app like Google Authenticator.\n  - Avoid SMS 2FA if possible - it can be intercepted.\n  - Enable 2FA on email, banking and social media first." },
            { new[] { "malware", "virus", "ransomware" },
                "Malware protection:\n  - Keep your OS and software updated.\n  - Don't open attachments from unknown senders.\n  - Use reputable antivirus software.\n  - Back up your files regularly in case of ransomware." },
        };

        // phishing gets a different random tip each time instead of always the same one
        static readonly List<string> PhishingResponses = new()
        {
            "Phishing tip: Check the sender's real email address, not just the display name.",
            "Phishing tip: Hover over links before clicking to see the real destination URL.",
            "Phishing tip: Legitimate companies never ask for your password via email.",
            "Phishing tip: Watch for urgent language like 'Your account will be closed!'.",
            "Phishing tip: When in doubt, go directly to the website instead of clicking links.",
        };

        public static string GetResponse(string input, string rememberedTopic, string lastTopic, out string newTopic)
        {
            newTopic = lastTopic;
            string normalised = input.Trim().ToLowerInvariant();

            // handle "tell me more" type follow-ups using whatever topic we last talked about
            if (normalised is "tell me more" or "more" or "give me another tip" or "explain more" or "continue")
            {
                if (!string.IsNullOrEmpty(lastTopic))
                    return GetFollowUp(lastTopic);
                return "What topic would you like to know more about? Try 'password', 'phishing' or 'privacy'.";
            }

            if (normalised.Contains("phish"))
            {
                newTopic = "phishing";
                return PhishingResponses[Rng.Next(PhishingResponses.Count)];
            }

            foreach (var (keywords, response) in SingleResponses)
            {
                foreach (string keyword in keywords)
                {
                    if (normalised.Contains(keyword))
                    {
                        newTopic = keyword;
                        // if they mentioned a different favourite topic earlier, tie it back in
                        if (!string.IsNullOrEmpty(rememberedTopic) && rememberedTopic != keyword)
                            return response + $"\n\nBy the way, since you're interested in {rememberedTopic}, make sure to apply these tips there too!";
                        return response;
                    }
                }
            }

            return "I didn't quite understand that. Could you rephrase? Type 'help' to see what I can answer.";
        }

        static string GetFollowUp(string topic)
        {
            return topic switch
            {
                "password" => "Extra password tip: Change passwords immediately if you hear about a data breach on a site you use.",
                "phishing" => PhishingResponses[Rng.Next(PhishingResponses.Count)],
                "privacy" => "Extra privacy tip: Regularly search your name online to see what information is publicly available.",
                "scam" => "Extra scam tip: Be suspicious of any unsolicited contact - phone, email or text.",
                "browsing" => "Extra browsing tip: Clear your cookies and cache regularly to reduce tracking.",
                "2fa" => "Extra 2FA tip: Store your backup codes somewhere safe offline in case you lose your device.",
                "malware" => "Extra malware tip: Avoid plugging in unknown USB drives - they can silently install malware.",
                _ => "Try asking about a specific topic like 'password' or 'phishing' for more tips."
            };
        }
    }
}