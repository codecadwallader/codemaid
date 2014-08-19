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
            "<summary>Lorem ipsum dolor sit amet.</summary>" +
            "<param name=\"p1\">Praesent sollicitudin massa nunc.</param>" +
            "<param name=\"param2\">Maecenas id neque ultricies.</param>" +
            "<returns>Praesent euismod diam porta pulvinar, quis ut pharetra.</returns>" +
            "<remark>Phasellus porta luctus lorem. Ut tincidunt sapien quam, <see cref=\"nec malesuada\"/> nec malesuada enim elementum at.</remark>";

        private readonly EnvDTE.Properties _editorProperties;
        private readonly EnvDTE.ColorableItems _commentColors;
        private CodeCommentOptions _options;

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

            PropertyChanged += (sender, args) => UpdatePreviewText();
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
            _options = new CodeCommentOptions(Settings.Default, 4);

            RaisePropertyChangedForAllOptionsProperties();
            UpdatePreviewText();
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            _options.Save(Settings.Default);
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets the flag indicating if comment formatting will run during cleanup.
        /// </summary>
        public bool FormatDuringCleanup
        {
            get { return _options.FormatDuringCleanup; }
            set
            {
                if (_options.FormatDuringCleanup != value)
                {
                    _options.FormatDuringCleanup = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if comment formatting should skip wrapping the last word.
        /// </summary>
        public bool SkipWrapOnLastWord
        {
            get { return _options.SkipWrapOnLastWord; }
            set
            {
                if (_options.SkipWrapOnLastWord != value)
                {
                    _options.SkipWrapOnLastWord = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the column where comments will attempt to wrap.
        /// </summary>
        public int WrapAtColumn
        {
            get { return _options.WrapAtColumn; }
            set
            {
                value = Math.Max(value, 0);
                if (_options.WrapAtColumn != value)
                {
                    _options.WrapAtColumn = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if the content of param tags should be aligned.
        /// </summary>
        public bool XmlAlignParamTags
        {
            get { return _options.XmlAlignParamTags; }
            set
            {
                if (_options.XmlAlignParamTags != value)
                {
                    _options.XmlAlignParamTags = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if an extra space should be added inside XML tags.
        /// </summary>
        public bool XmlSpaceTagContent
        {
            get { return _options.XmlSpaceTagContent; }
            set
            {
                if (_options.XmlSpaceTagContent != value)
                {
                    _options.XmlSpaceTagContent = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if summary tags should always be split to multiple lines.
        /// </summary>
        public bool XmlSplitSummaryTag
        {
            get { return _options.XmlSplitSummaryTag; }
            set
            {
                if (_options.XmlSplitSummaryTag != value)
                {
                    _options.XmlSplitSummaryTag = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the amount of extra spacing to add before XML values.
        /// </summary>
        public int XmlValueIndent
        {
            get { return _options.XmlValueIndent; }
            set
            {
                value = Math.Max(value, 0);
                if (_options.XmlValueIndent != value)
                {
                    _options.XmlValueIndent = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool XmlTagsToLowerCase
        {
            get { return _options.XmlTagsToLowerCase; }
            set
            {
                if (_options.XmlTagsToLowerCase != value)
                {
                    _options.XmlTagsToLowerCase = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool XmlSpaceSingleTags
        {
            get { return _options.XmlSpaceSingleTags; }
            set
            {
                if (_options.XmlSpaceSingleTags != value)
                {
                    _options.XmlSpaceSingleTags = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool XmlKeepTagsTogether
        {
            get { return _options.XmlKeepTagsTogether; }
            set
            {
                if (_options.XmlKeepTagsTogether != value)
                {
                    _options.XmlKeepTagsTogether = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool XmlSplitAllTags
        {
            get { return _options.XmlSplitAllTags; }
            set
            {
                if (_options.XmlSplitAllTags != value)
                {
                    _options.XmlSplitAllTags = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Calls <see cref="Bindable.RaisePropertyChanged"/> for every public property exposed on
        /// the <see cref="CodeCommentOptions"/> class. This ensures that changes to that class
        /// (which do not raise events) will be bubbled up to the UI correctly.
        /// </summary>
        /// <remarks>
        /// Note: For this to work properly, the wrapping property on this view model must match the
        ///       name of the underlying property within the options class.
        /// </remarks>
        private void RaisePropertyChangedForAllOptionsProperties()
        {
            var optionsProperties = _options.GetType().GetProperties();
            foreach (var optionsProperty in optionsProperties)
            {
                RaisePropertyChanged(optionsProperty.Name);
            }
        }

        #endregion Options

        #region Preview Text and Helpers

        public string CommentPreviewText
        {
            get { return GetPropertyValue<string>(); }
            private set { SetPropertyValue(value); }
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

        private void UpdatePreviewText()
        {
            var temp = _options.WrapAtColumn;
            _options.WrapAtColumn = 75; // Override to fit preview text better

            CommentPreviewText = CodeComment.FormatXml(
                UnformattedPreviewText,
                _options);

            _options.WrapAtColumn = temp;
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