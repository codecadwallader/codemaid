using EnvDTE;
using System;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A static helper class for working with regions.
    /// </summary>
    internal static class RegionHelper
    {
        #region Internal Methods

        internal static string GetRegionTagText(EditPoint editPoint, string name = null)
        {
            var codeLanguage = editPoint.Parent.GetCodeLanguage();
            switch (codeLanguage)
            {
                case CodeLanguage.CSharp:
                    return "#region " +
                           (name ?? string.Empty);

                case CodeLanguage.VisualBasic:
                    return "#Region " +
                           (name != null ? $"\"{name}\"" : string.Empty);

                default:
                    throw new NotImplementedException($"Regions are not supported for '{codeLanguage}'.");
            }
        }

        internal static string GetEndRegionTagText(EditPoint editPoint)
        {
            var codeLanguage = editPoint.Parent.GetCodeLanguage();
            switch (codeLanguage)
            {
                case CodeLanguage.CSharp:
                    return "#endregion";

                case CodeLanguage.VisualBasic:
                    return "#End Region";

                default:
                    throw new NotImplementedException($"Regions are not supported for '{codeLanguage}'.");
            }
        }

        internal static bool LanguageSupportsUpdatingEndRegionDirectives(EditPoint editPoint)
        {
            var codeLanguage = editPoint.Parent.GetCodeLanguage();

            switch (codeLanguage)
            {
                case CodeLanguage.CSharp:
                    return true;

                default:
                    return false;
            }
        }

        #endregion Internal Methods
    }
}