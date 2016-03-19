using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Cleaning
{
    /// <summary>
    /// The view model for cleaning insert options.
    /// </summary>
    public class CleaningInsertViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleaningInsertViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="activeSettings">The active settings.</param>
        public CleaningInsertViewModel(CodeMaidPackage package, Settings activeSettings)
            : base(package, activeSettings)
        {
            Mappings = new SettingsToOptionsList(ActiveSettings, this)
            {
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingAfterClasses, x => InsertBlankLinePaddingAfterClasses),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingAfterDelegates, x => InsertBlankLinePaddingAfterDelegates),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingAfterEndRegionTags, x => InsertBlankLinePaddingAfterEndRegionTags),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingAfterEnumerations, x => InsertBlankLinePaddingAfterEnumerations),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingAfterEvents, x => InsertBlankLinePaddingAfterEvents),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingAfterFieldsMultiLine, x => InsertBlankLinePaddingAfterFieldsMultiLine),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingAfterInterfaces, x => InsertBlankLinePaddingAfterInterfaces),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingAfterMethods, x => InsertBlankLinePaddingAfterMethods),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingAfterNamespaces, x => InsertBlankLinePaddingAfterNamespaces),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingAfterPropertiesMultiLine, x => InsertBlankLinePaddingAfterPropertiesMultiLine),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingAfterPropertiesSingleLine, x => InsertBlankLinePaddingAfterPropertiesSingleLine),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingAfterRegionTags, x => InsertBlankLinePaddingAfterRegionTags),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingAfterStructs, x => InsertBlankLinePaddingAfterStructs),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingAfterUsingStatementBlocks, x => InsertBlankLinePaddingAfterUsingStatementBlocks),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingBeforeCaseStatements, x => InsertBlankLinePaddingBeforeCaseStatements),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingBeforeClasses, x => InsertBlankLinePaddingBeforeClasses),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingBeforeDelegates, x => InsertBlankLinePaddingBeforeDelegates),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingBeforeEndRegionTags, x => InsertBlankLinePaddingBeforeEndRegionTags),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingBeforeEnumerations, x => InsertBlankLinePaddingBeforeEnumerations),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingBeforeEvents, x => InsertBlankLinePaddingBeforeEvents),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingBeforeFieldsMultiLine, x => InsertBlankLinePaddingBeforeFieldsMultiLine),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingBeforeInterfaces, x => InsertBlankLinePaddingBeforeInterfaces),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingBeforeMethods, x => InsertBlankLinePaddingBeforeMethods),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingBeforeNamespaces, x => InsertBlankLinePaddingBeforeNamespaces),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingBeforePropertiesMultiLine, x => InsertBlankLinePaddingBeforePropertiesMultiLine),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingBeforePropertiesSingleLine, x => InsertBlankLinePaddingBeforePropertiesSingleLine),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingBeforeRegionTags, x => InsertBlankLinePaddingBeforeRegionTags),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingBeforeSingleLineComments, x => InsertBlankLinePaddingBeforeSingleLineComments),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingBeforeStructs, x => InsertBlankLinePaddingBeforeStructs),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingBeforeUsingStatementBlocks, x => InsertBlankLinePaddingBeforeUsingStatementBlocks),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankLinePaddingBetweenPropertiesMultiLineAccessors, x => InsertBlankLinePaddingBetweenPropertiesMultiLineAccessors),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertBlankSpaceBeforeSelfClosingAngleBrackets, x => InsertBlankSpaceBeforeSelfClosingAngleBrackets),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertEndOfFileTrailingNewLine, x => InsertEndOfFileTrailingNewLine),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertExplicitAccessModifiersOnClasses, x => InsertExplicitAccessModifiersOnClasses),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertExplicitAccessModifiersOnDelegates, x => InsertExplicitAccessModifiersOnDelegates),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertExplicitAccessModifiersOnEnumerations, x => InsertExplicitAccessModifiersOnEnumerations),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertExplicitAccessModifiersOnEvents, x => InsertExplicitAccessModifiersOnEvents),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertExplicitAccessModifiersOnFields, x => InsertExplicitAccessModifiersOnFields),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertExplicitAccessModifiersOnInterfaces, x => InsertExplicitAccessModifiersOnInterfaces),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertExplicitAccessModifiersOnMethods, x => InsertExplicitAccessModifiersOnMethods),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertExplicitAccessModifiersOnProperties, x => InsertExplicitAccessModifiersOnProperties),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_InsertExplicitAccessModifiersOnStructs, x => InsertExplicitAccessModifiersOnStructs)
            };
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header => "Insert";

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after classes.
        /// </summary>
        public bool InsertBlankLinePaddingAfterClasses
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after delegates.
        /// </summary>
        public bool InsertBlankLinePaddingAfterDelegates
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after end region tags.
        /// </summary>
        public bool InsertBlankLinePaddingAfterEndRegionTags
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after enumerations.
        /// </summary>
        public bool InsertBlankLinePaddingAfterEnumerations
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after events.
        /// </summary>
        public bool InsertBlankLinePaddingAfterEvents
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after multi-line fields.
        /// </summary>
        public bool InsertBlankLinePaddingAfterFieldsMultiLine
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after interfaces.
        /// </summary>
        public bool InsertBlankLinePaddingAfterInterfaces
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after methods.
        /// </summary>
        public bool InsertBlankLinePaddingAfterMethods
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after namespaces.
        /// </summary>
        public bool InsertBlankLinePaddingAfterNamespaces
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after multi-line properties.
        /// </summary>
        public bool InsertBlankLinePaddingAfterPropertiesMultiLine
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after single-line properties.
        /// </summary>
        public bool InsertBlankLinePaddingAfterPropertiesSingleLine
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after region tags.
        /// </summary>
        public bool InsertBlankLinePaddingAfterRegionTags
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after structs.
        /// </summary>
        public bool InsertBlankLinePaddingAfterStructs
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after using
        /// statement blocks.
        /// </summary>
        public bool InsertBlankLinePaddingAfterUsingStatementBlocks
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before case statements.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeCaseStatements
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before classes.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeClasses
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before delegates.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeDelegates
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before end region tags.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeEndRegionTags
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before enumerations.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeEnumerations
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before events.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeEvents
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before multi-line fields.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeFieldsMultiLine
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before interfaces.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeInterfaces
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before methods.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeMethods
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before namespaces.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeNamespaces
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before multi-line properties.
        /// </summary>
        public bool InsertBlankLinePaddingBeforePropertiesMultiLine
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before single-line properties.
        /// </summary>
        public bool InsertBlankLinePaddingBeforePropertiesSingleLine
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before region tags.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeRegionTags
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before single
        /// line comments.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeSingleLineComments
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before structs.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeStructs
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before using
        /// statement blocks.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeUsingStatementBlocks
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added between
        /// properties with multi-line accessors.
        /// </summary>
        public bool InsertBlankLinePaddingBetweenPropertiesMultiLineAccessors
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if a blank space should be added before self closing
        /// angle brackets.
        /// </summary>
        public bool InsertBlankSpaceBeforeSelfClosingAngleBrackets
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if an end of file trailing newline should be inserted.
        /// </summary>
        public bool InsertEndOfFileTrailingNewLine
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if explicit access modifiers should be added on classes.
        /// </summary>
        public bool InsertExplicitAccessModifiersOnClasses
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if explicit access modifiers should be added on delegates.
        /// </summary>
        public bool InsertExplicitAccessModifiersOnDelegates
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if explicit access modifiers should be added on enumerations.
        /// </summary>
        public bool InsertExplicitAccessModifiersOnEnumerations
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if explicit access modifiers should be added on events.
        /// </summary>
        public bool InsertExplicitAccessModifiersOnEvents
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if explicit access modifiers should be added on fields.
        /// </summary>
        public bool InsertExplicitAccessModifiersOnFields
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if explicit access modifiers should be added on interfaces.
        /// </summary>
        public bool InsertExplicitAccessModifiersOnInterfaces
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if explicit access modifiers should be added on methods.
        /// </summary>
        public bool InsertExplicitAccessModifiersOnMethods
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if explicit access modifiers should be added on properties.
        /// </summary>
        public bool InsertExplicitAccessModifiersOnProperties
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if explicit access modifiers should be added on structs.
        /// </summary>
        public bool InsertExplicitAccessModifiersOnStructs
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        #endregion Options
    }
}