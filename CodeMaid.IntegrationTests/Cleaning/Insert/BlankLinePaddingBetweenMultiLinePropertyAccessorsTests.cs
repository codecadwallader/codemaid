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
    [DeploymentItem(@"Cleaning\Insert\Data\BlankLinePaddingBetweenMultiLinePropertyAccessors.cs", "Data")]
    [DeploymentItem(@"Cleaning\Insert\Data\BlankLinePaddingBetweenMultiLinePropertyAccessors_Cleaned.cs", "Data")]
    public class BlankLinePaddingBetweenMultiLinePropertyAccessorsTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\BlankLinePaddingBetweenMultiLinePropertyAccessors.cs");
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
        public void CleaningInsertBlankLinePaddingBetweenMultiLinePropertyAccessors_CleansAsExpected()
        {
            Settings.Default.Cleaning_InsertBlankLinePaddingBetweenPropertiesMultiLineAccessors = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunInsertBlankLinePaddingBetweenMultiLinePropertyAccessors, _projectItem, @"Data\BlankLinePaddingBetweenMultiLinePropertyAccessors_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertBlankLinePaddingBetweenMultiLinePropertyAccessors_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_InsertBlankLinePaddingBetweenPropertiesMultiLineAccessors = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunInsertBlankLinePaddingBetweenMultiLinePropertyAccessors, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertBlankLinePaddingBetweenMultiLinePropertyAccessors_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_InsertBlankLinePaddingBetweenPropertiesMultiLineAccessors = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunInsertBlankLinePaddingBetweenMultiLinePropertyAccessors, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunInsertBlankLinePaddingBetweenMultiLinePropertyAccessors(Document document)
        {
            var codeItems = TestOperations.CodeModelManager.RetrieveAllCodeItems(document);
            var properties = codeItems.OfType<CodeItemProperty>().ToList();

            _insertBlankLinePaddingLogic.InsertPaddingBetweenMultiLinePropertyAccessors(properties);
        }

        #endregion Helpers
    }
}