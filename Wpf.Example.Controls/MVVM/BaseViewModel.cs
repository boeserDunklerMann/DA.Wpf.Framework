using Microsoft.EntityFrameworkCore;

namespace Wpf.Example.Controls.MVVM
{
	internal abstract class BaseViewModel : ObservableObject, IDisposable
	{
		protected DbContext? dbcontext;

		protected BaseViewModel()
		{
			// TODO DA: get context from DI or so
		}

		public abstract void OnInit();
		public abstract void OnStart();
		public abstract void OnStop();
		public void Dispose()
		{
			if (dbcontext != null)
			{
				dbcontext.Dispose();
				dbcontext = null;
			}
		}
	}
}
