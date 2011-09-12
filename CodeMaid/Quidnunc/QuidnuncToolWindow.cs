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

using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SteveCadwallader.CodeMaid.CodeItems;

namespace SteveCadwallader.CodeMaid.Quidnunc
{
    /// <summary>
    /// The quidnunc tool window pane.
    /// </summary>
    [Guid("75d09b86-471e-4b30-8720-362d13ad0a45")]
    public class QuidnuncToolWindow : ToolWindowPane, IVsWindowFrameNotify3
    {
        #region Fields

        private readonly QuidnuncCodeModelRetriever _codeModelRetriever;
        private readonly QuidnuncViewHost _viewHost;
        private readonly QuidnuncViewModel _viewModel;

        private Document _document;
        private bool _isVisible;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QuidnuncToolWindow"/> class.
        /// </summary>
        public QuidnuncToolWindow() :
            base(null)
        {
            // Set the tool window caption.
            Caption = "CodeMaid Quidnunc";

            // Set the tool window image from resources.
            BitmapResourceID = 501;
            BitmapIndex = 0;

            // Create the toolbar for the tool window.
            ToolBar = new CommandID(GuidList.GuidCodeMaidToolbarQuidnuncBaseGroup, PkgCmdIDList.ToolbarIDCodeMaidToolbarQuidnunc);

            // Setup the associated classes.
            _codeModelRetriever = new QuidnuncCodeModelRetriever(UpdateViewModelRawCodeItems);
            _viewModel = new QuidnuncViewModel();
            _viewHost = new QuidnuncViewHost(_viewModel);
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the interaction mode.
        /// </summary>
        public QuidnuncInteractionMode InteractionMode
        {
            get { return _viewModel.InteractionMode; }
            set { _viewModel.InteractionMode = value; }
        }

        /// <summary>
        /// Gets or sets the layout mode.
        /// </summary>
        public QuidnuncLayoutMode LayoutMode
        {
            get { return _viewModel.LayoutMode; }
            set { _viewModel.LayoutMode = value; }
        }

        /// <summary>
        /// Retrieves the window associated with this window pane.
        /// </summary>
        public override IWin32Window Window
        {
            get { return _viewHost; }
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
            // Force a refresh by resetting the document.
            Document = null;
            Document = document;
        }

        /// <summary>
        /// This method can be overriden by the derived class to execute
        /// any code that needs to run after the IVsWindowFrame is created.
        /// If the toolwindow has a toolbar with a combobox, it should make
        /// sure its command handler are set by the time they return from
        /// this method.
        /// This is called when someone set the Frame property.
        /// </summary>
        public override void OnToolWindowCreated()
        {
            base.OnToolWindowCreated();

            // Register for events to this window.
            ((IVsWindowFrame)Frame).SetProperty(
                (int)__VSFPROPID.VSFPROPID_ViewHelper, this);
        }

        /// <summary>
        /// Refresh the quinunc tool window.
        /// </summary>
        public void Refresh()
        {
            var document = Document;
            if (document != null)
            {
                // Force a refresh by resetting the document.
                Document = null;
                Document = document;
            }
        }

        #endregion Public Methods

        #region Private Properties

        /// <summary>
        /// Gets or sets the current document.
        /// </summary>
        private Document Document
        {
            get { return _document; }
            set
            {
                if (_document != value)
                {
                    _document = value;
                    ConditionallyUpdateCodeModel();
                }
            }
        }

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
                    ConditionallyUpdateCodeModel();
                }
            }
        }

        #endregion Private Properties

        #region Private Methods

        /// <summary>
        /// Conditionally updates the code model.
        /// </summary>
        private void ConditionallyUpdateCodeModel()
        {
            if (IsVisible && Document != null)
            {
                // Clear any existing code items while processing.
                UpdateViewModelRawCodeItems(null);

                _codeModelRetriever.RetrieveCodeModelAsync(Document);
            }
        }

        /// <summary>
        /// Updates the view model's raw code items collection.
        /// </summary>
        /// <param name="codeItems">The code items.</param>
        private void UpdateViewModelRawCodeItems(IEnumerable<CodeItemBase> codeItems)
        {
            _viewModel.RawCodeItems = codeItems;
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
    }
}