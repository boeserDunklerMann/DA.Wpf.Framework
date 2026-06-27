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
	internal class BookingsViewModel(ISharedDeskPlannerContext ctx, IDialogService dialogService) : BaseViewModel(ctx)
	{
		#region BaseViewModel implementations
		public override async Task OnInitAsync()
		{
			await LoadBookingsAsync();
		}

		public override void OnStart()
		{	
		}

		public override void OnStop()
		{
		}
		#endregion

		#region Bound props
		private readonly ObservableCollection<Booking> _bookings=[];
		public ObservableCollection<Booking> Bookings => _bookings;
		private Booking? _selectedBooking;
		public Booking? SelectedBooking
		{
			get => _selectedBooking;
			set
			{
				_selectedBooking = value;
				RaisePropChanged(nameof(SelectedBooking));
			}
		}
		private readonly Booking _newBooking = BaseModel.Create<Booking>();
		public Booking NewBooking => _newBooking;
		#endregion
	
		#region Commands
		public DelegateCommand DeleteBooking => new DelegateCommand(CmdDeleteBooking);
		#endregion

		#region Actions
		private async void CmdDeleteBooking()
		{
			try
			{
				if (dbcontext != null)
				{
					_selectedBooking!.Deleted = true;
					await dbcontext.SaveChangesAsync();
					await LoadBookingsAsync();
				}
				else
					dialogService.ShowError("kein DB Context vorhanden");
			}
			catch (Exception ex)
			{
				dialogService.ShowError(ex.Message);
			}
		}
		#endregion

		#region private methods
		private async Task LoadBookingsAsync()
		{
			if (dbcontext != null)
			{
				var bookings = await dbcontext.Bookings
					.Include(b => b.Desk)
					.Include(b => b.User)
					.Where(b => !b.Deleted).ToListAsync();
				_bookings.Clear();
				bookings.ForEach(_bookings.Add);
				RaisePropChanged(nameof(Bookings));
			}
			else
			{
				dialogService.ShowError("keinen DB context gefunden.");
			}
		}
		#endregion
	}
}
