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

using System.Collections.Generic;
using EnvDTE;
using SteveCadwallader.CodeMaid.CodeItems;

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
                // Fall back to line placement comparison for matching elements.
                return x.StartLine.CompareTo(y.StartLine);
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
            int staticOffset = CalculateStaticOffset(codeItem);

            return (typeOffset * 10) + (accessOffset * 2) + staticOffset;
        }

        private static int CalculateTypeOffset(BaseCodeItem codeItem)
        {
            const int constantOffset = 1;
            const int fieldOffset = 2;
            const int constructorOffset = 3;
            const int destructorOffset = 4;
            const int delegateOffset = 5;
            const int eventOffset = 6;
            const int enumOffset = 7;
            const int interfaceOffset = 8;
            const int propertyOffset = 9;
            const int methodOffset = 10;
            const int structOffset = 11;
            const int classOffset = 12;

            switch (codeItem.Kind)
            {
                case KindCodeItem.Class: return classOffset;
                case KindCodeItem.Constant: return constantOffset;
                case KindCodeItem.Constructor: return constructorOffset;
                case KindCodeItem.Delegate: return delegateOffset;
                case KindCodeItem.Destructor: return destructorOffset;
                case KindCodeItem.Enum: return enumOffset;
                case KindCodeItem.Event: return eventOffset;
                case KindCodeItem.Field: return fieldOffset;
                case KindCodeItem.Interface: return interfaceOffset;
                case KindCodeItem.Method: return methodOffset;
                case KindCodeItem.Property: return propertyOffset;
                case KindCodeItem.Struct: return structOffset;
                default: return 0;
            }
        }

        private static int CalculateAccessOffset(BaseCodeItem codeItem)
        {
            var codeItemElement = codeItem as BaseCodeItemElement;
            if (codeItemElement == null) return 0;

            switch (codeItemElement.Access)
            {
                case vsCMAccess.vsCMAccessAssemblyOrFamily: return 1;
                case vsCMAccess.vsCMAccessProject: return 2;
                case vsCMAccess.vsCMAccessProtected: return 3;
                case vsCMAccess.vsCMAccessPrivate: return 4;
                default: return 0;
            }
        }

        private static int CalculateStaticOffset(BaseCodeItem codeItem)
        {
            var codeItemElement = codeItem as BaseCodeItemElement;
            if (codeItemElement == null) return 0;

            return codeItemElement.IsStatic ? 0 : 1;
        }
    }
}