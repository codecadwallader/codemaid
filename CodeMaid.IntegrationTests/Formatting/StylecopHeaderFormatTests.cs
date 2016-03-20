using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Formatting
{
    [TestClass]
    [DeploymentItem(@"Formatting\Data\StyleCopHeaderFormat.cs", "Data")]
    [DeploymentItem(@"Formatting\Data\StyleCopHeaderFormat_Formatted.cs", "Data")]
    public class StyleCopHeaderFormatTests : BaseCommentFormatTests
    {
        #region Setup

        protected override string TestBaseFileName => "StyleCopHeaderFormat";

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