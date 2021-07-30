using SteveCadwallader.CodeMaid.Properties;
using System;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A helper class for performing actions within the context of an undo transaction.
    /// </summary>
    public class UndoTransactionHelper
    {
        #region Fields

        private readonly CodeMaidPackage _package;
        private readonly string _transactionName;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoTransactionHelper" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="transactionName">The name of the transaction.</param>
        public UndoTransactionHelper(CodeMaidPackage package, string transactionName)
        {
            _package = package;
            _transactionName = transactionName;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Runs the specified try action within a try block, and conditionally the catch action
        /// within a catch block all conditionally within the context of an undo transaction.
        /// </summary>
        /// <param name="tryAction">The action to be performed within a try block.</param>
        /// <param name="catchAction">The action to be performed wihin a catch block.</param>
        public void Run(Action tryAction, Action<Exception> catchAction = null)
        {
            bool shouldCloseUndoContext = false;

            // Start an undo transaction (unless inside one already or within an auto save context).
            if (Settings.Default.General_UseUndoTransactions && !_package.IDE.UndoContext.IsOpen &&
                !(_package.IsAutoSaveContext && Settings.Default.General_SkipUndoTransactionsDuringAutoCleanupOnSave))
            {
                _package.IDE.UndoContext.Open(_transactionName);
                shouldCloseUndoContext = true;
            }

            try
            {
                tryAction();
            }
            catch (Exception ex)
            {
                var message = $"{_transactionName}{Resources.WasStopped}";
                OutputWindowHelper.ExceptionWriteLine(message, ex);
                _package.IDE.StatusBar.Text = $"{message}{Resources.SeeOutputWindowForMoreDetails}";

                catchAction?.Invoke(ex);

                if (shouldCloseUndoContext)
                {
                    _package.IDE.UndoContext.SetAborted();
                    shouldCloseUndoContext = false;
                }
            }
            finally
            {
                // Always close the undo transaction to prevent ongoing interference with the IDE.
                if (shouldCloseUndoContext)
                {
                    _package.IDE.UndoContext.Close();
                }
            }
        }

        #endregion Methods
    }
}