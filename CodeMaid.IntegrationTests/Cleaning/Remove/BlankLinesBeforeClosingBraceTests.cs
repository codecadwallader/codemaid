using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Remove\Data\BlankLinesBeforeClosingBrace.cs", "Data")]
    [DeploymentItem(@"Cleaning\Remove\Data\BlankLinesBeforeClosingBrace_Cleaned.cs", "Data")]
    public class BlankLinesBeforeClosingBraceTests
    {
        #region Setup

        private static RemoveWhitespaceLogic _removeWhitespaceLogic;
        private ProjectItem _projectItem;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _removeWhitespaceLogic = RemoveWhitespaceLogic.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_removeWhitespaceLogic);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\BlankLinesBeforeClosingBrace.cs");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            TestEnvironment.RemoveFromProject(_projectItem);
        }

        #endregion Setup

        #region Tests

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveBlankLinesBeforeClosingBrace_CleansAsExpected()
        {
            Settings.Default.Cleaning_RemoveBlankLinesBeforeClosingBrace = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunRemoveBlankLinesBeforeClosingBrace, _projectItem, @"Data\BlankLinesBeforeClosingBrace_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveBlankLinesBeforeClosingBrace_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_RemoveBlankLinesBeforeClosingBrace = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunRemoveBlankLinesBeforeClosingBrace, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveBlankLinesBeforeClosingBrace_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_RemoveBlankLinesBeforeClosingBrace = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunRemoveBlankLinesBeforeClosingBrace, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunRemoveBlankLinesBeforeClosingBrace(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            _removeWhitespaceLogic.RemoveBlankLinesBeforeClosingBrace(textDocument);
        }

        #endregion Helpers
    }
}