using System.Collections.Generic;
using System.Linq;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options
{
    /// <summary>
    /// A set of extensions methods for enumerable sets of <see cref="OptionsPageViewModel" />.
    /// </summary>
    public static class OptionsPageViewModelEnumerableExtensions
    {
        /// <summary>
        /// Flattens the specified collection by pulling out children.
        /// </summary>
        /// <remarks>This currently only recurses down one level as that's all that is needed.</remarks>
        /// <param name="pages">The initial pages collection.</param>
        /// <returns>The flattened collection including children.</returns>
        public static IEnumerable<OptionsPageViewModel> Flatten(this IEnumerable<OptionsPageViewModel> pages)
        {
            return pages.Union(pages.SelectMany(x => x.Children));
        }
    }
}