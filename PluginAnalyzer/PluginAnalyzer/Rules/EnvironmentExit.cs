namespace PluginAnalyzer.Rules
{
    using ICSharpCode.Decompiler.CSharp.Syntax;
    using ICSharpCode.Decompiler.Semantics;
    using ICSharpCode.Decompiler.TypeSystem;
    using System.Linq;

    class EnvironmentExit : TypedAnalyzerRule<Expression>
    {
        public override string Rule => "Do not use Environment.Exit methods in plugin.";

        protected override void Analyze(Expression node, AnnotationSummary annotations)
        {
            if (node is InvocationExpression invExp && invExp.Target is MemberReferenceExpression memberRefExp)
            {
                if (memberRefExp.Target is TypeReferenceExpression typeRefExp)
                {
                    if (typeRefExp.Type.IsSystemType(typeof(System.Environment)) && memberRefExp.MemberName == "Exit")
                    {
                        annotations.Add(this, AnnotationLevel.Error, node);
                    }
                }
            }
        }
    }
}
