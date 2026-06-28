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
	/// <Create Datum="27.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	internal class BookingsViewModel(IServiceProvider serviceProvider, IDialogService dialogService) : BaseViewModel
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
				using (var dbcontext = serviceProvider.GetRequiredService<ISharedDeskPlannerContext>())
				{
					_selectedBooking!.Deleted = true;
					await dbcontext.SaveChangesAsync();
					await LoadBookingsAsync();
				}
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
			using (var dbcontext = serviceProvider.GetRequiredService<ISharedDeskPlannerContext>())
			{
				var bookings = await dbcontext.Bookings
				.Include(b => b.Desk)
				.Include(b => b.User)
				.Where(b => !b.Deleted).ToListAsync();
				_bookings.Clear();
				bookings.ForEach(_bookings.Add);
			}
		}
		
		#endregion
	}
}
