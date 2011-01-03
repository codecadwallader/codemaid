#region CodeMaid is Copyright 2007-2011 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2011 Steve Cadwallader.

using System;
using System.Linq;
using System.Text;
using EnvDTE;

namespace SteveCadwallader.CodeMaid.Events
{
    /// <summary>
    /// A class that encapsulates listening for build status events.
    /// </summary>
    internal class BuildStatusEventListener : BaseEventListener
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildStatusEventListener"/> class.
        /// </summary>
        /// <param name="package">The package hosting the event listener.</param>
        internal BuildStatusEventListener(CodeMaidPackage package)
            : base(package)
        {
            // Store access to the build events, otherwise events will not register properly via DTE.
            BuildEvents = Package.IDE.Events.BuildEvents;
            BuildEvents.OnBuildBegin += BuildEvents_OnBuildBegin;
            BuildEvents.OnBuildProjConfigBegin += BuildEvents_OnBuildProjConfigBegin;
        }

        #endregion Constructors

        #region Private Properties

        /// <summary>
        /// Gets the active solution configuration.
        /// </summary>
        private SolutionConfiguration ActiveSolutionConfiguration
        {
            get { return Package.IDE.Solution.SolutionBuild.ActiveConfiguration; }
        }

        /// <summary>
        /// Gets or sets a pointer to the IDE build events.
        /// </summary>
        private BuildEvents BuildEvents { get; set; }

        /// <summary>
        /// Gets or sets the last known build action.
        /// </summary>
        private vsBuildAction BuildAction { get; set; }

        /// <summary>
        /// Gets or sets the last known build scope.
        /// </summary>
        private vsBuildScope BuildScope { get; set; }

        /// <summary>
        /// Gets or sets the number of projects built.
        /// </summary>
        private int NumberOfProjectsBuilt { get; set; }

        /// <summary>
        /// Gets or setse the number of projects to be built.
        /// </summary>
        private int NumberOfProjectsToBeBuilt { get; set; }

        #endregion Private Properties

        #region Private Event Handlers

        /// <summary>
        /// Event raised when a compilation begins.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="action">The action.</param>
        private void BuildEvents_OnBuildBegin(vsBuildScope scope, vsBuildAction action)
        {
            BuildAction = action;
            BuildScope = scope;
            NumberOfProjectsBuilt = 0;
            NumberOfProjectsToBeBuilt = GetNumberOfProjectsToBeBuilt();
        }

        /// <summary>
        /// Event raised when the compilation of a project within a build begins.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="projectConfig">The project config.</param>
        /// <param name="platform">The platform.</param>
        /// <param name="solutionConfig">The solution config.</param>
        private void BuildEvents_OnBuildProjConfigBegin(string project, string projectConfig, string platform, string solutionConfig)
        {
            if (Package.Options.BuildStatus.ExtendBuildStatusMessages && NumberOfProjectsToBeBuilt > 0)
            {
                string buildString = GetBuildTypeString(BuildScope, BuildAction);
                string projectName = ExtractProjectName(project);

                string projectsString = NumberOfProjectsToBeBuilt.ToString();
                string completeString = (++NumberOfProjectsBuilt).ToString().PadLeft(projectsString.Length);

                string outputString = String.Format(
                    "{0}  {1} of {2}     '{3}'     ({4} {5})...",
                    buildString, completeString, projectsString, projectName, projectConfig, platform);

                Package.IDE.StatusBar.Text = outputString;
            }
        }

        #endregion Private Event Handlers

        #region Private Methods

        /// <summary>
        /// Extracts the project name from the specified project string.
        /// </summary>
        /// <param name="project">The raw project string to process.</param>
        /// <returns>The extracted project name.</returns>
        private static string ExtractProjectName(string project)
        {
            int lastBackslash = project.LastIndexOf('\\') + 1;
            int lastPeriod = project.LastIndexOf('.') + 1;

            return project.Substring(lastBackslash, lastPeriod - lastBackslash - 1);
        }

        /// <summary>
        /// Gets the build type string from the specified build scope and action.
        /// </summary>
        /// <param name="buildScope">The build scope.</param>
        /// <param name="buildAction">The build action.</param>
        /// <returns>The build type string.</returns>
        private static string GetBuildTypeString(vsBuildScope buildScope, vsBuildAction buildAction)
        {
            StringBuilder stringBuilder = new StringBuilder();

            // First append the word 'Batch ' if this is a batch build event.
            if (buildScope == vsBuildScope.vsBuildScopeBatch)
            {
                stringBuilder.Append("Batch ");
            }

            // Next append the action-specific build string.
            switch (buildAction)
            {
                case vsBuildAction.vsBuildActionBuild:
                    stringBuilder.Append("Building");
                    break;

                case vsBuildAction.vsBuildActionClean:
                    stringBuilder.Append("Cleaning");
                    break;

                case vsBuildAction.vsBuildActionDeploy:
                    stringBuilder.Append("Deploying");
                    break;

                case vsBuildAction.vsBuildActionRebuildAll:
                    stringBuilder.Append("Rebuilding");
                    break;
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets the number of projects to be built based on the active solution configuration.
        /// </summary>
        /// <returns>The number of projects to be built.</returns>
        private int GetNumberOfProjectsToBeBuilt()
        {
            return ActiveSolutionConfiguration.SolutionContexts.Cast<SolutionContext>().Count(context => context.ShouldBuild);
        }

        #endregion Private Methods

        #region IDisposable Members

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;

                if (disposing && BuildEvents != null)
                {
                    BuildEvents.OnBuildBegin -= BuildEvents_OnBuildBegin;
                    BuildEvents.OnBuildProjConfigBegin -= BuildEvents_OnBuildProjConfigBegin;
                }
            }
        }

        #endregion IDisposable Members
    }
}