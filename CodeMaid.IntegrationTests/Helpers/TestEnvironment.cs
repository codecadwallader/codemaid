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
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using SteveCadwallader.CodeMaid.Integration;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Helpers
{
    /// <summary>
    /// The TestEnvironment performs an AssemblyInitialize unit test method to setup the test
    /// environment and capture some environment state for easy access by unit tests.
    /// </summary>
    [TestClass]
    public static class TestEnvironment
    {
        /// <summary>
        /// Gets the <see cref="CodeMaidPackage"/>.
        /// </summary>
        public static CodeMaidPackage Package { get; private set; }

        /// <summary>
        /// Gets the test project.
        /// </summary>
        public static Project Project { get; private set; }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                // Load the package into the shell.
                IVsShell shellService = (IVsShell)VsIdeTestHostContext.ServiceProvider.GetService(typeof(SVsShell));
                Guid packageGuid = new Guid(GuidList.GuidCodeMaidPackageString);
                IVsPackage package;

                shellService.IsPackageLoaded(ref packageGuid, out package);

                if (package == null)
                {
                    shellService.LoadPackage(ref packageGuid, out package);
                }

                Assert.IsTrue(package is CodeMaidPackage);
                Package = (CodeMaidPackage)package;

                // Generate an empty solution.
                const string projectName = "IntegrationTests";
                TestUtils.CreateEmptySolution(testContext.TestDir, projectName);
                Assert.AreEqual(0, TestUtils.ProjectCount());

                // Generate an empty project.
                TestUtils.CreateProjectFromTemplate(projectName, "ConsoleApplication.zip", "CSharp");
                Assert.AreEqual(1, TestUtils.ProjectCount());

                // Capture the project for later use.
                Project = Package.IDE.Solution.Projects.Item(1);
                Assert.IsNotNull(Project);
                Assert.AreEqual(Project.Name, projectName);
            }));
        }
    }
}