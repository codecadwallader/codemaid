using CodeMaidShared.Logic.Cleaning;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading.Tasks;
using StringAssert = NUnit.Framework.StringAssert;

namespace SteveCadwallader.CodeMaid.UnitTests.Cleanup
{
    public class TestWorkspace
    {
        public async Task VerifyCleanupAsync(string input, string expected)
        {
            var document = SetDocument(input);

            var syntaxTree = await Document.GetSyntaxRootAsync();
            var semanticModel = await Document.GetSemanticModelAsync();

            var rewriter = new RoslynCleaner();
            InsertExplicitAccessorMiddleware.Initialize(rewriter, semanticModel);
            InsertNodePaddingMiddleware.Initialize(rewriter);

            InsertTokenPaddingMiddleware.Initialize(rewriter);

            var result = rewriter.Process(syntaxTree, Workspace);
            var resultString = result.ToFullString();

            //To support cross platform line endings use shouldly's IgnoreLineEndings option.
            // TODO: Add cross platform string compare and remove shoudly.
            resultString.ShouldBe(expected, StringCompareShould.IgnoreLineEndings);
            //NUnit.Framework.Assert.AreEqual(expected, resultString);
            //StringAssert.AreEqualIgnoringCase(expected, result.ToFullString());
        }

        public TestWorkspace()
        {
            var source =
@"
public class ThisShouldAppear
{
}
";

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
            NUnit.Framework.Assert.IsTrue(Workspace.TryApplyChanges(Document.Project.Solution));
            return Document;
        }
    }
}