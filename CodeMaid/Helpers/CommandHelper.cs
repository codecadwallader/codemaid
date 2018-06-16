using EnvDTE;
using System;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A helper class for accessing commands.
    /// </summary>
    public class CommandHelper
    {
        #region Fields

        private readonly CodeMaidPackage _package;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="CommandHelper" /> class.
        /// </summary>
        private static CommandHelper _instance;

        /// <summary>
        /// Gets an instance of the <see cref="CommandHelper" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="CommandHelper" /> class.</returns>
        internal static CommandHelper GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new CommandHelper(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHelper" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private CommandHelper(CodeMaidPackage package)
        {
            _package = package;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Finds a command of any of the specified names, otherwise null.
        /// </summary>
        /// <param name="commandNames">The command names.</param>
        /// <returns>The found command, otherwise null.</returns>
        public Command FindCommand(params string[] commandNames)
        {
            if (commandNames == null || commandNames.Length == 0) return null;

            return _package.IDE.Commands.OfType<Command>().FirstOrDefault(x => commandNames.Contains(x.Name));
        }

        /// <summary>
        /// Finds a command by the specified guid/id pair.
        /// </summary>
        /// <param name="guid">The command guid.</param>
        /// <param name="id">The command id.</param>
        /// <returns>The found command, otherwise null.</returns>
        public Command FindCommand(string guid, int id)
        {
            return _package.IDE.Commands.OfType<Command>().FirstOrDefault(x => x.Guid == guid && x.ID == id);
        }

        /// <summary>
        /// Executes the specified command when available against the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        /// <param name="commandNames">The cleanup command name(s).</param>
        public void ExecuteCommand(TextDocument textDocument, params string[] commandNames)
        {
            try
            {
                var command = FindCommand(commandNames);
                if (command != null && command.IsAvailable)
                {
                    using (new CursorPositionRestorer(textDocument))
                    {
                        _package.IDE.ExecuteCommand(command.Name, string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                // OK if fails, not available for some file types.
                OutputWindowHelper.DiagnosticWriteLine($"Unable to execute command(s) {string.Join(",", commandNames)} on {textDocument.Parent.FullName}", ex);
            }
        }

        #endregion Methods
    }
}