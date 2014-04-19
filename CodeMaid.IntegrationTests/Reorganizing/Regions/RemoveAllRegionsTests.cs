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
using SteveCadwallader.CodeMaid.IntegrationTests.Cleaning;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Reorganizing;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Reorganizing.Regions
{
    [TestClass]
    [DeploymentItem(@"Reorganizing\Regions\Data\RemoveAllRegions.cs", "Data")]
    [DeploymentItem(@"Reorganizing\Regions\Data\RemoveAllRegions_Reorganized.cs", "Data")]
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
        public void ReorganizingRemoveAllRegions_CleansAsExpected()
        {
            CleaningTestHelper.ExecuteCommandAndVerifyResults(RunRemoveAllRegions, _projectItem, @"Data\RemoveAllRegions_Reorganized.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void ReorganizingRemoveAllRegions_DoesNothingOnSecondPass()
        {
            CleaningTestHelper.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunRemoveAllRegions, _projectItem);
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