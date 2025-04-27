namespace AnalyzerTest
{
    using PluginAnalyzer;

    public class EnvironmentExitTest : AnalyzerTestBase
    {
        [Fact]
        public void Run()
        {
            this.Analyzer.Analyze(AnalyzerRules.EnvironmentExit);

            var annotations = this.Analyzer.Annotations;

            Assert.True(annotations.Count() == 1);
            Assert.True(annotations.All(a => a.Level == AnnotationLevel.Error && a.Rule == AnalyzerRules.EnvironmentExit));
        }
    }
}