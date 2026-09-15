using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020008F4 RID: 2292
public class UIAutopsyWindowData : LazyWidgetDataBase
{
	// Token: 0x17000908 RID: 2312
	// (get) Token: 0x06003BF4 RID: 15348 RVA: 0x0011E4E3 File Offset: 0x0011C6E3
	// (set) Token: 0x06003BF5 RID: 15349 RVA: 0x0011E4EB File Offset: 0x0011C6EB
	public bool IsEmpty { get; private set; }

	// Token: 0x17000909 RID: 2313
	// (get) Token: 0x06003BF6 RID: 15350 RVA: 0x0011E4F4 File Offset: 0x0011C6F4
	// (set) Token: 0x06003BF7 RID: 15351 RVA: 0x0011E4FC File Offset: 0x0011C6FC
	public UICorpseWidgetData CorpseWidgetData { get; private set; }

	// Token: 0x1700090A RID: 2314
	// (get) Token: 0x06003BF8 RID: 15352 RVA: 0x0011E505 File Offset: 0x0011C705
	// (set) Token: 0x06003BF9 RID: 15353 RVA: 0x0011E50D File Offset: 0x0011C70D
	public BodyOrgansInventoryWidgetData BodyOrgansInventoryWidgetData { get; private set; }

	// Token: 0x1700090B RID: 2315
	// (get) Token: 0x06003BFA RID: 15354 RVA: 0x0011E516 File Offset: 0x0011C716
	// (set) Token: 0x06003BFB RID: 15355 RVA: 0x0011E51E File Offset: 0x0011C71E
	public BodyPocketInventoryWidgetData BodyPocketInventoryWidgetData { get; private set; }

	// Token: 0x1700090C RID: 2316
	// (get) Token: 0x06003BFC RID: 15356 RVA: 0x0011E527 File Offset: 0x0011C727
	// (set) Token: 0x06003BFD RID: 15357 RVA: 0x0011E52F File Offset: 0x0011C72F
	public UIInfoWidgetData InfoWidgetData { get; set; }

	// Token: 0x06003BFE RID: 15358 RVA: 0x0011E538 File Offset: 0x0011C738
	public UIAutopsyWindowData(WgoData wgoData)
	{
		this.autopsyTable = wgoData;
		foreach (Item item in wgoData.Inventory.Data.Inventory)
		{
			if (item.Definition.itemGroupIds.Contains("body"))
			{
				this.bodyItem = item;
				this.zombieWgoData = MainGame.ZombieSystemData.GetZombie(this.bodyItem.UniqueId);
				this.isZombie = this.zombieWgoData != null;
				break;
			}
		}
		this.IsEmpty = this.bodyItem == null;
		this.InfoWidgetData = new UIInfoWidgetData(wgoData, null, true);
		if (!this.IsEmpty)
		{
			this.CorpseWidgetData = new UICorpseWidgetData(this.bodyItem, wgoData, new Action(this.TakeBody), !this.IsEmpty, GameKey.ExtractBody, LLBase.L("ui_grave_corpse_widget_header"), null, LLBase.L("btn_take_body_two_lines"), null);
			if (this.isZombie)
			{
				Item collar = this.zombieWgoData.Collar;
				if (collar != null && !collar.IsEmpty && collar.Definition.redSkullsMaxCollar > collar.Definition.redSkullsMinCollar)
				{
					this.CorpseWidgetData.CollarRedSkullsLimit = collar.Definition.redSkullsMaxCollar;
				}
			}
			Inventory inventory = new Inventory(this.bodyItem);
			this.BodyOrgansInventoryWidgetData = new BodyOrgansInventoryWidgetData(true, wgoData, MainGame.ZombieSystemData.GetZombie(this.bodyItem.UniqueId), inventory, null, null, new Action<UIItemCell>(this.OnItemCellPressOrgans));
			this.BodyPocketInventoryWidgetData = new BodyPocketInventoryWidgetData(true, wgoData, MainGame.ZombieSystemData.GetZombie(this.bodyItem.UniqueId), inventory, null, null, new Action<UIItemCell>(this.OnItemCellPressPocket));
			return;
		}
		this.CorpseWidgetData = new UICorpseWidgetData(GameKey.ExtractBody);
		this.BodyPocketInventoryWidgetData = new BodyPocketInventoryWidgetData();
		this.BodyOrgansInventoryWidgetData = new BodyOrgansInventoryWidgetData();
	}

