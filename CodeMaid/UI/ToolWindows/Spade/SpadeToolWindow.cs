using EnvDTE;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SteveCadwallader.CodeMaid.Model;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Model.CodeTree;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;
using CodeModel = SteveCadwallader.CodeMaid.Model.CodeModel;

namespace SteveCadwallader.CodeMaid.UI.ToolWindows.Spade
{
    /// <summary>
    /// The Spade tool window pane.
    /// </summary>
    [Guid(PackageGuids.GuidCodeMaidToolWindowSpadeString)]
    public sealed class SpadeToolWindow : ToolWindowPane, IVsWindowFrameNotify3
    {
        #region Fields

        private readonly SpadeViewModel _viewModel;

        private CodeModelManager _codeModelManager;

        private Document _document;
        private bool _isVisible;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeToolWindow" /> class.
        /// </summary>
        public SpadeToolWindow()
            : base(null)
        {
            // Set the tool window caption.
            Caption = "CodeMaid Spade";

            // Set the tool window image from resources.
            BitmapResourceID = 508;
            BitmapIndex = 0;

            // Create the toolbar for the tool window.
            ToolBar = new CommandID(PackageGuids.GuidCodeMaidToolbarSpadeBaseGroup, PackageIds.ToolbarIDCodeMaidToolbarSpade);

            // Setup the associated classes.
            _viewModel = new SpadeViewModel { SortOrder = (CodeSortOrder)Settings.Default.Digging_PrimarySortOrder };

            // Register for view model requests to be refreshed.
            _viewModel.RequestingRefresh += (sender, args) => Refresh();

            // Create and set the view.
            Content = new SpadeView { DataContext = _viewModel };

            // Register for changes to settings.
            Settings.Default.SettingsLoaded += (sender, args) => OnSettingsChange();
            Settings.Default.SettingsSaving += (sender, args) => OnSettingsChange();
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the current document.
        /// </summary>
        public Document Document
        {
            get { return _document; }
            private set
            {
                if (_document != value)
                {
                    _document = value;
                    ConditionallyUpdateCodeModel(false);
                }
            }
        }

        /// <summary>
        /// Gets or sets the name filter.
        /// </summary>
        public string NameFilter
        {
            get { return _viewModel.NameFilter; }
            set { _viewModel.NameFilter = value; }
        }

        /// <summary>
        /// Gets the selected items.
        /// </summary>
        public IEnumerable<BaseCodeItem> SelectedItems => _viewModel.SelectedItems;

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        public CodeSortOrder SortOrder
        {
            get { return _viewModel.SortOrder; }
            set
            {
                if (_viewModel.SortOrder != value)
                {
                    Settings.Default.Digging_PrimarySortOrder = (int)value;
                    Settings.Default.Save();
                }
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// A method to be called to notify the tool window about the current active document.
        /// </summary>
        /// <param name="document">The active document.</param>
        public void NotifyActiveDocument(Document document)
        {
            Document = document;
        }

        /// <summary>
        /// A method to be called to notify the tool window that has a document has been saved.
        /// </summary>
        /// <param name="document">The document.</param>
        public void NotifyDocumentSave(Document document)
        {
            if (Document == document)
            {
                // Refresh the document if active.
                Refresh();
            }
        }

        /// <summary>
        /// This method can be overriden by the derived class to execute any code that needs to run
        /// after the IVsWindowFrame is created. If the toolwindow has a toolbar with a combobox, it
        /// should make sure its command handler are set by the time they return from this method.
        /// This is called when someone set the Frame property.
        /// </summary>
        public override void OnToolWindowCreated()
        {
            base.OnToolWindowCreated();

            // Register for events to this window.
            ((IVsWindowFrame)Frame).SetProperty((int)__VSFPROPID.VSFPROPID_ViewHelper, this);

            // Package is not available at constructor time.
            if (Package != null)
            {
                // Get an instance of the code model manager.
                _codeModelManager = CodeModelManager.GetInstance(Package);
                _codeModelManager.CodeModelBuilt += OnCodeModelBuilt;

                // Pass the package over to the view model.
                _viewModel.Package = Package;

                // Attempt to initialize the Document, may have been set before Spade was created.
                if (Document == null)
                {
                    Document = Package.ActiveDocument;
                }

                var spadeContent = Content as FrameworkElement;
                if (spadeContent != null)
                {
                    _viewModel.Dispatcher = spadeContent.Dispatcher;

                    spadeContent.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() => Package.ThemeManager.ApplyTheme()));
                }
            }
        }

        /// <summary>
        /// Refresh the Spade tool window.
        /// </summary>
        public void Refresh()
        {
            Package?.ThemeManager.ApplyTheme();

            ConditionallyUpdateCodeModel(true);
        }

        #endregion Public Methods

        #region Private Properties

        /// <summary>
        /// Gets or sets a flag indicating if this tool window is visible.
        /// </summary>
        private bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    ConditionallyUpdateCodeModel(false);
                }
            }
        }

        /// <summary>
        /// Gets or sets the package that owns the tool window.
        /// </summary>
        private new CodeMaidPackage Package => base.Package as CodeMaidPackage;

        #endregion Private Properties

        #region Private Methods

        /// <summary>
        /// Conditionally updates the code model.
        /// </summary>
        /// <param name="isRefresh">True if refreshing a document, otherwise false.</param>
        private void ConditionallyUpdateCodeModel(bool isRefresh)
        {
            if (!IsVisible) return;

            _viewModel.Document = Document;
            _viewModel.IsLoading = false;
            _viewModel.IsRefreshing = false;

            if (Document == null || !isRefresh)
            {
                _viewModel.RawCodeItems = null;
            }

            if (Document != null)
            {
                if (isRefresh)
                {
                    _codeModelManager.OnDocumentChanged(Document);
                    _viewModel.IsRefreshing = true;
                }
                else
                {
                    _viewModel.IsLoading = true;
                }

                var codeItems = _codeModelManager.RetrieveAllCodeItemsAsync(Document, true);
                if (codeItems != null)
                {
                    UpdateViewModelRawCodeItems(codeItems);
                }
            }
        }

        /// <summary>
        /// An event handler called when the <see cref="CodeModelManager" /> raises a <see
        /// cref="CodeModelManager.CodeModelBuilt" /> event. If the code model was built for the
        /// document currently being shown by Spade, the raw code items will be processed and displayed.
        /// </summary>
        /// <param name="codeModel">The code model.</param>
        private void OnCodeModelBuilt(CodeModel codeModel)
        {
            if (Document == codeModel.Document)
            {
                UpdateViewModelRawCodeItems(codeModel.CodeItems);
            }
        }

        /// <summary>
        /// An event handler called when settings are changed.
        /// </summary>
        private void OnSettingsChange()
        {
            _viewModel.SortOrder = (CodeSortOrder)Settings.Default.Digging_PrimarySortOrder;
            Refresh();
        }

        /// <summary>
        /// Update the view model's raw set of code items based on the specified code items.
        /// </summary>
        /// <param name="codeItems">The code items.</param>
        private void UpdateViewModelRawCodeItems(SetCodeItems codeItems)
        {
            // Create a copy of the original collection, filtering out undesired items.
            var filteredCodeItems = new SetCodeItems(
                codeItems.Where(x => !(x is CodeItemUsingStatement || x is CodeItemNamespace)));

            _viewModel.RawCodeItems = filteredCodeItems;
            _viewModel.IsLoading = false;
            _viewModel.IsRefreshing = false;
        }

        #endregion Private Methods

        #region IVsWindowFrameNotify3 Members

        public int OnClose(ref uint pgrfSaveOptions)
        {
            return VSConstants.S_OK;
        }

        public int OnDockableChange(int fDockable, int x, int y, int w, int h)
        {
            return VSConstants.S_OK;
        }

        public int OnMove(int x, int y, int w, int h)
        {
            return VSConstants.S_OK;
        }

        public int OnShow(int fShow)
        {
            // Track the visibility of this tool window.
            switch ((__FRAMESHOW)fShow)
            {
                case __FRAMESHOW.FRAMESHOW_WinShown:
                    IsVisible = true;
                    break;

                case __FRAMESHOW.FRAMESHOW_WinHidden:
                    IsVisible = false;
                    break;
            }

            return VSConstants.S_OK;
        }

        public int OnSize(int x, int y, int w, int h)
        {
            return VSConstants.S_OK;
        }

        #endregion IVsWindowFrameNotify3 Members

        #region IVsWindowSearch Members

        public override bool SearchEnabled => true;

        public override IVsSearchTask CreateSearch(uint dwCookie, IVsSearchQuery pSearchQuery, IVsSearchCallback pSearchCallback)
        {
            return new MemberSearchTask(dwCookie, pSearchQuery, pSearchCallback, x => NameFilter = x);
        }

        public override void ClearSearch()
        {
            NameFilter = null;
        }

        public override void ProvideSearchSettings(IVsUIDataSource pSearchSettings)
        {
            base.ProvideSearchSettings(pSearchSettings);

            Utilities.SetValue(pSearchSettings, SearchSettingsDataSource.PropertyNames.ControlMinWidth, 200U);
            Utilities.SetValue(pSearchSettings, SearchSettingsDataSource.PropertyNames.ControlMaxWidth, uint.MaxValue);
            Utilities.SetValue(pSearchSettings, SearchSettingsDataSource.PropertyNames.SearchWatermark, "Search CodeMaid Spade (Ctrl+M, ;)");
        }

        #endregion IVsWindowSearch Members
    }
}