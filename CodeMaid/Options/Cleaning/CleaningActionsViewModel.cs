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
    /// The view model for cleaning actions options.
    /// </summary>
    public class CleaningActionsViewModel : OptionsPageViewModel
    {
        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header
        {
            get { return "Actions"; }
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
            InsertBlankLinePaddingBeforeStructs = Settings.Default.Cleaning_InsertBlankLinePaddingBeforeStructs;
            InsertBlankLinePaddingBeforeUsingStatementBlocks = Settings.Default.Cleaning_InsertBlankLinePaddingBeforeUsingStatementBlocks;
            InsertBlankLinePaddingBetweenPropertiesMultiLineAccessors = Settings.Default.Cleaning_InsertBlankLinePaddingBetweenPropertiesMultiLineAccessors;
            InsertExplicitAccessModifiersOnClasses = Settings.Default.Cleaning_InsertExplicitAccessModifiersOnClasses;
            InsertExplicitAccessModifiersOnEnumerations = Settings.Default.Cleaning_InsertExplicitAccessModifiersOnEnumerations;
            InsertExplicitAccessModifiersOnEvents = Settings.Default.Cleaning_InsertExplicitAccessModifiersOnEvents;
            InsertExplicitAccessModifiersOnFields = Settings.Default.Cleaning_InsertExplicitAccessModifiersOnFields;
            InsertExplicitAccessModifiersOnInterfaces = Settings.Default.Cleaning_InsertExplicitAccessModifiersOnInterfaces;
            InsertExplicitAccessModifiersOnMethods = Settings.Default.Cleaning_InsertExplicitAccessModifiersOnMethods;
            InsertExplicitAccessModifiersOnProperties = Settings.Default.Cleaning_InsertExplicitAccessModifiersOnProperties;
            InsertExplicitAccessModifiersOnStructs = Settings.Default.Cleaning_InsertExplicitAccessModifiersOnStructs;
            RemoveBlankLinesAfterOpeningBrace = Settings.Default.Cleaning_RemoveBlankLinesAfterOpeningBrace;
            RemoveBlankLinesAtBottom = Settings.Default.Cleaning_RemoveBlankLinesAtBottom;
            RemoveBlankLinesAtTop = Settings.Default.Cleaning_RemoveBlankLinesAtTop;
            RemoveBlankLinesBeforeClosingBrace = Settings.Default.Cleaning_RemoveBlankLinesBeforeClosingBrace;
            RemoveEndOfLineWhitespace = Settings.Default.Cleaning_RemoveEndOfLineWhitespace;
            RemoveMultipleConsecutiveBlankLines = Settings.Default.Cleaning_RemoveMultipleConsecutiveBlankLines;
            RunVisualStudioFormatDocument = Settings.Default.Cleaning_RunVisualStudioFormatDocumentCommand;
            RunVisualStudioRemoveUnusedUsingStatements = Settings.Default.Cleaning_RunVisualStudioRemoveUnusedUsingStatements;
            RunVisualStudioSortUsingStatements = Settings.Default.Cleaning_RunVisualStudioSortUsingStatements;
            SkipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave = Settings.Default.Cleaning_SkipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave;
            UpdateEndRegionDirectives = Settings.Default.Cleaning_UpdateEndRegionDirectives;
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
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforeStructs = InsertBlankLinePaddingBeforeStructs;
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforeUsingStatementBlocks = InsertBlankLinePaddingBeforeUsingStatementBlocks;
            Settings.Default.Cleaning_InsertBlankLinePaddingBetweenPropertiesMultiLineAccessors = InsertBlankLinePaddingBetweenPropertiesMultiLineAccessors;
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnClasses = InsertExplicitAccessModifiersOnClasses;
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnEnumerations = InsertExplicitAccessModifiersOnEnumerations;
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnEvents = InsertExplicitAccessModifiersOnEvents;
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnFields = InsertExplicitAccessModifiersOnFields;
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnInterfaces = InsertExplicitAccessModifiersOnInterfaces;
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnMethods = InsertExplicitAccessModifiersOnMethods;
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnProperties = InsertExplicitAccessModifiersOnProperties;
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnStructs = InsertExplicitAccessModifiersOnStructs;
            Settings.Default.Cleaning_RemoveBlankLinesAfterOpeningBrace = RemoveBlankLinesAfterOpeningBrace;
            Settings.Default.Cleaning_RemoveBlankLinesAtBottom = RemoveBlankLinesAtBottom;
            Settings.Default.Cleaning_RemoveBlankLinesAtTop = RemoveBlankLinesAtTop;
            Settings.Default.Cleaning_RemoveBlankLinesBeforeClosingBrace = RemoveBlankLinesBeforeClosingBrace;
            Settings.Default.Cleaning_RemoveEndOfLineWhitespace = RemoveEndOfLineWhitespace;
            Settings.Default.Cleaning_RemoveMultipleConsecutiveBlankLines = RemoveMultipleConsecutiveBlankLines;
            Settings.Default.Cleaning_RunVisualStudioFormatDocumentCommand = RunVisualStudioFormatDocument;
            Settings.Default.Cleaning_RunVisualStudioRemoveUnusedUsingStatements = RunVisualStudioRemoveUnusedUsingStatements;
            Settings.Default.Cleaning_RunVisualStudioSortUsingStatements = RunVisualStudioSortUsingStatements;
            Settings.Default.Cleaning_SkipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave = SkipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave;
            Settings.Default.Cleaning_UpdateEndRegionDirectives = UpdateEndRegionDirectives;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        private bool _insertBlankLinePaddingAfterClasses;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after classes.
        /// </summary>
        public bool InsertBlankLinePaddingAfterClasses
        {
            get { return _insertBlankLinePaddingAfterClasses; }
            set
            {
                if (_insertBlankLinePaddingAfterClasses != value)
                {
                    _insertBlankLinePaddingAfterClasses = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingAfterClasses");
                }
            }
        }

        private bool _insertBlankLinePaddingAfterDelegates;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after delegates.
        /// </summary>
        public bool InsertBlankLinePaddingAfterDelegates
        {
            get { return _insertBlankLinePaddingAfterDelegates; }
            set
            {
                if (_insertBlankLinePaddingAfterDelegates != value)
                {
                    _insertBlankLinePaddingAfterDelegates = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingAfterDelegates");
                }
            }
        }

        private bool _insertBlankLinePaddingAfterEndRegionTags;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after end region tags.
        /// </summary>
        public bool InsertBlankLinePaddingAfterEndRegionTags
        {
            get { return _insertBlankLinePaddingAfterEndRegionTags; }
            set
            {
                if (_insertBlankLinePaddingAfterEndRegionTags != value)
                {
                    _insertBlankLinePaddingAfterEndRegionTags = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingAfterEndRegionTags");
                }
            }
        }

        private bool _insertBlankLinePaddingAfterEnumerations;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after enumerations.
        /// </summary>
        public bool InsertBlankLinePaddingAfterEnumerations
        {
            get { return _insertBlankLinePaddingAfterEnumerations; }
            set
            {
                if (_insertBlankLinePaddingAfterEnumerations != value)
                {
                    _insertBlankLinePaddingAfterEnumerations = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingAfterEnumerations");
                }
            }
        }

        private bool _insertBlankLinePaddingAfterEvents;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after events.
        /// </summary>
        public bool InsertBlankLinePaddingAfterEvents
        {
            get { return _insertBlankLinePaddingAfterEvents; }
            set
            {
                if (_insertBlankLinePaddingAfterEvents != value)
                {
                    _insertBlankLinePaddingAfterEvents = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingAfterEvents");
                }
            }
        }

        private bool _insertBlankLinePaddingAfterFieldsMultiLine;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after multi-line fields.
        /// </summary>
        public bool InsertBlankLinePaddingAfterFieldsMultiLine
        {
            get { return _insertBlankLinePaddingAfterFieldsMultiLine; }
            set
            {
                if (_insertBlankLinePaddingAfterFieldsMultiLine != value)
                {
                    _insertBlankLinePaddingAfterFieldsMultiLine = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingAfterFieldsMultiLine");
                }
            }
        }

        private bool _insertBlankLinePaddingAfterInterfaces;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after interfaces.
        /// </summary>
        public bool InsertBlankLinePaddingAfterInterfaces
        {
            get { return _insertBlankLinePaddingAfterInterfaces; }
            set
            {
                if (_insertBlankLinePaddingAfterInterfaces != value)
                {
                    _insertBlankLinePaddingAfterInterfaces = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingAfterInterfaces");
                }
            }
        }

        private bool _insertBlankLinePaddingAfterMethods;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after methods.
        /// </summary>
        public bool InsertBlankLinePaddingAfterMethods
        {
            get { return _insertBlankLinePaddingAfterMethods; }
            set
            {
                if (_insertBlankLinePaddingAfterMethods != value)
                {
                    _insertBlankLinePaddingAfterMethods = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingAfterMethods");
                }
            }
        }

        private bool _insertBlankLinePaddingAfterNamespaces;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after namespaces.
        /// </summary>
        public bool InsertBlankLinePaddingAfterNamespaces
        {
            get { return _insertBlankLinePaddingAfterNamespaces; }
            set
            {
                if (_insertBlankLinePaddingAfterNamespaces != value)
                {
                    _insertBlankLinePaddingAfterNamespaces = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingAfterNamespaces");
                }
            }
        }

        private bool _insertBlankLinePaddingAfterProperties;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after properties.
        /// </summary>
        public bool InsertBlankLinePaddingAfterProperties
        {
            get { return _insertBlankLinePaddingAfterProperties; }
            set
            {
                if (_insertBlankLinePaddingAfterProperties != value)
                {
                    _insertBlankLinePaddingAfterProperties = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingAfterProperties");
                }
            }
        }

        private bool _insertBlankLinePaddingAfterRegionTags;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after region tags.
        /// </summary>
        public bool InsertBlankLinePaddingAfterRegionTags
        {
            get { return _insertBlankLinePaddingAfterRegionTags; }
            set
            {
                if (_insertBlankLinePaddingAfterRegionTags != value)
                {
                    _insertBlankLinePaddingAfterRegionTags = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingAfterRegionTags");
                }
            }
        }

        private bool _insertBlankLinePaddingAfterStructs;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after structs.
        /// </summary>
        public bool InsertBlankLinePaddingAfterStructs
        {
            get { return _insertBlankLinePaddingAfterStructs; }
            set
            {
                if (_insertBlankLinePaddingAfterStructs != value)
                {
                    _insertBlankLinePaddingAfterStructs = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingAfterStructs");
                }
            }
        }

        private bool _insertBlankLinePaddingAfterUsingStatementBlocks;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added after using statement blocks.
        /// </summary>
        public bool InsertBlankLinePaddingAfterUsingStatementBlocks
        {
            get { return _insertBlankLinePaddingAfterUsingStatementBlocks; }
            set
            {
                if (_insertBlankLinePaddingAfterUsingStatementBlocks != value)
                {
                    _insertBlankLinePaddingAfterUsingStatementBlocks = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingAfterUsingStatementBlocks");
                }
            }
        }

        private bool _insertBlankLinePaddingBeforeClasses;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before classes.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeClasses
        {
            get { return _insertBlankLinePaddingBeforeClasses; }
            set
            {
                if (_insertBlankLinePaddingBeforeClasses != value)
                {
                    _insertBlankLinePaddingBeforeClasses = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingBeforeClasses");
                }
            }
        }

        private bool _insertBlankLinePaddingBeforeDelegates;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before delegates.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeDelegates
        {
            get { return _insertBlankLinePaddingBeforeDelegates; }
            set
            {
                if (_insertBlankLinePaddingBeforeDelegates != value)
                {
                    _insertBlankLinePaddingBeforeDelegates = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingBeforeDelegates");
                }
            }
        }

        private bool _insertBlankLinePaddingBeforeEndRegionTags;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before end region tags.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeEndRegionTags
        {
            get { return _insertBlankLinePaddingBeforeEndRegionTags; }
            set
            {
                if (_insertBlankLinePaddingBeforeEndRegionTags != value)
                {
                    _insertBlankLinePaddingBeforeEndRegionTags = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingBeforeEndRegionTags");
                }
            }
        }

        private bool _insertBlankLinePaddingBeforeEnumerations;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before enumerations.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeEnumerations
        {
            get { return _insertBlankLinePaddingBeforeEnumerations; }
            set
            {
                if (_insertBlankLinePaddingBeforeEnumerations != value)
                {
                    _insertBlankLinePaddingBeforeEnumerations = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingBeforeEnumerations");
                }
            }
        }

        private bool _insertBlankLinePaddingBeforeEvents;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before events.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeEvents
        {
            get { return _insertBlankLinePaddingBeforeEvents; }
            set
            {
                if (_insertBlankLinePaddingBeforeEvents != value)
                {
                    _insertBlankLinePaddingBeforeEvents = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingBeforeEvents");
                }
            }
        }

        private bool _insertBlankLinePaddingBeforeFieldsMultiLine;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before multi-line fields.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeFieldsMultiLine
        {
            get { return _insertBlankLinePaddingBeforeFieldsMultiLine; }
            set
            {
                if (_insertBlankLinePaddingBeforeFieldsMultiLine != value)
                {
                    _insertBlankLinePaddingBeforeFieldsMultiLine = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingBeforeFieldsMultiLine");
                }
            }
        }

        private bool _insertBlankLinePaddingBeforeInterfaces;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before interfaces.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeInterfaces
        {
            get { return _insertBlankLinePaddingBeforeInterfaces; }
            set
            {
                if (_insertBlankLinePaddingBeforeInterfaces != value)
                {
                    _insertBlankLinePaddingBeforeInterfaces = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingBeforeInterfaces");
                }
            }
        }

        private bool _insertBlankLinePaddingBeforeMethods;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before methods.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeMethods
        {
            get { return _insertBlankLinePaddingBeforeMethods; }
            set
            {
                if (_insertBlankLinePaddingBeforeMethods != value)
                {
                    _insertBlankLinePaddingBeforeMethods = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingBeforeMethods");
                }
            }
        }

        private bool _insertBlankLinePaddingBeforeNamespaces;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before namespaces.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeNamespaces
        {
            get { return _insertBlankLinePaddingBeforeNamespaces; }
            set
            {
                if (_insertBlankLinePaddingBeforeNamespaces != value)
                {
                    _insertBlankLinePaddingBeforeNamespaces = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingBeforeNamespaces");
                }
            }
        }

        private bool _insertBlankLinePaddingBeforeProperties;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before properties.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeProperties
        {
            get { return _insertBlankLinePaddingBeforeProperties; }
            set
            {
                if (_insertBlankLinePaddingBeforeProperties != value)
                {
                    _insertBlankLinePaddingBeforeProperties = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingBeforeProperties");
                }
            }
        }

        private bool _insertBlankLinePaddingBeforeRegionTags;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before region tags.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeRegionTags
        {
            get { return _insertBlankLinePaddingBeforeRegionTags; }
            set
            {
                if (_insertBlankLinePaddingBeforeRegionTags != value)
                {
                    _insertBlankLinePaddingBeforeRegionTags = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingBeforeRegionTags");
                }
            }
        }

        private bool _insertBlankLinePaddingBeforeStructs;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before structs.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeStructs
        {
            get { return _insertBlankLinePaddingBeforeStructs; }
            set
            {
                if (_insertBlankLinePaddingBeforeStructs != value)
                {
                    _insertBlankLinePaddingBeforeStructs = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingBeforeStructs");
                }
            }
        }

        private bool _insertBlankLinePaddingBeforeUsingStatementBlocks;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added before using statement blocks.
        /// </summary>
        public bool InsertBlankLinePaddingBeforeUsingStatementBlocks
        {
            get { return _insertBlankLinePaddingBeforeUsingStatementBlocks; }
            set
            {
                if (_insertBlankLinePaddingBeforeUsingStatementBlocks != value)
                {
                    _insertBlankLinePaddingBeforeUsingStatementBlocks = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingBeforeUsingStatementBlocks");
                }
            }
        }

        private bool _insertBlankLinePaddingBetweenPropertiesMultiLineAccessors;

        /// <summary>
        /// Gets or sets the flag indicating if blank line padding should be added between properties with multi-line accessors.
        /// </summary>
        public bool InsertBlankLinePaddingBetweenPropertiesMultiLineAccessors
        {
            get { return _insertBlankLinePaddingBetweenPropertiesMultiLineAccessors; }
            set
            {
                if (_insertBlankLinePaddingBetweenPropertiesMultiLineAccessors != value)
                {
                    _insertBlankLinePaddingBetweenPropertiesMultiLineAccessors = value;
                    NotifyPropertyChanged("InsertBlankLinePaddingBetweenPropertiesMultiLineAccessors");
                }
            }
        }

        private bool _insertExplicitAccessModifiersOnClasses;

        /// <summary>
        /// Gets or sets the flag indicating if explicit access modifiers should be added on classes.
        /// </summary>
        public bool InsertExplicitAccessModifiersOnClasses
        {
            get { return _insertExplicitAccessModifiersOnClasses; }
            set
            {
                if (_insertExplicitAccessModifiersOnClasses != value)
                {
                    _insertExplicitAccessModifiersOnClasses = value;
                    NotifyPropertyChanged("InsertExplicitAccessModifiersOnClasses");
                }
            }
        }

        private bool _insertExplicitAccessModifiersOnEnumerations;

        /// <summary>
        /// Gets or sets the flag indicating if explicit access modifiers should be added on enumerations.
        /// </summary>
        public bool InsertExplicitAccessModifiersOnEnumerations
        {
            get { return _insertExplicitAccessModifiersOnEnumerations; }
            set
            {
                if (_insertExplicitAccessModifiersOnEnumerations != value)
                {
                    _insertExplicitAccessModifiersOnEnumerations = value;
                    NotifyPropertyChanged("InsertExplicitAccessModifiersOnEnumerations");
                }
            }
        }

        private bool _insertExplicitAccessModifiersOnEvents;

        /// <summary>
        /// Gets or sets the flag indicating if explicit access modifiers should be added on events.
        /// </summary>
        public bool InsertExplicitAccessModifiersOnEvents
        {
            get { return _insertExplicitAccessModifiersOnEvents; }
            set
            {
                if (_insertExplicitAccessModifiersOnEvents != value)
                {
                    _insertExplicitAccessModifiersOnEvents = value;
                    NotifyPropertyChanged("InsertExplicitAccessModifiersOnEvents");
                }
            }
        }

        private bool _insertExplicitAccessModifiersOnFields;

        /// <summary>
        /// Gets or sets the flag indicating if explicit access modifiers should be added on fields.
        /// </summary>
        public bool InsertExplicitAccessModifiersOnFields
        {
            get { return _insertExplicitAccessModifiersOnFields; }
            set
            {
                if (_insertExplicitAccessModifiersOnFields != value)
                {
                    _insertExplicitAccessModifiersOnFields = value;
                    NotifyPropertyChanged("InsertExplicitAccessModifiersOnFields");
                }
            }
        }

        private bool _insertExplicitAccessModifiersOnInterfaces;

        /// <summary>
        /// Gets or sets the flag indicating if explicit access modifiers should be added on interfaces.
        /// </summary>
        public bool InsertExplicitAccessModifiersOnInterfaces
        {
            get { return _insertExplicitAccessModifiersOnInterfaces; }
            set
            {
                if (_insertExplicitAccessModifiersOnInterfaces != value)
                {
                    _insertExplicitAccessModifiersOnInterfaces = value;
                    NotifyPropertyChanged("InsertExplicitAccessModifiersOnInterfaces");
                }
            }
        }

        private bool _insertExplicitAccessModifiersOnMethods;

        /// <summary>
        /// Gets or sets the flag indicating if explicit access modifiers should be added on methods.
        /// </summary>
        public bool InsertExplicitAccessModifiersOnMethods
        {
            get { return _insertExplicitAccessModifiersOnMethods; }
            set
            {
                if (_insertExplicitAccessModifiersOnMethods != value)
                {
                    _insertExplicitAccessModifiersOnMethods = value;
                    NotifyPropertyChanged("InsertExplicitAccessModifiersOnMethods");
                }
            }
        }

        private bool _insertExplicitAccessModifiersOnProperties;

        /// <summary>
        /// Gets or sets the flag indicating if explicit access modifiers should be added on properties.
        /// </summary>
        public bool InsertExplicitAccessModifiersOnProperties
        {
            get { return _insertExplicitAccessModifiersOnProperties; }
            set
            {
                if (_insertExplicitAccessModifiersOnProperties != value)
                {
                    _insertExplicitAccessModifiersOnProperties = value;
                    NotifyPropertyChanged("InsertExplicitAccessModifiersOnProperties");
                }
            }
        }

        private bool _insertExplicitAccessModifiersOnStructs;

        /// <summary>
        /// Gets or sets the flag indicating if explicit access modifiers should be added on structs.
        /// </summary>
        public bool InsertExplicitAccessModifiersOnStructs
        {
            get { return _insertExplicitAccessModifiersOnStructs; }
            set
            {
                if (_insertExplicitAccessModifiersOnStructs != value)
                {
                    _insertExplicitAccessModifiersOnStructs = value;
                    NotifyPropertyChanged("InsertExplicitAccessModifiersOnStructs");
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

        private bool _runVisualStudioFormatDocument;

        /// <summary>
        /// Gets or sets the flag indicating if the Visual Studio format document command should be run.
        /// </summary>
        public bool RunVisualStudioFormatDocument
        {
            get { return _runVisualStudioFormatDocument; }
            set
            {
                if (_runVisualStudioFormatDocument != value)
                {
                    _runVisualStudioFormatDocument = value;
                    NotifyPropertyChanged("RunVisualStudioFormatDocument");
                }
            }
        }

        private bool _runVisualStudioRemoveUnusedUsingStatements;

        /// <summary>
        /// Gets or sets the flag indicating if unused using statements should be removed.
        /// </summary>
        public bool RunVisualStudioRemoveUnusedUsingStatements
        {
            get { return _runVisualStudioRemoveUnusedUsingStatements; }
            set
            {
                if (_runVisualStudioRemoveUnusedUsingStatements != value)
                {
                    _runVisualStudioRemoveUnusedUsingStatements = value;
                    NotifyPropertyChanged("RunVisualStudioRemoveUnusedUsingStatements");
                }
            }
        }

        private bool _runVisualStudioSortUsingStatements;

        /// <summary>
        /// Gets or sets the flag indicating if using statements should be sorted.
        /// </summary>
        public bool RunVisualStudioSortUsingStatements
        {
            get { return _runVisualStudioSortUsingStatements; }
            set
            {
                if (_runVisualStudioSortUsingStatements != value)
                {
                    _runVisualStudioSortUsingStatements = value;
                    NotifyPropertyChanged("RunVisualStudioSortUsingStatements");
                }
            }
        }

        private bool _skipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave;

        /// <summary>
        /// Gets or sets the flag indicating if unused using statements should be removed except during auto cleanup on save.
        /// </summary>
        public bool SkipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave
        {
            get { return _skipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave; }
            set
            {
                if (_skipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave != value)
                {
                    _skipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave = value;
                    NotifyPropertyChanged("SkipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave");
                }
            }
        }

        private bool _updateEndRegionDirectives;

        /// <summary>
        /// Gets or sets the flag indicating if end region directives should be updated.
        /// </summary>
        public bool UpdateEndRegionDirectives
        {
            get { return _updateEndRegionDirectives; }
            set
            {
                if (_updateEndRegionDirectives != value)
                {
                    _updateEndRegionDirectives = value;
                    NotifyPropertyChanged("UpdateEndRegionDirectives");
                }
            }
        }

        #endregion Options
    }
}