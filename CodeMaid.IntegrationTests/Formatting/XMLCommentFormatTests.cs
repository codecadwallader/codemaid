using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Formatting
{
    [TestClass]
    [DeploymentItem(@"Formatting\Data\XMLCommentFormat.cs", "Data")]
    [DeploymentItem(@"Formatting\Data\XMLCommentFormat_Formatted.cs", "Data")]
    public class XMLCommentFormatTests : BaseCommentFormatTests
    {
        #region Setup

        protected override string TestBaseFileName => "XMLCommentFormat";

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