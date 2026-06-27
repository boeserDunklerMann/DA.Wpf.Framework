using DA.SharedDeskPlanner.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wpf.Example.Controls.Model
{
	/// <ChangeLog>
		/// <Create Datum="27.06.2026" Entwickler="DA" />
		/// </ChangeLog>
	internal class InventoryItem4NewDesk(InventoryItem invItem)
	{
		public bool IsChecked { get; set; }
		public string Name => invItem.Name;
		public InventoryItem Item => invItem;
	}
}
