using System.Collections.ObjectModel;
using Wpf.Example.Model;

namespace Wpf.Example.Controls.MVVM
{
	/// <ChangeLog>
	/// <Create Datum="08.05.2026" Entwickler="DA" />
	/// </ChangeLog>
	internal class UserListViewModel : BaseViewModel
	{
		#region Bound lists
		private ObservableCollection<User> _users = [];
		public ObservableCollection<User> Users => _users;

		public override void OnInit()
		{
			// TODO DA: hier users laden
		}

		public override void OnStart()
		{
			//throw new NotImplementedException();
		}

		public override void OnStop()
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
