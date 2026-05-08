using DA.Wpf.Framework;
using DA.Wpf.Framework.Attributes;
using Microsoft.EntityFrameworkCore;
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
		public UserListCtrl()
		{
			InitializeComponent();
			vm = DataContext as UserListViewModel;
		}

		public FrameworkElement CreateMyself() => new UserListCtrl();

		public RibbonTabControlAttribute GetAttribute()
		{
			return GetType().GetCustomAttribute<RibbonTabControlAttribute>()!;
		}

		public void OnInit(DbContext ctx) => vm!.OnInit();

		public void OnStart() => vm!.OnStart();

		public void OnStop() => vm!.OnStop();
	}
}