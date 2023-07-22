using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
using System;

namespace CodeMaidShared.Logic.Cleaning
{
    internal class InsertExplicitAccessorMiddleware : IRoslynNodeMiddleware
    {
        private RoslynInsertExplicitAccessModifierLogic _insertAccess;
        public InsertExplicitAccessorMiddleware(SemanticModel semanticModel)
        {
            _insertAccess = new RoslynInsertExplicitAccessModifierLogic(semanticModel);
        }
        private Func<SyntaxNode, SyntaxNode, SyntaxNode> Next { get; set; }

        // Use this messy functions to ensure that the current node is not a descendant of an interface.
        // This is to mimic the recursive CSharpAddAccessibilityModifiersDiagnosticAnalyzer.ProcessMemberDeclaration
        // search where any non structs/classes are ignored.
        // FindAncestorOrSelf might help but would be slower.
        // Dont terminate on finding an interface in case I want to roslynize more cleanup functions.
        private bool InsideInterface { get; set; }

        public static RoslynCleaner Initialize(RoslynCleaner cleanup, SemanticModel model)
        {
            cleanup.AddNodeMiddleware(new InsertExplicitAccessorMiddleware(model));

            return cleanup;
        }

        public SyntaxNode Invoke(SyntaxNode original, SyntaxNode newNode)
        {
            if (original == null)
            {
                return original;
            }

            var inInterface = InsideInterface;

            if (original.IsKind(SyntaxKind.InterfaceDeclaration))
                InsideInterface = true;

            // Might have to account for namespaces here.
            newNode = Next(original, newNode);

            if (inInterface == false)
            {
                newNode = _insertAccess.TryAddExplicitModifier(original, newNode);
            }

            InsideInterface = inInterface;
            return newNode;
        }

        public void SetNodeDelegate(Func<SyntaxNode, SyntaxNode, SyntaxNode> next)
        {
            Next = next;
        }
    }
}