#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using System;
using System.IO;
using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Model;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning
{
    /// <summary>
    /// A helper class for performing standard cleaning test operations.
    /// </summary>
    public static class CleaningTestHelper
    {
        /// <summary>
        /// Gets the <see cref="CodeModelManager" />.
        /// </summary>
        internal static CodeModelManager CodeModelManager
        {
            get { return CodeModelManager.GetInstance(TestEnvironment.Package); }
        }

        /// <summary>
        /// Executes the specified command on the specified project item and verifies the results
        /// against the specified baseline file.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="projectItem">The project item to execute the command upon.</param>
        /// <param name="baselinePath">The path to the baseline file for results comparison.</param>
        public static void ExecuteCommandAndVerifyResults(Action<Document> command, ProjectItem projectItem, string baselinePath)
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                var document = GetActivatedDocument(projectItem);

                // Run command and assert it is not saved afterwards.
                Assert.IsTrue(document.Saved);
                command(document);
                Assert.IsFalse(document.Saved);

                // Save the document.
                document.Save();
                Assert.IsTrue(document.Saved);

                // Read the file contents of the baseline and cleaned content and assert they match.
                var baselineContent = File.ReadAllText(baselinePath);
                var cleanedContent = File.ReadAllText(document.FullName);

                Assert.AreEqual(baselineContent, cleanedContent);
            }));
        }

        /// <summary>
        /// Executes the specified command on the specified project item twice and verifies the
        /// second execution does not result in any changes.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="projectItem">The project item to execute the command upon.</param>
        public static void ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(Action<Document> command, ProjectItem projectItem)
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                var document = GetActivatedDocument(projectItem);

                // Run command a first time and assert it is not saved afterwards.
                Assert.IsTrue(document.Saved);
                command(document);
                Assert.IsFalse(document.Saved);

                // Save the document.
                document.Save();
                Assert.IsTrue(document.Saved);

                // Run command a second time and assert it is still in a saved state (i.e. no changes).
                command(document);
                Assert.IsTrue(document.Saved);
            }));
        }

        /// <summary>
        /// Executes the specified command on the specified project item and verifies no change occurs.
        /// </summary>
        /// <remarks>
        /// Used for disabled setting tests.
        /// </remarks>
        /// <param name="command">The command to execute.</param>
        /// <param name="projectItem">The project item to execute the command upon.</param>
        public static void ExecuteCommandAndVerifyNoChanges(Action<Document> command, ProjectItem projectItem)
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                var document = GetActivatedDocument(projectItem);

                // Run command and assert it is still in a saved state (i.e. no changes).
                Assert.IsTrue(document.Saved);
                command(document);
                Assert.IsTrue(document.Saved);
            }));
        }

        /// <summary>
        /// Gets an activated document for the specified project item.
        /// </summary>
        /// <param name="projectItem">The project item.</param>
        /// <returns>The document associated with the project item, opened and activated.</returns>
        public static Document GetActivatedDocument(ProjectItem projectItem)
        {
            projectItem.Open(Constants.vsViewKindTextView);

            var document = projectItem.Document;
            Assert.IsNotNull(projectItem.Document);

            document.Activate();

            return document;
        }
    }
}