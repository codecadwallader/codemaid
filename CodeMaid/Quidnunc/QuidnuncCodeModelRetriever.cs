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

using System;
using System.ComponentModel;
using EnvDTE;
using SteveCadwallader.CodeMaid.CodeItems;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Quidnunc
{
    /// <summary>
    /// A helper class for performing asynchronous code model retrievals.
    /// </summary>
    internal class QuidnuncCodeModelRetriever
    {
        #region Fields

        private readonly BackgroundWorker _bw;
        private readonly Action<SetCodeItems> _callback;
        private Document _pendingDocument;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QuidnuncCodeModelRetriever"/> class.
        /// </summary>
        /// <param name="callback">The callback for results.</param>
        internal QuidnuncCodeModelRetriever(Action<SetCodeItems> callback)
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
                e.Result = codeItems;
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
                var codeItems = e.Result as SetCodeItems;
                if (codeItems != null)
                {
                    _callback(codeItems);
                }
            }
        }

        #endregion Methods
    }
}