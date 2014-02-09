#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using EnvDTE;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A simple class encapsulating indentation settings.
    /// </summary>
    internal class IndentSettings
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IndentSettings" /> class.
        /// </summary>
        public IndentSettings()
        {
            IndentSize = 4;
            IndentStyle = vsIndentStyle.vsIndentStyleSmart;
            InsertTabs = false;
            TabSize = 4;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndentSettings" /> class based on the
        /// specified settings.
        /// </summary>
        /// <param name="settings">The environment settings used for initialization.</param>
        public IndentSettings(EnvDTE.Properties settings)
        {
            IndentSize = (short)settings.Item("IndentSize").Value;
            IndentStyle = (vsIndentStyle)settings.Item("IndentStyle").Value;
            InsertTabs = (bool)settings.Item("InsertTabs").Value;
            TabSize = (short)settings.Item("TabSize").Value;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the indent size.
        /// </summary>
        public short IndentSize { get; set; }

        /// <summary>
        /// Gets of sets the indent style.
        /// </summary>
        public vsIndentStyle IndentStyle { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating if tabs should be inserted.
        /// </summary>
        public bool InsertTabs { get; set; }

        /// <summary>
        /// Gets or sets the tab size.
        /// </summary>
        public short TabSize { get; set; }

        #endregion Properties
    }
}