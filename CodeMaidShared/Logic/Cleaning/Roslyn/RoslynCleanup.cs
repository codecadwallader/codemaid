using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using Microsoft.CodeAnalysis.Formatting;

namespace CodeMaidShared.Logic.Cleaning;

internal class RoslynCleanup : CSharpSyntaxRewriter
{
    internal Func<SyntaxNode, SyntaxNode, SyntaxNode> MemberWriter { get; set; }

    public RoslynCleanup()
    {
        MemberWriter = (_, x) => x;
    }

    public override SyntaxNode Visit(SyntaxNode node)
    {
        var newNode = base.Visit(node);
        newNode = MemberWriter(node, newNode);

        return newNode;
    }

    public SyntaxNode Process(SyntaxNode root, Workspace workspace)
    {
        var rewrite = Visit(root);
        return Formatter.Format(rewrite, SyntaxAnnotation.ElasticAnnotation, workspace);
    }
}