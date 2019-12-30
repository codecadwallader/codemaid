﻿using System.Configuration;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Properties
{
    /// <summary>
    /// This partial class instructs the <see cref="Settings"/> class to utilize the <see cref="CodeMaidSettingsProvider"/>.
    /// </summary>
    [SettingsProvider(typeof(CodeMaidSettingsProvider))]
    public sealed partial class Settings
    {
        /// <summary>
        /// Updates application settings to reflect a more recent installation of the application.
        /// </summary>
        public override void Upgrade()
        {
            var oldSettingsProvider = new LocalFileSettingsProvider();
            var oldPropertyValues = oldSettingsProvider.GetPropertyValues(Context, Properties);

            foreach (SettingsPropertyValue oldPropertyValue in oldPropertyValues)
            {
                if (!Equals(this[oldPropertyValue.Name], oldPropertyValue.PropertyValue))
                {
                    this[oldPropertyValue.Name] = oldPropertyValue.PropertyValue;
                }
            }
        }

        static char[] itemsDelimiters = new[] { '|' };

        public string[] Digging_HideSpadeItemsNames
            => (this.Digging_HideSpadeItems ?? "").Split(itemsDelimiters, System.StringSplitOptions.RemoveEmptyEntries);
    }
}