namespace AnalyzerUI.Data
{
    using ICSharpCode.Decompiler.CSharp.Syntax;
    using PluginAnalyzer;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Security.Policy;
    using System.Text;
    using static ICSharpCode.Decompiler.IL.Transforms.Stepper;

    internal class MainWindowViewModel
    {
        private readonly Analyzer _analyzer;

        public MainWindowViewModel(Analyzer analyzer)
        {
            _analyzer = analyzer;
        }

        public IEnumerable<SourceNode> SourceRoot { get { yield return new RootNode(_analyzer.Root); } }

        public IEnumerable<AnnotationItem> Annotations => _analyzer.Annotations.Select(a => new AnnotationItem(a));
    }


    internal abstract class SourceNode
    {
        public abstract AstNode Node { get; }

        public abstract string Name { get; }

        public string[] Lines => Node.ToString().Split(Environment.NewLine);

        public bool IsSelected { get; set; }

        public bool HasAnnotation => Lines.Any(line => AnnotationSummary.HasAnnotationMarker(line));
    }

    internal class RootNode : SourceNode
    {
        private readonly SyntaxTree _root;
        private readonly IList<NamespaceNode> _namespaces;

        public IList<NamespaceNode> Namespaces => _namespaces;

        public override AstNode Node => _root;

        public override string Name => _root.FileName;


        public RootNode(SyntaxTree root)
        {
            _root = root;
            _namespaces = _root.Children.OfType<NamespaceDeclaration>().Where(n => !n.Name.StartsWith("System.")).Select(n => new NamespaceNode(n)).ToList();
        }
    }

    internal class NamespaceNode : SourceNode
    {
        private NamespaceDeclaration _namespace;
        private readonly IList<TypeNode> _types;

        public override AstNode Node => _namespace;

        public override string Name => string.Join(".", _namespace.Identifiers);

        public IList<TypeNode> Types => _types;

        public NamespaceNode(NamespaceDeclaration node)
        {
            _namespace = node;
            _types = _namespace.Children.OfType<TypeDeclaration>().Select(n => new TypeNode(n)).ToList();
        }
    }

    internal class TypeNode : SourceNode
    {
        private readonly TypeDeclaration _type;

        public override AstNode Node => _type;

        public override string Name => _type.Name;

        public TypeNode(TypeDeclaration type)
        {
            _type = type;
        }
    }

    internal record AnnotationItem(Annotation Anotation)
    {
        public string Level => this.Anotation.Level.ToString();

        public string Rule => this.Anotation.Rule.Name;

        public string Type => $"{this.Anotation.Node.GetDescriptor().Namespace}.{this.Anotation.Node.GetDescriptor().TypeHierarchy}";

        public string TypeMember => this.Anotation.Node.GetDescriptor().TypeMember;

        public string Expression => this.Anotation.Node.GetDescriptor().Expression;

        public string Description => this.Anotation.Rule.Rule;
    }
}
