using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Insert\Data\InsertEndOfFileTrailingNewLine.cs", "Data")]
    [DeploymentItem(@"Cleaning\Insert\Data\InsertEndOfFileTrailingNewLine_Cleaned.cs", "Data")]
    public class InsertEndOfFileTrailingNewLineTests
    {
        #region Setup

        private static InsertWhitespaceLogic _insertWhitespaceLogic;
        private ProjectItem _projectItem;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _insertWhitespaceLogic = InsertWhitespaceLogic.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_insertWhitespaceLogic);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\InsertEndOfFileTrailingNewLine.cs");
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
        public void CleaningInsertEndOfFileTrailingNewLine_CleansAsExpected()
        {
            Settings.Default.Cleaning_InsertEndOfFileTrailingNewLine = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunInsertEndOfFileTrailingNewLine, _projectItem, @"Data\InsertEndOfFileTrailingNewLine_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertEndOfFileTrailingNewLine_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_InsertEndOfFileTrailingNewLine = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunInsertEndOfFileTrailingNewLine, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertEndOfFileTrailingNewLine_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_InsertEndOfFileTrailingNewLine = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunInsertEndOfFileTrailingNewLine, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunInsertEndOfFileTrailingNewLine(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            _insertWhitespaceLogic.InsertEOFTrailingNewLine(textDocument);
        }

        #endregion Helpers
    }
}