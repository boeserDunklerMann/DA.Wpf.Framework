using DA.SharedDeskPlanner.Model;
using DA.SharedDeskPlanner.Model.Contracts;
using DA.Wpf.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
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
	internal class DesksViewModel(IServiceProvider serviceProvider, IDialogService dialogService) : BaseViewModel
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
				if (_selectedRoom != null)
				{
					_newDesk.Room = _selectedRoom;
					foreach (var item in _availableInventories)
					{
						if (item.IsChecked && _newDesk.Inventory != null)
							_newDesk.Inventory.Add(item.Item);
					}
					using (var dbcontext = serviceProvider.GetRequiredService<ISharedDeskPlannerContext>())
					{
						await dbcontext.Desks.AddAsync(_newDesk);
						await dbcontext.SaveChangesAsync();
					}
					await LoadDesksAsync();
					_newDesk = BaseModel.Create<Desk>();
					RaisePropChanged(nameof(NewDesk));
				}
				else
				{
					dialogService.ShowError("Ein Raum muss angegeben werden.");
				}
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
				if (_selectedDesk != null)
					using (var dbcontext = serviceProvider.GetRequiredService<ISharedDeskPlannerContext>())
					{
						dbcontext.Desks.Attach(_selectedDesk);
						_selectedDesk.Deleted = true;
						await dbcontext.SaveChangesAsync();
						await LoadDesksAsync();
					}
				else
					dialogService.ShowError($"{nameof(_selectedDesk)} nicht gesetzt.");
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
			var selectedDeskID = _selectedDesk?.ID;

			using (var dbcontext = serviceProvider.GetRequiredService<ISharedDeskPlannerContext>())
			{
				var desks = await dbcontext.Desks
			.Include(d => d.Room)
			.Include(d => d.Inventory)
			.Where(d => !d.Deleted).ToListAsync();
				_desks.Clear();
				desks.ForEach(_desks.Add);

				if (selectedDeskID != null)
					SelectedDesk = desks.FirstOrDefault(d => d.ID == selectedDeskID);

				_availableRooms.Clear();
				var rooms = await dbcontext.Rooms.Where(r => !r.Deleted).ToListAsync();
				rooms.ForEach(_availableRooms.Add);
			}
			_availableInventories.Clear();
			using (var dbcontext = serviceProvider.GetRequiredService<ISharedDeskPlannerContext>())
			{
				var inventory = await dbcontext.InventoryItems.Where(ii => !ii.Deleted).ToListAsync();
				inventory.ForEach(i =>
				{
					InventoryItem4NewDesk item = new(i);
					_availableInventories.Add(item);
				});
			}
		}
		#endregion
	}
}