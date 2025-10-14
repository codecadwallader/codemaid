using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace CodeMaidShared.Logic.Cleaning
{
    internal class InsertNodePaddingMiddleware : IRoslynNodeMiddleware
    {
        public InsertNodePaddingMiddleware() { }

        private bool ShouldAddPadding { get; set; }
        private bool IsFirstNode { get; set; }
        private Func<SyntaxNode, SyntaxNode, SyntaxNode> Next { get; set; }

        public static RoslynCleaner Initialize(RoslynCleaner cleanup)
        {
            cleanup.AddNodeMiddleware(new InsertNodePaddingMiddleware());
            return cleanup;
        }

        public SyntaxNode Invoke(SyntaxNode original, SyntaxNode newNode)
        {
            var shouldAddPadding = ShouldAddPadding;
            var isFirst = IsFirstNode;

            ShouldAddPadding = false;
            IsFirstNode = true;

            newNode = Next(original, newNode);

            (newNode, ShouldAddPadding) =  RoslynInsertPaddingLogic.TryAddPadding(original, newNode, shouldAddPadding, isFirst);

            // Have to ignore inheritance/type/attribute nodes until the first member node.
            IsFirstNode = isFirst ? newNode is TypeParameterListSyntax or AttributeArgumentListSyntax or BaseListSyntax or TypeParameterConstraintClauseSyntax : false;
            return newNode;
        }

        public void SetNodeDelegate(Func<SyntaxNode, SyntaxNode, SyntaxNode> next)
        {
            Next = next;
        }
    }
}