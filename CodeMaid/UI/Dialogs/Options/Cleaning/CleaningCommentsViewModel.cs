#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Cleaning
{
    /// <summary>
    /// The view model for cleaning comment options.
    /// </summary>
    public class CleaningCommentsViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleaningCommentsViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        public CleaningCommentsViewModel(CodeMaidPackage package)
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
            get { return "Comments"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            CommentMajorTags = Settings.Default.Cleaning_CommentMajorTags;
            CommentMinorTags = Settings.Default.Cleaning_CommentMinorTags;
            CommentRunDuringCleanup = Settings.Default.Cleaning_CommentRunDuringCleanup;
            CommentSkipWrapOnLastWord = Settings.Default.Cleaning_CommentSkipWrapOnLastWord;
            CommentWrapColumn = Settings.Default.Cleaning_CommentWrapColumn;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.Cleaning_CommentMajorTags = CommentMajorTags;
            Settings.Default.Cleaning_CommentMinorTags = CommentMinorTags;
            Settings.Default.Cleaning_CommentRunDuringCleanup = CommentRunDuringCleanup;
            Settings.Default.Cleaning_CommentSkipWrapOnLastWord = CommentSkipWrapOnLastWord;
            Settings.Default.Cleaning_CommentWrapColumn = CommentWrapColumn;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        private string _commentMajorTags;

        /// <summary>
        /// Gets or sets the major tag names.
        /// </summary>
        public string CommentMajorTags
        {
            get { return _commentMajorTags; }
            set
            {
                if (_commentMajorTags != value)
                {
                    _commentMajorTags = value;
                    NotifyPropertyChanged("CommentMajorTags");
                }
            }
        }

        private string _commentMinorTags;

        /// <summary>
        /// Gets or sets the minor tag names.
        /// </summary>
        public string CommentMinorTags
        {
            get { return _commentMinorTags; }
            set
            {
                if (_commentMinorTags != value)
                {
                    _commentMinorTags = value;
                    NotifyPropertyChanged("CommentMinorTags");
                }
            }
        }

        private bool _commentRunDuringCleanup;

        /// <summary>
        /// Gets or sets the flag indicating if comment formatting will run during cleanup.
        /// </summary>
        public bool CommentRunDuringCleanup
        {
            get { return _commentRunDuringCleanup; }
            set
            {
                if (_commentRunDuringCleanup != value)
                {
                    _commentRunDuringCleanup = value;
                    NotifyPropertyChanged("CommentRunDuringCleanup");
                }
            }
        }

        private bool _commentSkipWrapOnLastWord;

        /// <summary>
        /// Gets or sets the flag indicating if comment formatting should skip wrapping the last word.
        /// </summary>
        public bool CommentSkipWrapOnLastWord
        {
            get { return _commentSkipWrapOnLastWord; }
            set
            {
                if (_commentSkipWrapOnLastWord != value)
                {
                    _commentSkipWrapOnLastWord = value;
                    NotifyPropertyChanged("CommentSkipWrapOnLastWord");
                }
            }
        }

        private int _commentWrapColumn;

        /// <summary>
        /// Gets or sets the column where comments will attempt to wrap.
        /// </summary>
        public int CommentWrapColumn
        {
            get { return _commentWrapColumn; }
            set
            {
                if (_commentWrapColumn != value)
                {
                    _commentWrapColumn = value;
                    NotifyPropertyChanged("CommentWrapColumn");
                }
            }
        }

        #endregion Options

        #region Enables

        /// <summary>
        /// Gets a flag indicating if this page should be enabled.
        /// </summary>
        /// <remarks>
        /// Disable comment formatting if using POSIX Regular Expressions (i.e. pre-Visual Studio 11
        /// versions) since not supported.
        /// </remarks>
        public bool IsEnabled
        {
            get { return !Package.UsePOSIXRegEx; }
        }

        #endregion Enables
    }
}