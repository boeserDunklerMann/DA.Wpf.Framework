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
	/// <Create Datum="28.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// Interaction logic for Dashboard.xaml
	/// </summary>
	[RibbonTabControl(heading:"Dashboard", position:0)]
	public partial class Dashboard : UserControl, IRibbonTabControl
	{
		private readonly DashboardViewModel vm;
		public Dashboard(ISharedDeskPlannerContext ctx, IDialogService dialogService)
		{
			InitializeComponent();
			vm = new(ctx, dialogService);
			DataContext = vm;

		}

		public RibbonTabControlAttribute GetAttribute()
			=> GetType().GetCustomAttribute<RibbonTabControlAttribute>()!;

		public async Task OnInitAsync(IServiceCollection services)
		{
			await vm!.OnInitAsync();
		}

		public void OnStart() => vm!.OnStart();

		public void OnStop() => vm!.OnStop();
	}
}
