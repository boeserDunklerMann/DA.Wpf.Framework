using System.ComponentModel.DataAnnotations;

namespace DA.SharedDeskPlanner.Validation
{
	/// <ChangeLog>
	/// <Create Datum="23.03.2026" Entwickler="DA" />
	/// </ChangeLog>
	public class UserVornameValidation : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if (value is string vorname)
			{
				if (!string.IsNullOrEmpty(vorname))
					return ValidationResult.Success;
			}
			return new ValidationResult(ErrorMessage ?? "Vorname darf nicht leer sein.");
		}
	}
	/// <ChangeLog>
	/// <Create Datum="23.03.2026" Entwickler="DA" />
	/// </ChangeLog>
	public class UserNachnameValidation : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if (value is string nachname)
			{
				if (!string.IsNullOrEmpty(nachname))
					return ValidationResult.Success;
			}
			return new ValidationResult(ErrorMessage ?? "Nachname darf nicht leer sein.");
		}
	}
}