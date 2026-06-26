using DA.SharedDeskPlanner.Model;
using DA.SharedDeskPlanner.Model.Contracts;
using DA.Wpf.Framework;
using DA.Wpf.Framework.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
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
		private readonly ObservableCollection<IRibbonTabControl> _tabControls = [];

		public MainWindow()
		{

			InitializeComponent();
			rbnMain.Items.Clear();
		}

		public void SetTabControls(IEnumerable<IRibbonTabControl> tabControls)
		{
			// SORTIERUNG: Das ist der entscheidende Fix!
			// Wir sortieren alle Tabs basierend auf ihrer Position, bevor wir sie verarbeiten.
			var sortedTabs = tabControls.OrderBy(c => c.GetAttribute().Position).ToList();

			_tabControls.Clear();
			sortedTabs.ForEach(_tabControls.Add);
			rbnMain.Items.Clear();
			sortedTabs.ForEach(t =>
			{
				RibbonTabControlAttribute attr = t.GetAttribute();
				RibbonTab tab = new() { Header = attr.Heading };
				rbnMain.Items.Add(tab);
			});
		}

		private void rbnMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Ribbon? ribbon = sender as Ribbon;
			if (ribbon != null && _tabControls.Count > 0)
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