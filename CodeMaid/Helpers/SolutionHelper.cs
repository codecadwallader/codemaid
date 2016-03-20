using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A static helper class for working with the solution.
    /// </summary>
    internal static class SolutionHelper
    {
        #region Internal Methods

        /// <summary>
        /// Gets an enumerable set of all items of the specified type within the solution.
        /// </summary>
        /// <typeparam name="T">The type of item to retrieve.</typeparam>
        /// <param name="solution">The solution.</param>
        /// <returns>The enumerable set of all items.</returns>
        internal static IEnumerable<T> GetAllItemsInSolution<T>(Solution solution)
            where T : class
        {
            var allProjects = new List<T>();

            if (solution != null)
            {
                allProjects.AddRange(GetItemsRecursively<T>(solution));
            }

            return allProjects;
        }

        /// <summary>
        /// Gets items of the specified type recursively from the specified parent item. Includes
        /// the parent item if it matches the specified type as well.
        /// </summary>
        /// <typeparam name="T">The type of item to retrieve.</typeparam>
        /// <param name="parentItem">The parent item.</param>
        /// <returns>The enumerable set of items within the parent item, may be empty.</returns>
        internal static IEnumerable<T> GetItemsRecursively<T>(object parentItem)
            where T : class
        {
            if (parentItem == null)
            {
                throw new ArgumentNullException(nameof(parentItem));
            }

            // Create a collection.
            var projectItems = new List<T>();

            // Include the parent item if it is of the desired type.
            var desiredType = parentItem as T;
            if (desiredType != null)
            {
                projectItems.Add(desiredType);
            }

            // Get all children based on the type of parent item.
            var children = GetChildren(parentItem);

            // Then recurse through all children.
            foreach (var childItem in children)
            {
                projectItems.AddRange(GetItemsRecursively<T>(childItem));
            }

            return projectItems;
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
                selectedProjectItems.AddRange(GetItemsRecursively<ProjectItem>(item));
            }

            return selectedProjectItems;
        }

        /// <summary>
        /// Gets an enumerable set of similar project items compared by file name, useful for shared projects.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="projectItem">The project item to match.</param>
        /// <returns>The enumerable set of similar project items.</returns>
        internal static IEnumerable<ProjectItem> GetSimilarProjectItems(CodeMaidPackage package, ProjectItem projectItem)
        {
            var allItems = GetAllItemsInSolution<ProjectItem>(package.IDE.Solution);

            return allItems.Where(x => x.Name == projectItem.Name && x.Kind == projectItem.Kind && x.Document.FullName == projectItem.Document.FullName);
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
            if (solution?.Projects != null)
            {
                return solution.Projects.Cast<Project>().Cast<object>().ToList();
            }

            // Next check if the item is a project.
            var project = parentItem as Project;
            if (project?.ProjectItems != null)
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