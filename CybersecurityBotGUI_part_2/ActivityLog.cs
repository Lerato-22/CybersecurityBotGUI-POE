using System;
using System.Collections.Generic;

namespace CybersecurityBotGUI_part_2
{
    public static class ActivityLog
    {
        private static List<string> _log = new List<string>();

        public static void Add(string action)
        {
            string entry = $"[{DateTime.Now:HH:mm:ss}] {action}";
            _log.Insert(0, entry); // newest first
        }

        public static List<string> GetRecent(int count = 10)
        {
            int take = Math.Min(count, _log.Count);
            return _log.GetRange(0, take);
        }

        public static List<string> GetAll()
        {
            return new List<string>(_log);
        }
    }
}
