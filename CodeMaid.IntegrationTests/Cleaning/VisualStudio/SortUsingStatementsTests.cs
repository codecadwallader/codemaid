using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.VisualStudio
{
    [TestClass]
    [DeploymentItem(@"Cleaning\VisualStudio\Data\SortUsingStatements.cs", "Data")]
    [DeploymentItem(@"Cleaning\VisualStudio\Data\SortUsingStatements_Cleaned.cs", "Data")]
    public class SortUsingStatementsTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\SortUsingStatements.cs");
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
        public void CleaningVisualStudioSortUsingStatements_RunsAsExpected()
        {
            Settings.Default.Cleaning_RunVisualStudioSortUsingStatements = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunSortUsingStatements, _projectItem, @"Data\SortUsingStatements_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningVisualStudioSortUsingStatements_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_RunVisualStudioSortUsingStatements = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunSortUsingStatements, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningVisualStudioSortUsingStatements_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_RunVisualStudioSortUsingStatements = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunSortUsingStatements, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunSortUsingStatements(Document document)
        {
            _usingStatementCleanupLogic.SortUsingStatements();
        }

        #endregion Helpers
    }
}