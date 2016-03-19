using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Remove\Data\BlankLinesBetweenChainedStatements.cs", "Data")]
    [DeploymentItem(@"Cleaning\Remove\Data\BlankLinesBetweenChainedStatements_Cleaned.cs", "Data")]
    public class BlankLinesBetweenChainedStatementsTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\BlankLinesBetweenChainedStatements.cs");
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
        public void CleaningRemoveBlankLinesBetweenChainedStatements_CleansAsExpected()
        {
            Settings.Default.Cleaning_RemoveBlankLinesBetweenChainedStatements = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunRemoveBlankLinesBetweenChainedStatements, _projectItem, @"Data\BlankLinesBetweenChainedStatements_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveBlankLinesBetweenChainedStatements_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_RemoveBlankLinesBetweenChainedStatements = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunRemoveBlankLinesBetweenChainedStatements, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveBlankLinesBetweenChainedStatements_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_RemoveBlankLinesBetweenChainedStatements = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunRemoveBlankLinesBetweenChainedStatements, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunRemoveBlankLinesBetweenChainedStatements(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            _removeWhitespaceLogic.RemoveBlankLinesBetweenChainedStatements(textDocument);
        }

        #endregion Helpers
    }
}