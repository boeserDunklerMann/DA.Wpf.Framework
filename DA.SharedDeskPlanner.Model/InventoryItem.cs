using System.Text.Json.Serialization;

namespace DA.SharedDeskPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="18.02.2026" Entwickler="DA" />
	/// <Change Datum="28.06.2026" Entwickler="DA">property Desk removed</Change>
	/// <Change Datum="28.06.2026" Entwickler="DA">ICollection Desks added</Change>
	/// <Change Datum="28.06.2026" Entwickler="DA">Prrop. InventoryNumber added</Change>
	/// </ChangeLog>
	/// <summary>
	/// Items that belongs to a desk, like mouse, port-replicator, monitor, ...
	/// </summary>
	public class InventoryItem : BaseModel
	{
		/// <summary>
		/// internal Inv-Nr.
		/// </summary>
		public string? InventoryNumber { get; set; }
		/// <summary>
		/// m:n-Beziehung zum Desk, damit ein Item mehrfach verwendet werden kann
		/// </summary>
		[JsonIgnore]
		public virtual ICollection<Desk> Desks { get; set; } = [];
		public override bool Equals(object? obj)
		{
			if (obj == null || !(obj is InventoryItem)) return false;
			return ID == ((InventoryItem)obj).ID;
		}
		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}
	}
}