namespace PluginAnalyzer.Rules
{
    using ICSharpCode.Decompiler.CSharp.Syntax;
    using ICSharpCode.Decompiler.Semantics;
    using ICSharpCode.Decompiler.TypeSystem;
    using System;
    using System.Linq;


    public class SyntaxtValidationRules
    {
        public static readonly AnalyzerRule[] All = new AnalyzerRule[]
        {
            new TopLevel(), new TypeLevel(), new FieldRule(), new EventdRule(),
        };

        class TopLevel : TypedAnalyzerRule<SyntaxTree>
        {
            public override string Rule => "Top level message";

            protected override void Analyze(SyntaxTree node, AnnotationSummary annotations)
            {
                annotations.Add(this, AnnotationLevel.Error, node.FirstChild);
            }
        }

        private static bool Filter(AstNode node)
        {
            return node.GetParent<NamespaceDeclaration>().Name == "AnalyzerTestSource.SyntaxValidation";
        }

        class TypeLevel : TypedAnalyzerRule<TypeDeclaration>
        {
            public override string Rule => "Type marked";

            protected override void Analyze(TypeDeclaration node, AnnotationSummary annotations)
            {
                if (Filter(node))
                { 
                    annotations.Add(this, AnnotationLevel.Error, node);
                }
            }
        }

        class FieldRule : TypedAnalyzerRule<FieldDeclaration>
        {
            public override string Rule => "Busted field";

            protected override void Analyze(FieldDeclaration node, AnnotationSummary annotations)
            {
                if (Filter(node))
                {
                    annotations.Add(this, AnnotationLevel.Error, node);
                }
            }
        }

        class EventdRule : TypedAnalyzerRule<EventDeclaration>
        {
            public override string Rule => "Busted event";

            protected override void Analyze(EventDeclaration node, AnnotationSummary annotations)
            {
                if (Filter(node))
                {
                    annotations.Add(this, AnnotationLevel.Error, node);
                }
            }
        }

        class ExpressionRule : TypedAnalyzerRule<InvocationExpression>
        {
            public override string Rule => "Busted invocation";

            protected override void Analyze(InvocationExpression node, AnnotationSummary annotations)
            {
                if (Filter(node))
                {
                    annotations.Add(this, AnnotationLevel.Error, node);
                }
            }
        }
    }


}
