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
	/// <Create Datum="26.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	internal class RoomsViewModel(ISharedDeskPlannerContext ctx, IDialogService dialog) : BaseViewModel(ctx)
	{
		private readonly ObservableCollection<Room> _rooms = [];
		public ObservableCollection<Room> Rooms => _rooms;
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
		private Room _newRoom = BaseModel.Create<Room>();
		public Room NewRoom => _newRoom;

		#region Overrides from BaseViewModel
		public override async Task OnInitAsync()
		{
			await LoadRoomsAsync();
		}

		public override async void OnStart()
		{
			await Task.CompletedTask;
		}

		public override async void OnStop()
		{
			await Task.CompletedTask;
		}
		#endregion
		#region private methods
		private async Task LoadRoomsAsync()
		{
			if (dbcontext != null)
			{
				var rooms = await dbcontext.Rooms
					.Include(nameof(Room.Desks))
					.Where(r => !r.Deleted).ToListAsync();
				_rooms.Clear();
				rooms.ForEach(_rooms.Add);
				RaisePropChanged(nameof(Rooms));
			}
			else
				dialog.ShowError("kein DB Context vorhanden");
		}
		#endregion
		#region Commands
		public DelegateCommand DeleteRoom => new DelegateCommand(CmdDeleteRoom);
		#endregion
		#region Actions
		private async void CmdDeleteRoom()
		{
			try
			{
				if (dbcontext != null && _selectedRoom != null)
				{
					_selectedRoom.Deleted = true;
					await dbcontext.SaveChangesAsync();
					await LoadRoomsAsync();
				}
				else
					dialog.ShowError($"DB Context oder {nameof(_selectedRoom)} nicht vorhanden");
			}
			catch (Exception ex)
			{
				dialog.ShowError(ex.Message);
			}
		}
		#endregion
	}
}
