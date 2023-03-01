using CodeMaidShared.Logic.Cleaning;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.UnitTests.Cleanup
{
    public class TestWorkspace
    {
        public async Task VerifyCleanupAsync(string input, string expected)
        {
            var document = SetDocument(input);

            var syntaxTree = await Document.GetSyntaxRootAsync();
            var syntaxGenerator = SyntaxGenerator.GetGenerator(document);
            var semanticModel = await Document.GetSemanticModelAsync();

            var modifierLogic = new RoslynInsertExplicitAccessModifierLogic(semanticModel, syntaxGenerator);
            var rewriter = new RoslynCleanup()
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
public class ThisShouldAppear
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