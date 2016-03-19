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
    [DeploymentItem(@"Cleaning\Insert\Data\ExplicitAccessModifiersOnInterfaces.cs", "Data")]
    [DeploymentItem(@"Cleaning\Insert\Data\ExplicitAccessModifiersOnInterfaces_Cleaned.cs", "Data")]
    public class ExplicitAccessModifiersOnInterfacesTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\ExplicitAccessModifiersOnInterfaces.cs");
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
        public void CleaningInsertExplicitAccessModifiersOnInterfaces_CleansAsExpected()
        {
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnInterfaces = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunInsertExplicitAccessModifiersOnInterfaces, _projectItem, @"Data\ExplicitAccessModifiersOnInterfaces_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertExplicitAccessModifiersOnInterfaces_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnInterfaces = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunInsertExplicitAccessModifiersOnInterfaces, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertExplicitAccessModifiersOnInterfaces_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_InsertExplicitAccessModifiersOnInterfaces = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunInsertExplicitAccessModifiersOnInterfaces, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunInsertExplicitAccessModifiersOnInterfaces(Document document)
        {
            var codeItems = TestOperations.CodeModelManager.RetrieveAllCodeItems(document);
            var interfaces = codeItems.OfType<CodeItemInterface>().ToList();

            _insertExplicitAccessModifierLogic.InsertExplicitAccessModifiersOnInterfaces(interfaces);
        }

        #endregion Helpers
    }
}