using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Reorganizing;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Reorganizing
{
    [TestClass]
    [DeploymentItem(@"Reorganizing\Data\RegionsInsertEvenIfEmptyWithEmptyRegion.cs", "Data")]
    [DeploymentItem(@"Reorganizing\Data\RegionsInsertEvenIfEmptyWithEmptyRegion_Reorganized.cs", "Data")]
    public class RegionsInsertEvenIfEmptyWithEmptyRegionTests
    {
        #region Setup

        private static CodeReorganizationManager _codeReorganizationManager;
        private ProjectItem _projectItem;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _codeReorganizationManager = CodeReorganizationManager.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_codeReorganizationManager);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\RegionsInsertEvenIfEmptyWithEmptyRegion.cs");
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
        public void ReorganizingRegionsInsertEvenIfEmptyWithEmptyRegion_ReorganizesAsExpected()
        {
            Settings.Default.Reorganizing_RegionsInsertNewRegions = true;
            Settings.Default.Reorganizing_RegionsInsertKeepEvenIfEmpty = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunReorganize, _projectItem, @"Data\RegionsInsertEvenIfEmptyWithEmptyRegion_Reorganized.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void ReorganizingRegionsInsertEvenIfEmptyWithEmptyRegion_DoesNothingOnSecondPass()
        {
            Settings.Default.Reorganizing_RegionsInsertNewRegions = true;
            Settings.Default.Reorganizing_RegionsInsertKeepEvenIfEmpty = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunReorganize, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void ReorganizingRegionsInsertEvenIfEmptyWithEmptyRegion_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Reorganizing_RegionsInsertNewRegions = false;
            Settings.Default.Reorganizing_RegionsInsertKeepEvenIfEmpty = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunReorganize, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunReorganize(Document document)
        {
            _codeReorganizationManager.Reorganize(document);
        }

        #endregion Helpers
    }
}