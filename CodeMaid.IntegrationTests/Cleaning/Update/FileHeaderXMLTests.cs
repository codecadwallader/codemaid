using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Update
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Update\Data\FileHeaderXML.xml", "Data")]
    [DeploymentItem(@"Cleaning\Update\Data\FileHeaderXML_Cleaned.xml", "Data")]
    public class FileHeaderXMLTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\FileHeaderXML.xml");
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
        public void CleaningUpdateFileHeaderXML_CleansAsExpected()
        {
            Settings.Default.Cleaning_UpdateFileHeaderXML = @"<?xml version=""1.0"" encoding=""utf-8"" ?>";

            TestOperations.ExecuteCommandAndVerifyResults(RunUpdateFileHeader, _projectItem, @"Data\FileHeaderXML_Cleaned.xml");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningUpdateFileHeaderXML_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_UpdateFileHeaderXML = @"<?xml version=""1.0"" encoding=""utf-8"" ?>";

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunUpdateFileHeader, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningUpdateFileHeaderXML_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_UpdateFileHeaderXML = null;

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