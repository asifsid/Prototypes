using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace ParallaxEditorExtension
{
    public class OutlineViewModel : ViewModel
    {
        private string _schemaPath;

        public OutlineViewModel(string docPath, string content)
        {
            Regex schemaPattern = new Regex(@"^;\@\s*schema\s*=\s*(\S+)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            var match = schemaPattern.Match(content);
            if (!match.Success)
            {
                return;
            }

            var schemaPath = match.Groups[1].Value;
            
            var searchDir = Path.GetDirectoryName(docPath);
            var schemaDir = Path.GetDirectoryName(docPath);
            
            if (schemaPath.StartsWith("..."))
            {
                schemaPath = schemaPath.Substring(3);
                while (searchDir != null)
                {
                    if (File.Exists(Path.Combine(searchDir, schemaPath)))
                    {
                        break;
                    }
                    searchDir = Directory.GetParent(searchDir).FullName;
                }
            }
            schemaPath = Path.Combine(searchDir, schemaPath);

            this.SchemaPath = schemaPath;
            this.SelectSchema = Command.Create(OnSelectSchema);
        }

        public string SchemaPath
        {
            get { return _schemaPath; }
            set
            {
                _schemaPath = value;
                InvokePropertyChanged(nameof(SchemaPath));
            }
        }

        public ICommand SelectSchema { get; }

        private void OnSelectSchema()
        {
            var dlg = new OpenFileDialog { Filter = "(Bond Files)|*.bond", Title = "Select Bind Schema file" };
            if (dlg.ShowDialog() ?? false)
            {
                    
            }
        }
    }
}
