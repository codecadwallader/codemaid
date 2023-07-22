using Microsoft.CodeAnalysis;
using System;

namespace CodeMaidShared.Logic.Cleaning
{
    internal interface IRoslynTokenMiddleware
    {
        public SyntaxToken Invoke(SyntaxToken original, SyntaxToken newToken);

        // TODO this is messy, don't know how else to do it.
        public void SetTokenDelegate(Func<SyntaxToken, SyntaxToken, SyntaxToken> next);
    }
}
