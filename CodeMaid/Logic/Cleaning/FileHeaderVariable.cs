using EnvDTE;
using System;

namespace SteveCadwallader.CodeMaid.Logic.Cleaning
{
    /// <summary>
    /// Represents a dynamic value that can be substituted into a file header by the file header update logic.
    /// </summary>
    public class FileHeaderVariable
    {
        private readonly Func<string,string> _getReplacementValue;

        /// <summary>
        /// Constructs a new instance of the <see cref="FileHeaderVariable"/> class.
        /// </summary>
        /// <param name="name">The string value that represents this variable in the file header settings</param>
        /// <param name="matchPattern">A pattern that matches this variable's value in a file header.</param>
        /// <param name="getReplacementValue">A function to generate a replacement value.</param>
        public FileHeaderVariable(string name, string matchPattern, Func<string,string> getReplacementValue)
        {
            Name = name;
            MatchPattern = matchPattern;
            _getReplacementValue = getReplacementValue;
        }

        /// <summary>
        /// Gets the string value that represents this variable in the File Header Settings.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// A regular expression that matches this variable in the document text's file header.
        /// </summary>
        public string MatchPattern { get; }

        /// <summary>
        /// Generates a value to replace this variable with when updating the file header.
        /// </summary>
        /// <param name="currentValue">The current value of the variable in the file header.</param>
        /// <returns>The value that should replace the variable</returns>
        public string GetReplacementValue(string currentValue) => _getReplacementValue(currentValue);
    }
}
