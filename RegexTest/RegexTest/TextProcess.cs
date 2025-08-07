namespace TestApp
{
    using System.Text;
    using System.Text.RegularExpressions;

    public static class RegexSubs
    {
        private static readonly Dictionary<string, string> _expressions = new()
            {
                {"10", "ten"},
                {"1", "one"},
                {"2", "two"},
                {"3", "three"},
                {"4", "four"},
                {"5", "five"},
                {"6", "six"},
                {"7", "seven"},
                {"8", "eight"},
                {"9", "nine"},
        }; 

        private static readonly Regex _pattern;

        static RegexSubs()
        {
            var index = 0;
            var expression = string.Join('|', _expressions.Select(i => $"(?<_{index++}>{i.Key})"));

            _pattern = new Regex(expression, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
        }


        public static string Replace(string input)
        {
            input = Regex.Replace(input, "10", "ten");
            input = Regex.Replace(input, "1", "one");
            input = Regex.Replace(input, "2", "two");
            input = Regex.Replace(input, "3", "three");
            input = Regex.Replace(input, "4", "four");
            input = Regex.Replace(input, "5", "five");
            input = Regex.Replace(input, "6", "six");
            input = Regex.Replace(input, "7", "seven");
            input = Regex.Replace(input, "8", "eight");
            input = Regex.Replace(input, "9", "nine");
            
            return input;
        }

        public static string ReplaceBetter(string input)
        {
            StringBuilder sb = new(input.Length);

            int index = 0;
            foreach (Match m in _pattern.Matches(input)) 
            {
                sb.Append(input[index..m.Index]);

                for (int i = 1; i < m.Groups.Count; i++)
                {
                    if (m.Groups[i].Length > 0)
                    {
                        sb.Append(_expressions[m.Groups[i].Value]);
                        break;
                    }
                }
                index = m.Index + m.Length;
            }

            sb.Append(input[index..(input.Length - 1)]);

            return sb.ToString();
        }


        public static string ReplaceEvenBetter(string input)
        {
            StringBuilder sb = new(input.Length);

            string replaced = _pattern.Replace(input, m => _expressions[m.Groups[0].Value]);
            return replaced;
        }
    }
}
