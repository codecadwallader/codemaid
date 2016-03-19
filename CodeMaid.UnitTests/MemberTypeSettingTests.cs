using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.UnitTests
{
    [TestClass]
    public class MemberTypeSettingTests
    {
        [TestMethod]
        public void CanSerializeMemberTypeSetting()
        {
            var memberTypeSetting = new MemberTypeSetting("Fields", "Member Variables", 1);
            Assert.IsNotNull(memberTypeSetting);

            var serializedString = (string)memberTypeSetting;
            Assert.IsFalse(string.IsNullOrWhiteSpace(serializedString));
        }

        [TestMethod]
        public void CanDeserializeMemberTypeSetting()
        {
            const string serializedString = @"Fields||1||Member Variables";

            var memberTypeSetting = (MemberTypeSetting)serializedString;

            Assert.IsNotNull(memberTypeSetting);
            Assert.AreEqual(memberTypeSetting.DefaultName, "Fields");
            Assert.AreEqual(memberTypeSetting.EffectiveName, "Member Variables");
            Assert.AreEqual(memberTypeSetting.Order, 1);
        }
    }
}