using DA.SharedDeskPlanner.Model;
using DA.SharedDeskPlanner.Model.Contracts;
using DA.Wpf.Framework;
using DA.Wpf.Framework.Attributes;
using Microsoft.Extensions.DependencyInjection;
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
		private readonly List<IRibbonTabControl> _tabControls = [];

		public MainWindow()
		{

			InitializeComponent();
			rbnMain.Items.Clear();
		}
		
		public void SetTabControls (List<IRibbonTabControl> tabControls)
		{
			_tabControls.Clear();
			_tabControls.AddRange(tabControls);
			int i = 0;
			tabControls.ForEach(ctrl =>
			{
				RibbonTabControlAttribute attr = ctrl.GetAttribute();
				if (attr.Position == i)
				{
					RibbonTab tab = new() { Header = attr.Heading };
					rbnMain.Items.Add(tab);
				}
				i++;
			});
		}

		private void rbnMain_SelectionChanged(object sender, SelectionChangedEventArgs e) 
		{
			Ribbon? ribbon = sender as Ribbon;
			if (ribbon != null && _tabControls.Count>0)
			{
				grdMain.Children.Clear();
				var ctrl = _tabControls[ribbon.SelectedIndex] as UIElement;
				var ictrl = ctrl as IRibbonTabControl;
				if (ictrl != null)
				{
					ictrl.OnStart();
					grdMain.Children.Add(ctrl);
				}
				else
					throw new NullReferenceException(nameof(ictrl));
			}
		}
}
}