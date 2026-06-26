using System;
using System.Collections.Generic;
using System.Text;

namespace DA.Wpf.Framework
{
	/// <ChangeLog>
		/// <Create Datum="26.06.2026" Entwickler="DA" />
		/// </ChangeLog>
	public interface IDialogService
	{
		void ShowError(string message, string title="Fehler");
		void ShowInfo(string message, string title = "Information");

	}
}
