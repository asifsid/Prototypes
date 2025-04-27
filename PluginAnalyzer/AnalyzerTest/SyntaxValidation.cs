namespace AnalyzerTest
{
    using PluginAnalyzer;
    using PluginAnalyzer.Rules;
    using System.Diagnostics;

    public class SyntaxValidation : AnalyzerTestBase
    {
        [Fact]
        public void Run()
        {
            this.Analyzer.Analyze(SyntaxtValidationRules.All);

            var annotations = this.Analyzer.Annotations;

            foreach (var annotation in annotations)
            {
                Debug.Print($"{annotation.Node.GetDescriptor()}, {annotation.Rule.Name}" );
            }
        }
    }
}