using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.Logic.Cleaning
{
    internal static class RoslynExtensions
    {
        // TODO Refactor
        public static (List<(SyntaxKind, int)>, int) ReadTriviaLines(List<SyntaxTrivia> trivia)
        {
            var output = new List<(SyntaxKind, int)>();
            var lineType = SyntaxKind.WhitespaceTrivia;
            var position = 0;

            for (int i = 0; i < trivia.Count; i++)
            {
                var value = trivia[i];
                if (value.IsKind(SyntaxKind.EndOfLineTrivia))
                {
                    output.Add((lineType, position));
                    position = i + 1;
                    lineType = SyntaxKind.WhitespaceTrivia;
                }
                else if (value.IsKind(SyntaxKind.WhitespaceTrivia))
                {
                }
                else if (value.Kind() is SyntaxKind.SingleLineCommentTrivia or SyntaxKind.SingleLineDocumentationCommentTrivia)
                {
                    if (lineType == SyntaxKind.WhitespaceTrivia)
                    {
                        lineType = value.Kind();
                    }
                }
                else if (value.Kind() is SyntaxKind.RegionDirectiveTrivia or SyntaxKind.EndRegionDirectiveTrivia)
                {
                    if (lineType == SyntaxKind.WhitespaceTrivia)
                    {
                        lineType = value.Kind();
                        output.Add((lineType, position));
                        position = i + 1;
                        lineType = SyntaxKind.WhitespaceTrivia;
                    }
                }
                else
                {
                    lineType = SyntaxKind.BadDirectiveTrivia;
                }
            }

            return (output, position);
        }

        public static bool SpansMultipleLines(this SyntaxNode node)
        {
            var startLine = node.SyntaxTree.GetLineSpan(node.Span).StartLinePosition.Line;
            var endLine = node.SyntaxTree.GetLineSpan(node.Span).EndLinePosition.Line;

            return startLine != endLine;
        }

        // TODO use GetRequiredSemanticModelAsync
        public static async ValueTask<SemanticModel> GetRequiredSemanticModelAsync(this Document document, CancellationToken cancellationToken)
        {
            if (document.TryGetSemanticModel(out var semanticModel))
                return semanticModel;

            semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            return semanticModel ?? throw new InvalidOperationException($"Syntax tree is required to accomplish the task but is not supported by document {document.Name}");
        }

        public static SyntaxNode GetRequiredParent(this SyntaxNode node)
           => node.Parent ?? throw new InvalidOperationException("Node's parent was null");

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
    }

    internal static class AddAccessibilityModifiersHelpers
    {
        internal static Accessibility GetPreferredAccessibility(ISymbol symbol)
        {
            // If we have an overridden member, then if we're adding an accessibility modifier, use the
            // accessibility of the member we're overriding as both should be consistent here.
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
}