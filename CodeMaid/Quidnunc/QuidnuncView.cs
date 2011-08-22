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

using System.Windows.Forms;

namespace SteveCadwallader.CodeMaid.Quidnunc
{
    /// <summary>
    /// The top level view hosted within the <see cref="QuidnuncToolWindow"/>.
    /// </summary>
    internal partial class QuidnuncView : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QuidnuncView"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        internal QuidnuncView(QuidnuncViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the view model.
        /// </summary>
        internal QuidnuncViewModel ViewModel { get; private set; }

        #endregion Properties
    }
}