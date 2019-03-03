using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace SteveCadwallader.CodeMaid.UI
{
    /// <summary>
    /// A behavior for making TreeView's SelectedItem bindable based on http://stackoverflow.com/questions/1000040/selecteditem-in-a-wpf-treeview.
    /// </summary>
    public class TreeViewBindableSelectedItemBehavior : Behavior<TreeView>
    {
        #region SelectedItem (Dependency Property)

        /// <summary>
        /// The dependency property definition for the SelectedItem property.
        /// </summary>
        public static DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem", typeof(object), typeof(TreeViewBindableSelectedItemBehavior),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// Called when the SelectedItem dependency property has changed.
        /// </summary>
        /// <param name="obj">The dependency object where the value has changed.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing
        /// the event data.
        /// </param>
        private static void OnSelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is TreeViewBindableSelectedItemBehavior behavior)
            {
                var treeView = behavior.AssociatedObject;
                if (treeView != null)
                {
                    var treeViewItem = FindTreeViewItemRecursively(treeView, e.NewValue);
                    if (treeViewItem != null)
                    {
                        treeViewItem.SetValue(TreeViewItem.IsSelectedProperty, true);
                    }
                }
            }
        }

        /// <summary>
        /// Attempts to find a TreeViewItem recursively that is the container for the specified
        /// content to find.
        /// </summary>
        /// <remarks>
        ///
        /// Source: http://blogs.msdn.com/b/wpfsdk/archive/2010/02/23/finding-an-object-treeviewitem.aspx.
        /// </remarks>
        /// <param name="itemsControl">The items control.</param>
        /// <param name="contentToFind">The content to find.</param>
        /// <returns>The matching TreeViewItem, otherwise null.</returns>
        private static TreeViewItem FindTreeViewItemRecursively(ItemsControl itemsControl, object contentToFind)
        {
            if (itemsControl == null)
            {
                return null;
            }

            if (itemsControl.DataContext == contentToFind)
            {
                return itemsControl as TreeViewItem;
            }

            ForceItemsControlToGenerateContainers(itemsControl);

            for (int i = 0; i < itemsControl.Items.Count; i++)
            {
                var childItemsControl = itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as ItemsControl;
                var result = FindTreeViewItemRecursively(childItemsControl, contentToFind);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Forces the specified items control to generate containers for its items.
        /// </summary>
        /// <param name="itemsControl">The items control.</param>
        private static void ForceItemsControlToGenerateContainers(ItemsControl itemsControl)
        {
            itemsControl.ApplyTemplate();

            var itemsPresenter = (ItemsPresenter)itemsControl.Template.FindName("ItemsHost", itemsControl);

            if (itemsPresenter != null)
            {
                itemsPresenter.ApplyTemplate();
            }
            else
            {
                // The Tree template has not named the ItemsPresenter, so walk the descendents and
                // find the child.
                itemsPresenter = itemsControl.FindVisualChild<ItemsPresenter>();

                if (itemsPresenter == null)
                {
                    itemsControl.UpdateLayout();

                    itemsPresenter = itemsControl.FindVisualChild<ItemsPresenter>();
                }
            }

            if (itemsPresenter != null)
            {
                var itemsHostPanel = (Panel)VisualTreeHelper.GetChild(itemsPresenter, 0);

                // Ensure that the generator for this panel has been created.
                var children = itemsHostPanel.Children;
            }
        }

        #endregion SelectedItem (Dependency Property)

        #region Methods

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectedItemChanged += OnSelectedItemChanged;
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has
        /// actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.SelectedItemChanged -= OnSelectedItemChanged;
            }
        }

        /// <summary>
        /// Called when the SelectedItem has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedPropertyChangedEventArgs&lt;Object&gt;" /> instance
        /// containing the event data.
        /// </param>
        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue;
        }

        #endregion Methods
    }
}