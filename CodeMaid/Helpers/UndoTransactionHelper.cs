#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

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
                var message = $"{_transactionName} was stopped";
                OutputWindowHelper.ExceptionWriteLine(message, ex);
                _package.IDE.StatusBar.Text = $"{message}.  See output window for more details.";

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