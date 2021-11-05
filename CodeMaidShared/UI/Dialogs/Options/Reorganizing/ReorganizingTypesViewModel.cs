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
        /// <param name="activeSettings">The active settings.</param>
        public ReorganizingTypesViewModel(CodeMaidPackage package, Settings activeSettings)
            : base(package, activeSettings)
        {
            Mappings = new SettingsToOptionsList(ActiveSettings, this)
            {
                new SettingToOptionMapping<string, MemberTypeSetting>(x => ActiveSettings.Reorganizing_MemberTypeClasses, x => Classes),
                new SettingToOptionMapping<string, MemberTypeSetting>(x => ActiveSettings.Reorganizing_MemberTypeConstructors, x => Constructors),
                new SettingToOptionMapping<string, MemberTypeSetting>(x => ActiveSettings.Reorganizing_MemberTypeDelegates, x => Delegates),
                new SettingToOptionMapping<string, MemberTypeSetting>(x => ActiveSettings.Reorganizing_MemberTypeDestructors, x => Destructors),
                new SettingToOptionMapping<string, MemberTypeSetting>(x => ActiveSettings.Reorganizing_MemberTypeEnums, x => Enums),
                new SettingToOptionMapping<string, MemberTypeSetting>(x => ActiveSettings.Reorganizing_MemberTypeEvents, x => Events),
                new SettingToOptionMapping<string, MemberTypeSetting>(x => ActiveSettings.Reorganizing_MemberTypeFields, x => Fields),
                new SettingToOptionMapping<string, MemberTypeSetting>(x => ActiveSettings.Reorganizing_MemberTypeIndexers, x => Indexers),
                new SettingToOptionMapping<string, MemberTypeSetting>(x => ActiveSettings.Reorganizing_MemberTypeInterfaces, x => Interfaces),
                new SettingToOptionMapping<string, MemberTypeSetting>(x => ActiveSettings.Reorganizing_MemberTypeMethods, x => Methods),
                new SettingToOptionMapping<string, MemberTypeSetting>(x => ActiveSettings.Reorganizing_MemberTypeProperties, x => Properties),
                new SettingToOptionMapping<string, MemberTypeSetting>(x => ActiveSettings.Reorganizing_MemberTypeStructs, x => Structs)
            };
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header => Resources.ReorganizingTypesViewModel_Types;

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            base.LoadSettings();

            CreateMemberTypesFromCurrentState();
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
        public DelegateCommand SplitCommand => _splitCommand ?? (_splitCommand = new DelegateCommand(OnSplitCommandExecuted));

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

        /// <summary>
        /// Gets an observable collection of the types.
        /// </summary>
        public ObservableCollection<object> MemberTypes
        {
            get { return GetPropertyValue<ObservableCollection<object>>(); }
            private set { SetPropertyValue(value); }
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
                // Raise NotifyPropertyChanged on the DefaultName of the MemberTypeSetting which matches the property name on this class.
                RaisePropertyChanged(memberTypeSetting.DefaultName);

                // If the EffectiveName changed for one member in a group, be sure all other members in the group are synchronized.
                if (e.PropertyName == "EffectiveName")
                {
                    var list = MemberTypes.OfType<IList>().FirstOrDefault(x => x.Contains(memberTypeSetting));
                    if (list != null && list.Count > 1)
                    {
                        foreach (var type in list.OfType<MemberTypeSetting>())
                        {
                            type.EffectiveName = memberTypeSetting.EffectiveName;
                        }
                    }
                }
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
                        if (distinctNames.Count > 1)
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