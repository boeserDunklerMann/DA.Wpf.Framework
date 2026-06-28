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
	[RibbonTabControl(heading:"Schreibtische", position:4)]
	public partial class Desks : UserControl, IRibbonTabControl
	{
		private readonly DesksViewModel vm;
		public Desks(ISharedDeskPlannerContext ctx, IDialogService dialogService)
		{
			InitializeComponent();
			vm = new(ctx, dialogService);
			DataContext = vm;
		}

		public string IconGeometry => "M3.75 5.25h16.5m-16.5 4.5h16.5m-16.5 4.5h16.5m-15 4.5v-13.5m13.5 13.5v-13.5";

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
