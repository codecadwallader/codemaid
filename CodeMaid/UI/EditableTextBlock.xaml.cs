using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.UI
{
    /// <summary>
    /// A user control for transitioning from a <see cref="TextBlock"/> to a <see cref="TextBox"/>.
    /// </summary>
    public partial class EditableTextBlock
    {
        #region Fields

        private string _originalValue;
        private bool _optionsVisible = false;
        private List<OptionRow> _optionRows = new List<OptionRow>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EditableTextBlock"/> class.
        /// </summary>
        public EditableTextBlock()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region IsEditing (Dependency Property)

        /// <summary>
        /// The dependency property definition for the IsEditing property.
        /// </summary>
        public static readonly DependencyProperty IsEditingProperty = DependencyProperty.Register(
            "IsEditing", typeof(bool), typeof(EditableTextBlock),
            new UIPropertyMetadata(false, OnIsEditingChanged));

        /// <summary>
        /// Gets or sets the flag indicating if the control is in edit mode.
        /// </summary>
        public bool IsEditing
        {
            get { return (bool)GetValue(IsEditingProperty); }
            set { SetValue(IsEditingProperty, value); }
        }

        /// <summary>
        /// Called when the IsEditing dependency property has changed.
        /// </summary>
        /// <param name="obj">The dependency object where the value has changed.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsEditingChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is EditableTextBlock editableTextBlock))
            {
                return;
            }

            if ((bool)e.NewValue)
            {
                // Store a copy of the original text.
                editableTextBlock._originalValue = editableTextBlock.Text;

                // Focus and select all of the text.
                editableTextBlock.Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
                {
                    editableTextBlock.TextBox.Focus();
                    editableTextBlock.TextBox.SelectAll();
                }));
            }
        }

        #endregion IsEditing (Dependency Property)

        #region Text (Dependency Property)

        /// <summary>
        /// The dependency property definition for the Text property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(EditableTextBlock),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        #endregion Text (Dependency Property)

        #region Methods

        /// <summary>
        /// Handles the <see cref="OnButtonClick"/> event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (false == _optionsVisible)
            {
                _optionsVisible = true;

                ButtonTextBlock.Text = "\u25BD";

                var target = FindParentListBoxItem(e.OriginalSource);
                if (target == null) return;

                var targetData = target.DataContext;
                if (targetData == null) return;

                var element = targetData as MemberTypeSetting;
                List<object> collection = targetData as List<object>;
                if (element == null && collection == null) return;

                if (element != null)
                {
                    addOptionBlock(1, 0, element);
                }

                if (collection != null)
                {
                    int index = 0;
                    foreach (object collectionElement in collection)
                    {
                        addOptionBlock(collection.Count, index, (MemberTypeSetting)(collectionElement));
                        index++;
                    }
                }

                return;
            }

            if (true == _optionsVisible)
            {
                _optionsVisible = false;

                ButtonTextBlock.Text = "\u25B3";

                foreach (OptionRow optionRow in _optionRows)
                {
                    EditableGrid.Children.Remove(optionRow.StackPanel);
                    EditableGrid.RowDefinitions.Remove(optionRow.RowDefinition);
                }

                _optionRows.Clear();

                return;
            }
        }

        private void addOptionBlock(int count, int index, MemberTypeSetting memberTypeSetting)
        {
            OptionRow optionRow = new OptionRow()
            {
                OptionBlock = new OptionBlock(),
                StackPanel = new StackPanel(),
                RowDefinition = new RowDefinition(),
                MemberTypeSetting = memberTypeSetting
            };

            optionRow.StackPanel.Children.Add(optionRow.OptionBlock);

            if (count > 1)
            {
                optionRow.OptionBlock.Label.Content =
                    "(" + optionRow.MemberTypeSetting.EffectiveName.Split('+')[index].Trim() + ")";
            }

            optionRow.OptionBlock.SetBinding(OptionBlock.CheckBoxStaticIsCheckedProperty, "OptionStatic");

            EditableGrid.RowDefinitions.Add(optionRow.RowDefinition);
            EditableGrid.Children.Add(optionRow.StackPanel);

            Grid.SetRow(optionRow.StackPanel, EditableGrid.RowDefinitions.Count - 1);
            Grid.SetColumn(optionRow.StackPanel, 0);
            Grid.SetColumnSpan(optionRow.StackPanel, 3);

            _optionRows.Add(optionRow);
        }

        /// <summary>
        /// Handles the <see cref="OnMouseDoubleClick"/> event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IsEditing = !IsEditing;
        }

        /// <summary>
        /// Handles the <see cref="OnKeyDown"/> event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    IsEditing = false;
                    e.Handled = true;
                    break;

                case Key.Escape:
                    Text = _originalValue;
                    IsEditing = false;
                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// Handles the <see cref="OnLostFocus"/> event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            IsEditing = false;
        }

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

        private class OptionRow
        {
            public RowDefinition RowDefinition { get; set; }

            public OptionBlock OptionBlock { get; set; }

            public StackPanel StackPanel { get; set; }

            public MemberTypeSetting MemberTypeSetting { get; set; }
        }

        #endregion Methods
    }
}