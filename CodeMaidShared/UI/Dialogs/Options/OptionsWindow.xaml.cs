using System.Reflection;
using System.Windows;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsWindow" /> class.
        /// </summary>
        public OptionsWindow()
        {
            Application.ResourceAssembly = Assembly.GetExecutingAssembly();

            HasMaximizeButton = true;

            InitializeComponent();
        }
    }
}