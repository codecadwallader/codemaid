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

namespace SteveCadwallader.CodeMaid.Options.Cleaning
{
    /// <summary>
    /// The view model for cleaning remove options.
    /// </summary>
    public class CleaningRemoveViewModel : OptionsPageViewModel
    {
        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header
        {
            get { return "Remove"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            RemoveBlankLinesAfterAttributes = Settings.Default.Cleaning_RemoveBlankLinesAfterAttributes;
            RemoveBlankLinesAfterOpeningBrace = Settings.Default.Cleaning_RemoveBlankLinesAfterOpeningBrace;
            RemoveBlankLinesAtBottom = Settings.Default.Cleaning_RemoveBlankLinesAtBottom;
            RemoveBlankLinesAtTop = Settings.Default.Cleaning_RemoveBlankLinesAtTop;
            RemoveBlankLinesBeforeClosingBrace = Settings.Default.Cleaning_RemoveBlankLinesBeforeClosingBrace;
            RemoveEndOfLineWhitespace = Settings.Default.Cleaning_RemoveEndOfLineWhitespace;
            RemoveMultipleConsecutiveBlankLines = Settings.Default.Cleaning_RemoveMultipleConsecutiveBlankLines;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.Cleaning_RemoveBlankLinesAfterAttributes = RemoveBlankLinesAfterAttributes;
            Settings.Default.Cleaning_RemoveBlankLinesAfterOpeningBrace = RemoveBlankLinesAfterOpeningBrace;
            Settings.Default.Cleaning_RemoveBlankLinesAtBottom = RemoveBlankLinesAtBottom;
            Settings.Default.Cleaning_RemoveBlankLinesAtTop = RemoveBlankLinesAtTop;
            Settings.Default.Cleaning_RemoveBlankLinesBeforeClosingBrace = RemoveBlankLinesBeforeClosingBrace;
            Settings.Default.Cleaning_RemoveEndOfLineWhitespace = RemoveEndOfLineWhitespace;
            Settings.Default.Cleaning_RemoveMultipleConsecutiveBlankLines = RemoveMultipleConsecutiveBlankLines;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        private bool _removeBlankLinesAfterAttributes;

        /// <summary>
        /// Gets or sets the flag indicating if blank lines should be removed after attributes.
        /// </summary>
        public bool RemoveBlankLinesAfterAttributes
        {
            get { return _removeBlankLinesAfterAttributes; }
            set
            {
                if (_removeBlankLinesAfterAttributes != value)
                {
                    _removeBlankLinesAfterAttributes = value;
                    NotifyPropertyChanged("RemoveBlankLinesAfterAttributes");
                }
            }
        }

        private bool _removeBlankLinesAfterOpeningBrace;

        /// <summary>
        /// Gets or sets the flag indicating if blank lines should be removed after an opening brace.
        /// </summary>
        public bool RemoveBlankLinesAfterOpeningBrace
        {
            get { return _removeBlankLinesAfterOpeningBrace; }
            set
            {
                if (_removeBlankLinesAfterOpeningBrace != value)
                {
                    _removeBlankLinesAfterOpeningBrace = value;
                    NotifyPropertyChanged("RemoveBlankLinesAfterOpeningBrace");
                }
            }
        }

        private bool _removeBlankLinesAtBottom;

        /// <summary>
        /// Gets or sets the flag indicating if blank lines should be removed at the bottom of the file.
        /// </summary>
        public bool RemoveBlankLinesAtBottom
        {
            get { return _removeBlankLinesAtBottom; }
            set
            {
                if (_removeBlankLinesAtBottom != value)
                {
                    _removeBlankLinesAtBottom = value;
                    NotifyPropertyChanged("RemoveBlankLinesAtBottom");
                }
            }
        }

        private bool _removeBlankLinesAtTop;

        /// <summary>
        /// Gets or sets the flag indicating if blank lines should be removed at the top of the file.
        /// </summary>
        public bool RemoveBlankLinesAtTop
        {
            get { return _removeBlankLinesAtTop; }
            set
            {
                if (_removeBlankLinesAtTop != value)
                {
                    _removeBlankLinesAtTop = value;
                    NotifyPropertyChanged("RemoveBlankLinesAtTop");
                }
            }
        }

        private bool _removeBlankLinesBeforeClosingBrace;

        /// <summary>
        /// Gets or sets the flag indicating if blank lines should be removed before a closing brace.
        /// </summary>
        public bool RemoveBlankLinesBeforeClosingBrace
        {
            get { return _removeBlankLinesBeforeClosingBrace; }
            set
            {
                if (_removeBlankLinesBeforeClosingBrace != value)
                {
                    _removeBlankLinesBeforeClosingBrace = value;
                    NotifyPropertyChanged("RemoveBlankLinesBeforeClosingBrace");
                }
            }
        }

        private bool _removeEndOfLineWhitespace;

        /// <summary>
        /// Gets or sets the flag indicating if end of line whitespace should be removed.
        /// </summary>
        public bool RemoveEndOfLineWhitespace
        {
            get { return _removeEndOfLineWhitespace; }
            set
            {
                if (_removeEndOfLineWhitespace != value)
                {
                    _removeEndOfLineWhitespace = value;
                    NotifyPropertyChanged("RemoveEndOfLineWhitespace");
                }
            }
        }

        private bool _removeMultipleConsecutiveBlankLines;

        /// <summary>
        /// Gets or sets the flag indicating if multiple consecutive blank lines should be removed.
        /// </summary>
        public bool RemoveMultipleConsecutiveBlankLines
        {
            get { return _removeMultipleConsecutiveBlankLines; }
            set
            {
                if (_removeMultipleConsecutiveBlankLines != value)
                {
                    _removeMultipleConsecutiveBlankLines = value;
                    NotifyPropertyChanged("RemoveMultipleConsecutiveBlankLines");
                }
            }
        }

        #endregion Options
    }
}