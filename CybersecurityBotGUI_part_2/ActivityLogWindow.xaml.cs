using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace CybersecurityBotGUI_part_2
{
    public partial class ActivityLogWindow : Window
    {
        private int _currentCount = 10;

        public ActivityLogWindow()
        {
            InitializeComponent();
            LoadLog(_currentCount);
        }

        private void LoadLog(int count)
        {
            LogPanel.Children.Clear();
            List<string> entries = ActivityLog.GetRecent(count);

            if (entries.Count == 0)
            {
                LogPanel.Children.Add(new System.Windows.Controls.TextBlock
                {
                    Text = "No activity recorded yet.",
                    Foreground = new SolidColorBrush(Colors.Gray),
                    FontSize = 13
                });
                return;
            }

            foreach (string entry in entries)
            {
                System.Windows.Controls.TextBlock tb = new System.Windows.Controls.TextBlock
                {
                    Text = entry,
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 13,
                    TextWrapping = System.Windows.TextWrapping.Wrap,
                    Margin = new Thickness(0, 0, 0, 8)
                };
                LogPanel.Children.Add(tb);
            }
        }

        private void ShowMore_Click(object sender, RoutedEventArgs e)
        {
            _currentCount += 10;
            LoadLog(_currentCount);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}