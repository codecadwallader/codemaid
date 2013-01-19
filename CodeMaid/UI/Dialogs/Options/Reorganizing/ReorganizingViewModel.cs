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

using System.Collections.ObjectModel;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Reorganizing
{
    /// <summary>
    /// The view model for reorganizing options.
    /// </summary>
    public class ReorganizingViewModel : OptionsPageViewModel
    {
        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header
        {
            get { return "Reorganizing"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            AlphabetizeMembersOfTheSameGroup = Settings.Default.Reorganizing_AlphabetizeMembersOfTheSameGroup;
            KeepMembersWithinRegions = Settings.Default.Reorganizing_KeepMembersWithinRegions;
            RunAtStartOfCleanup = Settings.Default.Reorganizing_RunAtStartOfCleanup;
            SortOrderTypeFields = Settings.Default.Reorganizing_SortOrderTypeFields;
            SortOrderTypeConstructors = Settings.Default.Reorganizing_SortOrderTypeConstructors;
            SortOrderTypeDestructors = Settings.Default.Reorganizing_SortOrderTypeDestructors;
            SortOrderTypeDelegates = Settings.Default.Reorganizing_SortOrderTypeDelegates;
            SortOrderTypeEvents = Settings.Default.Reorganizing_SortOrderTypeEvents;
            SortOrderTypeEnums = Settings.Default.Reorganizing_SortOrderTypeEnums;
            SortOrderTypeInterfaces = Settings.Default.Reorganizing_SortOrderTypeInterfaces;
            SortOrderTypeProperties = Settings.Default.Reorganizing_SortOrderTypeProperties;
            SortOrderTypeIndexers = Settings.Default.Reorganizing_SortOrderTypeIndexers;
            SortOrderTypeMethods = Settings.Default.Reorganizing_SortOrderTypeMethods;
            SortOrderTypeStructs = Settings.Default.Reorganizing_SortOrderTypeStructs;
            SortOrderTypeClasses = Settings.Default.Reorganizing_SortOrderTypeClasses;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.Reorganizing_AlphabetizeMembersOfTheSameGroup = AlphabetizeMembersOfTheSameGroup;
            Settings.Default.Reorganizing_KeepMembersWithinRegions = KeepMembersWithinRegions;
            Settings.Default.Reorganizing_RunAtStartOfCleanup = RunAtStartOfCleanup;
            Settings.Default.Reorganizing_SortOrderTypeFields = SortOrderTypeFields;
            Settings.Default.Reorganizing_SortOrderTypeConstructors = SortOrderTypeConstructors;
            Settings.Default.Reorganizing_SortOrderTypeDestructors = SortOrderTypeDestructors;
            Settings.Default.Reorganizing_SortOrderTypeDelegates = SortOrderTypeDelegates;
            Settings.Default.Reorganizing_SortOrderTypeEvents = SortOrderTypeEvents;
            Settings.Default.Reorganizing_SortOrderTypeEnums = SortOrderTypeEnums;
            Settings.Default.Reorganizing_SortOrderTypeInterfaces = SortOrderTypeInterfaces;
            Settings.Default.Reorganizing_SortOrderTypeProperties = SortOrderTypeProperties;
            Settings.Default.Reorganizing_SortOrderTypeIndexers = SortOrderTypeIndexers;
            Settings.Default.Reorganizing_SortOrderTypeMethods = SortOrderTypeMethods;
            Settings.Default.Reorganizing_SortOrderTypeStructs = SortOrderTypeStructs;
            Settings.Default.Reorganizing_SortOrderTypeClasses = SortOrderTypeClasses;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        private bool _alphabetizeMembersOfTheSameGroup;

        /// <summary>
        /// Gets or sets the flag indicating if members of the same group should be alphabetized.
        /// </summary>
        public bool AlphabetizeMembersOfTheSameGroup
        {
            get { return _alphabetizeMembersOfTheSameGroup; }
            set
            {
                if (_alphabetizeMembersOfTheSameGroup != value)
                {
                    _alphabetizeMembersOfTheSameGroup = value;
                    NotifyPropertyChanged("AlphabetizeMembersOfTheSameGroup");
                }
            }
        }

        private bool _keepMembersWithinRegions;

        /// <summary>
        /// Gets or sets the flag indicating if members should be kept within regions.
        /// </summary>
        public bool KeepMembersWithinRegions
        {
            get { return _keepMembersWithinRegions; }
            set
            {
                if (_keepMembersWithinRegions != value)
                {
                    _keepMembersWithinRegions = value;
                    NotifyPropertyChanged("KeepMembersWithinRegions");
                }
            }
        }

        private bool _runAtStartOfCleanup;

        /// <summary>
        /// Gets or sets the flag indicating if reorganizing should be run at the start of cleanup.
        /// </summary>
        public bool RunAtStartOfCleanup
        {
            get { return _runAtStartOfCleanup; }
            set
            {
                if (_runAtStartOfCleanup != value)
                {
                    _runAtStartOfCleanup = value;
                    NotifyPropertyChanged("RunAtStartOfCleanup");
                }
            }
        }

        private int _sortOrderTypeFields;

        /// <summary>
        /// Gets or sets the sort order for field types.
        /// </summary>
        public int SortOrderTypeFields
        {
            get { return _sortOrderTypeFields; }
            set
            {
                if (_sortOrderTypeFields != value)
                {
                    _sortOrderTypeFields = value;
                    NotifyPropertyChanged("SortOrderTypeFields");
                }
            }
        }

        private int _sortOrderTypeConstructors;

        /// <summary>
        /// Gets or sets the sort order for constructor types.
        /// </summary>
        public int SortOrderTypeConstructors
        {
            get { return _sortOrderTypeConstructors; }
            set
            {
                if (_sortOrderTypeConstructors != value)
                {
                    _sortOrderTypeConstructors = value;
                    NotifyPropertyChanged("SortOrderTypeConstructors");
                }
            }
        }

        private int _sortOrderTypeDestructors;

        /// <summary>
        /// Gets or sets the sort order for destructor types.
        /// </summary>
        public int SortOrderTypeDestructors
        {
            get { return _sortOrderTypeDestructors; }
            set
            {
                if (_sortOrderTypeDestructors != value)
                {
                    _sortOrderTypeDestructors = value;
                    NotifyPropertyChanged("SortOrderTypeDestructors");
                }
            }
        }

        private int _sortOrderTypeDelegates;

        /// <summary>
        /// Gets or sets the sort order for delegate types.
        /// </summary>
        public int SortOrderTypeDelegates
        {
            get { return _sortOrderTypeDelegates; }
            set
            {
                if (_sortOrderTypeDelegates != value)
                {
                    _sortOrderTypeDelegates = value;
                    NotifyPropertyChanged("SortOrderTypeDelegates");
                }
            }
        }

        private int _sortOrderTypeEvents;

        /// <summary>
        /// Gets or sets the sort order for event types.
        /// </summary>
        public int SortOrderTypeEvents
        {
            get { return _sortOrderTypeEvents; }
            set
            {
                if (_sortOrderTypeEvents != value)
                {
                    _sortOrderTypeEvents = value;
                    NotifyPropertyChanged("SortOrderTypeEvents");
                }
            }
        }

        private int _sortOrderTypeEnums;

        /// <summary>
        /// Gets or sets the sort order for enum types.
        /// </summary>
        public int SortOrderTypeEnums
        {
            get { return _sortOrderTypeEnums; }
            set
            {
                if (_sortOrderTypeEnums != value)
                {
                    _sortOrderTypeEnums = value;
                    NotifyPropertyChanged("SortOrderTypeEnums");
                }
            }
        }

        private int _sortOrderTypeInterfaces;

        /// <summary>
        /// Gets or sets the sort order for interface types.
        /// </summary>
        public int SortOrderTypeInterfaces
        {
            get { return _sortOrderTypeInterfaces; }
            set
            {
                if (_sortOrderTypeInterfaces != value)
                {
                    _sortOrderTypeInterfaces = value;
                    NotifyPropertyChanged("SortOrderTypeInterfaces");
                }
            }
        }

        private int _sortOrderTypeProperties;

        /// <summary>
        /// Gets or sets the sort order for property types.
        /// </summary>
        public int SortOrderTypeProperties
        {
            get { return _sortOrderTypeProperties; }
            set
            {
                if (_sortOrderTypeProperties != value)
                {
                    _sortOrderTypeProperties = value;
                    NotifyPropertyChanged("SortOrderTypeProperties");
                }
            }
        }

        private int _sortOrderTypeIndexers;

        /// <summary>
        /// Gets or sets the sort order for indexer types.
        /// </summary>
        public int SortOrderTypeIndexers
        {
            get { return _sortOrderTypeIndexers; }
            set
            {
                if (_sortOrderTypeIndexers != value)
                {
                    _sortOrderTypeIndexers = value;
                    NotifyPropertyChanged("SortOrderTypeIndexers");
                }
            }
        }

        private int _sortOrderTypeMethods;

        /// <summary>
        /// Gets or sets the sort order for method types.
        /// </summary>
        public int SortOrderTypeMethods
        {
            get { return _sortOrderTypeMethods; }
            set
            {
                if (_sortOrderTypeMethods != value)
                {
                    _sortOrderTypeMethods = value;
                    NotifyPropertyChanged("SortOrderTypeMethods");
                }
            }
        }

        private int _sortOrderTypeStructs;

        /// <summary>
        /// Gets or sets the sort order for struct types.
        /// </summary>
        public int SortOrderTypeStructs
        {
            get { return _sortOrderTypeStructs; }
            set
            {
                if (_sortOrderTypeStructs != value)
                {
                    _sortOrderTypeStructs = value;
                    NotifyPropertyChanged("SortOrderTypeStructs");
                }
            }
        }

        private int _sortOrderTypeClasses;

        /// <summary>
        /// Gets or sets the sort order for class types.
        /// </summary>
        public int SortOrderTypeClasses
        {
            get { return _sortOrderTypeClasses; }
            set
            {
                if (_sortOrderTypeClasses != value)
                {
                    _sortOrderTypeClasses = value;
                    NotifyPropertyChanged("SortOrderTypeClasses");
                }
            }
        }

        #endregion Options

        #region Properties

        private readonly ObservableCollection<object> _sortOrderTypes = new ObservableCollection<object>(
            new[]
                {
                    "Fields", "Constructors", "Destructors", "Delegates", "Events", "Enums", "Interfaces", "Properties", "Indexers", "Methods", "Structs", "Classes"
                });

        public ObservableCollection<object> SortOrderTypes
        {
            get { return _sortOrderTypes; }
        }

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}