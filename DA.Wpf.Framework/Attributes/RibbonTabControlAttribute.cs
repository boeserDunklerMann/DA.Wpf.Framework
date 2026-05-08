namespace DA.Wpf.Framework.Attributes
{
	/// <ChangeLog>
	/// <Create Datum="08.05.2026" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// Sets attributes for a RibbonTabControl
	/// </summary>
	/// <param name="heading">the Headertext of the Ribbon Tab</param>
	/// <param name="position">the one-based index of the Ribbon Tab</param>
	public class RibbonTabControlAttribute(string heading, int position) : Attribute
	{
		public string Heading => heading;
		public int Position => position;
	}
}
