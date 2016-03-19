using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Remove\Data\BlankLinesAtBottom.cs", "Data")]
    [DeploymentItem(@"Cleaning\Remove\Data\BlankLinesAtBottom_Cleaned.cs", "Data")]
    public class BlankLinesAtBottomTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\BlankLinesAtBottom.cs");
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
        public void CleaningRemoveBlankLinesAtBottom_CleansAsExpected()
        {
            Settings.Default.Cleaning_RemoveBlankLinesAtBottom = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunRemoveBlankLinesAtBottom, _projectItem, @"Data\BlankLinesAtBottom_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveBlankLinesAtBottom_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_RemoveBlankLinesAtBottom = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunRemoveBlankLinesAtBottom, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveBlankLinesAtBottom_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_RemoveBlankLinesAtBottom = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunRemoveBlankLinesAtBottom, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunRemoveBlankLinesAtBottom(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            _removeWhitespaceLogic.RemoveBlankLinesAtBottom(textDocument);
        }

        #endregion Helpers
    }
}