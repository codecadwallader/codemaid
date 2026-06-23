using SteveCadwallader.CodeMaid.Helpers;
using Xunit;

namespace SteveCadwallader.CodeMaid.UnitTests
{
    public class MemberTypeSettingTests
    {
        [Fact]
        public void CanSerializeMemberTypeSetting()
        {
            var memberTypeSetting = new MemberTypeSetting("Fields", "Member Variables", 1);
            Assert.NotNull(memberTypeSetting);

            var serializedString = (string)memberTypeSetting;
            Assert.False(string.IsNullOrWhiteSpace(serializedString));
        }

        [Fact]
        public void CanDeserializeMemberTypeSetting()
        {
            const string serializedString = @"Fields||1||Member Variables";

            var memberTypeSetting = (MemberTypeSetting)serializedString;

            Assert.NotNull(memberTypeSetting);
            Assert.Equal("Fields", memberTypeSetting.DefaultName);
            Assert.Equal("Member Variables", memberTypeSetting.EffectiveName);
            Assert.Equal(1, memberTypeSetting.Order);
        }
    }
}
