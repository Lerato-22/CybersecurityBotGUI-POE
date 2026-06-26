using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CybersecurityBotGUI_part_2
{
    public partial class QuizWindow : Window
    {
        private List<QuizQuestion> _questions;
        private int _currentIndex = 0;
        private int _score = 0;

        public QuizWindow()
        {
            InitializeComponent();
            LoadQuestions();
            ShowQuestion();
            ActivityLog.Add("Quiz opened");
        }

        private void LoadQuestions()
        {
            _questions = new List<QuizQuestion>
            {
                new QuizQuestion("What should you do if you receive an email asking for your password?",
                    new[] { "Reply with your password", "Delete the email", "Report it as phishing", "Ignore it" }, 2,
                    "Reporting phishing emails helps prevent scams and protects others."),

                new QuizQuestion("What does HTTPS mean in a website URL?",
                    new[] { "The site is fast", "The site is secure", "The site is free", "The site is popular" }, 1,
                    "HTTPS means the connection is encrypted and secure."),

                new QuizQuestion("How often should you change your passwords?",
                    new[] { "Never", "Every 5 years", "Every 3-6 months", "Only when hacked" }, 2,
                    "Changing passwords every 3-6 months reduces the risk of unauthorised access."),

                new QuizQuestion("What is two-factor authentication (2FA)?",
                    new[] { "Two passwords", "A backup email", "An extra security layer beyond your password", "A VPN service" }, 2,
                    "2FA adds an extra layer of security by requiring a second verification step."),

                new QuizQuestion("True or False: Your bank will ask for your PIN via SMS.",
                    new[] { "True", "False" }, 1,
                    "Legitimate banks will NEVER ask for your PIN via SMS or email — it's always a scam."),

                new QuizQuestion("What is phishing?",
                    new[] { "A type of malware", "A trick to steal your personal info", "A secure browser", "A firewall type" }, 1,
                    "Phishing is when criminals trick you into giving up personal information."),

                new QuizQuestion("Which password is the strongest?",
                    new[] { "password123", "John1990", "Tr@v3l!ngS@fe2024", "qwerty" }, 2,
                    "Strong passwords use uppercase, lowercase, numbers and symbols — at least 12 characters."),

                new QuizQuestion("True or False: Public WiFi is safe for online banking.",
                    new[] { "True", "False" }, 1,
                    "Never do banking on public WiFi — use mobile data instead."),

                new QuizQuestion("What is ransomware?",
                    new[] { "Software that speeds up your PC", "Malware that locks your files for payment", "A type of antivirus", "A browser extension" }, 1,
                    "Ransomware encrypts your files and demands payment to restore access."),

                new QuizQuestion("What should you do before clicking a link in an email?",
                    new[] { "Click it immediately", "Hover over it to check the real URL", "Forward it to friends", "Reply to the sender" }, 1,
                    "Always hover over links to verify the real destination URL before clicking."),

                new QuizQuestion("True or False: Using the same password for all accounts is safe.",
                    new[] { "True", "False" }, 1,
                    "Using the same password everywhere means one breach exposes all your accounts."),

                new QuizQuestion("What is social engineering?",
                    new[] { "Building social media apps", "Manipulating people to reveal confidential info", "A type of encryption", "Network security" }, 1,
                    "Social engineering manipulates human psychology rather than technical vulnerabilities.")
            };
        }

        private void ShowQuestion()
        {
            if (_currentIndex >= _questions.Count)
            {
                ShowResults();
                return;
            }

            var q = _questions[_currentIndex];
            QuestionNumber.Text = $"Question {_currentIndex + 1} of {_questions.Count}";
            QuestionText.Text = q.Question;
            FeedbackTextBlock.Text = "";
            FeedbackText.Visibility = Visibility.Collapsed;
            NextButton.Visibility = Visibility.Collapsed;

            AnswerPanel.Children.Clear();
            for (int i = 0; i < q.Options.Length; i++)
            {
                int index = i;
                Button btn = new Button
                {
                    Content = q.Options[i],
                    Tag = i,
                    Background = new SolidColorBrush(Color.FromRgb(26, 26, 46)),
                    Foreground = Brushes.White,
                    BorderBrush = new SolidColorBrush(Color.FromRgb(123, 47, 190)),
                    BorderThickness = new Thickness(1),
                    Padding = new Thickness(12, 10, 12, 10),
                    Margin = new Thickness(0, 0, 0, 8),
                    FontSize = 13,
                    Cursor = System.Windows.Input.Cursors.Hand,
                    HorizontalContentAlignment = HorizontalAlignment.Left
                };
                btn.Click += (s, e) => AnswerSelected(index);
                AnswerPanel.Children.Add(btn);
            }
        }

        private void AnswerSelected(int selectedIndex)
        {
            var q = _questions[_currentIndex];
            bool correct = selectedIndex == q.CorrectIndex;

            if (correct) _score++;

            for (int i = 0; i < AnswerPanel.Children.Count; i++)
            {
                Button btn = (Button)AnswerPanel.Children[i];
                btn.IsEnabled = false;
                if (i == q.CorrectIndex)
                    btn.Background = new SolidColorBrush(Colors.Green);
                else if (i == selectedIndex && !correct)
                    btn.Background = new SolidColorBrush(Colors.DarkRed);
            }

            FeedbackTextBlock.Text = correct
                ? $"✅ Correct! {q.Explanation}"
                : $"❌ Incorrect. {q.Explanation}";
            FeedbackTextBlock.Foreground = correct ? Brushes.LightGreen : Brushes.OrangeRed;
            FeedbackText.Visibility = Visibility.Visible;
            NextButton.Visibility = Visibility.Visible;

            ActivityLog.Add($"Quiz Q{_currentIndex + 1}: {(correct ? "correct" : "incorrect")}");
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _currentIndex++;
            ShowQuestion();
        }

        private void ShowResults()
        {
            QuestionPanel.Visibility = Visibility.Collapsed;
            ResultPanel.Visibility = Visibility.Visible;

            ScoreText.Text = $"{_score} / {_questions.Count}";

            if (_score >= 10)
                ResultMessage.Text = "🏆 Outstanding! You're a cybersecurity pro!";
            else if (_score >= 7)
                ResultMessage.Text = "👍 Great job! You know your cybersecurity basics.";
            else if (_score >= 5)
                ResultMessage.Text = "📚 Not bad! Keep learning to stay safer online.";
            else
                ResultMessage.Text = "💡 Keep practising — cybersecurity knowledge could save you!";

            ActivityLog.Add($"Quiz completed — score: {_score}/{_questions.Count}");
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            _currentIndex = 0;
            _score = 0;
            ResultPanel.Visibility = Visibility.Collapsed;
            QuestionPanel.Visibility = Visibility.Visible;
            ActivityLog.Add("Quiz restarted");
            ShowQuestion();
        }
    }

    public class QuizQuestion
    {
        public string Question { get; set; }
        public string[] Options { get; set; }
        public int CorrectIndex { get; set; }
        public string Explanation { get; set; }

        public QuizQuestion(string question, string[] options, int correctIndex, string explanation)
        {
            Question = question;
            Options = options;
            CorrectIndex = correctIndex;
            Explanation = explanation;
        }
    }
}