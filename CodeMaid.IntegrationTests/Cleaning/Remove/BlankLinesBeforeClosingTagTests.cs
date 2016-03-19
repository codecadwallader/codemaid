using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Remove\Data\BlankLinesBeforeClosingTag.xml", "Data")]
    [DeploymentItem(@"Cleaning\Remove\Data\BlankLinesBeforeClosingTag_Cleaned.xml", "Data")]
    public class BlankLinesBeforeClosingTagTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\BlankLinesBeforeClosingTag.xml");
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
        public void CleaningRemoveBlankLinesBeforeClosingTag_CleansAsExpected()
        {
            Settings.Default.Cleaning_RemoveBlankLinesBeforeClosingTags = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunRemoveBlankLinesBeforeClosingTag, _projectItem, @"Data\BlankLinesBeforeClosingTag_Cleaned.xml");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveBlankLinesBeforeClosingTag_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_RemoveBlankLinesBeforeClosingTags = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunRemoveBlankLinesBeforeClosingTag, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveBlankLinesBeforeClosingTag_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_RemoveBlankLinesBeforeClosingTags = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunRemoveBlankLinesBeforeClosingTag, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunRemoveBlankLinesBeforeClosingTag(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            _removeWhitespaceLogic.RemoveBlankLinesBeforeClosingTag(textDocument);
        }

        #endregion Helpers
    }
}