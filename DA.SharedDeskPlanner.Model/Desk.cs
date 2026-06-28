using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DA.SharedDeskPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="18.02.2026" Entwickler="DA" />
	/// <Change Datum="18.02.2026" Entwickler="DA">Bookings added</Change>
	/// <Change Datum="27.06.2026" Entwickler="DA">default initializer added to Inventory and Bookings</Change>
	/// </ChangeLog>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable. AD: Darum kümmert sich EFCore
	public class Desk : BaseModel
	{
		/// <summary>
		/// like "Höhenverstellbar"
		/// </summary>
		public string? Remarks { get; set; }

		public override bool Equals(object? obj)
		{
			if (obj == null || !(obj is Desk)) return false;
			return ID == ((Desk)obj).ID;
		}
		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}
		[JsonIgnore]
		public virtual ICollection<InventoryItem>? Inventory { get; set; } = [];
		[JsonIgnore]
		public virtual Room Room { get; set; }
		public int RoomID { get; set; }
		[JsonIgnore]
		public virtual ICollection<Booking> Bookings { get; set; } = [];
	}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable. AD: Darum kümmert sich EFCore
}