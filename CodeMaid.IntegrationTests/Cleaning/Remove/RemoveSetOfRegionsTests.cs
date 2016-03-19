using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using System.Linq;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Remove\Data\RemoveSetOfRegions.cs", "Data")]
    [DeploymentItem(@"Cleaning\Remove\Data\RemoveSetOfRegions_Cleaned.cs", "Data")]
    public class RemoveSetOfRegionsTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\RemoveSetOfRegions.cs");
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
        public void CleaningRemoveSetOfRegions_RunsAsExpected()
        {
            TestOperations.ExecuteCommandAndVerifyResults(RunRemoveSetOfRegions, _projectItem, @"Data\RemoveSetOfRegions_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveSetOfRegions_DoesNothingOnSecondPass()
        {
            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunRemoveSetOfRegions, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunRemoveSetOfRegions(Document document)
        {
            var codeItems = TestOperations.CodeModelManager.RetrieveAllCodeItems(document);
            var regions = codeItems.OfType<CodeItemRegion>().Where(x => x.Name == "In-Method region").ToList();

            _removeRegionLogic.RemoveRegions(regions);
        }

        #endregion Helpers
    }
}