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
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using System.Linq;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Insert\Data\BlankLinePaddingAfterDelegates.cs", "Data")]
    [DeploymentItem(@"Cleaning\Insert\Data\BlankLinePaddingAfterDelegates_Cleaned.cs", "Data")]
    public class BlankLinePaddingAfterDelegatesTests
    {
        #region Setup

        private static InsertBlankLinePaddingLogic _insertBlankLinePaddingLogic;
        private ProjectItem _projectItem;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _insertBlankLinePaddingLogic = InsertBlankLinePaddingLogic.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_insertBlankLinePaddingLogic);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\BlankLinePaddingAfterDelegates.cs");
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
        public void CleaningInsertBlankLinePaddingAfterDelegates_CleansAsExpected()
        {
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterDelegates = true;

            CleaningTestHelper.ExecuteCommandAndVerifyResults(RunInsertBlankLinePaddingAfterDelegates, _projectItem, @"Data\BlankLinePaddingAfterDelegates_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertBlankLinePaddingAfterDelegates_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterDelegates = true;

            CleaningTestHelper.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunInsertBlankLinePaddingAfterDelegates, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertBlankLinePaddingAfterDelegates_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterDelegates = false;

            CleaningTestHelper.ExecuteCommandAndVerifyNoChanges(RunInsertBlankLinePaddingAfterDelegates, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunInsertBlankLinePaddingAfterDelegates(Document document)
        {
            var codeItems = CleaningTestHelper.CodeModelManager.RetrieveAllCodeItems(document);
            var delegates = codeItems.OfType<CodeItemDelegate>().ToList();

            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(delegates);
        }

        #endregion Helpers
    }
}