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

using System;
using System.IO;
using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests
{
    [TestClass]
    [DeploymentItem(@"Data\CleaningRemoveEndOfLineWhitespace.cs", "Data")]
    [DeploymentItem(@"Data\CleaningRemoveEndOfLineWhitespace_After.cs", "Data")]
    public class CleaningRemoveEndOfLineWhitespaceTests
    {
        #region Setup

        private static RemoveWhitespaceLogic _removeWhitespaceLogic;
        private ProjectItem _projectItem;

        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            // Generate a logic manager.
            _removeWhitespaceLogic = RemoveWhitespaceLogic.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_removeWhitespaceLogic);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\CleaningRemoveEndOfLineWhitespace.cs");
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
        public void CleaningRemoveEndOfLineWhitespace_AsExpected()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                Settings.Default.Cleaning_RemoveEndOfLineWhitespace = true;

                var document = GetActivatedDocument(_projectItem);

                // Run command and assert it is not saved afterwards.
                Assert.IsTrue(document.Saved);
                RunRemoveEndOfLineWhitespace(document);
                Assert.IsFalse(document.Saved);

                // Save the document.
                document.Save();
                Assert.IsTrue(document.Saved);

                // Compare the contents of the baseline with the generated content.
                var baselineContent = File.ReadAllText(@"Data\CleaningRemoveEndOfLineWhitespace_After.cs");
                var cleanedContent = File.ReadAllText(document.FullName);

                Assert.AreEqual(baselineContent, cleanedContent);
            }));
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveEndOfLineWhitespace_DoesNothingOnSecondPass()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                Settings.Default.Cleaning_RemoveEndOfLineWhitespace = true;

                var document = GetActivatedDocument(_projectItem);

                // Run command a first time and assert it is not saved afterwards.
                Assert.IsTrue(document.Saved);
                RunRemoveEndOfLineWhitespace(document);
                Assert.IsFalse(document.Saved);

                // Save the document.
                document.Save();
                Assert.IsTrue(document.Saved);

                // Run command a second time and assert it is still in a saved state (i.e. no changes).
                RunRemoveEndOfLineWhitespace(document);
                Assert.IsTrue(document.Saved);
            }));
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveEndOfLineWhitespace_DoesNothingWhenSettingIsDisabled()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                Settings.Default.Cleaning_RemoveEndOfLineWhitespace = false;

                var document = GetActivatedDocument(_projectItem);

                // Run command and assert it is still in a saved state (i.e. no changes).
                Assert.IsTrue(document.Saved);
                RunRemoveEndOfLineWhitespace(document);
                Assert.IsTrue(document.Saved);
            }));
        }

        #endregion Tests

        #region Helpers

        private static Document GetActivatedDocument(ProjectItem projectItem)
        {
            projectItem.Open(Constants.vsViewKindTextView);

            var document = projectItem.Document;
            Assert.IsNotNull(projectItem.Document);

            document.Activate();

            return document;
        }

        private static TextDocument GetTextDocument(Document document)
        {
            var textDocument = (TextDocument)document.Object("TextDocument");
            Assert.IsNotNull(textDocument);

            return textDocument;
        }

        private void RunRemoveEndOfLineWhitespace(Document document)
        {
            var textDocument = GetTextDocument(document);

            _removeWhitespaceLogic.RemoveEOLWhitespace(textDocument);
        }

        #endregion Helpers
    }
}