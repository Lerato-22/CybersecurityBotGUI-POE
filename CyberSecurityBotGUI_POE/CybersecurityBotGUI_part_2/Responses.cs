using System;
using System.Collections.Generic;

// Responses.cs - NLP simulation and keyword recognition for cybersecurity chatbot
namespace CybersecurityBotGUI_part_2
{
    public class Responses
    {
        private static Dictionary<string, string> _memory = new Dictionary<string, string>();
        private static string _lastTopic = "";

        private static List<string> _phishingTips = new List<string>
        {
            "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations like SARS or your bank.",
            "Check the sender's email address carefully — scammers use addresses like 'support@absa-secure.co.za' which look real but aren't.",
            "If an email creates urgency like 'Act NOW or your account will be closed', that's a red flag. Legitimate companies don't rush you like that.",
            "Never click links in emails — rather go directly to the website by typing it in your browser.",
            "Phishing messages often have spelling mistakes and poor grammar. Always read carefully before clicking anything."
        };

        private static List<string> _passwordTips = new List<string>
        {
            "Use at least 12 characters mixing uppercase, lowercase, numbers and symbols.",
            "Never use your name, birthday or ID number as a password.",
            "Use a different password for every account — a password manager like Bitwarden can help.",
            "Change your passwords every 3 to 6 months, especially for banking and email.",
            "Never share your password with anyone — not even IT support."
        };

        private static Random _random = new Random();

        public static void Remember(string key, string value)
        {
            _memory[key] = value;
        }

        public static string Recall(string key)
        {
            return _memory.ContainsKey(key) ? _memory[key] : null;
        }

        // NLP helper — checks if input contains any of the given phrases
        private static bool Contains(string input, params string[] phrases)
        {
            foreach (string phrase in phrases)
                if (input.Contains(phrase)) return true;
            return false;
        }

