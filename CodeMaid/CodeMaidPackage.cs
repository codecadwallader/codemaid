using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Integration.Commands;
using SteveCadwallader.CodeMaid.Integration.Events;
using SteveCadwallader.CodeMaid.Model;
using SteveCadwallader.CodeMaid.Properties;
using SteveCadwallader.CodeMaid.UI;
using SteveCadwallader.CodeMaid.UI.ToolWindows.BuildProgress;
using SteveCadwallader.CodeMaid.UI.ToolWindows.Spade;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

namespace SteveCadwallader.CodeMaid
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio is to
    /// implement the IVsPackage interface and register itself with the shell. This package uses the
    /// helper classes defined inside the Managed Package Framework (MPF) to do it: it derives from
    /// the Package class that provides the implementation of the IVsPackage interface and uses the
    /// registration attributes defined in the framework to register itself and its components with
    /// the shell.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)] // Tells Visual Studio utilities that this is a package that needs registered.
    [InstalledProductRegistration("#110", "#112", Vsix.Version, IconResourceID = 400, LanguageIndependentName = "CodeMaid")] // VS Help/About details (Name, Description, Version, Icon).
    [ProvideAutoLoad("ADFC4E64-0397-11D1-9F4E-00A0C911004F")] // Force CodeMaid to load on startup so menu items can determine their state.
    [ProvideBindingPath]
    [ProvideMenuResource(1000, 1)] // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideToolWindow(typeof(BuildProgressToolWindow), MultiInstances = false, Height = 40, Width = 500, Style = VsDockStyle.Tabbed, Orientation = ToolWindowOrientation.Bottom, Window = EnvDTE.Constants.vsWindowKindMainWindow)]
    [ProvideToolWindow(typeof(SpadeToolWindow), MultiInstances = false, Style = VsDockStyle.Tabbed, Orientation = ToolWindowOrientation.Left, Window = EnvDTE.Constants.vsWindowKindSolutionExplorer)]
    [ProvideToolWindowVisibility(typeof(SpadeToolWindow), "{F1536EF8-92EC-443C-9ED7-FDADF150DA82}")]
    [Guid(PackageGuids.GuidCodeMaidPackageString)] // Package unique GUID.
    public sealed class CodeMaidPackage : Package, IVsInstalledProduct, IVsPackageDynamicToolOwner
    {
        #region Fields

        /// <summary>
        /// The build progress tool window.
        /// </summary>
        private BuildProgressToolWindow _buildProgress;

        /// <summary>
        /// An internal collection of the commands registered by this package.
        /// </summary>
        private readonly ICollection<BaseCommand> _commands = new List<BaseCommand>();

        /// <summary>
        /// The IComponentModel service.
        /// </summary>
        private IComponentModel _componentModel;

        /// <summary>
        /// The top level application instance of the VS IDE that is executing this package.
        /// </summary>
        private DTE2 _ide;

        /// <summary>
        /// The service provider.
        /// </summary>
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// The Spade tool window.
        /// </summary>
        private SpadeToolWindow _spade;

        /// <summary>
        /// The theme manager.
        /// </summary>
        private ThemeManager _themeManager;

        /// <summary>
        /// The Visual Studio workspace for Roslyn interaction.
        /// </summary>
        private VisualStudioWorkspace _workspace;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default constructor of the package. Inside this method you can place any initialization
        /// code that does not require any Visual Studio service because at this point the package
        /// object is created but not sited yet inside Visual Studio environment. The place to do
        /// all the other initialization is the Initialize method.
        /// </summary>
        public CodeMaidPackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this));

            if (Application.Current != null)
            {
                Application.Current.DispatcherUnhandledException += OnDispatcherUnhandledException;
            }

            // If an existing user settings file cannot be found, perform a one-time settings upgrade.
            if (!File.Exists(SettingsContextHelper.GetUserSettingsPath()))
            {
                Settings.Default.Upgrade();
                Settings.Default.Save();
            }
        }

        #endregion Constructors

        #region Public Integration Properties

        /// <summary>
        /// Gets the currently active document, otherwise null.
        /// </summary>
        public Document ActiveDocument
        {
            get
            {
                try
                {
                    return IDE.ActiveDocument;
                }
                catch (Exception)
                {
                    // If a project property page is active, accessing the ActiveDocument causes an exception.
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the build progress tool window, creating it if necessary.
        /// </summary>
        public BuildProgressToolWindow BuildProgress =>
            _buildProgress ?? (_buildProgress = (FindToolWindow(typeof(BuildProgressToolWindow), 0, true) as BuildProgressToolWindow));

        /// <summary>
        /// Gets the IComponentModel service.
        /// </summary>
        public IComponentModel ComponentModel =>
            _componentModel ?? (_componentModel = GetGlobalService(typeof(SComponentModel)) as IComponentModel);

        /// <summary>
        /// Gets the top level application instance of the VS IDE that is executing this package.
        /// </summary>
        public DTE2 IDE => _ide ?? (_ide = (DTE2)GetService(typeof(DTE)));

        /// <summary>
        /// Gets the version of the running IDE instance.
        /// </summary>
        public double IDEVersion => Convert.ToDouble(IDE.Version, CultureInfo.InvariantCulture);

        /// <summary>
        /// Gets or sets a flag indicating if CodeMaid is running inside an AutoSave context.
        /// </summary>
        public bool IsAutoSaveContext { get; set; }

        /// <summary>
        /// Gets the menu command service.
        /// </summary>
        public OleMenuCommandService MenuCommandService => GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        public IServiceProvider ServiceProvider => _serviceProvider ?? (_serviceProvider = new ServiceProvider((Microsoft.VisualStudio.OLE.Interop.IServiceProvider)IDE));

        /// <summary>
        /// Gets the Spade tool window, iff it already exists.
        /// </summary>
        public SpadeToolWindow Spade =>
            _spade ?? (_spade = (FindToolWindow(typeof(SpadeToolWindow), 0, false) as SpadeToolWindow));

        /// <summary>
        /// Gets the Spade tool window, creating it if necessary.
        /// </summary>
        public SpadeToolWindow SpadeForceLoad =>
            _spade ?? (_spade = (FindToolWindow(typeof(SpadeToolWindow), 0, true) as SpadeToolWindow));

        /// <summary>
        /// Gets the theme manager.
        /// </summary>
        public ThemeManager ThemeManager => _themeManager ?? (_themeManager = ThemeManager.GetInstance(this));

        /// <summary>
        /// Gets the Visual Studio workspace for Roslyn interaction.
        /// </summary>
        public VisualStudioWorkspace Workspace => _workspace ?? (_workspace = ComponentModel.GetService<VisualStudioWorkspace>());

        #endregion Public Integration Properties

        #region Private Event Listener Properties

        /// <summary>
        /// Gets or sets the build progress event listener.
        /// </summary>
        private BuildProgressEventListener BuildProgressEventListener { get; set; }

        /// <summary>
        /// Gets or sets the document event listener.
        /// </summary>
        private DocumentEventListener DocumentEventListener { get; set; }

        /// <summary>
        /// Gets or sets the running document table event listener.
        /// </summary>
        private RunningDocumentTableEventListener RunningDocumentTableEventListener { get; set; }

        /// <summary>
        /// Gets or sets the shell event listener.
        /// </summary>
        private ShellEventListener ShellEventListener { get; set; }

        /// <summary>
        /// Gets or sets the solution event listener.
        /// </summary>
        private SolutionEventListener SolutionEventListener { get; set; }

        /// <summary>
        /// Gets or sets the text editor event listener.
        /// </summary>
        private TextEditorEventListener TextEditorEventListener { get; set; }

        /// <summary>
        /// Gets or sets the window event listener.
        /// </summary>
        private WindowEventListener WindowEventListener { get; set; }

        /// <summary>
        /// Gets or sets the workspace event listener.
        /// </summary>
        private WorkspaceEventListener WorkspaceEventListener { get; set; }

        #endregion Private Event Listener Properties

        #region Private Service Properties

        /// <summary>
        /// Gets the shell service.
        /// </summary>
        private IVsShell ShellService => GetService(typeof(SVsShell)) as IVsShell;

        #endregion Private Service Properties

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited,
        /// so this is the place where you can put all the initialization code that rely on services
        /// provided by VisualStudio.
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
            var resourceManager = (IVsResourceManager)GetService(typeof(SVsResourceManager));
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

        #region IVsPackageDynamicToolOwner members

        /// <summary>
        /// Allows the package to control whether the tool window should be shown or hidden. This
        /// method is called by the shell when the user switches to a different window view or
        /// context, for example Design, Debugging, Full Screen, etc.
        /// </summary>
        /// <returns>
        /// If the method succeeds, it returns <see
        /// cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.
        /// </returns>
        /// <param name="rguidPersistenceSlot">[in] The GUID of the window.</param>
        /// <param name="pfShowTool">[out] true to show the window, otherwise false.</param>
        public int QueryShowTool(ref Guid rguidPersistenceSlot, out int pfShowTool)
        {
            pfShowTool = 1;

            if (rguidPersistenceSlot == PackageGuids.GuidCodeMaidToolWindowSpade)
            {
                var monitorSelection = GetService(typeof(IVsMonitorSelection)) as IVsMonitorSelection;
                if (monitorSelection != null)
                {
                    var guidCmdUI = VSConstants.UICONTEXT_FullScreenMode;
                    uint dwCmdUICookie;
                    monitorSelection.GetCmdUIContextCookie(ref guidCmdUI, out dwCmdUICookie);

                    int fActive;
                    monitorSelection.IsCmdUIContextActive(dwCmdUICookie, out fActive);
                    if (fActive == 1)
                    {
                        pfShowTool = 0;
                    }
                }
            }

            return VSConstants.S_OK;
        }

        #endregion IVsPackageDynamicToolOwner members

        #region Private Methods

        /// <summary>
        /// Called when a DispatcherUnhandledException is raised by Visual Studio.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="DispatcherUnhandledExceptionEventArgs" /> instance containing the event data.
        /// </param>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (!Settings.Default.General_DiagnosticsMode) return;

            OutputWindowHelper.ExceptionWriteLine("Diagnostics mode caught and marked as handled the following DispatcherUnhandledException raised in Visual Studio", e.Exception);
            e.Handled = true;
        }

        /// <summary>
        /// Called when a solution is closed to conditionally show the start page.
        /// </summary>
        private void OnSolutionClosedShowStartPage()
        {
            if (!Settings.Default.General_ShowStartPageOnSolutionClose) return;

            IDE.ExecuteCommand("View.StartPage");
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
                _commands.Add(new CleanupOpenCodeCommand(this));
                _commands.Add(new CleanupSelectedCodeCommand(this));
                _commands.Add(new CloseAllReadOnlyCommand(this));
                _commands.Add(new CollapseAllSolutionExplorerCommand(this));
                _commands.Add(new CollapseSelectedSolutionExplorerCommand(this));
                _commands.Add(new CommentFormatCommand(this));
                _commands.Add(new FindInSolutionExplorerCommand(this));
                _commands.Add(new JoinLinesCommand(this));
                _commands.Add(new OptionsCommand(this));
                _commands.Add(new ReadOnlyToggleCommand(this));
                _commands.Add(new RemoveRegionCommand(this));
                _commands.Add(new ReorganizeActiveCodeCommand(this));
                _commands.Add(new SettingCleanupOnSaveCommand(this));
                _commands.Add(new SortLinesCommand(this));
                _commands.Add(new SpadeContextDeleteCommand(this));
                _commands.Add(new SpadeContextFindReferencesCommand(this));
                _commands.Add(new SpadeContextInsertRegionCommand(this));
                _commands.Add(new SpadeContextRemoveRegionCommand(this));
                _commands.Add(new SpadeOptionsCommand(this));
                _commands.Add(new SpadeRefreshCommand(this));
                _commands.Add(new SpadeSearchCommand(this));
                _commands.Add(new SpadeSortOrderAlphaCommand(this));
                _commands.Add(new SpadeSortOrderFileCommand(this));
                _commands.Add(new SpadeSortOrderTypeCommand(this));
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
            ShellEventListener.EnvironmentColorChanged += () => ThemeManager.ApplyTheme();
            ShellEventListener.ShellAvailable += RegisterNonShellEventListeners;
        }

        /// <summary>
        /// Register the package event listeners.
        /// </summary>
        /// <remarks>
        /// This must occur after the DTE service is available since many of the events are based
        /// off of the DTE object.
        /// </remarks>
        private void RegisterNonShellEventListeners()
        {
            // Create event listeners and register for events.
            var menuCommandService = MenuCommandService;
            if (menuCommandService != null)
            {
                var buildProgressToolWindowCommand = _commands.OfType<BuildProgressToolWindowCommand>().First();
                var cleanupActiveCodeCommand = _commands.OfType<CleanupActiveCodeCommand>().First();
                var collapseAllSolutionExplorerCommand = _commands.OfType<CollapseAllSolutionExplorerCommand>().First();
                var spadeToolWindowCommand = _commands.OfType<SpadeToolWindowCommand>().First();

                var codeModelManager = CodeModelManager.GetInstance(this);
                var settingsContextHelper = SettingsContextHelper.GetInstance(this);

                BuildProgressEventListener = new BuildProgressEventListener(this);
                BuildProgressEventListener.BuildBegin += buildProgressToolWindowCommand.OnBuildBegin;
                BuildProgressEventListener.BuildProjConfigBegin += buildProgressToolWindowCommand.OnBuildProjConfigBegin;
                BuildProgressEventListener.BuildProjConfigDone += buildProgressToolWindowCommand.OnBuildProjConfigDone;
                BuildProgressEventListener.BuildDone += buildProgressToolWindowCommand.OnBuildDone;

                DocumentEventListener = new DocumentEventListener(this);
                DocumentEventListener.OnDocumentClosing += codeModelManager.OnDocumentClosing;

                RunningDocumentTableEventListener = new RunningDocumentTableEventListener(this);
                RunningDocumentTableEventListener.BeforeSave += cleanupActiveCodeCommand.OnBeforeDocumentSave;
                RunningDocumentTableEventListener.AfterSave += spadeToolWindowCommand.OnAfterDocumentSave;

                SolutionEventListener = new SolutionEventListener(this);
                SolutionEventListener.OnSolutionOpened += collapseAllSolutionExplorerCommand.OnSolutionOpened;
                SolutionEventListener.OnSolutionOpened += settingsContextHelper.OnSolutionOpened;
                SolutionEventListener.OnSolutionClosed += settingsContextHelper.OnSolutionClosed;
                SolutionEventListener.OnSolutionClosed += OnSolutionClosedShowStartPage;

                // Check if a solution has already been opened before CodeMaid was initialized.
                if (IDE.Solution != null && IDE.Solution.IsOpen)
                {
                    collapseAllSolutionExplorerCommand.OnSolutionOpened();
                }

                TextEditorEventListener = new TextEditorEventListener(this);
                TextEditorEventListener.OnLineChanged += codeModelManager.OnDocumentChanged;

                WindowEventListener = new WindowEventListener(this);
                WindowEventListener.OnWindowChange += spadeToolWindowCommand.OnWindowChange;

                WorkspaceEventListener = new WorkspaceEventListener(this);
            }
        }

        #endregion Private Methods

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
            base.Dispose(disposing);

            // Dispose of any event listeners.
            BuildProgressEventListener?.Dispose();
            DocumentEventListener?.Dispose();
            RunningDocumentTableEventListener?.Dispose();
            ShellEventListener?.Dispose();
            SolutionEventListener?.Dispose();
            TextEditorEventListener?.Dispose();
            WindowEventListener?.Dispose();
            WorkspaceEventListener?.Dispose();
        }

        #endregion IDisposable Members
    }
}