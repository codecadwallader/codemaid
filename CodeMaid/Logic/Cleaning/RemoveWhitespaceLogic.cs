#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System;

namespace SteveCadwallader.CodeMaid.Logic.Cleaning
{
    /// <summary>
    /// A class for encapsulating the logic of removing whitespace.
    /// </summary>
    internal class RemoveWhitespaceLogic
    {
        #region Fields

        private readonly CodeMaidPackage _package;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="RemoveWhitespaceLogic" /> class.
        /// </summary>
        private static RemoveWhitespaceLogic _instance;

        /// <summary>
        /// Gets an instance of the <see cref="RemoveWhitespaceLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="RemoveWhitespaceLogic" /> class.</returns>
        internal static RemoveWhitespaceLogic GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new RemoveWhitespaceLogic(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveWhitespaceLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private RemoveWhitespaceLogic(CodeMaidPackage package)
        {
            _package = package;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Removes blank lines from the bottom of the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveBlankLinesAtBottom(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveBlankLinesAtBottom) return;

            EditPoint cursor = textDocument.EndPoint.CreateEditPoint();
            cursor.DeleteWhitespace(vsWhitespaceOptions.vsWhitespaceOptionsVertical);
        }

        /// <summary>
        /// Removes blank lines from the top of the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveBlankLinesAtTop(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveBlankLinesAtTop) return;

            EditPoint cursor = textDocument.StartPoint.CreateEditPoint();
            cursor.DeleteWhitespace(vsWhitespaceOptions.vsWhitespaceOptionsVertical);
        }

        /// <summary>
        /// Removes blank lines after attributes.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveBlankLinesAfterAttributes(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveBlankLinesAfterAttributes) return;

            string pattern = _package.UsePOSIXRegEx
                                 ? @"\]{:b*(//.*)*}\n\n~(:b*//)"
                                 : @"\]([ \t]*(//[^\r\n]*)*)(\r?\n){2}(?![ \t]*//)";
            string replacement = _package.UsePOSIXRegEx
                                     ? @"\]\1" + Environment.NewLine
                                     : @"]$1" + Environment.NewLine;

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// Removes blank lines after an opening brace.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveBlankLinesAfterOpeningBrace(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveBlankLinesAfterOpeningBrace) return;

            string pattern = _package.UsePOSIXRegEx
                                 ? @"\{{:b*(//.*)*}\n\n"
                                 : @"\{([ \t]*(//[^\r\n]*)*)(\r?\n){2,}";
            string replacement = _package.UsePOSIXRegEx
                                     ? @"\{\1" + Environment.NewLine
                                     : @"{$1" + Environment.NewLine;

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// Removes blank lines before a closing brace.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveBlankLinesBeforeClosingBrace(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveBlankLinesBeforeClosingBrace) return;

            string pattern = _package.UsePOSIXRegEx
                                 ? @"\n\n{:b*}\}"
                                 : @"(\r?\n){2,}([ \t]*)\}";
            string replacement = _package.UsePOSIXRegEx
                                     ? Environment.NewLine + @"\1\}"
                                     : Environment.NewLine + @"$2}";

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// Removes blank lines before a closing tag.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveBlankLinesBeforeClosingTag(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveBlankLinesBeforeClosingTags) return;

            string pattern = _package.UsePOSIXRegEx
                                 ? @"\n\n{:b*}\</"
                                 : @"(\r?\n){2,}([ \t]*)</";
            string replacement = _package.UsePOSIXRegEx
                                     ? Environment.NewLine + @"\1</"
                                     : Environment.NewLine + @"$2</";

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// Removes blank lines between chained statements.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveBlankLinesBetweenChainedStatements(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveBlankLinesBetweenChainedStatements) return;

            string pattern = _package.UsePOSIXRegEx
                                 ? @"\n\n{:b*}{else|catch|finally}{:b|\n}"
                                 : @"(\r?\n){2,}([ \t]*)(else|catch|finally)( |\t|\r?\n)";
            string replacement = _package.UsePOSIXRegEx
                                     ? Environment.NewLine + @"\1\2\3"
                                     : Environment.NewLine + @"$2$3$4";

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// Removes blank spaces before a closing angle bracket.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveBlankSpacesBeforeClosingAngleBracket(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveBlankSpacesBeforeClosingAngleBrackets) return;

            // Remove blank spaces before regular closing angle brackets.
            string pattern = _package.UsePOSIXRegEx ? @"\n*:b+\>\n" : @"(\r?\n)*[ \t]+>\r?\n";
            string replacement = @">" + Environment.NewLine;

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);

            // Handle blank spaces before self closing angle brackets based on insert blank space setting.
            if (Settings.Default.Cleaning_InsertBlankSpaceBeforeSelfClosingAngleBrackets)
            {
                string oneSpacePattern = _package.UsePOSIXRegEx ? @"\n*:b:b+/\>\n" : @"(\r?\n)*[ \t]{2,}/>\r?\n";
                string oneSpaceReplacement = @" />" + Environment.NewLine;

                TextDocumentHelper.SubstituteAllStringMatches(textDocument, oneSpacePattern, oneSpaceReplacement);
            }
            else
            {
                string noSpacePattern = _package.UsePOSIXRegEx ? @"\n*:b+/\>\n" : @"(\r?\n)*[ \t]+/>\r?\n";
                string noSpaceReplacement = @"/>" + Environment.NewLine;

                TextDocumentHelper.SubstituteAllStringMatches(textDocument, noSpacePattern, noSpaceReplacement);
            }
        }

        /// <summary>
        /// Removes the trailing newline from the end of the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveEOFTrailingNewLine(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveEndOfFileTrailingNewLine) return;

            EditPoint cursor = textDocument.EndPoint.CreateEditPoint();

            if (cursor.AtEndOfDocument && cursor.AtStartOfLine && cursor.AtEndOfLine)
            {
                // Make an exception for C++ resource files to work-around known EOF issue: http://connect.microsoft.com/VisualStudio/feedback/details/173903/resource-compiler-returns-a-rc1004-unexpected-eof-found-error#details
                if (textDocument.Language == "C/C++" && 
                    (textDocument.Parent.FullName.EndsWith(".h") || textDocument.Parent.FullName.EndsWith(".rc2")))
                {
                    return;
                }

                var backCursor = cursor.CreateEditPoint();
                backCursor.CharLeft();
                backCursor.Delete(cursor);
            }
        }

        /// <summary>
        /// Removes all end of line whitespace from the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveEOLWhitespace(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveEndOfLineWhitespace) return;

            string pattern = _package.UsePOSIXRegEx ? @":b+\n" : @"[ \t]+\r?\n";
            string replacement = Environment.NewLine;

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// Removes multiple consecutive blank lines from the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void RemoveMultipleConsecutiveBlankLines(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveMultipleConsecutiveBlankLines) return;

            string pattern = _package.UsePOSIXRegEx ? @"\n\n\n+" : @"(\r?\n){3,}";
            string replacement = Environment.NewLine + Environment.NewLine;

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        #endregion Methods
    }
}