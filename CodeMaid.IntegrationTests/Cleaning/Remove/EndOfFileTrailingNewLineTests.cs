#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Remove\Data\EndOfFileTrailingNewLine.cs", "Data")]
    [DeploymentItem(@"Cleaning\Remove\Data\EndOfFileTrailingNewLine_Cleaned.cs", "Data")]
    public class EndOfFileTrailingNewLineTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\EndOfFileTrailingNewLine.cs");
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
        public void CleaningRemoveEndOfFileTrailingNewLine_CleansAsExpected()
        {
            Settings.Default.Cleaning_RemoveEndOfFileTrailingNewLine = true;

            CleaningTestHelper.ExecuteCommandAndVerifyResults(RunRemoveEndOfFileTrailingNewLine, _projectItem, @"Data\EndOfFileTrailingNewLine_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveEndOfFileTrailingNewLine_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_RemoveEndOfFileTrailingNewLine = true;

            CleaningTestHelper.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunRemoveEndOfFileTrailingNewLine, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveEndOfFileTrailingNewLine_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_RemoveEndOfFileTrailingNewLine = false;

            CleaningTestHelper.ExecuteCommandAndVerifyNoChanges(RunRemoveEndOfFileTrailingNewLine, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunRemoveEndOfFileTrailingNewLine(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            _removeWhitespaceLogic.RemoveEOFTrailingNewLine(textDocument);
        }

        #endregion Helpers
    }
}