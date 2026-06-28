using DA.Wpf.Framework;
using DA.Wpf.Framework.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Windows.Controls;
using Wpf.Example.Controls.MVVM;

namespace Wpf.Example.Controls
{
	/// <ChangeLog>
	/// <Create Datum="26.06.2026" Entwickler="DA" />
	/// <Change Datum="28.06.2026" Entwickler="DA">prop. IconGeometry added</Change>
	/// </ChangeLog>
	/// <summary>
	/// Interaction logic for Inventory.xaml
	/// </summary>
	[RibbonTabControl(heading:"Inventar", position:2)]
	public partial class Inventory : UserControl, IRibbonTabControl
	{
		private readonly InventoryViewModel vm;
		public Inventory(IServiceProvider serviceProvider, IDialogService dlgService)
		{
			InitializeComponent();
			vm = new InventoryViewModel(serviceProvider, dlgService);
			DataContext = vm;
		}

		public string IconGeometry => "m21 7.5-9-5.25L3 7.5m18 0-9 5.25m9-5.25v9l-9 5.25M3 7.5l9 5.25M3 7.5v9l9 5.25M12 12.75v9";

		public RibbonTabControlAttribute GetAttribute()
			=> GetType().GetCustomAttribute<RibbonTabControlAttribute>()!;
		

		public async Task OnInitAsync(IServiceCollection services)
		{
			await vm.OnInitAsync();
		}

		public void OnStart()
		{
			vm.OnStart();
		}

		public void OnStop()
		{
			vm.OnStop();
		}
	}
}
