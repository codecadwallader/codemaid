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
    /// The view model for cleaning insert options.
    /// </summary>
    public class CleaningInsertViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleaningInsertViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        public CleaningInsertViewModel(CodeMaidPackage package)
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
            get { return "Insert"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            InsertBlankLinePaddingAfterClasses = Settings.Default.Cleaning_InsertBlankLinePaddingAfterClasses;
            InsertBlankLinePaddingAfterDelegates = Settings.Default.Cleaning_InsertBlankLinePaddingAfterDelegates;
            InsertBlankLinePaddingAfterEndRegionTags = Settings.Default.Cleaning_InsertBlankLinePaddingAfterEndRegionTags;
            InsertBlankLinePaddingAfterEnumerations = Settings.Default.Cleaning_InsertBlankLinePaddingAfterEnumerations;
            InsertBlankLinePaddingAfterEvents = Settings.Default.Cleaning_InsertBlankLinePaddingAfterEvents;
            InsertBlankLinePaddingAfterFieldsMultiLine = Settings.Default.Cleaning_InsertBlankLinePaddingAfterFieldsMultiLine;
            InsertBlankLinePaddingAfterInterfaces = Settings.Default.Cleaning_InsertBlankLinePaddingAfterInterfaces;
            InsertBlankLinePaddingAfterMethods = Settings.Default.Cleaning_InsertBlankLinePaddingAfterMethods;
            InsertBlankLinePaddingAfterNamespaces = Settings.Default.Cleaning_InsertBlankLinePaddingAfterNamespaces;
            InsertBlankLinePaddingAfterProperties = Settings.Default.Cleaning_InsertBlankLinePaddingAfterProperties;
            InsertBlankLinePaddingAfterRegionTags = Settings.Default.Cleaning_InsertBlankLinePaddingAfterRegionTags;
            InsertBlankLinePaddingAfterStructs = Settings.Default.Cleaning_InsertBlankLinePaddingAfterStructs;
            InsertBlankLinePaddingAfterUsingStatementBlocks = Settings.Default.Cleaning_InsertBlankLinePaddingAfterUsingStatementBlocks;
            InsertBlankLinePaddingBeforeCaseStatements = Settings.Default.Cleaning_InsertBlankLinePaddingBeforeCaseStatements;
            InsertBlankLinePaddingBeforeClasses = Settings.Default.Cleaning_InsertBlankLinePaddingBeforeClasses;
            InsertBlankLinePaddingBeforeDelegates = Settings.Default.Cleaning_InsertBlankLinePaddingBeforeDelegates;
            InsertBlankLinePaddingBeforeEndRegionTags = Settings.Default.Cleaning_InsertBlankLinePaddingBeforeEndRegionTags;
            InsertBlankLinePaddingBeforeEnumerations = Settings.Default.Cleaning_InsertBlankLinePaddingBeforeEnumerations;
            InsertBlankLinePaddingBeforeEvents = Settings.Default.Cleaning_InsertBlankLinePaddingBeforeEvents;
            InsertBlankLinePaddingBeforeFieldsMultiLine = Settings.Default.Cleaning_InsertBlankLinePaddingBeforeFieldsMultiLine;
            InsertBlankLinePaddingBeforeInterfaces = Settings.Default.Cleaning_InsertBlankLinePaddingBeforeInterfaces;
            InsertBlankLinePaddingBeforeMethods = Settings.Default.Cleaning_InsertBlankLinePaddingBeforeMethods;
            InsertBlankLinePaddingBeforeNamespaces = Settings.Default.Cleaning_InsertBlankLinePaddingBeforeNamespaces;
            InsertBlankLinePaddingBeforeProperties = Settings.Default.Cleaning_InsertBlankLinePaddingBeforeProperties;
            InsertBlankLinePaddingBeforeRegionTags = Settings.Default.Cleaning_InsertBlankLinePaddingBeforeRegionTags;
            InsertBlankLinePaddingBeforeSingleLineComments = Settings.Default.Cleaning_InsertBlankLinePaddingBeforeSingleLineComments;
            InsertBlankLinePaddingBeforeStructs = Settings.Default.Cleaning_InsertBlankLinePaddingBeforeStructs;
            InsertBlankLinePaddingBeforeUsingStatementBlocks = Settings.Default.Cleaning_InsertBlankLinePaddingBeforeUsingStatementBlocks;
            InsertBlankLinePaddingBetweenPropertiesMultiLineAccessors = Settings.Default.Cleaning_InsertBlankLinePaddingBetweenPropertiesMultiLineAccessors;
            InsertBlankSpaceBeforeSelfClosingAngleBrackets = Settings.Default.Cleaning_InsertBlankSpaceBeforeSelfClosingAngleBrackets;
            InsertEndOfFileTrailingNewLine = Settings.Default.Cleaning_InsertEndOfFileTrailingNewLine;
            InsertExplicitAccessModifiersOnClasses = Settings.Default.Cleaning_InsertExplicitAccessModifiersOnClasses;
            InsertExplicitAccessModifiersOnDelegates = Settings.Default.Cleaning_InsertExplicitAccessModifiersOnDelegates;
            InsertExplicitAccessModifiersOnEnumerations = Settings.Default.Cleaning_InsertExplicitAccessModifiersOnEnumerations;
            InsertExplicitAccessModifiersOnEvents = Settings.Default.Cleaning_InsertExplicitAccessModifiersOnEvents;
            InsertExplicitAccessModifiersOnFields = Settings.Default.Cleaning_InsertExplicitAccessModifiersOnFields;
            InsertExplicitAccessModifiersOnInterfaces = Settings.Default.Cleaning_InsertExplicitAccessModifiersOnInterfaces;
            InsertExplicitAccessModifiersOnMethods = Settings.Default.Cleaning_InsertExplicitAccessModifiersOnMethods;
            InsertExplicitAccessModifiersOnProperties = Settings.Default.Cleaning_InsertExplicitAccessModifiersOnProperties;
            InsertExplicitAccessModifiersOnStructs = Settings.Default.Cleaning_InsertExplicitAccessModifiersOnStructs;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterClasses = InsertBlankLinePaddingAfterClasses;
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterDelegates = InsertBlankLinePaddingAfterDelegates;
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterEndRegionTags = InsertBlankLinePaddingAfterEndRegionTags;
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterEnumerations = InsertBlankLinePaddingAfterEnumerations;
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterEvents = InsertBlankLinePaddingAfterEvents;
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterFieldsMultiLine = InsertBlankLinePaddingAfterFieldsMultiLine;
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterInterfaces = InsertBlankLinePaddingAfterInterfaces;
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterMethods = InsertBlankLinePaddingAfterMethods;
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterNamespaces = InsertBlankLinePaddingAfterNamespaces;
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterProperties = InsertBlankLinePaddingAfterProperties;
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterRegionTags = InsertBlankLinePaddingAfterRegionTags;
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterStructs = InsertBlankLinePaddingAfterStructs;
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterUsingStatementBlocks = InsertBlankLinePaddingAfterUsingStatementBlocks;
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforeCaseStatements = InsertBlankLinePaddingBeforeCaseStatements;
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforeClasses = InsertBlankLinePaddingBeforeClasses;
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforeDelegates = InsertBlankLinePaddingBeforeDelegates;
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforeEndRegionTags = InsertBlankLinePaddingBeforeEndRegionTags;
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforeEnumerations = InsertBlankLinePaddingBeforeEnumerations;
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforeEvents = InsertBlankLinePaddingBeforeEvents;
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforeFieldsMultiLine = InsertBlankLinePaddingBeforeFieldsMultiLine;
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforeInterfaces = InsertBlankLinePaddingBeforeInterfaces;
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforeMethods = InsertBlankLinePaddingBeforeMethods;
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforeNamespaces = InsertBlankLinePaddingBeforeNamespaces;
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforeProperties = InsertBlankLinePaddingBeforeProperties;
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforeRegionTags = InsertBlankLinePaddingBeforeRegionTags;
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforeSingleLineComments = InsertBlankLinePaddingBeforeSingleLineComments;
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforeStructs = InsertBlankLinePaddingBeforeStructs;
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforeUsingStatementBlocks = InsertBlankLinePaddingBeforeUsingStatementBlocks;
            Settings.Default.Cleaning_InsertBlankLinePaddingBetweenPropertiesMultiLineAccessors = InsertBlankLinePaddingBetweenPropertiesMultiLineAccessors;
            Settings.Default.Cleaning_InsertBlankSpaceBeforeSelfClosingAngleBrackets = InsertBlankSpaceBeforeSelfClosingAngleBrackets;
            Settings.Default.Cleaning_InsertEndOfFileTrailingNewLine = InsertEndOfFileTrailingNewLine;
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnClasses = InsertExplicitAccessModifiersOnClasses;
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnDelegates = InsertExplicitAccessModifiersOnDelegates;
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnEnumerations = InsertExplicitAccessModifiersOnEnumerations;
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnEvents = InsertExplicitAccessModifiersOnEvents;
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnFields = InsertExplicitAccessModifiersOnFields;
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnInterfaces = InsertExplicitAccessModifiersOnInterfaces;
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnMethods = InsertExplicitAccessModifiersOnMethods;
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnProperties = InsertExplicitAccessModifiersOnProperties;
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnStructs = InsertExplicitAccessModifiersOnStructs;
        }

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
        /// Gets or sets the flag indicating if blank line padding should be added after properties.
        /// </summary>
        public bool InsertBlankLinePaddingAfterProperties
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
        /// Gets or sets the flag indicating if blank line padding should be added before properties.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeProperties
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