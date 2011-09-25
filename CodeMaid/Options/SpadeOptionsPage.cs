#region CodeMaid is Copyright 2007-2011 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2011 Steve Cadwallader.

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;

namespace SteveCadwallader.CodeMaid.Options
{
    /// <summary>
    /// An options page for spade options that are integrated into the IDE options window.
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    [Guid("bff0adb0-1a53-4e58-bfb4-b38ec00487b7")]
    public class SpadeOptionsPage : DialogPage
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeOptionsPage"/> class.
        /// </summary>
        public SpadeOptionsPage()
        {
            // Set the default settings, will be trumped if there are stored values.
            CenterOnWhole = true;
            ComplexityWarningThreshold = 10;
            ComplexityAlertThreshold = 15;
            ShowItemComplexity = true;
            ShowItemMetadata = true;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the option for centering on the whole item upon navigation.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Center on whole item upon navigation")]
        [Description("An option to center on the whole item upon navigation")]
        public bool CenterOnWhole { get; set; }

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

        /// <summary>
        /// Gets or sets the option to show the item complexity.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Show itemcomplexity")]
        [Description("An option to determine if the item complexity should be shown.")]
        public bool ShowItemComplexity { get; set; }

        /// <summary>
        /// Gets or sets the option to show item metadata.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Show item metadata")]
        [Description("An option to determine if the item metadata should be shown.")]
        public bool ShowItemMetadata { get; set; }

        #endregion Public Properties

        #region Overrides

        /// <summary>
        /// Gets the window this options page will use for its UI.
        /// </summary>
        protected override IWin32Window Window
        {
            get { return new SpadeOptionsControl(this); }
        }

        #endregion Overrides
    }
}