#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using EnvDTE;
using EnvDTE80;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A helper class for identifying explicit interface implementations.
    /// </summary>
    public static class ExplicitInterfaceImplementationHelper
    {
        /// <summary>
        /// Determines if the specified code event is an explicit interface implementation.
        /// </summary>
        /// <remarks>
        /// CodeEvent does not report the interface name as usual, but it can be identified by
        /// looking at the FullName and comparing it to the expected concatenation of parent name +
        /// event name.
        /// </remarks>
        /// <param name="codeEvent">The code event.</param>
        /// <returns>True if an explicit interface implementation, otherwise false.</returns>
        public static bool IsExplicitInterfaceImplementation(CodeEvent codeEvent)
        {
            return codeEvent != null &&
                   codeEvent.Parent is CodeElement &&
                   codeEvent.FullName != (((CodeElement)codeEvent.Parent).FullName + "." + codeEvent.Name);
        }

        /// <summary>
        /// Determines if the specified code function is an explicit interface implementation.
        /// </summary>
        /// <param name="codeFunction">The code function.</param>
        /// <returns>True if an explicit interface implementation, otherwise false.</returns>
        public static bool IsExplicitInterfaceImplementation(CodeFunction2 codeFunction)
        {
            return codeFunction != null &&
                   codeFunction.Name.Contains(".");
        }

        /// <summary>
        /// Determines if the specified code property is an explicit interface implementation.
        /// </summary>
        /// <param name="codeProperty">The code property.</param>
        /// <returns>True if an explicit interface implementation, otherwise false.</returns>
        public static bool IsExplicitInterfaceImplementation(CodeProperty codeProperty)
        {
            return codeProperty != null &&
                   codeProperty.Name.Contains(".");
        }
    }
}