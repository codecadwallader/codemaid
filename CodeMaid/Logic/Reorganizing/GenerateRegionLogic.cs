using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using Thread = System.Threading.Thread;

namespace SteveCadwallader.CodeMaid.Logic.Reorganizing
{
    /// <summary>
    /// A class for encapsulating the logic of generating regions.
    /// </summary>
    internal class GenerateRegionLogic
    {
        #region Fields

        private readonly CodeMaidPackage _package;
        private readonly InsertBlankLinePaddingLogic _insertBlankLinePaddingLogic;
        private readonly RegionComparerByName _regionComparerByName;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="GenerateRegionLogic" /> class.
        /// </summary>
        private static GenerateRegionLogic _instance;

        /// <summary>
        /// Gets an instance of the <see cref="GenerateRegionLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="GenerateRegionLogic" /> class.</returns>
        internal static GenerateRegionLogic GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new GenerateRegionLogic(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateRegionLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private GenerateRegionLogic(CodeMaidPackage package)
        {
            _package = package;
            _insertBlankLinePaddingLogic = InsertBlankLinePaddingLogic.GetInstance(_package);
            _regionComparerByName = new RegionComparerByName();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// A list of possible access modifiers.
        /// </summary>
        private static IEnumerable<string> AccessModifiers => new[] { "Public", "Internal", "Protected Internal", "Protected", "Private" };

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the enumerable set of regions to be removed based on the specified code items.
        /// </summary>
        /// <param name="codeItems">The code items.</param>
        /// <returns>An enumerable set of regions to be removed.</returns>
        public IEnumerable<CodeItemRegion> GetRegionsToRemove(IEnumerable<BaseCodeItem> codeItems)
        {
            var existingRegions = codeItems.OfType<CodeItemRegion>();

            // If also inserting regions, remove all existing for more comprehensive reorganization.
            if (Settings.Default.Reorganizing_RegionsInsertNewRegions)
            {
                return existingRegions;
            }

            var composedRegions = ComposeRegionsList(codeItems);
            var regionsToRemove = existingRegions.Except(composedRegions, _regionComparerByName);

            return regionsToRemove;
        }

        /// <summary>
        /// Inserts regions per user settings.
        /// </summary>
        /// <param name="codeItems">The code items.</param>
        /// <param name="insertPoint">The default insertion point.</param>
        public void InsertRegions(IEnumerable<BaseCodeItem> codeItems, EditPoint insertPoint)
        {
            // Refresh and sort the code items.
            foreach (var codeItem in codeItems)
            {
                codeItem.RefreshCachedPositionAndName();
            }

            codeItems = codeItems.OrderBy(x => x.StartOffset).ToList();

            var regions = ComposeRegionsList(codeItems);
            var codeItemEnumerator = codeItems.GetEnumerator();
            codeItemEnumerator.MoveNext();
            EditPoint cursor = insertPoint.CreateEditPoint();

            foreach (var region in regions)
            {
                // While the current code item is a region not in the list, advance to the next code item.
                var currentCodeItemAsRegion = codeItemEnumerator.Current as CodeItemRegion;
                while (currentCodeItemAsRegion != null && !regions.Contains(currentCodeItemAsRegion, _regionComparerByName))
                {
                    cursor = codeItemEnumerator.Current.EndPoint;
                    codeItemEnumerator.MoveNext();
                    currentCodeItemAsRegion = codeItemEnumerator.Current as CodeItemRegion;
                }

                // If the current code item is this region, advance to the next code item and end this iteration.
                if (_regionComparerByName.Equals(currentCodeItemAsRegion, region))
                {
                    cursor = codeItemEnumerator.Current.EndPoint;
                    codeItemEnumerator.MoveNext();
                    continue;
                }

                // Update the cursor position to the current code item.
                if (codeItemEnumerator.Current != null)
                {
                    cursor = codeItemEnumerator.Current.StartPoint;
                }

                // If the current code item is a region, offset the position by 1 to workaround points not tracking for region types.
                if (currentCodeItemAsRegion != null)
                {
                    cursor = cursor.CreateEditPoint();
                    currentCodeItemAsRegion.StartPoint.CharRight();
                    currentCodeItemAsRegion.EndPoint.CharRight();
                }

                // Insert the #region tag
                cursor = InsertRegionTag(region, cursor);

                // Keep jumping forwards in code items as long as there's matches.
                while (CodeItemBelongsInRegion(codeItemEnumerator.Current, region))
                {
                    cursor = codeItemEnumerator.Current.EndPoint;
                    codeItemEnumerator.MoveNext();
                }

                // Insert the #endregion tag
                cursor = InsertEndRegionTag(region, cursor);

                // If the current code item is a region, reverse offset of the position.
                if (currentCodeItemAsRegion != null)
                {
                    currentCodeItemAsRegion.StartPoint.CharLeft();
                    currentCodeItemAsRegion.EndPoint.CharLeft();
                }
            }
        }

        /// <summary>
        /// Inserts a #region tag for the specified region preceding the specified start point.
        /// </summary>
        /// <param name="region">The region to start.</param>
        /// <param name="startPoint">The starting point.</param>
        /// <returns>The updated cursor.</returns>
        public EditPoint InsertRegionTag(CodeItemRegion region, EditPoint startPoint)
        {
            var cursor = startPoint.CreateEditPoint();

            // If the cursor is not preceeded only by whitespace, insert a new line.
            var firstNonWhitespaceIndex = cursor.GetLine().TakeWhile(char.IsWhiteSpace).Count();
            if (cursor.DisplayColumn > firstNonWhitespaceIndex + 1)
            {
                cursor.Insert(Environment.NewLine);
            }

            cursor.Insert($"{RegionHelper.GetRegionTagText(cursor, region.Name)}{Environment.NewLine}");

            startPoint.SmartFormat(cursor);

            region.StartPoint = cursor.CreateEditPoint();
            region.StartPoint.LineUp();
            region.StartPoint.StartOfLine();

            var regionWrapper = new[] { region };
            _insertBlankLinePaddingLogic.InsertPaddingBeforeRegionTags(regionWrapper);
            _insertBlankLinePaddingLogic.InsertPaddingAfterRegionTags(regionWrapper);

            return cursor;
        }

        /// <summary>
        /// Inserts an #endregion tag for the specified region following the specified end point.
        /// </summary>
        /// <param name="region">The region to end.</param>
        /// <param name="endPoint">The end point.</param>
        /// <returns>The updated cursor.</returns>
        public EditPoint InsertEndRegionTag(CodeItemRegion region, EditPoint endPoint)
        {
            var cursor = endPoint.CreateEditPoint();

            // If the cursor is not preceeded only by whitespace, insert a new line.
            var firstNonWhitespaceIndex = cursor.GetLine().TakeWhile(char.IsWhiteSpace).Count();
            if (cursor.DisplayColumn > firstNonWhitespaceIndex + 1)
            {
                cursor.Insert(Environment.NewLine);
            }

            cursor.Insert(RegionHelper.GetEndRegionTagText(cursor));

            if (Settings.Default.Cleaning_UpdateEndRegionDirectives &&
                RegionHelper.LanguageSupportsUpdatingEndRegionDirectives(cursor))
            {
                cursor.Insert(" " + region.Name);
            }

            // If the cursor is not followed only by whitespace, insert a new line.
            var lastNonWhitespaceIndex = cursor.GetLine().TrimEnd().Length;
            if (cursor.DisplayColumn < lastNonWhitespaceIndex + 1)
            {
                cursor.Insert(Environment.NewLine);
                cursor.LineUp();
                cursor.EndOfLine();
            }

            endPoint.SmartFormat(cursor);

            region.EndPoint = cursor.CreateEditPoint();

            var regionWrapper = new[] { region };
            _insertBlankLinePaddingLogic.InsertPaddingBeforeEndRegionTags(regionWrapper);
            _insertBlankLinePaddingLogic.InsertPaddingAfterEndRegionTags(regionWrapper);

            return cursor;
        }

        /// <summary>
        /// Determines if the specified code item belongs in the specified region.
        /// </summary>
        /// <param name="codeItem">The code item.</param>
        /// <param name="region">The region.</param>
        /// <returns>True if the specified code item belongs in the specified region, otherwise false.</returns>
        private bool CodeItemBelongsInRegion(BaseCodeItem codeItem, CodeItemRegion region)
        {
            return codeItem != null && _regionComparerByName.Equals(region, ComposeRegionForCodeItem(codeItem));
        }

        /// <summary>
        /// Composes a list of regions that should be present for the specified set of code items based on user settings.
        /// </summary>
        /// <param name="codeItems">The code items.</param>
        /// <returns>An enumerable set of regions that should be present.</returns>
        private IEnumerable<CodeItemRegion> ComposeRegionsList(IEnumerable<BaseCodeItem> codeItems)
        {
            return Settings.Default.Reorganizing_RegionsInsertKeepEvenIfEmpty
                ? ComposeAllPossibleRegionsList()
                : ComposePresentTypesRegionsList(codeItems);
        }

        /// <summary>
        /// Composes a list of all possible regions.
        /// </summary>
        /// <returns>An enumerable set of regions.</returns>
        private IEnumerable<CodeItemRegion> ComposeAllPossibleRegionsList()
        {
            var regions = new List<CodeItemRegion>();

            var types = MemberTypeSettingHelper.AllSettings.GroupBy(x => x.Order).Select(y => new List<MemberTypeSetting>(y)).OrderBy(z => z[0].Order);
            foreach (var type in types)
            {
                if (Settings.Default.Reorganizing_RegionsIncludeAccessLevel)
                {
                    regions.AddRange(AccessModifiers.Select(x => new CodeItemRegion { Name = x + " " + type[0].EffectiveName }));
                }
                else
                {
                    regions.Add(new CodeItemRegion { Name = type[0].EffectiveName });
                }
            }

            return regions;
        }

        /// <summary>
        /// Composes a list of regions based on the specified code items.
        /// </summary>
        /// <param name="codeItems">The code items.</param>
        /// <returns>An enumerable set of regions.</returns>
        private IEnumerable<CodeItemRegion> ComposePresentTypesRegionsList(IEnumerable<BaseCodeItem> codeItems)
        {
            var regions = new HashSet<CodeItemRegion>(_regionComparerByName);

            foreach (var codeItem in codeItems)
            {
                var region = ComposeRegionForCodeItem(codeItem);
                if (region != null)
                {
                    regions.Add(region);
                }
                else
                {
                    region = codeItem as CodeItemRegion;
                    if (region != null)
                    {
                        // Add an existing region to the list iff it has a child whose composed region name would match it.
                        var childrenRegions = ComposePresentTypesRegionsList(region.Children);
                        if (childrenRegions.Contains(region, _regionComparerByName))
                        {
                            regions.Add(region);
                        }
                    }
                }
            }

            return regions;
        }

        /// <summary>
        /// Composes a region based on the specified code item.
        /// </summary>
        /// <param name="codeItem">The code item.</param>
        /// <returns>A region.</returns>
        private CodeItemRegion ComposeRegionForCodeItem(BaseCodeItem codeItem)
        {
            if (codeItem == null) return null;

            var setting = MemberTypeSettingHelper.LookupByKind(codeItem.Kind);
            if (setting == null) return null;

            var regionName = string.Empty;

            if (Settings.Default.Reorganizing_RegionsIncludeAccessLevel)
            {
                var element = codeItem as BaseCodeItemElement;
                if (element != null)
                {
                    var accessModifier = CodeElementHelper.GetAccessModifierKeyword(element.Access);
                    if (accessModifier != null)
                    {
                        regionName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(accessModifier) + " ";
                    }
                }
            }

            regionName += setting.EffectiveName;

            return new CodeItemRegion { Name = regionName };
        }

        #endregion Methods
    }
}