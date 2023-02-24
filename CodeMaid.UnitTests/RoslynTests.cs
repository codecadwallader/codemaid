using CodeMaidShared.Logic.Cleaning;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.UnitTests;

internal class Rewriter : CSharpSyntaxRewriter
{
    internal Func<PropertyDeclarationSyntax, PropertyDeclarationSyntax, SyntaxNode> PropertyWriter { get; set; }
    internal Func<MethodDeclarationSyntax, MethodDeclarationSyntax, SyntaxNode> MethodWriter { get; set; }
    internal Func<ClassDeclarationSyntax, ClassDeclarationSyntax, SyntaxNode> ClassWriter { get; set; }

    public Rewriter()
    {
        PropertyWriter = (_, x) => x;
        MethodWriter = (_, x) => x;
        ClassWriter = (_, x) => x;
    }

    public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node)
    {
        var newNode = base.VisitPropertyDeclaration(node);
        return PropertyWriter(node, newNode as PropertyDeclarationSyntax);
    }

    public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        var newNode = base.VisitMethodDeclaration(node);
        return MethodWriter(node, newNode as MethodDeclarationSyntax);
    }

    public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        var newNode = base.VisitClassDeclaration(node);
        return ClassWriter(node, newNode as ClassDeclarationSyntax);
    }
}

[TestClass]
public class RoslynTests
{
    private readonly TestWorkspace testWorkspace;

    public RoslynTests()
    {
        testWorkspace = new TestWorkspace();
    }

