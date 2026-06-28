using DA.SharedDeskPlanner.Model;
using DA.SharedDeskPlanner.Model.Contracts;
using DA.Wpf.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Wpf.Example.Controls.MVVM
{
	/// <ChangeLog>
	/// <Create Datum="28.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	internal class AddBookingViewModel(IServiceProvider serviceProvider, IDialogService dialogService) : BaseViewModel
	{
		private bool iamLoading = false;
		#region BaseViewModel implementation
		public override async Task OnInitAsync()
		{
			await LoadDataAsync();
		}

		public override void OnStart()
		{
		}

		public override void OnStop()
		{
		}
		#endregion

		#region Bound props
		private ObservableCollection<Room> _rooms = [];
		public ObservableCollection<Room> Rooms => _rooms;
		private Room? _selectedRoom;
		public Room? SelectedRoom
		{
			get => _selectedRoom;
			set
			{
				_selectedRoom = value;
				RaisePropChanged(nameof(SelectedRoom));
				Reload();
			}
		}
		private DateOnly _selectedDate;
		public DateOnly SelectedDate
		{
			get => _selectedDate;
			set
			{
				_selectedDate = value;
				RaisePropChanged(nameof(SelectedDate));
			}
		}
		private ObservableCollection<Desk> _availableDesks = [];
		public ObservableCollection<Desk> AvailableDesks => _availableDesks;
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
		private ObservableCollection<User> _availableUsers = [];
		public ObservableCollection<User> AvailableUsers => _availableUsers;
		private User? _selectedUser;
		public User? SelectedUser
		{
			get => _selectedUser;
			set
			{
				_selectedUser = value;
				RaisePropChanged(nameof(SelectedUser));
				Reload();
			}
		}
		#endregion

		#region Commands
		public DelegateCommand CancelCommand => new(CmdCancelCommand);
		public DelegateCommand SaveBookingCommand => new(CmdSaveBookingCommand);
		#endregion

		#region Actions
		private async void CmdCancelCommand()
		{

		}
		private async void CmdSaveBookingCommand()
		{

		}
		#endregion

		#region private methods
		private async void Reload()
		{
			if (!iamLoading)
				await LoadDataAsync();
		}
		private async Task LoadDataAsync()
		{
			try
			{
				iamLoading = true;
				// Speicher den aktuell gewählten Raum zwischen, um ihn nach dem Clear wiederherzustellen
				var previousSelectionId = _selectedRoom?.ID;

				// Räume laden
				using (var dbcontext = serviceProvider.GetRequiredService<ISharedDeskPlannerContext>())
				{
					var rooms = await dbcontext.Rooms.Where(r => !r.Deleted).ToListAsync();

					_rooms.Clear();
					rooms.ForEach(_rooms.Add);
				}

				// Versuche, die vorherige Auswahl wiederherzustellen, falls sie noch existiert
				if (previousSelectionId != null)
				{
					SelectedRoom = _rooms.FirstOrDefault(r => r.ID == previousSelectionId);
				}
				var selectedUID = _selectedUser?.ID;
				using (var dbcontext = serviceProvider.GetRequiredService<ISharedDeskPlannerContext>())
				{
					var users = await dbcontext.Users.Where(u => !u.Deleted).ToListAsync();
					_availableUsers.Clear();
					users.ForEach(_availableUsers.Add);
					if (selectedUID != null)
						SelectedUser = users.FirstOrDefault(u => u.ID == selectedUID);
				}
				if (_selectedRoom != null)
				{
					_availableDesks.Clear();
					using (var dbcontext = serviceProvider.GetRequiredService<ISharedDeskPlannerContext>())
					{
						var allDesks = await dbcontext.Desks.Where(d => !d.Deleted && d.RoomID == _selectedRoom.ID).ToListAsync();
						allDesks.ForEach(_availableDesks.Add);
					}
				}
			}
			catch (Exception ex)
			{
				dialogService.ShowError(ex.Message);
			}
			finally
			{
				iamLoading = false;
			}
		}
		
		#endregion
	}
}