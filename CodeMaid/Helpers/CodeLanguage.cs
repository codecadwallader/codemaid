#region CodeMaid is Copyright 2007-2016 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2016 Steve Cadwallader.

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// An internal helper enumeration of code languages.
    /// </summary>
    /// <remarks>
    /// This is used to encapsulate Visual Studio reported differences we do not want to consider
    /// (e.g. JavaScript vs. JScript or HTML vs. HTMLX)
    /// </remarks>
    internal enum CodeLanguage
    {
        Unknown,
        CPlusPlus,
        CSharp,
        CSS,
        FSharp,
        HTML,
        JavaScript,
        JSON,
        LESS,
        PHP,
        PowerShell,
        SCSS,
        TypeScript,
        VisualBasic,
        XAML,
        XML
    }
}