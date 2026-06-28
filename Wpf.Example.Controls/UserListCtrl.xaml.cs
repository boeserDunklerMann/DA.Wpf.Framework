using DA.Wpf.Framework;
using DA.Wpf.Framework.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Windows.Controls;
using Wpf.Example.Controls.MVVM;

namespace Wpf.Example.Controls
{
	/// <ChangeLog>
	/// <Create Datum="08.05.2026" Entwickler="DA" />
	/// <Change Datum="28.06.2026" Entwickler="DA">prop. IconGeometry added</Change>
	/// </ChangeLog>
	/// <summary>
	/// Interaction logic for UserListCtrl.xaml
	/// </summary>
	[RibbonTabControl(heading:"Benutzer", position:0)]
	public partial class UserListCtrl : UserControl, IRibbonTabControl
	{
		private UserListViewModel? vm;
		public UserListCtrl(IServiceProvider serviceProvider, IDialogService dialogService)
		{
			InitializeComponent();
			vm = new UserListViewModel(serviceProvider, dialogService);
			DataContext = vm;
		}

		public string IconGeometry => "M15.75 6a3.75 3.75 0 1 1-7.5 0 3.75 3.75 0 0 1 7.5 0ZM4.501 20.118a7.5 7.5 0 0 1 14.998 0A17.933 17.933 0 0 1 12 21.75c-2.676 0-5.216-.584-7.499-1.632Z";

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