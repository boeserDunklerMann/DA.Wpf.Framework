using DA.SharedDeskPlanner.Model.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Wpf.Example.Controls.MVVM
{
	internal abstract class BaseViewModel : ObservableObject
	{
		protected ISharedDeskPlannerContext? dbcontext;

		protected BaseViewModel(ISharedDeskPlannerContext context)
		{
			// TODO DA: get context from DI or so
			dbcontext = context;
		}

		public abstract Task OnInitAsync();
		public abstract void OnStart();
		public abstract void OnStop();
	}
}
