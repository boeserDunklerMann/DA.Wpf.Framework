using DA.SharedDeskPlanner.Model;

namespace DA.Wpf.Framework.Auth
{
	/// <ChangeLog>
	/// <Create Datum="28.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	public interface ICurrentUserService
	{
		User? IsLoggedInUser { get; }
		bool IsAuthenticated => IsLoggedInUser != null;
		void SetCurrentUser(User user);
		void ClearSession();
	}
}