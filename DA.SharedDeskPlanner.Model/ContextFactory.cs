using Microsoft.EntityFrameworkCore;

namespace DA.SharedDeskPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="19.02.2026" Entwickler="DA" />
	/// </ChangeLog>
	public class ContextFactory : IDbContextFactory<SharedDeskPlannerContext>
	{
		public SharedDeskPlannerContext CreateDbContext() => new();
		public SharedDeskPlannerContext Create() => CreateDbContext();
	}
	/// <ChangeLog>
	/// <Create Datum="19.02.2026" Entwickler="DA" />
	/// </ChangeLog>
	public static class ContextSingletonFactory
	{
		private static readonly Lazy<SharedDeskPlannerContext> lazy = new Lazy<SharedDeskPlannerContext>(() => new SharedDeskPlannerContext());
		public static SharedDeskPlannerContext Instance => lazy.Value;
	}
}