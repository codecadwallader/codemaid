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
    public class CleaningRemoveTests
    {
        #region Setup

        private static CodeMaidPackage _package;
        private static RemoveWhitespaceLogic _removeWhitespaceLogic;
        private static TestUtils _testUtils;
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

                _testUtils = new TestUtils();

                // Generate an empty solution.
                const string projectName = "CleaningRemoveTests";
                _testUtils.CreateEmptySolution(testContext.TestDir, projectName);
                Assert.AreEqual(0, _testUtils.ProjectCount());

                // Generate an empty project.
                _testUtils.CreateProjectFromTemplate(projectName, "ConsoleApplication.zip", "CSharp");
                Assert.AreEqual(1, _testUtils.ProjectCount());

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

                _projectItem = _project.ProjectItems.AddFromFileCopy(@"Data\CleaningRemoveEndOfLineWhitespace.cs");

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
        public void CleaningRemoveEndOfLineWhitespace_AsExpected()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                Settings.Default.Cleaning_RemoveEndOfLineWhitespace = true;

                _projectItem.Open(Constants.vsViewKindTextView);

                var document = _projectItem.Document;
                Assert.IsNotNull(_projectItem.Document);

                document.Activate();

                var textDocument = (TextDocument)document.Object("TextDocument");
                Assert.IsNotNull(textDocument);

                Assert.IsTrue(document.Saved);
                _removeWhitespaceLogic.RemoveEOLWhitespace(textDocument);
                Assert.IsFalse(document.Saved);

                //TODO: Confirm state of TextDocument is as expected, probably go with a before/after deployment file approach and do a checksum or binary comparison?
            }));
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveEndOfLineWhitespace_DoesNothingOnSecondPass()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                Settings.Default.Cleaning_RemoveEndOfLineWhitespace = true;

                //TODO: Refactor logic above, run it twice and confirm document saved states.

                _removeWhitespaceLogic.RemoveEOLWhitespace(null);
            }));
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveEndOfLineWhitespace_DoesNothingWhenSettingIsDisabled()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                Settings.Default.Cleaning_RemoveEndOfLineWhitespace = false;

                //TODO: Utilize same logic as above after refactored.

                _removeWhitespaceLogic.RemoveEOLWhitespace(null);
            }));
        }

        #endregion Tests
    }
}