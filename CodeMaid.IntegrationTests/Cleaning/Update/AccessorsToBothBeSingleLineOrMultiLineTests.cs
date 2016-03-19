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
    [DeploymentItem(@"Cleaning\Update\Data\AccessorsToBothBeSingleLineOrMultiLine.cs", "Data")]
    [DeploymentItem(@"Cleaning\Update\Data\AccessorsToBothBeSingleLineOrMultiLine_Cleaned.cs", "Data")]
    public class AccessorsToBothBeSingleLineOrMultiLineTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\AccessorsToBothBeSingleLineOrMultiLine.cs");
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
        public void CleaningUpdateAccessorsToBothBeSingleLineOrMultiLine_CleansAsExpected()
        {
            Settings.Default.Cleaning_UpdateAccessorsToBothBeSingleLineOrMultiLine = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunUpdateAccessorsToBothBeSingleLineOrMultiLine, _projectItem, @"Data\AccessorsToBothBeSingleLineOrMultiLine_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningUpdateAccessorsToBothBeSingleLineOrMultiLine_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_UpdateAccessorsToBothBeSingleLineOrMultiLine = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunUpdateAccessorsToBothBeSingleLineOrMultiLine, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningUpdateAccessorsToBothBeSingleLineOrMultiLine_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_UpdateAccessorsToBothBeSingleLineOrMultiLine = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunUpdateAccessorsToBothBeSingleLineOrMultiLine, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunUpdateAccessorsToBothBeSingleLineOrMultiLine(Document document)
        {
            var codeItems = TestOperations.CodeModelManager.RetrieveAllCodeItems(document);
            var events = codeItems.OfType<CodeItemEvent>().ToList();
            var properties = codeItems.OfType<CodeItemProperty>().ToList();

            _updateLogic.UpdateEventAccessorsToBothBeSingleLineOrMultiLine(events);
            _updateLogic.UpdatePropertyAccessorsToBothBeSingleLineOrMultiLine(properties);
        }

        #endregion Helpers
    }
}