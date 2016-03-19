namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    public class BlankLinePaddingAfterEndRegionTags
    {
        #region Region One
        #endregion

        #region Region Two
        #region Nested Region
        #endregion

        #endregion

        #region Region Three
        #endregion
    }
}
