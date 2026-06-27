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
