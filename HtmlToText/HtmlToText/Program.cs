namespace HtmlToText
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            string html = File.ReadAllText("Raw.txt");

            //Console.WriteLine(html);

            var text = HtmlConverter.ToPlainText(html);
            Console.WriteLine(text.Replace('\r','\n'));
            
            Console.ReadKey();
        }
    }
}
