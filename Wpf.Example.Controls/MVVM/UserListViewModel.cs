using DA.SharedDeskPlanner.Model;
using DA.SharedDeskPlanner.Model.Contracts;
using DA.Wpf.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Wpf.Example.Controls.MVVM
{
	/// <ChangeLog>
	/// <Create Datum="08.05.2026" Entwickler="DA" />
	/// </ChangeLog>
	internal class UserListViewModel(IServiceProvider serviceProvider, IDialogService dialogService) : BaseViewModel
	{
		#region Bound lists & props
		private User _newUser = BaseModel.Create<User>();
		public User NewUser => _newUser;

		private User? _selectedUser;
		public User? SelectedUser
		{
			get => _selectedUser;
			set
			{
				_selectedUser = value;
				RaisePropChanged(nameof(SelectedUser));
			}
		}

		private ObservableCollection<User> _users = [];

		public ObservableCollection<User> Users => _users;
		#endregion

		public override async Task OnInitAsync()
		{
			await LoadUsersAsync();
		}

		public override void OnStart()
		{
		}

		public override void OnStop()
		{
		}

		#region Commands
		public DelegateCommand CreateUser => new DelegateCommand(CmdCreateUser);
		public DelegateCommand DeleteUser => new DelegateCommand(CmdDeleteUser);
		#endregion

		#region Actions
		private async void CmdDeleteUser()
		{
			try
			{
				if (_selectedUser != null)
					using (var dbcontext = serviceProvider.GetRequiredService<ISharedDeskPlannerContext>())
					{
						dbcontext.Users.Attach(_selectedUser);
						_selectedUser.Deleted = true;

						await dbcontext.SaveChangesAsync();
						await LoadUsersAsync();
						dialogService.ShowInfo("Benutzer gelöscht.");
					}
				else
					dialogService.ShowError($"{nameof(_selectedUser)} nicht vorhanden");
			}
			catch (Exception ex)
			{
				dialogService.ShowError(ex.Message);
			}
		}
		private async void CmdCreateUser()
		{
			try
			{
				using (var dbcontext = serviceProvider.GetRequiredService<ISharedDeskPlannerContext>())
				{
					await dbcontext.Users.AddAsync(_newUser);
					await dbcontext.SaveChangesAsync();
					await LoadUsersAsync();
					_newUser = BaseModel.Create<User>();
					RaisePropChanged(nameof(NewUser));
				}
			}
			catch (Exception ex)
			{
				dialogService.ShowError(ex.Message);
			}
		}
		#endregion
		
		#region private methods
		private async Task LoadUsersAsync()
		{
			using (var dbcontext = serviceProvider.GetRequiredService<ISharedDeskPlannerContext>())
			{
				// FIX: Auswahl erhalten (Selection Preservation)
				var selectedId = SelectedUser?.ID;
				var user = await dbcontext.Users.Include(nameof(User.Bookings)).Where(u => !u.Deleted).ToListAsync();
				_users.Clear();
				user.ForEach(_users.Add);

				if (selectedId != null)
					SelectedUser = _users.FirstOrDefault(u => u.ID == selectedId);
			}
		}

		#endregion
	}
}