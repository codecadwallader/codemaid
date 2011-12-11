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

namespace SteveCadwallader.CodeMaid.Spade
{
    /// <summary>
    /// The WPF based control/view for the <see cref="SpadeToolWindow"/>.
    /// </summary>
    public partial class SpadeView
    {
        #region Fields

        private const string DragSource = "DragSource";

        private Point _startPoint;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeView"/> class.
        /// </summary>
        public SpadeView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region ShowDropAboveTarget (Attached Property)

        /// <summary>
        /// The dependency property definition for the ShowDropAboveTarget attached property.
        /// </summary>
        public static DependencyProperty ShowDropAboveTargetProperty = DependencyProperty.RegisterAttached(
            "ShowDropAboveTarget", typeof(bool), typeof(SpadeView));

        /// <summary>
        /// Gets the ShowDropAboveTarget value from the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>The value.</returns>
        public static bool GetShowDropAboveTarget(UIElement target)
        {
            return (bool)target.GetValue(ShowDropAboveTargetProperty);
        }

        /// <summary>
        /// Sets the ShowDropAboveTarget value on the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        public static void SetShowDropAboveTarget(UIElement target, bool value)
        {
            target.SetValue(ShowDropAboveTargetProperty, value);
        }

        #endregion ShowDropAboveTarget (Attached Property)

        #region ShowDropBelowTarget (Attached Property)

        /// <summary>
        /// The dependency property definition for the ShowDropBelowTarget attached property.
        /// </summary>
        public static DependencyProperty ShowDropBelowTargetProperty = DependencyProperty.RegisterAttached(
            "ShowDropBelowTarget", typeof(bool), typeof(SpadeView));

        /// <summary>
        /// Gets the ShowDropBelowTarget value from the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>The value.</returns>
        public static bool GetShowDropBelowTarget(UIElement target)
        {
            return (bool)target.GetValue(ShowDropBelowTargetProperty);
        }

        /// <summary>
        /// Sets the ShowDropBelowTarget value on the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        public static void SetShowDropBelowTarget(UIElement target, bool value)
        {
            target.SetValue(ShowDropBelowTargetProperty, value);
        }

        #endregion ShowDropBelowTarget (Attached Property)

        #region Properties

        /// <summary>
        /// Gets the view model.
        /// </summary>
        private SpadeViewModel ViewModel
        {
            get { return DataContext as SpadeViewModel; }
        }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Called when a KeyDown event is raised by a TreeViewItem (not automatically handled by TreeView).
        /// Used to jump to a code item upon enter, or toggle the expansion state upon space.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void OnTreeViewItemKeyDown(object sender, KeyEventArgs e)
        {
            var treeViewItem = e.Source as TreeViewItem;
            if (treeViewItem == null) return;

            switch (e.Key)
            {
                case Key.Return:
                    JumpToCodeItem(treeViewItem.DataContext as BaseCodeItem);
                    break;

                case Key.Space:
                    treeViewItem.IsExpanded = !treeViewItem.IsExpanded;
                    break;
            }
        }

        /// <summary>
        /// Called when the header of a TreeViewItem receives a mouse down event.
        /// Used to start detecting a drag and drop operation or toggle the expansion state depending on conditions.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void OnTreeViewItemHeaderMouseDown(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = FindParentTreeViewItem(e.Source);
            if (treeViewItem == null) return;

            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    _startPoint = e.GetPosition(null);
                    break;

