#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using EnvDTE;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using System.Collections.Generic;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A helper for comparing code items by type following the C# standard.
    /// </summary>
    public class CodeItemTypeComparer : Comparer<BaseCodeItem>
    {
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
                if (Settings.Default.Digging_SecondarySortTypeByName)
                {
                    int nameComparison = x.Name.CompareTo(y.Name);
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

        /// <summary>
        /// Calculates an ordered numeric representation of the specified code item.
        /// </summary>
        /// <param name="codeItem">The code item.</param>
        /// <returns>A numeric representation.</returns>
        public static int CalculateNumericRepresentation(BaseCodeItem codeItem)
        {
            int typeOffset = CalculateTypeOffset(codeItem);
            int accessOffset = CalculateAccessOffset(codeItem);
            int constantOffset = CalculateConstantOffset(codeItem);
            int staticOffset = CalculateStaticOffset(codeItem);
            int readOnlyOffset = CalculateReadOnlyOffset(codeItem);

            return (typeOffset * 10000) + (accessOffset * 1000) + (constantOffset * 100) + (staticOffset * 10) + readOnlyOffset;
        }

        private static int CalculateTypeOffset(BaseCodeItem codeItem)
        {
            switch (codeItem.Kind)
            {
                case KindCodeItem.Class: return MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeClasses).Order;
                case KindCodeItem.Constructor: return MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeConstructors).Order;
                case KindCodeItem.Delegate: return MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeDelegates).Order;
                case KindCodeItem.Destructor: return MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeDestructors).Order;
                case KindCodeItem.Enum: return MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeEnums).Order;
                case KindCodeItem.Event: return MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeEvents).Order;
                case KindCodeItem.Field: return MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeFields).Order;
                case KindCodeItem.Indexer: return MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeIndexers).Order;
                case KindCodeItem.Interface: return MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeInterfaces).Order;
                case KindCodeItem.Method: return MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeMethods).Order;
                case KindCodeItem.Property: return MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeProperties).Order;
                case KindCodeItem.Struct: return MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeStructs).Order;
                default: return 0;
            }
        }

        private static int CalculateAccessOffset(BaseCodeItem codeItem)
        {
            var codeItemElement = codeItem as BaseCodeItemElement;
            if (codeItemElement == null) return 0;

            switch (codeItemElement.Access)
            {
                case vsCMAccess.vsCMAccessPublic: return 1;
                case vsCMAccess.vsCMAccessAssemblyOrFamily: return 2;
                case vsCMAccess.vsCMAccessProject: return 3;
                case vsCMAccess.vsCMAccessProjectOrProtected: return 4;
                case vsCMAccess.vsCMAccessProtected: return 5;
                case vsCMAccess.vsCMAccessPrivate: return 6;
                default: return 0;
            }
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
    }
}