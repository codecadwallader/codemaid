using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.VisualStudio.PlatformUI;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;
using System;

namespace CodeMaidShared.Logic.Cleaning;

/// <summary>
/// A class for encapsulating insertion of explicit access modifier logic.
/// </summary>
internal class RoslynInsertExplicitAccessModifierLogic
{
    #region Fields

    private readonly SemanticModel _semanticModel;
    private readonly SyntaxGenerator _syntaxGenerator;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// The singleton instance of the <see cref="RoslynInsertExplicitAccessModifierLogic" /> class.
    /// </summary>
    //private static RoslynInsertExplicitAccessModifierLogic _instance;

    ///// <summary>
    ///// Gets an instance of the <see cref="RoslynInsertExplicitAccessModifierLogic" /> class.
    ///// </summary>
    ///// <returns>An instance of the <see cref="RoslynInsertExplicitAccessModifierLogic" /> class.</returns>
    //internal static RoslynInsertExplicitAccessModifierLogic GetInstance(AsyncPackage package)
    //{
    //    return new RoslynInsertExplicitAccessModifierLogic(semanticModel, syntaxGenerator);
    //}

    /// <summary>
    /// Initializes a new instance of the <see cref="RoslynInsertExplicitAccessModifierLogic" /> class.
    /// </summary>
    public RoslynInsertExplicitAccessModifierLogic(SemanticModel semanticModel, SyntaxGenerator syntaxGenerator)
    {
        _semanticModel = semanticModel;
        _syntaxGenerator = syntaxGenerator;
    }

    #endregion Constructors

    public static RoslynCleanup Initialize(RoslynCleanup cleanup, SemanticModel model, SyntaxGenerator generator)
    {
        var explicitLogic = new RoslynInsertExplicitAccessModifierLogic(model, generator);
        cleanup.MemberWriter = explicitLogic.ProcessMember;
        return cleanup;
    }

    public SyntaxNode ProcessMember(SyntaxNode original, SyntaxNode node)
    {
        return node switch
        {
            DelegateDeclarationSyntax when Settings.Default.Cleaning_InsertExplicitAccessModifiersOnDelegates => AddAccessibility(original, node),
            EventFieldDeclarationSyntax when Settings.Default.Cleaning_InsertExplicitAccessModifiersOnEvents => AddAccessibility(original, node),
            EnumDeclarationSyntax when Settings.Default.Cleaning_InsertExplicitAccessModifiersOnEnumerations => AddAccessibility(original, node),
            FieldDeclarationSyntax when Settings.Default.Cleaning_InsertExplicitAccessModifiersOnFields => AddAccessibility(original, node),
            InterfaceDeclarationSyntax when Settings.Default.Cleaning_InsertExplicitAccessModifiersOnInterfaces => AddAccessibility(original, node),

            PropertyDeclarationSyntax when Settings.Default.Cleaning_InsertExplicitAccessModifiersOnProperties => AddAccessibility(original, node),
            MethodDeclarationSyntax when Settings.Default.Cleaning_InsertExplicitAccessModifiersOnMethods => AddAccessibility(original, node),

            ClassDeclarationSyntax when Settings.Default.Cleaning_InsertExplicitAccessModifiersOnClasses => AddAccessibility(original, node),
            StructDeclarationSyntax when Settings.Default.Cleaning_InsertExplicitAccessModifiersOnStructs => AddAccessibility(original, node),

            //RecordDeclarationSyntax when node.IsKind(SyntaxKind.RecordDeclaration) && Settings.Default.Cleaning_InsertExplicitAccessModifiersOnRecords => AddAccessibility(original, node),
            //RecordDeclarationSyntax when node.IsKind(SyntaxKind.RecordStructDeclaration) && Settings.Default.Cleaning_InsertExplicitAccessModifiersOnRecordStructs => AddAccessibility(original, node),

            _ => node,
        };
    }

    private SyntaxNode AddAccessibility(SyntaxNode original, SyntaxNode newNode)
    {
        if (!CSharpAccessibilityFacts.ShouldUpdateAccessibilityModifier(original as MemberDeclarationSyntax, AccessibilityModifiersRequired.Always, out var _, out var canChange))
        {
            return newNode;
        }

        var mapped = MapToDeclarator(original);

        var symbol = _semanticModel.GetDeclaredSymbol(mapped);
        if (symbol is null)
        {
            throw new ArgumentNullException(nameof(symbol));
        }

        var preferredAccessibility = AddAccessibilityModifiersHelpers.GetPreferredAccessibility(symbol);
        return InternalGenerator.WithAccessibility(newNode, preferredAccessibility);
        //return _syntaxGenerator.WithAccessibility(newNode, preferredAccessibility);
    }

    private static SyntaxNode MapToDeclarator(SyntaxNode node)
    {
        return node switch
        {
            FieldDeclarationSyntax field => field.Declaration.Variables[0],
            EventFieldDeclarationSyntax eventField => eventField.Declaration.Variables[0],
            _ => node,
        };
    }
}