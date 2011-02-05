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

using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A static helper class for working with the solution.
    /// </summary>
    internal static class SolutionHelper
    {
        #region Internal Methods

        /// <summary>
        /// Gets an enumerable set of all project items within the solution.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>The enumerable set of all project items.</returns>
        internal static IEnumerable<ProjectItem> GetAllProjectItemsInSolution(CodeMaidPackage package)
        {
            var allProjectItems = new List<ProjectItem>();

            var solution = package.IDE.Solution;
            if (solution != null)
            {
                allProjectItems.AddRange(GetProjectItemsRecursively(solution));
            }

            return allProjectItems;
        }

        /// <summary>
        /// Gets an enumerable set of the selected project items.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>The enumerable set of selected project items.</returns>
        internal static IEnumerable<ProjectItem> GetSelectedProjectItemsRecursively(CodeMaidPackage package)
        {
            var selectedProjectItems = new List<ProjectItem>();
            var selectedUIHierarchyItems = UIHierarchyHelper.GetSelectedUIHierarchyItems(package);

            foreach (var item in selectedUIHierarchyItems.Select(uiHierarchyItem => uiHierarchyItem.Object))
            {
                selectedProjectItems.AddRange(GetProjectItemsRecursively(item));
            }

            return selectedProjectItems;
        }

        /// <summary>
        /// Gets the project items recursively from the specified parent item.
        /// Includes the parent item if it is a project item as well.
        /// </summary>
        /// <param name="parentItem">The parent item.</param>
        /// <returns>The enumerable set of project items within the parent item, may be empty.</returns>
        internal static IEnumerable<ProjectItem> GetProjectItemsRecursively(object parentItem)
        {
            if (parentItem == null)
            {
                throw new ArgumentNullException("parentItem");
            }

            // Create a collection.
            var projectItems = new List<ProjectItem>();

            // Include the parent item if it is a project item.
            if (parentItem is ProjectItem)
            {
                projectItems.Add((ProjectItem)parentItem);
            }

            // Get all children based on the type of parent item.
            var children = GetChildren(parentItem);

            // Then recurse through all children.
            foreach (var childItem in children)
            {
                projectItems.AddRange(GetProjectItemsRecursively(childItem));
            }

            return projectItems;
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Gets the children of the specified parent item if applicable.
        /// </summary>
        /// <param name="parentItem">The parent item.</param>
        /// <returns>An enumerable set of children, may be empty.</returns>
        private static IEnumerable<object> GetChildren(object parentItem)
        {
            // First check if the item is a solution.
            var solution = parentItem as Solution;
            if (solution != null && solution.Projects != null)
            {
                return solution.Projects.Cast<Project>().Cast<object>().ToList();
            }

            // Next check if the item is a project.
            var project = parentItem as Project;
            if (project != null && project.ProjectItems != null)
            {
                return project.ProjectItems.Cast<ProjectItem>().Cast<object>().ToList();
            }

            // Next check if the item is a project item.
            var projectItem = parentItem as ProjectItem;
            if (projectItem != null)
            {
                // Standard projects.
                if (projectItem.ProjectItems != null)
                {
                    return projectItem.ProjectItems.Cast<ProjectItem>().Cast<object>().ToList();
                }

                // Projects within a solution folder.
                if (projectItem.SubProject != null)
                {
                    return new[] { projectItem.SubProject };
                }
            }

            // Otherwise return an empty array.
            return new object[0];
        }

        #endregion Private Methods
    }
}