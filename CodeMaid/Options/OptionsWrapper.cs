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

namespace SteveCadwallader.CodeMaid.Options
{
    /// <summary>
    /// A wrapper for the individual options pages.
    /// </summary>
    public class OptionsWrapper
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsWrapper"/> class.
        /// </summary>
        /// <param name="buildStatusOptionsPage">The build status options page.</param>
        /// <param name="cleanupGeneralOptionsPage">The cleanup general options page.</param>
        /// <param name="cleanupInsertOptionsPage">The cleanup insert options page.</param>
        /// <param name="cleanupRemoveOptionsPage">The cleanup remove options page.</param>
        /// <param name="cleanupUpdateOptionsPage">The cleanup update options page.</param>
        /// <param name="snooperOptionsPage">The snooper options page.</param>
        /// <param name="switchFileOptionsPage">The switch file options page.</param>
        public OptionsWrapper(BuildStatusOptionsPage buildStatusOptionsPage,
                              CleanupGeneralOptionsPage cleanupGeneralOptionsPage,
                              CleanupInsertOptionsPage cleanupInsertOptionsPage,
                              CleanupRemoveOptionsPage cleanupRemoveOptionsPage,
                              CleanupUpdateOptionsPage cleanupUpdateOptionsPage,
                              SnooperOptionsPage snooperOptionsPage,
                              SwitchFileOptionsPage switchFileOptionsPage)
        {
            BuildStatus = buildStatusOptionsPage;
            CleanupGeneral = cleanupGeneralOptionsPage;
            CleanupInsert = cleanupInsertOptionsPage;
            CleanupRemove = cleanupRemoveOptionsPage;
            CleanupUpdate = cleanupUpdateOptionsPage;
            Snooper = snooperOptionsPage;
            SwitchFile = switchFileOptionsPage;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets the build status options.
        /// </summary>
        public BuildStatusOptionsPage BuildStatus { get; private set; }

        /// <summary>
        /// Gets the cleanup general options.
        /// </summary>
        public CleanupGeneralOptionsPage CleanupGeneral { get; private set; }

        /// <summary>
        /// Gets the cleanup insert options.
        /// </summary>
        public CleanupInsertOptionsPage CleanupInsert { get; private set; }

        /// <summary>
        /// Gets the cleanup remove options.
        /// </summary>
        public CleanupRemoveOptionsPage CleanupRemove { get; private set; }

        /// <summary>
        /// Gets the cleanup update options.
        /// </summary>
        public CleanupUpdateOptionsPage CleanupUpdate { get; private set; }

        /// <summary>
        /// Gets the snooper options.
        /// </summary>
        public SnooperOptionsPage Snooper { get; private set; }

        /// <summary>
        /// Gets the switch file options.
        /// </summary>
        public SwitchFileOptionsPage SwitchFile { get; private set; }

        #endregion Public Properties
    }
}