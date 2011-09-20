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
using System.Collections.Generic;
using System.ComponentModel;
using SteveCadwallader.CodeMaid.CodeItems;

namespace SteveCadwallader.CodeMaid.Quidnunc
{
    /// <summary>
    /// A helper class for performing asynchronous code tree building.
    /// </summary>
    internal class QuidnuncCodeTreeBuilder
    {
        #region Fields

        private readonly BackgroundWorker _bw;
        private readonly Action<SetCodeItems> _callback;
        private QuidnuncCodeTreeRequest _pendingRequest;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QuidnuncCodeTreeBuilder"/> class.
        /// </summary>
        /// <param name="callback">The callback for results.</param>
        internal QuidnuncCodeTreeBuilder(Action<SetCodeItems> callback)
        {
            _bw = new BackgroundWorker { WorkerSupportsCancellation = true };
            _bw.DoWork += OnDoWork;
            _bw.RunWorkerCompleted += OnRunWorkerCompleted;

            _callback = callback;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Builds a code tree asynchronously from the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        internal void RetrieveCodeTreeAsync(QuidnuncCodeTreeRequest request)
        {
            if (_bw.IsBusy)
            {
                _pendingRequest = request;
                _bw.CancelAsync();
            }
            else
            {
                _pendingRequest = null;
                _bw.RunWorkerAsync(request);
            }
        }

        private static void OnDoWork(object sender, DoWorkEventArgs e)
        {
            var request = e.Argument as QuidnuncCodeTreeRequest;
            if (request == null) return;

            SetCodeItems codeItems = null;

            switch (request.LayoutMode)
            {
                case QuidnuncLayoutMode.AlphaLayout:
                    codeItems = OrganizeCodeItemsByAlphaLayout(request.RawCodeItems);
                    break;

                case QuidnuncLayoutMode.FileLayout:
                    codeItems = OrganizeCodeItemsByFileLayout(request.RawCodeItems);
                    break;

                case QuidnuncLayoutMode.TypeLayout:
                    codeItems = OrganizeCodeItemsByTypeLayout(request.RawCodeItems);
                    break;
            }

            if (!e.Cancel)
            {
                e.Result = codeItems;
            }
        }

        private void OnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_pendingRequest != null)
            {
                RetrieveCodeTreeAsync(_pendingRequest);
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

        /// <summary>
        /// Organizes the code items by alpha layout.
        /// </summary>
        /// <param name="rawCodeItems">The raw code items.</param>
        /// <returns>The organized code items.</returns>
        private static SetCodeItems OrganizeCodeItemsByAlphaLayout(IEnumerable<CodeItemBase> rawCodeItems)
        {
            var organizedCodeItems = new SetCodeItems();

            if (rawCodeItems != null)
            {
                organizedCodeItems.AddRange(rawCodeItems);

                // Sort the list of code items by name.
                organizedCodeItems.Sort((x, y) => x.Name.CompareTo(y.Name));
            }

            return organizedCodeItems;
        }

        /// <summary>
        /// Organizes the code items by file layout.
        /// </summary>
        /// <param name="rawCodeItems">The raw code items.</param>
        /// <returns>The organized code items.</returns>
        private static SetCodeItems OrganizeCodeItemsByFileLayout(IEnumerable<CodeItemBase> rawCodeItems)
        {
            var organizedCodeItems = new SetCodeItems();

            if (rawCodeItems != null)
            {
                organizedCodeItems.AddRange(rawCodeItems);

                // Sort the list of code items by starting location.
                organizedCodeItems.Sort((x, y) => x.StartLine.CompareTo(y.StartLine));
            }

            return organizedCodeItems;
        }

        /// <summary>
        /// Organizes the code items by type layout.
        /// </summary>
        /// <param name="rawCodeItems">The raw code items.</param>
        /// <returns>The organized code items.</returns>
        private static SetCodeItems OrganizeCodeItemsByTypeLayout(IEnumerable<CodeItemBase> rawCodeItems)
        {
            var organizedCodeItems = new SetCodeItems();

            if (rawCodeItems != null)
            {
                organizedCodeItems.AddRange(rawCodeItems);

                // Sort the list of code items by name.
                organizedCodeItems.Sort((x, y) => x.Name.CompareTo(y.Name));
            }

            return organizedCodeItems;
        }

        #endregion Methods
    }
}