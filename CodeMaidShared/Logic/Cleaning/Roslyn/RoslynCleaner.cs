using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.VisualStudio.Shell;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using System;

namespace CodeMaidShared.Logic.Cleaning
{
    internal class RoslynCleaner : CSharpSyntaxRewriter
    {
        public RoslynCleaner()
        {
            UpdateNodePipeline = (x, _) => base.Visit(x);
            UpdateTokenPipeline = (x, _) => base.VisitToken(x);
        }

        private Func<SyntaxNode, SyntaxNode, SyntaxNode> UpdateNodePipeline { get; set; }
        private Func<SyntaxToken, SyntaxToken, SyntaxToken> UpdateTokenPipeline { get; set; }

        public override SyntaxNode Visit(SyntaxNode original)
        {
            if (original == null)
            {
                return original;
            }
            var newNode = original;

            return UpdateNodePipeline(original, newNode);
        }

        public override SyntaxToken VisitToken(SyntaxToken token)
        {
            if (token == null)
            {
                return token;
            }
            var newToken = token;

            return UpdateTokenPipeline(token, newToken);
        }

        public SyntaxNode Process(SyntaxNode root, Workspace workspace)
        {
            var rewrite = Visit(root);
            return rewrite;

            //return Formatter.Format(rewrite, SyntaxAnnotation.ElasticAnnotation, workspace);
        }

        public void AddNodeMiddleware(IRoslynNodeMiddleware middleware)
        {
            middleware.SetNodeDelegate(UpdateNodePipeline);

            UpdateNodePipeline = middleware.Invoke;
        }

        public void AddTokenMiddleware(IRoslynTokenMiddleware middleware)
        {
            middleware.SetTokenDelegate(UpdateTokenPipeline);

            UpdateTokenPipeline = middleware.Invoke;
        }
    }
}