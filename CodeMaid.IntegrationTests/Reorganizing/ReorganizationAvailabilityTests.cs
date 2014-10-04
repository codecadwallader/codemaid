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
using Microsoft.VSSDK.Tools.VsIdeTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Reorganizing;
using System;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Reorganizing
{
    [TestClass]
    [DeploymentItem(@"Reorganizing\Data\ReorganizationAvailability.cs", "Data")]
    public class ReorganizationAvailabilityTests
    {
        #region Setup

        private static CodeReorganizationAvailabilityLogic _codeReorganizationAvailabilityLogic;
        private ProjectItem _projectItem;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _codeReorganizationAvailabilityLogic = CodeReorganizationAvailabilityLogic.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_codeReorganizationAvailabilityLogic);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\ReorganizationAvailability.cs");
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
        public void ReorganizationAvailability_DisabledWhenPreprocessorConditionalCompilationDirectivesExist()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                // Make sure the document is the active document for the environment.
                var document = TestOperations.GetActivatedDocument(_projectItem);
                Assert.AreEqual(document, TestEnvironment.Package.ActiveDocument);

                // Confirm the code reorganization availability logic is in the expected state.
                Assert.IsFalse(_codeReorganizationAvailabilityLogic.CanReorganize(document));
            }));
        }

        #endregion Tests
    }
}