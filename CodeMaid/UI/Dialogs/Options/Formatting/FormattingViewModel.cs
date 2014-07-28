#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using System;
using System.Windows.Media;
using SteveCadwallader.CodeMaid.Model.Comments;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Formatting
{
    /// <summary>
    /// The view model for comment formatting options.
    /// </summary>
    public class FormattingViewModel : OptionsPageViewModel
    {
        #region Fields

        private const string UnformattedPreviewText =
            "<summary>Lorem ipsum dolor sit amet.</summary>" +
            "<param name=\"p1\">Praesent sollicitudin massa nunc.</param>" +
            "<param name=\"param2\">Maecenas id neque ultricies.</param>" +
            "<returns>Praesent euismod diam porta pulvinar, quis ut pharetra.</returns>" +
            "<remark>Phasellus porta luctus lorem. Ut tincidunt sapien quam, <see cref=\"nec malesuada\"/> nec malesuada enim elementum at.</remark>";

        private readonly EnvDTE.Properties _editorProperties;
        private readonly EnvDTE.ColorableItems _commentColors;
        private CodeCommentOptions options;
        private string _commentPreviewText;

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
            options = new CodeCommentOptions(4);

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

        /// <summary>
        /// Gets or sets the flag indicating if comment formatting will run during cleanup.
        /// </summary>
        public bool CommentRunDuringCleanup
        {
            get { return options.FormatDuringCleanup; }
            set
            {
                if (options.FormatDuringCleanup != value)
                {
                    options.FormatDuringCleanup = value;
                    NotifyPropertyChanged("CommentRunDuringCleanup");
                }
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if comment formatting should skip wrapping the last word.
        /// </summary>
        public bool CommentSkipWrapOnLastWord
        {
            get { return options.SkipWrapOnLastWord; }
            set
            {
                if (options.SkipWrapOnLastWord != value)
                {
                    options.SkipWrapOnLastWord = value;
                    NotifyPropertyChanged("CommentSkipWrapOnLastWord");
                }
            }
        }

        /// <summary>
        /// Gets or sets the column where comments will attempt to wrap.
        /// </summary>
        public int CommentWrapColumn
        {
            get { return options.WrapAtColumn; }
            set
            {
                value = Math.Max(value, 0);
                if (options.WrapAtColumn != value)
                {
                    options.WrapAtColumn = value;
                    NotifyPropertyChanged("CommentWrapColumn");
                }
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if the content of param tags should be aligned.
        /// </summary>
        public bool CommentXmlAlignParamTags
        {
            get { return options.XmlAlignParamTags; }
            set
            {
                if (options.XmlAlignParamTags != value)
                {
                    options.XmlAlignParamTags = value;
                    NotifyPropertyChanged("CommentXmlAlignParamTags");
                }
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if an extra space should be added inside XML tags.
        /// </summary>
        public bool CommentXmlSpaceTags
        {
            get { return options.XmlSpaceTagContent; }
            set
            {
                if (options.XmlSpaceTagContent != value)
                {
                    options.XmlSpaceTagContent = value;
                    NotifyPropertyChanged("CommentXmlSpaceTags");
                }
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if summary tags should always be split to multiple lines.
        /// </summary>
        public bool CommentXmlSplitSummaryTagToMultipleLines
        {
            get { return options.XmlSplitSummaryTag; }
            set
            {
                if (options.XmlSplitSummaryTag != value)
                {
                    options.XmlSplitSummaryTag = value;
                    NotifyPropertyChanged("CommentXmlSplitSummaryTagToMultipleLines");
                }
            }
        }

        /// <summary>
        /// Gets or sets the amount of extra spacing to add before XML values.
        /// </summary>
        public int CommentXmlValueIndent
        {
            get { return options.XmlValueIndent; }
            set
            {
                value = Math.Max(value, 0);
                if (options.XmlValueIndent != value)
                {
                    options.XmlValueIndent = value;
                    NotifyPropertyChanged("CommentXmlValueIndent");
                }
            }
        }

        public bool CommentXmlTagsToLowerCase
        {
            get { return options.XmlTagsToLowerCase; }
            set
            {
                if (options.XmlTagsToLowerCase != value)
                {
                    options.XmlTagsToLowerCase = value;
                    NotifyPropertyChanged("CommentXmlTagsToLowerCase");
                }
            }
        }

        public bool CommentXmlSpaceSingleTags
        {
            get { return options.XmlSpaceSingleTags; }
            set
            {
                if (options.XmlSpaceSingleTags != value)
                {
                    options.XmlSpaceSingleTags = value;
                    NotifyPropertyChanged("CommentXmlSpaceSingleTags");
                }
            }
        }

        public bool CommentXmlKeepTagsTogether
        {
            get { return options.XmlKeepTagsTogether; }
            set
            {
                if (options.XmlKeepTagsTogether != value)
                {
                    options.XmlKeepTagsTogether = value;
                    NotifyPropertyChanged("CommentXmlKeepTagsTogether");
                }
            }
        }

        public bool CommentXmlSplitAllTags
        {
            get { return options.XmlSplitAllTags; }
            set
            {
                if (options.XmlSplitAllTags != value)
                {
                    options.XmlSplitAllTags = value;
                    NotifyPropertyChanged("CommentXmlSplitAllTags");
                }
            }
        }

        #endregion Options

        #region Preview Text and Helpers

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
            var temp = options.WrapAtColumn;
            options.WrapAtColumn = 75; // Override to fit preview text better

            CommentPreviewText = CodeComment.FormatXml(
                UnformattedPreviewText,
                options);

            options.WrapAtColumn = temp;
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