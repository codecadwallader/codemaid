#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Reorganizing;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Reorganizing.Regions
{
    [TestClass]
    [DeploymentItem(@"Reorganizing\Regions\Data\RemoveSelectedRegions.cs", "Data")]
    [DeploymentItem(@"Reorganizing\Regions\Data\RemoveSelectedRegions_Reorganized.cs", "Data")]
    public class RemoveSelectedRegionsTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\RemoveSelectedRegions.cs");
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
        public void ReorganizingRemoveSelectedRegions_RunsAsExpected()
        {
            TestOperations.ExecuteCommandAndVerifyResults(RunRemoveSelectedRegions, _projectItem, @"Data\RemoveSelectedRegions_Reorganized.cs");
        }

        #endregion Tests

        #region Helpers

        private static void RunRemoveSelectedRegions(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            textDocument.Selection.MoveToLineAndOffset(14, 9);
            textDocument.Selection.MoveToLineAndOffset(45, 5, true);

            _removeRegionLogic.RemoveRegions(textDocument.Selection);
        }

        #endregion Helpers
    }
}