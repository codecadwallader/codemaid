#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace SteveCadwallader.CodeMaid.UI
{
    /// <summary>
    /// A behavior for supporting list box drag and drop reordering.
    /// </summary>
    public class ListBoxReorderBehavior : Behavior<ListBox>
    {
        #region Fields

        private ListBoxItem _dragCandidate;
        private Point? _dragStartPoint;

        #endregion Fields

        #region Behavior Methods

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.AllowDrop = true;
            AssociatedObject.PreviewMouseDown += OnPreviewMouseDown;
            AssociatedObject.PreviewMouseMove += OnPreviewMouseMove;
            AssociatedObject.PreviewMouseUp += OnPreviewMouseUp;
            AssociatedObject.DragEnter += OnDragEvent;
            AssociatedObject.DragOver += OnDragEvent;
            AssociatedObject.DragLeave += OnDragLeave;
            AssociatedObject.Drop += OnDrop;
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.AllowDrop = false;
                AssociatedObject.PreviewMouseDown -= OnPreviewMouseDown;
                AssociatedObject.PreviewMouseMove -= OnPreviewMouseMove;
                AssociatedObject.PreviewMouseUp -= OnPreviewMouseUp;
                AssociatedObject.DragEnter -= OnDragEvent;
                AssociatedObject.DragOver -= OnDragEvent;
                AssociatedObject.DragLeave -= OnDragLeave;
                AssociatedObject.Drop -= OnDrop;
            }
        }

        #endregion Behavior Methods

        #region Event Handlers

        /// <summary>
        /// Called when a PreviewMouseDown event is received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs" /> instance containing the event data.</param>
        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _dragCandidate = FindParentListBoxItem(e.OriginalSource);
                _dragStartPoint = e.GetPosition(null);
            }
        }

        /// <summary>
        /// Called when a PreviewMouseMove event is received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_dragCandidate == null || !_dragStartPoint.HasValue) return;

            var delta = _dragStartPoint.Value - e.GetPosition(null);
            if (Math.Abs(delta.X) <= SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(delta.Y) <= SystemParameters.MinimumVerticalDragDistance) return;

            _dragCandidate.SetValue(DragDropAttachedProperties.IsBeingDraggedProperty, true);

            DragDrop.DoDragDrop(_dragCandidate, new DataObject(typeof(object), _dragCandidate.DataContext), DragDropEffects.Move);

            _dragCandidate.SetValue(DragDropAttachedProperties.IsBeingDraggedProperty, false);

            _dragCandidate = null;
            _dragStartPoint = null;
        }

        /// <summary>
        /// Called when a PreviewMouseUp event is received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs" /> instance containing the event data.</param>
        private void OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            _dragCandidate = null;
            _dragStartPoint = null;
        }

        /// <summary>
        /// Called when a drag event (DragEnter, DragOver) is received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DragEventArgs" /> instance containing the event data.</param>
        private void OnDragEvent(object sender, DragEventArgs e)
        {
            var target = FindParentListBoxItem(e.OriginalSource);
            if (target != null && e.Data.GetDataPresent(typeof(object)))
            {
                var sourceData = e.Data.GetData(typeof(object));
                var targetData = target.DataContext;

                if (sourceData != null && targetData != null && sourceData != targetData)
                {
                    if (IsDropAbove(e, target))
                    {
                        target.SetValue(DragDropAttachedProperties.IsDropAboveTargetProperty, true);
                        target.SetValue(DragDropAttachedProperties.IsDropBelowTargetProperty, false);
                    }
                    else
                    {
                        target.SetValue(DragDropAttachedProperties.IsDropAboveTargetProperty, false);
                        target.SetValue(DragDropAttachedProperties.IsDropBelowTargetProperty, true);
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
        /// Called when a DragLeave event is received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DragEventArgs" /> instance containing the event data.</param>
        private void OnDragLeave(object sender, DragEventArgs e)
        {
            var target = FindParentListBoxItem(e.OriginalSource);
            if (target != null)
            {
                target.SetValue(DragDropAttachedProperties.IsDropAboveTargetProperty, false);
                target.SetValue(DragDropAttachedProperties.IsDropBelowTargetProperty, false);
            }
        }

        /// <summary>
        /// Called when a Drop event is received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DragEventArgs" /> instance containing the event data.</param>
        private void OnDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(object))) return;

            var target = FindParentListBoxItem(e.OriginalSource);
            if (target == null) return;

            var sourceData = e.Data.GetData(typeof(object));
            var targetData = target.DataContext;

            if (sourceData == null || targetData == null || sourceData == targetData) return;

            var collection = AssociatedObject.ItemsSource as ObservableCollection<object>;
            if (collection == null) return;

            var sourceIndex = collection.IndexOf(sourceData);
            var targetIndex = collection.IndexOf(targetData);

            if (!IsDropAbove(e, target))
            {
                targetIndex++;
            }

            if (sourceIndex < targetIndex)
            {
                targetIndex--;
            }

            collection.Move(sourceIndex, targetIndex);

            target.SetValue(DragDropAttachedProperties.IsDropAboveTargetProperty, false);
            target.SetValue(DragDropAttachedProperties.IsDropBelowTargetProperty, false);
            e.Handled = true;
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Attempts to find the parent ListBoxItem from the specified event source.
        /// </summary>
        /// <param name="eventSource">The event source.</param>
        /// <returns>The parent ListBoxItem, otherwise null.</returns>
        private static ListBoxItem FindParentListBoxItem(object eventSource)
        {
            var source = eventSource as DependencyObject;
            if (source == null) return null;

            var listBoxItem = source.FindVisualAncestor<ListBoxItem>();

            return listBoxItem;
        }

        /// <summary>
        /// Determines whether a drop event is occurring above or below a specified target.
        /// </summary>
        /// <param name="e">The <see cref="DragEventArgs" /> instance containing the event data.</param>
        /// <param name="target">The target.</param>
        /// <returns>True if the drop should occur above the target, otherwise false.</returns>
        private static bool IsDropAbove(DragEventArgs e, ListBoxItem target)
        {
            return e.GetPosition(target).Y <= target.ActualHeight / 2;
        }

        #endregion Methods
    }
}