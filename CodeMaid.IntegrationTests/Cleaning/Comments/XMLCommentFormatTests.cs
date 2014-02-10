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

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Comments
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Comments\Data\XMLCommentFormat.cs", "Data")]
    [DeploymentItem(@"Cleaning\Comments\Data\XMLCommentFormat_Cleaned.cs", "Data")]
    public class XMLCommentFormatTests
    {
        #region Setup

        private static CommentFormatLogic _commentFormatLogic;
        private ProjectItem _projectItem;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _commentFormatLogic = CommentFormatLogic.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_commentFormatLogic);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\XMLCommentFormat.cs");
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
        public void CleaningFormatXMLComments_CleansAsExpected()
        {
            Settings.Default.Cleaning_CommentRunDuringCleanup = true;

            CleaningTestHelper.ExecuteCommandAndVerifyResults(RunFormatComments, _projectItem, @"Data\XMLCommentFormat_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningFormatXMLComments_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_CommentRunDuringCleanup = true;

            CleaningTestHelper.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunFormatComments, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningFormatXMLComments_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_CommentRunDuringCleanup = false;

            CleaningTestHelper.ExecuteCommandAndVerifyNoChanges(RunFormatComments, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunFormatComments(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            _commentFormatLogic.FormatComments(textDocument);
        }

        #endregion Helpers
    }
}