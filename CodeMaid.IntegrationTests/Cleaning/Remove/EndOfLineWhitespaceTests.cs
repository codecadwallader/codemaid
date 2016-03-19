using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Remove\Data\EndOfLineWhitespace.cs", "Data")]
    [DeploymentItem(@"Cleaning\Remove\Data\EndOfLineWhitespace_Cleaned.cs", "Data")]
    public class EndOfLineWhitespaceTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\EndOfLineWhitespace.cs");
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
        public void CleaningRemoveEndOfLineWhitespace_CleansAsExpected()
        {
            Settings.Default.Cleaning_RemoveEndOfLineWhitespace = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunRemoveEndOfLineWhitespace, _projectItem, @"Data\EndOfLineWhitespace_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveEndOfLineWhitespace_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_RemoveEndOfLineWhitespace = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunRemoveEndOfLineWhitespace, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveEndOfLineWhitespace_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_RemoveEndOfLineWhitespace = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunRemoveEndOfLineWhitespace, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunRemoveEndOfLineWhitespace(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            _removeWhitespaceLogic.RemoveEOLWhitespace(textDocument);
        }

        #endregion Helpers
    }
}