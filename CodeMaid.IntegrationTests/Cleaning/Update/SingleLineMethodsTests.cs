#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Update
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Update\Data\SingleLineMethods.cs", "Data")]
    [DeploymentItem(@"Cleaning\Update\Data\SingleLineMethods_Cleaned.cs", "Data")]
    public class SingleLineMethodsTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\SingleLineMethods.cs");
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
        public void CleaningUpdateSingleLineMethods_CleansAsExpected()
        {
            Settings.Default.Cleaning_UpdateSingleLineMethods = true;

            CleaningTestHelper.ExecuteCommandAndVerifyResults(RunUpdateSingleLineMethods, _projectItem, @"Data\SingleLineMethods_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningUpdateSingleLineMethods_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_UpdateSingleLineMethods = true;

            CleaningTestHelper.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunUpdateSingleLineMethods, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningUpdateSingleLineMethods_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_UpdateSingleLineMethods = false;

            CleaningTestHelper.ExecuteCommandAndVerifyNoChanges(RunUpdateSingleLineMethods, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunUpdateSingleLineMethods(Document document)
        {
            var codeItems = CleaningTestHelper.CodeModelManager.RetrieveAllCodeItems(document);
            var methods = codeItems.OfType<CodeItemMethod>().ToList();

            _updateLogic.UpdateSingleLineMethods(methods);
        }

        #endregion Helpers
    }
}