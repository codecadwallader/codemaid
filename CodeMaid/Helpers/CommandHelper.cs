#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using EnvDTE;
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

        #endregion Methods
    }
}