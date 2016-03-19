using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Remove\Data\BlankLinesAtTop.cs", "Data")]
    [DeploymentItem(@"Cleaning\Remove\Data\BlankLinesAtTop_Cleaned.cs", "Data")]
    public class BlankLinesAtTopTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\BlankLinesAtTop.cs");
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
        public void CleaningRemoveBlankLinesAtTop_CleansAsExpected()
        {
            Settings.Default.Cleaning_RemoveBlankLinesAtTop = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunRemoveBlankLinesAtTop, _projectItem, @"Data\BlankLinesAtTop_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveBlankLinesAtTop_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_RemoveBlankLinesAtTop = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunRemoveBlankLinesAtTop, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveBlankLinesAtTop_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_RemoveBlankLinesAtTop = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunRemoveBlankLinesAtTop, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunRemoveBlankLinesAtTop(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            _removeWhitespaceLogic.RemoveBlankLinesAtTop(textDocument);
        }

        #endregion Helpers
    }
}