using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Reorganizing;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Reorganizing
{
    [TestClass]
    [DeploymentItem(@"Reorganizing\Data\RegionsInsertEvenIfEmptyWithEmptyClass.cs", "Data")]
    [DeploymentItem(@"Reorganizing\Data\RegionsInsertEvenIfEmptyWithEmptyClass_Reorganized.cs", "Data")]
    public class RegionsInsertEvenIfEmptyWithEmptyClassTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\RegionsInsertEvenIfEmptyWithEmptyClass.cs");
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
        public void ReorganizingRegionsInsertEvenIfEmptyWithEmptyClass_ReorganizesAsExpected()
        {
            Settings.Default.Reorganizing_RegionsInsertKeepEvenIfEmpty = true;
            Settings.Default.Reorganizing_RegionsInsertNewRegions = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunReorganize, _projectItem, @"Data\RegionsInsertEvenIfEmptyWithEmptyClass_Reorganized.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void ReorganizingRegionsInsertEvenIfEmptyWithEmptyClass_DoesNothingOnSecondPass()
        {
            Settings.Default.Reorganizing_RegionsInsertKeepEvenIfEmpty = true;
            Settings.Default.Reorganizing_RegionsInsertNewRegions = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunReorganize, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void ReorganizingRegionsInsertEvenIfEmptyWithEmptyClass_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Reorganizing_RegionsInsertKeepEvenIfEmpty = false;
            Settings.Default.Reorganizing_RegionsInsertNewRegions = false;

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