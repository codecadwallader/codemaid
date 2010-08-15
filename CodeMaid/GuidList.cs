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

using System;

namespace SteveCadwallader.CodeMaid
{
    internal static class GuidList
    {
        // Package level guids.
        public const string GuidCodeMaidPackageString = "4c82e17d-927e-42d2-8460-b473ac7df316";
        public static readonly Guid GuidCodeMaidPackage = new Guid(GuidCodeMaidPackageString);

        // Menu group guids.
        public static readonly Guid GuidCodeMaidMenuBaseGroup = new Guid("186512ff-6c4e-42c9-b9a1-032f35109993");
        public static readonly Guid GuidCodeMaidMenuCleanupGroup = new Guid("eb1c95fd-1b99-47ab-913a-62e7f69a0319");
        public static readonly Guid GuidCodeMaidMenuDocumentGroup = new Guid("5c29c8d8-681c-4cf3-8481-cc3ee484639a");
        public static readonly Guid GuidCodeMaidMenuExtendGroup = new Guid("4527c6ae-82e7-4fd8-9755-43bfbcfd2a61");
        public static readonly Guid GuidCodeMaidMenuMetaGroup = new Guid("590f8d08-eb82-444c-bee2-4652d3f30145");
        public static readonly Guid GuidCodeMaidMenuSolutionExplorerGroup = new Guid("d69f1580-274f-4d12-b13a-c365c759de66");
        public static readonly Guid GuidCodeMaidMenuVisualizeGroup = new Guid("a4ef7624-6477-4860-85bc-46564429f935");

        // Toolbar menu group guids.
        public static readonly Guid GuidCodeMaidToolbarSolutionExplorerGroup = new Guid("ed596e0f-d1cb-46e7-8dd3-741758451f27");
        public static readonly Guid GuidCodeMaidToolbarToolWindowBaseGroup = new Guid("93e0e5d3-d335-4c12-ae8b-bccccdcd1eda");
        public static readonly Guid GuidCodeMaidToolbarToolWindowCleanupGroup = new Guid("c4640933-0ac0-485c-875f-4aaa94fdd8d0");
        public static readonly Guid GuidCodeMaidToolbarToolWindowExtendGroup = new Guid("e5e5ca6a-7cdd-473f-9d55-9d726e220bd3");
        public static readonly Guid GuidCodeMaidToolbarToolWindowMetaGroup = new Guid("e56ddcf8-dc5f-45cb-84f5-c88d2c8e556b");

        // Miscellaneous context menu group guids.
        public static readonly Guid GuidCodeMaidContextCodeWindowGroup = new Guid("29039753-26f9-4548-b81c-d7baa5ebc661");
        public static readonly Guid GuidCodeMaidContextDocumentTabGroup = new Guid("9cba7ef0-ce80-42bb-9f25-c69a9710d328");

        // Solution explorer context menu group guids.
        public static readonly Guid GuidCodeMaidContextSolutionNodeGroup = new Guid("eda37cf1-ff29-46aa-ba07-05abf7ba6596");
        public static readonly Guid GuidCodeMaidContextSolutionFolderGroup = new Guid("39de3da3-f28c-41fc-85c8-6d0fa801f3f1");
        public static readonly Guid GuidCodeMaidContextProjectNodeGroup = new Guid("a04159ed-c5ee-4146-954a-4e662b297513");
        public static readonly Guid GuidCodeMaidContextFolderNodeGroup = new Guid("9979d6f0-4af6-446a-a9ff-b84e27452435");
        public static readonly Guid GuidCodeMaidContextItemNodeGroup = new Guid("36669853-51f0-4da1-828d-d43d67b8450c");

        // Command guids.
        public static readonly Guid GuidCodeMaidCommandAbout = new Guid("369b04df-0688-4074-911d-d0d6c6a31632");
        public static readonly Guid GuidCodeMaidCommandCleanupActiveCode = new Guid("36de540f-25cb-4151-957f-d63a5a3a10a7");
        public static readonly Guid GuidCodeMaidCommandCleanupAllCode = new Guid("eb2efdb6-2efe-405c-94af-383d36ad58dd");
        public static readonly Guid GuidCodeMaidCommandCleanupSelectedCode = new Guid("b2979ac6-2853-442c-8df6-21637af5130e");
        public static readonly Guid GuidCodeMaidCommandCloseAllReadOnly = new Guid("170a0d22-19ea-452c-839a-fb33535abbb7");
        public static readonly Guid GuidCodeMaidCommandCollapseAllSolutionExplorer = new Guid("f0b58f42-6f6e-49db-8787-411979a4a4e3");
        public static readonly Guid GuidCodeMaidCommandCollapseSelectedSolutionExplorer = new Guid("ed5ec7f2-c011-43c4-9a02-4f7fb907c734");
        public static readonly Guid GuidCodeMaidCommandConfiguration = new Guid("56718f89-01d2-4405-99b5-4aa665af5947");
        public static readonly Guid GuidCodeMaidCommandFindInSolutionExplorer = new Guid("9e7a4c5b-5de4-418c-8ede-71fcfa4e811f");
        public static readonly Guid GuidCodeMaidCommandJoinLines = new Guid("7912f0a2-ada9-4888-9d2f-cde09cd310c6");
        public static readonly Guid GuidCodeMaidCommandReadOnlyToggle = new Guid("f8a69cbc-1c88-4229-8a28-eac0e7e55c30");
        public static readonly Guid GuidCodeMaidCommandSnooperToolWindow = new Guid("aaf6a75d-2ccc-4081-b1ab-76306653807c");
        public static readonly Guid GuidCodeMaidCommandSwitchFile = new Guid("8cd97d41-8750-4dbb-9e89-eaa91620b078");
    };
}