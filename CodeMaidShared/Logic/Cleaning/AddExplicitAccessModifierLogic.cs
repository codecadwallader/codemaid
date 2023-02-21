using EnvDTE;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Elfie.Model;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.VisualStudio.Shell;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace SteveCadwallader.CodeMaid.Logic.Cleaning
{
    /// <summary>
    /// A class for encapsulating insertion of explicit access modifier logic.
    /// </summary>
    internal static class AddExplicitAccessModifierLogic
    {
        //#region Constants

        //private const string PartialKeyword = "partial";

        //#endregion Constants

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="AddExplicitAccessModifierLogic" /> class.
        /// </summary>
        //private static AddExplicitAccessModifierLogic _instance;

        /// <summary>
        /// Gets an instance of the <see cref="AddExplicitAccessModifierLogic" /> class.
        /// </summary>
        /// <returns>An instance of the <see cref="AddExplicitAccessModifierLogic" /> class.</returns>
        //internal static AddExplicitAccessModifierLogic GetInstance()
        //{
        //    return _instance ?? (_instance = new AddExplicitAccessModifierLogic());
        //}

        ///// <summary>
        ///// Initializes a new instance of the <see cref="AddExplicitAccessModifierLogic" /> class.
        ///// </summary>
        //private AddExplicitAccessModifierLogic()
        //{
        //}

        #endregion Constructors

        public static void InsertExplicitMemberModifiers(CodeMaidPackage package)
        {
            Start(package);
        }
        //int MyProperty { get; set; }
        //public int MyProperty2 { get; set; }
        //public required int MyProperty3 { get; set; }

        private static void Start(CodeMaidPackage package)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            Global.Package = package;

            var document = Global.GetActiveDocument();

            if (document != null && document.TryGetSyntaxRoot(out SyntaxNode root))
            {
                root = Process(root, document).Result;

                document = document.WithSyntaxRoot(root);

                Global.Workspace.TryApplyChanges(document.Project.Solution);
            }
        }

        public static async Task<SyntaxNode> Process(SyntaxNode root, Microsoft.CodeAnalysis.Document document)
        {
            if (!Settings.Default.Cleaning_InsertExplicitAccessModifiersOnMethods) return root;

            //var propertyNodes = root.(node => node.IsKind(SyntaxKind.PropertyDeclaration));
            var propertyNodes = root.DescendantNodes().OfType<PropertyDeclarationSyntax>();

            var semanticModel = await document.GetRequiredSemanticModelAsync(default).ConfigureAwait(false);

            if (propertyNodes.Any())
            {
                //root = root.ReplaceNodes(propertyNodes,
                //           (originalNode, newNode) =>
                //           {
                //               var symbol = semanticModel.GetDeclaredSymbol(originalNode, default);

                //               if (symbol is null)
                //               {
                //                   throw new ArgumentNullException(nameof(symbol));
                //               }

                //               var editor = new SyntaxEditor(originalNode, Global.Workspace.Services);

                //               AddAccessibilityModifiersHelpers.UpdateDeclaration(editor, symbol, originalNode);
                //               return originalNode;
                //           });

                //foreach (var node in propertyNodes)
                //{
                //    var symbol = semanticModel.GetDeclaredSymbol(node, default);

                //    if (symbol is null)
                //    {
                //        throw new ArgumentNullException(nameof(symbol));
                //    }
                //    var editor = new SyntaxEditor(node, Global.Workspace.Services);
                //    AddAccessibilityModifiersHelpers.UpdateDeclaration(editor, symbol, node);
                //}

                var editor = new SyntaxEditor(root, Global.Workspace.Services);
                root = root.ReplaceNodes(propertyNodes,
                                   (originalNode, newNode) =>
                                   {
                                       var symbol = semanticModel.GetDeclaredSymbol(originalNode);

                                       if (symbol is null)
                                       {
                                           throw new ArgumentNullException(nameof(symbol));
                                       }
                                       //AddAccessibilityModifiersHelpers.UpdateDeclaration(editor, symbol, node);

                                       var preferredAccessibility = AddAccessibilityModifiersHelpers.GetPreferredAccessibility(symbol);

                                       return UpdateAccessibility(originalNode, preferredAccessibility);

                                       SyntaxNode UpdateAccessibility(SyntaxNode declaration, Accessibility preferredAccessibility)
                                       {
                                           var generator = editor.Generator;

                                           // If there was accessibility on the member, then remove it.  If there was no accessibility, then add
                                           // the preferred accessibility for this member.
                                           var newNode = generator.GetAccessibility(declaration) == Accessibility.NotApplicable
                                               ? generator.WithAccessibility(declaration, preferredAccessibility)
                                               : generator.WithAccessibility(declaration, Accessibility.NotApplicable);

                                           return newNode;
                                       }
                                   });
            }

            return Formatter.Format(root, SyntaxAnnotation.ElasticAnnotation, Global.Workspace);
        }

        //public void FixAllAsync(
        //   Document document, ImmutableArray<Diagnostic> diagnostics,
        //   SyntaxEditor editor)
        //{
        //    var semanticModel = await document.GetRequiredSemanticModelAsync(cancellationToken).ConfigureAwait(false);

        //    foreach (var diagnostic in diagnostics)
        //    {
        //        var declaration = diagnostic.AdditionalLocations[0].FindNode(cancellationToken);
        //        var declarator = MapToDeclarator(declaration);
        //        var symbol = semanticModel.GetDeclaredSymbol(declarator, cancellationToken);
        //        Contract.ThrowIfNull(symbol);
        //        AddAccessibilityModifiersHelpers.UpdateDeclaration(editor, symbol, declaration);
        //    }
        //}
    }

    internal static class DocumentExtensions
    {
        public static async ValueTask<SemanticModel> GetRequiredSemanticModelAsync(this Microsoft.CodeAnalysis.Document document, CancellationToken cancellationToken)
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
            //if (symbol.GetOverriddenMember() is { DeclaredAccessibility: var accessibility })
            //    return accessibility;

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

        //    internal static Symbol GetOverriddenMember(Symbol substitutedOverridingMember, Symbol overriddenByDefinitionMember)
        //    {
        //        Debug.Assert(!substitutedOverridingMember.IsDefinition);

        //        if ((object)overriddenByDefinitionMember != null)
        //        {
        //            NamedTypeSymbol overriddenByDefinitionContaining = overriddenByDefinitionMember.ContainingType;
        //            NamedTypeSymbol overriddenByDefinitionContainingTypeDefinition = overriddenByDefinitionContaining.OriginalDefinition;
        //            for (NamedTypeSymbol baseType = substitutedOverridingMember.ContainingType.BaseTypeNoUseSiteDiagnostics;
        //                (object)baseType != null;
        //                baseType = baseType.BaseTypeNoUseSiteDiagnostics)
        //            {
        //                if (TypeSymbol.Equals(baseType.OriginalDefinition, overriddenByDefinitionContainingTypeDefinition, TypeCompareKind.ConsiderEverything2))
        //                {
        //                    if (TypeSymbol.Equals(baseType, overriddenByDefinitionContaining, TypeCompareKind.ConsiderEverything2))
        //                    {
        //                        return overriddenByDefinitionMember;
        //                    }

        //                    return overriddenByDefinitionMember.OriginalDefinition.SymbolAsMember(baseType);
        //                }
        //            }

        //            throw ExceptionUtilities.Unreachable();
        //        }

        //        return null;
        //    }
    }
}