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
    [DeploymentItem(@"Cleaning\Remove\Data\MultipleConsecutiveBlankLines.cs", "Data")]
    [DeploymentItem(@"Cleaning\Remove\Data\MultipleConsecutiveBlankLines_Cleaned.cs", "Data")]
    public class MultipleConsecutiveBlankLinesTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\MultipleConsecutiveBlankLines.cs");
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
        public void CleaningRemoveMultipleConsecutiveBlankLines_CleansAsExpected()
        {
            Settings.Default.Cleaning_RemoveMultipleConsecutiveBlankLines = true;

            CleaningTestHelper.ExecuteCommandAndVerifyResults(RunRemoveMultipleConsecutiveBlankLines, _projectItem, @"Data\MultipleConsecutiveBlankLines_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveMultipleConsecutiveBlankLines_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_RemoveMultipleConsecutiveBlankLines = true;

            CleaningTestHelper.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunRemoveMultipleConsecutiveBlankLines, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveMultipleConsecutiveBlankLines_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_RemoveMultipleConsecutiveBlankLines = false;

            CleaningTestHelper.ExecuteCommandAndVerifyNoChanges(RunRemoveMultipleConsecutiveBlankLines, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private void RunRemoveMultipleConsecutiveBlankLines(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            _removeWhitespaceLogic.RemoveMultipleConsecutiveBlankLines(textDocument);
        }

        #endregion Helpers
    }
}