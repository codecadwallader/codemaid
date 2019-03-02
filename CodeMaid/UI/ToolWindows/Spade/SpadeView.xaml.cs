using Microsoft.VisualStudio.Shell;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Logic.Reorganizing;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using SteveCadwallader.CodeMaid.UI.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace SteveCadwallader.CodeMaid.UI.ToolWindows.Spade
{
    /// <summary>
    /// The WPF based control/view for the <see cref="SpadeToolWindow" />.
    /// </summary>
    public partial class SpadeView
    {
        #region Fields

        private CodeReorganizationManager _codeReorganizationManager;
        private TreeViewItem _dragCandidate;
        private Point? _dragStartPoint;
        private bool _isDoubleClick;
        private ScrollViewer _scrollViewer;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeView" /> class.
        /// </summary>
        public SpadeView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the lazy-initialized code reorganization manager.
        /// </summary>
        private CodeReorganizationManager CodeReorganizationManager =>
            _codeReorganizationManager ?? (_codeReorganizationManager = CodeReorganizationManager.GetInstance(ViewModel.Package));

        /// <summary>
        /// Gets the lazy-initialized scroll viewer.
        /// </summary>
        private ScrollViewer ScrollViewer => _scrollViewer ?? (_scrollViewer = this.FindVisualChild<ScrollViewer>());

        /// <summary>
        /// Gets the view model.
        /// </summary>
        private SpadeViewModel ViewModel => DataContext as SpadeViewModel;

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
            var value = (double)basevalue;

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
        /// <param name="e">
        /// The <see cref="System.Windows.Input.MouseButtonEventArgs" /> instance containing the
        /// event data.
        /// </param>
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
        /// <param name="e">
        /// The <see cref="System.Windows.Input.MouseWheelEventArgs" /> instance containing the
        /// event data.
        /// </param>
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
        /// Called when a KeyDown event is raised by a TreeViewItem (not automatically handled by
        /// TreeView) . Used to jump to a code item upon enter, or toggle the expansion state upon space.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.KeyEventArgs" /> instance containing the event data.
        /// </param>
        private void OnTreeViewItemKeyDown(object sender, KeyEventArgs e)
        {
            var treeViewItem = e.Source as TreeViewItem;
            if (treeViewItem == null || Keyboard.Modifiers != ModifierKeys.None) return;

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
        /// Called when the header of a TreeViewItem receives a mouse down event. Used to start
        /// detecting a drag and drop operation or toggle the expansion state depending on conditions.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.MouseButtonEventArgs" /> instance containing the
        /// event data.
        /// </param>
        private void OnTreeViewItemHeaderMouseDown(object sender, MouseButtonEventArgs e)
        {
            _isDoubleClick = false;

            var treeViewItem = FindParentTreeViewItem(e.Source);
            if (treeViewItem == null) return;

            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    if (e.ClickCount == 2)
                    {
                        _isDoubleClick = true;
                        e.Handled = true;
                    }
                    else if (treeViewItem.DataContext is BaseCodeItem)
                    {
                        _dragCandidate = treeViewItem;
                        _dragStartPoint = e.GetPosition(null);
                    }
                    break;

                case MouseButton.Middle:
                    treeViewItem.IsExpanded = !treeViewItem.IsExpanded;
                    break;
            }
        }

        /// <summary>
        /// Called when the header of a TreeViewItem receives a mouse move event. Used to
        /// conditionally initiate a drag and drop operation.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.MouseEventArgs" /> instance containing the event data.
        /// </param>
        private void OnTreeViewItemHeaderMouseMove(object sender, MouseEventArgs e)
        {
            if (_dragCandidate == null || !_dragStartPoint.HasValue) return;

            var delta = _dragStartPoint.Value - e.GetPosition(null);
            if (Math.Abs(delta.X) <= SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(delta.Y) <= SystemParameters.MinimumVerticalDragDistance)
            {
                return;
            }

            var selectedTreeViewItems = GetSelectedTreeViewItemsIncluding(_dragCandidate);

            foreach (var selectedTreeViewItem in selectedTreeViewItems)
            {
                selectedTreeViewItem.SetValue(DragDropAttachedProperties.IsBeingDraggedProperty, true);
            }

            DragDrop.DoDragDrop(_dragCandidate, new DataObject(typeof(IList<BaseCodeItem>), ViewModel.SelectedItems), DragDropEffects.Move);

            foreach (var selectedTreeViewItem in selectedTreeViewItems)
            {
                selectedTreeViewItem.SetValue(DragDropAttachedProperties.IsBeingDraggedProperty, false);
            }

            _dragCandidate = null;
            _dragStartPoint = null;
        }

        /// <summary>
        /// Called when the header of a TreeViewItem receives a mouse up event. Used to
        /// conditionally jump to a code item.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.MouseButtonEventArgs" /> instance containing the
        /// event data.
        /// </param>
        private void OnTreeViewItemHeaderMouseUp(object sender, MouseButtonEventArgs e)
        {
            _dragCandidate = null;
            _dragStartPoint = null;

            var treeViewItem = FindParentTreeViewItem(e.Source);
            if (treeViewItem == null) return;

            var baseCodeItem = treeViewItem.DataContext as BaseCodeItem;

            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    if (_isDoubleClick)
                    {
                        SelectCodeItem(baseCodeItem);
                    }
                    else
                    {
                        JumpToCodeItem(baseCodeItem);
                    }
                    break;

                case MouseButton.Right:
                    GetSelectedTreeViewItemsIncluding(treeViewItem);
                    ShowContextMenu(PointToScreen(e.GetPosition(this)));
                    break;
            }
        }

        /// <summary>
        /// Handles the drag events for a TreeViewItem header. Used to conditionally determine if a
        /// drop operation is allowed or not.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.DragEventArgs"/> instance containing the event data.
        /// </param>
        private void OnTreeViewItemHeaderDragEvent(object sender, DragEventArgs e)
        {
            HandleDragScrolling(ScrollViewer, e);

            var targetTreeViewItem = FindParentTreeViewItem(sender);

            if (targetTreeViewItem != null &&
                e.Data.GetDataPresent(typeof(IList<BaseCodeItem>)))
            {
                var baseCodeItem = targetTreeViewItem.DataContext as BaseCodeItem;
                var codeItemsToMove = e.Data.GetData(typeof(IList<BaseCodeItem>)) as IList<BaseCodeItem>;

                if (baseCodeItem != null && codeItemsToMove != null &&
                    !codeItemsToMove.Contains(baseCodeItem) &&
                    !codeItemsToMove.Any(x => IsItemAncestorOfBase(x, baseCodeItem)))
                {
                    switch (GetDropPosition(e, baseCodeItem, targetTreeViewItem))
                    {
                        case DropPosition.Above:
                            targetTreeViewItem.SetValue(DragDropAttachedProperties.IsDropAboveTargetProperty, true);
                            targetTreeViewItem.SetValue(DragDropAttachedProperties.IsDropBelowTargetProperty, false);
                            targetTreeViewItem.SetValue(DragDropAttachedProperties.IsDropOnTargetProperty, false);
                            break;

                        case DropPosition.Below:
                            targetTreeViewItem.SetValue(DragDropAttachedProperties.IsDropAboveTargetProperty, false);
                            targetTreeViewItem.SetValue(DragDropAttachedProperties.IsDropBelowTargetProperty, true);
                            targetTreeViewItem.SetValue(DragDropAttachedProperties.IsDropOnTargetProperty, false);
                            break;

                        case DropPosition.On:
                            targetTreeViewItem.SetValue(DragDropAttachedProperties.IsDropAboveTargetProperty, false);
                            targetTreeViewItem.SetValue(DragDropAttachedProperties.IsDropBelowTargetProperty, false);
                            targetTreeViewItem.SetValue(DragDropAttachedProperties.IsDropOnTargetProperty, true);
                            break;
                    }

                    e.Effects = DragDropEffects.Move;
                    e.Handled = true;
                    return;
                }
            }

            // Not a valid drop target.
            e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        /// <summary>
        /// Called when the header of a TreeViewItem receives a drag leave event. Used to
        /// conditionally clear the show drop attached properties.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.DragEventArgs" /> instance containing the event data.
        /// </param>
        private void OnTreeViewItemHeaderDragLeave(object sender, DragEventArgs e)
        {
            var targetTreeViewItem = FindParentTreeViewItem(sender);
            if (targetTreeViewItem != null)
            {
                targetTreeViewItem.SetValue(DragDropAttachedProperties.IsDropAboveTargetProperty, false);
                targetTreeViewItem.SetValue(DragDropAttachedProperties.IsDropBelowTargetProperty, false);
                targetTreeViewItem.SetValue(DragDropAttachedProperties.IsDropOnTargetProperty, false);
            }
        }

        /// <summary>
        /// Called when the header of a TreeViewItem receives a drop event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.DragEventArgs" /> instance containing the event data.
        /// </param>
        private void OnTreeViewItemHeaderDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(IList<BaseCodeItem>))) return;

            var treeViewItem = FindParentTreeViewItem(sender);
            if (treeViewItem == null || ReferenceEquals(e.Source, treeViewItem)) return;

            var baseCodeItem = treeViewItem.DataContext as BaseCodeItem;
            if (baseCodeItem == null) return;

            var codeItemsToMove = e.Data.GetData(typeof(IList<BaseCodeItem>)) as IList<BaseCodeItem>;
            if (codeItemsToMove == null) return;

            switch (GetDropPosition(e, baseCodeItem, treeViewItem))
            {
                case DropPosition.Above:
                    foreach (var codeItemToMove in codeItemsToMove.OrderBy(x => x.StartLine))
                    {
                        CodeReorganizationManager.MoveItemAboveBase(codeItemToMove, baseCodeItem);
                    }
                    break;

                case DropPosition.Below:
                    foreach (var codeItemToMove in codeItemsToMove.OrderByDescending(x => x.StartLine))
                    {
                        CodeReorganizationManager.MoveItemBelowBase(codeItemToMove, baseCodeItem);
                    }
                    break;

                case DropPosition.On:
                    foreach (var codeItemToMove in codeItemsToMove.OrderByDescending(x => x.StartLine))
                    {
                        CodeReorganizationManager.MoveItemIntoBase(codeItemToMove, baseCodeItem as ICodeItemParent);
                    }
                    break;
            }

            Refresh();
            e.Handled = true;
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Gets the selected tree view items, ensuring the specified item is one of them. If it is
        /// not, all other selections are cleared and it is the only item selected.
        /// </summary>
        /// <param name="treeViewItem">The tree view item that must be selected.</param>
        /// <returns>The set of selected tree view items, guaranteed to include the specified one.</returns>
        private IList<TreeViewItem> GetSelectedTreeViewItemsIncluding(TreeViewItem treeViewItem)
        {
            // Get the currently selected tree view items.
            var selectedTreeViewItems = treeView.FindVisualChildren<TreeViewItem>()
                .Where(TreeViewMultipleSelectionBehavior.GetIsItemSelected)
                .ToList();

            // If the specified tree view item is not selected, change the current selection to it.
            if (!selectedTreeViewItems.Contains(treeViewItem))
            {
                var behavior = Interaction.GetBehaviors(treeView).OfType<TreeViewMultipleSelectionBehavior>().FirstOrDefault();
                behavior?.SelectSingleItem(treeViewItem);

                selectedTreeViewItems.Clear();
                selectedTreeViewItems.Add(treeViewItem);
            }

            return selectedTreeViewItems;
        }

        /// <summary>
        /// Attempts to find the parent TreeViewItem from the specified event source.
        /// </summary>
        /// <param name="eventSource">The event source.</param>
        /// <returns>The parent TreeViewItem, otherwise null.</returns>
        private static TreeViewItem FindParentTreeViewItem(object eventSource)
        {
            var source = eventSource as DependencyObject;

            var treeViewItem = source?.FindVisualAncestor<TreeViewItem>();

            return treeViewItem;
        }

        /// <summary>
        /// Determines the drop position for the specified drag event and the drop target.
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.Windows.DragEventArgs" /> instance containing the event data.
        /// </param>
        /// <param name="targetItem">The target item.</param>
        /// <param name="targetElement">The target element.</param>
        /// <returns>The drop position.</returns>
        private static DropPosition GetDropPosition(DragEventArgs e, BaseCodeItem targetItem, TreeViewItem targetElement)
        {
            var header = targetElement.Template.FindName("PART_HeaderBorder", targetElement) as FrameworkElement;
            var targetHeight = header?.ActualHeight ?? targetElement.ActualHeight;
            var dropPoint = e.GetPosition(targetElement);
            bool canDropOn = targetItem is ICodeItemParent;

            if (canDropOn)
            {
                bool isTopThird = dropPoint.Y <= targetHeight / 3;
                bool isBottomThird = dropPoint.Y > targetHeight * 2 / 3;

                return isTopThird ? DropPosition.Above : (isBottomThird ? DropPosition.Below : DropPosition.On);
            }

            bool isTopHalf = dropPoint.Y <= targetHeight / 2;

            return isTopHalf ? DropPosition.Above : DropPosition.Below;
        }

        /// <summary>
        /// Handles scrolling the specified scroll viewer if the drag event indicates the drag
        /// operation is nearing the scroll viewers top or bottom boundaries.
        /// </summary>
        /// <param name="scrollViewer">The scroll viewer.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.DragEventArgs" /> instance containing the event data.
        /// </param>
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
        /// Determines if the specified item is an ancestor of the specified base.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="baseItem">The base item.</param>
        /// <returns>True if item is an ancestor of the specified base, otherwise false.</returns>
        private static bool IsItemAncestorOfBase(BaseCodeItem item, BaseCodeItem baseItem)
        {
            var itemAsParent = item as ICodeItemParent;
            if (itemAsParent == null)
            {
                return false;
            }

            return itemAsParent.Children.Contains(baseItem) ||
                   itemAsParent.Children.Any(x => IsItemAncestorOfBase(x, baseItem));
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
                new Action(() => TextDocumentHelper.MoveToCodeItem(viewModel.Document, codeItem, Settings.Default.Digging_CenterOnWhole)));
        }

        /// <summary>
        /// Requests a refresh of Spade.
        /// </summary>
        private void Refresh()
        {
            ViewModel.RequestRefresh();
        }

        /// <summary>
        /// Selects the specified code item.
        /// </summary>
        /// <param name="codeItem">The code item.</param>
        private void SelectCodeItem(BaseCodeItem codeItem)
        {
            var viewModel = ViewModel;
            if (codeItem == null || viewModel == null || codeItem.StartOffset <= 0) return;

            Dispatcher.BeginInvoke(
                new Action(() => TextDocumentHelper.SelectCodeItem(viewModel.Document, codeItem)));
        }

        /// <summary>
        /// Shows a context menu at the specified point.
        /// </summary>
        /// <param name="point">The point where the context menu should be shown.</param>
        private void ShowContextMenu(Point point)
        {
            if (ViewModel?.Package is var package)
            {
                package.JoinableTaskFactory.RunAsync(async () =>
                {
                    if (await package.GetServiceAsync(typeof(IMenuCommandService)) is OleMenuCommandService menuCommandService)
                    {
                        var contextMenuCommandID = new CommandID(PackageGuids.GuidCodeMaidMenuSet, PackageIds.MenuIDCodeMaidContextSpade);

                        menuCommandService.ShowContextMenu(contextMenuCommandID, (int)point.X, (int)point.Y);
                    }
                });
            }
        }

        #endregion Methods
    }
}