using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using Xunit;

namespace SteveCadwallader.CodeMaid.UnitTests
{
    public class CachedSettingTests
    {
        private int _lookupCount;
        private int _parseCount;
        private CachedSetting<MemberTypeSetting> _cachedSetting;

        public CachedSettingTests()
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

            Assert.Equal(0, _lookupCount);
            Assert.Equal(0, _parseCount);
            Assert.NotNull(_cachedSetting);
        }

        [Fact]
        public void CachedSettingCanLookupAndParse()
        {
            var memberTypeSetting = _cachedSetting.Value;

            Assert.NotNull(memberTypeSetting);
            Assert.Equal(1, _lookupCount);
            Assert.Equal(1, _parseCount);
        }

        [Fact]
        public void CachedSettingUsesCacheOnSecondLookup()
        {
            var memberTypeSetting = _cachedSetting.Value;

            Assert.NotNull(memberTypeSetting);
            Assert.Equal(1, _lookupCount);
            Assert.Equal(1, _parseCount);

            var memberTypeSetting2 = _cachedSetting.Value;

            Assert.NotNull(memberTypeSetting2);
            Assert.Equal(2, _lookupCount);
            Assert.Equal(1, _parseCount);
        }

        [Fact]
        public void CachedSettingReParsesOnChange()
        {
            var memberTypeSetting = _cachedSetting.Value;

            Assert.NotNull(memberTypeSetting);
            Assert.Equal(1, _lookupCount);
            Assert.Equal(1, _parseCount);

            memberTypeSetting.EffectiveName = "Member Variables";
            Settings.Default.Reorganizing_MemberTypeFields = (string)memberTypeSetting;

            var memberTypeSetting2 = _cachedSetting.Value;

            Assert.NotNull(memberTypeSetting2);
            Assert.Equal(2, _lookupCount);
            Assert.Equal(2, _parseCount);
        }
    }
}
