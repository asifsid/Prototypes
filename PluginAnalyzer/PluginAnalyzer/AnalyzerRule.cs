namespace PluginAnalyzer
{
    using ICSharpCode.Decompiler.CSharp.Syntax;
    using ICSharpCode.Decompiler.TypeSystem;
    using System.Collections.Generic;
    using System.Threading.Tasks;


    public abstract class AnalyzerRule
    {
        public string Name => GetType().Name;

        public abstract string Rule { get; }

        public abstract void Analyze(AstNode node, AnnotationSummary annotations);
    }

    public abstract class TypedAnalyzerRule<T> : AnalyzerRule
        where T : AstNode
    {
        public override void Analyze(AstNode node, AnnotationSummary annotations)
        {
            if (node is T typedNode)
            {
                Analyze(typedNode, annotations);
            }
        }

        protected abstract void Analyze(T node, AnnotationSummary annotations);
    }
}
