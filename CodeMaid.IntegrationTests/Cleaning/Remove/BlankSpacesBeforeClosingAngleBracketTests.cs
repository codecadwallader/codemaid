using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Remove\Data\BlankSpacesBeforeClosingAngleBracket.xml", "Data")]
    [DeploymentItem(@"Cleaning\Remove\Data\BlankSpacesBeforeClosingAngleBracket_Cleaned.xml", "Data")]
    public class BlankSpacesBeforeClosingAngleBracketTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\BlankSpacesBeforeClosingAngleBracket.xml");
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
        public void CleaningRemoveBlankSpacesBeforeClosingAngleBracket_CleansAsExpected()
        {
            Settings.Default.Cleaning_RemoveBlankSpacesBeforeClosingAngleBrackets = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunRemoveBlankSpacesBeforeClosingAngleBracket, _projectItem, @"Data\BlankSpacesBeforeClosingAngleBracket_Cleaned.xml");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveBlankSpacesBeforeClosingAngleBracket_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_RemoveBlankSpacesBeforeClosingAngleBrackets = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunRemoveBlankSpacesBeforeClosingAngleBracket, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveBlankSpacesBeforeClosingAngleBracket_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_RemoveBlankSpacesBeforeClosingAngleBrackets = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunRemoveBlankSpacesBeforeClosingAngleBracket, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunRemoveBlankSpacesBeforeClosingAngleBracket(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            _removeWhitespaceLogic.RemoveBlankSpacesBeforeClosingAngleBracket(textDocument);
        }

        #endregion Helpers
    }
}