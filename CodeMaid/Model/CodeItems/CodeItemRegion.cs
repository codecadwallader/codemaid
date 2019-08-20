using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    static class CodeItemRegionDefaults
    {
        public static bool GetDefaultIsExpandedFor(string name)
        {
            if (IsExpanded.ContainsKey(name))
                return IsExpanded[name];
            return true;
        }

        public static void SetDefaultIsExpandedFor(string name, bool isExpanded)
        {
            IsExpanded[name] = isExpanded;
        }

        static Dictionary<string, bool> IsExpanded = new Dictionary<string, bool>();
    }

    /// <summary>
    /// The representation of a code region.
    /// </summary>
    public class CodeItemRegion : BaseCodeItem, ICodeItemParent
    {
        #region Fields

        private bool _isExpanded = true;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeItemRegion" /> class.
        /// </summary>
        public CodeItemRegion()
        {
            Children = new SetCodeItems();
        }

        #endregion Constructors

        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind => KindCodeItem.Region;

        #endregion BaseCodeItem Overrides

        #region Implementation of ICodeItemParent

        /// <summary>
        /// An event raised when the IsExpanded state has changed.
        /// </summary>
        public event EventHandler IsExpandedChanged;

        /// <summary>
        /// Gets the children of this code item, may be empty.
        /// </summary>
        public SetCodeItems Children { get; private set; }

        /// <summary>
        /// Gets the insert point, may be null.
        /// </summary>
        public EditPoint InsertPoint
        {
            get
            {
                var startPoint = StartPoint;
                if (startPoint != null)
                {
                    var insertPoint = startPoint.CreateEditPoint();
                    insertPoint.LineDown();
                    return insertPoint;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if this parent item is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    RaisePropertyChanged();

                    IsExpandedChanged?.Invoke(this, EventArgs.Empty);
                    CodeItemRegionDefaults.SetDefaultIsExpandedFor(this.Name, _isExpanded);
                }
            }
        }

        #endregion Implementation of ICodeItemParent

        #region Properties

        /// <summary>
        /// Gets a flag indicating if this region is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if (Children.Any())
                {
                    return false;
                }

                var start = StartPoint.CreateEditPoint();
                start.EndOfLine();

                var end = EndPoint.CreateEditPoint();
                end.StartOfLine();

                var text = start.GetText(end);

                return string.IsNullOrWhiteSpace(text);
            }
        }

        /// <summary>
        /// Gets or sets a flag indicating if this region has been invalidated.
        /// </summary>
        public bool IsInvalidated { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if this is a pseudo group.
        /// </summary>
        public bool IsPseudoGroup { get; set; }

        #endregion Properties
    }
}