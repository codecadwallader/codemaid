using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A static helper class for working with regions.
    /// </summary>
    internal static class RegionHelper
    {
        #region Internal Methods

        internal static string GetRegionTagText(EditPoint editPoint)
        {
            switch (editPoint.Parent.GetCodeLanguage())
            {
                case CodeLanguage.VisualBasic:
                    return "#Region";

                default:
                    return "#region";
            }
        }

        internal static string GetEndRegionTagText(EditPoint editPoint)
        {
            switch (editPoint.Parent.GetCodeLanguage())
            {
                case CodeLanguage.VisualBasic:
                    return "#End Region";

                default:
                    return "#endregion";
            }
        }

        #endregion Internal Methods
    }
}