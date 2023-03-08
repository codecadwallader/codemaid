using Microsoft.CodeAnalysis;
using System;

namespace CodeMaidShared.Logic.Cleaning
{
    internal interface IRoslynNodeMiddleware
    {
        public SyntaxNode Invoke(SyntaxNode original, SyntaxNode newNode);

        // TODO this is messy, don't know how else to do it.
        public void SetNodeDelegate(Func<SyntaxNode, SyntaxNode, SyntaxNode> next);
    }
}
