using DA.SharedDeskPlanner.Model;
using DA.SharedDeskPlanner.Model.Contracts;
using DA.Wpf.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Text;
using Wpf.Example.Controls.Model;

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
		
		private readonly ObservableCollection<Room> _availableRooms = [];
		public ObservableCollection<Room> AvailableRooms => _availableRooms;
		private Room? _selectedRoom;
		public Room? SelectedRoom
		{
			get => _selectedRoom;
			set
			{
				_selectedRoom = value;
				RaisePropChanged(nameof(SelectedRoom));
			}
		}

		private readonly ObservableCollection<InventoryItem4NewDesk> _availableInventories = [];
		public ObservableCollection<InventoryItem4NewDesk> AvailableInventories => _availableInventories;

		#endregion

		#region Commands
		public DelegateCommand DeleteDesk => new DelegateCommand(CmdDeleteDesk);
		public DelegateCommand CreateDesk => new DelegateCommand(CmdCreateDesk);
		#endregion

		#region Actions
		private async void CmdCreateDesk()
		{
			try
			{
				if (dbcontext != null)
				{
					if (_selectedRoom != null)
					{
						_newDesk.Room = _selectedRoom;
						foreach (var item in _availableInventories)
						{
							if (item.IsChecked && _newDesk.Inventory != null)
								_newDesk.Inventory.Add(item.Item);
						}
						await dbcontext.Desks.AddAsync(_newDesk);
						await dbcontext.SaveChangesAsync();
						await LoadDesksAsync();
						_newDesk = BaseModel.Create<Desk>();
						RaisePropChanged(nameof(NewDesk));
					}
					else
					{
						dialogService.ShowError("Ein Raum muss angegeben werden.");
					}
				}
				else
					dialogService.ShowError("kein DB Context vorhanden");
			}
			catch (Exception ex)
			{
				dialogService.ShowError(ex.Message);
			}
		}

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
				var selectedDeskID = _selectedDesk?.ID;

				var desks = await dbcontext.Desks
					.Include(d => d.Room)
					.Include(d => d.Inventory)
					.Where(d => !d.Deleted).ToListAsync();
				_desks.Clear();
				desks.ForEach(_desks.Add);
				RaisePropChanged(nameof(Desktops));

				if (selectedDeskID != null)
					SelectedDesk = desks.FirstOrDefault(d => d.ID == selectedDeskID);

				_availableRooms.Clear();
				var rooms = await dbcontext.Rooms.Where(r => !r.Deleted).ToListAsync();
				rooms.ForEach(_availableRooms.Add);
				RaisePropChanged(nameof(AvailableRooms));

				_availableInventories.Clear();
				var inventory = await dbcontext.InventoryItems.Where(ii => !ii.Deleted).ToListAsync();
				inventory.ForEach(i =>
				{
					InventoryItem4NewDesk item = new(i);
					_availableInventories.Add(item);
				});
				RaisePropChanged(nameof(AvailableInventories));
			}
			else
				dialogService.ShowError("kein DB context vorhanden.");
		}
		#endregion
	}
}