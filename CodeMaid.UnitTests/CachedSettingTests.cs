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
        private CachedSetting<MemberTypeSetting> _cachedSetting;

        [TestInitialize]
        public void TestInitialize()
        {
            Settings.Default.Reset();

            _lookupCount = 0;
            _parseCount = 0;
            _cachedSetting = new CachedSetting<MemberTypeSetting>(
               () =>
               {
                   _lookupCount++;
                   return Settings.Default.Reorganizing_MemberTypeFields;
               },
               x =>
               {
                   _parseCount++;
                   return (MemberTypeSetting)x;
               });

            Assert.AreEqual(0, _lookupCount);
            Assert.AreEqual(0, _parseCount);
            Assert.IsNotNull(_cachedSetting);
        }

        [TestMethod]
        public void CachedSettingCanLookupAndParse()
        {
            var memberTypeSetting = _cachedSetting.Value;

            Assert.IsNotNull(memberTypeSetting);
            Assert.AreEqual(1, _lookupCount);
            Assert.AreEqual(1, _parseCount);
        }

        [TestMethod]
        public void CachedSettingUsesCacheOnSecondLookup()
        {
            var memberTypeSetting = _cachedSetting.Value;

            Assert.IsNotNull(memberTypeSetting);
            Assert.AreEqual(1, _lookupCount);
            Assert.AreEqual(1, _parseCount);

            var memberTypeSetting2 = _cachedSetting.Value;

            Assert.IsNotNull(memberTypeSetting2);
            Assert.AreEqual(2, _lookupCount);
            Assert.AreEqual(1, _parseCount);
        }

        [TestMethod]
        public void CachedSettingReParsesOnChange()
        {
            var memberTypeSetting = _cachedSetting.Value;

            Assert.IsNotNull(memberTypeSetting);
            Assert.AreEqual(1, _lookupCount);
            Assert.AreEqual(1, _parseCount);

            memberTypeSetting.EffectiveName = "Member Variables";
            Settings.Default.Reorganizing_MemberTypeFields = (string)memberTypeSetting;

            var memberTypeSetting2 = _cachedSetting.Value;

            Assert.IsNotNull(memberTypeSetting2);
            Assert.AreEqual(2, _lookupCount);
            Assert.AreEqual(2, _parseCount);
        }
    }
}