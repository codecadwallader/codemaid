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
    /// A builder class for generating code models.
    /// </summary>
    internal class CodeModelBuilder
    {
        #region Fields

        private readonly CodeMaidPackage _package;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="CodeModelBuilder"/> class.
        /// </summary>
        private static CodeModelBuilder _instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeModelBuilder"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private CodeModelBuilder(CodeMaidPackage package)
        {
            _package = package;
        }

        /// <summary>
        /// Gets an instance of the <see cref="CodeModelBuilder"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="CodeModelBuilder"/> class.</returns>
        internal static CodeModelBuilder GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new CodeModelBuilder(package));
        }

        #endregion Constructors
    }
}