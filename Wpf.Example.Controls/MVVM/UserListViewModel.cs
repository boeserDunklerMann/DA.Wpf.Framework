using System.Collections.ObjectModel;
using DA.SharedDeskPlanner.Model;
using DA.SharedDeskPlanner.Model.Contracts;
using DA.Wpf.Framework;
using Microsoft.EntityFrameworkCore;

namespace Wpf.Example.Controls.MVVM
{
	/// <ChangeLog>
	/// <Create Datum="08.05.2026" Entwickler="DA" />
	/// </ChangeLog>
	internal class UserListViewModel(ISharedDeskPlannerContext context, IDialogService dialogService) : BaseViewModel(context)
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
			if (dbcontext == null)
				throw new NullReferenceException(nameof(dbcontext));

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
				if (dbcontext != null)
				{
					_selectedUser!.Deleted = true;

					await dbcontext.SaveChangesAsync();
					await LoadUsersAsync();
					dialogService.ShowInfo("Benutzer gelöscht.");
				}
				else
					dialogService.ShowError("kein DB Context vorhanden");
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
				if (dbcontext != null)
				{
					await dbcontext.Users.AddAsync(_newUser);
					await dbcontext.SaveChangesAsync();
					await LoadUsersAsync();
					_newUser = BaseModel.Create<User>();
					RaisePropChanged(nameof(NewUser));
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
		private async Task LoadUsersAsync()
		{
			if (dbcontext != null)
			{
				// FIX: Auswahl erhalten (Selection Preservation)
				var selectedId = SelectedUser?.ID;
				var user = await dbcontext.Users.Include(nameof(User.Bookings)).Where(u => !u.Deleted).ToListAsync();
				_users.Clear();
				user.ForEach(_users.Add);
				RaisePropChanged(nameof(Users));

				if (selectedId != null)
					SelectedUser = _users.FirstOrDefault(u => u.ID == selectedId);
			}
			else
				dialogService.ShowError("kein DB Context vorhanden");
		}

		#endregion
	}
}