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
	/// <Create Datum="26.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// Interaction logic for Rooms.xaml
	/// </summary>
	[RibbonTabControl(heading:"Räume", position:3)]
	public partial class Rooms : UserControl, IRibbonTabControl
	{
		private readonly RoomsViewModel vm;
		public Rooms(ISharedDeskPlannerContext ctx, IDialogService dlgService)
		{
			InitializeComponent();
			vm = new RoomsViewModel(ctx, dlgService);
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
