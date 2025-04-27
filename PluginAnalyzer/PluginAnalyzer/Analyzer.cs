namespace PluginAnalyzer
{
    using ICSharpCode.Decompiler;
    using ICSharpCode.Decompiler.CSharp;
    using ICSharpCode.Decompiler.CSharp.Syntax;
    using ICSharpCode.Decompiler.Metadata;
    using ICSharpCode.Decompiler.TypeSystem;
    using PluginAnalyzer.Rules;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using static ICSharpCode.Decompiler.IL.Transforms.Stepper;

    public static class AnalyzerRules
    {
        public static readonly AnalyzerRule StaticMemberCheck = new StaticMemberCheck();
        public static readonly AnalyzerRule ConsoleRead = new ConsoleRead();
        public static readonly AnalyzerRule EnvironmentExit = new EnvironmentExit();

        public static readonly AnalyzerRule[] All = typeof(AnalyzerRules).GetFields(BindingFlags.Static | BindingFlags.Public).Where(f => f.FieldType == typeof(AnalyzerRule)).Select(f => (AnalyzerRule)f.GetValue(null)).ToArray();
    }

    public class Analyzer 
    {
        private readonly CSharpDecompiler _decompiler;
        private readonly SyntaxTree _root;
        private readonly AnnotationSummary _annotations = new AnnotationSummary();
    
        public SyntaxTree Root => _root;

        public AnnotationSummary Annotations => _annotations;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <exception cref="Exception"></exception>
        public Analyzer(string assembly)
        {
            var setting = new DecompilerSettings(LanguageVersion.CSharp7_2) 
            {
                ThrowOnAssemblyResolveErrors = false,
                RemoveDeadCode = true,
                RemoveDeadStores = true,
                AlwaysUseBraces = true,
            };

            try
            { 
                var module = new PEFile(assembly);
                _decompiler = new CSharpDecompiler(assembly,new UniversalAssemblyResolver(assembly, false, module.DetectTargetFrameworkId()), setting);
                
                _root = _decompiler.DecompileWholeModuleAsSingleFile();
                _root.FileName = Path.GetFileName(assembly);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to decompile plugin assembly: {assembly}", ex);
            }
        }

        /// <summary>
        /// Analyzes specified or All rules on the loaded assembly.
        /// </summary>
        /// <param name="rules">Use <see cref="AnalyzerRules"/> to specify specific rules to analyze. If ommitted, all rules are applied.</param>
        public void Analyze(params AnalyzerRule[] rules)
        {
            if (rules.Length == 0)
            {
                rules = AnalyzerRules.All;
            }

            // Run all root level rules
            Parallel.ForEach(rules, rule => rule.Analyze(_root, _annotations));

            foreach (var entity in _root.GetTypes())
            {
                // Run all type/delegate level rules
                Parallel.ForEach(rules, rule => rule.Analyze(entity, _annotations));

                if (entity is TypeDeclaration type)
                { 
                    Analyze(type, rules);
                }
            }
        }

        private void Analyze(TypeDeclaration type, AnalyzerRule[] rules)
        {
            foreach (var typeMember in type.GetChildrenByRole(Roles.TypeMemberRole))
            {
                // Run all type member level rules
                Parallel.ForEach(rules, rule => rule.Analyze(typeMember, _annotations));

                // All statement blocks
                foreach (var block in typeMember.DescendantNodes(n => n is not TypeDeclaration).OfType<BlockStatement>())
                {
                    foreach (var statement in block.Statements)
                    {
                        Parallel.ForEach(rules, rule => rule.Analyze(statement, _annotations));

                        foreach (var expression in statement.DescendantNodes(n => true).OfType<Expression>())
                        {
                            Parallel.ForEach(rules, rule => rule.Analyze(expression, _annotations));
                        }
                    }
                }

                // All non-statement block expressions
                foreach (var expression in typeMember.DescendantNodes(n => n is not TypeDeclaration and not BlockStatement).OfType<Expression>())
                {
                    Parallel.ForEach(rules, rule => rule.Analyze(expression, _annotations));
                }

                // Nested types
                if (typeMember is TypeDeclaration nestedType)
                {
                    Analyze(nestedType, rules);
                }
            }
        }
    }
}
