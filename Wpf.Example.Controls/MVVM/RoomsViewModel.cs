using DA.SharedDeskPlanner.Model;
using DA.SharedDeskPlanner.Model.Contracts;
using DA.Wpf.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using Wpf.Example.Controls.Model;

namespace Wpf.Example.Controls.MVVM
{
	/// <ChangeLog>
	/// <Create Datum="27.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	internal class RoomsViewModel(IServiceProvider serviceProvider, IDialogService dialogService) : BaseViewModel
	{
		// Das DataGrid bindet an diese flache Liste
		public ObservableCollection<TreeGridNode> FlatView { get; set; } = new ObservableCollection<TreeGridNode>();

		private async Task LoadRoomsAsync()
		{
			using (var dbcontext = serviceProvider.GetRequiredService<ISharedDeskPlannerContext>())
			{
				var raeume = await dbcontext.Rooms
					.Include(r => r.Desks)
					.Where(r => !r.Deleted).ToListAsync();
				FlatView.Clear();
				raeume.ForEach(r =>
				{
					TreeGridNode topNode = new() { Name = r.Name, Level = 0 };
					if (r.Desks != null)
						foreach (var d in r.Desks)
						{
							TreeGridNode deskNode = new() { Name = d.Name, ExtraInfo = d.Remarks!, Level = 1 };
							topNode.Children.Add(deskNode);
						}
					AddNodeToFlatView(topNode);
				});
			}
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
				// Das Event sauber abmelden, wenn der Knoten aus der FlatView fliegt
				child.ExpansionChanged -= Node_ExpansionChanged;
				FlatView.Remove(child);
			}
		}

		public override async Task OnInitAsync()
		{
			await LoadRoomsAsync();
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