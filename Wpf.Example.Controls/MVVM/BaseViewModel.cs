using DA.SharedDeskPlanner.Model.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Wpf.Example.Controls.MVVM
{
	internal abstract class BaseViewModel : ObservableObject
	{
		public abstract Task OnInitAsync();
		public abstract void OnStart();
		public abstract void OnStop();
	}
}
