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

using System;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using SteveCadwallader.CodeMaid.Integration;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Helpers
{
    public static class TestEnvironment
    {
        /// <summary>
        /// Gets the <see cref="CodeMaidPackage"/>, loading it into the shell if not already present.
        /// </summary>
        /// <returns>The <see cref="CodeMaidPackage"/> instance.</returns>
        public static CodeMaidPackage GetCodeMaidPackage()
        {
            IVsShell shellService = (IVsShell)VsIdeTestHostContext.ServiceProvider.GetService(typeof(SVsShell));
            Guid packageGuid = new Guid(GuidList.GuidCodeMaidPackageString);
            IVsPackage package;

            shellService.IsPackageLoaded(ref packageGuid, out package);

            if (package == null)
            {
                shellService.LoadPackage(ref packageGuid, out package);
            }

            var codeMaidPackage = (CodeMaidPackage)package;

            Assert.IsNotNull(codeMaidPackage);

            return codeMaidPackage;
        }
    }
}