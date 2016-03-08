#region CodeMaid is Copyright 2007-2016 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2016 Steve Cadwallader.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Formatting
{
    [TestClass]
    [DeploymentItem(@"Formatting\Data\XMLCommentFormat.cs", "Data")]
    [DeploymentItem(@"Formatting\Data\XMLCommentFormat_Formatted.cs", "Data")]
    public class XMLCommentFormatTests : BaseCommentFormatTests
    {
        #region Setup

        protected override string TestBaseFileName
        {
            get { return "XMLCommentFormat"; }
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
        public void FormatXMLComments_FormatsAsExpected()
        {
            FormatsAsExpected();
        }

        [TestMethod]
        [HostType("VS IDE")]
        [TestCategory("Formatting")]
        public void FormatXMLComments_DoesNothingOnSecondPass()
        {
            DoesNothingOnSecondPass();
        }

        [TestMethod]
        [HostType("VS IDE")]
        [TestCategory("Formatting")]
        public void FormatXMLComments_DoesNothingWhenSettingIsDisabled()
        {
            DoesNothingWhenSettingIsDisabled();
        }

        #endregion Tests
    }
}