using DA.SharedDeskPlanner.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Wpf.Example.Controls.Model
{
	/// <ChangeLog>
		/// <Create Datum="27.06.2026" Entwickler="DA" />
		/// </ChangeLog>
		/// <summary>
		/// Model class for UI-view
		/// </summary>
	public class TreeGridNode : INotifyPropertyChanged
	{
		private bool _isExpanded;
		public string Name { get; set; } = "";
		public string ExtraInfo { get; set; } = "";

		// Die hierarchische Tiefe: 0 = Hauptknoten, 1 = Kind, 2 = Enkel, usw.
		public int Level { get; set; }

		public bool HasChildren => Children != null && Children.Count > 0;
		public List<TreeGridNode> Children { get; set; } = new List<TreeGridNode>();

		public bool IsExpanded
		{
			get => _isExpanded;
			set
			{
				if (_isExpanded != value)
				{
					_isExpanded = value;
					OnPropertyChanged();
					// Event feuern, damit das Haupt-ViewModel die Liste aktualisiert
					ExpansionChanged?.Invoke(this, value);
				}
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		public event System.Action<TreeGridNode, bool>? ExpansionChanged;

		protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}