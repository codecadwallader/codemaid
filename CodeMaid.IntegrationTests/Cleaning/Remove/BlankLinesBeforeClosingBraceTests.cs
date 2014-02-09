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

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Remove\Data\BlankLinesBeforeClosingBrace.cs", "Data")]
    [DeploymentItem(@"Cleaning\Remove\Data\BlankLinesBeforeClosingBrace_Cleaned.cs", "Data")]
    public class BlankLinesBeforeClosingBraceTests
    {
        #region Setup

        private static RemoveWhitespaceLogic _removeWhitespaceLogic;
        private ProjectItem _projectItem;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _removeWhitespaceLogic = RemoveWhitespaceLogic.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_removeWhitespaceLogic);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\BlankLinesBeforeClosingBrace.cs");
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
        public void CleaningRemoveBlankLinesBeforeClosingBrace_CleansAsExpected()
        {
            Settings.Default.Cleaning_RemoveBlankLinesBeforeClosingBrace = true;

            CleaningTestHelper.ExecuteCommandAndVerifyResults(RunRemoveBlankLinesBeforeClosingBrace, _projectItem, @"Data\BlankLinesBeforeClosingBrace_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveBlankLinesBeforeClosingBrace_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_RemoveBlankLinesBeforeClosingBrace = true;

            CleaningTestHelper.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunRemoveBlankLinesBeforeClosingBrace, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveBlankLinesBeforeClosingBrace_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_RemoveBlankLinesBeforeClosingBrace = false;

            CleaningTestHelper.ExecuteCommandAndVerifyNoChanges(RunRemoveBlankLinesBeforeClosingBrace, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunRemoveBlankLinesBeforeClosingBrace(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            _removeWhitespaceLogic.RemoveBlankLinesBeforeClosingBrace(textDocument);
        }

        #endregion Helpers
    }
}