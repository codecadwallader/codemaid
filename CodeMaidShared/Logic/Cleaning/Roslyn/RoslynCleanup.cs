using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.VisualStudio.Shell;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using System;

namespace CodeMaidShared.Logic.Cleaning;

internal class RoslynCleanup : CSharpSyntaxRewriter
{
    public Func<SyntaxNode, SyntaxNode, SyntaxNode> MemberWriter { get; set; }

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

    public static void RunExplicit(AsyncPackage package)
    {
        ThreadHelper.ThrowIfNotOnUIThread();

        Global.Package = package;

        var document = Global.GetActiveDocument();

        if (document == null || !document.TryGetSyntaxRoot(out SyntaxNode root))
        {
            throw new InvalidOperationException();
        }

        var semanticModel = document.GetSemanticModelAsync().Result;
        var syntaxGenerator = SyntaxGenerator.GetGenerator(document);

        var cleaner = new RoslynCleanup();
        AddExplicitAccessModifierLogic.Initialize(cleaner, semanticModel, syntaxGenerator);
        cleaner.Process(root, Global.Workspace);

        document = document.WithSyntaxRoot(root);
        Global.Workspace.TryApplyChanges(document.Project.Solution);
    }
}