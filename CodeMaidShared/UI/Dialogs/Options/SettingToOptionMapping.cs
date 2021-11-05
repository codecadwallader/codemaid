using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options
{
    /// <summary>
    /// A model class for mapping a setting and an option together, with optional overrides for how
    /// the cast between the two occurs.
    /// </summary>
    public class SettingToOptionMapping<TS, TO> : ISettingToOptionMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingToOptionMapping{TS,TO}"/> class.
        /// </summary>
        /// <param name="settingExpression">The expression describing the setting property.</param>
        /// <param name="optionExpression">The expression describing the option property.</param>
        public SettingToOptionMapping(Expression<Func<object, TS>> settingExpression, Expression<Func<object, TO>> optionExpression)
        {
            SettingProperty = GetPropertyInfo(settingExpression);
            OptionProperty = GetPropertyInfo(optionExpression);
        }

        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> defining the setting property.
        /// </summary>
        public PropertyInfo SettingProperty { get; private set; }

        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> defining the option property.
        /// </summary>
        public PropertyInfo OptionProperty { get; private set; }

        /// <summary>
        /// Copies the value within the setting property onto the option property.
        /// </summary>
        /// <param name="settingsClass">The class instance for the settings property.</param>
        /// <param name="optionClass">The class instance for the option property.</param>
        public void CopySettingToOption(Settings settingsClass, object optionClass)
        {
            var settingValue = SettingProperty.GetValue(settingsClass);

            // Special case handling for MemberTypeSetting as operator casts for generics don't work as expected.
            if (typeof(TS) == typeof(string) && typeof(TO) == typeof(MemberTypeSetting))
            {
                var optionValue = (MemberTypeSetting)(string)settingValue;

                // Note: No need to do an equality comparison before assignment as all options already have that through the Bindable base class.
                OptionProperty.SetValue(optionClass, optionValue);
            }
            else
            {
                var optionValue = (TO)settingValue;

                // Note: No need to do an equality comparison before assignment as all options already have that through the Bindable base class.
                OptionProperty.SetValue(optionClass, optionValue);
            }
        }

        /// <summary>
        /// Copies the value within the option property onto the setting property.
        /// </summary>
        /// <param name="settingsClass">The class instance for the settings property.</param>
        /// <param name="optionClass">The class instance for the option property.</param>
        public void CopyOptionToSetting(Settings settingsClass, object optionClass)
        {
            // Special case handling for MemberTypeSetting as operator casts for generics don't work as expected.
            if (typeof(TS) == typeof(string) && typeof(TO) == typeof(MemberTypeSetting))
            {
                var optionValue = (string)(MemberTypeSetting)OptionProperty.GetValue(optionClass);
                var settingValue = (string)SettingProperty.GetValue(settingsClass);

                if (!EqualityComparer<string>.Default.Equals(optionValue, settingValue))
                {
                    SettingProperty.SetValue(settingsClass, optionValue);
                }
            }
            else
            {
                var optionValue = (TS)OptionProperty.GetValue(optionClass);
                var settingValue = (TS)SettingProperty.GetValue(settingsClass);

                if (!EqualityComparer<TS>.Default.Equals(optionValue, settingValue))
                {
                    SettingProperty.SetValue(settingsClass, optionValue);
                }
            }
        }

        /// <summary>
        /// Retrieves a <see cref="PropertyInfo"/> from the specified expression.
        /// </summary>
        /// <typeparam name="TA">The type of the source object.</typeparam>
        /// <typeparam name="TR">The type of the source property.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>A <see cref="PropertyInfo"/> described by the expression.</returns>
        private static PropertyInfo GetPropertyInfo<TA, TR>(Expression<Func<TA, TR>> expression)
        {
            var body = (MemberExpression)expression.Body;
            var prop = (PropertyInfo)body.Member;
            return prop;
        }
    }
}