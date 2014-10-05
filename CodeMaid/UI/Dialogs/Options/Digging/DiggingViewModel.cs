#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Model.CodeTree;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Digging
{
    /// <summary>
    /// The view model for digging options.
    /// </summary>
    public class DiggingViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DiggingViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        public DiggingViewModel(CodeMaidPackage package)
            : base(package)
        {
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header
        {
            get { return "Digging (Spade)"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            CenterOnWhole = Settings.Default.Digging_CenterOnWhole;
            ComplexityAlertThreshold = Settings.Default.Digging_ComplexityAlertThreshold;
            ComplexityWarningThreshold = Settings.Default.Digging_ComplexityWarningThreshold;
            IndentationMargin = Settings.Default.Digging_IndentationMargin;
            PrimarySortOrder = (CodeSortOrder)Settings.Default.Digging_PrimarySortOrder;
            SecondarySortTypeByName = Settings.Default.Digging_SecondarySortTypeByName;
            ShowItemComplexity = Settings.Default.Digging_ShowItemComplexity;
            ShowItemMetadata = Settings.Default.Digging_ShowItemMetadata;
            ShowMethodParameters = Settings.Default.Digging_ShowMethodParameters;
            SynchronizeOutlining = Settings.Default.Digging_SynchronizeOutlining;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.Digging_CenterOnWhole = CenterOnWhole;
            Settings.Default.Digging_ComplexityAlertThreshold = ComplexityAlertThreshold;
            Settings.Default.Digging_ComplexityWarningThreshold = ComplexityWarningThreshold;
            Settings.Default.Digging_IndentationMargin = IndentationMargin;
            Settings.Default.Digging_PrimarySortOrder = (int)PrimarySortOrder;
            Settings.Default.Digging_SecondarySortTypeByName = SecondarySortTypeByName;
            Settings.Default.Digging_ShowItemComplexity = ShowItemComplexity;
            Settings.Default.Digging_ShowItemMetadata = ShowItemMetadata;
            Settings.Default.Digging_ShowMethodParameters = ShowMethodParameters;
            Settings.Default.Digging_SynchronizeOutlining = SynchronizeOutlining;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets the flag indicating if the view should center on the whole item upon navigation.
        /// </summary>
        public bool CenterOnWhole
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the complexity alert threshold.
        /// </summary>
        public int ComplexityAlertThreshold
        {
            get { return GetPropertyValue<int>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the complexity warning threshold.
        /// </summary>
        public int ComplexityWarningThreshold
        {
            get { return GetPropertyValue<int>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the indentation margin.
        /// </summary>
        public int IndentationMargin
        {
            get { return GetPropertyValue<int>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the primary sort order.
        /// </summary>
        public CodeSortOrder PrimarySortOrder
        {
            get { return GetPropertyValue<CodeSortOrder>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if secondary sorting during type sort should be on name.
        /// </summary>
        public bool SecondarySortTypeByName
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if item complexity should be shown.
        /// </summary>
        public bool ShowItemComplexity
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if item metadata should be shown.
        /// </summary>
        public bool ShowItemMetadata
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if method parameters should be shown.
        /// </summary>
        public bool ShowMethodParameters
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if outlining should be synchronized with the code file.
        /// </summary>
        public bool SynchronizeOutlining
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        #endregion Options
    }
}