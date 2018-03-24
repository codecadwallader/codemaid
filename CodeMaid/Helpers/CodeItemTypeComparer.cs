using EnvDTE;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using System.Collections.Generic;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A helper for comparing code items by type, access level, etc.
    /// </summary>
    public class CodeItemTypeComparer : Comparer<BaseCodeItem>
    {
        #region Fields

        private readonly bool _sortByName;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeItemTypeComparer"/> class.
        /// </summary>
        /// <param name="sortByName">Determines whether a secondary sort by name is performed or not.</param>
        public CodeItemTypeComparer(bool sortByName)
        {
            _sortByName = sortByName;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Performs a comparison of two objects of the same type and returns a value indicating
        /// whether one object is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// Less than zero: <paramref name="x" /> is less than <paramref name="y" />.
        /// Zero: <paramref name="x" /> equals <paramref name="y" />.
        /// Greater than zero: <paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        public override int Compare(BaseCodeItem x, BaseCodeItem y)
        {
            int first = CalculateNumericRepresentation(x);
            int second = CalculateNumericRepresentation(y);

            if (first == second)
            {
                // Check if secondary sort by name should occur.
                if (_sortByName)
                {
                    int nameComparison = NormalizeName(x).CompareTo(NormalizeName(y));
                    if (nameComparison != 0)
                    {
                        return nameComparison;
                    }
                }

                // Fall back to position comparison for matching elements.
                return x.StartOffset.CompareTo(y.StartOffset);
            }

            return first.CompareTo(second);
        }

        private static int CalculateNumericRepresentation(BaseCodeItem codeItem)
        {
            int typeOffset = CalculateTypeOffset(codeItem);
            int accessOffset = CalculateAccessOffset(codeItem);
            int explicitOffset = CalculateExplicitInterfaceOffset(codeItem);
            int constantOffset = CalculateConstantOffset(codeItem);
            int staticOffset = CalculateStaticOffset(codeItem);
            int readOnlyOffset = CalculateReadOnlyOffset(codeItem);

            int calc = 0;

            if (!Settings.Default.Reorganizing_PrimaryOrderByAccessLevel)
            {
                calc += typeOffset * 100000;
                calc += accessOffset * 10000;
            }
            else
            {
                calc += accessOffset * 100000;
                calc += typeOffset * 10000;
            }

            calc += (explicitOffset * 1000) + (constantOffset * 100) + (staticOffset * 10) + readOnlyOffset;

            return calc;
        }

        private static int CalculateTypeOffset(BaseCodeItem codeItem)
        {
            switch (codeItem.Kind)
            {
                case KindCodeItem.Class: return MemberTypeSettingHelper.ClassSettings.Order;
                case KindCodeItem.Constructor: return MemberTypeSettingHelper.ConstructorSettings.Order;
                case KindCodeItem.Delegate: return MemberTypeSettingHelper.DelegateSettings.Order;
                case KindCodeItem.Destructor: return MemberTypeSettingHelper.DestructorSettings.Order;
                case KindCodeItem.Enum: return MemberTypeSettingHelper.EnumSettings.Order;
                case KindCodeItem.Event: return MemberTypeSettingHelper.EventSettings.Order;
                case KindCodeItem.Field: return MemberTypeSettingHelper.FieldSettings.Order;
                case KindCodeItem.Indexer: return MemberTypeSettingHelper.IndexerSettings.Order;
                case KindCodeItem.Interface: return MemberTypeSettingHelper.InterfaceSettings.Order;
                case KindCodeItem.Method: return MemberTypeSettingHelper.MethodSettings.Order;
                case KindCodeItem.Property: return MemberTypeSettingHelper.PropertySettings.Order;
                case KindCodeItem.Struct: return MemberTypeSettingHelper.StructSettings.Order;
                default: return 0;
            }
        }

        private static int CalculateAccessOffset(BaseCodeItem codeItem)
        {
            var codeItemElement = codeItem as BaseCodeItemElement;
            if (codeItemElement == null) return 0;

            var itemsOrder = new List<vsCMAccess>
            {
                vsCMAccess.vsCMAccessPublic,
                vsCMAccess.vsCMAccessAssemblyOrFamily,
                vsCMAccess.vsCMAccessProject,
                vsCMAccess.vsCMAccessProjectOrProtected,
                vsCMAccess.vsCMAccessProtected,
                vsCMAccess.vsCMAccessPrivate
            };

            if (Settings.Default.Reorganizing_ReverseOrderByAccessLevel)
            {
                itemsOrder.Reverse();
            }

            return itemsOrder.IndexOf(codeItemElement.Access) + 1;
        }

        private static int CalculateExplicitInterfaceOffset(BaseCodeItem codeItem)
        {
            if (Settings.Default.Reorganizing_ExplicitMembersAtEnd)
            {
                var interfaceItem = codeItem as IInterfaceItem;
                if ((interfaceItem != null) && interfaceItem.IsExplicitInterfaceImplementation)
                {
                    return 1;
                }
            }

            return 0;
        }

        private static int CalculateConstantOffset(BaseCodeItem codeItem)
        {
            var codeItemField = codeItem as CodeItemField;
            if (codeItemField == null) return 0;

            return codeItemField.IsConstant ? 0 : 1;
        }

        private static int CalculateStaticOffset(BaseCodeItem codeItem)
        {
            var codeItemElement = codeItem as BaseCodeItemElement;
            if (codeItemElement == null) return 0;

            return codeItemElement.IsStatic ? 0 : 1;
        }

        private static int CalculateReadOnlyOffset(BaseCodeItem codeItem)
        {
            var codeItemField = codeItem as CodeItemField;
            if (codeItemField == null) return 0;

            return codeItemField.IsReadOnly ? 0 : 1;
        }

        private static string NormalizeName(BaseCodeItem codeItem)
        {
            string name = codeItem.Name;
            var interfaceItem = codeItem as IInterfaceItem;
            if ((interfaceItem != null) && interfaceItem.IsExplicitInterfaceImplementation)
            {
                // Try to find where the interface ends and the method starts
                int dot = name.LastIndexOf('.') + 1;
                if (0 < dot && dot < name.Length)
                {
                    return name.Substring(dot);
                }
            }

            return name;
        }

        #endregion Methods
    }
}