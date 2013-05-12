#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VsSDK.UnitTestLibrary;

namespace SteveCadwallader.CodeMaid.UnitTests
{
    [TestClass]
    public class PackageTest
    {
        [TestMethod]
        public void CreateInstance()
        {
            var package = new CodeMaidPackage();
            Assert.IsNotNull(package);
        }

        [TestMethod]
        public void IsIVsPackage()
        {
            var package = new CodeMaidPackage();
            Assert.IsNotNull(package as IVsPackage, "The object does not implement IVsPackage");
        }

        [TestMethod]
        public void SetSite()
        {
            // Create the package
            var package = new CodeMaidPackage() as IVsPackage;
            Assert.IsNotNull(package, "The object does not implement IVsPackage");

            // Create a basic service provider
            OleServiceProvider serviceProvider = OleServiceProvider.CreateOleServiceProviderWithBasicServices();

            // Site the package
            Assert.AreEqual(0, package.SetSite(serviceProvider), "SetSite did not return S_OK");

            // Unsite the package
            Assert.AreEqual(0, package.SetSite(null), "SetSite(null) did not return S_OK");
        }
    }
}