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

namespace SteveCadwallader.CodeMaid.CodeItems
{
    /// <summary>
    /// A static factory class for generating code items.
    /// </summary>
    public static class FactoryCodeItems
    {
        /// <summary>
        /// Attempts to generate a code item from the specified code element.
        /// </summary>
        /// <param name="codeElement">The code element.</param>
        /// <returns>A generated code item, otherwise null.</returns>
        public static CodeItemBase CreateCodeItem(CodeElement codeElement)
        {
            if (codeElement == null) return null;

            CodeItemBase codeItem;
            bool isNameless = false;

            switch (codeElement.Kind)
            {
                case vsCMElement.vsCMElementClass:
                    codeItem = new CodeItemClass { CodeClass = codeElement as CodeClass };
                    break;

                case vsCMElement.vsCMElementDelegate:
                    codeItem = new CodeItemDelegate { CodeDelegate = codeElement as CodeDelegate };
                    break;

                case vsCMElement.vsCMElementEnum:
                    codeItem = new CodeItemEnum { CodeEnum = codeElement as CodeEnum };
                    break;

                case vsCMElement.vsCMElementEvent:
                    codeItem = new CodeItemEvent();
                    break;

                case vsCMElement.vsCMElementFunction:
                    codeItem = new CodeItemMethod { CodeFunction = codeElement as CodeFunction };
                    break;

                case vsCMElement.vsCMElementImportStmt:
                    codeItem = new CodeItemUsingStatement();
                    isNameless = true;
                    break;

                case vsCMElement.vsCMElementInterface:
                    codeItem = new CodeItemInterface { CodeInterface = codeElement as CodeInterface };
                    break;

                case vsCMElement.vsCMElementNamespace:
                    codeItem = new CodeItemNamespace { CodeNamespace = codeElement as CodeNamespace };
                    break;

                case vsCMElement.vsCMElementProperty:
                    codeItem = new CodeItemProperty { CodeProperty = codeElement as CodeProperty };
                    break;

                case vsCMElement.vsCMElementStruct:
                    codeItem = new CodeItemStruct { CodeStruct = codeElement as CodeStruct };
                    break;

                case vsCMElement.vsCMElementVariable:
                    codeItem = new CodeItemField { CodeVariable = codeElement as CodeVariable };
                    break;

                default:
                    return null;
            }

            // Populate the common fields.
            codeItem.StartLine = codeElement.StartPoint.Line;
            codeItem.EndLine = codeElement.EndPoint.Line;
            codeItem.CodeElement = codeElement;

            if (!isNameless)
            {
                codeItem.Name = codeElement.Name;
            }

            return codeItem;
        }
    }
}