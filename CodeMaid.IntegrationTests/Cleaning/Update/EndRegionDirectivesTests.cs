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
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Update
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Update\Data\EndRegionDirectives.cs", "Data")]
    [DeploymentItem(@"Cleaning\Update\Data\EndRegionDirectives_Cleaned.cs", "Data")]
    public class EndRegionDirectivesTests
    {
        #region Setup

        private static UpdateLogic _updateLogic;
        private ProjectItem _projectItem;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _updateLogic = UpdateLogic.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_updateLogic);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\EndRegionDirectives.cs");
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
        public void CleaningUpdateEndRegionDirectives_CleansAsExpected()
        {
            Settings.Default.Cleaning_UpdateEndRegionDirectives = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunUpdateEndRegionDirectives, _projectItem, @"Data\EndRegionDirectives_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningUpdateEndRegionDirectives_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_UpdateEndRegionDirectives = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunUpdateEndRegionDirectives, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningUpdateEndRegionDirectives_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_UpdateEndRegionDirectives = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunUpdateEndRegionDirectives, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunUpdateEndRegionDirectives(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            _updateLogic.UpdateEndRegionDirectives(textDocument);
        }

        #endregion Helpers
    }
}