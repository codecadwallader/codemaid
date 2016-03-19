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
    [DeploymentItem(@"Cleaning\Insert\Data\ExplicitAccessModifiersOnDelegates.cs", "Data")]
    [DeploymentItem(@"Cleaning\Insert\Data\ExplicitAccessModifiersOnDelegates_Cleaned.cs", "Data")]
    public class ExplicitAccessModifiersOnDelegatesTests
    {
        #region Setup

        private static InsertExplicitAccessModifierLogic _insertExplicitAccessModifierLogic;
        private ProjectItem _projectItem;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _insertExplicitAccessModifierLogic = InsertExplicitAccessModifierLogic.GetInstance();
            Assert.IsNotNull(_insertExplicitAccessModifierLogic);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\ExplicitAccessModifiersOnDelegates.cs");
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
        public void CleaningInsertExplicitAccessModifiersOnDelegates_CleansAsExpected()
        {
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnDelegates = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunInsertExplicitAccessModifiersOnDelegates, _projectItem, @"Data\ExplicitAccessModifiersOnDelegates_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertExplicitAccessModifiersOnDelegates_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnDelegates = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunInsertExplicitAccessModifiersOnDelegates, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertExplicitAccessModifiersOnDelegates_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnDelegates = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunInsertExplicitAccessModifiersOnDelegates, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunInsertExplicitAccessModifiersOnDelegates(Document document)
        {
            var codeItems = TestOperations.CodeModelManager.RetrieveAllCodeItems(document);
            var delegates = codeItems.OfType<CodeItemDelegate>().ToList();

            _insertExplicitAccessModifierLogic.InsertExplicitAccessModifiersOnDelegates(delegates);
        }

        #endregion Helpers
    }
}