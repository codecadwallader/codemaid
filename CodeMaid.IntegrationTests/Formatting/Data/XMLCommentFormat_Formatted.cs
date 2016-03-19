namespace SteveCadwallader.CodeMaid.IntegrationTests.Formatting.Data
{
    public class XMLCommentFormat
    {
        /// <summary>
        /// Firsts the method. This method has a really long description that will cover over
        /// multiple lines without any regard for the duration expected of a traditional comment that
        /// has more respect for being at a reasonable length instead of an exceedingly long length.
        /// </summary>
        /// <param name="param1">The first parameter.</param>
        /// <param name="param2">The second parameter.</param>
        /// <returns>True if the method has a true result, otherwise false.</returns>
        public bool FirstMethod(string param1, string param2)
        {
            return false;
        }

        /// <summary>
        /// The method summary with a long description that just goes on for a long period of time
        /// that will exceed the line boundary.
        /// </summary>
        /// <remarks>
        /// A remarks section that has a very long description that just goes on for a long period of
        /// time that will exceed the line boundary.
        /// </remarks>
        /// <example>
        /// An example that has a very long description that just goes on for a long period of time
        /// that will exceed the line boundary.
        /// </example>
        /// <param name="isParam">
        /// A parameter that has a very long description that just goes on for a long period of time
        /// that will exceed the line boundary.
        /// </param>
        /// <returns>
        /// A return statement that has a very long description that just goes on for a long period
        /// of time that will exceed the line boundary.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// An exception that has a very long description that just goes on for a long period of time
        /// that will exceed the line boundary.
        /// </exception>
        public bool SecondMethod(bool isParam)
        {
            return false;
        }

        /// <summary>
        /// <see cref="CommentFormatting"/><see cref="CommentFormatting"/>
        /// </summary>
        public void ThirdMethod()
        {
        }

        /// <summary>
        /// The summary for the comment.
        /// </summary>
        /// <param name="maxSize">Max number of elements returned in <paramref name="array"/></param>
        /// <param name="array">The array.</param>
        public void FourthMethod(int maxSize, int[] array)
        {
        }

        /// <summary>
        /// This comment contains <c>XMLCommentFormat</c> an example of the c tag.
        /// </summary>
        public void FifthMethod()
        {
        }

        /// <summary>
        /// This comment would only have a single word to wrap, which is wasteful and should be ignored.
        /// </summary>
        public void SixthMethod()
        {
        }

        /// <summary>
        /// </summary>
        public void SeventhMethod()
        {
        }
    }
}