namespace StripHtml
{
    using System.IO;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            string html = File.ReadAllText("Raw.txt");

            Console.WriteLine(html);

            var text = HtmlToText.Convert(html);

            Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine(text);
            Console.Write(text.Substring()
            Console.ReadKey();
        }
    }
}
