using DA.SharedDeskPlanner.Model.Contracts;
using DA.Wpf.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wpf.Example.Controls.MVVM
{
	/// <ChangeLog>
	/// <Create Datum="28.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	internal class DashboardViewModel(ISharedDeskPlannerContext ctx, IDialogService dialogService) : BaseViewModel(ctx)
	{
		public override async Task OnInitAsync()
		{
			await Task.CompletedTask;
		}

		public override void OnStart()
		{
		}

		public override void OnStop()
		{
		}
	}
}