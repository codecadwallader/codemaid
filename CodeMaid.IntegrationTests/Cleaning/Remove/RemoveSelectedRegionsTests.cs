using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Remove\Data\RemoveSelectedRegions.cs", "Data")]
    [DeploymentItem(@"Cleaning\Remove\Data\RemoveSelectedRegions_Cleaned.cs", "Data")]
    public class RemoveSelectedRegionsTests
    {
        #region Setup

        private static RemoveRegionLogic _removeRegionLogic;
        private ProjectItem _projectItem;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _removeRegionLogic = RemoveRegionLogic.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_removeRegionLogic);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\RemoveSelectedRegions.cs");
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
        public void CleaningRemoveSelectedRegions_RunsAsExpected()
        {
            TestOperations.ExecuteCommandAndVerifyResults(RunRemoveSelectedRegions, _projectItem, @"Data\RemoveSelectedRegions_Cleaned.cs");
        }

        #endregion Tests

        #region Helpers

        private static void RunRemoveSelectedRegions(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            textDocument.Selection.MoveToLineAndOffset(14, 9);
            textDocument.Selection.MoveToLineAndOffset(45, 5, true);

            _removeRegionLogic.RemoveRegions(textDocument.Selection);
        }

        #endregion Helpers
    }
}