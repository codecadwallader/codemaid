using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.Helpers
{
    public sealed class SettingsMonitor<TSetting>
        where TSetting : ApplicationSettingsBase
    {
        private readonly JoinableTaskFactory _joinableTaskFactory;
        private readonly Dictionary<string[], Monitor> _monitors = new Dictionary<string[], Monitor>(new StringArrayComparer());
        private readonly TSetting _settings;

        public SettingsMonitor(TSetting settings, JoinableTaskFactory joinableTaskFactory)
        {
            _joinableTaskFactory = joinableTaskFactory;
            _settings = settings;
            _settings.SettingsSaving += OnSettingsSaving;
        }

        public async Task WatchAsync<TValue>(Expression<Func<TSetting, TValue>> setting, Func<TValue, Task> changedCallback)
        {
            var settingName = (setting.Body as MemberExpression).Member.Name;
            await WatchAsync<TValue>(new[] { settingName }, async values => await changedCallback(values[0]));
        }

        public async Task WatchAsync<TValue>(string[] settings, Func<TValue[], Task> changedCallback)
        {
            await WatchAsync(settings, async (object[] values) =>
            {
                var typedValues = Array.ConvertAll(values, v => (TValue)v);
                await changedCallback(typedValues);
            });
        }

        public async Task WatchAsync(string[] settings, Func<object[], Task> changedCallback)
        {
            var values = FindValues(settings);

            await changedCallback(values);

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

        internal async Task NotifySettingsChangedAsync()
        {
            foreach (var item in _monitors)
            {
                var monitor = item.Value;
                var oldValues = monitor.LastValues;
                var newValues = FindValues(item.Key);
                if (!Enumerable.SequenceEqual(oldValues, newValues))
                {
                    monitor.LastValues = newValues;
                    await monitor.Callback(newValues);
                }
            }
        }

        private object[] FindValues(string[] settings) => Array.ConvertAll(settings, key => _settings[key]);

        private async void OnSettingsSaving(object sender, CancelEventArgs e)
        {
            if (_joinableTaskFactory != null)
            {
                await _joinableTaskFactory.RunAsync(NotifySettingsChangedAsync);
            }
            else
            {
                await NotifySettingsChangedAsync();
            }
        }

        private class Monitor
        {
            public Func<object[], Task> Callback;
            public object[] LastValues;
        }

        private class StringArrayComparer : IEqualityComparer<string[]>
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