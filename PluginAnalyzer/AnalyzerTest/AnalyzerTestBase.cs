namespace AnalyzerTest
{
    using PluginAnalyzer;
    using System.Diagnostics;

    public abstract class AnalyzerTestBase
    {
        private Analyzer _analyzer;

        protected Analyzer Analyzer => _analyzer;

        public AnalyzerTestBase()
        {
            _analyzer = new Analyzer("AnalyzerTestSource.dll");
        }
    }
}
