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
    public abstract class CommentFormatTestsHelper
    {
        private static CommentFormatLogic _commentFormatLogic;

        private ProjectItem _projectItem;

        protected abstract string BaseFileName { get; }

        public static void ClassInitialize(TestContext testContext)
        {
            _commentFormatLogic = CommentFormatLogic.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_commentFormatLogic);
        }

        public virtual void TestCleanup()
        {
            TestEnvironment.RemoveFromProject(_projectItem);
        }

        public virtual void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(string.Format(@"Data\{0}.cs", BaseFileName));
        }

        #region Tests

        protected void CleansAsExpected()
        {
            Settings.Default.Cleaning_CommentRunDuringCleanup = true;
            CleaningTestHelper.ExecuteCommandAndVerifyResults(RunFormatComments, _projectItem, string.Format(@"Data\{0}_Cleaned.cs", this.BaseFileName));
        }

        protected void DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_CommentRunDuringCleanup = true;
            CleaningTestHelper.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunFormatComments, _projectItem);
        }

        protected void DoesNothingWhenSettingIsDisabled()
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