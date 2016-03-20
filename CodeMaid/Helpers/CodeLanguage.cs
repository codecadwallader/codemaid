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
        R,
        SCSS,
        TypeScript,
        VisualBasic,
        XAML,
        XML
    }
}