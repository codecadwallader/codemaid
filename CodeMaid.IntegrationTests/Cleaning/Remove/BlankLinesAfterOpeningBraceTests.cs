using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Remove\Data\BlankLinesAfterOpeningBrace.cs", "Data")]
    [DeploymentItem(@"Cleaning\Remove\Data\BlankLinesAfterOpeningBrace_Cleaned.cs", "Data")]
    public class BlankLinesAfterOpeningBraceTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\BlankLinesAfterOpeningBrace.cs");
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
        public void CleaningRemoveBlankLinesAfterOpeningBrace_CleansAsExpected()
        {
            Settings.Default.Cleaning_RemoveBlankLinesAfterOpeningBrace = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunRemoveBlankLinesAfterOpeningBrace, _projectItem, @"Data\BlankLinesAfterOpeningBrace_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveBlankLinesAfterOpeningBrace_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_RemoveBlankLinesAfterOpeningBrace = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunRemoveBlankLinesAfterOpeningBrace, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveBlankLinesAfterOpeningBrace_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_RemoveBlankLinesAfterOpeningBrace = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunRemoveBlankLinesAfterOpeningBrace, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunRemoveBlankLinesAfterOpeningBrace(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            _removeWhitespaceLogic.RemoveBlankLinesAfterOpeningBrace(textDocument);
        }

        #endregion Helpers
    }
}