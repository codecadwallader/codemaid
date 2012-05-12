#region CodeMaid is Copyright 2007-2012 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2012 Steve Cadwallader.

using System.Collections.Generic;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Options
{
    /// <summary>
    /// A set of extensions methods for enumerable sets of <see cref="OptionsPageViewModel"/>.
    /// </summary>
    public static class OptionsPageViewModelEnumerableExtensions
    {
        /// <summary>
        /// Flattens the specified collection by pulling out children.
        /// </summary>
        /// <remarks>
        /// This currently only recurses down one level as that's all that is needed.
        /// </remarks>
        /// <param name="pages">The initial pages collection.</param>
        /// <returns>The flattened collection including children.</returns>
        public static IEnumerable<OptionsPageViewModel> Flatten(this IEnumerable<OptionsPageViewModel> pages)
        {
            return pages.Union(pages.SelectMany(x => x.Children));
        }
    }
}