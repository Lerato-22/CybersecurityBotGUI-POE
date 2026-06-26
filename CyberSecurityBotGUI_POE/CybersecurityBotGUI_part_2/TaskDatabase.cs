using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace CybersecurityBotGUI_part_2
{
    public class CyberTask
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class TaskDatabase
    {
        private const string ConnectionString =
            "server=localhost;database=CyberBotDB;user=root;password=Root@123;";

        // Add a new task to the database
        public static bool AddTask(string title, string description, DateTime? reminderDate = null)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Tasks (Title, Description, ReminderDate) VALUES (@title, @desc, @reminder)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@desc", description);
                        cmd.Parameters.AddWithValue("@reminder", (object)reminderDate ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB Error: " + ex.Message);
                return false;
            }
        }

        // Get all tasks from the database
        public static List<CyberTask> GetAllTasks()
        {
            var tasks = new List<CyberTask>();
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Tasks ORDER BY CreatedAt DESC";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new CyberTask
                            {
                                TaskId = reader.GetInt32("TaskId"),
                                Title = reader.GetString("Title"),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString("Description"),
                                ReminderDate = reader.IsDBNull(reader.GetOrdinal("ReminderDate")) ? (DateTime?)null : reader.GetDateTime("ReminderDate"),
                                IsCompleted = reader.GetBoolean("IsCompleted"),
                                CreatedAt = reader.GetDateTime("CreatedAt")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB Error: " + ex.Message);
            }
            return tasks;
        }

        // Mark a task as completed
        public static bool MarkAsCompleted(int taskId)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    string query = "UPDATE Tasks SET IsCompleted = TRUE WHERE TaskId = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", taskId);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB Error: " + ex.Message);
                return false;
            }
        }

        // Delete a task from the database
        public static bool DeleteTask(int taskId)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Tasks WHERE TaskId = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", taskId);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB Error: " + ex.Message);
                return false;
            }
        }
    }
}