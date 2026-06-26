using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DA.SharedDeskPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="26.03.2026" Entwickler="DA" />
	/// </ChangeLog>
	internal class DateTimeOffsetToUtcConverter : ValueConverter<DateTimeOffset, DateTime>
	{
		public DateTimeOffsetToUtcConverter()
		: base(
			v => v.UtcDateTime,           // In die DB (als UTC DateTime)
			v => new DateTimeOffset(v, TimeSpan.Zero) // Aus der DB (als Offset 0)
		)
		{ }
	}
}