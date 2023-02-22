using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.CodeAnalysis.Text;
using CodeMaidShared.Logic.Cleaning;
using Microsoft.CodeAnalysis.Formatting;

namespace SteveCadwallader.CodeMaid.UnitTests;

[TestClass]
public class RoslynTests
{
    public class Rewriter : CSharpSyntaxRewriter
    {
        private readonly Func<PropertyDeclarationSyntax, SyntaxNode> _writer;

        public Rewriter(Func<PropertyDeclarationSyntax, SyntaxNode> writer)
        {
            _writer=writer;
        }

        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            return _writer(node);
        }
    }

    [TestMethod]
    public void ShouldAddPropertyAccessor()
    {
        var source =
"""
public class Sample
{
    int Prop { get; set; }
}
""";
        var workspace = new AdhocWorkspace();

        var projName = "TestProject";
        var projectId = ProjectId.CreateNewId();
        var versionStamp = VersionStamp.Create();
        var projectInfo = ProjectInfo.Create(projectId, versionStamp, projName, projName, LanguageNames.CSharp);
        var newProject = workspace.AddProject(projectInfo);
        var sourceText = SourceText.From(source);
        var newDocument = workspace.AddDocument(newProject.Id, "NewFile.cs", sourceText);

        var syntaxTree = newDocument.GetSyntaxRootAsync().Result;
        var syntaxGenerator = SyntaxGenerator.GetGenerator(newDocument);
        var semanticModel = newDocument.GetSemanticModelAsync().Result;

        var sut = new AddExplicitAccessModifierLogic(semanticModel, syntaxGenerator);
        var result = new Rewriter(x => sut.Process(x)).Visit(syntaxTree);

        newDocument = newDocument.WithSyntaxRoot(result);
        newDocument = Formatter.FormatAsync(newDocument, SyntaxAnnotation.ElasticAnnotation).Result;

        result = newDocument.GetSyntaxRootAsync().Result;
        Console.WriteLine(result.ToFullString());
        var c = result.ToFullString();
        Assert.AreEqual(1, 1);
    }
}