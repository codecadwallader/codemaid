#region CodeMaid is Copyright 2007-2010 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2010 Steve Cadwallader.

namespace SteveCadwallader.CodeMaid
{
    internal static class PkgCmdIDList
    {
        public const uint CmdIDCodeMaidCleanupActiveCode = 0x1000;
        public const uint CmdIDCodeMaidCleanupAllCode = 0x1100;
        public const uint CmdIDCodeMaidCleanupSelectedCode = 0x1200;
        public const uint CmdIDCodeMaidCloseReadOnly = 0x2000;
        public const uint CmdIDCodeMaidCollapseAllSolutionExplorer = 0x1300;
        public const uint CmdIDCodeMaidCollapseSelectedSolutionExplorer = 0x1400;
        public const uint CmdIDCodeMaidConfiguration = 0x1900;
        public const uint CmdIDCodeMaidFindInSolutionExplorer = 0x2100;
        public const uint CmdIDCodeMaidJoinLines = 0x1500;
        public const uint CmdIDCodeMaidReadOnlyToggle = 0x1600;
        public const uint CmdIDCodeMaidSnooperToolWindow = 0x1800;
        public const uint CmdIDCodeMaidSwitchFile = 0x1700;

        public const int ToolbarIDCodeMaidToolbarToolWindow = 0x1040;
    };
}