#region CodeMaid is Copyright 2007-2011 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2011 Steve Cadwallader.

using EnvDTE;
using SteveCadwallader.CodeMaid.CodeItems;

namespace SteveCadwallader.CodeMaid.Factories
{
    /// <summary>
    /// A static factory class for generating code items.
    /// </summary>
    public static class CodeItemFactory
    {
        /// <summary>
        /// Attempts to generate a code item from the specified code element.
        /// </summary>
        /// <param name="codeElement">The code element.</param>
        /// <returns>A generated code item, otherwise null.</returns>
        public static CodeItemBase CreateCodeItem(CodeElement codeElement)
        {
            CodeItemBase codeItem = null;

            if (codeElement is CodeClass)
            {
                codeItem = new CodeItemClass { CodeClass = (CodeClass)codeElement };
            }
            else if (codeElement is CodeDelegate)
            {
                codeItem = new CodeItemDelegate { CodeDelegate = (CodeDelegate)codeElement };
            }
            else if (codeElement is CodeEnum)
            {
                codeItem = new CodeItemEnum { CodeEnum = (CodeEnum)codeElement };
            }
            else if (codeElement.Kind == vsCMElement.vsCMElementEvent)
            {
                codeItem = new CodeItemEvent();
            }
            else if (codeElement is CodeVariable)
            {
                codeItem = new CodeItemField { CodeVariable = (CodeVariable)codeElement };
            }
            else if (codeElement is CodeInterface)
            {
                codeItem = new CodeItemInterface { CodeInterface = (CodeInterface)codeElement };
            }
            else if (codeElement is CodeFunction)
            {
                codeItem = new CodeItemMethod { CodeFunction = (CodeFunction)codeElement };
            }
            else if (codeElement is CodeNamespace)
            {
                codeItem = new CodeItemNamespace { CodeNamespace = (CodeNamespace)codeElement };
            }
            else if (codeElement is CodeProperty)
            {
                codeItem = new CodeItemProperty { CodeProperty = (CodeProperty)codeElement };
            }
            else if (codeElement is CodeStruct)
            {
                codeItem = new CodeItemStruct { CodeStruct = (CodeStruct)codeElement };
            }

            if (codeItem != null)
            {
                codeItem.Name = codeElement.Name;
                codeItem.StartLine = codeElement.StartPoint.Line;
                codeItem.EndLine = codeElement.EndPoint.Line;
            }

            return codeItem;
        }
    }
}