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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests
{
    [TestClass]
    public class CleaningRemoveTests
    {
        #region Setup

        private static CodeMaidPackage _package;
        private static RemoveWhitespaceLogic _removeWhitespaceLogic;

        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void TestClassInitialize(TestContext testContext)
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                _package = CodeMaidPackageHelper.GetCodeMaidPackage();
                _removeWhitespaceLogic = RemoveWhitespaceLogic.GetInstance(_package);
            }));
        }

        #endregion Setup

        #region Tests

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveEndOfLineWhitespace_AsExpected()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                Settings.Default.Cleaning_RemoveEndOfLineWhitespace = true;

                _removeWhitespaceLogic.RemoveEOLWhitespace(null);
            }));
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveEndOfLineWhitespace_DoesNothingOnSecondPass()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                Settings.Default.Cleaning_RemoveEndOfLineWhitespace = true;

                _removeWhitespaceLogic.RemoveEOLWhitespace(null);
            }));
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveEndOfLineWhitespace_DoesNothingWhenSettingIsDisabled()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                Settings.Default.Cleaning_RemoveEndOfLineWhitespace = false;

                _removeWhitespaceLogic.RemoveEOLWhitespace(null);
            }));
        }

        #endregion Tests
    }
}