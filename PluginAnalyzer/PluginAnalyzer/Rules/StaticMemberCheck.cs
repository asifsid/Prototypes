namespace PluginAnalyzer.Rules
{
    using ICSharpCode.Decompiler.CSharp;
    using ICSharpCode.Decompiler.CSharp.Syntax;
    using ICSharpCode.Decompiler.TypeSystem;
    using Microsoft.Xrm.Sdk;

    class StaticMemberCheck : TypedAnalyzerRule<FieldDeclaration>
    {
        public override string Rule => "Do not use static member of type IOrganizationService";

        protected override void Analyze(FieldDeclaration node, AnnotationSummary annotations)
        {
            if (node.HasModifier(Modifiers.Static) && node.ReturnType.IsType(typeof(IOrganizationService)))
            {
                annotations.Add(this, AnnotationLevel.Error, node);
            }
        }
    }
}
