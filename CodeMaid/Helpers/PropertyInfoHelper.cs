#region CodeMaid is Copyright 2007-2016 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2016 Steve Cadwallader.

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A helper class that provides reflection access to property information.
    /// </summary>
    /// <typeparam name="T">The class where the property exists.</typeparam>
    public static class PropertyInfoHelper<T>
    {
        /// <summary>
        /// Gets the property info referenced by the specified lambda expression.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="lambda">The lambda.</param>
        /// <returns>The property info for the referenced property, otherwise null.</returns>
        public static PropertyInfo GetPropertyInfo<TValue>(Expression<Func<T, TValue>> lambda)
        {
            return lambda.Body.NodeType == ExpressionType.MemberAccess
                       ? ((MemberExpression)lambda.Body).Member as PropertyInfo
                       : null;
        }
    }
}