                case MouseButton.Middle:
                    treeViewItem.IsExpanded = !treeViewItem.IsExpanded;
                    break;
            }
        }

        /// <summary>
        /// Called when the header of a TreeViewItem receives a mouse move event.
        /// Used to conditionally initiate a drag and drop operation.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
        private void OnTreeViewItemHeaderMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            var delta = _startPoint - e.GetPosition(null);
            if (Math.Abs(delta.X) <= SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(delta.Y) <= SystemParameters.MinimumVerticalDragDistance) return;

            var treeViewItem = FindParentTreeViewItem(sender);
            if (treeViewItem == null) return;

            var codeItem = treeViewItem.DataContext as BaseCodeItemElement;
            if (codeItem == null) return;

            treeViewItem.Opacity = 0.5;

            var dataObject = new DataObject(typeof(BaseCodeItemElement), codeItem);
            dataObject.SetData(DragSource, treeViewItem);

            DragDrop.DoDragDrop(treeViewItem, dataObject, DragDropEffects.Move);

            treeViewItem.Opacity = 1;
        }

        /// <summary>
        /// Called when the header of a TreeViewItem receives a mouse up event.
        /// Used to conditionally jump to a code item.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void OnTreeViewItemHeaderMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;

            var treeViewItem = FindParentTreeViewItem(e.Source);
            if (treeViewItem == null) return;

            JumpToCodeItem(treeViewItem.DataContext as BaseCodeItem);
        }

        /// <summary>
        /// Handles the drag events for a TreeViewItem header.
        /// Used to conditionally determine if a drop operation is allowed or not.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.DragEventArgs"/> instance containing the event data.</param>
        private void OnTreeViewItemHeaderDragEvent(object sender, DragEventArgs e)
        {
            var targetTreeViewItem = FindParentTreeViewItem(sender);

            if (targetTreeViewItem != null &&
                targetTreeViewItem != e.Data.GetData(DragSource) &&
                targetTreeViewItem.DataContext is BaseCodeItemElement &&
                e.Data.GetDataPresent(typeof(BaseCodeItemElement)))
            {
                bool isDropOnTopHalfOfTarget = IsDropOnTopHalfOfTarget(e, targetTreeViewItem);

                targetTreeViewItem.SetValue(ShowDropAboveTargetProperty, isDropOnTopHalfOfTarget);
                targetTreeViewItem.SetValue(ShowDropBelowTargetProperty, !isDropOnTopHalfOfTarget);
            }
            else
            {
                // Not a valid drop target.
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Called when the header of a TreeViewItem receives a drag leave event.
        /// Used to conditionally clear the show drop attached properties.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.DragEventArgs"/> instance containing the event data.</param>
        private void OnTreeViewItemHeaderDragLeave(object sender, DragEventArgs e)
        {
            var targetTreeViewItem = FindParentTreeViewItem(sender);
            if (targetTreeViewItem != null)
            {
                targetTreeViewItem.SetValue(ShowDropAboveTargetProperty, false);
                targetTreeViewItem.SetValue(ShowDropBelowTargetProperty, false);
            }
        }

        /// <summary>
        /// Called when the header of a TreeViewItem receives a drop event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.DragEventArgs"/> instance containing the event data.</param>
        private void OnTreeViewItemHeaderDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(BaseCodeItemElement))) return;

            var treeViewItem = FindParentTreeViewItem(sender);
            if (treeViewItem == null || e.Source == treeViewItem) return;

            var baseCodeItem = treeViewItem.DataContext as BaseCodeItemElement;
            if (baseCodeItem == null) return;

            var codeItemToMove = e.Data.GetData(typeof(BaseCodeItemElement)) as BaseCodeItemElement;
            if (codeItemToMove == null) return;

            if (IsDropOnTopHalfOfTarget(e, treeViewItem))
            {
                CodeReorderHelper.MoveItemAboveBase(codeItemToMove, baseCodeItem);
            }
            else
            {
                CodeReorderHelper.MoveItemBelowBase(codeItemToMove, baseCodeItem);
            }

            Refresh();
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Attempts to find the parent TreeViewItem from the specified event source.
        /// </summary>
        /// <param name="eventSource">The event source.</param>
        /// <returns>The parent TreeViewItem, otherwise null.</returns>
        private static TreeViewItem FindParentTreeViewItem(object eventSource)
        {
            var source = eventSource as DependencyObject;
            if (source == null) return null;

            var treeViewItem = source.FindVisualAncestor<TreeViewItem>();

            return treeViewItem;
        }

        /// <summary>
        /// Determines whether the drag event occurs in the top half of the specified target.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.DragEventArgs"/> instance containing the event data.</param>
        /// <param name="target">The target.</param>
        /// <returns>True if drag event occurs in the top half of the specified target, otherwise false.</returns>
        private static bool IsDropOnTopHalfOfTarget(DragEventArgs e, FrameworkElement target)
        {
            var dropPoint = e.GetPosition(target);

            return dropPoint.Y <= target.ActualHeight / 2;
        }

        /// <summary>
        /// Jumps to the specified code item.
        /// </summary>
        /// <param name="codeItem">The code item.</param>
        private void JumpToCodeItem(BaseCodeItem codeItem)
        {
            var viewModel = ViewModel;
            if (codeItem == null || viewModel == null || codeItem.StartLine <= 0) return;

            Dispatcher.BeginInvoke(
                new Action(() => TextDocumentHelper.MoveToCodeItem(viewModel.Document, codeItem, viewModel.Package.Options.Spade.CenterOnWhole)));
        }

        /// <summary>
        /// Requests a refresh of Spade.
        /// </summary>
        private void Refresh()
        {
            ViewModel.RequestRefresh();
        }

        #endregion Methods
    }
}