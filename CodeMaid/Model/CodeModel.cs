#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using EnvDTE;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using System.Threading;

namespace SteveCadwallader.CodeMaid.Model
{
    /// <summary>
    /// This class encapsulates the representation of a document, including its code items and
    /// current state.
    /// </summary>
    internal class CodeModel
    {
        #region Fields

        private bool _isBuilding;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeModel" /> class.
        /// </summary>
        /// <param name="document">The document.</param>
        internal CodeModel(Document document)
        {
            Document = document;
            IsBuiltWaitHandle = new ManualResetEvent(false);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the document.
        /// </summary>
        internal Document Document { get; private set; }

        /// <summary>
        /// Gets or sets the code items.
        /// </summary>
        internal SetCodeItems CodeItems { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if this model is currently being built.
        /// </summary>
        internal bool IsBuilding
        {
            get { return _isBuilding; }
            set
            {
                if (_isBuilding != value)
                {
                    _isBuilding = value;
                    if (_isBuilding)
                    {
                        IsBuiltWaitHandle.Reset();
                    }
                    else
                    {
                        IsBuiltWaitHandle.Set();
                    }
                }
            }
        }

        /// <summary>
        /// Gets a wait handle that will be signaled when building is complete.
        /// </summary>
        internal ManualResetEvent IsBuiltWaitHandle { get; private set; }

        /// <summary>
        /// Gets or sets a flag indicating if this model is stale.
        /// </summary>
        internal bool IsStale { get; set; }

        #endregion Properties
    }
}