using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Reorganizing;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Reorganizing
{
    [TestClass]
    [DeploymentItem(@"Reorganizing\Data\RegionsRemoveExisting.cs", "Data")]
    [DeploymentItem(@"Reorganizing\Data\RegionsRemoveExisting_Reorganized.cs", "Data")]
    public class RegionsRemoveExistingTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\RegionsRemoveExisting.cs");
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
        public void ReorganizingRegionsRemoveExisting_ReorganizesAsExpected()
        {
            Settings.Default.Reorganizing_RegionsRemoveExistingRegions = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunReorganize, _projectItem, @"Data\RegionsRemoveExisting_Reorganized.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void ReorganizingRegionsRemoveExisting_DoesNothingOnSecondPass()
        {
            Settings.Default.Reorganizing_RegionsRemoveExistingRegions = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunReorganize, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void ReorganizingRegionsRemoveExisting_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Reorganizing_RegionsRemoveExistingRegions = false;

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