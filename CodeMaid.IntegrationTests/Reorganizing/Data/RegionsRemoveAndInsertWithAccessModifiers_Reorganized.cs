using System;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Reorganizing.Data
{
    public class RegionsRemoveAndInsertWithAccessModifiers
    {

        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionsRemoveAndInsertWithAccessModifiers" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal RegionsRemoveAndInsertWithAccessModifiers(CodeMaidPackage package)
        {
            #region RegionsInMethodsShouldBeIgnored

            Package = package;

            StartTracking();

            #endregion RegionsInMethodsShouldBeIgnored
        }

        #endregion Internal Constructors

        #region Private Properties

        /// <summary>
        /// Gets or sets the hosting package.
        /// </summary>
        private CodeMaidPackage Package { get; set; }

        /// <summary>
        /// Gets or sets the active document.
        /// </summary>
        private Document TrackedDocument { get; set; }

        #endregion Private Properties

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            RestoreTrackedDocument();
        }

        #endregion Public Methods

        #region Internal Methods

        /// <summary>
        /// Restores the tracked document if not already active.
        /// </summary>
        internal void RestoreTrackedDocument()
        {
            if (TrackedDocument != null && Package.ActiveDocument != TrackedDocument)
            {
                TrackedDocument.Activate();
            }
        }

        /// <summary>
        /// Starts tracking the active document.
        /// </summary>
        internal void StartTracking()
        {
            // Cache the active document.
            TrackedDocument = Package.ActiveDocument;
        }

        #endregion Internal Methods
    }
}