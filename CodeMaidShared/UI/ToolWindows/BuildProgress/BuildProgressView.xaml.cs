using System.Reflection;
using System.Windows;

namespace SteveCadwallader.CodeMaid.UI.ToolWindows.BuildProgress
{
    /// <summary>
    /// Interaction logic for BuildProgressView.xaml
    /// </summary>
    public partial class BuildProgressView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildProgressView" /> class.
        /// </summary>
        public BuildProgressView()
        {
            Application.ResourceAssembly = Assembly.GetExecutingAssembly();

            InitializeComponent();
        }
    }
}