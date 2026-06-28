using CommunityToolkit.Mvvm.Input;
using DA.SharedDeskPlanner.Model.Contracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace DA.Wpf.Framework.Auth
{
	/// <ChangeLog>
	/// <Create Datum="28.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// ViewModel for Login-Window
	/// </summary>
	public class LoginViewModel(ICurrentUserService userService, ISharedDeskPlannerContext ctx) : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;
		protected void RaisePropChanged(string propertyName = "") =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		public string? Username { get; set; }
		public string? Password { get; set; }    // TODO DA: sicherheit beachten

		// Ein einfaches Backing-Field für das Command
		private IAsyncRelayCommand<object>? _loginCommand;

		// Die Eigenschaft initialisiert das Command erst beim ersten Zugriff (Lazy)
		// Zu diesem Zeitpunkt ist 'this' garantiert vollständig und sicher verfügbar!
		public IAsyncRelayCommand<object> LoginCommand =>
			_loginCommand ??= new AsyncRelayCommand<object>(ExecuteLoginAsync);

		private async Task ExecuteLoginAsync(object? parameter)
		{
			if (parameter is not PasswordBox passwordBox) return;
			// Hol das Fenster, in dem die PasswordBox liegt, um es später zu schließen
			var currentWindow = Window.GetWindow(passwordBox);

			// Das Passwort sicher auslesen
			string clearTextPassword = passwordBox.Password;

			// 1. User aus der DB fischen
			var user = await ctx.Users
				.FirstOrDefaultAsync(u => u.Name == Username && !u.Deleted);

			if (user != null /* && PasswordHashValid(Password, user.PasswordHash) */)
			{
				// 2. In den globalen Session-Service schreiben!
				userService.SetCurrentUser(user);

				// 3. Dem Fenster signalisieren, dass es sich schließen kann
				if (currentWindow != null)
				{
					currentWindow.DialogResult = true;
					currentWindow.Close();
				}
				if (parameter is Window window)
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
