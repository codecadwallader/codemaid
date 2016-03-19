using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using System.Linq;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Insert\Data\BlankLinePaddingAfterFieldsMultiLine.cs", "Data")]
    [DeploymentItem(@"Cleaning\Insert\Data\BlankLinePaddingAfterFieldsMultiLine_Cleaned.cs", "Data")]
    public class BlankLinePaddingAfterFieldsMultiLineTests
    {
        #region Setup

        private static InsertBlankLinePaddingLogic _insertBlankLinePaddingLogic;
        private ProjectItem _projectItem;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _insertBlankLinePaddingLogic = InsertBlankLinePaddingLogic.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_insertBlankLinePaddingLogic);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\BlankLinePaddingAfterFieldsMultiLine.cs");
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
        public void CleaningInsertBlankLinePaddingAfterFieldsMultiLine_CleansAsExpected()
        {
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterFieldsMultiLine = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunInsertBlankLinePaddingAfterFieldsMultiLine, _projectItem, @"Data\BlankLinePaddingAfterFieldsMultiLine_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertBlankLinePaddingAfterFieldsMultiLine_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterFieldsMultiLine = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunInsertBlankLinePaddingAfterFieldsMultiLine, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertBlankLinePaddingAfterFieldsMultiLine_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_InsertBlankLinePaddingAfterFieldsMultiLine = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunInsertBlankLinePaddingAfterFieldsMultiLine, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunInsertBlankLinePaddingAfterFieldsMultiLine(Document document)
        {
            var codeItems = TestOperations.CodeModelManager.RetrieveAllCodeItems(document);
            var fields = codeItems.OfType<CodeItemField>().ToList();

            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(fields);
        }

        #endregion Helpers
    }
}