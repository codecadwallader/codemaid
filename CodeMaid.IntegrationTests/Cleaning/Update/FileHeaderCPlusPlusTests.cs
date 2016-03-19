using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Update
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Update\Data\FileHeaderCPlusPlus.cpp", "Data")]
    [DeploymentItem(@"Cleaning\Update\Data\FileHeaderCPlusPlus_Cleaned.cpp", "Data")]
    public class FileHeaderCPlusPlusTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\FileHeaderCPlusPlus.cpp");
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
        public void CleaningUpdateFileHeaderCPlusPlus_CleansAsExpected()
        {
            Settings.Default.Cleaning_UpdateFileHeaderCPlusPlus =
@"//-----------------------------------------------------------------------
// <copyright file=""FileHeaderCPlusPlus.cpp"" company=""CodeMaid"">
//     Sample copyright.
// </copyright>
//-----------------------------------------------------------------------";

            TestOperations.ExecuteCommandAndVerifyResults(RunUpdateFileHeader, _projectItem, @"Data\FileHeaderCPlusPlus_Cleaned.cpp");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningUpdateFileHeaderCPlusPlus_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_UpdateFileHeaderCPlusPlus =
@"//-----------------------------------------------------------------------
// <copyright file=""FileHeaderCPlusPlus.cpp"" company=""CodeMaid"">
//     Sample copyright.
// </copyright>
//-----------------------------------------------------------------------";

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunUpdateFileHeader, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningUpdateFileHeaderCPlusPlus_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_UpdateFileHeaderCPlusPlus = null;

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