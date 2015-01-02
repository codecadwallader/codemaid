#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Formatting
{
    [TestClass]
    [DeploymentItem(@"Formatting\Data\StyleCopHeaderFormat.cs", "Data")]
    [DeploymentItem(@"Formatting\Data\StyleCopHeaderFormat_Formatted.cs", "Data")]
    public class StyleCopHeaderFormatTests : BaseCommentFormatTests
    {
        #region Setup

        protected override string TestBaseFileName
        {
            get { return "StyleCopHeaderFormat"; }
        }

        [ClassInitialize]
        public new static void ClassInitialize(TestContext testContext)
        {
            BaseCommentFormatTests.ClassInitialize(testContext);
        }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
        }

        [TestCleanup]
        public override void TestCleanup()
        {
            base.TestCleanup();
        }

        #endregion Setup

        #region Tests

        [TestMethod]
        [HostType("VS IDE")]
        [TestCategory("Formatting")]
        public void FormatStyleCopHeaderComments_FormatsAsExpected()
        {
            FormatsAsExpected();
        }

        [TestMethod]
        [HostType("VS IDE")]
        [TestCategory("Formatting")]
        public void FormatStyleCopHeaderComments_DoesNothingOnSecondPass()
        {
            DoesNothingOnSecondPass();
        }

        [TestMethod]
        [HostType("VS IDE")]
        [TestCategory("Formatting")]
        public void FormatStyleCopHeaderComments_DoesNothingWhenSettingIsDisabled()
        {
            DoesNothingWhenSettingIsDisabled();
        }

        #endregion Tests
    }
}