using CommunityToolkit.Mvvm.Input;
using DA.SharedDeskPlanner.Model.Contracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Windows;

namespace DA.Wpf.Framework.Auth
{
	/// <ChangeLog>
	/// <Create Datum="28.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// ViewModel for Login-Window
	/// </summary>
	internal class LoginViewModel(ICurrentUserService userService, ISharedDeskPlannerContext ctx) : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;
		protected void RaisePropChanged(string propertyName = "") =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		public string? Username { get; set; }
		public string? Password { get; set; }    // TODO DA: sicherheit beachten

		// 1. Nur deklarieren, noch nicht initialisieren!
		public IAsyncRelayCommand<object> LoginCommand { get; } = new AsyncRelayCommand<object>(p => ((LoginViewModel)null!).ExecuteLoginAsync(p));

		private async Task ExecuteLoginAsync(object? windowParameter)
		{
			// 1. User aus der DB fischen
			var user = await ctx.Users
				.FirstOrDefaultAsync(u => u.Name == Username && !u.Deleted);

			if (user != null /* && PasswordHashValid(Password, user.PasswordHash) */)
			{
				// 2. In den globalen Session-Service schreiben!
				userService.SetCurrentUser(user);

				// 3. Dem Fenster signalisieren, dass es sich schließen kann
				if (windowParameter is Window window)
				{
					window.DialogResult = true;
					window.Close();
				}
			}
			else
			{
				// Fehlermeldung an UI senden (z.B. "Falscher Benutzername")
			}
		}
	}
}
