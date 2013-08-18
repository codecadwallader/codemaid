#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using System;
using EnvDTE;
using EnvDTE80;

namespace SteveCadwallader.CodeMaid.Integration.Events
{
    /// <summary>
    /// A class that encapsulates listening for code model events.
    /// </summary>
    internal class CodeModelEventListener : BaseEventListener
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeModelEventListener"/> class.
        /// </summary>
        /// <param name="package">The package hosting the event listener.</param>
        internal CodeModelEventListener(CodeMaidPackage package)
            : base(package)
        {
            // Store access to the code model events, otherwise events will not register properly via DTE.
            CodeModelEvents = ((Events2)Package.IDE.Events).CodeModelEvents;
            CodeModelEvents.ElementAdded += CodeModelEvents_ElementAdded;
            CodeModelEvents.ElementChanged += CodeModelEvents_ElementChanged;
            CodeModelEvents.ElementDeleted += CodeModelEvents_ElementDeleted;
        }

        #endregion Constructors

        #region Internal Events

        /// <summary>
        /// An event raised when an element has been added.
        /// </summary>
        internal event Action<Document> OnElementAdded;

        /// <summary>
        /// An event raised when an element has been changed.
        /// </summary>
        internal event Action<Document> OnElementChanged;

        /// <summary>
        /// An event raised when an element has been deleted.
        /// </summary>
        internal event Action<Document> OnElementDeleted;

        #endregion Internal Events

        #region Private Properties

        /// <summary>
        /// Gets or sets a pointer to the IDE code model events.
        /// </summary>
        private CodeModelEvents CodeModelEvents { get; set; }

        #endregion Private Properties

        #region Private Event Handlers

        /// <summary>
        /// An event handler for a code element being added.
        /// </summary>
        /// <param name="codeElement">The code element that has been added.</param>
        private void CodeModelEvents_ElementAdded(CodeElement codeElement)
        {
            if (OnElementAdded != null)
            {
                OnElementAdded(codeElement.ProjectItem.Document);
            }
        }

        /// <summary>
        /// An event handler for a code element being changed.
        /// </summary>
        /// <param name="codeElement">The code element that has been changed.</param>
        /// <param name="change">The type of change that has occurred.</param>
        private void CodeModelEvents_ElementChanged(CodeElement codeElement, vsCMChangeKind change)
        {
            if (OnElementChanged != null)
            {
                OnElementChanged(codeElement.ProjectItem.Document);
            }
        }

        /// <summary>
        /// An event handler for a code element being deleted.
        /// </summary>
        /// <param name="parent">The parent of the code element that was deleted.</param>
        /// <param name="codeElement">The code element that has been deleted.</param>
        private void CodeModelEvents_ElementDeleted(object parent, CodeElement codeElement)
        {
            if (OnElementDeleted != null)
            {
                OnElementDeleted(codeElement.ProjectItem.Document);
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

                if (disposing && CodeModelEvents != null)
                {
                    CodeModelEvents.ElementAdded -= CodeModelEvents_ElementAdded;
                    CodeModelEvents.ElementChanged -= CodeModelEvents_ElementChanged;
                    CodeModelEvents.ElementDeleted -= CodeModelEvents_ElementDeleted;
                }
            }
        }

        #endregion IDisposable Members
    }
}