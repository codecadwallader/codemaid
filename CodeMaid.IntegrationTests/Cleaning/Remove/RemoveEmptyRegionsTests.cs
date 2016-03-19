using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using SteveCadwallader.CodeMaid.UI.Enumerations;
using System.Linq;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Remove\Data\RemoveEmptyRegions.cs", "Data")]
    [DeploymentItem(@"Cleaning\Remove\Data\RemoveEmptyRegions_Cleaned.cs", "Data")]
    public class RemoveEmptyRegionsTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\RemoveEmptyRegions.cs");
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
        public void CleaningRemoveEmptyRegions_RunsAsExpected()
        {
            Settings.Default.Cleaning_RemoveRegions = (int)NoneEmptyAll.Empty;

            TestOperations.ExecuteCommandAndVerifyResults(RunRemoveEmptyRegions, _projectItem, @"Data\RemoveEmptyRegions_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveEmptyRegions_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_RemoveRegions = (int)NoneEmptyAll.Empty;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunRemoveEmptyRegions, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveEmptyRegions_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_RemoveRegions = (int)NoneEmptyAll.None;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunRemoveEmptyRegions, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunRemoveEmptyRegions(Document document)
        {
            var codeItems = TestOperations.CodeModelManager.RetrieveAllCodeItems(document);
            var regions = codeItems.OfType<CodeItemRegion>().ToList();

            _removeRegionLogic.RemoveRegionsPerSettings(regions);
        }

        #endregion Helpers
    }
}