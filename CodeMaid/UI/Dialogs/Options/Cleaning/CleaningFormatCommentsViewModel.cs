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

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Cleaning
{
    /// <summary>
    /// The view model for cleaning file types options.
    /// </summary>
    public class CleaningFormatCommentsViewModel : OptionsPageViewModel
    {
        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header
        {
            get { return "Comments"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            FormatComments = Settings.Default.Cleaning_CommentReformat;
            CommentMaxWidth = Settings.Default.Cleaning_CommentMaxWidth;
            MajorTags = Settings.Default.Cleaning_CommentMajorTags;
            MinorTags = Settings.Default.Cleaning_CommentMinorTags;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.Cleaning_ExcludeT4GeneratedCode = FormatComments;
            Settings.Default.Cleaning_CommentMaxWidth = CommentMaxWidth;
            Settings.Default.Cleaning_CommentMajorTags = MajorTags;
            Settings.Default.Cleaning_CommentMinorTags = MinorTags;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        private bool _formatComments;

        /// <summary>
        /// Gets or sets the flag indicating if comments will be formatted.
        /// </summary>
        public bool FormatComments
        {
            get { return _formatComments; }
            set
            {
                if (_formatComments != value)
                {
                    _formatComments = value;
                    NotifyPropertyChanged("FormatComments");
                }
            }
        }

        private int _commentMaxWidth;

        /// <summary>
        /// Gets or sets the maximum width of comments.
        /// </summary>
        public int CommentMaxWidth
        {
            get { return _commentMaxWidth; }
            set
            {
                if (_commentMaxWidth != value)
                {
                    _commentMaxWidth = value;
                    NotifyPropertyChanged("CommentMaxWidth");
                }
            }
        }

        private string _majorTags;

        /// <summary>
        /// Gets or sets the major tag names.
        /// </summary>
        public string MajorTags
        {
            get { return _majorTags; }
            set
            {
                if (_majorTags != value)
                {
                    _majorTags = value;
                    NotifyPropertyChanged("MajorTags");
                }
            }
        }

        private string _minorTags;

        /// <summary>
        /// Gets or sets the minor tag names.
        /// </summary>
        public string MinorTags
        {
            get { return _minorTags; }
            set
            {
                if (_minorTags != value)
                {
                    _minorTags = value;
                    NotifyPropertyChanged("MinorTags");
                }
            }
        }

        #endregion Options
    }
}