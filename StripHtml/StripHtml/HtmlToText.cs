namespace StripHtml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Text.RegularExpressions;

    public class HtmlToText
    {
        // The sections to be stripped out
        private static readonly string[] _sections =
            {
                "head",
                "script",
                "style",
            };

        // Tags to be replaced with corresponding control character 
        private static readonly Dictionary<string, string> _tags = new Dictionary<string, string>
            {
                { "td", "\t" },
                { "br", "\r" },
                { "li", "\r" },
                { "div", "\r\r" },
                { "tr", "\r\r" },
                { "p", "\r\r" },
                { "", "" }, // All other tags - removed
            };

        // Escape characters to be replaced with corresponding character
        private static readonly Dictionary<string, string> _escapes = new Dictionary<string, string>
            {
                { "nbsp", "\u00A0" }, // non-breaking whitespace
                { "amp", "&" },
                { "bull", " * " },
                { "lsaquo", "<" },
                { "rsaquo", ">" },
                { "trade", "(tm)" },
                { "frasl", "/" },
                { "lt", "<" },
                { "gt", ">" },
                { "copy", "(c)" },
                { "reg", "(r)" },
                { "{.{2,6}}", ""} // all other escapes removed
            };

        

        private const RegexOptions DefaultOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture;

        private static Regex _spacesExp = new Regex(@"(\r|\n|\t)+| {2,}", DefaultOptions);
        
        private static Regex _sectionsExp = new Regex(string.Join("|", 
            _sections.Select(s => $@"< *{s}[^>]*>.*<\/ *{s}[^>]*>")), DefaultOptions); // match begin and end tag and everything within

        private static Regex _tagsExp = new Regex(string.Join("|", 
            _tags.Keys.Select(t => $@"< *(?<tag>{t})[^>]*>")), DefaultOptions); // match tag with captured tag name (to be used for replacement lookup)
        
        private static Regex _escapesExp = new Regex(string.Join("|", 
            _escapes.Keys.Select(e => $@"&(?<esc>{e});")), DefaultOptions); // match escape and capture to for replacement lookup

        public static string Convert(string html)
        {
            html = _spacesExp.Replace(html, " "); // clean up spaces
            html = _sectionsExp.Replace(html, ""); // remove sections
            html = _tagsExp.Replace(html, match => _tags[match.Groups["tag"].Value]); // replace tags with loopup value
            html = _escapesExp.Replace(html, match => _escapes[match.Groups["esc"].Value]); // replace escape with lookup value

            return html;
        }
    }
}
