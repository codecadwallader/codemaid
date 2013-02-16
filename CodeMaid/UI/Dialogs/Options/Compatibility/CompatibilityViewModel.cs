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

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Compatibility
{
	/// <summary>
	/// The view model for digging options.
	/// </summary>
	public class CompatibilityViewModel : OptionsPageViewModel
	{

		#region Overrides of OptionsPageViewModel

		/// <summary>
		/// Gets the header.
		/// </summary>
		public override string Header
		{
			get { return "Compatibility"; }
		}

		/// <summary>
		/// Loads the settings.
		/// </summary>
		public override void LoadSettings()
		{
			UseResharperSilentCleanup = Settings.Default.Compatibility_UseReSharperSilentCleanup;
		}

		/// <summary>
		/// Saves the settings.
		/// </summary>
		public override void SaveSettings()
		{
		    Settings.Default.Compatibility_UseReSharperSilentCleanup = UseResharperSilentCleanup;
		}

		#endregion Overrides of OptionsPageViewModel

		#region Options

		/// <summary> 
		/// Gets or sets if resharper silent cleanup should be used instead of visual studio formatting.
		/// </summary>
		public bool UseResharperSilentCleanup
		{
			get { return _useResharperSilentCleanup; }
			set
			{
				if (_useResharperSilentCleanup != value)
				{
					_useResharperSilentCleanup = value;
					NotifyPropertyChanged("UseResharperSilentCleanup");
				}
			}
		}

		private bool _useResharperSilentCleanup;

		#endregion Options
	}
}