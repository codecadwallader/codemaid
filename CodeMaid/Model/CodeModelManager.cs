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
using System.Threading.Tasks;
using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
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

        /// <summary>
        /// An event raised when a <see cref="CodeModel"/> has been built.
        /// </summary>
        internal event Action<CodeModel> CodeModelBuilt;

        #endregion Events

        #region Internal Event Handlers

        /// <summary>
        /// An event callback that is raised when a document has changed.
        /// </summary>
        /// <param name="document">The document.</param>
        internal void OnDocumentChanged(Document document)
        {
            _codeModelCache.StaleCodeModel(document);
        }

        /// <summary>
        /// An event callback that is raised when a document is closing.
        /// </summary>
        /// <param name="document">The document.</param>
        internal void OnDocumentClosing(Document document)
        {
            _codeModelCache.RemoveCodeModel(document);
        }

        #endregion Internal Event Handlers

        #region Internal Methods

        /// <summary>
        /// Retrieves a <see cref="SetCodeItems"/> of CodeItems within the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>The set of code items within the document.</returns>
        internal SetCodeItems RetrieveAllCodeItems(Document document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            var codeModel = _codeModelCache.GetCodeModel(document);
            if (codeModel.IsBuilding)
            {
                if (!codeModel.IsBuiltWaitHandle.WaitOne(TimeSpan.FromSeconds(30)))
                {
                    OutputWindowHelper.WriteLine(String.Format(
                        "CodeMaid warning: Timed out waiting for code model to be built for {0}.",
                        codeModel.Document.FullName));
                    return null;
                }
            }
            else if (codeModel.IsStale)
            {
                BuildCodeItems(codeModel);
            }

            return codeModel.CodeItems;
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
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            var codeModel = _codeModelCache.GetCodeModel(document);
            if (codeModel.IsBuilding)
            {
                // Exit out and wait for the event to be raised.
                return null;
            }

            if (codeModel.IsStale)
            {
                // Asynchronously build the code items then raise an event.
                Task.Run(() =>
                {
                    BuildCodeItems(codeModel);
                    RaiseCodeModelBuilt(codeModel);
                });

                return null;
            }

            return codeModel.CodeItems;
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Builds a <see cref="SetCodeItems"/> of CodeItems based on the specified code model. If
        /// the document gets marked as stale during execution this process will recursively call
        /// itself to start over in order to guarantee a valid code model is returned.
        /// </summary>
        /// <param name="codeModel">The code model.</param>
        private void BuildCodeItems(CodeModel codeModel)
        {
            try
            {
                codeModel.IsBuilding = true;

                var codeItems = _codeModelBuilder.RetrieveAllCodeItems(codeModel.Document);

                if (codeModel.IsStale)
                {
                    codeModel.IsStale = false;
                    BuildCodeItems(codeModel);
                    return;
                }

                codeModel.CodeItems = codeItems;
                codeModel.IsBuilding = false;
            }
            catch (Exception ex)
            {
                OutputWindowHelper.WriteLine(String.Format(
                    "CodeMaid exception: Unable to build code model for {0}: {1}",
                    codeModel.Document.FullName, ex));

                codeModel.CodeItems = null;
                codeModel.IsBuilding = false;
            }
        }

        /// <summary>
        /// Raises the <see cref="CodeModelBuilt"/> event.
        /// </summary>
        /// <param name="codeModel">The code model.</param>
        private void RaiseCodeModelBuilt(CodeModel codeModel)
        {
            var codeModelBuilt = CodeModelBuilt;
            if (codeModelBuilt != null)
            {
                codeModelBuilt(codeModel);
            }
        }

        #endregion Private Methods
    }
}