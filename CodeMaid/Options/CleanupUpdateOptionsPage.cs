#region CodeMaid is Copyright 2007-2010 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2010 Steve Cadwallader.

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;

namespace SteveCadwallader.CodeMaid.Options
{
    /// <summary>
    /// An options page for cleanup update options that are integrated into the IDE options window.
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    [Guid("df0e8a40-bb30-440e-984f-abeaaee669cb")]
    public class CleanupUpdateOptionsPage : DialogPage
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupUpdateOptionsPage"/> class.
        /// </summary>
        public CleanupUpdateOptionsPage()
        {
            // Set the default settings, will be trumped if there are stored values.
            SortUsingStatements = true;
            UpdateRegionDirectives = true;
        }

        #endregion Constructors

        #region Public Properties

        [Category("CodeMaid")]
        [DisplayName(@"Sort using statements")]
        [Description("On cleanup code, sorts the using statements alphabetically.")]
        public bool SortUsingStatements { get; set; }

        [Category("CodeMaid")]
        [DisplayName(@"Update region directives")]
        [Description("On cleanup code, sets endregion tags with their matching region name.\r\nExample: '#endregion Methods'.")]
        public bool UpdateRegionDirectives { get; set; }

        #endregion Public Properties

        #region Overrides

        /// <summary>
        /// Gets the window this options page will use for its UI.
        /// </summary>
        protected override IWin32Window Window
        {
            get { return new CleanupUpdateOptionsControl(this); }
        }

        #endregion Overrides
    }
}