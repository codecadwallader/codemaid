using SteveCadwallader.CodeMaid.Model.Comments;
using SteveCadwallader.CodeMaid.Properties;
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

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FormattingViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="activeSettings">The active settings.</param>
        public FormattingViewModel(CodeMaidPackage package, Settings activeSettings)
            : base(package, activeSettings)
        {
            Mappings = new SettingsToOptionsList(ActiveSettings, this)
            {
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Formatting_CommentRunDuringCleanup, x => CommentRunDuringCleanup),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Formatting_CommentSkipWrapOnLastWord, x => CommentSkipWrapOnLastWord),
                new SettingToOptionMapping<int, int>(x => ActiveSettings.Formatting_CommentWrapColumn, x => CommentWrapColumn),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Formatting_CommentXmlAlignParamTags, x => CommentXmlAlignParamTags),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Formatting_CommentXmlKeepTagsTogether, x => CommentXmlKeepTagsTogether),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Formatting_CommentXmlSpaceSingleTags, x => CommentXmlSpaceSingleTags),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Formatting_CommentXmlSpaceTags, x => CommentXmlSpaceTags),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Formatting_CommentXmlSplitAllTags, x => CommentXmlSplitAllTags),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Formatting_CommentXmlSplitSummaryTagToMultipleLines, x => CommentXmlSplitSummaryTagToMultipleLines),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Formatting_CommentXmlTagsToLowerCase, x => CommentXmlTagsToLowerCase),
                new SettingToOptionMapping<int, int>(x => ActiveSettings.Formatting_CommentXmlValueIndent, x => CommentXmlValueIndent)
            };

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
        public override string Header => "Formatting";

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            base.LoadSettings();

            UpdatePreviewText();
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets the flag indicating if comment formatting will run during cleanup.
        /// </summary>
        public bool CommentRunDuringCleanup
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if comment formatting should skip wrapping the last word.
        /// </summary>
        public bool CommentSkipWrapOnLastWord
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the column where comments will attempt to wrap.
        /// </summary>
        public int CommentWrapColumn
        {
            get { return GetPropertyValue<int>(); }
            set
            {
                if (value >= 0)
                {
                    SetPropertyValue(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if the content of param tags should be aligned.
        /// </summary>
        public bool CommentXmlAlignParamTags
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        public bool CommentXmlKeepTagsTogether
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        public bool CommentXmlSpaceSingleTags
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if an extra space should be added inside XML tags.
        /// </summary>
        public bool CommentXmlSpaceTags
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        public bool CommentXmlSplitAllTags
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if summary tags should always be split to multiple lines.
        /// </summary>
        public bool CommentXmlSplitSummaryTagToMultipleLines
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        public bool CommentXmlTagsToLowerCase
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the amount of extra spacing to add before XML values.
        /// </summary>
        public int CommentXmlValueIndent
        {
            get { return GetPropertyValue<int>(); }
            set
            {
                if (value >= 0)
                {
                    SetPropertyValue(value);
                }
            }
        }

        #endregion Options

        #region Preview Text and Helpers

        public string CommentPreviewText
        {
            get { return GetPropertyValue<string>(); }
            private set { SetPropertyValue(value); }
        }

        public FontFamily CommentPreviewTextFont => new FontFamily(_editorProperties.Item("FontFamily").Value.ToString());

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
            //TODO: Preview functionality used to work against CodeCommentOptions, but now is
            // working directly against the Settings object which is not updated until save.
            // Utilize an alternate Settings object to show settings on the fly?
            CommentPreviewText = CodeComment.FormatXml(UnformattedPreviewText);
        }

        #endregion Preview Text and Helpers
    }
}