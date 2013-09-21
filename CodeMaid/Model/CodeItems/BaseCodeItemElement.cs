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

using System;
using System.Text.RegularExpressions;
using EnvDTE;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// A base class representation of all code items that have an underlying VSX CodeElement.
    /// </summary>
    public abstract class BaseCodeItemElement : BaseCodeItem
    {
        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the start point adjusted for leading comments, may be null.
        /// </summary>
        public override EditPoint StartPoint
        {
            get { return CodeElement != null ? GetStartPointAdjustedForComments(CodeElement.StartPoint) : null; }
        }

        /// <summary>
        /// Gets the end point, may be null.
        /// </summary>
        public override EditPoint EndPoint
        {
            get { return CodeElement != null ? CodeElement.EndPoint.CreateEditPoint() : null; }
        }

        /// <summary>
        /// Refreshes the cached fields on this item.
        /// </summary>
        public override void Refresh()
        {
            StartLine = CodeElement.StartPoint.Line;
            StartOffset = CodeElement.StartPoint.AbsoluteCharOffset;
            EndLine = CodeElement.EndPoint.Line;
            EndOffset = CodeElement.EndPoint.AbsoluteCharOffset;
            Name = CodeElement.Name;
        }

        #endregion BaseCodeItem Overrides

        #region Properties

        /// <summary>
        /// Gets or sets the code element, may be null.
        /// </summary>
        public CodeElement CodeElement { get; set; }

        /// <summary>
        /// Gets the access level.
        /// </summary>
        public vsCMAccess Access { get; protected set; }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        public CodeElements Attributes { get; protected set; }

        /// <summary>
        /// Gets the doc comment.
        /// </summary>
        public string DocComment { get; protected set; }

        /// <summary>
        /// Gets a flag indicating if this instance is static.
        /// </summary>
        public bool IsStatic { get; protected set; }

        /// <summary>
        /// Gets the type string.
        /// </summary>
        public string TypeString { get; protected set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Tries to execute the specified function, returning the default of the type on error.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="func">The function to execute.</param>
        /// <returns>The result of the function, otherwise the default for the result type.</returns>
        protected static T TryDefault<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Gets a starting point adjusted for leading comments.
        /// </summary>
        /// <param name="originalPoint">The original point.</param>
        /// <returns>The adjusted starting point.</returns>
        private static EditPoint GetStartPointAdjustedForComments(TextPoint originalPoint)
        {
            var point = originalPoint.CreateEditPoint();

            while (point.Line > 1)
            {
                string text = point.GetLines(point.Line - 1, point.Line);

                if (Regex.IsMatch(text, @"^\s*//"))
                {
                    point.LineUp();
                    point.StartOfLine();
                }
                else
                {
                    break;
                }
            }

            return point;
        }

        #endregion Methods
    }
}