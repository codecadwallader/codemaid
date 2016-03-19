using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VsSDK.UnitTestLibrary;

namespace SteveCadwallader.CodeMaid.UnitTests.Helpers
{
    internal static class CodeMaidPackageHelper
    {
        public static CodeMaidPackage CreateInitializedPackage()
        {
            // Create the package.
            var package = new CodeMaidPackage();

            // Create a basic service provider.
            var serviceProvider = OleServiceProvider.CreateOleServiceProviderWithBasicServices();

            // Add UIShell service that knows how to create a toolwindow.
            BaseMock uiShellService = UIShellServiceMock.GetUiShellInstanceCreateToolWin();
            serviceProvider.AddService(typeof(SVsUIShell), uiShellService, false);

            // Site the package
            Assert.AreEqual(0, ((IVsPackage)package).SetSite(serviceProvider), "SetSite did not return S_OK");

            return package;
        }
    }
}