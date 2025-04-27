namespace StripHtml
{
    using BenchmarkDotNet.Attributes;
    using System.Text.RegularExpressions;

    public class BenchmarkTests
    {
        private string _stringToReplace = "This string contains some 1 to 10 numbers and that is 1, 2, 3 ... 9.";

        [Benchmark]
        public string Replace1()
        {
            return RemoveNums.Replace(_stringToReplace);
        }

        [Benchmark]
        public string Replace2()
        {
            return RemoveNums.ReplaceBetter(_stringToReplace);
        }
    }

    public class RemoveNums
    {
        // using this array to hold all of the patterns that need to be stripped out
        private static readonly string[] _expressions =
            {
                    "10",
                    "1",
                    "2",
                    "3",
                    "4",
                    "5",
                    "6",
                    "7",
                    "8",
                    "9"
                };


        // Creating a single regular expression with all of the patterns:  "10|1|2|3|4|5|6|7|8|9"
        // Using Compiled, IgnoreCase etc.
        private static readonly Regex _pattern = new Regex(string.Join("|", _expressions), RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);


        // Using the patterns one by one.
        // This will create 10 new copies of string
        public static string Replace(string input)
        {
            input = Regex.Replace(input, "10", "-");
            input = Regex.Replace(input, "1", "-");
            input = Regex.Replace(input, "2", "-");
            input = Regex.Replace(input, "3", "-");
            input = Regex.Replace(input, "4", "-");
            input = Regex.Replace(input, "5", "-");
            input = Regex.Replace(input, "6", "-");
            input = Regex.Replace(input, "7", "-");
            input = Regex.Replace(input, "8", "-");
            input = Regex.Replace(input, "9", "-");

            return input;
        }

        // This will only create 1 more copy of the string 
        // Also gets better execution time (will be dependent on complexity of the expressions of course)
        public static string ReplaceBetter(string input)
        {
            return _pattern.Replace(input, "-");
        }
    }
}
