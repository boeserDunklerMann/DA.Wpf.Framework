
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DA.Wpf.Framework
{
	/// <ChangeLog>
	/// <Create Datum="08.05.2026" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// Interface for controls that should be inserted into a RibbonTab
	/// </summary>
	public interface IRibbonTabControl
	{
		/// <summary>
		/// Will be called before Initializing, after loading the conrol.
		/// Here you need to register your services (eg. the ViewModel)
		/// </summary>
		void OnInit(IServiceCollection services);
		/// <summary>
		/// will be called after Initializing the control
		/// </summary>
		void OnStart();
		/// <summary>
		/// will be called before unloading the control. So dispose all resources here.
		/// </summary>
		void OnStop();

		/// <summary>
		/// returns my own RibbonTabXControlAttribute
		/// </summary>
		/// <returns></returns>
		Attributes.RibbonTabControlAttribute GetAttribute();
		///// <summary>
		///// creates the instance of the control
		///// </summary>
		///// <returns></returns>
		//FrameworkElement CreateMyself();
	}
}
