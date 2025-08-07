// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

namespace TestApp
{
    public static class Program
    {
        public static void Main()
        {
            string testStr = "This string contains some 1 to 10 numbers and that is 1, 2, 3 ... 9.";

            Console.WriteLine("Test replacements:");
            Console.WriteLine($"Original        : {testStr}");


            Console.WriteLine($"Replaced bad    : {RegexSubs.Replace(testStr)}");
            Test(() => RegexSubs.Replace(testStr));

            Console.WriteLine($"Replaced good   : {RegexSubs.ReplaceBetter(testStr)}");
            Test(() => RegexSubs.ReplaceBetter(testStr));

            Console.WriteLine($"Replaced better   : {RegexSubs.ReplaceEvenBetter(testStr)}");
            Test(() => RegexSubs.ReplaceEvenBetter(testStr));
        }

        static void Test(Action a)
        {
            var st = Stopwatch.StartNew();

            for (int i = 0; i < 100000; i++)
            {
                a();
            }

            st.Stop();

            Console.WriteLine($"{st.ElapsedMilliseconds} ms for 100000 executions");
        }

    }

}