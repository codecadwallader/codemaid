#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

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