using Microsoft.Extensions.DependencyInjection;

namespace DA.Wpf.Framework
{
	/// <ChangeLog>
	/// <Create Datum="08.05.2026" Entwickler="DA" />
	/// <Change Datum="28.06.2026" Entwickler="DA">prop. IconGeometry added</Change>
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
		Task OnInitAsync(IServiceCollection services);
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
		/// <summary>
		/// Liefert die XAML-Pfadgeometrie für das Icon (z.B. "M10,20 L30,40...")
		/// </summary>
		string IconGeometry { get; }
	}
}
