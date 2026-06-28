using DA.SharedDeskPlanner.Model.Contracts;
using DA.Wpf.Framework;
using DA.Wpf.Framework.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Windows.Controls;
using Wpf.Example.Controls.MVVM;

namespace Wpf.Example.Controls
{
	/// <ChangeLog>
	/// <Create Datum="27.06.2026" Entwickler="DA" />
	/// <Change Datum="28.06.2026" Entwickler="DA">prop. IconGeometry added</Change>
	/// </ChangeLog>
	/// <summary>
	/// Interaction logic for Bookings.xaml
	/// </summary>
	[RibbonTabControl(heading:"Buchungen",position:5)]
	public partial class Bookings : UserControl, IRibbonTabControl
	{
		private readonly BookingsViewModel vm;
		public Bookings(ISharedDeskPlannerContext ctx, IDialogService dlgService)
		{
			InitializeComponent();
			vm = new BookingsViewModel(ctx, dlgService);
			DataContext = vm;
		}

		public string IconGeometry => "M6.75 3v2.25M17.25 3v2.25M3 18.75V7.5a2.25 2.25 0 0 1 2.25-2.25h13.5A2.25 2.25 0 0 1 21 7.5v11.25m-18 0A2.25 2.25 0 0 0 5.25 21h13.5A2.25 2.25 0 0 0 21 18.75m-18 0v-7.5A2.25 2.25 0 0 1 5.25 9h13.5A2.25 2.25 0 0 1 21 11.25v7.5m-12-3 2.25 2.25L15.75 12";

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
