using DA.Wpf.Framework;
using DA.Wpf.Framework.Attributes;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf.Example
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private List<IRibbonTabControl> tabControls = [];
		public MainWindow()
		{
			InitializeComponent();
			var ctrls = Common.GetRibbonTabControlsAsync().Result;
			rbnMain.Items.Clear();
			tabControls.Clear();
			for (int i = 0; i < ctrls.Count; i++)
			{
				IRibbonTabControl ctrl = ctrls[i];
				RibbonTabControlAttribute attr = ctrl.GetAttribute();
				if (attr.Position == i)
				{
					RibbonTab tab = new() { Header = attr.Heading };
					rbnMain.Items.Add(tab);
					tabControls.Add(ctrl);
				}
			}
		}
		private void rbnMain_SelectionChanged(object sender, SelectionChangedEventArgs e) 
		{
			Ribbon? ribbon = sender as Ribbon;
			if (ribbon != null)
			{
				grdMain.Children.Clear();
				var ctrl = tabControls[ribbon.SelectedIndex] as UIElement;
				grdMain.Children.Add(ctrl);
			}
		}
}
}