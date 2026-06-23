using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SteveCadwallader.CodeMaid.UnitTests
{
    public class CachedSettingSetTests
    {
        private int _lookupCount;
        private int _parseCount;
        private CachedSettingSet<string> _cachedSettingSet;

        public CachedSettingSetTests()
        {
            Settings.Default.Reset();

            _lookupCount = 0;
            _parseCount = 0;
            _cachedSettingSet = new CachedSettingSet<string>(
               () =>
               {
                   _lookupCount++;
                   return Settings.Default.Cleaning_ExclusionExpression;
               },
               x =>
               {
                   _parseCount++;
                   return x.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries)
                           .Select(y => y.Trim().ToLower())
                           .Where(z => !string.IsNullOrEmpty(z))
                           .ToList();
               });

            Assert.Equal(0, _lookupCount);
            Assert.Equal(0, _parseCount);
            Assert.NotNull(_cachedSettingSet);
        }

        [Fact]
        public void CachedSettingSetCanLookupAndParse()
        {
            var cleanupExclusions = _cachedSettingSet.Value;

            Assert.NotNull(cleanupExclusions);
            Assert.Equal(1, _lookupCount);
            Assert.Equal(1, _parseCount);
        }

        [Fact]
        public void CachedSettingSetUsesCacheOnSecondLookup()
        {
            var cleanupExclusions = _cachedSettingSet.Value;

            Assert.NotNull(cleanupExclusions);
            Assert.Equal(1, _lookupCount);
            Assert.Equal(1, _parseCount);

            var cleanupExclusions2 = _cachedSettingSet.Value;

            Assert.NotNull(cleanupExclusions2);
            Assert.Equal(2, _lookupCount);
            Assert.Equal(1, _parseCount);
        }

        [Fact]
        public void CachedSettingSetReParsesOnChange()
        {
            var cleanupExclusions = _cachedSettingSet.Value;

            Assert.NotNull(cleanupExclusions);
            Assert.Equal(1, _lookupCount);
            Assert.Equal(1, _parseCount);

            var cleanupExclusion2 = new List<string>(cleanupExclusions) { ".*Test.*" };
            var serializedCleanupExclusions = string.Join("||", cleanupExclusion2);

            Settings.Default.Cleaning_ExclusionExpression = serializedCleanupExclusions;

            var memberTypeSetting2 = _cachedSettingSet.Value;

            Assert.NotNull(memberTypeSetting2);
            Assert.Equal(2, _lookupCount);
            Assert.Equal(2, _parseCount);
        }
    }
}
