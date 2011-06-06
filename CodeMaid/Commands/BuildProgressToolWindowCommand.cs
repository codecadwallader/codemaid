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

using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;

namespace SteveCadwallader.CodeMaid.Commands
{
    /// <summary>
    /// A command that provides for launching the build progress tool window.
    /// </summary>
    internal class BuildProgressToolWindowCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildProgressToolWindowCommand"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal BuildProgressToolWindowCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(GuidList.GuidCodeMaidCommandBuildProgressToolWindow, (int)PkgCmdIDList.CmdIDCodeMaidBuildProgressToolWindow))
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the build progress window frame.
        /// </summary>
        private IVsWindowFrame BuildProgressWindowFrame
        {
            get
            {
                var buildProgress = Package.BuildProgress;
                if (buildProgress != null)
                {
                    return buildProgress.Frame as IVsWindowFrame;
                }

                return null;
            }
        }

        #endregion Properties

        #region BaseCommand Methods

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            ShowBuildProgressToolWindow();
        }

        #endregion BaseCommand Methods

        #region Internal Methods

        /// <summary>
        /// Called when a build has begun.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="action">The action.</param>
        internal void OnBuildBegin(vsBuildScope scope, vsBuildAction action)
        {
            var buildProgress = Package.BuildProgress;
            if (buildProgress != null)
            {
                buildProgress.NotifyBuildBegin(scope, action);

                if (Package.Options.BuildProgress.AutoShowBuildProgressOnBuildStart)
                {
                    ShowBuildProgressToolWindowWithoutActivation();
                }
            }
        }

        /// <summary>
        /// Called when an individual project build has begun.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="projectConfig">The project config.</param>
        /// <param name="platform">The platform.</param>
        /// <param name="solutionConfig">The solution config.</param>
        internal void OnBuildProjConfigBegin(string project, string projectConfig, string platform, string solutionConfig)
        {
            var buildProgress = Package.BuildProgress;
            if (buildProgress != null)
            {
                buildProgress.NotifyBuildProjConfigBegin(project, projectConfig, platform, solutionConfig);
            }
        }

        /// <summary>
        /// Called when a build is done.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="action">The action.</param>
        internal void OnBuildDone(vsBuildScope scope, vsBuildAction action)
        {
            var buildProgress = Package.BuildProgress;
            if (buildProgress != null)
            {
                buildProgress.NotifyBuildDone(scope, action);

                if (Package.Options.BuildProgress.AutoHideBuildProgressOnBuildStop)
                {
                    HideBuildProgressToolWindow();
                }
            }
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Shows the build progress tool window.
        /// </summary>
        private void ShowBuildProgressToolWindow()
        {
            var frame = BuildProgressWindowFrame;
            if (frame != null)
            {
                frame.Show();
            }
        }

        /// <summary>
        /// Shows the build progress tool window without activation.
        /// </summary>
        private void ShowBuildProgressToolWindowWithoutActivation()
        {
            var frame = BuildProgressWindowFrame;
            if (frame != null)
            {
                frame.ShowNoActivate();
            }
        }

        /// <summary>
        /// Hides the build progress tool window.
        /// </summary>
        private void HideBuildProgressToolWindow()
        {
            var frame = BuildProgressWindowFrame;
            if (frame != null)
            {
                frame.Hide();
            }
        }

        #endregion Private Methods
    }
}