using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UnitTests
{
    [TestClass]
    public class CachedSettingTests
    {
        private int _lookupCount;
        private int _parseCount;
        private CachedSetting<string> _cachedSetting;

        [TestInitialize]
        public void TestInitialize()
        {
            Settings.Default.Reset();

            _lookupCount = 0;
            _parseCount = 0;
            _cachedSetting = new CachedSetting<string>(
               () =>
               {
                   _lookupCount++;
                   return Settings.Default.Cleaning_ExclusionExpression;
               },
               x =>
               {
                   _parseCount++;
                   return x;
               });

            Assert.AreEqual(0, _lookupCount);
            Assert.AreEqual(0, _parseCount);
            Assert.IsNotNull(_cachedSetting);
        }

        [TestMethod]
        public void CachedSettingCanLookupAndParse()
        {
            var value = _cachedSetting.Value;

            Assert.IsNotNull(value);
            Assert.AreEqual(1, _lookupCount);
            Assert.AreEqual(1, _parseCount);
        }

        [TestMethod]
        public void CachedSettingUsesCacheOnSecondLookup()
        {
            var value = _cachedSetting.Value;

            Assert.IsNotNull(value);
            Assert.AreEqual(1, _lookupCount);
            Assert.AreEqual(1, _parseCount);

            var value2 = _cachedSetting.Value;

            Assert.IsNotNull(value2);
            Assert.AreEqual(2, _lookupCount);
            Assert.AreEqual(1, _parseCount);
        }

        [TestMethod]
        public void CachedSettingReParsesOnChange()
        {
            var value = _cachedSetting.Value;

            Assert.IsNotNull(value);
            Assert.AreEqual(1, _lookupCount);
            Assert.AreEqual(1, _parseCount);

            Settings.Default.Cleaning_ExclusionExpression = "changed";

            var value2 = _cachedSetting.Value;

            Assert.AreEqual("changed", value2);
            Assert.AreEqual(2, _lookupCount);
            Assert.AreEqual(2, _parseCount);
        }
    }
}
