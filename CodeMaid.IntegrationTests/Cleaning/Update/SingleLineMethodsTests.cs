using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using System.Linq;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Update
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Update\Data\SingleLineMethods.cs", "Data")]
    [DeploymentItem(@"Cleaning\Update\Data\SingleLineMethods_Cleaned.cs", "Data")]
    public class SingleLineMethodsTests
    {
        #region Setup

        private static UpdateLogic _updateLogic;
        private ProjectItem _projectItem;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _updateLogic = UpdateLogic.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_updateLogic);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\SingleLineMethods.cs");
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
        public void CleaningUpdateSingleLineMethods_CleansAsExpected()
        {
            Settings.Default.Cleaning_UpdateSingleLineMethods = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunUpdateSingleLineMethods, _projectItem, @"Data\SingleLineMethods_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningUpdateSingleLineMethods_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_UpdateSingleLineMethods = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunUpdateSingleLineMethods, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningUpdateSingleLineMethods_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_UpdateSingleLineMethods = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunUpdateSingleLineMethods, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunUpdateSingleLineMethods(Document document)
        {
            var codeItems = TestOperations.CodeModelManager.RetrieveAllCodeItems(document);
            var methods = codeItems.OfType<CodeItemMethod>().ToList();

            _updateLogic.UpdateSingleLineMethods(methods);
        }

        #endregion Helpers
    }
}