using DA.SharedDeskPlanner.Model.Contracts;
using DA.Wpf.Framework;
using DA.Wpf.Framework.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Example.Controls.MVVM;

namespace Wpf.Example.Controls
{
	/// <ChangeLog>
	/// <Create Datum="28.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// Interaction logic for AddBooking.xaml
	/// </summary>
	[RibbonTabControl(heading:"neue Buchung vornehmen", position:1)]
	public partial class AddBooking : UserControl, IRibbonTabControl
	{
		private readonly AddBookingViewModel vm;
		public AddBooking(IServiceProvider serviceProvider, IDialogService dialogService)
		{
			InitializeComponent();
			vm = new(serviceProvider, dialogService);
			DataContext = vm;
		}

		public string IconGeometry => "";

		public RibbonTabControlAttribute GetAttribute()
			=> GetType().GetCustomAttribute<RibbonTabControlAttribute>()!;

		public async Task OnInitAsync(IServiceCollection services)
		{
			await vm.OnInitAsync();
		}

		public void OnStart() => vm.OnStart();

		public void OnStop() => vm.OnStop();

	}
}
