using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.VisualStudio
{
    [TestClass]
    [DeploymentItem(@"Cleaning\VisualStudio\Data\RemoveAndSortUsingStatements.cs", "Data")]
    [DeploymentItem(@"Cleaning\VisualStudio\Data\RemoveAndSortUsingStatements_Cleaned.cs", "Data")]
    public class RemoveAndSortUsingStatementsTests
    {
        #region Setup

        private static UsingStatementCleanupLogic _usingStatementCleanupLogic;
        private ProjectItem _projectItem;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _usingStatementCleanupLogic = UsingStatementCleanupLogic.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_usingStatementCleanupLogic);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\RemoveAndSortUsingStatements.cs");
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
        public void CleaningVisualStudioRemoveAndSortUsingStatements_RunsAsExpected()
        {
            Settings.Default.Cleaning_RunVisualStudioRemoveAndSortUsingStatements = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunRemoveAndSortUsingStatements, _projectItem, @"Data\RemoveAndSortUsingStatements_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningVisualStudioRemoveAndSortUsingStatements_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_RunVisualStudioRemoveAndSortUsingStatements = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunRemoveAndSortUsingStatements, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningVisualStudioRemoveAndSortUsingStatements_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_RunVisualStudioRemoveAndSortUsingStatements = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunRemoveAndSortUsingStatements, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunRemoveAndSortUsingStatements(Document document)
        {
            _usingStatementCleanupLogic.RemoveAndSortUsingStatements(document.GetTextDocument());
        }

        #endregion Helpers
    }
}