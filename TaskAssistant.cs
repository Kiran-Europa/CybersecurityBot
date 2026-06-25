using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CybersecurityBotGUI
{
    static class TaskAssistant
    {
        public static bool IsAddTaskCommand(string input)
        {
            string lower = input.ToLowerInvariant();
            bool mentionsTask = NlpHelper.MatchesIntent(input, "task");
            bool mentionsAdd = lower.Contains("add") || lower.Contains("create") || lower.Contains("set");
            return mentionsTask && mentionsAdd;
        }

        public static bool IsViewTasksCommand(string input)
        {
            string lower = input.ToLowerInvariant();
            return lower.Contains("show task") || lower.Contains("view task")
                || lower.Contains("my tasks") || lower.Contains("list task");
        }

        public static bool IsCompleteTaskCommand(string input)
        {
            string lower = input.ToLowerInvariant();
            return lower.Contains("complete task") || lower.Contains("mark task")
                || lower.Contains("finish task") || lower.Contains("done with task");
        }

        public static bool IsDeleteTaskCommand(string input)
        {
            string lower = input.ToLowerInvariant();
            return lower.Contains("delete task") || lower.Contains("remove task");
        }

        public static string ExtractTaskTitle(string input)
        {
            string lower = input.ToLowerInvariant();

            int remindIndex = lower.IndexOf("remind me to");
            if (remindIndex >= 0)
            {
                string after = input.Substring(remindIndex + "remind me to".Length).Trim();
                after = RemoveDatePhrase(after);
                return Capitalise(after);
            }

            string[] triggers = { "add a task to", "add task to", "add a task:", "add task:", "create a task to", "create task to", "add a task", "add task" };
            foreach (string trigger in triggers)
            {
                int idx = lower.IndexOf(trigger);
                if (idx >= 0)
                {
                    string after = input.Substring(idx + trigger.Length).Trim(':', ' ');
                    after = RemoveDatePhrase(after);
                    if (!string.IsNullOrWhiteSpace(after))
                        return Capitalise(after);
                }
            }

            return "New Task";
        }

        static string RemoveDatePhrase(string text)
        {
            text = Regex.Replace(text, @"\s*(tomorrow|today)\s*$", "", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"\s*in\s+\d+\s+days?\s*$", "", RegexOptions.IgnoreCase);
            return text.Trim();
        }

        public static DateTime? ExtractReminderDate(string input)
        {
            string lower = input.ToLowerInvariant();

            if (lower.Contains("tomorrow"))
                return DateTime.Today.AddDays(1);

            if (lower.Contains("today"))
                return DateTime.Today;

            var match = Regex.Match(lower, @"in\s+(\d+)\s+days?");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int days))
                return DateTime.Today.AddDays(days);

            return null;
        }

        public static int? ExtractTaskId(string input)
        {
            var match = Regex.Match(input, @"\d+");
            if (match.Success && int.TryParse(match.Value, out int id))
                return id;
            return null;
        }

        public static string FormatTaskList()
        {
            var tasks = DatabaseHelper.GetAllTasks();

            if (tasks.Count == 0)
                return "You don't have any tasks yet. Try saying 'add a task to enable two-factor authentication'.";

            var sb = new StringBuilder();
            sb.AppendLine("Here are your tasks:");

            foreach (var t in tasks)
            {
                string status = t.IsCompleted ? "[DONE]" : "[ ]";
                string reminder = t.ReminderDate.HasValue ? $" — reminder on {t.ReminderDate.Value:dd MMM yyyy}" : "";
                sb.AppendLine($"  {status} #{t.Id}: {t.Title}{reminder}");
            }

            sb.Append("\nSay 'complete task [id]' or 'delete task [id]' to manage them.");
            return sb.ToString();
        }

        static string Capitalise(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "New Task";
            return char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}