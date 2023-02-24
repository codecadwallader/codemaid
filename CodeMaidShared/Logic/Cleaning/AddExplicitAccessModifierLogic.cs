using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.VisualStudio.Shell;
using SteveCadwallader.CodeMaid;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace CodeMaidShared.Logic.Cleaning
{

    internal class RoslynRewriter : CSharpSyntaxRewriter
    {
        internal Func<PropertyDeclarationSyntax, SyntaxNode> PropertyWriter { get; set; }
        internal Func<MethodDeclarationSyntax, SyntaxNode> MethodWriter { get; set; }
        internal Func<ClassDeclarationSyntax, SyntaxNode> ClassWriter { get; set; }

        public RoslynRewriter()
        {
            PropertyWriter = x => x;
            MethodWriter = x => x;
            ClassWriter = x => x;
        }

        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            return PropertyWriter(node);
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            return MethodWriter(node);
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            return ClassWriter(node);
        }
    }

    /// <summary>
    /// A class for encapsulating insertion of explicit access modifier logic.
    /// </summary>
    internal class AddExplicitAccessModifierLogic
    {
        #region Fields
        private readonly SemanticModel _semanticModel;
        private readonly SyntaxGenerator _syntaxGenerator;
        #endregion Fields


        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="AddExplicitAccessModifierLogic" /> class.
        /// </summary>
        //private static AddExplicitAccessModifierLogic _instance;

        /// <summary>
        /// Gets an instance of the <see cref="AddExplicitAccessModifierLogic" /> class.
        /// </summary>
        /// <returns>An instance of the <see cref="AddExplicitAccessModifierLogic" /> class.</returns>
        internal static AddExplicitAccessModifierLogic GetInstance(AsyncPackage package)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            Global.Package = package;

            var document = Global.GetActiveDocument();

            if (document != null && document.TryGetSyntaxRoot(out SyntaxNode root))
            {
                var syntaxGenerator = SyntaxGenerator.GetGenerator(document);
                var semanticModel = document.GetSemanticModelAsync().Result;

                return new AddExplicitAccessModifierLogic(semanticModel, syntaxGenerator);

                root = Formatter.Format(root, SyntaxAnnotation.ElasticAnnotation, Global.Workspace);
                
                document = document.WithSyntaxRoot(root);
                Global.Workspace.TryApplyChanges(document.Project.Solution);
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddExplicitAccessModifierLogic" /> class.
        /// </summary>
        public AddExplicitAccessModifierLogic(SemanticModel semanticModel, SyntaxGenerator syntaxGenerator)
        {
            _semanticModel = semanticModel;
            _syntaxGenerator = syntaxGenerator;
        }

        #endregion Constructors

        public static void Process(AsyncPackage package)
        {
            var mod = GetInstance(package);

            var document = Global.GetActiveDocument();

            if (document != null && document.TryGetSyntaxRoot(out SyntaxNode root))
            {
                var rewriter = new RoslynRewriter() {  };
                var result = rewriter.Visit(root);

                root = Formatter.Format(result, SyntaxAnnotation.ElasticAnnotation, Global.Workspace);

                document = document.WithSyntaxRoot(root);
                Global.Workspace.TryApplyChanges(document.Project.Solution);
            }
            throw new InvalidOperationException();

        }

        public SyntaxNode ProcessMember(SyntaxNode original, SyntaxNode node)
        {
            return node switch
            {
                ClassDeclarationSyntax when Settings.Default.Cleaning_InsertExplicitAccessModifiersOnClasses => GenericApplyAccessibility(original, node),
                PropertyDeclarationSyntax when Settings.Default.Cleaning_InsertExplicitAccessModifiersOnProperties=> GenericApplyAccessibility(original, node),
                MethodDeclarationSyntax when Settings.Default.Cleaning_InsertExplicitAccessModifiersOnMethods=> GenericApplyAccessibility(original, node),
                StructDeclarationSyntax when Settings.Default.Cleaning_InsertExplicitAccessModifiersOnStructs=> GenericApplyAccessibility(original, node),
                _ => node,
            };
        }

        public SyntaxNode ProcessProperty(PropertyDeclarationSyntax original, PropertyDeclarationSyntax node)
        {
            if (!Settings.Default.Cleaning_InsertExplicitAccessModifiersOnProperties) return node;
            return GenericApplyAccessibility(original, node);
        }

        public SyntaxNode ProcessClass(ClassDeclarationSyntax original, ClassDeclarationSyntax node)
        {
            if (!Settings.Default.Cleaning_InsertExplicitAccessModifiersOnClasses) return node;
            return GenericApplyAccessibility(original, node);
        }

        public SyntaxNode ProcessMethod(MethodDeclarationSyntax original, MethodDeclarationSyntax node)
        {
            if (!Settings.Default.Cleaning_InsertExplicitAccessModifiersOnMethods) return node;
            return GenericApplyAccessibility(original, node);
        }

        private SyntaxNode GenericApplyAccessibility(SyntaxNode original, SyntaxNode newNode)
        {
            var symbol = _semanticModel.GetDeclaredSymbol(original);

            if (symbol is null)
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            if (!AccessibilityHelper.ShouldUpdateAccessibilityModifier(original as MemberDeclarationSyntax, AccessibilityModifiersRequired.Always, out var accessibility, out var canChange) || !canChange)
            {
                return newNode;
            }

            var preferredAccessibility = AddAccessibilityModifiersHelpers.GetPreferredAccessibility(symbol);

            return UpdateAccessibility(newNode, preferredAccessibility);

            SyntaxNode UpdateAccessibility(SyntaxNode declaration, Accessibility preferredAccessibility)
            {
                // If there was accessibility on the member, then remove it.  If there was no accessibility, then add
                // the preferred accessibility for this member.
                var newNode = _syntaxGenerator.GetAccessibility(declaration) == Accessibility.NotApplicable
                    ? _syntaxGenerator.WithAccessibility(declaration, preferredAccessibility)
                    : _syntaxGenerator.WithAccessibility(declaration, Accessibility.NotApplicable);

                return newNode;
            }
        }

        private SyntaxNode GenericApplyAccessibility(MemberDeclarationSyntax node)
        {
            var symbol = _semanticModel.GetDeclaredSymbol(node);

            if (symbol is null)
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            if (!AccessibilityHelper.ShouldUpdateAccessibilityModifier(node, AccessibilityModifiersRequired.Always, out var accessibility, out var canChange) || !canChange)
            {
                return node;
            }

            var preferredAccessibility = AddAccessibilityModifiersHelpers.GetPreferredAccessibility(symbol);

            return UpdateAccessibility(node, preferredAccessibility);

            SyntaxNode UpdateAccessibility(SyntaxNode declaration, Accessibility preferredAccessibility)
            {
                // If there was accessibility on the member, then remove it.  If there was no accessibility, then add
                // the preferred accessibility for this member.
                var newNode = _syntaxGenerator.GetAccessibility(declaration) == Accessibility.NotApplicable
                    ? _syntaxGenerator.WithAccessibility(declaration, preferredAccessibility)
                    : _syntaxGenerator.WithAccessibility(declaration, Accessibility.NotApplicable);

                return newNode;
            }
        }
    }

    internal static class DocumentExtensions
    {
        public static async ValueTask<SemanticModel> GetRequiredSemanticModelAsync(this Document document, CancellationToken cancellationToken)
        {
            if (document.TryGetSemanticModel(out var semanticModel))
                return semanticModel;

            semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            return semanticModel ?? throw new InvalidOperationException($"Syntax tree is required to accomplish the task but is not supported by document {document.Name}");
        }
    }

    internal static partial class AddAccessibilityModifiersHelpers
    {
        public static void UpdateDeclaration(
            SyntaxEditor editor, ISymbol symbol, SyntaxNode declaration)
        {
            if (symbol is null)
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            var preferredAccessibility = GetPreferredAccessibility(symbol);

            // Check to see if we need to add or remove
            // If there's a modifier, then we need to remove it, otherwise no modifier, add it.
            editor.ReplaceNode(
                declaration,
                (currentDeclaration, _) => UpdateAccessibility(currentDeclaration, preferredAccessibility));

            return;

            SyntaxNode UpdateAccessibility(SyntaxNode declaration, Accessibility preferredAccessibility)
            {
                var generator = editor.Generator;

                // If there was accessibility on the member, then remove it.  If there was no accessibility, then add
                // the preferred accessibility for this member.
                return generator.GetAccessibility(declaration) == Accessibility.NotApplicable
                    ? generator.WithAccessibility(declaration, preferredAccessibility)
                    : generator.WithAccessibility(declaration, Accessibility.NotApplicable);
            }
        }

        internal static Accessibility GetPreferredAccessibility(ISymbol symbol)
        {
            // If we have an overridden member, then if we're adding an accessibility modifier, use the
            // accessibility of the member we're overriding as both should be consistent here.
            // TODO Check override
            if (symbol.GetOverriddenMember() is { DeclaredAccessibility: var accessibility })
                return accessibility;

            // Default abstract members to be protected, and virtual members to be public.  They can't be private as
            // that's not legal.  And these are reasonable default values for them.
            if (symbol is IMethodSymbol or IPropertySymbol or IEventSymbol)
            {
                if (symbol.IsAbstract)
                    return Accessibility.Protected;

                if (symbol.IsVirtual)
                    return Accessibility.Public;
            }

            // Otherwise, default to whatever accessibility no-accessibility means for this member;
            return symbol.DeclaredAccessibility;
        }

        public static ISymbol? GetOverriddenMember(this ISymbol? symbol)
            => symbol switch
            {
                IMethodSymbol method => method.OverriddenMethod,
                IPropertySymbol property => property.OverriddenProperty,
                IEventSymbol @event => @event.OverriddenEvent,
                _ => null,
            };
    }

    internal enum AccessibilityModifiersRequired
    {
        // The rule is not run
        Never = 0,

        // Accessibility modifiers are added if missing, even if default
        Always = 1,

        // Future proofing for when C# adds default interface methods.  At that point
        // accessibility modifiers will be allowed in interfaces, and some people may
        // want to require them, while some may want to keep the traditional C# style
        // that public interface members do not need accessibility modifiers.
        ForNonInterfaceMembers = 2,

        // Remove any accessibility modifier that matches the default
        OmitIfDefault = 3
    }

    internal static class AccessibilityHelper
    {
        public static bool ShouldUpdateAccessibilityModifier(
            MemberDeclarationSyntax member,
            AccessibilityModifiersRequired option,
            out Accessibility accessibility,
            out bool modifierAdded)
        {
            modifierAdded = false;
            accessibility = Accessibility.NotApplicable;

            // Have to have a name to report the issue on.
            var name = member.GetNameToken();
            if (name.IsKind(SyntaxKind.None))
                return false;

            // Certain members never have accessibility. Don't bother reporting on them.
            if (!CSharpAccessibilityFacts.CanHaveAccessibility(member))
                return false;

            // This analyzer bases all of its decisions on the accessibility
            accessibility = CSharpAccessibilityFacts.GetAccessibility(member);

            // Omit will flag any accessibility values that exist and are default
            // The other options will remove or ignore accessibility
            var isOmit = option == AccessibilityModifiersRequired.OmitIfDefault;
            modifierAdded = !isOmit;

            if (isOmit)
            {
                if (accessibility == Accessibility.NotApplicable)
                    return false;

                var parentKind = member.GetRequiredParent().Kind();
                switch (parentKind)
                {
                    // Check for default modifiers in namespace and outside of namespace
                    case SyntaxKind.CompilationUnit:
                    case SyntaxKind.FileScopedNamespaceDeclaration:
                    case SyntaxKind.NamespaceDeclaration:
                        {
                            // Default is internal
                            if (accessibility != Accessibility.Internal)
                                return false;
                        }

                        break;

                    case SyntaxKind.ClassDeclaration:
                    case SyntaxKind.RecordDeclaration:
                    case SyntaxKind.StructDeclaration:
                    case SyntaxKind.RecordStructDeclaration:
                        {
                            // Inside a type, default is private
                            if (accessibility != Accessibility.Private)
                                return false;
                        }

                        break;

                    default:
                        return false; // Unknown parent kind, don't do anything
                }
            }
            else
            {
                // Mode is always, so we have to flag missing modifiers
                if (accessibility != Accessibility.NotApplicable)
                    return false;
            }

            return true;
        }
    }

    internal static partial class MemberDeclarationSyntaxExtensions
    {
        private static readonly ConditionalWeakTable<MemberDeclarationSyntax, Dictionary<string, ImmutableArray<SyntaxToken>>> s_declarationCache = new();

        public static SyntaxToken GetNameToken(this MemberDeclarationSyntax member)
        {
            if (member != null)
            {
                switch (member.Kind())
                {
                    case SyntaxKind.EnumDeclaration:
                        return ((EnumDeclarationSyntax)member).Identifier;

                    case SyntaxKind.ClassDeclaration:
                    case SyntaxKind.RecordDeclaration:
                    case SyntaxKind.InterfaceDeclaration:
                    case SyntaxKind.StructDeclaration:
                    case SyntaxKind.RecordStructDeclaration:
                        return ((TypeDeclarationSyntax)member).Identifier;

                    case SyntaxKind.DelegateDeclaration:
                        return ((DelegateDeclarationSyntax)member).Identifier;

                    case SyntaxKind.FieldDeclaration:
                        return ((FieldDeclarationSyntax)member).Declaration.Variables.First().Identifier;

                    case SyntaxKind.EventFieldDeclaration:
                        return ((EventFieldDeclarationSyntax)member).Declaration.Variables.First().Identifier;

                    case SyntaxKind.PropertyDeclaration:
                        return ((PropertyDeclarationSyntax)member).Identifier;

                    case SyntaxKind.EventDeclaration:
                        return ((EventDeclarationSyntax)member).Identifier;

                    case SyntaxKind.MethodDeclaration:
                        return ((MethodDeclarationSyntax)member).Identifier;

                    case SyntaxKind.ConstructorDeclaration:
                        return ((ConstructorDeclarationSyntax)member).Identifier;

                    case SyntaxKind.DestructorDeclaration:
                        return ((DestructorDeclarationSyntax)member).Identifier;

                    case SyntaxKind.IndexerDeclaration:
                        return ((IndexerDeclarationSyntax)member).ThisKeyword;

                    case SyntaxKind.OperatorDeclaration:
                        return ((OperatorDeclarationSyntax)member).OperatorToken;
                }
            }

            // Conversion operators don't have names.
            return default;
        }

        public static int GetArity(this MemberDeclarationSyntax member)
        {
            if (member != null)
            {
                switch (member.Kind())
                {
                    case SyntaxKind.ClassDeclaration:
                    case SyntaxKind.RecordDeclaration:
                    case SyntaxKind.InterfaceDeclaration:
                    case SyntaxKind.StructDeclaration:
                    case SyntaxKind.RecordStructDeclaration:
                        return ((TypeDeclarationSyntax)member).Arity;

                    case SyntaxKind.DelegateDeclaration:
                        return ((DelegateDeclarationSyntax)member).Arity;

                    case SyntaxKind.MethodDeclaration:
                        return ((MethodDeclarationSyntax)member).Arity;
                }
            }

            return 0;
        }

        public static TypeParameterListSyntax GetTypeParameterList(this MemberDeclarationSyntax member)
        {
            if (member != null)
            {
                switch (member.Kind())
                {
                    case SyntaxKind.ClassDeclaration:
                    case SyntaxKind.RecordDeclaration:
                    case SyntaxKind.InterfaceDeclaration:
                    case SyntaxKind.StructDeclaration:
                    case SyntaxKind.RecordStructDeclaration:
                        return ((TypeDeclarationSyntax)member).TypeParameterList;

                    case SyntaxKind.DelegateDeclaration:
                        return ((DelegateDeclarationSyntax)member).TypeParameterList;

                    case SyntaxKind.MethodDeclaration:
                        return ((MethodDeclarationSyntax)member).TypeParameterList;
                }
            }

            return null;
        }

        public static MemberDeclarationSyntax WithParameterList(
            this MemberDeclarationSyntax member,
            BaseParameterListSyntax parameterList)
        {
            if (member != null)
            {
                switch (member.Kind())
                {
                    case SyntaxKind.DelegateDeclaration:
                        return ((DelegateDeclarationSyntax)member).WithParameterList((ParameterListSyntax)parameterList);

                    case SyntaxKind.MethodDeclaration:
                        return ((MethodDeclarationSyntax)member).WithParameterList((ParameterListSyntax)parameterList);

                    case SyntaxKind.ConstructorDeclaration:
                        return ((ConstructorDeclarationSyntax)member).WithParameterList((ParameterListSyntax)parameterList);

                    case SyntaxKind.IndexerDeclaration:
                        return ((IndexerDeclarationSyntax)member).WithParameterList((BracketedParameterListSyntax)parameterList);

                    case SyntaxKind.OperatorDeclaration:
                        return ((OperatorDeclarationSyntax)member).WithParameterList((ParameterListSyntax)parameterList);

                    case SyntaxKind.ConversionOperatorDeclaration:
                        return ((ConversionOperatorDeclarationSyntax)member).WithParameterList((ParameterListSyntax)parameterList);
                }
            }

            return null;
        }

        public static TypeSyntax GetMemberType(this MemberDeclarationSyntax member)
        {
            if (member != null)
            {
                switch (member.Kind())
                {
                    case SyntaxKind.DelegateDeclaration:
                        return ((DelegateDeclarationSyntax)member).ReturnType;

                    case SyntaxKind.MethodDeclaration:
                        return ((MethodDeclarationSyntax)member).ReturnType;

                    case SyntaxKind.OperatorDeclaration:
                        return ((OperatorDeclarationSyntax)member).ReturnType;

                    case SyntaxKind.PropertyDeclaration:
                        return ((PropertyDeclarationSyntax)member).Type;

                    case SyntaxKind.IndexerDeclaration:
                        return ((IndexerDeclarationSyntax)member).Type;

                    case SyntaxKind.EventDeclaration:
                        return ((EventDeclarationSyntax)member).Type;

                    case SyntaxKind.EventFieldDeclaration:
                        return ((EventFieldDeclarationSyntax)member).Declaration.Type;

                    case SyntaxKind.FieldDeclaration:
                        return ((FieldDeclarationSyntax)member).Declaration.Type;
                }
            }

            return null;
        }

        public static bool HasMethodShape(this MemberDeclarationSyntax memberDeclaration)
            => memberDeclaration is BaseMethodDeclarationSyntax;

        public static BlockSyntax GetBody(this MemberDeclarationSyntax memberDeclaration)
            => memberDeclaration switch
            {
                BaseMethodDeclarationSyntax method => method.Body,
                _ => null,
            };

        public static ArrowExpressionClauseSyntax GetExpressionBody(this MemberDeclarationSyntax memberDeclaration)
            => memberDeclaration switch
            {
                BaseMethodDeclarationSyntax method => method.ExpressionBody,
                IndexerDeclarationSyntax indexer => indexer.ExpressionBody,
                PropertyDeclarationSyntax property => property.ExpressionBody,
                _ => null,
            };

        public static MemberDeclarationSyntax WithBody(this MemberDeclarationSyntax memberDeclaration, BlockSyntax body)
            => (memberDeclaration as BaseMethodDeclarationSyntax)?.WithBody(body);
    }
}