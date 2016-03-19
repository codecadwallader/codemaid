using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Model;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Remove\Data\RemoveCurrentRegion.cs", "Data")]
    [DeploymentItem(@"Cleaning\Remove\Data\RemoveCurrentRegion_Cleaned.cs", "Data")]
    public class RemoveCurrentRegionTests
    {
        #region Setup

        private static CodeModelHelper _codeModelHelper;
        private static RemoveRegionLogic _removeRegionLogic;
        private ProjectItem _projectItem;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _codeModelHelper = CodeModelHelper.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_codeModelHelper);

            _removeRegionLogic = RemoveRegionLogic.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_removeRegionLogic);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\RemoveCurrentRegion.cs");
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
        public void CleaningRemoveCurrentRegion_RunsAsExpected()
        {
            TestOperations.ExecuteCommandAndVerifyResults(RunRemoveCurrentRegion, _projectItem, @"Data\RemoveCurrentRegion_Cleaned.cs");
        }

        #endregion Tests

        #region Helpers

        private static void RunRemoveCurrentRegion(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            textDocument.Selection.MoveToLineAndOffset(18, 13);
            Assert.AreEqual(textDocument.Selection.CurrentLine, 18);

            var region = _codeModelHelper.RetrieveCodeRegionUnderCursor(textDocument);
            Assert.IsNotNull(region);

            _removeRegionLogic.RemoveRegion(region);
        }

        #endregion Helpers
    }
}