using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
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
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;
using VSColorTheme = Microsoft.VisualStudio.PlatformUI.VSColorTheme;

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
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExistsAndFullyLoaded_string)] // Force CodeMaid to load so menu items can determine their state.
    [ProvideBindingPath]
    [ProvideMenuResource(1000, 1)] // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideToolWindow(typeof(BuildProgressToolWindow), MultiInstances = false, Height = 40, Width = 500, Style = VsDockStyle.Tabbed, Orientation = ToolWindowOrientation.Bottom, Window = EnvDTE.Constants.vsWindowKindMainWindow)]
    [ProvideToolWindow(typeof(SpadeToolWindow), MultiInstances = false, Style = VsDockStyle.Tabbed, Orientation = ToolWindowOrientation.Left, Window = EnvDTE.Constants.vsWindowKindSolutionExplorer)]
    [Guid(PackageGuids.GuidCodeMaidPackageString)] // Package unique GUID.
    public sealed class CodeMaidPackage : Package, IVsInstalledProduct
    {
        #region Fields

        /// <summary>
        /// The build progress tool window.
        /// </summary>
        private BuildProgressToolWindow _buildProgress;

        /// <summary>
        /// The IComponentModel service.
        /// </summary>
        private IComponentModel _componentModel;

        /// <summary>
        /// The top level application instance of the VS IDE that is executing this package.
        /// </summary>
        private DTE2 _ide;

        /// <summary>
        /// The Spade tool window.
        /// </summary>
        private SpadeToolWindow _spade;

        /// <summary>
        /// The theme manager.
        /// </summary>
        private ThemeManager _themeManager;

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

            SettingMonitor = new SettingMonitor<Settings>(Settings.Default);
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
        /// Gets the build progress tool window, if it already exists.
        /// </summary>
        public BuildProgressToolWindow BuildProgress =>
            _buildProgress ?? (_buildProgress = (FindToolWindow(typeof(BuildProgressToolWindow), 0, false) as BuildProgressToolWindow));

        /// <summary>
        /// Gets the build progress tool window, creating it if necessary.
        /// </summary>
        public BuildProgressToolWindow BuildProgressForceLoad =>
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
        /// Gets the Spade tool window, if it already exists.
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

        public SettingMonitor<Settings> SettingMonitor { get; }

        #endregion Public Integration Properties

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
            RegisterEventListeners();
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
            // Initialize the individual commands, which internally register for command events.
            AboutCommand.Initialize(this);
            BuildProgressToolWindowCommand.Initialize(this);
            CleanupActiveCodeCommand.Initialize(this);
            CleanupAllCodeCommand.Initialize(this);
            CleanupOpenCodeCommand.Initialize(this);
            CleanupSelectedCodeCommand.Initialize(this);
            CloseAllReadOnlyCommand.Initialize(this);
            CollapseAllSolutionExplorerCommand.Initialize(this);
            CollapseSelectedSolutionExplorerCommand.Initialize(this);
            CommentFormatCommand.Initialize(this);
            FindInSolutionExplorerCommand.Initialize(this);
            JoinLinesCommand.Initialize(this);
            OptionsCommand.Initialize(this);
            ReadOnlyToggleCommand.Initialize(this);
            RemoveRegionCommand.Initialize(this);
            ReorganizeActiveCodeCommand.Initialize(this);
            SettingCleanupOnSaveCommand.Initialize(this);
            SortLinesCommand.Initialize(this);
            SpadeContextDeleteCommand.Initialize(this);
            SpadeContextFindReferencesCommand.Initialize(this);
            SpadeContextInsertRegionCommand.Initialize(this);
            SpadeContextRemoveRegionCommand.Initialize(this);
            SpadeOptionsCommand.Initialize(this);
            SpadeRefreshCommand.Initialize(this);
            SpadeSearchCommand.Initialize(this);
            SpadeSortOrderAlphaCommand.Initialize(this);
            SpadeSortOrderFileCommand.Initialize(this);
            SpadeSortOrderTypeCommand.Initialize(this);
            SpadeToolWindowCommand.Initialize(this);
            SwitchFileCommand.Initialize(this);
        }

        /// <summary>
        /// Register the package event listeners.
        /// </summary>
        /// <remarks>
        /// Every event listener registers VS events by itself.
        /// </remarks>
        private void RegisterEventListeners()
        {
            var codeModelManager = CodeModelManager.GetInstance(this);
            var settingsContextHelper = SettingsContextHelper.GetInstance(this);

            VSColorTheme.ThemeChanged += _ => ThemeManager.ApplyTheme();

            BuildProgressEventListener.Intialize(this);
            BuildProgressEventListener.Instance.BuildBegin += BuildProgressToolWindowCommand.Instance.OnBuildBegin;
            BuildProgressEventListener.Instance.BuildProjConfigBegin += BuildProgressToolWindowCommand.Instance.OnBuildProjConfigBegin;
            BuildProgressEventListener.Instance.BuildProjConfigDone += BuildProgressToolWindowCommand.Instance.OnBuildProjConfigDone;
            BuildProgressEventListener.Instance.BuildDone += BuildProgressToolWindowCommand.Instance.OnBuildDone;

            DocumentEventListener.Intialize(this);
            DocumentEventListener.Instance.OnDocumentClosing += codeModelManager.OnDocumentClosing;

            RunningDocumentTableEventListener.Intialize(this);
            SettingMonitor.Watch(s => s.Feature_SettingCleanupOnSave, on =>
            {
                if (on)
                    RunningDocumentTableEventListener.Instance.BeforeSave += CleanupActiveCodeCommand.Instance.OnBeforeDocumentSave;
                else
                    RunningDocumentTableEventListener.Instance.BeforeSave -= CleanupActiveCodeCommand.Instance.OnBeforeDocumentSave;
            });
            SettingMonitor.Watch(s => s.Feature_SpadeToolWindow, on =>
            {
                if (on)
                    RunningDocumentTableEventListener.Instance.AfterSave += SpadeToolWindowCommand.Instance.OnAfterDocumentSave;
                else
                    RunningDocumentTableEventListener.Instance.AfterSave -= SpadeToolWindowCommand.Instance.OnAfterDocumentSave;
            });

            SolutionEventListener.Intialize(this);
            SettingMonitor.Watch(s => s.Feature_CollapseAllSolutionExplorer, on =>
            {
                if (on)
                    SolutionEventListener.Instance.OnSolutionOpened += CollapseAllSolutionExplorerCommand.Instance.OnSolutionOpened;
                else
                    SolutionEventListener.Instance.OnSolutionOpened -= CollapseAllSolutionExplorerCommand.Instance.OnSolutionOpened;
            });
            SolutionEventListener.Instance.OnSolutionOpened += settingsContextHelper.OnSolutionOpened;
            SolutionEventListener.Instance.OnSolutionClosed += settingsContextHelper.OnSolutionClosed;
            SolutionEventListener.Instance.OnSolutionClosed += OnSolutionClosedShowStartPage;

            TextEditorEventListener.Intialize(this);
            TextEditorEventListener.Instance.OnLineChanged += codeModelManager.OnDocumentChanged;

            WindowEventListener.Intialize(this);
            WindowEventListener.Instance.OnWindowChange += SpadeToolWindowCommand.Instance.OnWindowChange;

            // Check if a solution has already been opened before CodeMaid was initialized.
            if (IDE.Solution != null && IDE.Solution.IsOpen)
            {
                SolutionEventListener.Instance.FireSolutionOpenedEvent();
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
            BuildProgressEventListener.Instance.Dispose();
            DocumentEventListener.Instance.Dispose();
            RunningDocumentTableEventListener.Instance.Dispose();
            SolutionEventListener.Instance.Dispose();
            TextEditorEventListener.Instance.Dispose();
            WindowEventListener.Instance.Dispose();
        }

        #endregion IDisposable Members
    }
}