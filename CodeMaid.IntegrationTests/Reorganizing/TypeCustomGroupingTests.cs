using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Reorganizing;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Reorganizing
{
    [TestClass]
    [DeploymentItem(@"Reorganizing\Data\TypeCustomGrouping.cs", "Data")]
    [DeploymentItem(@"Reorganizing\Data\TypeCustomGrouping_Reorganized.cs", "Data")]
    public class TypeCustomGroupingTests
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
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\TypeCustomGrouping.cs");
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
        public void ReorganizingTypeCustomGroups_ReorganizesAsExpected()
        {
            Settings.Default.Reorganizing_MemberTypeDelegates = @"Delegates||4||Delegates + Methods";
            Settings.Default.Reorganizing_MemberTypeMethods = @"Methods||4||Delegates + Methods";

            TestOperations.ExecuteCommandAndVerifyResults(RunReorganize, _projectItem, @"Data\TypeCustomGrouping_Reorganized.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void ReorganizingTypeCustomGroups_DoesNothingOnSecondPass()
        {
            Settings.Default.Reorganizing_MemberTypeDelegates = @"Delegates||4||Delegates + Methods";
            Settings.Default.Reorganizing_MemberTypeMethods = @"Methods||4||Delegates + Methods";

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunReorganize, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void ReorganizingTypeCustomGroups_DoesNothingWhenSettingsAreUnchanged()
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