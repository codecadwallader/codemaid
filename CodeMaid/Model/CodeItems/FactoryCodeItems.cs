using EnvDTE;
using EnvDTE80;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// A static factory class for generating code items.
    /// </summary>
    public static class FactoryCodeItems
    {
        /// <summary>
        /// Attempts to generate a code item element from the specified code element.
        /// </summary>
        /// <param name="codeElement">The code element.</param>
        /// <returns>A generated code item element, otherwise null.</returns>
        public static BaseCodeItemElement CreateCodeItemElement(CodeElement codeElement)
        {
            if (codeElement == null) return null;

            BaseCodeItemElement codeItem;

            switch (codeElement.Kind)
            {
                case vsCMElement.vsCMElementClass:
                    codeItem = new CodeItemClass { CodeClass = codeElement as CodeClass2 };
                    break;

                case vsCMElement.vsCMElementDelegate:
                    codeItem = new CodeItemDelegate { CodeDelegate = codeElement as CodeDelegate2 };
                    break;

                case vsCMElement.vsCMElementEnum:
                    codeItem = new CodeItemEnum { CodeEnum = codeElement as CodeEnum };
                    break;

                case vsCMElement.vsCMElementEvent:
                    codeItem = new CodeItemEvent { CodeEvent = codeElement as CodeEvent };
                    break;

                case vsCMElement.vsCMElementFunction:
                    codeItem = new CodeItemMethod { CodeFunction = codeElement as CodeFunction2 };
                    break;

                case vsCMElement.vsCMElementImportStmt:
                    codeItem = new CodeItemUsingStatement();
                    break;

                case vsCMElement.vsCMElementInterface:
                    codeItem = new CodeItemInterface { CodeInterface = codeElement as CodeInterface2 };
                    break;

                case vsCMElement.vsCMElementNamespace:
                    codeItem = new CodeItemNamespace { CodeNamespace = codeElement as CodeNamespace };
                    break;

                case vsCMElement.vsCMElementProperty:
                    codeItem = new CodeItemProperty { CodeProperty = codeElement as CodeProperty2 };
                    break;

                case vsCMElement.vsCMElementStruct:
                    codeItem = new CodeItemStruct { CodeStruct = codeElement as CodeStruct2 };
                    break;

                case vsCMElement.vsCMElementVariable:
                    codeItem = new CodeItemField { CodeVariable = codeElement as CodeVariable2 };
                    break;

                default:
                    return null;
            }

            // Populate the common fields.
            codeItem.CodeElement = codeElement;
            codeItem.RefreshCachedPositionAndName();

            return codeItem;
        }
    }
}