#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

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

            var serializedString = memberTypeSetting.Serialize();
            Assert.IsFalse(string.IsNullOrWhiteSpace(serializedString));
        }

        [TestMethod]
        public void CanDeserializeMemberTypeSetting()
        {
            const string serializedString = @"Fields||1||Member Variables";

            var memberTypeSetting = MemberTypeSetting.Deserialize(serializedString);

            Assert.IsNotNull(memberTypeSetting);
            Assert.AreEqual(memberTypeSetting.DefaultName, "Fields");
            Assert.AreEqual(memberTypeSetting.EffectiveName, "Member Variables");
            Assert.AreEqual(memberTypeSetting.Order, 1);
        }
    }
}