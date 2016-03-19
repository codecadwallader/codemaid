using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SteveCadwallader.CodeMaid.UnitTests
{
    [TestClass]
    public class CachedSettingSetTests
    {
        private int _lookupCount;
        private int _parseCount;
        private CachedSettingSet<string> _cachedSettingSet;

        [TestInitialize]
        public void TestInitialize()
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

            Assert.AreEqual(0, _lookupCount);
            Assert.AreEqual(0, _parseCount);
            Assert.IsNotNull(_cachedSettingSet);
        }

        [TestMethod]
        public void CachedSettingSetCanLookupAndParse()
        {
            var cleanupExclusions = _cachedSettingSet.Value;

            Assert.IsNotNull(cleanupExclusions);
            Assert.AreEqual(1, _lookupCount);
            Assert.AreEqual(1, _parseCount);
        }

        [TestMethod]
        public void CachedSettingSetUsesCacheOnSecondLookup()
        {
            var cleanupExclusions = _cachedSettingSet.Value;

            Assert.IsNotNull(cleanupExclusions);
            Assert.AreEqual(1, _lookupCount);
            Assert.AreEqual(1, _parseCount);

            var cleanupExclusions2 = _cachedSettingSet.Value;

            Assert.IsNotNull(cleanupExclusions2);
            Assert.AreEqual(2, _lookupCount);
            Assert.AreEqual(1, _parseCount);
        }

        [TestMethod]
        public void CachedSettingSetReParsesOnChange()
        {
            var cleanupExclusions = _cachedSettingSet.Value;

            Assert.IsNotNull(cleanupExclusions);
            Assert.AreEqual(1, _lookupCount);
            Assert.AreEqual(1, _parseCount);

            var cleanupExclusion2 = new List<string>(cleanupExclusions) { ".*Test.*" };
            var serializedCleanupExclusions = string.Join("||", cleanupExclusion2);

            Settings.Default.Cleaning_ExclusionExpression = serializedCleanupExclusions;

            var memberTypeSetting2 = _cachedSettingSet.Value;

            Assert.IsNotNull(memberTypeSetting2);
            Assert.AreEqual(2, _lookupCount);
            Assert.AreEqual(2, _parseCount);
        }
    }
}