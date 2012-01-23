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
    /// An options page for cleanup file types options that are integrated into the IDE options window.
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    [Guid("2a6e3edf-7783-4c33-a899-9419d94e9f76")]
    public class CleanupFileTypesOptionsPage : DialogPage
    {
        #region Constants

        /// <summary>
        /// The default cleanup exclusion expression.
        /// </summary>
        public const string DefaultCleanupExclusionExpression = ".*.Designer.cs ; .*.resx";

        #endregion Constants

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupFileTypesOptionsPage"/> class.
        /// </summary>
        public CleanupFileTypesOptionsPage()
        {
            // Set the default settings, will be trumped if there are stored values.
            CleanupIncludeCPlusPlus = true;
            CleanupIncludeCSharp = true;
            CleanupIncludeCSS = true;
            CleanupIncludeHTML = true;
            CleanupIncludeJavaScript = true;
            CleanupIncludeXAML = true;
            CleanupIncludeXML = true;
            CleanupExclusionExpression = DefaultCleanupExclusionExpression;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the option to include C++ file types in cleanup.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Include C++ file types in cleanup")]
        [Description("C++ file types are included in cleanup operations.")]
        public bool CleanupIncludeCPlusPlus { get; set; }

        /// <summary>
        /// Gets or sets the option to include C# file types in cleanup.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Include C# file types in cleanup")]
        [Description("C# file types are included in cleanup operations.")]
        public bool CleanupIncludeCSharp { get; set; }

        /// <summary>
        /// Gets or sets the option to include CSS file types in cleanup.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Include CSS file types in cleanup")]
        [Description("CSS file types are included in cleanup operations.")]
        public bool CleanupIncludeCSS { get; set; }

        /// <summary>
        /// Gets or sets the option to include HTML file types in cleanup.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Include HTML file types in cleanup")]
        [Description("HTML file types are included in cleanup operations.")]
        public bool CleanupIncludeHTML { get; set; }

        /// <summary>
        /// Gets or sets the option to include JavaScript file types in cleanup.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Include JavaScript file types in cleanup")]
        [Description("JavaScript file types are included in cleanup operations.")]
        public bool CleanupIncludeJavaScript { get; set; }

        /// <summary>
        /// Gets or sets the option to include XAML file types in cleanup.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Include XAML file types in cleanup")]
        [Description("XAML file types are included in cleanup operations.")]
        public bool CleanupIncludeXAML { get; set; }

        /// <summary>
        /// Gets or sets the option to include XML file types in cleanup.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Include XML file types in cleanup")]
        [Description("XML file types are included in cleanup operations.")]
        public bool CleanupIncludeXML { get; set; }

        /// <summary>
        /// Gets or sets the expression for files to exclude from cleanup.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Expression for files to exclude from cleanup")]
        [Description("An expresion representing files to exclude from cleanup operations.")]
        public string CleanupExclusionExpression { get; set; }

        #endregion Public Properties

        #region Overrides

        /// <summary>
        /// Gets the window this options page will use for its UI.
        /// </summary>
        protected override IWin32Window Window
        {
            get { return new CleanupFileTypesOptionsControl(this); }
        }

        #endregion Overrides
    }
}