#region CodeMaid is Copyright 2007-2016 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2016 Steve Cadwallader.

using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.Integration;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Sorting
{
    [TestClass]
    [DeploymentItem(@"Sorting\Data\SortAtTop.cs", "Data")]
    [DeploymentItem(@"Sorting\Data\SortAtTop_Sorted.cs", "Data")]
    public class SortAtTopTests
    {
        #region Setup

        private ProjectItem _projectItem;

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\SortAtTop.cs");
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
        public void SortAtTop_FormatsAsExpected()
        {
            TestOperations.ExecuteCommandAndVerifyResults(RunSort, _projectItem, @"Data\SortAtTop_Sorted.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void SortAtTop_DoesNothingOnSecondPass()
        {
            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunSort, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunSort(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);
            textDocument.Selection.StartOfDocument();
            textDocument.Selection.LineDown(true, 4);

            var sortCommand = new CommandID(GuidList.GuidCodeMaidCommandSortLines, (int)PkgCmdIDList.CmdIDCodeMaidSortLines);
            TestUtils.ExecuteCommand(sortCommand);
        }

        #endregion Helpers
    }
}