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

using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Switching
{
    /// <summary>
    /// The view model for switching options.
    /// </summary>
    public class SwitchingViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchingViewModel"/> class.
        /// </summary>
        /// <param name="package"> The hosting package. </param>
        public SwitchingViewModel(CodeMaidPackage package)
            : base(package)
        {
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header
        {
            get { return "Switching"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            RelatedFileExtensionsExpression = Settings.Default.Switching_RelatedFileExtensionsExpression;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.Switching_RelatedFileExtensionsExpression = RelatedFileExtensionsExpression;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        private string _relatedFileExtensionsExpression;

        /// <summary>
        /// Gets or sets the expression for related file extensions.
        /// </summary>
        public string RelatedFileExtensionsExpression
        {
            get { return _relatedFileExtensionsExpression; }
            set
            {
                if (_relatedFileExtensionsExpression != value)
                {
                    _relatedFileExtensionsExpression = value;
                    NotifyPropertyChanged("RelatedFileExtensionsExpression");
                }
            }
        }

        #endregion Options
    }
}