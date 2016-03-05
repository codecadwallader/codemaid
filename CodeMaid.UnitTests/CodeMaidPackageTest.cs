#region CodeMaid is Copyright 2007-2016 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2016 Steve Cadwallader.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.UnitTests.Helpers;

namespace SteveCadwallader.CodeMaid.UnitTests
{
    [TestClass]
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