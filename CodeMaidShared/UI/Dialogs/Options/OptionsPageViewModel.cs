using SteveCadwallader.CodeMaid.Properties;
using System.Collections.Generic;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options
{
    /// <summary>
    /// The abstract base class for option pages.
    /// </summary>
    public abstract class OptionsPageViewModel : Bindable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsPageViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="activeSettings">The active settings.</param>
        protected OptionsPageViewModel(CodeMaidPackage package, Settings activeSettings)
        {
            Package = package;
            ActiveSettings = activeSettings;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the header.
        /// </summary>
        public abstract string Header { get; }

        /// <summary>
        /// Gets the hosting package.
        /// </summary>
        public CodeMaidPackage Package { get; private set; }

        /// <summary>
        /// Gets the active settings.
        /// </summary>
        public Settings ActiveSettings { get; private set; }

        private IEnumerable<OptionsPageViewModel> _children;

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        public IEnumerable<OptionsPageViewModel> Children
        {
            get { return _children ?? (_children = new OptionsPageViewModel[0]); }
            set
            {
                if (_children != value)
                {
                    _children = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the list of settings to options mappings.
        /// </summary>
        protected SettingsToOptionsList Mappings { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public virtual void LoadSettings()
        {
            Mappings?.CopySettingsToOptions();
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public virtual void SaveSettings()
        {
            Mappings?.CopyOptionsToSettings();
        }

        #endregion Methods
    }
}