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

using System.Collections.Generic;
using EnvDTE;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.Model
{
    /// <summary>
    /// A class for encapsulating a cache of code models.
    /// </summary>
    internal class CodeModelCache
    {
        #region Fields

        private readonly Dictionary<Document, SnapshotCodeItems> _codeItemsCache;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeModelCache"/> class.
        /// </summary>
        internal CodeModelCache()
        {
            _codeItemsCache = new Dictionary<Document, SnapshotCodeItems>();
        }

        #endregion Constructors

        #region Internal Methods

        /// <summary>
        /// Checks the cache (if enabled) to retrieve the code items for the specified document if present.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>The cached code items, otherwise null.</returns>
        internal SetCodeItems GetCodeItemsFromCacheIfEnabled(Document document)
        {
            if (Settings.Default.Digging_CacheFiles)
            {
                SnapshotCodeItems snapshot;
                if (_codeItemsCache.TryGetValue(document, out snapshot))
                {
                    return snapshot.CodeItems;
                }
            }

            return null;
        }

        /// <summary>
        /// Places the specified code items into the cache (if enabled).
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="codeItems">The code items.</param>
        internal void PlaceCodeItemsInCacheIfEnabled(Document document, SetCodeItems codeItems)
        {
            if (Settings.Default.Digging_CacheFiles)
            {
                _codeItemsCache[document] = new SnapshotCodeItems(document, codeItems);
            }
        }

        #endregion Internal Methods
    }
}