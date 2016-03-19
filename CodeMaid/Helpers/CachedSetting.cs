using System;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A class that encapsulates caching a setting expression that can be parsed.
    /// </summary>
    public class CachedSetting<T>
        where T : class
    {
        #region Fields

        /// <summary>
        /// The cached expression.
        /// </summary>
        private string _cachedExpression;

        /// <summary>
        /// The cached result from parsing the expression.
        /// </summary>
        private T _cachedResult;

        /// <summary>
        /// The function to be executed to lookup the setting expression.
        /// </summary>
        private readonly Func<string> _lookupFunction;

        /// <summary>
        /// The function to be executed to parse a setting expression.
        /// </summary>
        private readonly Func<string, T> _parseFunction;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedSetting{T}" /> class.
        /// </summary>
        /// <param name="lookupFunction">The function to be executed to lookup the setting expression.</param>
        /// <param name="parseFunction">The function to be executed to parse a setting expression.</param>
        public CachedSetting(Func<string> lookupFunction, Func<string, T> parseFunction)
        {
            _lookupFunction = lookupFunction;
            _parseFunction = parseFunction;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Retrieves the value using the specified functions and considering the cached value.
        /// </summary>
        public T Value
        {
            get
            {
                var expression = _lookupFunction();
                if (expression != _cachedExpression)
                {
                    _cachedResult = string.IsNullOrEmpty(expression) ? default(T) : _parseFunction(expression);

                    _cachedExpression = expression;
                }

                return _cachedResult;
            }
        }

        #endregion Properties
    }
}