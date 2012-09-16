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
using System.ComponentModel;
using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Model.CodeItems;

namespace SteveCadwallader.CodeMaid.Logic.Digging
{
    /// <summary>
    /// A helper class for performing asynchronous code model retrievals.
    /// </summary>
    internal class SpadeCodeModelRetriever
    {
        #region Fields

        private readonly BackgroundWorker _bw;
        private readonly Action<DocumentCodeItemsSnapshot> _callback;
        private Document _pendingDocument;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeCodeModelRetriever"/> class.
        /// </summary>
        /// <param name="callback">The callback for results.</param>
        internal SpadeCodeModelRetriever(Action<DocumentCodeItemsSnapshot> callback)
        {
            _bw = new BackgroundWorker { WorkerSupportsCancellation = true };
            _bw.DoWork += OnDoWork;
            _bw.RunWorkerCompleted += OnRunWorkerCompleted;

            _callback = callback;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Retrieves the code model asynchronously from the specified document.
        /// </summary>
        /// <param name="document">The document to process.</param>
        internal void RetrieveCodeModelAsync(Document document)
        {
            if (_bw.IsBusy)
            {
                _pendingDocument = document;
                _bw.CancelAsync();
            }
            else
            {
                _pendingDocument = null;
                _bw.RunWorkerAsync(document);
            }
        }

        private static void OnDoWork(object sender, DoWorkEventArgs e)
        {
            var document = e.Argument as Document;
            if (document == null) return;

            var codeItems = CodeModelHelper.RetrieveCodeItemsIncludingRegions(document);
            codeItems.RemoveAll(x => x is CodeItemUsingStatement || x is CodeItemNamespace);

            if (!e.Cancel)
            {
                e.Result = new DocumentCodeItemsSnapshot(document, codeItems);
            }
        }

        private void OnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_pendingDocument != null)
            {
                RetrieveCodeModelAsync(_pendingDocument);
            }
            else if (e.Error == null)
            {
                var snapshot = e.Result as DocumentCodeItemsSnapshot;
                if (snapshot != null)
                {
                    _callback(snapshot);
                }
            }
        }

        #endregion Methods
    }
}