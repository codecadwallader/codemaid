namespace SteveCadwallader.CodeMaid.IntegrationTests.Reorganizing.Data
{
    public static class PackageGuids
    {
        #region Fields

        public const string GuidCodeMaidToolWindowBuildProgressString = "260978c3-582c-487d-ab12-c1fdde07c578";
        public const string GuidCodeMaidToolWindowSpadeString = "75d09b86-471e-4b30-8720-362d13ad0a45";
        public static readonly Guid GuidCodeMaidMenuSolutionExplorerGroup = new Guid("d69f1580-274f-4d12-b13a-c365c759de66");
        public static readonly Guid GuidCodeMaidMenuVisualizeGroup = new Guid("a4ef7624-6477-4860-85bc-46564429f935");
        public static readonly Guid GuidCodeMaidToolWindowBuildProgress = new Guid(GuidCodeMaidToolWindowBuildProgressString);
        public static readonly Guid GuidCodeMaidToolWindowSpade = new Guid(GuidCodeMaidToolWindowSpadeString);

        #endregion Fields
    }

    public class RegionsInsertAfterReorder
    {
        #region Fields

        public const int MenuIDCodeMaidContextSpade = 0x1060;
        public const int ToolbarIDCodeMaidToolbarSpade = 0x1040;

        #endregion Fields
    }
}