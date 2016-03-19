using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
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
        private bool _isStale;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeModel" /> class.
        /// </summary>
        /// <param name="document">The document.</param>
        internal CodeModel(Document document)
        {
            CodeItems = new SetCodeItems();
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
                    OutputWindowHelper.DiagnosticWriteLine(
                        string.Format("CodeModel.IsBuilding changing to '{0}' for '{1}'", value, Document.FullName));

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
        internal bool IsStale
        {
            get { return _isStale; }
            set
            {
                if (_isStale != value)
                {
                    OutputWindowHelper.DiagnosticWriteLine(
                        string.Format("CodeModel.IsStale changing to '{0}' for '{1}'", value, Document.FullName));

                    _isStale = value;
                }
            }
        }

        #endregion Properties
    }
}