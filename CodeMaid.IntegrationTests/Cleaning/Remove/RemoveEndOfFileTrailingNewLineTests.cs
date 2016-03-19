using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Remove\Data\RemoveEndOfFileTrailingNewLine.cs", "Data")]
    [DeploymentItem(@"Cleaning\Remove\Data\RemoveEndOfFileTrailingNewLine_Cleaned.cs", "Data")]
    public class RemoveEndOfFileTrailingNewLineTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\RemoveEndOfFileTrailingNewLine.cs");
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
        public void CleaningRemoveEndOfFileTrailingNewLine_CleansAsExpected()
        {
            Settings.Default.Cleaning_RemoveEndOfFileTrailingNewLine = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunRemoveEndOfFileTrailingNewLine, _projectItem, @"Data\RemoveEndOfFileTrailingNewLine_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveEndOfFileTrailingNewLine_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_RemoveEndOfFileTrailingNewLine = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunRemoveEndOfFileTrailingNewLine, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveEndOfFileTrailingNewLine_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_RemoveEndOfFileTrailingNewLine = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunRemoveEndOfFileTrailingNewLine, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunRemoveEndOfFileTrailingNewLine(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            _removeWhitespaceLogic.RemoveEOFTrailingNewLine(textDocument);
        }

        #endregion Helpers
    }
}