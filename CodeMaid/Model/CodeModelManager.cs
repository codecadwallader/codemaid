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
    /// A manager class for centralizing code model creation and life cycles.
    /// </summary>
    internal class CodeModelManager
    {
        #region Fields

        private readonly CodeMaidPackage _package;

        private readonly CodeModelBuilder _codeModelBuilder;

        private readonly Dictionary<Document, SnapshotCodeItems> _codeItemsCache;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="CodeModelManager"/> class.
        /// </summary>
        private static CodeModelManager _instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeModelManager"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private CodeModelManager(CodeMaidPackage package)
        {
            _package = package;

            _codeModelBuilder = CodeModelBuilder.GetInstance(_package);

            _codeItemsCache = new Dictionary<Document, SnapshotCodeItems>();
        }

        /// <summary>
        /// Gets an instance of the <see cref="CodeModelManager"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="CodeModelManager"/> class.</returns>
        internal static CodeModelManager GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new CodeModelManager(package));
        }

        #endregion Constructors

        #region Internal Methods

        /// <summary>
        /// Retrieves a <see cref="SetCodeItems"/> of CodeItems within the specified document including regions.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>The set of code items within the document, including regions.</returns>
        internal SetCodeItems RetrieveAllCodeItems(Document document)
        {
            var codeItems = GetCodeItemsFromCacheIfEnabled(document);
            if (codeItems == null)
            {
                codeItems = _codeModelBuilder.RetrieveAllCodeItems(document);
                PlaceCodeItemsInCacheIfEnabled(document, codeItems);
            }

            return codeItems;
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Checks the cache (if enabled) to retrieve the code items for the specified document if present.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>The cached code items, otherwise null.</returns>
        private SetCodeItems GetCodeItemsFromCacheIfEnabled(Document document)
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
        private void PlaceCodeItemsInCacheIfEnabled(Document document, SetCodeItems codeItems)
        {
            if (Settings.Default.Digging_CacheFiles)
            {
                _codeItemsCache[document] = new SnapshotCodeItems(document, codeItems);
            }
        }

        #endregion Private Methods
    }
}