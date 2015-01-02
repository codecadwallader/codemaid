#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using System.Collections.Generic;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A helper class that simplifies access to <see cref="MemberTypeSetting"/> instances.
    /// </summary>
    public static class MemberTypeSettingHelper
    {
        #region Fields

        private static readonly CachedSetting<MemberTypeSetting> CachedClassSettings;
        private static readonly CachedSetting<MemberTypeSetting> CachedConstructorSettings;
        private static readonly CachedSetting<MemberTypeSetting> CachedDelegateSettings;
        private static readonly CachedSetting<MemberTypeSetting> CachedDestructorSettings;
        private static readonly CachedSetting<MemberTypeSetting> CachedEnumSettings;
        private static readonly CachedSetting<MemberTypeSetting> CachedEventSettings;
        private static readonly CachedSetting<MemberTypeSetting> CachedFieldSettings;
        private static readonly CachedSetting<MemberTypeSetting> CachedIndexerSettings;
        private static readonly CachedSetting<MemberTypeSetting> CachedInterfaceSettings;
        private static readonly CachedSetting<MemberTypeSetting> CachedMethodSettings;
        private static readonly CachedSetting<MemberTypeSetting> CachedPropertySettings;
        private static readonly CachedSetting<MemberTypeSetting> CachedStructSettings;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The static initializer for the <see cref="MemberTypeSettingHelper"/> class.
        /// </summary>
        static MemberTypeSettingHelper()
        {
            CachedClassSettings = new CachedSetting<MemberTypeSetting>(() => Settings.Default.Reorganizing_MemberTypeClasses, MemberTypeSetting.Deserialize);
            CachedConstructorSettings = new CachedSetting<MemberTypeSetting>(() => Settings.Default.Reorganizing_MemberTypeConstructors, MemberTypeSetting.Deserialize);
            CachedDelegateSettings = new CachedSetting<MemberTypeSetting>(() => Settings.Default.Reorganizing_MemberTypeDelegates, MemberTypeSetting.Deserialize);
            CachedDestructorSettings = new CachedSetting<MemberTypeSetting>(() => Settings.Default.Reorganizing_MemberTypeDestructors, MemberTypeSetting.Deserialize);
            CachedEnumSettings = new CachedSetting<MemberTypeSetting>(() => Settings.Default.Reorganizing_MemberTypeEnums, MemberTypeSetting.Deserialize);
            CachedEventSettings = new CachedSetting<MemberTypeSetting>(() => Settings.Default.Reorganizing_MemberTypeEvents, MemberTypeSetting.Deserialize);
            CachedFieldSettings = new CachedSetting<MemberTypeSetting>(() => Settings.Default.Reorganizing_MemberTypeFields, MemberTypeSetting.Deserialize);
            CachedIndexerSettings = new CachedSetting<MemberTypeSetting>(() => Settings.Default.Reorganizing_MemberTypeIndexers, MemberTypeSetting.Deserialize);
            CachedInterfaceSettings = new CachedSetting<MemberTypeSetting>(() => Settings.Default.Reorganizing_MemberTypeInterfaces, MemberTypeSetting.Deserialize);
            CachedMethodSettings = new CachedSetting<MemberTypeSetting>(() => Settings.Default.Reorganizing_MemberTypeMethods, MemberTypeSetting.Deserialize);
            CachedPropertySettings = new CachedSetting<MemberTypeSetting>(() => Settings.Default.Reorganizing_MemberTypeProperties, MemberTypeSetting.Deserialize);
            CachedStructSettings = new CachedSetting<MemberTypeSetting>(() => Settings.Default.Reorganizing_MemberTypeStructs, MemberTypeSetting.Deserialize);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets an enumerable set of all of the settings.
        /// </summary>
        public static IEnumerable<MemberTypeSetting> AllSettings
        {
            get
            {
                return new[]
                {
                    ClassSettings, ConstructorSettings, DelegateSettings, DestructorSettings,
                    EnumSettings, EventSettings, FieldSettings, IndexerSettings,
                    InterfaceSettings, MethodSettings, PropertySettings, StructSettings
                };
            }
        }

        /// <summary>
        /// Gets the settings associated with the <see cref="KindCodeItem.Class"/> type.
        /// </summary>
        public static MemberTypeSetting ClassSettings
        {
            get { return CachedClassSettings.Value; }
        }

        /// <summary>
        /// Gets the settings associated with the <see cref="KindCodeItem.Constructor"/> type.
        /// </summary>
        public static MemberTypeSetting ConstructorSettings
        {
            get { return CachedConstructorSettings.Value; }
        }

        /// <summary>
        /// Gets the settings associated with the <see cref="KindCodeItem.Delegate"/> type.
        /// </summary>
        public static MemberTypeSetting DelegateSettings
        {
            get { return CachedDelegateSettings.Value; }
        }

        /// <summary>
        /// Gets the settings associated with the <see cref="KindCodeItem.Destructor"/> type.
        /// </summary>
        public static MemberTypeSetting DestructorSettings
        {
            get { return CachedDestructorSettings.Value; }
        }

        /// <summary>
        /// Gets the settings associated with the <see cref="KindCodeItem.Enum"/> type.
        /// </summary>
        public static MemberTypeSetting EnumSettings
        {
            get { return CachedEnumSettings.Value; }
        }

        /// <summary>
        /// Gets the settings associated with the <see cref="KindCodeItem.Event"/> type.
        /// </summary>
        public static MemberTypeSetting EventSettings
        {
            get { return CachedEventSettings.Value; }
        }

        /// <summary>
        /// Gets the settings associated with the <see cref="KindCodeItem.Field"/> type.
        /// </summary>
        public static MemberTypeSetting FieldSettings
        {
            get { return CachedFieldSettings.Value; }
        }

        /// <summary>
        /// Gets the settings associated with the <see cref="KindCodeItem.Indexer"/> type.
        /// </summary>
        public static MemberTypeSetting IndexerSettings
        {
            get { return CachedIndexerSettings.Value; }
        }

        /// <summary>
        /// Gets the settings associated with the <see cref="KindCodeItem.Interface"/> type.
        /// </summary>
        public static MemberTypeSetting InterfaceSettings
        {
            get { return CachedInterfaceSettings.Value; }
        }

        /// <summary>
        /// Gets the settings associated with the <see cref="KindCodeItem.Method"/> type.
        /// </summary>
        public static MemberTypeSetting MethodSettings
        {
            get { return CachedMethodSettings.Value; }
        }

        /// <summary>
        /// Gets the settings associated with the <see cref="KindCodeItem.Property"/> type.
        /// </summary>
        public static MemberTypeSetting PropertySettings
        {
            get { return CachedPropertySettings.Value; }
        }

        /// <summary>
        /// Gets the settings associated with the <see cref="KindCodeItem.Struct"/> type.
        /// </summary>
        public static MemberTypeSetting StructSettings
        {
            get { return CachedStructSettings.Value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Looks up the <see cref="MemberTypeSetting"/> associated with the specified kind, otherwise null.
        /// </summary>
        /// <param name="kindCodeItem">The kind of code item.</param>
        /// <returns>The associated <see cref="MemberTypeSetting"/>, otherwise null.</returns>
        public static MemberTypeSetting LookupByKind(KindCodeItem kindCodeItem)
        {
            switch (kindCodeItem)
            {
                case KindCodeItem.Class: return ClassSettings;
                case KindCodeItem.Constructor: return ConstructorSettings;
                case KindCodeItem.Delegate: return DelegateSettings;
                case KindCodeItem.Destructor: return DestructorSettings;
                case KindCodeItem.Enum: return EnumSettings;
                case KindCodeItem.Event: return EventSettings;
                case KindCodeItem.Field: return FieldSettings;
                case KindCodeItem.Indexer: return IndexerSettings;
                case KindCodeItem.Interface: return InterfaceSettings;
                case KindCodeItem.Method: return MethodSettings;
                case KindCodeItem.Property: return PropertySettings;
                case KindCodeItem.Struct: return StructSettings;
                default: return null;
            }
        }

        #endregion Methods
    }
}