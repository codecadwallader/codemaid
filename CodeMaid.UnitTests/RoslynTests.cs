using CodeMaidShared.Logic.Cleaning;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.UnitTests;

internal class Rewriter : CSharpSyntaxRewriter
{
    internal Func<SyntaxNode, SyntaxNode, SyntaxNode> MemberWriter { get; set; }

    public Rewriter()
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

        await testWorkspace.VerifyCleanupAsync(source, expected);
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

        await testWorkspace.VerifyCleanupAsync(source, expected);
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

        await testWorkspace.VerifyCleanupAsync(source, expected);
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

        await testWorkspace.VerifyCleanupAsync(source, expected);
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

        await testWorkspace.VerifyCleanupAsync(source, expected);
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
        await testWorkspace.VerifyCleanupAsync(source, expected);
    }

    public class TestWorkspace
    {
        public async Task VerifyCleanupAsync(string input, string expected)
        {
            var document = SetDocument(input);

            var syntaxTree = await Document.GetSyntaxRootAsync();
            var syntaxGenerator = SyntaxGenerator.GetGenerator(document);
            var semanticModel = await Document.GetSemanticModelAsync();

            var modifierLogic = new AddExplicitAccessModifierLogic(semanticModel, syntaxGenerator);
            var rewriter = new Rewriter()
            {
                MemberWriter = modifierLogic.ProcessMember
            };
            var result = rewriter.Process(syntaxTree, Workspace);

            Assert.AreEqual(expected, result.ToFullString());
        }

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
            Document = Document.WithText(SourceText.From(text));
            Assert.IsTrue(Workspace.TryApplyChanges(Document.Project.Solution));
            return Document;
        }
    }
}