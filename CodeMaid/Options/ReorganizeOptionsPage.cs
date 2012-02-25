#region CodeMaid is Copyright 2007-2012 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2012 Steve Cadwallader.

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;

namespace SteveCadwallader.CodeMaid.Options
{
    /// <summary>
    /// An options page for reorganization options that are integrated into the IDE options window.
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    [Guid("7c8cb02c-1532-4f33-b60e-fbf15c2b7bcb")]
    public class ReorganizeOptionsPage : DialogPage
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReorganizeOptionsPage"/> class.
        /// </summary>
        public ReorganizeOptionsPage()
        {
            // Set the default settings, will be trumped if there are stored values.
            RunAtStartOfCleanup = false;
            AlphabetizeMembersOfTheSameGroup = true;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the option to run reorganization at start of cleanup.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Run reorganization at start of cleanup")]
        [Description("An option to run reorganization at start of cleanup.")]
        public bool RunAtStartOfCleanup { get; set; }

        /// <summary>
        /// Gets or sets the option for alphabetizing members of the same group.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Alphabetize members of the same group")]
        [Description("An option to alphabetize members of the same group")]
        public bool AlphabetizeMembersOfTheSameGroup { get; set; }

        #endregion Public Properties

        #region Overrides

        /// <summary>
        /// Gets the window this options page will use for its UI.
        /// </summary>
        protected override IWin32Window Window
        {
            get { return new ReorganizeOptionsControl(this); }
        }

        #endregion Overrides
    }
}