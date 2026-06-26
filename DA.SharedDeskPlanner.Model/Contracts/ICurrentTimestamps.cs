using System;
using System.Collections.Generic;
using System.Text;

namespace DA.SharedDeskPlanner.Model.Contracts
{
	/// <ChangeLog>
		/// <Create Datum="19.02.2026" Entwickler="DA" />
		/// </ChangeLog>
	internal interface ICurrentTimestamps
	{
		/// <summary>
		/// Änderungsdatum
		/// </summary>
		DateTimeOffset? ChangeDate { get; set; }

		/// <summary>
		/// Erstelldatum
		/// </summary>
		DateTimeOffset CreationDate { get; set; }
	}
}
