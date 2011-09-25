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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SteveCadwallader.CodeMaid.CodeItems;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Quidnunc
{
    /// <summary>
    /// The WPF based control/view for the <see cref="QuidnuncToolWindow"/>.
    /// </summary>
    public partial class QuidnuncView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuidnuncView"/> class.
        /// </summary>
        public QuidnuncView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        private QuidnuncViewModel ViewModel
        {
            get { return DataContext as QuidnuncViewModel; }
        }

        /// <summary>
        /// Called when when the SelectedItem has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The event arguments containing the event data.</param>
        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> eventArgs)
        {
            var codeItem = eventArgs.NewValue as BaseCodeItem;
            var viewModel = ViewModel;
            if (codeItem == null || viewModel == null) return;

            Dispatcher.BeginInvoke(
                new Action(() => TextDocumentHelper.MoveToCodeItem(viewModel.Document, codeItem, viewModel.Package.Options.Spade.CenterOnWhole)));
        }

        /// <summary>
        /// Called when the header of a TreeViewItem receives a mouse down event.
        /// Used to toggle the expansion of a tree view item upon middle click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void OnTreeViewItemHeaderMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Middle) return;

            var source = e.Source as DependencyObject;
            if (source == null) return;

            var treeViewItem = source.FindVisualAncestor<TreeViewItem>();
            if (treeViewItem != null)
            {
                treeViewItem.IsExpanded = !treeViewItem.IsExpanded;
            }
        }
    }
}