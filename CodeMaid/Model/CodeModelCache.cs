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

namespace SteveCadwallader.CodeMaid.Model
{
    /// <summary>
    /// A class for encapsulating a cache of code models.
    /// </summary>
    internal class CodeModelCache
    {
        #region Fields

        private readonly Dictionary<Document, CodeModel> _cache;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeModelCache"/> class.
        /// </summary>
        internal CodeModelCache()
        {
            _cache = new Dictionary<Document, CodeModel>();
        }

        #endregion Constructors

        #region Internal Methods

        /// <summary>
        /// Gets a code model for the specified document. If the code model is not present in the
        /// cache, a new code model will be generated and added to the cache.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>A code model representing the document.</returns>
        internal CodeModel GetCodeModel(Document document)
        {
            CodeModel codeModel;

            lock (_cache)
            {
                if (!_cache.TryGetValue(document, out codeModel))
                {
                    codeModel = new CodeModel(document) { IsStale = true };
                    _cache.Add(document, codeModel);
                }
            }

            return codeModel;
        }

        /// <summary>
        /// Removes the code model associated with the specified document if it exists.
        /// </summary>
        /// <param name="document">The document to remove.</param>
        internal void RemoveCodeModel(Document document)
        {
            lock (_cache)
            {
                _cache.Remove(document);
            }
        }

        #endregion Internal Methods
    }
}