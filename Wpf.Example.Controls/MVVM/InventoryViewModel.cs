using DA.SharedDeskPlanner.Model;
using DA.SharedDeskPlanner.Model.Contracts;
using DA.Wpf.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Wpf.Example.Controls.MVVM
{
	/// <ChangeLog>
	/// <Create Datum="26.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	internal class InventoryViewModel(ISharedDeskPlannerContext ctx, IDialogService dialogService) : BaseViewModel(ctx)
	{
		private InventoryItem? _selectedInventoryItem;
		public InventoryItem? SelectedInventoryItem
		{
			get => _selectedInventoryItem;
			set
			{
				_selectedInventoryItem = value;
				RaisePropChanged(nameof(SelectedInventoryItem));
			}
		}
		private InventoryItem _newInventoryItem = BaseModel.Create<InventoryItem>();
		public InventoryItem NewInventoryItem => _newInventoryItem;
		private readonly ObservableCollection<InventoryItem> _inventory = [];
		public ObservableCollection<InventoryItem> Inventory => _inventory;

		public override async Task OnInitAsync()
		{
			await LoadInventoryAsync();
		}

		public override async void OnStart()
		{
			await Task.CompletedTask;
		}

		public override async void OnStop()
		{
			await Task.CompletedTask;
		}

		#region Commands
		public DelegateCommand DeleteInventoryItem => new DelegateCommand(CmdDeleteInventoryItem);
		public DelegateCommand CreateInventoryItem => new DelegateCommand(CmdCreateInventoryItem);
		#endregion

		#region Actions
		private async void CmdCreateInventoryItem()
		{
			try
			{
				if (dbcontext != null)
				{
					await dbcontext.InventoryItems.AddAsync(_newInventoryItem);
					await dbcontext.SaveChangesAsync();
					await LoadInventoryAsync();
					_newInventoryItem = BaseModel.Create<InventoryItem>();
					RaisePropChanged(nameof(NewInventoryItem));
				}
				else
					dialogService.ShowError("kein DB Context vorhanden");
			}
			catch (Exception e)
			{
				dialogService.ShowError(e.Message);
			}
		}
		private async void CmdDeleteInventoryItem()
		{
			try
			{
				if (dbcontext != null && _selectedInventoryItem != null)
				{
					_selectedInventoryItem.Deleted = true;
					await dbcontext.SaveChangesAsync();
					await LoadInventoryAsync();
				}
				else
					dialogService.ShowError($"DB Context oder {nameof(_selectedInventoryItem)} nicht vorhanden");
			}
			catch (Exception e)
			{
				dialogService.ShowError(e.Message);
			}
		}
		#endregion

		#region private methods
		private async Task LoadInventoryAsync()
		{
			if (dbcontext != null)
			{
				var selectedID = SelectedInventoryItem?.ID;

				var invitems = await dbcontext.InventoryItems.Where(ii => !ii.Deleted).ToListAsync();
				_inventory.Clear();
				invitems.ForEach(_inventory.Add);
				RaisePropChanged(nameof(Inventory));
				if (selectedID != null)
					SelectedInventoryItem = _inventory.FirstOrDefault(i => i.ID == selectedID);
			}
			else
				dialogService.ShowError("kein DB Context vorhanden");
		}

		#endregion
	}
}