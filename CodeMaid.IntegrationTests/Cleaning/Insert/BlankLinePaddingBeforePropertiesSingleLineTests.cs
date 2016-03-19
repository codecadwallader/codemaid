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
    [DeploymentItem(@"Cleaning\Insert\Data\BlankLinePaddingBeforePropertiesSingleLine.cs", "Data")]
    [DeploymentItem(@"Cleaning\Insert\Data\BlankLinePaddingBeforePropertiesSingleLine_Cleaned.cs", "Data")]
    public class BlankLinePaddingBeforePropertiesSingleLineTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\BlankLinePaddingBeforePropertiesSingleLine.cs");
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
        public void CleaningInsertBlankLinePaddingBeforePropertiesSingleLine_CleansAsExpected()
        {
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforePropertiesSingleLine = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunInsertBlankLinePaddingBeforePropertiesSingleLine, _projectItem, @"Data\BlankLinePaddingBeforePropertiesSingleLine_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertBlankLinePaddingBeforePropertiesSingleLine_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforePropertiesSingleLine = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunInsertBlankLinePaddingBeforePropertiesSingleLine, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertBlankLinePaddingBeforePropertiesSingleLine_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforePropertiesSingleLine = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunInsertBlankLinePaddingBeforePropertiesSingleLine, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunInsertBlankLinePaddingBeforePropertiesSingleLine(Document document)
        {
            var codeItems = TestOperations.CodeModelManager.RetrieveAllCodeItems(document);
            var properties = codeItems.OfType<CodeItemProperty>().ToList();

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(properties);
        }

        #endregion Helpers
    }
}