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
            BuildEvents.OnBuildDone += BuildEvents_OnBuildDone;
        }

        #endregion Constructors

        #region Internal Events

        /// <summary>
        /// An event raised when a build has begun.
        /// </summary>
        internal event _dispBuildEvents_OnBuildBeginEventHandler BuildBegin;

        /// <summary>
        /// An event raised when an individual project build has begun.
        /// </summary>
        internal event _dispBuildEvents_OnBuildProjConfigBeginEventHandler BuildProjConfigBegin;

        /// <summary>
        /// An event raised when a build is done.
        /// </summary>
        internal event _dispBuildEvents_OnBuildDoneEventHandler BuildDone;

        #endregion Internal Events

        #region Private Properties

        /// <summary>
        /// Gets or sets a pointer to the IDE build events.
        /// </summary>
        private BuildEvents BuildEvents { get; set; }

        #endregion Private Properties

        #region Private Event Handlers

        /// <summary>
        /// Event raised when a build begins.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="action">The action.</param>
        private void BuildEvents_OnBuildBegin(vsBuildScope scope, vsBuildAction action)
        {
            if (BuildBegin != null)
            {
                BuildBegin(scope, action);
            }
        }

        /// <summary>
        /// Event raised when the build of an individual project begins.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="projectConfig">The project config.</param>
        /// <param name="platform">The platform.</param>
        /// <param name="solutionConfig">The solution config.</param>
        private void BuildEvents_OnBuildProjConfigBegin(string project, string projectConfig, string platform, string solutionConfig)
        {
            if (BuildProjConfigBegin != null)
            {
                BuildProjConfigBegin(project, projectConfig, platform, solutionConfig);
            }
        }

        /// <summary>
        /// Event raised when a build is done.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="action">The action.</param>
        private void BuildEvents_OnBuildDone(vsBuildScope scope, vsBuildAction action)
        {
            if (BuildDone != null)
            {
                BuildDone(scope, action);
            }
        }

        #endregion Private Event Handlers

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
                    BuildEvents.OnBuildDone -= BuildEvents_OnBuildDone;
                }
            }
        }

        #endregion IDisposable Members
    }
}