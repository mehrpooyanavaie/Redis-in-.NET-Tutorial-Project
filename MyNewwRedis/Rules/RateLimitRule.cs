using System;
using System.Text.RegularExpressions;

namespace MyNewwRedis.Rules
{
    public class RateLimitRule
    {
        private static readonly Regex TimePattern = new Regex("([0-9]+(s|m|d|h))");

        private enum TimeUnit
        {
            s = 1,
            m = 60,
            h = 3600,
            d = 86400
        }

        public string Path { get; set; }
        public string PathRegex { get; set; }
        public string Window { get; set; }
        public int MaxRequests { get; set; }

        public int GetWindowInSeconds()
        {
            var match = TimePattern.Match(Window);
            if (string.IsNullOrEmpty(match.Value))
                throw new ArgumentException("format is not correct ");

            var unit = Enum.Parse<TimeUnit>(match.Value.Last().ToString());
            var num = int.Parse(match.Value.Substring(0, match.Value.Length - 1));
            return num * (int)unit;
        }
    }

}
