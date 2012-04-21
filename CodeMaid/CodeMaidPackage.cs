#region CodeMaid is Copyright 2007-2012 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2012 Steve Cadwallader.

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SteveCadwallader.CodeMaid.BuildProgress;
using SteveCadwallader.CodeMaid.Commands;
using SteveCadwallader.CodeMaid.Events;
using SteveCadwallader.CodeMaid.Options;
using SteveCadwallader.CodeMaid.Spade;

namespace SteveCadwallader.CodeMaid
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)] // Tells Visual Studio utilities that this is a package that needs registered.
    [InstalledProductRegistration("#110", "#112", "#114", IconResourceID = 400, LanguageIndependentName = "CodeMaid")] // VS Help/About details (Name, Description, Version, Icon).
    [ProvideLoadKey("Standard", "0.4.2", "CodeMaid", "Steve Cadwallader", 1)]
    [ProvideAutoLoad("ADFC4E64-0397-11D1-9F4E-00A0C911004F")] // Force CodeMaid to load on startup so menu items can determine their state.
    [ProvideMenuResource(1000, 1)] // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideOptionPage(typeof(CleanupFileTypesOptionsPage), "CodeMaid", @"Cleanup\File Types", 116, 132, true)]
    [ProvideOptionPage(typeof(CleanupGeneralOptionsPage), "CodeMaid", @"Cleanup\General", 116, 118, true)]
    [ProvideOptionPage(typeof(CleanupInsertOptionsPage), "CodeMaid", @"Cleanup\Insert", 116, 120, true)]
    [ProvideOptionPage(typeof(CleanupRemoveOptionsPage), "CodeMaid", @"Cleanup\Remove", 116, 122, true)]
    [ProvideOptionPage(typeof(CleanupUpdateOptionsPage), "CodeMaid", @"Cleanup\Update", 116, 124, true)]
    [ProvideOptionPage(typeof(BuildProgressOptionsPage), "CodeMaid", "Build Progress", 116, 126, true)]
    [ProvideOptionPage(typeof(ReorganizeOptionsPage), "CodeMaid", "Reorganize", 116, 134, true)]
    [ProvideOptionPage(typeof(SpadeOptionsPage), "CodeMaid", "Spade", 116, 128, true)]
    [ProvideOptionPage(typeof(SwitchFileOptionsPage), "CodeMaid", "Switch File", 116, 130, true)]
    [ProvideToolWindow(typeof(BuildProgressToolWindow), MultiInstances = false, Height = 40, Width = 500, Style = VsDockStyle.Tabbed, Orientation = ToolWindowOrientation.Bottom, Window = EnvDTE.Constants.vsWindowKindMainWindow)]
    [ProvideToolWindow(typeof(SpadeToolWindow), MultiInstances = false, Style = VsDockStyle.Tabbed, Orientation = ToolWindowOrientation.Left, Window = EnvDTE.Constants.vsWindowKindSolutionExplorer)]
    [ProvideToolWindowVisibility(typeof(SpadeToolWindow), "{F1536EF8-92EC-443C-9ED7-FDADF150DA82}")]
    [Guid(GuidList.GuidCodeMaidPackageString)] // Package unique GUID.
    public sealed class CodeMaidPackage : Package, IVsInstalledProduct
    {
        #region Constructors

        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require
        /// any Visual Studio service because at this point the package object is created but
        /// not sited yet inside Visual Studio environment. The place to do all the other
        /// initialization is the Initialize method.
        /// </summary>
        public CodeMaidPackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this));
        }

        #endregion Constructors

        #region Public Integration Properties

        /// <summary>
        /// Gets the build progress tool window, creating it if necessary.
        /// </summary>
        public BuildProgressToolWindow BuildProgress
        {
            get
            {
                return _buildProgress ??
                    (_buildProgress = (FindToolWindow(typeof(BuildProgressToolWindow), 0, true) as BuildProgressToolWindow));
            }
        }

        /// <summary>
        /// Gets the top level application instance of the VS IDE that is executing this package.
        /// </summary>
        public DTE2 IDE
        {
            get { return _ide ?? (_ide = (DTE2)GetService(typeof(DTE))); }
        }

        /// <summary>
        /// Gets the version of the running IDE instance.
        /// </summary>
        public double IDEVersion { get { return Convert.ToDouble(IDE.Version, CultureInfo.InvariantCulture); } }

        /// <summary>
        /// Gets the configuration options.
        /// </summary>
        public OptionsWrapper Options
        {
            get
            {
                return _optionsWrapper ??
                       (_optionsWrapper = new OptionsWrapper(GetOptionsPage<BuildProgressOptionsPage>(),
                                                             GetOptionsPage<CleanupFileTypesOptionsPage>(),
                                                             GetOptionsPage<CleanupGeneralOptionsPage>(),
                                                             GetOptionsPage<CleanupInsertOptionsPage>(),
                                                             GetOptionsPage<CleanupRemoveOptionsPage>(),
                                                             GetOptionsPage<CleanupUpdateOptionsPage>(),
                                                             GetOptionsPage<ReorganizeOptionsPage>(),
                                                             GetOptionsPage<SpadeOptionsPage>(),
                                                             GetOptionsPage<SwitchFileOptionsPage>()));
            }
        }

        /// <summary>
        /// Gets the Spade tool window, iff it already exists.
        /// </summary>
        public SpadeToolWindow Spade
        {
            get
            {
                return _spade ??
                    (_spade = (FindToolWindow(typeof(SpadeToolWindow), 0, false) as SpadeToolWindow));
            }
        }

        /// <summary>
        /// Gets the Spade tool window, creating it if necessary.
        /// </summary>
        public SpadeToolWindow SpadeForceLoad
        {
            get
            {
                return _spade ??
                    (_spade = (FindToolWindow(typeof(SpadeToolWindow), 0, true) as SpadeToolWindow));
            }
        }

        /// <summary>
        /// Gets a flag indicating if POSIX regular expressions should be used for TextDocument Find/Replace actions.
        /// Applies to pre-Visual Studio 11 versions.
        /// </summary>
        public bool UsePOSIXRegEx
        {
            get { return IDEVersion < 11; }
        }

        #endregion Public Integration Properties

        #region Private Event Listener Properties

        /// <summary>
        /// Gets or sets the build progress event listener.
        /// </summary>
        private BuildProgressEventListener BuildProgressEventListener { get; set; }

        /// <summary>
        /// Gets or sets the running document table event listener.
        /// </summary>
        private RunningDocumentTableEventListener RunningDocumentTableEventListener { get; set; }

        /// <summary>
        /// Gets or sets the shell event listener.
        /// </summary>
        private ShellEventListener ShellEventListener { get; set; }

        /// <summary>
        /// Gets or sets the window event listener.
        /// </summary>
        private WindowEventListener WindowEventListener { get; set; }

        #endregion Private Event Listener Properties

        #region Private Service Properties

        /// <summary>
        /// Gets the menu command service.
        /// </summary>
        private OleMenuCommandService MenuCommandService
        {
            get { return GetService(typeof(IMenuCommandService)) as OleMenuCommandService; }
        }

        /// <summary>
        /// Gets the shell service.
        /// </summary>
        private IVsShell ShellService
        {
            get { return GetService(typeof(SVsShell)) as IVsShell; }
        }

        #endregion Private Service Properties

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this));
            base.Initialize();

            RegisterCommands();
            RegisterShellEventListener();
        }

        #endregion Package Members

        #region IVsInstalledProduct Members

        public int IdBmpSplash(out uint pIdBmp)
        {
            pIdBmp = 400;
            return VSConstants.S_OK;
        }

        public int IdIcoLogoForAboutbox(out uint pIdIco)
        {
            pIdIco = 400;
            return VSConstants.S_OK;
        }

        public int OfficialName(out string pbstrName)
        {
            pbstrName = GetResourceString("@110");
            return VSConstants.S_OK;
        }

        public int ProductDetails(out string pbstrProductDetails)
        {
            pbstrProductDetails = GetResourceString("@112");
            return VSConstants.S_OK;
        }

        public int ProductID(out string pbstrPID)
        {
            pbstrPID = GetResourceString("@114");
            return VSConstants.S_OK;
        }

        public string GetResourceString(string resourceName)
        {
            string resourceValue;
            IVsResourceManager resourceManager = (IVsResourceManager)GetService(typeof(SVsResourceManager));
            if (resourceManager == null)
            {
                throw new InvalidOperationException(
                    "Could not get SVsResourceManager service. Make sure that the package is sited before calling this method");
            }

            Guid packageGuid = GetType().GUID;
            int hr = resourceManager.LoadResourceString(
                ref packageGuid, -1, resourceName, out resourceValue);
            ErrorHandler.ThrowOnFailure(hr);

            return resourceValue;
        }

        #endregion IVsInstalledProduct Members

        #region Private Methods

        /// <summary>
        /// Gets the specified options page.
        /// </summary>
        /// <typeparam name="T">The type of the options page to retrieve.</typeparam>
        /// <returns>The retrieved options page.</returns>
        private T GetOptionsPage<T>()
            where T : DialogPage
        {
            return (T)GetDialogPage(typeof(T));
        }

        /// <summary>
        /// Register the package commands (which must exist in the .vsct file).
        /// </summary>
        private void RegisterCommands()
        {
            var menuCommandService = MenuCommandService;
            if (menuCommandService != null)
            {
                // Create the individual commands, which internally register for command events.
                _commands.Add(new AboutCommand(this));
                _commands.Add(new BuildProgressToolWindowCommand(this));
                _commands.Add(new CleanupActiveCodeCommand(this));
                _commands.Add(new CleanupAllCodeCommand(this));
                _commands.Add(new CleanupSelectedCodeCommand(this));
                _commands.Add(new CloseAllReadOnlyCommand(this));
                _commands.Add(new CollapseAllSolutionExplorerCommand(this));
                _commands.Add(new CollapseSelectedSolutionExplorerCommand(this));
                _commands.Add(new ConfigurationCommand(this));
                _commands.Add(new FindInSolutionExplorerCommand(this));
                _commands.Add(new JoinLinesCommand(this));
                _commands.Add(new ReadOnlyToggleCommand(this));
                _commands.Add(new ReorganizeActiveCodeCommand(this));
                _commands.Add(new SpadeConfigurationCommand(this));
                _commands.Add(new SpadeLayoutAlphaCommand(this));
                _commands.Add(new SpadeLayoutFileCommand(this));
                _commands.Add(new SpadeLayoutTypeCommand(this));
                _commands.Add(new SpadeRefreshCommand(this));
                _commands.Add(new SpadeToolWindowCommand(this));
                _commands.Add(new SwitchFileCommand(this));

                // Add all commands to the menu command service.
                foreach (var command in _commands)
                {
                    menuCommandService.AddCommand(command);
                }
            }
        }

        /// <summary>
        /// Registers the shell event listener.
        /// </summary>
        /// <remarks>
        /// This event listener is registered by itself and first to wait for the shell to be ready
        /// for other event listeners to be registered.
        /// </remarks>
        private void RegisterShellEventListener()
        {
            ShellEventListener = new ShellEventListener(this, ShellService);
            ShellEventListener.ShellAvailable += RegisterNonShellEventListeners;
        }

        /// <summary>
        /// Register the package event listeners.
        /// </summary>
        /// <remarks>
        /// This must occur after the DTE service is available since many of the events
        /// are based off of the DTE object.
        /// </remarks>
        private void RegisterNonShellEventListeners()
        {
            // Create event listeners and register for events.
            var menuCommandService = MenuCommandService;
            if (menuCommandService != null)
            {
                var buildProgressToolWindowCommand = _commands.OfType<BuildProgressToolWindowCommand>().First();
                var cleanupActiveCodeCommand = _commands.OfType<CleanupActiveCodeCommand>().First();
                var spadeToolWindowCommand = _commands.OfType<SpadeToolWindowCommand>().First();

                BuildProgressEventListener = new BuildProgressEventListener(this);
                BuildProgressEventListener.BuildBegin += buildProgressToolWindowCommand.OnBuildBegin;
                BuildProgressEventListener.BuildProjConfigBegin += buildProgressToolWindowCommand.OnBuildProjConfigBegin;
                BuildProgressEventListener.BuildDone += buildProgressToolWindowCommand.OnBuildDone;

                RunningDocumentTableEventListener = new RunningDocumentTableEventListener(this);
                RunningDocumentTableEventListener.BeforeSave += cleanupActiveCodeCommand.OnBeforeDocumentSave;
                RunningDocumentTableEventListener.AfterSave += spadeToolWindowCommand.OnAfterDocumentSave;

                WindowEventListener = new WindowEventListener(this);
                WindowEventListener.OnWindowChange += spadeToolWindowCommand.OnWindowChange;
            }
        }

        #endregion Private Methods

        #region IDisposable Members

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            // Dispose of any event listeners.
            if (BuildProgressEventListener != null)
            {
                BuildProgressEventListener.Dispose();
            }

            if (RunningDocumentTableEventListener != null)
            {
                RunningDocumentTableEventListener.Dispose();
            }

            if (ShellEventListener != null)
            {
                ShellEventListener.Dispose();
            }

            if (WindowEventListener != null)
            {
                WindowEventListener.Dispose();
            }
        }

        #endregion IDisposable Members

        #region Private Fields

        /// <summary>
        /// The build progress tool window.
        /// </summary>
        private BuildProgressToolWindow _buildProgress;

        /// <summary>
        /// An internal collection of the commands registered by this package.
        /// </summary>
        private readonly ICollection<BaseCommand> _commands = new List<BaseCommand>();

        /// <summary>
        /// The top level application instance of the VS IDE that is executing this package.
        /// </summary>
        private DTE2 _ide;

        /// <summary>
        /// A wrapper for the options.
        /// </summary>
        private OptionsWrapper _optionsWrapper;

        /// <summary>
        /// The Spade tool window.
        /// </summary>
        private SpadeToolWindow _spade;

        #endregion Private Fields
    }
}