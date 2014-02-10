#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Comments
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Comments\Data\StylecopHeaderFormat.cs", "Data")]
    [DeploymentItem(@"Cleaning\Comments\Data\StylecopHeaderFormat_Cleaned.cs", "Data")]
    public class StylecopHeaderFormatTests
        : CommentFormatTestsHelper
    {
        protected override string BaseFileName
        {
            get { return "StylecopHeaderFormat"; }
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void StylecopHeaderFormatTests_CleansAsExpected()
        {
            base.CleansAsExpected();
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void StylecopHeaderFormatTests_DoesNothingOnSecondPass()
        {
            base.DoesNothingOnSecondPass();
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void StylecopHeaderFormatTests_DoesNothingWhenSettingIsDisabled()
        {
            base.DoesNothingWhenSettingIsDisabled();
        }
    }
}