using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;

namespace SteveCadwallader.CodeMaid.Helpers
{
    public sealed class SettingMonitor<TSetting> where TSetting : ApplicationSettingsBase
    {
        private readonly Dictionary<string[], Monitor> _monitors = new Dictionary<string[], Monitor>(new StringArrayComparor());

        public SettingMonitor(TSetting settings)
        {
            Settings = settings;
            Settings.SettingsSaving += (s, e) => NotifySettingsChanged();
        }

        public TSetting Settings { get; }

        internal void NotifySettingsChanged()
        {
            foreach (var item in _monitors)
            {
                var monitor = item.Value;
                var oldValues = monitor.LastValues;
                var newValues = FindValues(item.Key);
                if (!Enumerable.SequenceEqual(oldValues, newValues))
                {
                    monitor.LastValues = newValues;
                    monitor.Callback.Invoke(newValues);
                }
            }
        }

        private object[] FindValues(string[] settings) => Array.ConvertAll(settings, key => Settings[key]);

        public void Watch<TValue>(Expression<Func<TSetting, TValue>> setting, Action<TValue> changedCallback)
        {
            var settingName = (setting.Body as MemberExpression).Member.Name;
            Watch<TValue>(new[] { settingName }, values => changedCallback.Invoke(values[0]));
        }

        public void Watch<TValue>(string[] settings, Action<TValue[]> changedCallback)
        {
            Watch(settings, (object[] values) =>
            {
                var typedValues = Array.ConvertAll(values, v => (TValue)v);
                changedCallback.Invoke(typedValues);
            });
        }

        public void Watch(string[] settings, Action<object[]> changedCallback)
        {
            var values = FindValues(settings);

            changedCallback.Invoke(values);

            if (_monitors.TryGetValue(settings, out var monitor))
            {
                monitor.Callback += changedCallback;
            }
            else
            {
                monitor = new Monitor { LastValues = values, Callback = changedCallback };
                _monitors.Add(settings, monitor);
            }
        }

        private class Monitor
        {
            public object[] LastValues;
            public Action<object[]> Callback;
        }

        private class StringArrayComparor : IEqualityComparer<string[]>
        {
            private static readonly StringComparer ElementComparer = StringComparer.OrdinalIgnoreCase;

            public bool Equals(string[] x, string[] y)
                => Enumerable.SequenceEqual(x, y, ElementComparer);

            public int GetHashCode(string[] strings)
            {
                int hash = 0;
                for (int i = 0; i < strings.Length; i++)
                {
                    hash = unchecked(
                        hash * 31 ^ ElementComparer.GetHashCode(strings[i])
                    );
                }
                return hash;
            }
        }
    }
}