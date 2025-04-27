namespace PluginAnalyzer
{
    using ICSharpCode.Decompiler.CSharp.Syntax;
    using ICSharpCode.Decompiler.Metadata;
    using ICSharpCode.Decompiler.Semantics;
    using ICSharpCode.Decompiler.TypeSystem;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.PluginTelemetry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class AnalyzerExtensions
    {
        public static bool IsType(this AstType type, Type targetType)
        {
            if (type.Annotation<TypeResolveResult>()?.Type is ITypeDefinition typeDef)
            {
                return typeDef.FullName == targetType.FullName && typeDef.ParentModule.Name == targetType.Module.Assembly.GetName().Name;
            }
            return false;
        }

        public static bool IsSystemType(this AstType type, Type targetType)
        {
            if (type.Annotation<TypeResolveResult>()?.Type is ITypeDefinition typeDef)
            {
                return typeDef.FullName == targetType.FullName && typeDef.ParentModule.Name == "mscorlib";
            }
            return false;
        }

        public record NodeDescriptor(string Namespace, string TopLevelType, string TypeHierarchy, string TypeMember, string Expression)
        {
            public static NodeDescriptor Empty = new NodeDescriptor("", "", "", "", "");
        }

        public static NodeDescriptor GetDescriptor(this AstNode node)
        {
            var descriptor = node.Annotation<NodeDescriptor>();
            if (descriptor == null)
            {
                descriptor = GetDescriptor(node, NodeDescriptor.Empty);
                node.AddAnnotation(descriptor);
            }

            return descriptor;
        }

        public static NodeDescriptor GetDescriptor(AstNode node, NodeDescriptor descriptor)
        {
            switch (node)
            {
                case SyntaxTree:
                case UsingAliasDeclaration:
                case UsingDeclaration:
                    return descriptor;
                case NamespaceDeclaration ns:
                    return descriptor with { Namespace = ns.Name };
                case TypeDeclaration type:
                    string hierarchy = type.Name;
                    while (type.Parent is TypeDeclaration parent)
                    {
                        hierarchy = parent.Name + "+" + hierarchy;
                        type = parent;
                    }
                    return GetDescriptor(type.Parent, descriptor with { TopLevelType = type.Name, TypeHierarchy = hierarchy });
                case DelegateDeclaration @delegate:
                    if (@delegate.Parent is TypeDeclaration)
                        return GetDescriptor(node.Parent, descriptor with { TypeMember = "Delegate: " + @delegate.Name });
                    else
                        return GetDescriptor(node.Parent, descriptor with { TopLevelType = "Delegate: " + @delegate.Name });
                case FieldDeclaration field: 
                    return GetDescriptor(node.Parent, descriptor with { TypeMember = "Field: " + field.GetChildByRole(Roles.Variable).Name });
                case EventDeclaration @event: 
                    return GetDescriptor(node.Parent, descriptor with { TypeMember = "Event:" + @event.GetChildByRole(Roles.Variable).Name });
                case CustomEventDeclaration @event:
                    return GetDescriptor(node.Parent, descriptor with { TypeMember = "Event:" + @event.Name });
                case PropertyDeclaration property:
                    return GetDescriptor(node.Parent, descriptor with { TypeMember = "Property:" + property.Name });
                case MethodDeclaration method:
                    return GetDescriptor(node.Parent, descriptor with { TypeMember = "Method:" + method.Name });
                case ConstructorDeclaration constructor:
                    return GetDescriptor(node.Parent, descriptor with { TypeMember = "Ctor:" + constructor.Name });
                case EntityDeclaration entity:
                    return GetDescriptor(node.Parent, descriptor);
                case Expression expression:
                    return GetDescriptor(node.Parent, descriptor.Expression == "" ? descriptor with { Expression = expression.ToString() } : descriptor);
                default:
                    return GetDescriptor(node.Parent, descriptor);
            }
        }
    }
}
