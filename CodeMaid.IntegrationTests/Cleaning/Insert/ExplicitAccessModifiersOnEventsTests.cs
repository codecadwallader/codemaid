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
    [DeploymentItem(@"Cleaning\Insert\Data\ExplicitAccessModifiersOnEvents.cs", "Data")]
    [DeploymentItem(@"Cleaning\Insert\Data\ExplicitAccessModifiersOnEvents_Cleaned.cs", "Data")]
    public class ExplicitAccessModifiersOnEventsTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\ExplicitAccessModifiersOnEvents.cs");
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
        public void CleaningInsertExplicitAccessModifiersOnEvents_CleansAsExpected()
        {
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnEvents = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunInsertExplicitAccessModifiersOnEvents, _projectItem, @"Data\ExplicitAccessModifiersOnEvents_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertExplicitAccessModifiersOnEvents_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnEvents = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunInsertExplicitAccessModifiersOnEvents, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertExplicitAccessModifiersOnEvents_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnEvents = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunInsertExplicitAccessModifiersOnEvents, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunInsertExplicitAccessModifiersOnEvents(Document document)
        {
            var codeItems = TestOperations.CodeModelManager.RetrieveAllCodeItems(document);
            var events = codeItems.OfType<CodeItemEvent>().ToList();

            _insertExplicitAccessModifierLogic.InsertExplicitAccessModifiersOnEvents(events);
        }

        #endregion Helpers
    }
}