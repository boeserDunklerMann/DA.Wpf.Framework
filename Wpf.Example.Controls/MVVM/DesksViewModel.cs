using DA.SharedDeskPlanner.Model;
using DA.SharedDeskPlanner.Model.Contracts;
using DA.Wpf.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Wpf.Example.Controls.MVVM
{
	/// <ChangeLog>
	/// <Create Datum="27.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	internal class DesksViewModel(ISharedDeskPlannerContext ctx, IDialogService dialogService) : BaseViewModel(ctx)
	{
		#region BaseViewModel implementation
		public override async Task OnInitAsync()
		{
			await LoadDesksAsync();
		}

		public override void OnStart()
		{
		}

		public override void OnStop()
		{
		}
		#endregion

		#region bound props
		private readonly ObservableCollection<Desk> _desks = [];
		public ObservableCollection<Desk> Desktops => _desks;
		private Desk? _selectedDesk;
		public Desk? SelectedDesk
		{
			get => _selectedDesk;
			set
			{
				_selectedDesk = value;
				RaisePropChanged(nameof(SelectedDesk));
			}
		}
		private Desk _newDesk = BaseModel.Create<Desk>();
		public Desk NewDesk => _newDesk;
		#endregion

		#region Commands
		public DelegateCommand DeleteDesk => new DelegateCommand(CmdDeleteDesk);
		#endregion

		#region Actions
		private async void CmdDeleteDesk()
		{
			try
			{
				if (dbcontext != null)
				{
					_selectedDesk!.Deleted = true;
					await dbcontext.SaveChangesAsync();
					await LoadDesksAsync();
				}
				else
					dialogService.ShowError("kein DB context vorhanden.");
			}
			catch (Exception ex)
			{
				dialogService.ShowError(ex.Message);
			}
		}
		#endregion
		#region private methods
		private async Task LoadDesksAsync()
		{
			if (dbcontext != null)
			{
				var desks = await dbcontext.Desks
					.Include(d => d.Room)
					.Where(d => !d.Deleted).ToListAsync();
				_desks.Clear();
				desks.ForEach(_desks.Add);
				RaisePropChanged(nameof(Desktops));
			}
			else
				dialogService.ShowError("kein DB context vorhanden.");
		}
		#endregion
	}
}