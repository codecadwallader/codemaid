using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;
using System;

namespace CodeMaidShared.Logic.Cleaning;

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

    ///// <summary>
    ///// Gets an instance of the <see cref="AddExplicitAccessModifierLogic" /> class.
    ///// </summary>
    ///// <returns>An instance of the <see cref="AddExplicitAccessModifierLogic" /> class.</returns>
    //internal static AddExplicitAccessModifierLogic GetInstance(AsyncPackage package)
    //{
    //    return new AddExplicitAccessModifierLogic(semanticModel, syntaxGenerator);
    //}

    /// <summary>
    /// Initializes a new instance of the <see cref="AddExplicitAccessModifierLogic" /> class.
    /// </summary>
    public AddExplicitAccessModifierLogic(SemanticModel semanticModel, SyntaxGenerator syntaxGenerator)
    {
        _semanticModel = semanticModel;
        _syntaxGenerator = syntaxGenerator;
    }

    #endregion Constructors

    public static RoslynCleanup Initialize(RoslynCleanup cleanup, SemanticModel model, SyntaxGenerator generator)
    {
        var explicitLogic = new AddExplicitAccessModifierLogic(model, generator);
        cleanup.MemberWriter = explicitLogic.ProcessMember;
        return cleanup;
    }

    public SyntaxNode ProcessMember(SyntaxNode original, SyntaxNode node)
    {
        return node switch
        {
            ClassDeclarationSyntax when Settings.Default.Cleaning_InsertExplicitAccessModifiersOnClasses => AddAccessibility(original, node),
            PropertyDeclarationSyntax when Settings.Default.Cleaning_InsertExplicitAccessModifiersOnProperties => AddAccessibility(original, node),
            MethodDeclarationSyntax when Settings.Default.Cleaning_InsertExplicitAccessModifiersOnMethods => AddAccessibility(original, node),
            StructDeclarationSyntax when Settings.Default.Cleaning_InsertExplicitAccessModifiersOnStructs => AddAccessibility(original, node),
            _ => node,
        };
    }

    private SyntaxNode AddAccessibility(SyntaxNode original, SyntaxNode newNode)
    {
        var symbol = _semanticModel.GetDeclaredSymbol(original);

        if (symbol is null)
        {
            throw new ArgumentNullException(nameof(symbol));
        }

        if (!CSharpAccessibilityFacts.ShouldUpdateAccessibilityModifier(original as MemberDeclarationSyntax, AccessibilityModifiersRequired.Always, out var _, out var canChange) || !canChange)
        {
            return newNode;
        }

        var preferredAccessibility = AddAccessibilityModifiersHelpers.GetPreferredAccessibility(symbol);

        return _syntaxGenerator.WithAccessibility(newNode, preferredAccessibility);
    }
}