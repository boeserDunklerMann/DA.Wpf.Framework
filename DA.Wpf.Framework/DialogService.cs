using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace DA.Wpf.Framework
{
	/// <ChangeLog>
	/// <Create Datum="26.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	public class DialogService : IDialogService
	{
		public void ShowError(string message, string title = "Fehler")
		{
			MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
		}

		public void ShowInfo(string message, string title = "Information")
		{
			MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
		}
	}
}