using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System.Collections.Generic;

namespace SteveCadwallader.CodeMaid.Model
{
    /// <summary>
    /// A class for encapsulating a cache of code models.
    /// </summary>
    internal class CodeModelCache
    {
        #region Fields

        private readonly Dictionary<string, CodeModel> _cache;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeModelCache" /> class.
        /// </summary>
        internal CodeModelCache()
        {
            _cache = new Dictionary<string, CodeModel>();
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

            OutputWindowHelper.DiagnosticWriteLine($"CodeModelCache.GetCodeModel for '{document.FullName}'");

            lock (_cache)
            {
                if (!_cache.TryGetValue(document.FullName, out codeModel))
                {
                    codeModel = new CodeModel(document) { IsStale = true };

                    if (Settings.Default.General_CacheFiles)
                    {
                        _cache.Add(document.FullName, codeModel);
                        OutputWindowHelper.DiagnosticWriteLine("  --added to cache (stale).");
                    }
                }
                else
                {
                    OutputWindowHelper.DiagnosticWriteLine(codeModel.IsStale
                        ? "  --retrieved from cache (stale)."
                        : "  --retrieved from cache (not stale).");
                }
            }

            return codeModel;
        }

        /// <summary>
        /// Removes the code model associated with the specified document if it exists.
        /// </summary>
        /// <param name="document">The document.</param>
        internal void RemoveCodeModel(Document document)
        {
            lock (_cache)
            {
                if (_cache.Remove(document.FullName))
                {
                    OutputWindowHelper.DiagnosticWriteLine($"CodeModelCache.RemoveCodeModel from cache for '{document.FullName}'");
                }
            }
        }

        /// <summary>
        /// Marks the code model associated with the specified document as stale if it exists.
        /// </summary>
        /// <param name="document">The document.</param>
        internal void StaleCodeModel(Document document)
        {
            if (_cache.TryGetValue(document.FullName, out CodeModel codeModel))
            {
                codeModel.IsStale = true;
                OutputWindowHelper.DiagnosticWriteLine($"CodeModelCache.StaleCodeModel in cache for '{document.FullName}'");
            }
        }

        #endregion Internal Methods
    }
}