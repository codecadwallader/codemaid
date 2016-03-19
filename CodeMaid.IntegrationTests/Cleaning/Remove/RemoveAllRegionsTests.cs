using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Remove\Data\RemoveAllRegions.cs", "Data")]
    [DeploymentItem(@"Cleaning\Remove\Data\RemoveAllRegions_Cleaned.cs", "Data")]
    public class RemoveAllRegionsTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\RemoveAllRegions.cs");
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
        public void CleaningRemoveAllRegions_RunsAsExpected()
        {
            TestOperations.ExecuteCommandAndVerifyResults(RunRemoveAllRegions, _projectItem, @"Data\RemoveAllRegions_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveAllRegions_DoesNothingOnSecondPass()
        {
            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunRemoveAllRegions, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunRemoveAllRegions(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            _removeRegionLogic.RemoveRegions(textDocument);
        }

        #endregion Helpers
    }
}