        public static string GetResponse(string input, string name)
        {
            input = input.ToLower().Trim();

            // NLP: TASK MANAGER triggers
            if (Contains(input,
                "add a task", "create a task", "new task", "add task",
                "set a reminder", "add a reminder", "create reminder",
                "manage tasks", "view tasks", "my tasks", "open tasks",
                "remind me", "task manager", "task list"))
            {
                ActivityLog.Add("NLP detected: task-related request");
                return "__OPEN_TASK__";
            }

            // NLP: QUIZ triggers
            if (Contains(input,
                "start quiz", "take a quiz", "play quiz", "open quiz",
                "test my knowledge", "test me", "quiz me",
                "play the game", "start the game", "cybersecurity quiz",
                "i want to be tested", "challenge me"))
            {
                ActivityLog.Add("NLP detected: quiz-related request");
                return "__OPEN_QUIZ__";
            }

            // NLP: ACTIVITY LOG triggers
            if (Contains(input,
                "activity log", "show log", "what have you done",
                "recent actions", "show activity", "action history",
                "what did you do", "log history", "show history"))
            {
                return "__OPEN_LOG__";
            }

            // GOODBYE
            if (Contains(input, "bye", "goodbye", "exit", "quit", "thank"))
            {
                return $"Goodbye {name}! 👋 Stay safe online and remember — when in doubt, don't click! See you next time! 🛡️";
            }

            // SENTIMENT DETECTION
            if (Contains(input, "worried", "scared", "afraid", "anxious"))
            {
                string tip = _phishingTips[_random.Next(_phishingTips.Count)];
                return $"It's completely understandable to feel that way, {name}. Cyber threats are real but you can protect yourself. Here's a tip to help:\n\n🛡️ {tip}";
            }

            if (Contains(input, "frustrated", "confused", "don't understand"))
            {
                return $"No worries at all, {name}! Cybersecurity can feel overwhelming at first. Try typing 'help' to see a list of topics and we'll go through them one by one. I'm here for you! 😊";
            }

            if (Contains(input, "curious", "interested", "want to learn"))
            {
                return $"I love the enthusiasm, {name}! 🎉 Curiosity is the first step to staying safe online. Type 'help' to see all the topics I can teach you about!";
            }

            // MEMORY
            if (Contains(input, "i'm interested in", "i am interested in"))
            {
                string topic = input.Replace("i'm interested in", "").Replace("i am interested in", "").Trim();
                Remember("interest", topic);
                return $"Great! I'll remember that you're interested in {topic}. It's a crucial part of staying safe online. 🔐\n\nAsk me anything about it!";
            }

            // FOLLOW-UP
            if (Contains(input, "tell me more", "explain more", "give me another tip", "more info", "another tip", "more tips"))
            {
                if (_lastTopic == "phishing")
                    return "🎣 Another phishing tip:\n\n" + _phishingTips[_random.Next(_phishingTips.Count)];
                if (_lastTopic == "password")
                    return "🔑 Another password tip:\n\n" + _passwordTips[_random.Next(_passwordTips.Count)];
                if (_lastTopic != "")
                    return $"I last talked about {_lastTopic}. Ask me more specifically about it and I'll help you! 😊";
                return "What topic would you like to know more about? Type 'help' to see the list.";
            }

            // HOW ARE YOU
            if (Contains(input, "how are you"))
                return $"I'm doing great, thanks for asking, {name}! Ready to help you stay safe online. 💪";

            // PURPOSE
            if (Contains(input, "purpose", "why are you here", "what do you do"))
                return "My purpose is to help South Africans learn about cybersecurity threats and how to stay safe online. Cybercrime is a serious problem in South Africa and I'm here to help! 🇿🇦";

            // HELP
            if (Contains(input, "help", "topics", "what can i ask"))
            {
                string interestNote = "";
                string remembered = Recall("interest");
                if (remembered != null)
                    interestNote = $"\n\n💡 Since you're interested in {remembered}, you might want to ask about that first!";

                return "Here are the topics you can ask me about:\n\n" +
                       "🔑 Passwords\n" +
                       "🎣 Phishing\n" +
                       "🌐 Safe Browsing\n" +
                       "🦠 Malware & Viruses\n" +
                       "🏦 Banking Safety\n" +
                       "📱 Social Media Safety\n" +
                       "🔐 Two-Factor Authentication\n" +
                       "🎭 Social Engineering\n\n" +
                       "💡 You can also say things like:\n" +
                       "• 'Add a task' or 'Create a reminder'\n" +
                       "• 'Start quiz' or 'Test my knowledge'\n" +
                       "• 'Show activity log'" + interestNote;
            }

            // KEYWORDS
            if (Contains(input, "password"))
            {
                _lastTopic = "password";
                string tip = _passwordTips[_random.Next(_passwordTips.Count)];
                return $"🔑 PASSWORD TIP:\n\n{tip}\n\nType 'tell me more' for another password tip!";
            }

            if (Contains(input, "phishing"))
            {
                _lastTopic = "phishing";
                string tip = _phishingTips[_random.Next(_phishingTips.Count)];
                return $"🎣 PHISHING TIP:\n\n{tip}\n\nType 'tell me more' for another phishing tip!";
            }

            if (Contains(input, "browsing", "website"))
            {
                _lastTopic = "browsing";
                return "🌐 SAFE BROWSING TIPS:\n\n" +
                       "• Always look for HTTPS and the padlock icon\n" +
                       "• Don't do banking on public WiFi — use mobile data\n" +
                       "• Keep your browser updated\n" +
                       "• Ignore popups saying your device is infected — they're scams\n" +
                       "• Don't click links from people you don't know";
            }

            if (Contains(input, "malware", "virus"))
            {
                _lastTopic = "malware";
                return "🦠 MALWARE PROTECTION:\n\n" +
                       "• Install a good antivirus like Malwarebytes\n" +
                       "• Never download software from untrusted websites\n" +
                       "• Always keep your device updated\n" +
                       "• Back up your important files regularly\n" +
                       "• Common types: Virus, Ransomware, Spyware, Trojan";
            }

            if (Contains(input, "banking", "bank"))
            {
                _lastTopic = "banking";
                return "🏦 BANKING SAFETY:\n\n" +
                       "• Only use your bank's official app or website\n" +
                       "• Your bank will NEVER ask for your PIN via SMS or email\n" +
                       "• Turn on transaction notifications\n\n" +
                       "SA Bank Fraud Lines:\n" +
                       "  ABSA: 0800 111 155\n" +
                       "  FNB: 087 575 9444\n" +
                       "  Standard Bank: 0800 020 600\n" +
                       "  Nedbank: 0800 110 929";
            }

            if (Contains(input, "social media", "facebook", "instagram"))
            {
                _lastTopic = "social media";
                return "📱 SOCIAL MEDIA SAFETY:\n\n" +
                       "• Set your profile to private\n" +
                       "• Never post your ID number or home address online\n" +
                       "• Be careful of friend requests from strangers\n" +
                       "• Think before you post — what goes online stays online\n" +
                       "• Romance scams and fake lottery wins are very common in SA";
            }

            if (Contains(input, "2fa", "two factor", "authentication"))
            {
                _lastTopic = "2fa";
                return "🔐 TWO-FACTOR AUTHENTICATION:\n\n" +
                       "• This adds an extra layer of security beyond your password\n" +
                       "• Use Google Authenticator instead of SMS\n" +
                       "• Enable 2FA on email, banking and social media\n" +
                       "• NEVER share your OTP with anyone — not even your bank\n" +
                       "• If someone asks for your OTP — it's a scam!";
            }

            if (Contains(input, "social engineering", "manipulation", "scam"))
            {
                _lastTopic = "social engineering";
                return "🎭 SOCIAL ENGINEERING:\n\n" +
                       "• Criminals trick you into giving them your information\n" +
                       "• Phone scams — criminals call pretending to be your bank\n" +
                       "• Fake SMS — messages stealing your personal details\n" +
                       "• Always verify who you're talking to before sharing anything\n" +
                       "• When in doubt — hang up and call the official number";
            }

            // MEMORY RECALL
            string userInterest = Recall("interest");
            if (userInterest != null && input.Contains(userInterest))
                return $"As someone interested in {userInterest}, here's something important: always stay updated on the latest threats. Check reputable sources like https://www.cybersecurity.co.za regularly! 🔒";

            // UNKNOWN INPUT
            return "__UNKNOWN__";
        }
    }
}