	// Token: 0x06003BFF RID: 15359 RVA: 0x0011E734 File Offset: 0x0011C934
	private void TakeBody()
	{
		if (this.IsEmpty)
		{
			return;
		}
		PlayerData playerData = MainGame.PlayerData;
		if (!playerData.HasFreeOverheadSlot)
		{
			MainGame.Instance.dropSystem.DropItem(this.bodyItem, this.autopsyTable.WorldId, playerData.position.Value, null);
		}
		else
		{
			playerData.AddOverheadItem(this.bodyItem);
		}
		this.autopsyTable.Inventory.RemoveItemFromInventoryByUID(this.bodyItem, -1);
		Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(this.autopsyTable.UniqueId);
		if (wgoViewGlobal != null)
		{
			wgoViewGlobal.DrawWidgets();
		}
		LazyUI.GetWindow<UIAutopsyWindow>().Close();
	}

	// Token: 0x06003C00 RID: 15360 RVA: 0x0011E7D0 File Offset: 0x0011C9D0
	private void OnItemCellPressPocket(UIItemCell itemCell)
	{
		if (itemCell.DisplayingItem != null && !itemCell.DisplayingItem.IsEmpty)
		{
			this.TryExtractItemFromPocket(itemCell);
		}
	}

	// Token: 0x06003C01 RID: 15361 RVA: 0x0011E7F0 File Offset: 0x0011C9F0
	private void OnItemCellPressOrgans(UIItemCell itemCell)
	{
		this.selectedItemType = itemCell.GetComponentInParent<UIFixedTypeItemCell>().ItemType;
		if (itemCell.DisplayingItem == null)
		{
			LazyWindow<UIMultiInventoryWindowData> window = LazyUI.GetWindow<UIMultiInventoryWindow>();
			UIMultiInventoryWindowData uimultiInventoryWindowData = new UIMultiInventoryWindowData(MainGame.PlayerData, new Action<UIItemCell>(this.TryInsertMainOrgan), new Func<Item, bool>(this.CanInsertOrgan), true, null, null);
			window.Open(uimultiInventoryWindowData);
			this.currentMultiInventory = uimultiInventoryWindowData.MultiInventory;
			return;
		}
		if (itemCell.DisplayingItem.Definition.isOrganMistake)
		{
			return;
		}
		if (this.isZombie)
		{
			LazyWindow<UIMultiInventoryWindowData> window2 = LazyUI.GetWindow<UIMultiInventoryWindow>();
			UIMultiInventoryWindowData uimultiInventoryWindowData2 = new UIMultiInventoryWindowData(MainGame.PlayerData, new Action<UIItemCell>(this.TryChangeMainOrgan), new Func<Item, bool>(this.CanChangeOrgan), true, "change_organ", new Func<Item, string>(this.GetOrganChangeFailedTooltipLocId));
			window2.Open(uimultiInventoryWindowData2);
			this.currentMultiInventory = uimultiInventoryWindowData2.MultiInventory;
			return;
		}
		this.TryExtractMainOrgan(itemCell);
	}

	// Token: 0x06003C02 RID: 15362 RVA: 0x0011E8C4 File Offset: 0x0011CAC4
	private bool CanInsertOrgan(Item item)
	{
		if (item == null || item.IsEmpty)
		{
			return false;
		}
		bool flag = !this.isZombie || this.zombieWgoData.CanAddItemToBody(item);
		return item.Definition.type == this.selectedItemType && !item.Definition.isOrganMistake && flag;
	}

	// Token: 0x06003C03 RID: 15363 RVA: 0x0011E91C File Offset: 0x0011CB1C
	private bool CanChangeOrgan(Item item)
	{
		if (item == null || item.IsEmpty)
		{
			return false;
		}
		bool flag = this.zombieWgoData.CanChangeItemInBody(this.zombieWgoData.ZombieItem.GetItemByType(this.selectedItemType), item);
		return item.Definition.type == this.selectedItemType && !item.Definition.isOrganMistake && flag;
	}

