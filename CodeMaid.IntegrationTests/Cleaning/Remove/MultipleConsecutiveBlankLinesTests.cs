using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Remove\Data\MultipleConsecutiveBlankLines.cs", "Data")]
    [DeploymentItem(@"Cleaning\Remove\Data\MultipleConsecutiveBlankLines_Cleaned.cs", "Data")]
    public class MultipleConsecutiveBlankLinesTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\MultipleConsecutiveBlankLines.cs");
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
        public void CleaningRemoveMultipleConsecutiveBlankLines_CleansAsExpected()
        {
            Settings.Default.Cleaning_RemoveMultipleConsecutiveBlankLines = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunRemoveMultipleConsecutiveBlankLines, _projectItem, @"Data\MultipleConsecutiveBlankLines_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveMultipleConsecutiveBlankLines_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_RemoveMultipleConsecutiveBlankLines = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunRemoveMultipleConsecutiveBlankLines, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveMultipleConsecutiveBlankLines_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_RemoveMultipleConsecutiveBlankLines = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunRemoveMultipleConsecutiveBlankLines, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private void RunRemoveMultipleConsecutiveBlankLines(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            _removeWhitespaceLogic.RemoveMultipleConsecutiveBlankLines(textDocument);
        }

        #endregion Helpers
    }
}