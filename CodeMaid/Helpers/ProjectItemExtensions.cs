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
        /// Attempts to retrieve the file name for the specified project item.
        /// </summary>
        /// <param name="projectItem">The project item.</param>
        /// <returns>The filename of the project item if available, otherwise null.</returns>
        internal static string GetFileName(this ProjectItem projectItem)
        {
            try
            {
                return projectItem.FileNames[1];
            }
            catch (Exception ex)
            {
                OutputWindowHelper.DiagnosticWriteLine("Unable to retrieve ProjectItem file name", ex);
                return null;
            }
        }

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
                OutputWindowHelper.DiagnosticWriteLine("Unable to retrieve parent ProjectItem", ex);
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
                OutputWindowHelper.DiagnosticWriteLine("Unable to determine if ProjectItem is external", ex);
                return false;
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
                OutputWindowHelper.DiagnosticWriteLine("Unable to determine if ProjectItem is a physical file", ex);
                return false;
            }
        }
    }
}