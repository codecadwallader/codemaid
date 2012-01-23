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

using System.Windows.Forms;

namespace SteveCadwallader.CodeMaid.Spade
{
    /// <summary>
    /// The top level view hosted within the <see cref="SpadeToolWindow"/>.
    /// </summary>
    internal partial class SpadeViewHost : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeViewHost"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        internal SpadeViewHost(SpadeViewModel viewModel)
        {
            InitializeComponent();

            elementHost.Child = new SpadeView
                                    {
                                        DataContext = viewModel
                                    };
        }

        #endregion Constructors
    }
}