	// Token: 0x06003C04 RID: 15364 RVA: 0x0011E980 File Offset: 0x0011CB80
	private string GetOrganChangeFailedTooltipLocId(Item item)
	{
		if (item == null || item.IsEmpty)
		{
			return null;
		}
		if (item.Definition.type != this.selectedItemType || item.Definition.isOrganMistake)
		{
			return null;
		}
		if (this.zombieWgoData.CanChangeItemInBody(this.zombieWgoData.ZombieItem.GetItemByType(this.selectedItemType), item))
		{
			return null;
		}
		return "ui_operation_failed_desc";
	}

	// Token: 0x06003C05 RID: 15365 RVA: 0x0011E9E8 File Offset: 0x0011CBE8
	private void ShowZombieRelatedFailedOperationWindow()
	{
		UIDialogWindowData uidialogWindowData = new UIDialogWindowData(LLBase.L("ui_operation_failed_header"), LLBase.L("ui_operation_failed_desc"), new UIDialogWindowData.ButtonData(new Action(LazyUI.GetWindow<UIDialogWindow>().Close), LLBase.L("btn_ok"), null, true, GameKey.Select, ""));
		LazyUI.GetWindow<UIDialogWindow>().Open(uidialogWindowData);
	}

	// Token: 0x06003C06 RID: 15366 RVA: 0x0011EA48 File Offset: 0x0011CC48
	private void TryExtractItemFromPocket(UIItemCell itemCell)
	{
		UIAutopsyWindowData.<>c__DisplayClass34_0 CS$<>8__locals1 = new UIAutopsyWindowData.<>c__DisplayClass34_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.item = itemCell.DisplayingItem;
		if (this.isZombie && !this.zombieWgoData.CanRemoveItemFromBody(CS$<>8__locals1.item))
		{
			this.ShowZombieRelatedFailedOperationWindow();
			return;
		}
		CS$<>8__locals1.extractCraft = GameBalance.GetAutopsyCraftDef(AutopsyTypeCraft.PocketExtract, "");
		if (CS$<>8__locals1.extractCraft == null)
		{
			Debug.LogError("Can not get extract organ craft for pockets [" + CS$<>8__locals1.item.id + "]");
			return;
		}
		UIDialogWindowData uidialogWindowData = new UIDialogWindowData(LLBase.L("ui_extract_item_header"), LLBase.L("ui_extract_item_desc"), new Action(CS$<>8__locals1.<TryExtractItemFromPocket>g__OnCraftStartPressed|0), new Action(LazyUI.GetWindow<UIDialogWindow>().Close), false);
		LazyUI.GetWindow<UIDialogWindow>().Open(uidialogWindowData);
	}

	// Token: 0x06003C07 RID: 15367 RVA: 0x0011EB0C File Offset: 0x0011CD0C
	private void TryExtractMainOrgan(UIItemCell itemCell)
	{
		UIAutopsyWindowData.<>c__DisplayClass35_0 CS$<>8__locals1 = new UIAutopsyWindowData.<>c__DisplayClass35_0();
		CS$<>8__locals1.<>4__this = this;
		if (this.isZombie && !this.zombieWgoData.CanRemoveItemFromBody(itemCell.DisplayingItem))
		{
			this.ShowZombieRelatedFailedOperationWindow();
			return;
		}
		CS$<>8__locals1.extractCraft = GameBalance.GetAutopsyCraftDef(AutopsyTypeCraft.ExtractOrgan, itemCell.DisplayingItem.id);
		if (CS$<>8__locals1.extractCraft == null)
		{
			Debug.LogError("Can not get extract organ craft for organ [" + itemCell.DisplayingItem.id + "]");
			return;
		}
		UICraftSelectionWindowData uicraftSelectionWindowData = new UICraftSelectionWindowData(this.autopsyTable, CS$<>8__locals1.extractCraft, null, new Action<CraftDef, List<NeedItemData>, CraftParamsData, int>(CS$<>8__locals1.<TryExtractMainOrgan>g__OnCraftStartPressed|0));
		LazyUI.GetWindow<UICraftSelectionWindow>().Open(uicraftSelectionWindowData);
	}

