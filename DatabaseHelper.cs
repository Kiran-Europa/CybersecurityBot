using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace CybersecurityBotGUI
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
    }

    static class DatabaseHelper
    {
        private const string ConnectionString =
            "Server=localhost;Database=cybersecurity_bot;Uid=root;Pwd=FakePswd;";

        public static bool AddTask(string title, string description, DateTime? reminderDate)
        {
            try
            {
                using var conn = new MySqlConnection(ConnectionString);
                conn.Open();

                string sql = "INSERT INTO tasks (title, description, reminder_date) VALUES (@title, @desc, @date)";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@desc", description);
                cmd.Parameters.AddWithValue("@date", reminderDate.HasValue ? reminderDate.Value : (object)DBNull.Value);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB Error: " + ex.Message);
                return false;
            }
        }

        public static List<TaskItem> GetAllTasks()
        {
            var tasks = new List<TaskItem>();
            try
            {
                using var conn = new MySqlConnection(ConnectionString);
                conn.Open();

                string sql = "SELECT id, title, description, reminder_date, is_completed FROM tasks ORDER BY id DESC";
                using var cmd = new MySqlCommand(sql, conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tasks.Add(new TaskItem
                    {
                        Id = reader.GetInt32("id"),
                        Title = reader.GetString("title"),
                        Description = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description"),
                        ReminderDate = reader.IsDBNull(reader.GetOrdinal("reminder_date")) ? null : reader.GetDateTime("reminder_date"),
                        IsCompleted = reader.GetBoolean("is_completed")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB Error: " + ex.Message);
            }
            return tasks;
        }

        public static bool CompleteTask(int id)
        {
            try
            {
                using var conn = new MySqlConnection(ConnectionString);
                conn.Open();
                string sql = "UPDATE tasks SET is_completed = 1 WHERE id = @id";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB Error: " + ex.Message);
                return false;
            }
        }

        public static bool DeleteTask(int id)
        {
            try
            {
                using var conn = new MySqlConnection(ConnectionString);
                conn.Open();
                string sql = "DELETE FROM tasks WHERE id = @id";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB Error: " + ex.Message);
                return false;
            }
        }
    }
}
