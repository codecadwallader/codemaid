using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Sorting
{
    [TestClass]
    [DeploymentItem(@"Sorting\Data\SortNoChange.cs", "Data")]
    [DeploymentItem(@"Sorting\Data\SortNoChange_Sorted.cs", "Data")]
    public class SortNoChangeTests
    {
        #region Setup

        private ProjectItem _projectItem;

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\SortNoChange.cs");
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
        public void SortNoChange_DoesNothingOnFirstPass()
        {
            TestOperations.ExecuteCommandAndVerifyNoChanges(RunSort, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunSort(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);
            textDocument.Selection.SelectAll();

            var sortCommand = new CommandID(PackageGuids.GuidCodeMaidCommandSortLines, PackageIds.CmdIDCodeMaidSortLines);
            TestUtils.ExecuteCommand(sortCommand);
        }

        #endregion Helpers
    }
}