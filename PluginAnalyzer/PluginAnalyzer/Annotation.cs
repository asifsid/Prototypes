namespace PluginAnalyzer
{
    using ICSharpCode.Decompiler.CSharp.Syntax;
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;

    public enum AnnotationLevel
    {
        Error, Warning
    }

    public record Annotation
    {
        public required AnalyzerRule Rule { get; init; }
        public required AnnotationLevel Level { get; init; }
        public AstNode Node { get; init; }
        public required Comment Comment { get; init; }
    }

    public class AnnotationSummary : IEnumerable<Annotation>
    {
        private const string Marker = "#~~";
        private readonly IList<Annotation> _summary = new List<Annotation>();

        

        public void Add(AnalyzerRule rule, AnnotationLevel level, AstNode source)
        {
            var annotation = new Annotation
            {
                Rule = rule,
                Level = level,
                Node = source,
                Comment = new Comment($" *** Analyzer {level} #{rule.Name} : {rule.Rule} {Marker}", CommentType.SingleLine),
            };

            source.Parent.InsertChildBefore(source, annotation.Comment, Roles.Comment);
            source.AddAnnotation(annotation);

            _summary.Add(annotation);
        }

        public static bool HasAnnotationMarker(string line)
        {
            return line.EndsWith(Marker);
        }

        public static string RemoveMarker(string line)
        {
            return line.Replace(Marker, "");
        }

        IEnumerator<Annotation> IEnumerable<Annotation>.GetEnumerator() => _summary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _summary.GetEnumerator();
        
    }
}
