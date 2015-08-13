using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace ParallaxEditorExtension
{
    public class OutlineViewModel
    {
        private string _bondPath;

        public OutlineViewModel(string docPath, string content)
        {
            Regex schemaPattern = new Regex(@"^;\@\s*schema\s*=\s*(\S+)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            var match = schemaPattern.Match(content);
            if (!match.Success)
            {
                return;
            }

            var schemaPath = match.Groups[1].Value;
            //if (!System.IO.File.Exists(schemaPath))
            //{
            //    return;
            //}

            this.SchemaPath = schemaPath;
            this.SelectSchema = Command.Create(OnSelectSchema);
        }

        public string SchemaPath { get; set; }

        public ICommand SelectSchema { get; }

        private void OnSelectSchema()
        {
            var dlg = new OpenFileDialog { Filter = "(Bond Files)|*.bond", Title = "Select Bind Schema file" };
            if (dlg.ShowDialog() ?? false)
            {
                _bondPath = dlg.FileName;
            }
        }
    }
}
