#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for launching the build progress tool window.
    /// </summary>
    internal class BuildProgressToolWindowCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildProgressToolWindowCommand" /> class.
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

                if (Settings.Default.Progressing_ShowBuildProgressOnBuildStart)
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
        /// Called when an individual project build is done.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="projectConfig">The project config.</param>
        /// <param name="platform">The platform.</param>
        /// <param name="solutionConfig">The solution config.</param>
        /// <param name="success">True if project build was successful, otherwise false.</param>
        internal void OnBuildProjConfigDone(string project, string projectConfig, string platform, string solutionConfig, bool success)
        {
            var buildProgress = Package.BuildProgress;
            if (buildProgress != null)
            {
                buildProgress.NotifyBuildProjConfigDone(project, projectConfig, platform, solutionConfig, success);
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

                if (Settings.Default.Progressing_HideBuildProgressOnBuildStop)
                {
                    HideBuildProgressToolWindow();
                }
            }
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Docks the specified frame window if it is currently floating.
        /// </summary>
        /// <remarks>Works in VS2010, does not appear to work in VS2008.</remarks>
        /// <param name="frame">The frame.</param>
        private static void DockWindowIfFloating(IVsWindowFrame frame)
        {
            // Get the current tool window frame mode.
            object currentFrameMode;
            frame.GetProperty((int)__VSFPROPID.VSFPROPID_FrameMode, out currentFrameMode);

            // If currently floating, switch to dock mode.
            if ((VSFRAMEMODE)currentFrameMode == VSFRAMEMODE.VSFM_Float)
            {
                frame.SetProperty((int)__VSFPROPID.VSFPROPID_FrameMode, VSFRAMEMODE.VSFM_Dock);
            }
        }

        /// <summary>
        /// Shows the build progress tool window.
        /// </summary>
        private void ShowBuildProgressToolWindow()
        {
            var frame = BuildProgressWindowFrame;
            if (frame != null)
            {
                frame.Show();
                DockWindowIfFloating(frame);
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
                DockWindowIfFloating(frame);
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