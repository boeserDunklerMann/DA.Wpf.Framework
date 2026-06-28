using DA.SharedDeskPlanner.Model.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DA.SharedDeskPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="18.02.2026" Entwickler="DA" />
	/// <Change Datum="18.02.2026" Entwickler="DA">User and Booking added</Change>
	/// <Change Datum="26.06.2026" Entwickler="DA">Interface ISharedDeskPlanner added</Change>
	/// <Change Datum="28.06.2026" Entwickler="DA">InventoryItem.Desk removed</Change>
		/// </ChangeLog>
	public class SharedDeskPlannerContext : DbContext, ISharedDeskPlannerContext, IDisposable
	{
		private string _connectionString = "";
		private IConfiguration? _configuration;
		public DbSet<InventoryItem> InventoryItems { get; set; }
		public DbSet<Desk> Desks { get; set; }
		public DbSet<Room> Rooms { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Booking> Bookings { get; set; }

		//public SharedDeskPlannerContext(IConfiguration? cfg = null) : base()
		//{
		//	if (cfg != null)
		//	{
		//		_configuration = cfg;
		//		_connectionString = _configuration["ConnectionStrings:da_sdp_db"]!;
		//	}
		//}

		public SharedDeskPlannerContext(string connString) : base()
		{
			_connectionString = connString;
		}

		public void SetConfiguration(IConfiguration cfg)
		{
			if (cfg != null)
			{
				_configuration = cfg;
				_connectionString = _configuration["ConnectionStrings:da_sdp_db"]!;
				Database.SetConnectionString(_connectionString);
			}
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// https://stackoverflow.com/questions/74060289/mysqlconnection-open-system-invalidcastexception-object-cannot-be-cast-from-d
			// MariaDB 11+ doesnt work because of nullable PKs?
			optionsBuilder.UseMySQL(_connectionString);  // CaptainTrips works with MariaDB 10
														 //this.SavingChanges += OnSavingChanges;
														 //this.ChangeTracker.StateChanged += OnStateChanged;
		}
		private void OnStateChanged(object? sender, Microsoft.EntityFrameworkCore.ChangeTracking.EntityStateChangedEventArgs e)
		{
			// TODO AD: https://learn.microsoft.com/de-de/ef/core/logging-events-diagnostics/events
		}

		private void OnSavingChanges(object? sender, SavingChangesEventArgs e)
		{
			// TODO AD: https://learn.microsoft.com/de-de/ef/core/logging-events-diagnostics/events
		}

		public override int SaveChanges()
		{
			UpdateChangeDate();
			return base.SaveChanges();
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			UpdateChangeDate();
			return await base.SaveChangesAsync(cancellationToken);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Room>(entity =>
			{
				entity.HasKey(r => r.ID);
				entity.Property(r => r.Name).IsRequired();
			});
			modelBuilder.Entity<Desk>(entity =>
			{
				entity.HasKey(d => d.ID);
				entity.Property(d => d.Name).IsRequired();
				entity.HasOne(d => d.Room).WithMany(r => r.Desks);
				entity.HasMany(d => d.Bookings).WithOne(b => b.Desk);
			});
			modelBuilder.Entity<InventoryItem>(entity =>
			{
				entity.HasKey(ii => ii.ID);
				entity.Property(ii => ii.Name).IsRequired();
				//entity.HasOne(ii => ii.Desk).WithMany(d => d.Inventory);
			});
			modelBuilder.Entity<User>(ent =>
			{
				ent.HasKey(u => u.ID);
				ent.Property(u => u.Name).IsRequired();
				ent.HasMany(u => u.Bookings).WithOne(b => b.User);
			});
			modelBuilder.Entity<Booking>(ent =>
			{
				ent.HasKey(b => b.ID);
				ent.Property(b => b.Name).IsRequired();
				ent.Property(b => b.ChangeDate).HasConversion(
					v => v.HasValue ? v.Value.UtcDateTime : (DateTime?)null,    // in die DB als UTC
					v => new DateTimeOffset(v.HasValue ? v.Value : DateTime.MinValue, TimeSpan.Zero));  // aus der DB als Offset 0
			});
		}

		protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
		{
			configurationBuilder.Properties<DateTimeOffset>()
				.HaveConversion<DateTimeOffsetToUtcConverter>();
		}

		private void UpdateChangeDate()
		{
			DateTimeOffset now = DateTime.UtcNow;
			var createdEntries = ChangeTracker.Entries().Where(entry => entry.State == EntityState.Added).ToList();
			var changedEntries = ChangeTracker.Entries().Where(entry => entry.State == EntityState.Modified).ToList();
			createdEntries.ForEach(e =>
			{
				var prop = e.Properties.FirstOrDefault(prop => prop.Metadata.Name.Equals(nameof(ICurrentTimestamps.CreationDate)));
				if (prop != null)
					prop.CurrentValue = now;
			});
			changedEntries.ForEach(e =>
			{
				var prop = e.Properties.FirstOrDefault(prop => prop.Metadata.Name.Equals(nameof(ICurrentTimestamps.ChangeDate)));
				if (prop != null)
					prop.CurrentValue = now;
			});
		}
	}
}