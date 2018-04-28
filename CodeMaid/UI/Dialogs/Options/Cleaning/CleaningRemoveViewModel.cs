using SteveCadwallader.CodeMaid.Properties;
using SteveCadwallader.CodeMaid.UI.Enumerations;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Cleaning
{
    /// <summary>
    /// The view model for cleaning remove options.
    /// </summary>
    public class CleaningRemoveViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleaningRemoveViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="activeSettings">The active settings.</param>
        public CleaningRemoveViewModel(CodeMaidPackage package, Settings activeSettings)
            : base(package, activeSettings)
        {
            Mappings = new SettingsToOptionsList(ActiveSettings, this)
            {
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_RemoveBlankLinesAfterAttributes, x => RemoveBlankLinesAfterAttributes),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_RemoveBlankLinesAfterOpeningBrace, x => RemoveBlankLinesAfterOpeningBraces),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_RemoveBlankLinesAtBottom, x => RemoveBlankLinesAtBottom),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_RemoveBlankLinesAtTop, x => RemoveBlankLinesAtTop),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_RemoveBlankLinesBeforeClosingBrace, x => RemoveBlankLinesBeforeClosingBraces),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_RemoveBlankLinesBeforeClosingTags, x => RemoveBlankLinesBeforeClosingTags),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_RemoveBlankLinesBetweenChainedStatements, x => RemoveBlankLinesBetweenChainedStatements),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_RemoveBlankSpacesBeforeClosingAngleBrackets, x => RemoveBlankSpacesBeforeClosingAngleBrackets),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_RemoveEndOfFileTrailingNewLine, x => RemoveEndOfFileTrailingNewLine),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_RemoveEndOfLineWhitespace, x => RemoveEndOfLineWhitespace),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_RemoveMultipleConsecutiveBlankLines, x => RemoveMultipleConsecutiveBlankLines),
                new SettingToOptionMapping<int, NoneEmptyAll>(x => ActiveSettings.Cleaning_RemoveRegions, x => RemoveRegions)
            };
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header => Resources.Remove;

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets the flag indicating if blank lines should be removed after attributes.
        /// </summary>
        public bool RemoveBlankLinesAfterAttributes
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank lines should be removed after an opening brace.
        /// </summary>
        public bool RemoveBlankLinesAfterOpeningBraces
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank lines should be removed at the bottom of the file.
        /// </summary>
        public bool RemoveBlankLinesAtBottom
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank lines should be removed at the top of the file.
        /// </summary>
        public bool RemoveBlankLinesAtTop
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank lines should be removed before a closing brace.
        /// </summary>
        public bool RemoveBlankLinesBeforeClosingBraces
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank lines should be removed before a closing tag.
        /// </summary>
        public bool RemoveBlankLinesBeforeClosingTags
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank lines should be removed between chained statements.
        /// </summary>
        public bool RemoveBlankLinesBetweenChainedStatements
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank spaces should be removed before a closing
        /// angle bracket.
        /// </summary>
        public bool RemoveBlankSpacesBeforeClosingAngleBrackets
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if the end of file trailing newline should be removed.
        /// </summary>
        public bool RemoveEndOfFileTrailingNewLine
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if end of line whitespace should be removed.
        /// </summary>
        public bool RemoveEndOfLineWhitespace
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if multiple consecutive blank lines should be removed.
        /// </summary>
        public bool RemoveMultipleConsecutiveBlankLines
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the options for removing regions.
        /// </summary>
        public NoneEmptyAll RemoveRegions
        {
            get { return GetPropertyValue<NoneEmptyAll>(); }
            set { SetPropertyValue(value); }
        }

        #endregion Options
    }
}