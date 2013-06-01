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
    [DeploymentItem(@"Data\CleaningRemoveMultipleConsecutiveBlankLines.cs", "Data")]
    [DeploymentItem(@"Data\CleaningRemoveMultipleConsecutiveBlankLines_After.cs", "Data")]
    public class CleaningRemoveMultipleConsecutiveBlankLinesTests
    {
        #region Setup

        private static CodeMaidPackage _package;
        private static RemoveWhitespaceLogic _removeWhitespaceLogic;
        private static Project _project;
        private ProjectItem _projectItem;

        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                // Generate a package.
                _package = CodeMaidPackageHelper.GetCodeMaidPackage();
                Assert.IsNotNull(_package);

                // Generate a logic manager.
                _removeWhitespaceLogic = RemoveWhitespaceLogic.GetInstance(_package);
                Assert.IsNotNull(_removeWhitespaceLogic);

                // Generate an empty solution.
                const string projectName = "CleaningTests";
                TestUtils.CreateEmptySolution(testContext.TestDir, projectName);
                Assert.AreEqual(0, TestUtils.ProjectCount());

                // Generate an empty project.
                TestUtils.CreateProjectFromTemplate(projectName, "ConsoleApplication.zip", "CSharp");
                Assert.AreEqual(1, TestUtils.ProjectCount());

                // Capture the project for later use.
                _project = _package.IDE.Solution.Projects.Item(1);
                Assert.IsNotNull(_project);
                Assert.AreEqual(_project.Name, projectName);
            }));
        }

        [TestInitialize]
        public void TestInitialize()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                int initialCount = _project.ProjectItems.Count;

                _projectItem = _project.ProjectItems.AddFromFileCopy(@"Data\CleaningRemoveMultipleConsecutiveBlankLines.cs");

                Assert.IsNotNull(_projectItem);
                Assert.AreEqual(initialCount + 1, _project.ProjectItems.Count);
            }));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                int initialCount = _project.ProjectItems.Count;

                _projectItem.Delete();

                Assert.AreEqual(initialCount - 1, _project.ProjectItems.Count);
            }));
        }

        #endregion Setup

        #region Tests

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveMultipleConsecutiveBlankLines_AsExpected()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                Settings.Default.Cleaning_RemoveMultipleConsecutiveBlankLines = true;

                var document = GetActivatedDocument(_projectItem);

                // Run command and assert it is not saved afterwards.
                Assert.IsTrue(document.Saved);
                RunRemoveMultipleConsecutiveBlankLines(document);
                Assert.IsFalse(document.Saved);

                // Save the document.
                document.Save();
                Assert.IsTrue(document.Saved);

                // Compare the contents of the baseline with the generated content.
                var baselineContent = File.ReadAllText(@"Data\CleaningRemoveMultipleConsecutiveBlankLines_After.cs");
                var cleanedContent = File.ReadAllText(document.FullName);

                Assert.AreEqual(baselineContent, cleanedContent);
            }));
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveMultipleConsecutiveBlankLines_DoesNothingOnSecondPass()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                Settings.Default.Cleaning_RemoveMultipleConsecutiveBlankLines = true;

                var document = GetActivatedDocument(_projectItem);

                // Run command a first time and assert it is not saved afterwards.
                Assert.IsTrue(document.Saved);
                RunRemoveMultipleConsecutiveBlankLines(document);
                Assert.IsFalse(document.Saved);

                // Save the document.
                document.Save();
                Assert.IsTrue(document.Saved);

                // Run command a second time and assert it is still in a saved state (i.e. no changes).
                RunRemoveMultipleConsecutiveBlankLines(document);
                Assert.IsTrue(document.Saved);
            }));
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveMultipleConsecutiveBlankLines_DoesNothingWhenSettingIsDisabled()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                Settings.Default.Cleaning_RemoveMultipleConsecutiveBlankLines = false;

                var document = GetActivatedDocument(_projectItem);

                // Run command and assert it is still in a saved state (i.e. no changes).
                Assert.IsTrue(document.Saved);
                RunRemoveMultipleConsecutiveBlankLines(document);
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

        private void RunRemoveMultipleConsecutiveBlankLines(Document document)
        {
            var textDocument = GetTextDocument(document);

            _removeWhitespaceLogic.RemoveMultipleConsecutiveBlankLines(textDocument);
        }

        #endregion Helpers
    }
}