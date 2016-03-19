using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Integration.Events
{
    /// <summary>
    /// A class that encapsulates listening for build progress events.
    /// </summary>
    internal class BuildProgressEventListener : BaseEventListener
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildProgressEventListener" /> class.
        /// </summary>
        /// <param name="package">The package hosting the event listener.</param>
        internal BuildProgressEventListener(CodeMaidPackage package)
            : base(package)
        {
            // Store access to the build events, otherwise events will not register properly via DTE.
            BuildEvents = Package.IDE.Events.BuildEvents;
            BuildEvents.OnBuildBegin += BuildEvents_OnBuildBegin;
            BuildEvents.OnBuildProjConfigBegin += BuildEvents_OnBuildProjConfigBegin;
            BuildEvents.OnBuildProjConfigDone += BuildEvents_OnBuildProjConfigDone;
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
        /// An event raised when an individual project build is done.
        /// </summary>
        internal event _dispBuildEvents_OnBuildProjConfigDoneEventHandler BuildProjConfigDone;

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
            var buildBegin = BuildBegin;
            if (buildBegin != null)
            {
                OutputWindowHelper.DiagnosticWriteLine("BuildProgressEventListener.BuildBegin raised");

                buildBegin(scope, action);
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
            var buildProjConfigBegin = BuildProjConfigBegin;
            if (buildProjConfigBegin != null)
            {
                OutputWindowHelper.DiagnosticWriteLine("BuildProgressEventListener.BuildProjConfigBegin raised");

                buildProjConfigBegin(project, projectConfig, platform, solutionConfig);
            }
        }

        /// <summary>
        /// Event raised when the build of an individual project is done.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="projectConfig">The project config.</param>
        /// <param name="platform">The platform.</param>
        /// <param name="solutionConfig">The solution config.</param>
        /// <param name="success">True if project build was successful, otherwise false.</param>
        private void BuildEvents_OnBuildProjConfigDone(string project, string projectConfig, string platform, string solutionConfig, bool success)
        {
            var buildProjConfigDone = BuildProjConfigDone;
            if (buildProjConfigDone != null)
            {
                OutputWindowHelper.DiagnosticWriteLine("BuildProgressEventListener.BuildProjConfigDone raised");

                buildProjConfigDone(project, projectConfig, platform, solutionConfig, success);
            }
        }

        /// <summary>
        /// Event raised when a build is done.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="action">The action.</param>
        private void BuildEvents_OnBuildDone(vsBuildScope scope, vsBuildAction action)
        {
            var buildDone = BuildDone;
            if (buildDone != null)
            {
                OutputWindowHelper.DiagnosticWriteLine("BuildProgressEventListener.BuildDone raised");

                buildDone(scope, action);
            }
        }

        #endregion Private Event Handlers

        #region IDisposable Members

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
        /// only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;

                if (disposing && BuildEvents != null)
                {
                    BuildEvents.OnBuildBegin -= BuildEvents_OnBuildBegin;
                    BuildEvents.OnBuildProjConfigBegin -= BuildEvents_OnBuildProjConfigBegin;
                    BuildEvents.OnBuildProjConfigDone -= BuildEvents_OnBuildProjConfigDone;
                    BuildEvents.OnBuildDone -= BuildEvents_OnBuildDone;
                }
            }
        }

        #endregion IDisposable Members
    }
}