	// Token: 0x06003C08 RID: 15368 RVA: 0x0011EBB4 File Offset: 0x0011CDB4
	private void TryInsertMainOrgan(UIItemCell itemCell)
	{
		UIAutopsyWindowData.<>c__DisplayClass36_0 CS$<>8__locals1 = new UIAutopsyWindowData.<>c__DisplayClass36_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.item = itemCell.DisplayingItem;
		if (this.isZombie && !this.zombieWgoData.CanAddItemToBody(CS$<>8__locals1.item))
		{
			LazyUI.GetWindow<UIMultiInventoryWindow>().Close();
			this.ShowZombieRelatedFailedOperationWindow();
			return;
		}
		CS$<>8__locals1.insertionCraft = GameBalance.GetAutopsyCraftDef(AutopsyTypeCraft.InsertOrgan, CS$<>8__locals1.item.id);
		if (CS$<>8__locals1.insertionCraft == null)
		{
			Debug.LogError(string.Format("Can not get insertion organ craft for organ type [{0}]", this.selectedItemType));
			return;
		}
		LazyUI.GetWindow<UIMultiInventoryWindow>().Close();
		UICraftSelectionWindowData uicraftSelectionWindowData = new UICraftSelectionWindowData(this.autopsyTable, CS$<>8__locals1.insertionCraft, null, new Action<CraftDef, List<NeedItemData>, CraftParamsData, int>(CS$<>8__locals1.<TryInsertMainOrgan>g__OnCraftStartPressed|0));
		LazyUI.GetWindow<UICraftSelectionWindow>().Open(uicraftSelectionWindowData);
	}

	// Token: 0x06003C09 RID: 15369 RVA: 0x0011EC74 File Offset: 0x0011CE74
	private void TryChangeMainOrgan(UIItemCell itemCell)
	{
		UIAutopsyWindowData.<>c__DisplayClass37_0 CS$<>8__locals1 = new UIAutopsyWindowData.<>c__DisplayClass37_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.item = itemCell.DisplayingItem;
		if (this.isZombie && !this.zombieWgoData.CanChangeItemInBody(this.zombieWgoData.ZombieItem.GetItemByType(this.selectedItemType), CS$<>8__locals1.item))
		{
			LazyUI.GetWindow<UIMultiInventoryWindow>().Close();
			this.ShowZombieRelatedFailedOperationWindow();
			return;
		}
		CS$<>8__locals1.changeCraft = GameBalance.GetAutopsyCraftDef(AutopsyTypeCraft.ChangeOrgan, CS$<>8__locals1.item.id);
		if (CS$<>8__locals1.changeCraft == null)
		{
			Debug.LogError(string.Format("Can not get change organ craft for organ type [{0}]", this.selectedItemType));
			return;
		}
		LazyUI.GetWindow<UIMultiInventoryWindow>().Close();
		UICraftSelectionWindowData uicraftSelectionWindowData = new UICraftSelectionWindowData(this.autopsyTable, CS$<>8__locals1.changeCraft, null, new Action<CraftDef, List<NeedItemData>, CraftParamsData, int>(CS$<>8__locals1.<TryChangeMainOrgan>g__OnCraftStartPressed|0));
		LazyUI.GetWindow<UICraftSelectionWindow>().Open(uicraftSelectionWindowData);
	}

	// Token: 0x04002F37 RID: 12087
	private Item bodyItem;

	// Token: 0x04002F38 RID: 12088
	private WgoData autopsyTable;

	// Token: 0x04002F39 RID: 12089
	private ZombieWgoData zombieWgoData;

	// Token: 0x04002F3A RID: 12090
	private bool isZombie;

	// Token: 0x04002F3B RID: 12091
	private ItemType selectedItemType;

	// Token: 0x04002F3C RID: 12092
	private MultiInventory currentMultiInventory;
}
