using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CybersecurityBotGUI
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Description { get; set; } = "";
    }

    static class ActivityLog
    {
        static readonly List<LogEntry> Entries = new();

        const int DefaultShowCount = 10;

        public static void Add(string description)
        {
            Entries.Add(new LogEntry
            {
                Timestamp = DateTime.Now,
                Description = description
            });
        }

        public static bool IsViewLogCommand(string input)
        {
            return NlpHelper.MatchesIntent(input, "log");
        }

        public static bool IsShowMoreCommand(string input)
        {
            string lower = input.ToLowerInvariant();
            return lower.Contains("show more") || lower.Contains("full history") || lower.Contains("show all");
        }

        public static string FormatLog(bool showAll = false)
        {
            if (Entries.Count == 0)
                return "No actions have been logged yet. Try adding a task or starting the quiz!";

            var entriesToShow = showAll
                ? Entries
                : Entries.Skip(Math.Max(0, Entries.Count - DefaultShowCount));

            var sb = new StringBuilder();
            sb.AppendLine(showAll ? "Full activity history:" : "Here's a summary of recent actions:");

            int number = 1;
            foreach (var entry in entriesToShow)
            {
                sb.AppendLine($"  {number}. [{entry.Timestamp:HH:mm}] {entry.Description}");
                number++;
            }

            if (!showAll && Entries.Count > DefaultShowCount)
                sb.Append("\nSay 'show more' to see the full history.");

            return sb.ToString();
        }
    }
}