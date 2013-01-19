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

using System.Collections.Generic;
using EnvDTE;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A helper for comparing code items by type following the C# standard.
    /// </summary>
    public class CodeItemTypeComparer : Comparer<BaseCodeItem>
    {
        /// <summary>
        /// Performs a comparison of two objects of the same type and returns a value indicating whether one object is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// Less than zero: <paramref name="x"/> is less than <paramref name="y"/>.
        /// Zero: <paramref name="x"/> equals <paramref name="y"/>.
        /// Greater than zero: <paramref name="x"/> is greater than <paramref name="y"/>.
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
                case KindCodeItem.Class: return Settings.Default.Reorganizing_SortOrderTypeClasses;
                case KindCodeItem.Constructor: return Settings.Default.Reorganizing_SortOrderTypeConstructors;
                case KindCodeItem.Delegate: return Settings.Default.Reorganizing_SortOrderTypeDelegates;
                case KindCodeItem.Destructor: return Settings.Default.Reorganizing_SortOrderTypeDestructors;
                case KindCodeItem.Enum: return Settings.Default.Reorganizing_SortOrderTypeEnums;
                case KindCodeItem.Event: return Settings.Default.Reorganizing_SortOrderTypeEvents;
                case KindCodeItem.Field: return Settings.Default.Reorganizing_SortOrderTypeFields;
                case KindCodeItem.Indexer: return Settings.Default.Reorganizing_SortOrderTypeIndexers;
                case KindCodeItem.Interface: return Settings.Default.Reorganizing_SortOrderTypeInterfaces;
                case KindCodeItem.Method: return Settings.Default.Reorganizing_SortOrderTypeMethods;
                case KindCodeItem.Property: return Settings.Default.Reorganizing_SortOrderTypeProperties;
                case KindCodeItem.Struct: return Settings.Default.Reorganizing_SortOrderTypeStructs;
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