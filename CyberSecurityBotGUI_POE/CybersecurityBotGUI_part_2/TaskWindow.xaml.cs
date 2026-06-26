using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CybersecurityBotGUI_part_2
{
    public partial class TaskWindow : Window
    {
        public TaskWindow()
        {
            InitializeComponent();
            LoadTasks();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleInput.Text.Trim();
            string desc = DescInput.Text.Trim();
            DateTime? reminder = ReminderPicker.SelectedDate;

            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Please enter a task title.", "Missing Title",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool success = TaskDatabase.AddTask(title, desc, reminder);

            if (success)
            {
                ActivityLog.Add($"Task added: '{title}'" + (reminder.HasValue ? $" (Reminder: {reminder.Value:dd MMM yyyy})" : ""));
                TitleInput.Clear();
                DescInput.Clear();
                ReminderPicker.SelectedDate = null;
                LoadTasks();
            }
            else
            {
                MessageBox.Show("Failed to add task. Check your database connection.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadTasks()
        {
            TaskList.Children.Clear();
            var tasks = TaskDatabase.GetAllTasks();

            if (tasks.Count == 0)
            {
                TextBlock empty = new TextBlock
                {
                    Text = "No tasks yet — add one above!",
                    Foreground = new SolidColorBrush(Colors.Gray),
                    FontSize = 13,
                    Margin = new Thickness(0, 10, 0, 0)
                };
                TaskList.Children.Add(empty);
                return;
            }

            foreach (var task in tasks)
            {
                Border card = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(13, 13, 26)),
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(12),
                    Margin = new Thickness(0, 0, 0, 10),
                    BorderBrush = task.IsCompleted
                        ? new SolidColorBrush(Colors.Green)
                        : new SolidColorBrush(Color.FromRgb(123, 47, 190)),
                    BorderThickness = new Thickness(1)
                };

                StackPanel cardContent = new StackPanel();

                Grid topRow = new Grid();
                topRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                topRow.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                string statusIcon = task.IsCompleted ? "✅" : "⏳";
                TextBlock titleText = new TextBlock
                {
                    Text = $"{statusIcon} {task.Title}",
                    Foreground = task.IsCompleted
                        ? new SolidColorBrush(Colors.LightGreen)
                        : new SolidColorBrush(Colors.White),
                    FontSize = 14,
                    FontWeight = FontWeights.Bold,
                    TextWrapping = TextWrapping.Wrap
                };
                Grid.SetColumn(titleText, 0);

                StackPanel btnPanel = new StackPanel { Orientation = Orientation.Horizontal };

                if (!task.IsCompleted)
                {
                    Button completeBtn = new Button
                    {
                        Content = "✔ Complete",
                        Tag = task.TaskId,
                        Background = new SolidColorBrush(Colors.Green),
                        Foreground = new SolidColorBrush(Colors.White),
                        BorderThickness = new Thickness(0),
                        Padding = new Thickness(8, 4, 8, 4),
                        Margin = new Thickness(0, 0, 6, 0),
                        Cursor = System.Windows.Input.Cursors.Hand,
                        FontSize = 11
                    };
                    completeBtn.Click += CompleteTask_Click;
                    btnPanel.Children.Add(completeBtn);
                }

                Button deleteBtn = new Button
                {
                    Content = "🗑 Delete",
                    Tag = task.TaskId,
                    Background = new SolidColorBrush(Colors.DarkRed),
                    Foreground = new SolidColorBrush(Colors.White),
                    BorderThickness = new Thickness(0),
                    Padding = new Thickness(8, 4, 8, 4),
                    Cursor = System.Windows.Input.Cursors.Hand,
                    FontSize = 11
                };
                deleteBtn.Click += DeleteTask_Click;
                btnPanel.Children.Add(deleteBtn);

                Grid.SetColumn(btnPanel, 1);
                topRow.Children.Add(titleText);
                topRow.Children.Add(btnPanel);
                cardContent.Children.Add(topRow);

                if (!string.IsNullOrEmpty(task.Description))
                {
                    TextBlock descText = new TextBlock
                    {
                        Text = task.Description,
                        Foreground = new SolidColorBrush(Colors.LightGray),
                        FontSize = 12,
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 6, 0, 0)
                    };
                    cardContent.Children.Add(descText);
                }

                if (task.ReminderDate.HasValue)
                {
                    TextBlock reminderText = new TextBlock
                    {
                        Text = $"🔔 Reminder: {task.ReminderDate.Value:dd MMM yyyy}",
                        Foreground = new SolidColorBrush(Colors.Yellow),
                        FontSize = 11,
                        Margin = new Thickness(0, 4, 0, 0)
                    };
                    cardContent.Children.Add(reminderText);
                }

                card.Child = cardContent;
                TaskList.Children.Add(card);
            }
        }

        private void CompleteTask_Click(object sender, RoutedEventArgs e)
        {
            int taskId = (int)((Button)sender).Tag;
            TaskDatabase.MarkAsCompleted(taskId);
            ActivityLog.Add($"Task #{taskId} marked as completed");
            LoadTasks();
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            int taskId = (int)((Button)sender).Tag;
            var result = MessageBox.Show("Are you sure you want to delete this task?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                TaskDatabase.DeleteTask(taskId);
                ActivityLog.Add($"Task #{taskId} deleted");
                LoadTasks();
            }
        }
    }
}