#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

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
        private static IEnumerable<string> AccessModifiers
        {
            get { return new[] { "Public", "Internal", "Protected Internal", "Protected", "Private" }; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the enumerable set of regions to be removed based on the specified code items.
        /// </summary>
        /// <param name="codeItems">The code items.</param>
        /// <returns>An enumerable set of regions to be removed.</returns>
        public IEnumerable<CodeItemRegion> GetRegionsToRemove(IEnumerable<BaseCodeItem> codeItems)
        {
            var regionsToKeep = ComposeRegionsList(codeItems);

            var existingRegions = codeItems.OfType<CodeItemRegion>();
            var regionsToRemove = existingRegions.Except(regionsToKeep, _regionComparerByName);

            return regionsToRemove;
        }

        /// <summary>
        /// Inserts regions per user settings.
        /// </summary>
        /// <param name="codeItems">The code items.</param>
        public void InsertRegions(IEnumerable<BaseCodeItem> codeItems)
        {
            var regionsToExist = ComposeRegionsList(codeItems);

            var existingRegions = codeItems.OfType<CodeItemRegion>();
            var regionsToInsert = regionsToExist.Except(existingRegions, _regionComparerByName);

            CodeItemRegion currentRegion = null;
            BaseCodeItem lastCodeItem = null;

            foreach (var codeItem in codeItems)
            {
                var region = ComposeRegionForCodeItem(codeItem);
                if (_regionComparerByName.Equals(currentRegion, region))
                {
                    lastCodeItem = codeItem;
                    continue;
                }

                if (currentRegion != null)
                {
                    InsertEndRegionTag(currentRegion, lastCodeItem.EndPoint);
                    currentRegion = null;
                }

                if (region != null && regionsToInsert.Contains(region, _regionComparerByName))
                {
                    currentRegion = region;
                    lastCodeItem = codeItem;

                    InsertRegionTag(currentRegion, codeItem.StartPoint);
                    regionsToInsert = regionsToInsert.Except(new[] { region }, _regionComparerByName);
                }
            }

            if (currentRegion != null)
            {
                InsertEndRegionTag(currentRegion, lastCodeItem.EndPoint);
            }

            //TODO: When finished, dump all of the other remaining regions in.. or insert them somehow.. reorganization is done at this point, so items *should* be ordered.
        }

        /// <summary>
        /// Inserts a #region tag for the specified region preceding the specified start point.
        /// </summary>
        /// <param name="region">The region to start.</param>
        /// <param name="startPoint">The starting point.</param>
        private void InsertRegionTag(CodeItemRegion region, EditPoint startPoint)
        {
            region.StartPoint = startPoint.CreateEditPoint();
            region.StartPoint.Insert(string.Format("#region {0}{1}", region.Name, Environment.NewLine));
            region.StartPoint.StartOfLine();
            region.StartPoint.SmartFormat(startPoint);

            var regionWrapper = new[] { region };
            _insertBlankLinePaddingLogic.InsertPaddingBeforeRegionTags(regionWrapper);
            _insertBlankLinePaddingLogic.InsertPaddingAfterRegionTags(regionWrapper);
        }

        /// <summary>
        /// Inserts an #endregion tag for the specified region following the specified end point.
        /// </summary>
        /// <param name="region">The region to end.</param>
        /// <param name="endPoint">The end point.</param>
        private void InsertEndRegionTag(CodeItemRegion region, EditPoint endPoint)
        {
            region.EndPoint = endPoint.CreateEditPoint();
            region.EndPoint.Insert(string.Format("{0}#endregion {1}", Environment.NewLine, region.Name));
            region.EndPoint.EndOfLine();
            region.EndPoint.SmartFormat(endPoint);

            var regionWrapper = new[] { region };
            _insertBlankLinePaddingLogic.InsertPaddingBeforeEndRegionTags(regionWrapper);
            _insertBlankLinePaddingLogic.InsertPaddingAfterEndRegionTags(regionWrapper);
        }

        /// <summary>
        /// Composes a list of regions that should be present for the specified set of code items based on user settings.
        /// </summary>
        /// <param name="codeItems">The code items.</param>
        /// <returns>An enumerable set of regions that should be present.</returns>
        private IEnumerable<CodeItemRegion> ComposeRegionsList(IEnumerable<BaseCodeItem> codeItems)
        {
            return Settings.Default.Reorganizing_RegionsInsertEvenIfEmpty
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
                        regions.UnionWith(ComposePresentTypesRegionsList(region.Children));
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