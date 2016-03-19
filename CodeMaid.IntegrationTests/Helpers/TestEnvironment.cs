using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Helpers
{
    /// <summary>
    /// The TestEnvironment performs an AssemblyInitialize unit test method to setup the test
    /// environment and capture state for easy access by unit tests.
    /// </summary>
    [TestClass]
    public static class TestEnvironment
    {
        /// <summary>
        /// Gets the <see cref="CodeMaidPackage" />.
        /// </summary>
        public static CodeMaidPackage Package { get; private set; }

        /// <summary>
        /// Gets the test project.
        /// </summary>
        public static Project Project { get; private set; }

        /// <summary>
        /// This method perform a one-time initialization across all unit tests in the assembly.
        /// </summary>
        /// <param name="testContext">The test context.</param>
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                // Load the package into the shell.
                IVsShell shellService = (IVsShell)VsIdeTestHostContext.ServiceProvider.GetService(typeof(SVsShell));
                Guid packageGuid = new Guid(PackageGuids.GuidCodeMaidPackageString);
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

        /// <summary>
        /// A set of common actions for test initialization to be shared across most unit tests.
        /// </summary>
        public static void CommonTestInitialize()
        {
            // Reset all settings to default.
            Settings.Default.Reset();
        }

        /// <summary>
        /// Gets the package command based on the specified command ID.
        /// </summary>
        /// <param name="commandID">The command ID to retrieve.</param>
        /// <returns>The package command.</returns>
        public static MenuCommand GetPackageCommand(CommandID commandID)
        {
            var command = Package.MenuCommandService.FindCommand(commandID);
            Assert.IsNotNull(command);

            return command;
        }

        /// <summary>
        /// Loads the specified file into the test project.
        /// </summary>
        /// <param name="path">The path to the file to load.</param>
        /// <returns>The project item representing the loaded file.</returns>
        public static ProjectItem LoadFileIntoProject(string path)
        {
            ProjectItem projectItem = null;

            UIThreadInvoker.Invoke(new Action(() =>
            {
                int initialCount = Project.ProjectItems.Count;

                projectItem = Project.ProjectItems.AddFromFileCopy(path);

                Assert.IsNotNull(projectItem);
                Assert.AreEqual(initialCount + 1, Project.ProjectItems.Count);
            }));

            Assert.IsNotNull(projectItem);

            return projectItem;
        }

        /// <summary>
        /// Removes the specified project item from the test project.
        /// </summary>
        /// <param name="projectItem">The project item to remove.</param>
        public static void RemoveFromProject(ProjectItem projectItem)
        {
            Assert.IsNotNull(projectItem);

            UIThreadInvoker.Invoke(new Action(() =>
            {
                int initialCount = Project.ProjectItems.Count;

                projectItem.Delete();

                Assert.AreEqual(initialCount - 1, Project.ProjectItems.Count);
            }));
        }
    }
}