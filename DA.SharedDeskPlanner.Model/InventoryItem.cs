namespace DA.SharedDeskPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="18.02.2026" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// Items that belongs to a desk, like mouse, port-replicator, monitor, ...
	/// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable. AD: Darum kümmert sich EFCore
	public class InventoryItem : BaseModel
	{
		public override bool Equals(object? obj)
		{
			if (obj == null || !(obj is InventoryItem)) return false;
			return ID == ((InventoryItem)obj).ID;
		}
		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}

		public virtual Desk? Desk { get; set; }
	}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable. AD: Darum kümmert sich EFCore
}