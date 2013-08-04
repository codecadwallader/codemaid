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

namespace SteveCadwallader.CodeMaid.Model
{
    /// <summary>
    /// A manager class for centralizing code model creation and life cycles.
    /// </summary>
    internal class CodeModelManager
    {
        #region Fields

        private readonly CodeMaidPackage _package;

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
    }
}