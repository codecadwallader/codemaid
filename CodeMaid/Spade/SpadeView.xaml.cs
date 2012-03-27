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

        private CodeReorderHelper _codeReorderHelper;
        private TreeViewItem _dragCandidate;
        private ScrollViewer _scrollViewer;
        private Point? _startPoint;

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

        #region Properties

        /// <summary>
        /// Gets the lazy-initialized code reorder helper.
        /// </summary>
        private CodeReorderHelper CodeReorderHelper
        {
            get { return _codeReorderHelper ?? (_codeReorderHelper = CodeReorderHelper.GetInstance(ViewModel.Package)); }
        }

        /// <summary>
        /// Gets the lazy-initialized scroll viewer.
        /// </summary>
        private ScrollViewer ScrollViewer
        {
            get { return _scrollViewer ?? (_scrollViewer = this.FindVisualChild<ScrollViewer>()); }
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        private SpadeViewModel ViewModel
        {
            get { return DataContext as SpadeViewModel; }
        }

        #endregion Properties

        #region ScaleFactor (Dependency Property)

        /// <summary>
        /// The dependency property definition for the ScaleFactor property.
        /// </summary>
        public static DependencyProperty ScaleFactorProperty = DependencyProperty.Register(
            "ScaleFactor", typeof(double), typeof(SpadeView),
            new FrameworkPropertyMetadata(1.0d, null, OnCoerceScaleFactor));

        /// <summary>
        /// Gets or sets the scale factor.
        /// </summary>
        public double ScaleFactor
        {
            get { return (double)GetValue(ScaleFactorProperty); }
            set { SetValue(ScaleFactorProperty, value); }
        }

        /// <summary>
        /// Called to coerce the value of the ScaleFactor.
        /// </summary>
        /// <param name="obj">The dependency object where the value has changed..</param>
        /// <param name="basevalue">The base value.</param>
        /// <returns>The coerced value.</returns>
        private static object OnCoerceScaleFactor(DependencyObject obj, object basevalue)
        {
            double value = (double)basevalue;

            value = Math.Max(0.2, value);
            value = Math.Min(5.0, value);

            return value;
        }

        #endregion ScaleFactor (Dependency Property)

        #region Event Handlers

        /// <summary>
        /// Called when a PreviewMouseDown event is received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control &&
                e.ChangedButton == MouseButton.Middle &&
                e.ButtonState == MouseButtonState.Pressed)
            {
                ClearValue(ScaleFactorProperty);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Called when a PreviewMouseWheel event is received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseWheelEventArgs"/> instance containing the event data.</param>
        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Delta != 0)
            {
                var steps = e.Delta / 120;
                var percentage = 1 + (0.05 * steps);

                ScaleFactor *= percentage;
                e.Handled = true;
            }
        }

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
                    if (treeViewItem.DataContext is BaseCodeItemElement)
                    {
                        _dragCandidate = treeViewItem;
                        _startPoint = e.GetPosition(null);
                    }
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
            if (_dragCandidate == null || !_startPoint.HasValue) return;

            var delta = _startPoint.Value - e.GetPosition(null);
            if (Math.Abs(delta.X) <= SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(delta.Y) <= SystemParameters.MinimumVerticalDragDistance) return;

            var codeItem = _dragCandidate.DataContext as BaseCodeItemElement;
            if (codeItem == null) return;

            _dragCandidate.SetValue(DragDropAttachedProperties.IsBeingDraggedProperty, true);

            DragDrop.DoDragDrop(_dragCandidate, new DataObject(typeof(BaseCodeItemElement), codeItem), DragDropEffects.Move);

            _dragCandidate.SetValue(DragDropAttachedProperties.IsBeingDraggedProperty, false);

            _dragCandidate = null;
            _startPoint = null;
        }

        /// <summary>
        /// Called when the header of a TreeViewItem receives a mouse up event.
        /// Used to conditionally jump to a code item.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void OnTreeViewItemHeaderMouseUp(object sender, MouseButtonEventArgs e)
        {
            _dragCandidate = null;
            _startPoint = null;

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
            HandleDragScrolling(ScrollViewer, e);

            var targetTreeViewItem = FindParentTreeViewItem(sender);

            if (targetTreeViewItem != null &&
                e.Data.GetDataPresent(typeof(BaseCodeItemElement)))
            {
                var baseCodeItem = targetTreeViewItem.DataContext as BaseCodeItemElement;
                var codeItemToMove = e.Data.GetData(typeof(BaseCodeItemElement)) as BaseCodeItemElement;

                if (baseCodeItem != null && codeItemToMove != null && baseCodeItem != codeItemToMove && !codeItemToMove.IsAncestorOf(baseCodeItem))
                {
                    bool isDropOnTopHalfOfTarget = IsDropOnTopHalfOfTarget(e, targetTreeViewItem);

                    targetTreeViewItem.SetValue(DragDropAttachedProperties.IsDropAboveTargetProperty, isDropOnTopHalfOfTarget);
                    targetTreeViewItem.SetValue(DragDropAttachedProperties.IsDropBelowTargetProperty, !isDropOnTopHalfOfTarget);
                    return;
                }
            }

            // Not a valid drop target.
            e.Effects = DragDropEffects.None;
            e.Handled = true;
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
                targetTreeViewItem.SetValue(DragDropAttachedProperties.IsDropAboveTargetProperty, false);
                targetTreeViewItem.SetValue(DragDropAttachedProperties.IsDropBelowTargetProperty, false);
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
        /// Handles scrolling the specified scroll viewer if the drag event indicates the drag operation
        /// is nearing the scroll viewers top or bottom boundaries.
        /// </summary>
        /// <param name="scrollViewer">The scroll viewer.</param>
        /// <param name="e">The <see cref="System.Windows.DragEventArgs"/> instance containing the event data.</param>
        private static void HandleDragScrolling(ScrollViewer scrollViewer, DragEventArgs e)
        {
            const int threshold = 20;
            const int offset = 10;

            var mousePoint = e.GetPosition(scrollViewer);

            if (mousePoint.Y < threshold)
            {
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - offset);
            }
            else if (mousePoint.Y > (scrollViewer.ActualHeight - threshold))
            {
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + offset);
            }
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
            if (codeItem == null || viewModel == null || codeItem.StartOffset <= 0) return;

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