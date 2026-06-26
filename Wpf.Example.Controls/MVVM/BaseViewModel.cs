using DA.SharedDeskPlanner.Model.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Wpf.Example.Controls.MVVM
{
	internal abstract class BaseViewModel : ObservableObject
	{
		protected DbContext? dbcontext;

		protected BaseViewModel(ISharedDeskPlannerContext context)
		{
			// TODO DA: get context from DI or so
			dbcontext = (DbContext)context;
		}

		public abstract void OnInit();
		public abstract void OnStart();
		public abstract void OnStop();
	}
}
