using EnvDTE;
using EnvDTE80;
using System;
using System.Text.RegularExpressions;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A static helper class for common CodeElement requests.
    /// </summary>
    internal static class CodeElementHelper
    {
        #region Internal Methods

        /// <summary>
        /// Calculates the complexity of the given element.
        /// </summary>
        /// <param name="element">The code element to examine.</param>
        /// <returns>The calculated complexity.</returns>
        internal static int CalculateComplexity(CodeElement element)
        {
            EditPoint startPoint = element.StartPoint.CreateEditPoint();
            string functionText = startPoint.GetText(element.EndPoint);

            // Rip out single line comments.
            functionText = Regex.Replace(functionText, @"//.*" + Environment.NewLine, Environment.NewLine);

            // Rip out multi-line comments.
            functionText = Regex.Replace(functionText, @"/\*.*?\*/", String.Empty, RegexOptions.Singleline);

            // Rip out strings.
            functionText = Regex.Replace(functionText, @"""[^""]*""", String.Empty);

            // Rip out characters.
            functionText = Regex.Replace(functionText, @"'[^']*'", String.Empty);

            int ifCount = Regex.Matches(functionText, @"\sif[\s\(]").Count;
            int elseCount = Regex.Matches(functionText, @"\selse\s").Count;
            int elseIfCount = Regex.Matches(functionText, @"\selse if[\s\(]").Count;
            int whileCount = Regex.Matches(functionText, @"\swhile[\s\(]").Count;
            int forCount = Regex.Matches(functionText, @"\sfor[\s\(]").Count;
            int forEachCount = Regex.Matches(functionText, @"\sforeach[\s\(]").Count;
            int switchCount = Regex.Matches(functionText, @"\sswitch[\s\(]").Count;
            int caseCount = Regex.Matches(functionText, @"\scase\s[^;]*;").Count;
            int catchCount = Regex.Matches(functionText, @"\scatch[\s\(]").Count;
            int tertiaryCount = Regex.Matches(functionText, @"\s\?\s").Count;
            int andCount = Regex.Matches(functionText, @"\&\&").Count;
            int orCount = Regex.Matches(functionText, @"\|\|").Count;

            int complexity = 1 +
                             ifCount + elseCount - elseIfCount + // else if will have been counted twice already by 'if' and 'else'
                             whileCount + forCount + forEachCount + switchCount + caseCount +
                             catchCount + tertiaryCount + andCount + orCount;

            return complexity;
        }

        /// <summary>
        /// Gets the keyword for the specified access modifier.
        /// </summary>
        /// <param name="accessModifier">The access modifier.</param>
        /// <returns>The matching keyword, otherwise null.</returns>
        internal static string GetAccessModifierKeyword(vsCMAccess accessModifier)
        {
            switch (accessModifier)
            {
                case vsCMAccess.vsCMAccessPublic: return "public";
                case vsCMAccess.vsCMAccessProtected: return "protected";
                case vsCMAccess.vsCMAccessProject: return "internal";
                case vsCMAccess.vsCMAccessProjectOrProtected: return "protected internal";
                case vsCMAccess.vsCMAccessPrivate: return "private";
                default: return null;
            }
        }

        /// <summary>
        /// Gets the declaration of the specified code class as a string.
        /// </summary>
        /// <param name="codeClass">The code class.</param>
        /// <returns>The string declaration.</returns>
        internal static string GetClassDeclaration(CodeClass codeClass)
        {
            // Get the start point after the attributes.
            var startPoint = codeClass.GetStartPoint(vsCMPart.vsCMPartHeader);

            return TextDocumentHelper.GetTextToFirstMatch(startPoint, @"\{");
        }

        /// <summary>
        /// Gets the declaration of the specified code delegate as a string.
        /// </summary>
        /// <param name="codeDelegate">The code delegate.</param>
        /// <returns>The string declaration.</returns>
        internal static string GetDelegateDeclaration(CodeDelegate codeDelegate)
        {
            // Get the start point at the end of the attributes if there are any (vsCMPartHeader is
            // not available for delegates).
            var startPoint = codeDelegate.Attributes.Count > 0
                ? codeDelegate.GetEndPoint(vsCMPart.vsCMPartAttributesWithDelimiter)
                : codeDelegate.StartPoint;

            return TextDocumentHelper.GetTextToFirstMatch(startPoint, @";");
        }

        /// <summary>
        /// Gets the declaration of the specified code enum as a string.
        /// </summary>
        /// <param name="codeEnum">The code enum.</param>
        /// <returns>The string declaration.</returns>
        internal static string GetEnumerationDeclaration(CodeEnum codeEnum)
        {
            // Get the start point after the attributes.
            var startPoint = codeEnum.GetStartPoint(vsCMPart.vsCMPartHeader);

            return TextDocumentHelper.GetTextToFirstMatch(startPoint, @"\{");
        }

        /// <summary>
        /// Gets the declaration of the specified code event as a string.
        /// </summary>
        /// <param name="codeEvent">The code event.</param>
        /// <returns>The string declaration.</returns>
        internal static string GetEventDeclaration(CodeEvent codeEvent)
        {
            // Get the start point at the end of the attributes if there are any (vsCMPartHeader is
            // not available for events).
            var startPoint = codeEvent.Attributes.Count > 0
                ? codeEvent.GetEndPoint(vsCMPart.vsCMPartAttributesWithDelimiter)
                : codeEvent.StartPoint;

            return TextDocumentHelper.GetTextToFirstMatch(startPoint, @"[\{;]");
        }

        /// <summary>
        /// Gets the declaration of the specified code field as a string.
        /// </summary>
        /// <param name="codeField">The code field.</param>
        /// <returns>The string declaration.</returns>
        internal static string GetFieldDeclaration(CodeVariable codeField)
        {
            // Get the start point at the end of the attributes if there are any (vsCMPartHeader is
            // not available for fields).
            var startPoint = codeField.Attributes.Count > 0
                ? codeField.GetEndPoint(vsCMPart.vsCMPartAttributesWithDelimiter)
                : codeField.StartPoint;

            return TextDocumentHelper.GetTextToFirstMatch(startPoint, @"[,;]");
        }

        /// <summary>
        /// Gets the declaration of the specified code interface as a string.
        /// </summary>
        /// <param name="codeInterface">The code interface.</param>
        /// <returns>The string declaration.</returns>
        internal static string GetInterfaceDeclaration(CodeInterface codeInterface)
        {
            // Get the start point after the attributes.
            var startPoint = codeInterface.GetStartPoint(vsCMPart.vsCMPartHeader);

            return TextDocumentHelper.GetTextToFirstMatch(startPoint, @"\{");
        }

        /// <summary>
        /// Gets the declaration of the specified code method as a string.
        /// </summary>
        /// <param name="codeFunction">The code method.</param>
        /// <returns>The string declaration.</returns>
        internal static string GetMethodDeclaration(CodeFunction codeFunction)
        {
            // Get the start point after the attributes.
            var startPoint = codeFunction.GetStartPoint(vsCMPart.vsCMPartHeader);

            return TextDocumentHelper.GetTextToFirstMatch(startPoint, @"[\(\{;]");
        }

        /// <summary>
        /// Gets the declaration of the specified code property as a string.
        /// </summary>
        /// <param name="codeProperty">The code property.</param>
        /// <returns>The string declaration.</returns>
        internal static string GetPropertyDeclaration(CodeProperty codeProperty)
        {
            // Get the start point at the end of the attributes if there are any (vsCMPartHeader is
            // not available for properties).
            var startPoint = codeProperty.Attributes.Count > 0
                ? codeProperty.GetEndPoint(vsCMPart.vsCMPartAttributesWithDelimiter)
                : codeProperty.StartPoint;

            return TextDocumentHelper.GetTextToFirstMatch(startPoint, @"\{");
        }

        /// <summary>
        /// Gets the declaration of the specified code struct as a string.
        /// </summary>
        /// <param name="codeStruct">The code struct.</param>
        /// <returns>The string declaration.</returns>
        internal static string GetStructDeclaration(CodeStruct codeStruct)
        {
            // Get the start point after the attributes.
            var startPoint = codeStruct.GetStartPoint(vsCMPart.vsCMPartHeader);

            return TextDocumentHelper.GetTextToFirstMatch(startPoint, @"\{");
        }

        #endregion Internal Methods
    }
}