namespace PluginAnalyzer.Rules
{
    using ICSharpCode.Decompiler.CSharp.Syntax;
    using ICSharpCode.Decompiler.Semantics;
    using ICSharpCode.Decompiler.TypeSystem;
    using System.Linq;

    class ConsoleRead : TypedAnalyzerRule<Expression>
    {
        public override string Rule => "Do not use Console.Read* methods in plugin.";

        protected override void Analyze(Expression node, AnnotationSummary annotations)
        {
            if (node is InvocationExpression invExp && invExp.Target is MemberReferenceExpression memberRefExp)
            {
                if (memberRefExp.Target is TypeReferenceExpression typeRefExp)
                {
                    if (typeRefExp.Type.IsSystemType(typeof(System.Console)) && memberRefExp.MemberName.StartsWith("Read"))
                    {
                        annotations.Add(this, AnnotationLevel.Error, node);
                    }
                }
            }
        }
    }
}
