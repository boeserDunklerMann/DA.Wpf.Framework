using DA.SharedDeskPlanner.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DA.SharedDeskPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="18.02.2026" Entwickler="DA" />
	/// <Change Datum="23.03.2026" Entwickler="DA">DataAnnotations added</Change>
/// <Change Datum="23.03.2026" Entwickler="DA">Own validation class added</Change>
	/// </ChangeLog>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable. AD: Darum kümmert sich EFCore
	public class User : BaseModel
	{
		[Required(ErrorMessage = "Vorname darf nicht leer sein.")]
		[UserVornameValidation]
		public string? FirstName { get; set; }
		[Required(ErrorMessage = "Nachname darf nicht leer sein.")]
		[UserNachnameValidation]
		public string? LastName { get; set; }

		#region Overrides
		public override bool Equals(object? obj)
		{
			if (obj == null || !(obj is User)) return false;
			return ID == ((User)obj).ID;
		}
		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}
		#endregion

		[JsonIgnore]
		public ICollection<Booking>? Bookings { get; set; }
	}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable. AD: Darum kümmert sich EFCore
}