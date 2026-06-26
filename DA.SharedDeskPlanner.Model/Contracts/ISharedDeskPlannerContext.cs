using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DA.SharedDeskPlanner.Model.Contracts
{
	/// <ChangeLog>
	/// <Create Datum="26.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	public interface ISharedDeskPlannerContext
	{
		DbSet<Booking> Bookings { get; set; }
		DbSet<Desk> Desks { get; set; }
		DbSet<InventoryItem> InventoryItems { get; set; }
		DbSet<Room> Rooms { get; set; }
		DbSet<User> Users { get; set; }

		int SaveChanges();
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
		void SetConfiguration(IConfiguration cfg);
	}
}