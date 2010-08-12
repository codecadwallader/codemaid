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
    /// An options page for snooper options that are integrated into the IDE options window.
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    [Guid("bff0adb0-1a53-4e58-bfb4-b38ec00487b7")]
    public class SnooperOptionsPage : DialogPage
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SnooperOptionsPage"/> class.
        /// </summary>
        public SnooperOptionsPage()
        {
            // Set the default settings, will be trumped if there are stored values.
            ComplexityWarningThreshold = 10;
            ComplexityAlertThreshold = 15;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the option for the complexity warning threshold.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Complexity warning threshold")]
        [Description("The threshold at which items are considered to have a complexity score of status 'warning'.")]
        public int ComplexityWarningThreshold { get; set; }

        /// <summary>
        /// Gets or sets the option for the complexity alert threshold.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Complexity alert threshold")]
        [Description("The threshold at which items are considered to have a complexity score of status 'alert'.")]
        public int ComplexityAlertThreshold { get; set; }

        #endregion Public Properties

        #region Overrides

        /// <summary>
        /// Gets the window this options page will use for its UI.
        /// </summary>
        protected override IWin32Window Window
        {
            get { return new SnooperOptionsControl(this); }
        }

        #endregion Overrides
    }
}