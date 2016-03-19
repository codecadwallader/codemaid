namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove.Data
{
    internal class Regions
    {

        #region Region with a comment
        // Region should remain
        #endregion
        #region Region with a nested region with a comment
        #region Nested region with a comment
        // Region and parent region should remain
        #endregion

        #endregion
    }
}