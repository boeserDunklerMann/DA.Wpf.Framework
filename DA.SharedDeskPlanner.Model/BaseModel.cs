using DA.SharedDeskPlanner.Model.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace DA.SharedDeskPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="18.02.2026" Entwickler="DA" />
	/// <Change Datum="19.02.2026" Entwickler="DA">ICurrentTimestamps implemented</Change>
	/// </ChangeLog>
	public abstract class BaseModel : ICurrentTimestamps
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }
		public string Name { get; set; } = "";

		/// <summary>
		/// Änderungsdatum des Datensatzes
		/// </summary>
		public DateTimeOffset? ChangeDate { get; set; }

		/// <summary>
		/// Erstelldatum
		/// </summary>
		public DateTimeOffset CreationDate { get; set; }
		public bool Deleted { get; set; }
		public override string ToString()
		{
			return Name;
		}
		public static T Create<T>(string name = "") where T : BaseModel, new()
		{
			return new T { Name = name, CreationDate = DateTime.UtcNow, ChangeDate = DateTime.UtcNow };
		}
	}
}