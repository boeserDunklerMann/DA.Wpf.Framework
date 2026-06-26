using DA.SharedDeskPlanner.Model.Contracts;
using DA.Wpf.Framework;
using DA.Wpf.Framework.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Wpf.Example.Controls.MVVM;

namespace Wpf.Example.Controls
{
	/// <ChangeLog>
	/// <Create Datum="08.05.2026" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// Interaction logic for UserListCtrl.xaml
	/// </summary>
	[RibbonTabControl(heading:"Benutzer", position:0)]
	public partial class UserListCtrl : UserControl, IRibbonTabControl
	{
		private UserListViewModel? vm;
		public UserListCtrl(ISharedDeskPlannerContext ctx)
		{
			InitializeComponent();
			vm = new UserListViewModel(ctx);
			DataContext = vm;
		}

		public RibbonTabControlAttribute GetAttribute()
		{
			return GetType().GetCustomAttribute<RibbonTabControlAttribute>()!;
		}

		public void OnInit(IServiceCollection services)
		{
			services.AddTransient<UserListViewModel>();
			vm!.OnInit();
		}

		public void OnStart() => vm!.OnStart();

		public void OnStop() => vm!.OnStop();
	}
}