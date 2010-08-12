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
    /// An options page for build status options that are integrated into the IDE options window.
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    [Guid("7e14280f-a7ec-4326-bb1e-57e0f5053920")]
    public class BuildStatusOptionsPage : DialogPage
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildStatusOptionsPage"/> class.
        /// </summary>
        public BuildStatusOptionsPage()
        {
            // Set the default settings, will be trumped if there are stored values.
            ExtendBuildStatusMessages = true;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the option to extend build status messages.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Extend build status messages")]
        [Description("When a build is running, extend the build status messages that appear in the status bar.")]
        public bool ExtendBuildStatusMessages { get; set; }

        #endregion Public Properties

        #region Overrides

        /// <summary>
        /// Gets the window this options page will use for its UI.
        /// </summary>
        protected override IWin32Window Window
        {
            get { return new BuildStatusOptionsControl(this); }
        }

        #endregion Overrides
    }
}