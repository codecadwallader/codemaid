#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Reorganizing
{
    /// <summary>
    /// The view model for reorganizing types options.
    /// </summary>
    public class ReorganizingTypesViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReorganizingTypesViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        public ReorganizingTypesViewModel(CodeMaidPackage package)
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
            get { return "Types"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            Classes = MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeClasses);
            Constructors = MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeConstructors);
            Delegates = MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeDelegates);
            Destructors = MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeDestructors);
            Enums = MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeEnums);
            Events = MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeEvents);
            Fields = MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeFields);
            Indexers = MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeIndexers);
            Interfaces = MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeInterfaces);
            Methods = MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeMethods);
            Properties = MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeProperties);
            Structs = MemberTypeSetting.Deserialize(Settings.Default.Reorganizing_MemberTypeStructs);

            CreateMemberTypesFromCurrentState();
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.Reorganizing_MemberTypeClasses = Classes.Serialize();
            Settings.Default.Reorganizing_MemberTypeConstructors = Constructors.Serialize();
            Settings.Default.Reorganizing_MemberTypeDelegates = Delegates.Serialize();
            Settings.Default.Reorganizing_MemberTypeDestructors = Destructors.Serialize();
            Settings.Default.Reorganizing_MemberTypeEnums = Enums.Serialize();
            Settings.Default.Reorganizing_MemberTypeEvents = Events.Serialize();
            Settings.Default.Reorganizing_MemberTypeFields = Fields.Serialize();
            Settings.Default.Reorganizing_MemberTypeIndexers = Indexers.Serialize();
            Settings.Default.Reorganizing_MemberTypeInterfaces = Interfaces.Serialize();
            Settings.Default.Reorganizing_MemberTypeMethods = Methods.Serialize();
            Settings.Default.Reorganizing_MemberTypeProperties = Properties.Serialize();
            Settings.Default.Reorganizing_MemberTypeStructs = Structs.Serialize();
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets the settings associated with classes.
        /// </summary>
        public MemberTypeSetting Classes { get; set; }

        /// <summary>
        /// Gets or sets the settings associated with constructors.
        /// </summary>
        public MemberTypeSetting Constructors { get; set; }

        /// <summary>
        /// Gets or sets the settings associated with delegates.
        /// </summary>
        public MemberTypeSetting Delegates { get; set; }

        /// <summary>
        /// Gets or sets the settings associated with destructors.
        /// </summary>
        public MemberTypeSetting Destructors { get; set; }

        /// <summary>
        /// Gets or sets the settings associated with enums.
        /// </summary>
        public MemberTypeSetting Enums { get; set; }

        /// <summary>
        /// Gets or sets the settings associated with events.
        /// </summary>
        public MemberTypeSetting Events { get; set; }

        /// <summary>
        /// Gets or sets the settings associated with fields.
        /// </summary>
        public MemberTypeSetting Fields { get; set; }

        /// <summary>
        /// Gets or sets the settings associated with indexers.
        /// </summary>
        public MemberTypeSetting Indexers { get; set; }

        /// <summary>
        /// Gets or sets the settings associated with interfaces.
        /// </summary>
        public MemberTypeSetting Interfaces { get; set; }

        /// <summary>
        /// Gets or sets the settings associated with methods.
        /// </summary>
        public MemberTypeSetting Methods { get; set; }

        /// <summary>
        /// Gets or sets the settings associated with properties.
        /// </summary>
        public MemberTypeSetting Properties { get; set; }

        /// <summary>
        /// Gets or sets the settings associated with structs.
        /// </summary>
        public MemberTypeSetting Structs { get; set; }

        #endregion Options

        #region Split Command

        private DelegateCommand _splitCommand;

        /// <summary>
        /// Gets the split command.
        /// </summary>
        public DelegateCommand SplitCommand
        {
            get { return _splitCommand ?? (_splitCommand = new DelegateCommand(OnSplitCommandExecuted)); }
        }

        /// <summary>
        /// Called when the <see cref="SplitCommand" /> is executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnSplitCommandExecuted(object parameter)
        {
            var list = parameter as IList;
            if (list != null)
            {
                // Determine the position of the combined item and remove it.
                int index = MemberTypes.IndexOf(parameter);
                MemberTypes.Remove(parameter);

                // Reset each item in the list and insert it into the specified position.
                var memberTypeSettings = list.OfType<MemberTypeSetting>().Reverse();
                foreach (var memberTypeSetting in memberTypeSettings)
                {
                    memberTypeSetting.EffectiveName = memberTypeSetting.DefaultName;
                    MemberTypes.Insert(index, memberTypeSetting);
                }
            }
        }

        #endregion Split Command

        #region Logic

        private ObservableCollection<object> _memberTypes;

        /// <summary>
        /// Gets an observable collection of the types.
        /// </summary>
        public ObservableCollection<object> MemberTypes
        {
            get { return _memberTypes; }
            private set
            {
                if (_memberTypes != value)
                {
                    _memberTypes = value;
                    NotifyPropertyChanged("MemberTypes");
                }
            }
        }

        /// <summary>
        /// Creates the member types collection from the current state.
        /// </summary>
        private void CreateMemberTypesFromCurrentState()
        {
            var allMemberTypes = new[] { Classes, Constructors, Delegates, Destructors, Enums, Events, Fields, Indexers, Interfaces, Methods, Properties, Structs };
            foreach (var memberType in allMemberTypes)
            {
                memberType.PropertyChanged += OnMemberTypeSettingPropertyChanged;
            }

            MemberTypes = new ObservableCollection<object>(allMemberTypes.GroupBy(x => x.Order)
                                                                         .Select(y => new List<object>(y))
                                                                         .OrderBy(z => ((MemberTypeSetting)z[0]).Order));

            MemberTypes.CollectionChanged += (sender, args) => UpdateMemberTypeSettings();
        }

        /// <summary>
        /// Handles a PropertyChanged event raised from a <see cref="MemberTypeSetting"/> and echoes it on the local object.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnMemberTypeSettingPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var memberTypeSetting = sender as MemberTypeSetting;
            if (memberTypeSetting != null)
            {
                NotifyPropertyChanged(memberTypeSetting.DefaultName);
            }
        }

        /// <summary>
        /// Updates the member type settings based on the current collection state.
        /// </summary>
        private void UpdateMemberTypeSettings()
        {
            int index = 1;

            foreach (var memberType in MemberTypes)
            {
                var memberTypeSetting = memberType as MemberTypeSetting;
                if (memberTypeSetting != null)
                {
                    memberTypeSetting.Order = index;
                }
                else
                {
                    var list = memberType as IList;
                    if (list != null)
                    {
                        var types = list.OfType<MemberTypeSetting>().ToList();

                        // If merged member types have distinct names, create a new effective name from joining their names together.
                        string newEffectiveName = null;
                        var distinctNames = types.Select(x => x.EffectiveName).Distinct().ToList();
                        if (distinctNames.Count() > 1)
                        {
                            newEffectiveName = string.Join(" + ", distinctNames);
                        }

                        foreach (var type in types)
                        {
                            type.Order = index;
                            if (!string.IsNullOrWhiteSpace(newEffectiveName))
                            {
                                type.EffectiveName = newEffectiveName;
                            }
                        }
                    }
                }

                index++;
            }
        }

        #endregion Logic
    }
}