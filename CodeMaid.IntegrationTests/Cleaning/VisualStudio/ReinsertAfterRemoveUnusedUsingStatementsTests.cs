using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.VisualStudio
{
    [TestClass]
    [DeploymentItem(@"Cleaning\VisualStudio\Data\ReinsertAfterRemoveUnusedUsingStatements.cs", "Data")]
    [DeploymentItem(@"Cleaning\VisualStudio\Data\ReinsertAfterRemoveUnusedUsingStatements_Cleaned.cs", "Data")]
    public class ReinsertAfterRemoveUnusedUsingStatementsTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\ReinsertAfterRemoveUnusedUsingStatements.cs");
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
        public void CleaningVisualStudioReinsertAfterRemoveUnusedUsingStatements_RunsAsExpected()
        {
            Settings.Default.Cleaning_RunVisualStudioRemoveUnusedUsingStatements = true;
            Settings.Default.Cleaning_UsingStatementsToReinsertWhenRemovedExpression = "using System;||using System.Linq;";

            TestOperations.ExecuteCommandAndVerifyResults(RunRemoveUnusedUsingStatements, _projectItem, @"Data\ReinsertAfterRemoveUnusedUsingStatements_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningVisualStudioReinsertAfterRemoveUnusedUsingStatements_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_RunVisualStudioRemoveUnusedUsingStatements = false;
            Settings.Default.Cleaning_UsingStatementsToReinsertWhenRemovedExpression = "using System;||using System.Linq;";

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunRemoveUnusedUsingStatements, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunRemoveUnusedUsingStatements(Document document)
        {
            _usingStatementCleanupLogic.RemoveUnusedUsingStatements(document.GetTextDocument());
        }

        #endregion Helpers
    }
}