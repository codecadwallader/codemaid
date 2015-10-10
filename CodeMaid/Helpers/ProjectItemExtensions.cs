#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using EnvDTE;
using System;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A set of extension methods for <see cref="ProjectItem" />.
    /// </summary>
    internal static class ProjectItemExtensions
    {
        /// <summary>
        /// Attempts to retrieve the parent project item for the specified project item.
        /// </summary>
        /// <param name="projectItem">The project item.</param>
        /// <returns>The parent project item, otherwise null.</returns>
        internal static ProjectItem GetParentProjectItem(this ProjectItem projectItem)
        {
            try
            {
                var parentProjectItem = projectItem.Collection?.Parent as ProjectItem;
                return parentProjectItem;
            }
            catch (Exception ex)
            {
                OutputWindowHelper.ExceptionWriteLine("Unable to retrieve parent ProjectItem", ex);
                return null;
            }
        }

        /// <summary>
        /// Determines if the specified project item is external to the solution.
        /// </summary>
        /// <param name="projectItem">The project item.</param>
        /// <returns>True if the project item is external, otherwise false.</returns>
        internal static bool IsExternal(this ProjectItem projectItem)
        {
            try
            {
                if (projectItem.Collection == null || !projectItem.IsPhysicalFile())
                {
                    return true;
                }

                return projectItem.Collection.OfType<ProjectItem>().All(x => x.Object != projectItem.Object);
            }
            catch (Exception ex)
            {
                OutputWindowHelper.ExceptionWriteLine("Unable to determine if ProjectItem is external", ex);
                return true;
            }
        }

        /// <summary>
        /// Determines if the specified project item is a physical file.
        /// </summary>
        /// <param name="projectItem">The project item.</param>
        /// <returns>True if the project item is a physical file, otherwise false.</returns>
        internal static bool IsPhysicalFile(this ProjectItem projectItem)
        {
            try
            {
                return string.Equals(projectItem.Kind, Constants.vsProjectItemKindPhysicalFile, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                // Some ProjectItem types (e.g. WiX) may throw an error when accessing the Kind member.
                OutputWindowHelper.ExceptionWriteLine("Unable to determine if ProjectItem is a physical file", ex);
                return false;
            }
        }
    }
}