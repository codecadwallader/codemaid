using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Reorganizing;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Reorganizing
{
    [TestClass]
    [DeploymentItem(@"Reorganizing\Data\RegionsRemoveAndInsertWithoutAccessModifiers.cs", "Data")]
    [DeploymentItem(@"Reorganizing\Data\RegionsRemoveAndInsertWithoutAccessModifiers_Reorganized.cs", "Data")]
    public class RegionsRemoveAndInsertWithoutAccessModifiersTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\RegionsRemoveAndInsertWithoutAccessModifiers.cs");
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
        public void ReorganizingRegionsRemoveAndInsertWithoutAccessModifiers_ReorganizesAsExpected()
        {
            Settings.Default.Reorganizing_RegionsIncludeAccessLevel = false;
            Settings.Default.Reorganizing_RegionsInsertNewRegions = true;
            Settings.Default.Reorganizing_RegionsRemoveExistingRegions = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunReorganize, _projectItem, @"Data\RegionsRemoveAndInsertWithoutAccessModifiers_Reorganized.cs");
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