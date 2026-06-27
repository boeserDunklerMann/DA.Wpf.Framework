using DA.SharedDeskPlanner.Model.Contracts;
using DA.Wpf.Framework;
using System.Collections.ObjectModel;
using Wpf.Example.Controls.Model;

namespace Wpf.Example.Controls.MVVM
{
	/// <ChangeLog>
	/// <Create Datum="27.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	internal class RoomsViewModel:BaseViewModel
	{
		// Das DataGrid bindet an diese flache Liste
		public ObservableCollection<TreeGridNode> FlatView { get; set; } = new ObservableCollection<TreeGridNode>();

		public RoomsViewModel(ISharedDeskPlannerContext ctx, IDialogService dialogService):base(ctx)
		{
			LoadDemoData();
		}

		private void LoadDemoData()
		{
			// Beispiel-Daten aufbauen
			var root1 = new TreeGridNode { Name = "Projekt A", ExtraInfo = "Hauptordner", Level = 0 };
			var child1 = new TreeGridNode { Name = "Spezifikation.docx", ExtraInfo = "64 KB", Level = 1 };
			var child2 = new TreeGridNode { Name = "Architektur", ExtraInfo = "Ordner", Level = 1 };
			var subChild = new TreeGridNode { Name = "Diagramm.png", ExtraInfo = "1.2 MB", Level = 2 };

			child2.Children.Add(subChild);
			root1.Children.Add(child1);
			root1.Children.Add(child2);

			var root2 = new TreeGridNode { Name = "Projekt B", ExtraInfo = "Inaktiv", Level = 0 };

			// Nur die Root-Elemente initial in die flache Liste werfen
			AddNodeToFlatView(root1);
			AddNodeToFlatView(root2);
		}

		private void AddNodeToFlatView(TreeGridNode node)
		{
			node.ExpansionChanged += Node_ExpansionChanged;
			FlatView.Add(node);

			// Falls es initial schon offen sein soll
			if (node.IsExpanded)
			{
				foreach (var child in node.Children)
				{
					AddNodeToFlatView(child);
				}
			}
		}

		private void Node_ExpansionChanged(TreeGridNode node, bool isExpanded)
		{
			int index = FlatView.IndexOf(node);
			if (index == -1) return;

			if (isExpanded)
			{
				// Kinder rekursiv einfügen
				InsertChildren(node, ref index);
			}
			else
			{
				// Alle Kinder und Unterkinder entfernen
				RemoveChildren(node);
			}
		}

		private void InsertChildren(TreeGridNode parent, ref int index)
		{
			foreach (var child in parent.Children)
			{
				index++;
				child.ExpansionChanged -= Node_ExpansionChanged; // Doppelte Registrierung vermeiden
				child.ExpansionChanged += Node_ExpansionChanged;
				FlatView.Insert(index, child);

				if (child.IsExpanded)
				{
					InsertChildren(child, ref index);
				}
			}
		}

		private void RemoveChildren(TreeGridNode parent)
		{
			foreach (var child in parent.Children)
			{
				if (child.IsExpanded)
				{
					RemoveChildren(child);
				}
				FlatView.Remove(child);
			}
		}

		public override async Task OnInitAsync()
		{
			await Task.CompletedTask;
		}

		public override async void OnStart()
		{
			await Task.CompletedTask;
		}

		public override async void OnStop()
		{
			await Task.CompletedTask;
		}
	}
}