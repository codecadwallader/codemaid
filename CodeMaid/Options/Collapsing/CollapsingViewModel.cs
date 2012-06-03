#region CodeMaid is Copyright 2007-2012 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2012 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.Options.Collapsing
{
    /// <summary>
    /// The view model for collapsing options.
    /// </summary>
    public class CollapsingViewModel : OptionsPageViewModel
    {
        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header
        {
            get { return "Collapsing"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            CollapseSolutionWhenOpened = Settings.Default.Collapsing_CollapseSolutionWhenOpened;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.Collapsing_CollapseSolutionWhenOpened = CollapseSolutionWhenOpened;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        private bool _collapseSolutionWhenOpened;

        /// <summary>
        /// Gets or sets a flag indicating if the solution should be collapsed when it is opened.
        /// </summary>
        public bool CollapseSolutionWhenOpened
        {
            get { return _collapseSolutionWhenOpened; }
            set
            {
                if (_collapseSolutionWhenOpened != value)
                {
                    _collapseSolutionWhenOpened = value;
                    NotifyPropertyChanged("CollapseSolutionWhenOpened");
                }
            }
        }

        #endregion Options
    }
}