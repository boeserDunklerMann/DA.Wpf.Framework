using DA.SharedDeskPlanner.Model;

namespace DA.Wpf.Framework.Auth
{
	/// <ChangeLog>
	/// <Create Datum="28.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	public class CurrentUserService : ICurrentUserService
	{
		public User? IsLoggedInUser { get; private set; }

		public void ClearSession()
		{
			IsLoggedInUser = null;
		}

		public void SetCurrentUser(User user)
		{
			IsLoggedInUser = user ?? throw new ArgumentNullException(nameof(user));
		}
	}
}
