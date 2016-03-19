using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace SteveCadwallader.CodeMaid.UI
{
    /// <summary>
    /// A behavior that extends a <see cref="TreeView"/> with multiple selection capabilities.
    /// </summary>
    /// <remarks>
    /// Largely based on http://chrigas.blogspot.com/2014/08/wpf-treeview-with-multiple-selection.html
    /// </remarks>
    public class TreeViewMultipleSelectionBehavior : Behavior<TreeView>
    {
        #region SelectedItems (Public Dependency Property)

        /// <summary>
        /// The dependency property definition for the SelectedItems property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
            "SelectedItems", typeof(IList), typeof(TreeViewMultipleSelectionBehavior));

        /// <summary>
        /// Gets or sets the selected items.
        /// </summary>
        public IList SelectedItems
        {
            get { return (IList)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        #endregion SelectedItems (Public Dependency Property)

        #region AnchorItem (Private Dependency Property)

        /// <summary>
        /// The dependency property definition for the AnchorItem property.
        /// </summary>
        private static readonly DependencyProperty AnchorItemProperty = DependencyProperty.Register(
            "AnchorItem", typeof(TreeViewItem), typeof(TreeViewMultipleSelectionBehavior));

        /// <summary>
        /// Gets or sets the anchor item.
        /// </summary>
        private TreeViewItem AnchorItem
        {
            get { return (TreeViewItem)GetValue(AnchorItemProperty); }
            set { SetValue(AnchorItemProperty, value); }
        }

        #endregion AnchorItem (Private Dependency Property)

        #region IsItemSelected (TreeViewItem Attached Property)

        /// <summary>
        /// The dependency property definition for the IsItemSelected attached property.
        /// </summary>
        public static readonly DependencyProperty IsItemSelectedProperty = DependencyProperty.RegisterAttached(
            "IsItemSelected", typeof(bool), typeof(TreeViewMultipleSelectionBehavior),
            new FrameworkPropertyMetadata(OnIsItemSelectedChanged));

        /// <summary>
        /// Gets the IsItemSelected value from the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>The value.</returns>
        public static bool GetIsItemSelected(TreeViewItem target)
        {
            return (bool)target.GetValue(IsItemSelectedProperty);
        }

        /// <summary>
        /// Sets the IsItemSelected value on the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        public static void SetIsItemSelected(TreeViewItem target, bool value)
        {
            target.SetValue(IsItemSelectedProperty, value);
        }

        /// <summary>
        /// Called when the IsItemSelected dependency property has changed.
        /// </summary>
        /// <param name="obj">The dependency object where the value has changed.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsItemSelectedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var treeViewItem = obj as TreeViewItem;
            var treeView = treeViewItem?.FindVisualAncestor<TreeView>();
            if (treeView != null)
            {
                var behavior = Interaction.GetBehaviors(treeView).OfType<TreeViewMultipleSelectionBehavior>().FirstOrDefault();
                var selectedItems = behavior?.SelectedItems;
                if (selectedItems != null)
                {
                    if (GetIsItemSelected(treeViewItem))
                    {
                        selectedItems.Add(treeViewItem.DataContext);
                    }
                    else
                    {
                        selectedItems.Remove(treeViewItem.DataContext);
                    }
                }
            }
        }

        #endregion IsItemSelected (TreeViewItem Attached Property)

        #region Behavior

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.AddHandler(UIElement.KeyDownEvent, new KeyEventHandler(OnTreeViewItemKeyDown), true);
            AssociatedObject.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(OnTreeViewItemMouseUp), true);
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has
        /// actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.RemoveHandler(UIElement.KeyDownEvent, new KeyEventHandler(OnTreeViewItemKeyDown));
            AssociatedObject.RemoveHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(OnTreeViewItemMouseUp));
        }

        #endregion Behavior

        #region Event Handlers

        /// <summary>
        /// Called when a TreeViewItem receives a key down event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.
        /// </param>
        private void OnTreeViewItemKeyDown(object sender, KeyEventArgs e)
        {
            var treeViewItem = e.OriginalSource as TreeViewItem;
            if (treeViewItem != null)
            {
                TreeViewItem targetItem = null;

                switch (e.Key)
                {
                    case Key.Down:
                        targetItem = GetRelativeItem(treeViewItem, 1);
                        break;

                    case Key.Space:
                        if (Keyboard.Modifiers == ModifierKeys.Control)
                        {
                            ToggleSingleItem(treeViewItem);
                        }
                        break;

                    case Key.Up:
                        targetItem = GetRelativeItem(treeViewItem, -1);
                        break;
                }

                if (targetItem != null)
                {
                    switch (Keyboard.Modifiers)
                    {
                        case ModifierKeys.Control:
                            Keyboard.Focus(targetItem);
                            break;

                        case ModifierKeys.Shift:
                            SelectMultipleItemsContinuously(targetItem);
                            break;

                        case ModifierKeys.None:
                            SelectSingleItem(targetItem);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Called when a TreeViewItem receives a mouse up event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the
        /// event data.
        /// </param>
        private void OnTreeViewItemMouseUp(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = FindParentTreeViewItem(e.OriginalSource);
            if (treeViewItem != null)
            {
                switch (Keyboard.Modifiers)
                {
                    case ModifierKeys.Control:
                        ToggleSingleItem(treeViewItem);
                        break;

                    case ModifierKeys.Shift:
                        SelectMultipleItemsContinuously(treeViewItem);
                        break;

                    default:
                        SelectSingleItem(treeViewItem);
                        break;
                }
            }
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Selects a range of consecutive items from the specified tree view item to the anchor (if exists).
        /// </summary>
        /// <param name="treeViewItem">The triggering tree view item.</param>
        public void SelectMultipleItemsContinuously(TreeViewItem treeViewItem)
        {
            if (AnchorItem != null)
            {
                if (ReferenceEquals(AnchorItem, treeViewItem))
                {
                    SelectSingleItem(treeViewItem);
                    return;
                }

                var isBetweenAnchors = false;
                var items = DeSelectAll();

                foreach (var item in items)
                {
                    if (ReferenceEquals(item, treeViewItem) || ReferenceEquals(item, AnchorItem))
                    {
                        // Toggle isBetweenAnchors when first item is found, and back again when last item is found.
                        isBetweenAnchors = !isBetweenAnchors;

                        SetIsItemSelected(item, true);
                    }
                    else if (isBetweenAnchors)
                    {
                        SetIsItemSelected(item, true);
                    }
                }
            }
        }

        /// <summary>
        /// Selects the specified tree view item, removing any other selections.
        /// </summary>
        /// <param name="treeViewItem">The triggering tree view item.</param>
        public void SelectSingleItem(TreeViewItem treeViewItem)
        {
            DeSelectAll();
            SetIsItemSelected(treeViewItem, true);
            AnchorItem = treeViewItem;
        }

        /// <summary>
        /// Toggles the selection state of the specified tree view item.
        /// </summary>
        /// <param name="treeViewItem">The triggering tree view item.</param>
        public void ToggleSingleItem(TreeViewItem treeViewItem)
        {
            SetIsItemSelected(treeViewItem, !GetIsItemSelected(treeViewItem));

            if (AnchorItem == null)
            {
                if (GetIsItemSelected(treeViewItem))
                {
                    AnchorItem = treeViewItem;
                }
            }
            else if (SelectedItems.Count == 0)
            {
                AnchorItem = null;
            }
        }

        /// <summary>
        /// Clears all selections.
        /// </summary>
        /// <remarks>
        /// The list of all items is returned as a convenience to avoid multiple iterations.
        /// </remarks>
        /// <returns>The list of all items.</returns>
        private IEnumerable<TreeViewItem> DeSelectAll()
        {
            var items = GetItemsRecursively<TreeViewItem>(AssociatedObject);
            foreach (var item in items)
            {
                SetIsItemSelected(item, false);
            }

            return items;
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
        /// Gets items of the specified type recursively from the specified parent item.
        /// </summary>
        /// <typeparam name="T">The type of item to retrieve.</typeparam>
        /// <param name="parentItem">The parent item.</param>
        /// <returns>The list of items within the parent item, may be empty.</returns>
        private static IList<T> GetItemsRecursively<T>(ItemsControl parentItem)
            where T : ItemsControl
        {
            if (parentItem == null)
            {
                throw new ArgumentNullException(nameof(parentItem));
            }

            var items = new List<T>();

            for (int i = 0; i < parentItem.Items.Count; i++)
            {
                var item = parentItem.ItemContainerGenerator.ContainerFromIndex(i) as T;
                if (item != null)
                {
                    items.Add(item);
                    items.AddRange(GetItemsRecursively<T>(item));
                }
            }

            return items;
        }

        /// <summary>
        /// Gets an item with a relative position (e.g. +1, -1) to the specified item.
        /// </summary>
        /// <remarks>This deliberately works against a flattened collection (i.e. no hierarchy).</remarks>
        /// <typeparam name="T">The type of item to retrieve.</typeparam>
        /// <param name="item">The item.</param>
        /// <param name="relativePosition">The relative position offset (e.g. +1, -1).</param>
        /// <returns>The item in the relative position, otherwise null.</returns>
        private T GetRelativeItem<T>(T item, int relativePosition)
            where T : ItemsControl
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var items = GetItemsRecursively<T>(AssociatedObject);
            int index = items.IndexOf(item);
            if (index >= 0)
            {
                var relativeIndex = index + relativePosition;
                if (relativeIndex >= 0 && relativeIndex < items.Count)
                {
                    return items[relativeIndex];
                }
            }

            return null;
        }

        #endregion Methods
    }
}