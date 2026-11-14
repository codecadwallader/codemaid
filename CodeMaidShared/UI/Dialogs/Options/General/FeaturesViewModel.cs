using SteveCadwallader.CodeMaid.Properties;
using Mapping = SteveCadwallader.CodeMaid.UI.Dialogs.Options.SettingToOptionMapping<bool, bool>;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.General
{
    public class FeaturesViewModel : OptionsPageViewModel
    {
        public FeaturesViewModel(CodeMaidPackage package, Settings activeSettings)
            : base(package, activeSettings)
        {
            Mappings = new SettingsToOptionsList(ActiveSettings, this)
            {
                new Mapping(x => ActiveSettings.Feature_CleanupActiveCode, x => CleanupActiveCode),
                new Mapping(x => ActiveSettings.Feature_CleanupAllCode, x => CleanupAllCode),
                new Mapping(x => ActiveSettings.Feature_CleanupOpenCode, x => CleanupOpenCode),
                new Mapping(x => ActiveSettings.Feature_CleanupSelectedCode, x => CleanupSelectedCode),
                new Mapping(x => ActiveSettings.Feature_CommentFormat, x => CommentFormat),
                new Mapping(x => ActiveSettings.Feature_JoinLines, x => JoinLines),
                new Mapping(x => ActiveSettings.Feature_SettingCleanupOnSave, x => SettingCleanupOnSave),
                new Mapping(x => ActiveSettings.Feature_SortLines, x => SortLines)
            };
        }

        public override string Header => Resources.FeaturesViewModel_Features;

        public bool CleanupActiveCode
        {
            get => GetPropertyValue<bool>();
            set
            {
                SetPropertyValue(value);

                if (!value)
                {
                    SettingCleanupOnSave = false;
                }
            }
        }

        public bool CleanupAllCode
        {
            get => GetPropertyValue<bool>();
            set => SetPropertyValue(value);
        }

        public bool CleanupOpenCode
        {
            get => GetPropertyValue<bool>();
            set => SetPropertyValue(value);
        }

        public bool CleanupSelectedCode
        {
            get => GetPropertyValue<bool>();
            set => SetPropertyValue(value);
        }

        public bool CommentFormat
        {
            get => GetPropertyValue<bool>();
            set => SetPropertyValue(value);
        }

        public bool JoinLines
        {
            get => GetPropertyValue<bool>();
            set => SetPropertyValue(value);
        }

        public bool SettingCleanupOnSave
        {
            get => GetPropertyValue<bool>();
            set => SetPropertyValue(value);
        }

        public bool SortLines
        {
            get => GetPropertyValue<bool>();
            set => SetPropertyValue(value);
        }
    }
}
