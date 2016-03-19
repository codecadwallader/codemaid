using SteveCadwallader.CodeMaid.Model.CodeItems;
using System.Windows;
using System.Windows.Controls;

namespace SteveCadwallader.CodeMaid.UI.ToolWindows.Spade
{
    /// <summary>
    /// A template selector for code items.
    /// </summary>
    public class CodeItemTemplateSelector : DataTemplateSelector
    {
        #region Properties

        /// <summary>
        /// Gets or sets the delegate data template.
        /// </summary>
        public DataTemplate DelegateDataTemplate { get; set; }

        /// <summary>
        /// Gets or sets the method data template.
        /// </summary>
        public DataTemplate MethodDataTemplate { get; set; }

        /// <summary>
        /// Gets or sets the parent data template.
        /// </summary>
        public DataTemplate ParentDataTemplate { get; set; }

        /// <summary>
        /// Gets or sets the property data template.
        /// </summary>
        public DataTemplate PropertyDataTemplate { get; set; }

        /// <summary>
        /// Gets or sets the region data template.
        /// </summary>
        public DataTemplate RegionDataTemplate { get; set; }

        /// <summary>
        /// Gets or sets the standard data template.
        /// </summary>
        public DataTemplate StandardDataTemplate { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// When overridden in a derived class, returns a <see cref="T:System.Windows.DataTemplate"
        /// /> based on custom logic.
        /// </summary>
        /// <param name="item">The data object for which to select the template.</param>
        /// <param name="container">The data-bound object.</param>
        /// <returns>
        /// Returns a <see cref="T:System.Windows.DataTemplate" /> or null. The default value is null.
        /// </returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var codeItem = item as BaseCodeItem;
            if (codeItem != null)
            {
                switch (codeItem.Kind)
                {
                    case KindCodeItem.Delegate:
                        return DelegateDataTemplate;

                    case KindCodeItem.Constructor:
                    case KindCodeItem.Destructor:
                    case KindCodeItem.Method:
                        return MethodDataTemplate;

                    case KindCodeItem.Class:
                    case KindCodeItem.Enum:
                    case KindCodeItem.Interface:
                    case KindCodeItem.Struct:
                        return ParentDataTemplate;

                    case KindCodeItem.Indexer:
                    case KindCodeItem.Property:
                        return PropertyDataTemplate;

                    case KindCodeItem.Region:
                        return RegionDataTemplate;

                    case KindCodeItem.Event:
                    case KindCodeItem.Field:
                        return StandardDataTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }

        #endregion Methods
    }
}