namespace AnalyzerTest
{
    using PluginAnalyzer;

    public class ConsoleReadTest : AnalyzerTestBase
    {
        [Fact]
        public void Run()
        {
            this.Analyzer.Analyze(AnalyzerRules.ConsoleRead);

            var annotations = this.Analyzer.Annotations;

            Assert.True(annotations.Count() == 3);
            Assert.True(annotations.All(a => a.Level == AnnotationLevel.Error && a.Rule == AnalyzerRules.ConsoleRead));
        }
    }
}