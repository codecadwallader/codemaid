using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Update
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Update\Data\FileHeaderSCSS.scss", "Data")]
    [DeploymentItem(@"Cleaning\Update\Data\FileHeaderSCSS_Cleaned.scss", "Data")]
    public class FileHeaderSCSSTests
    {
        #region Setup

        private static FileHeaderLogic _fileHeaderLogic;
        private ProjectItem _projectItem;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _fileHeaderLogic = FileHeaderLogic.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_fileHeaderLogic);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\FileHeaderSCSS.scss");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            TestEnvironment.RemoveFromProject(_projectItem);
        }

        #endregion Setup

        #region Tests

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningUpdateFileHeaderSCSS_CleansAsExpected()
        {
            Settings.Default.Cleaning_UpdateFileHeaderSCSS = @"/* SCSS Sample Copyright */";

            TestOperations.ExecuteCommandAndVerifyResults(RunUpdateFileHeader, _projectItem, @"Data\FileHeaderSCSS_Cleaned.scss");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningUpdateFileHeaderSCSS_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_UpdateFileHeaderSCSS = @"/* SCSS Sample Copyright */";

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunUpdateFileHeader, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningUpdateFileHeaderSCSS_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_UpdateFileHeaderSCSS = null;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunUpdateFileHeader, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunUpdateFileHeader(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            _fileHeaderLogic.UpdateFileHeader(textDocument);
        }

        #endregion Helpers
    }
}