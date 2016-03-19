using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Reorganizing;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Reorganizing
{
    [TestClass]
    [DeploymentItem(@"Reorganizing\Data\TypeCustomOrder.cs", "Data")]
    [DeploymentItem(@"Reorganizing\Data\TypeCustomOrder_Reorganized.cs", "Data")]
    public class TypeCustomOrderTests
    {
        #region Setup

        private static CodeReorganizationManager _codeReorganizationManager;
        private ProjectItem _projectItem;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _codeReorganizationManager = CodeReorganizationManager.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_codeReorganizationManager);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\TypeCustomOrder.cs");
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
        public void ReorganizingTypeCustomOrder_ReorganizesAsExpected()
        {
            Settings.Default.Reorganizing_MemberTypeDestructors = @"Destructors||14||Destructors";
            Settings.Default.Reorganizing_MemberTypeFields = @"Fields||13||Fields";

            TestOperations.ExecuteCommandAndVerifyResults(RunReorganize, _projectItem, @"Data\TypeCustomOrder_Reorganized.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void ReorganizingTypeCustomOrder_DoesNothingOnSecondPass()
        {
            Settings.Default.Reorganizing_MemberTypeDestructors = @"Destructors||14||Destructors";
            Settings.Default.Reorganizing_MemberTypeFields = @"Fields||13||Fields";

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunReorganize, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void ReorganizingTypeCustomOrder_DoesNothingWhenSettingsAreUnchanged()
        {
            TestOperations.ExecuteCommandAndVerifyNoChanges(RunReorganize, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunReorganize(Document document)
        {
            _codeReorganizationManager.Reorganize(document);
        }

        #endregion Helpers
    }
}