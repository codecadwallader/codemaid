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
    /// An options page for build progress options that are integrated into the IDE options window.
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    [Guid("7e14280f-a7ec-4326-bb1e-57e0f5053920")]
    public class BuildProgressOptionsPage : DialogPage
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildProgressOptionsPage"/> class.
        /// </summary>
        public BuildProgressOptionsPage()
        {
            // Set the default settings, will be trumped if there are stored values.
            AutoShowBuildProgressOnBuildStart = true;
            AutoHideBuildProgressOnBuildStop = true;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the option to automatically show the build progress tool window.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Automatically show build progress on start of build")]
        [Description("When a build starts, automatically show the build progress tool window.")]
        public bool AutoShowBuildProgressOnBuildStart { get; set; }

        /// <summary>
        /// Gets or sets the option to automatically hide the build progress tool window.
        /// </summary>
        [Category("CodeMaid")]
        [DisplayName(@"Automatically hide build progress on stop of build")]
        [Description("When a build stops, automatically hide the build progress tool window.")]
        public bool AutoHideBuildProgressOnBuildStop { get; set; }

        #endregion Public Properties

        #region Overrides

        /// <summary>
        /// Gets the window this options page will use for its UI.
        /// </summary>
        protected override IWin32Window Window
        {
            get { return new BuildProgressOptionsControl(this); }
        }

        #endregion Overrides
    }
}