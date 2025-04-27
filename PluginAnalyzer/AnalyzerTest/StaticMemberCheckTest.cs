namespace AnalyzerTest
{
    using PluginAnalyzer;

    public class StaticMemberCheckTest : AnalyzerTestBase
    {
        [Fact]
        public void Run()
        {
            this.Analyzer.Analyze(AnalyzerRules.StaticMemberCheck);

            var annotations = this.Analyzer.Annotations;

            Assert.True(annotations.Count() == 2);
            Assert.True(annotations.All(a => a.Level == AnnotationLevel.Error && a.Rule == AnalyzerRules.StaticMemberCheck));
        }
    }
}