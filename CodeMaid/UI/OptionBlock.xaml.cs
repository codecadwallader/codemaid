using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SteveCadwallader.CodeMaid.UI
{
    /// <summary>
    /// Interaction logic for OptionBlock.xaml
    /// </summary>
    public partial class OptionBlock : UserControl
    {
        #region Constructors

        public OptionBlock()
        {
            InitializeComponent();
        }

        #endregion

        #region CheckBoxStaticIsChecked (Dependency Property)

        /// <summary>
        /// The dependency property definition for the CheckBoxStaticIsChecked property.
        /// </summary>
        public static readonly DependencyProperty CheckBoxStaticIsCheckedProperty = DependencyProperty.Register(
            "CheckBoxStaticIsChecked", typeof(bool), typeof(OptionBlock),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Gets or sets the flag indicating if the control has static checked.
        /// </summary>
        public bool CheckBoxStaticIsChecked
        {
            get
            {
                return (bool)GetValue(CheckBoxStaticIsCheckedProperty);
            }
            set
            {
                SetValue(CheckBoxStaticIsCheckedProperty, value);
            }
        }

        #endregion

    }
}
