using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Insert\Data\BlankSpaceBeforeSelfClosingAngleBracket.xml", "Data")]
    [DeploymentItem(@"Cleaning\Insert\Data\BlankSpaceBeforeSelfClosingAngleBracket_Cleaned.xml", "Data")]
    public class BlankSpaceBeforeSelfClosingAngleBracketTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\BlankSpaceBeforeSelfClosingAngleBracket.xml");
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
        public void CleaningInsertBlankSpaceBeforeSelfClosingAngleBracket_CleansAsExpected()
        {
            Settings.Default.Cleaning_InsertBlankSpaceBeforeSelfClosingAngleBrackets = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunInsertBlankSpaceBeforeSelfClosingAngleBracket, _projectItem, @"Data\BlankSpaceBeforeSelfClosingAngleBracket_Cleaned.xml");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertBlankSpaceBeforeSelfClosingAngleBracket_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_InsertBlankSpaceBeforeSelfClosingAngleBrackets = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunInsertBlankSpaceBeforeSelfClosingAngleBracket, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertBlankSpaceBeforeSelfClosingAngleBracket_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_InsertBlankSpaceBeforeSelfClosingAngleBrackets = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunInsertBlankSpaceBeforeSelfClosingAngleBracket, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunInsertBlankSpaceBeforeSelfClosingAngleBracket(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            _insertWhitespaceLogic.InsertBlankSpaceBeforeSelfClosingAngleBracket(textDocument);
        }

        #endregion Helpers
    }
}