using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.UnitTests.Helpers;

namespace SteveCadwallader.CodeMaid.UnitTests
{
    [TestClass]
    [Ignore]
    //TODO: Disabled while experimenting with VS2017 RC.
    public class CodeMaidPackageTest
    {
        [TestMethod]
        public void CreateCodeMaidPackage()
        {
            var package = CodeMaidPackageHelper.CreateInitializedPackage();
            Assert.IsNotNull(package);
        }
    }
}