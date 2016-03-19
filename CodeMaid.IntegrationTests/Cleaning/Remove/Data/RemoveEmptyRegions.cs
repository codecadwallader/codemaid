namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove.Data
{
    internal class Regions
    {
        #region Region One
        #endregion
        #region Region Two
        #region Nested Region
        #endregion
        #endregion
        #region Region Three
        #endregion
        #region Region Four
        #region Region Four.One
        #region Region Four.One.One
        #region Region Four.One.One.One
        #endregion
        #region Region Four.One.One.Two
        #endregion
        #endregion
        #region Region Four.One.Two
        #endregion
        #region Region Four.One.Three


        #endregion
        #endregion
        #region Region Four.Two
   
                
        #endregion
        #endregion
        #region Region with a comment
        // Region should remain
        #endregion
        #region Region with a nested region with a comment
        #region Nested region with a comment
        // Region and parent region should remain
        #endregion
        #region Other nested regions should still be removed
        #region Including nested nested
        #endregion
        #endregion
        #endregion
    }
}