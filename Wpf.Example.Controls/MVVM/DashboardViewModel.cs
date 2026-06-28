using DA.SharedDeskPlanner.Model;
using DA.SharedDeskPlanner.Model.Contracts;
using DA.Wpf.Framework;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace Wpf.Example.Controls.MVVM
{
	/// <ChangeLog>
	/// <Create Datum="28.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	internal class DashboardViewModel(ISharedDeskPlannerContext ctx, IDialogService dialogService) : BaseViewModel(ctx)
	{
		#region BaseViewModel implementations
		public override async Task OnInitAsync()
		{
			await LoadDashboardAsync();
		}

		public override void OnStart()
		{
		}

		public override void OnStop()
		{
		}
		#endregion

		#region Bound props.
		public int TotalBookingsCount { get; private set; }
		public string NextBookingDateString { get; private set; } = "";

		private ObservableCollection<Booking> _bookings = [];
		public ObservableCollection<Booking> UpcomingBookings => _bookings;

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
				ReloadDashboardAsync();
			}
		}
		#endregion

		#region Commands
		public IAsyncRelayCommand<Booking> CancelBookingCommand => new AsyncRelayCommand<Booking>(CmdCancelBookingCommandAsync);
		#endregion

		#region Actions
		private async Task CmdCancelBookingCommandAsync(Booking? sender)
		{
			if (sender == null) return;
			if (dbcontext != null)
			{
				sender.Deleted = true;
				dbcontext.Bookings.Update(sender);
				await dbcontext.SaveChangesAsync();
				_bookings.Remove(sender);
			}
			else
				dialogService.ShowError("keinen DB context gefunden.");
		}
		#endregion

		#region private methods
		/// <summary>
		/// will be called after SelectedUser has changed
		/// </summary>
		private async void ReloadDashboardAsync()
		{
			await LoadDashboardAsync();
		}
		private async Task LoadDashboardAsync()
		{
			if (dbcontext != null)
			{
				if (_selectedUser != null)
				{
					var usersBookingsFromToday = await dbcontext.Bookings
						.Where(b => b.UserId == _selectedUser.ID && b.BookingStart >= DateTime.UtcNow.Date && !b.Deleted).OrderBy(b=>b.BookingStart)
						.ToListAsync();
					TotalBookingsCount = usersBookingsFromToday.Count;
					RaisePropChanged(nameof(TotalBookingsCount));
					if (usersBookingsFromToday.Count > 0)
					{
						NextBookingDateString = usersBookingsFromToday.First().BookingStart.ToString("D", System.Globalization.CultureInfo.CurrentCulture);
						RaisePropChanged(nameof(NextBookingDateString));
						_bookings.Clear();
						usersBookingsFromToday.ForEach(_bookings.Add);
					}
				}
				var users = await dbcontext.Users
					.Where(u => !u.Deleted).ToListAsync();
				_availableUsers.Clear();
				users.ForEach(_availableUsers.Add);
				RaisePropChanged(nameof(AvailableUsers));
			}
			else
				dialogService.ShowError("keinen DB context gefunden.");
		}

		#endregion
	}
}