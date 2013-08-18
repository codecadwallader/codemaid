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

using System.Threading.Tasks;
using EnvDTE;
using SteveCadwallader.CodeMaid.Model.CodeItems;

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
        private readonly CodeModelCache _codeModelCache;

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
            _codeModelCache = new CodeModelCache();
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

        #region Events

        #endregion Events

        #region Internal Methods

        /// <summary>
        /// Retrieves a <see cref="SetCodeItems"/> of CodeItems within the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>The set of code items within the document.</returns>
        internal SetCodeItems RetrieveAllCodeItems(Document document)
        {
            var codeItems = _codeModelCache.GetCodeItemsFromCache(document);
            if (codeItems == null)
            {
                codeItems = BuildCodeModelAndPlaceInCache(document);
            }

            return codeItems;
        }

        /// <summary>
        /// Retrieves a <see cref="SetCodeItems"/> of CodeItems within the specified document.
        /// If the code items are already available they will be returned,
        /// otherwise an event will be raised once the code items have been asynchronously built.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>The set of code items within the document if already available, otherwise null.</returns>
        internal SetCodeItems RetrieveAllCodeItemsAsync(Document document)
        {
            var codeItems = _codeModelCache.GetCodeItemsFromCache(document);
            if (codeItems == null)
            {
                Task.Run(() => BuildCodeModelAndPlaceInCache(document))
                    .ContinueWith(task => Raise(task.Result));
            }

            return codeItems;
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Retrieves a <see cref="SetCodeItems"/> of CodeItems within the specified document.
        /// After retrieving the CodeItems, places them into the cache.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>The set of code items within the document.</returns>
        private SetCodeItems BuildCodeModelAndPlaceInCache(Document document)
        {
            var codeItems = _codeModelBuilder.RetrieveAllCodeItems(document);
            _codeModelCache.PlaceCodeItemsInCache(document, codeItems);

            return codeItems;
        }

        private void Raise(SetCodeItems codeItems)
        {
            //TODO: Implement
        }

        #endregion Private Methods
    }
}