    [TestMethod]
    public async Task ShouldAddPropertyAccessorAsync()
    {
        var source =
"""
public class Sample
{
    int Prop { get; set; }
}
""";

        var expected =
"""
public class Sample
{
    private int Prop { get; set; }
}
""";

        var document = testWorkspace.SetDocument(source);

        var syntaxTree = await document.GetSyntaxRootAsync();
        var syntaxGenerator = SyntaxGenerator.GetGenerator(document);
        var semanticModel = await document.GetSemanticModelAsync();

        var sut = new AddExplicitAccessModifierLogic(semanticModel, syntaxGenerator);
        var rewriter = new Rewriter() { PropertyWriter = sut.ProcessProperty };
        var result = rewriter.Visit(syntaxTree);

        result = Formatter.Format(result, SyntaxAnnotation.ElasticAnnotation, testWorkspace.Workspace);

        Console.WriteLine(result.ToFullString());
        var actual = result.ToFullString();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task ShouldAddPropertyAccessor2Async()
    {
        var source =
"""
public class Sample
{
    required int Prop { get; set; }
}
""";

        var expected =
"""
public class Sample
{
    private required int Prop { get; set; }
}
""";

        var document = testWorkspace.SetDocument(source);

        var syntaxTree = await document.GetSyntaxRootAsync();
        var syntaxGenerator = SyntaxGenerator.GetGenerator(document);
        var semanticModel = await document.GetSemanticModelAsync();

        var sut = new AddExplicitAccessModifierLogic(semanticModel, syntaxGenerator);
        var rewriter = new Rewriter() { PropertyWriter = sut.ProcessProperty };
        var result = rewriter.Visit(syntaxTree);

        result = Formatter.Format(result, SyntaxAnnotation.ElasticAnnotation, testWorkspace.Workspace);

        Console.WriteLine(result.ToFullString());
        var actual = result.ToFullString();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task ShouldAddPropertyAccessorMethodsAsync()
    {
        var source =
"""
public partial class ExampleClass
{
    void Do()
    {
    }
}
""";

        var expected =
"""
public partial class ExampleClass
{
    private void Do()
    {
    }
}
""";

        var document = testWorkspace.SetDocument(source);

        var syntaxTree = await document.GetSyntaxRootAsync();
        var syntaxGenerator = SyntaxGenerator.GetGenerator(document);
        var semanticModel = await document.GetSemanticModelAsync();

        var sut = new AddExplicitAccessModifierLogic(semanticModel, syntaxGenerator);
        var rewriter = new Rewriter() { PropertyWriter = sut.ProcessProperty, MethodWriter = sut.ProcessMethod };
        var result = rewriter.Visit(syntaxTree);

        result = Formatter.Format(result, SyntaxAnnotation.ElasticAnnotation, testWorkspace.Workspace);

        Console.WriteLine(result.ToFullString());
        var actual = result.ToFullString();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task ShouldAddPropertyAccessorClassAsync()
    {
        var source =
"""
public partial class Temp
{
}

partial class Temp
{
}
""";

        var expected =
"""
public partial class Temp
{
}

public partial class Temp
{
}
""";

        var document = testWorkspace.SetDocument(source);

        var syntaxTree = await document.GetSyntaxRootAsync();
        var syntaxGenerator = SyntaxGenerator.GetGenerator(document);
        var semanticModel = await document.GetSemanticModelAsync();

        var sut = new AddExplicitAccessModifierLogic(semanticModel, syntaxGenerator);
        var rewriter = new Rewriter()
        {
            PropertyWriter = sut.ProcessProperty,
            MethodWriter = sut.ProcessMethod,
            ClassWriter = sut.ProcessClass
        };
        var result = rewriter.Visit(syntaxTree);

        result = Formatter.Format(result, SyntaxAnnotation.ElasticAnnotation, testWorkspace.Workspace);

        Console.WriteLine(result.ToFullString());
        var actual = result.ToFullString();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task TestDepthAsync()
    {
        var source =
"""
class Temp
{
    int MyProperty { get; set; }
}
""";

        var expected =
"""
internal class Temp
{
    private int MyProperty { get; set; }
}
""";
        var document = testWorkspace.SetDocument(source);

        var syntaxTree = await document.GetSyntaxRootAsync();
        var syntaxGenerator = SyntaxGenerator.GetGenerator(document);
        var semanticModel = await document.GetSemanticModelAsync();

        var sut = new AddExplicitAccessModifierLogic(semanticModel, syntaxGenerator);
        var rewriter = new Rewriter()
        {
            PropertyWriter = sut.ProcessProperty,
            MethodWriter = sut.ProcessMethod,
            ClassWriter = sut.ProcessClass
        };
        var result = rewriter.Visit(syntaxTree);

        result = Formatter.Format(result, SyntaxAnnotation.ElasticAnnotation, testWorkspace.Workspace);

        Console.WriteLine(result.ToFullString());
        var actual = result.ToFullString();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task TestNestAsync()
    {
        var source =
"""
class Temp
{
    int MyProperty { get; set; }
}

public class Outer
{
    class Temp
    {
        int MyProperty { get; set; }
    }
}
""";

        var expected =
"""
internal class Temp
{
    private int MyProperty { get; set; }
}

public class Outer
{
    private class Temp
    {
        private int MyProperty { get; set; }
    }
}
""";
        var document = testWorkspace.SetDocument(source);

        var syntaxTree = await document.GetSyntaxRootAsync();
        var syntaxGenerator = SyntaxGenerator.GetGenerator(document);
        var semanticModel = await document.GetSemanticModelAsync();

        var sut = new AddExplicitAccessModifierLogic(semanticModel, syntaxGenerator);
        var rewriter = new Rewriter()
        {
            PropertyWriter = sut.ProcessProperty,
            MethodWriter = sut.ProcessMethod,
            ClassWriter = sut.ProcessClass
        };
        var result = rewriter.Visit(syntaxTree);

        result = Formatter.Format(result, SyntaxAnnotation.ElasticAnnotation, testWorkspace.Workspace);

        Console.WriteLine(result.ToFullString());
        var actual = result.ToFullString();
        Assert.AreEqual(expected, actual);
    }

    public class TestWorkspace
    {
        public TestWorkspace()
        {
            var source =
"""
public class Sample
{
}
""";

            Workspace = new AdhocWorkspace();

            var projName = "TestProject";
            var projectId = ProjectId.CreateNewId();
            var versionStamp = VersionStamp.Create();
            var projectInfo = ProjectInfo.Create(projectId, versionStamp, projName, projName, LanguageNames.CSharp);
            var newProject = Workspace.AddProject(projectInfo);

            var sourceText = SourceText.From(source);
            Document = Workspace.AddDocument(newProject.Id, "NewFile.cs", sourceText);
        }

        public AdhocWorkspace Workspace { get; private set; }

        public Document Document { get; private set; }

        public Document SetDocument(string text)
        {
            var newDocument = Document.WithText(SourceText.From(text));
            Workspace.TryApplyChanges(newDocument.Project.Solution);
            return newDocument;
        }
    }
}