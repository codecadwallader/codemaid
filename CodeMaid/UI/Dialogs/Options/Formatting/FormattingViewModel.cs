#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Model.Comments;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Windows.Media;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Formatting
{
    /// <summary>
    /// The view model for comment formatting options.
    /// </summary>
    public class FormattingViewModel : OptionsPageViewModel
    {
        #region Fields

        private const string UnformattedPreviewText =
            "<summary>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus nisi neque, placerat sed neque vitae. Donec mattis vitae velit sed imperdiet.</summary>" +
            "<param name=\"p1\">Praesent sollicitudin massa nunc.</param>" +
            "<param name=\"param2\">Maecenas id neque ultricies.</param>" +
            "<returns>Praesent euismod diam porta pulvinar, quis ut pharetra.</returns>";

        private readonly EnvDTE.Properties _editorProperties;
        private readonly EnvDTE.ColorableItems _commentColors;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FormattingViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        public FormattingViewModel(CodeMaidPackage package)
            : base(package)
        {
            _editorProperties = Package.IDE.Properties["FontsAndColors", "TextEditor"];
            var property = _editorProperties.Item("FontsAndColorsItems");
            var fontsAndColorsItems = (EnvDTE.FontsAndColorsItems)property.Object;
            _commentColors = fontsAndColorsItems.Item("Comment");
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header
        {
            get { return "Formatting"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            CommentRunDuringCleanup = Settings.Default.Formatting_CommentRunDuringCleanup;
            CommentSkipWrapOnLastWord = Settings.Default.Formatting_CommentSkipWrapOnLastWord;
            CommentWrapColumn = Settings.Default.Formatting_CommentWrapColumn;
            CommentXmlAlignParamTags = Settings.Default.Formatting_CommentXmlAlignParamTags;
            CommentXmlSpaceTags = Settings.Default.Formatting_CommentXmlSpaceTags;
            CommentXmlSplitSummaryTagToMultipleLines = Settings.Default.Formatting_CommentXmlSplitSummaryTagToMultipleLines;
            CommentXmlValueIndent = Settings.Default.Formatting_CommentXmlValueIndent;

            UpdatePreviewText();
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.Formatting_CommentRunDuringCleanup = CommentRunDuringCleanup;
            Settings.Default.Formatting_CommentSkipWrapOnLastWord = CommentSkipWrapOnLastWord;
            Settings.Default.Formatting_CommentWrapColumn = CommentWrapColumn;
            Settings.Default.Formatting_CommentXmlAlignParamTags = CommentXmlAlignParamTags;
            Settings.Default.Formatting_CommentXmlSpaceTags = CommentXmlSpaceTags;
            Settings.Default.Formatting_CommentXmlSplitSummaryTagToMultipleLines = CommentXmlSplitSummaryTagToMultipleLines;
            Settings.Default.Formatting_CommentXmlValueIndent = CommentXmlValueIndent;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

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
                value = Math.Max(value, 0);
                if (_commentWrapColumn != value)
                {
                    _commentWrapColumn = value;
                    NotifyPropertyChanged("CommentWrapColumn");
                }
            }
        }

        private bool _commentXmlAlignParamTags;

        /// <summary>
        /// Gets or sets the flag indicating if the content of param tags should be aligned.
        /// </summary>
        public bool CommentXmlAlignParamTags
        {
            get { return _commentXmlAlignParamTags; }
            set
            {
                if (_commentXmlAlignParamTags != value)
                {
                    _commentXmlAlignParamTags = value;
                    NotifyPropertyChanged("CommentXmlAlignParamTags");
                }
            }
        }

        private bool _commentXmlSpaceTags;

        /// <summary>
        /// Gets or sets the flag indicating if an extra space should be added inside XML tags.
        /// </summary>
        public bool CommentXmlSpaceTags
        {
            get { return _commentXmlSpaceTags; }
            set
            {
                if (_commentXmlSpaceTags != value)
                {
                    _commentXmlSpaceTags = value;
                    NotifyPropertyChanged("CommentXmlSpaceTags");
                }
            }
        }

        private bool _commentXmlSplitSummaryTagToMultipleLines;

        /// <summary>
        /// Gets or sets the flag indicating if summary tags should always be split to multiple lines.
        /// </summary>
        public bool CommentXmlSplitSummaryTagToMultipleLines
        {
            get { return _commentXmlSplitSummaryTagToMultipleLines; }
            set
            {
                if (_commentXmlSplitSummaryTagToMultipleLines != value)
                {
                    _commentXmlSplitSummaryTagToMultipleLines = value;
                    NotifyPropertyChanged("CommentXmlSplitSummaryTagToMultipleLines");
                }
            }
        }

        private int _commentXmlValueIndent;

        /// <summary>
        /// Gets or sets the amount of extra spacing to add before XML values.
        /// </summary>
        public int CommentXmlValueIndent
        {
            get { return _commentXmlValueIndent; }
            set
            {
                value = Math.Max(value, 0);
                if (_commentXmlValueIndent != value)
                {
                    _commentXmlValueIndent = value;
                    NotifyPropertyChanged("CommentXmlValueIndent");
                }
            }
        }

        #endregion Options

        #region Preview Text and Helpers

        private string _commentPreviewText;

        public string CommentPreviewText
        {
            get { return _commentPreviewText; }
            private set
            {
                if (_commentPreviewText != value)
                {
                    _commentPreviewText = value;
                    NotifyPropertyChanged("CommentPreviewText");
                }
            }
        }

        public FontFamily CommentPreviewTextFont
        {
            get { return new FontFamily(_editorProperties.Item("FontFamily").Value.ToString()); }
        }

        public Brush CommentPreviewTextForeground
        {
            get
            {
                var color = System.Drawing.ColorTranslator.FromOle((int)_commentColors.Foreground);

                return new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            }
        }

        public Brush CommentPreviewTextBackground
        {
            get
            {
                var color = System.Drawing.ColorTranslator.FromOle((int)_commentColors.Background);

                return new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            }
        }

        protected override void NotifyPropertyChanged(string propertyName)
        {
            base.NotifyPropertyChanged(propertyName);
            UpdatePreviewText();
        }

        private void UpdatePreviewText()
        {
            CommentPreviewText = CodeComment.FormatXml(
                UnformattedPreviewText,
                new CodeCommentOptions
                {
                    SkipWrapOnLastWord = CommentSkipWrapOnLastWord,
                    TabSize = 4, // Not important for preview
                    WrapAtColumn = 75, // Overridden to fit interface better
                    XmlValueIndent = CommentXmlValueIndent,
                    XmlSpaceTagContent = CommentXmlSpaceTags,
                    XmlAlignParamTags = CommentXmlAlignParamTags,
                    XmlBreakSummaryTag = CommentXmlSplitSummaryTagToMultipleLines
                });
        }

        #endregion Preview Text and Helpers

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