using System.ComponentModel.DataAnnotations.Schema;

namespace DA.SharedDeskPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="18.02.2026" Entwickler="DA" />
	/// <Change Datum="22.02.2026" Entwickler="DA">IsActive property added</Change>
	/// </ChangeLog>
	/// <summary>Definiert eine Desk-Buchung
	/// defines a desk-booking
	/// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable. AD: Darum kümmert sich EFCore
	public class Booking : BaseModel
	{
		public DateTimeOffset BookingStart { get; set; }
		public DateTimeOffset BookingEnd { get; set; }

		#region Overrides
		public override bool Equals(object? obj)
		{
			if (obj == null || !(obj is Booking)) return false;
			return ID == ((Booking)obj).ID;
		}
		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}
		#endregion

		public virtual User? User { get; set; }
		public int UserId { get; set; }
		public virtual Desk? Desk { get; set; }
		public int DeskId { get; set; }
		//[NotMapped]
		//public bool IsActive => (BookingStart < DateTimeOffset.UtcNow) && (BookingEnd > DateTimeOffset.UtcNow);
	}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable. AD: Darum kümmert sich EFCore
}