namespace SteveCadwallader.CodeMaid
{
    using System;

    /// <summary>
    /// Helper class that exposes all GUIDs used across VS Package.
    /// </summary>
    internal sealed partial class PackageGuids
    {
        public const string GuidVisualStudioCSSWindowContextMenuString = "a764e896-518d-11d2-9a89-00c04f79efc3";
        public const string GuidVisualStudioCSSDocumentContextMenuString = "64da400e-b4ad-4d67-aa92-4b7acb01ecd5";
        public const string GuidVisualStudioHTMLWindowContextMenuString = "d7e8c5e1-bdb8-11d0-9c88-0000f8040a53";
        public const string GuidVisualStudioHTMLDocumentContextMenuString = "78f03954-2fb8-4087-8ce7-59d71710b3bb";
        public const string GuidVisualStudioJSONDocumentContextMenuString = "f718ca06-cf4f-4a0c-9106-e79e9ee5e7cd";
        public const string GuidVisualStudioXAMLWindowContextMenuString = "4c87b692-1202-46aa-b64c-ef01faec53da";
        public const string GuidCodeMaidPackageString = "4c82e17d-927e-42d2-8460-b473ac7df316";
        public const string GuidCodeMaidOutputPaneString = "4e7ba904-9311-4dd0-abe5-a61c1739780f";
        public const string GuidCodeMaidMenuSetString = "369b04df-0688-4074-911d-d0d6c6a31632";
        public const string GuidCodeMaidImageMonikerString = "e4c3c7f4-7250-4f02-b07b-575a320898ab";
        public const string GuidImageCleanupString = "54aa78c8-285a-4166-9eba-369dde5d787e";
        public const string GuidImageCleanupAllString = "e3d926ae-03d7-4c69-8226-440eeddab292";
        public const string GuidImageCommentFormatString = "512526cf-2bb2-47fc-88af-dbd3678fe428";
        public const string GuidImageInfoString = "0c75ffd4-f452-47c7-a36b-3179b6d6309f";
        public const string GuidImageJoinString = "e6e6dcd8-accc-4552-890b-a68ef0cf7279";
        public const string GuidImageOptionsString = "a81c0e9b-bfdf-4b2f-a7ba-ca7e3183658a";
        public const string GuidImageSortAlphaString = "735bc8ba-0eaf-4fa3-a8ce-6f2e23c615b6";
        public static Guid GuidVisualStudioCSSWindowContextMenu = new Guid(GuidVisualStudioCSSWindowContextMenuString);
        public static Guid GuidVisualStudioCSSDocumentContextMenu = new Guid(GuidVisualStudioCSSDocumentContextMenuString);
        public static Guid GuidVisualStudioHTMLWindowContextMenu = new Guid(GuidVisualStudioHTMLWindowContextMenuString);
        public static Guid GuidVisualStudioHTMLDocumentContextMenu = new Guid(GuidVisualStudioHTMLDocumentContextMenuString);
        public static Guid GuidVisualStudioJSONDocumentContextMenu = new Guid(GuidVisualStudioJSONDocumentContextMenuString);
        public static Guid GuidVisualStudioXAMLWindowContextMenu = new Guid(GuidVisualStudioXAMLWindowContextMenuString);
        public static Guid GuidCodeMaidPackage = new Guid(GuidCodeMaidPackageString);
        public static Guid GuidCodeMaidOutputPane = new Guid(GuidCodeMaidOutputPaneString);
        public static Guid GuidCodeMaidMenuSet = new Guid(GuidCodeMaidMenuSetString);
        public static Guid GuidCodeMaidImageMoniker = new Guid(GuidCodeMaidImageMonikerString);
        public static Guid GuidImageCleanup = new Guid(GuidImageCleanupString);
        public static Guid GuidImageCleanupAll = new Guid(GuidImageCleanupAllString);
        public static Guid GuidImageCommentFormat = new Guid(GuidImageCommentFormatString);
        public static Guid GuidImageInfo = new Guid(GuidImageInfoString);
        public static Guid GuidImageJoin = new Guid(GuidImageJoinString);
        public static Guid GuidImageOptions = new Guid(GuidImageOptionsString);
        public static Guid GuidImageSortAlpha = new Guid(GuidImageSortAlphaString);
    }
    /// <summary>
    /// Helper class that encapsulates all CommandIDs uses across VS Package.
    /// </summary>
    internal sealed partial class PackageIds
    {
        public const int IDVisualStudioCSSWindowContextMenu = 0x0102;
        public const int IDVisualStudioCSSDocumentContextMenu = 0x0002;
        public const int IDVisualStudioASPXWindowContextMenu = 0x0035;
        public const int IDVisualStudioASPXCodeWindowContextMenu = 0x0036;
        public const int IDVisualStudioHTMLWindowContextMenu = 0x0033;
        public const int IDVisualStudioScriptWindowContextMenu = 0x0034;
        public const int IDVisualStudioHTMLDocumentContextMenu = 0x0001;
        public const int IDVisualStudioJSONDocumentContextMenu = 0x0003;
        public const int IDVisualStudioXAMLWindowContextMenu = 0x0103;
        public const int GroupIDCodeMaidMenuBaseGroup = 0x1020;
        public const int GroupIDCodeMaidMenuCleanupGroup = 0x1021;
        public const int GroupIDCodeMaidMenuExtendGroup = 0x1023;
        public const int GroupIDCodeMaidMenuMetaGroup = 0x1024;
        public const int GroupIDCodeMaidContextSolutionNodeGroup = 0x1050;
        public const int GroupIDCodeMaidContextSolutionFolderGroup = 0x1051;
        public const int GroupIDCodeMaidContextProjectNodeGroup = 0x1052;
        public const int GroupIDCodeMaidContextFolderNodeGroup = 0x1053;
        public const int GroupIDCodeMaidContextItemNodeGroup = 0x1054;
        public const int GroupIDCodeMaidContextCrossProjectSolutionProjectGroup = 0x1055;
        public const int GroupIDCodeMaidContextCrossProjectSolutionItemGroup = 0x1056;
        public const int GroupIDCodeMaidContextCrossProjectProjectItemGroup = 0x1057;
        public const int GroupIDCodeMaidContextCrossProjectMultiProjectGroup = 0x1058;
        public const int GroupIDCodeMaidContextCrossProjectMultiItemGroup = 0x1059;
        public const int GroupIDCodeMaidContextCrossProjectMultiSolutionFolderGroup = 0x105A;
        public const int GroupIDCodeMaidContextCrossProjectMultiProjectFolderGroup = 0x105B;
        public const int GroupIDCodeMaidContextDocumentTabGroup = 0x1070;
        public const int GroupIDCodeMaidContextASPXWindowGroup = 0x1080;
        public const int GroupIDCodeMaidContextASPXCodeWindowGroup = 0x1081;
        public const int GroupIDCodeMaidContextCodeWindowGroup = 0x1082;
        public const int GroupIDCodeMaidContextCSSWindowGroup = 0x1083;
        public const int GroupIDCodeMaidContextCSSDocumentGroup = 0x1084;
        public const int GroupIDCodeMaidContextHTMLWindowGroup = 0x1085;
        public const int GroupIDCodeMaidContextHTMLDocumentGroup = 0x1086;
        public const int GroupIDCodeMaidContextJSONDocumentGroup = 0x1087;
        public const int GroupIDCodeMaidContextScriptWindowGroup = 0x1088;
        public const int GroupIDCodeMaidContextXAMLWindowGroup = 0x1089;
        public const int MenuIDCodeMaidContextASPXWindowMenu = 0x1090;
        public const int MenuIDCodeMaidContextASPXCodeWindowMenu = 0x1091;
        public const int MenuIDCodeMaidContextCodeWindowMenu = 0x1092;
        public const int MenuIDCodeMaidContextCSSWindowMenu = 0x1093;
        public const int MenuIDCodeMaidContextCSSDocumentMenu = 0x1094;
        public const int MenuIDCodeMaidContextHTMLWindowMenu = 0x1095;
        public const int MenuIDCodeMaidContextHTMLDocumentMenu = 0x1096;
        public const int MenuIDCodeMaidContextJSONDocumentMenu = 0x1097;
        public const int MenuIDCodeMaidContextScriptWindowMenu = 0x1098;
        public const int MenuIDCodeMaidContextXAMLWindowMenu = 0x1099;
        public const int GroupIDCodeMaidContextASPXWindowSubGroup = 0x10A0;
        public const int GroupIDCodeMaidContextASPXCodeWindowSubGroup = 0x10A1;
        public const int GroupIDCodeMaidContextCodeWindowSubGroup = 0x10A2;
        public const int GroupIDCodeMaidContextCSSWindowSubGroup = 0x10A3;
        public const int GroupIDCodeMaidContextCSSDocumentSubGroup = 0x10A4;
        public const int GroupIDCodeMaidContextHTMLWindowSubGroup = 0x10A5;
        public const int GroupIDCodeMaidContextHTMLDocumentSubGroup = 0x10A6;
        public const int GroupIDCodeMaidContextJSONDocumentSubGroup = 0x10A7;
        public const int GroupIDCodeMaidContextScriptWindowSubGroup = 0x10A8;
        public const int GroupIDCodeMaidContextXAMLWindowSubGroup = 0x10A9;
        public const int CmdIDCodeMaidAbout = 0x1000;
        public const int CmdIDCodeMaidCleanupActiveCode = 0x1200;
        public const int CmdIDCodeMaidCleanupAllCode = 0x1300;
        public const int CmdIDCodeMaidCleanupOpenCode = 0x1400;
        public const int CmdIDCodeMaidCleanupSelectedCode = 0x1500;
        public const int CmdIDCodeMaidCommentFormat = 0x1900;
        public const int CmdIDCodeMaidJoinLines = 0x2100;
        public const int CmdIDCodeMaidOptions = 0x2200;
        public const int CmdIDCodeMaidSettingCleanupOnSave = 0x2600;
        public const int CmdIDCodeMaidSortLines = 0x2700;
        public const int IconCleanup = 0x0001;
        public const int IconCleanupAll = 0x0001;
        public const int IconCommentFormat = 0x0001;
        public const int IconJoin = 0x0001;
        public const int IconOptions = 0x0001;
        public const int IconSortAlpha = 0x0001;
    }
}
