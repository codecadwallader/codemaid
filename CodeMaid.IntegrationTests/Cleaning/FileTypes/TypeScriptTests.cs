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

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.FileTypes
{
    [TestClass]
    [DeploymentItem(@"Cleaning\FileTypes\Data\TypeScript.ts", "Data")]
    public class TypeScriptTests
    {
        #region Setup

        private static CodeCleanupAvailabilityLogic _codeCleanupAvailabilityLogic;
        private ProjectItem _projectItem;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _codeCleanupAvailabilityLogic = CodeCleanupAvailabilityLogic.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_codeCleanupAvailabilityLogic);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\TypeScript.ts");
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
        public void CleaningFileTypesTypeScript_EnablesForDocument()
        {
            Settings.Default.Cleaning_IncludeTypeScript = true;

            UIThreadInvoker.Invoke(new Action(() =>
            {
                // Make sure the document is the active document for the environment.
                var document = CleaningTestHelper.GetActivatedDocument(_projectItem);
                Assert.AreEqual(document, TestEnvironment.Package.IDE.ActiveDocument);

                // Confirm the code cleanup availability logic is in the expected state.
                Assert.IsTrue(_codeCleanupAvailabilityLogic.ShouldCleanup(document));
            }));
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningFileTypesTypeScript_EnablesForProjectItem()
        {
            Settings.Default.Cleaning_IncludeTypeScript = true;

            UIThreadInvoker.Invoke(new Action(() =>
            {
                // Confirm the code cleanup availability logic is in the expected state.
                Assert.IsTrue(_codeCleanupAvailabilityLogic.ShouldCleanup(_projectItem));
            }));
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningFileTypesTypeScript_DisablesForDocumentWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_IncludeTypeScript = false;

            UIThreadInvoker.Invoke(new Action(() =>
            {
                // Make sure the document is the active document for the environment.
                var document = CleaningTestHelper.GetActivatedDocument(_projectItem);
                Assert.AreEqual(document, TestEnvironment.Package.IDE.ActiveDocument);

                // Confirm the code cleanup availability logic is in the expected state.
                Assert.IsFalse(_codeCleanupAvailabilityLogic.ShouldCleanup(document));
            }));
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningFileTypesTypeScript_DisablesForProjectItemWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_IncludeTypeScript = false;

            UIThreadInvoker.Invoke(new Action(() =>
            {
                // Confirm the code cleanup availability logic is in the expected state.
                Assert.IsFalse(_codeCleanupAvailabilityLogic.ShouldCleanup(_projectItem));
            }));
        }

        #endregion Tests
    }
}