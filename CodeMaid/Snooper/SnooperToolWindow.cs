#region CodeMaid is Copyright 2007-2010 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2010 Steve Cadwallader.

using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace SteveCadwallader.CodeMaid.Snooper
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    ///
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    ///
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </summary>
    [Guid("75d09b86-471e-4b30-8720-362d13ad0a45")]
    public class SnooperToolWindow : ToolWindowPane, IVsWindowFrameNotify3
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SnooperToolWindow"/> class.
        /// </summary>
        public SnooperToolWindow() :
            base(null)
        {
            // Set the tool window caption.
            Caption = "CodeMaid Snooper";

            // Set the tool window image from resources.
            BitmapResourceID = 501;
            BitmapIndex = 0;

            // Create the toolbar for the tool window.
            ToolBar = new CommandID(GuidList.GuidCodeMaidToolbarToolWindowBaseGroup, PkgCmdIDList.ToolbarIDCodeMaidToolbarToolWindow);

            // Set the tool window content.
            Control = new SnooperControl();
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Retrieves the window associated with this window pane.
        /// </summary>
        public override IWin32Window Window
        {
            get { return Control; }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// A method to be called to notify the tool window about the current active document.
        /// </summary>
        /// <param name="document">The active document.</param>
        public void NotifyActiveDocument(Document document)
        {
            Control.Document = document;
        }

        /// <summary>
        /// A method to be called to notify the tool window that has a document has been saved.
        /// </summary>
        /// <param name="document">The document.</param>
        public void NotifyDocumentSave(Document document)
        {
            Control.ForceRefresh();
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

            // Pass the package down to the control.
            Control.Package = Package as CodeMaidPackage;

            // Register for events to this window.
            ((IVsWindowFrame)Frame).SetProperty(
                (int)__VSFPROPID.VSFPROPID_ViewHelper, this);
        }

        #endregion Public Methods

        #region Private Properties

        /// <summary>
        /// Gets the tool control hosted in the tool window.
        /// </summary>
        private SnooperControl Control { get; set; }

        #endregion Private Properties

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
                    Control.IsSnooperVisible = true;
                    break;

                case __FRAMESHOW.FRAMESHOW_WinHidden:
                    Control.IsSnooperVisible = false;
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