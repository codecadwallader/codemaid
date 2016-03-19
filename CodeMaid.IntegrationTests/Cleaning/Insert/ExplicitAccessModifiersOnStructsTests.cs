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
    [DeploymentItem(@"Cleaning\Insert\Data\ExplicitAccessModifiersOnStructs.cs", "Data")]
    [DeploymentItem(@"Cleaning\Insert\Data\ExplicitAccessModifiersOnStructs_Cleaned.cs", "Data")]
    public class ExplicitAccessModifiersOnStructsTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\ExplicitAccessModifiersOnStructs.cs");
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
        public void CleaningInsertExplicitAccessModifiersOnStructs_CleansAsExpected()
        {
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnStructs = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunInsertExplicitAccessModifiersOnStructs, _projectItem, @"Data\ExplicitAccessModifiersOnStructs_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertExplicitAccessModifiersOnStructs_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnStructs = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunInsertExplicitAccessModifiersOnStructs, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertExplicitAccessModifiersOnStructs_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnStructs = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunInsertExplicitAccessModifiersOnStructs, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunInsertExplicitAccessModifiersOnStructs(Document document)
        {
            var codeItems = TestOperations.CodeModelManager.RetrieveAllCodeItems(document);
            var structs = codeItems.OfType<CodeItemStruct>().ToList();

            _insertExplicitAccessModifierLogic.InsertExplicitAccessModifiersOnStructs(structs);
        }

        #endregion Helpers
    }
}