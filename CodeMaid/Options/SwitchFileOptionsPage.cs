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
    /// An options page for switch file options that are integrated into the IDE options window.
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    [Guid("0e984617-c912-44ac-bd8b-1930c0d00198")]
    public class SwitchFileOptionsPage : DialogPage
    {
        #region Constants

        /// <summary>
        /// The default related file extensions expression.
        /// </summary>
        public const string DefaultRelatedFileExtensionsExpression = ".cpp .h ; .xaml .xaml.cs ; .xml .xsd";

        #endregion Constants

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchFileOptionsPage"/> class.
        /// </summary>
        public SwitchFileOptionsPage()
        {
            // Set the default settings, will be trumped if there are stored values.
            RelatedFileExtensionsExpression = DefaultRelatedFileExtensionsExpression;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the expression for related file extensions.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Expression for related file extensions")]
        [Description("An expression representing related file extensions, used to switch between related files.")]
        public string RelatedFileExtensionsExpression { get; set; }

        #endregion Public Properties

        #region Overrides

        /// <summary>
        /// Gets the window this options page will use for its UI.
        /// </summary>
        protected override IWin32Window Window
        {
            get { return new SwitchFileOptionsControl(this); }
        }

        #endregion Overrides
    }
}