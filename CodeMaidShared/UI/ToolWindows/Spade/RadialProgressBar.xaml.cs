using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace SteveCadwallader.CodeMaid.UI.ToolWindows.Spade
{
    public partial class RadialProgressBar : UserControl
    {
        public RadialProgressBar()
        {
            Application.ResourceAssembly = Assembly.GetExecutingAssembly();

            InitializeComponent();
        }
    }
}