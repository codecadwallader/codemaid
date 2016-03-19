using EnvDTE;
using EnvDTE80;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A helper class for identifying explicit interface implementations.
    /// </summary>
    /// <remarks>
    /// These regular expression based matches may cause false positives, but that is considered
    /// preferable to false negatives. A future implementation built against Roslyn will hopefully
    /// be more precise.
    /// </remarks>
    public static class ExplicitInterfaceImplementationHelper
    {
        /// <summary>
        /// Determines if the specified code event is an explicit interface implementation.
        /// </summary>
        /// <param name="codeEvent">The code event.</param>
        /// <returns>True if an explicit interface implementation, otherwise false.</returns>
        public static bool IsExplicitInterfaceImplementation(CodeEvent codeEvent)
        {
            // In some VS editions, the name may be reported including the interface name.
            if (codeEvent.Name.Contains("."))
            {
                return true;
            }

            // Otherwise, look for the element name with a preceding dot.
            var declaration = CodeElementHelper.GetEventDeclaration(codeEvent);
            var matchString = @"\." + codeEvent.Name;

            return RegexNullSafe.IsMatch(declaration, matchString);
        }

        /// <summary>
        /// Determines if the specified code function is an explicit interface implementation.
        /// </summary>
        /// <param name="codeFunction">The code function.</param>
        /// <returns>True if an explicit interface implementation, otherwise false.</returns>
        public static bool IsExplicitInterfaceImplementation(CodeFunction2 codeFunction)
        {
            // In some VS editions, the name may be reported including the interface name.
            if (codeFunction.Name.Contains("."))
            {
                return true;
            }

            // Otherwise, look for the element name with a preceding dot.
            var declaration = CodeElementHelper.GetMethodDeclaration(codeFunction);
            var matchString = @"\." + codeFunction.Name;

            return RegexNullSafe.IsMatch(declaration, matchString);
        }

        /// <summary>
        /// Determines if the specified code property is an explicit interface implementation.
        /// </summary>
        /// <param name="codeProperty">The code property.</param>
        /// <returns>True if an explicit interface implementation, otherwise false.</returns>
        public static bool IsExplicitInterfaceImplementation(CodeProperty codeProperty)
        {
            // In some VS editions, the name may be reported including the interface name.
            if (codeProperty.Name.Contains("."))
            {
                return true;
            }

            // Otherwise, look for the element name with a preceding dot.
            var declaration = CodeElementHelper.GetPropertyDeclaration(codeProperty);
            var matchString = @"\." + codeProperty.Name;

            return RegexNullSafe.IsMatch(declaration, matchString);
        }
    }
}