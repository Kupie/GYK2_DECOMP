using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020009BA RID: 2490
public class UIEmbalmWindowData : LazyWidgetDataBase
{
	// Token: 0x17000A0B RID: 2571
	// (get) Token: 0x06004255 RID: 16981 RVA: 0x0013B18B File Offset: 0x0013938B
	// (set) Token: 0x06004256 RID: 16982 RVA: 0x0013B193 File Offset: 0x00139393
	public bool IsEmpty { get; private set; }

	// Token: 0x17000A0C RID: 2572
	// (get) Token: 0x06004257 RID: 16983 RVA: 0x0013B19C File Offset: 0x0013939C
	// (set) Token: 0x06004258 RID: 16984 RVA: 0x0013B1A4 File Offset: 0x001393A4
	public UICorpseWidgetData CorpseWidgetData { get; private set; }

	// Token: 0x17000A0D RID: 2573
	// (get) Token: 0x06004259 RID: 16985 RVA: 0x0013B1AD File Offset: 0x001393AD
	// (set) Token: 0x0600425A RID: 16986 RVA: 0x0013B1B5 File Offset: 0x001393B5
	public BodyOrgansInventoryWidgetData BodyOrgansInventoryWidgetData { get; private set; }

	// Token: 0x17000A0E RID: 2574
	// (get) Token: 0x0600425B RID: 16987 RVA: 0x0013B1BE File Offset: 0x001393BE
	// (set) Token: 0x0600425C RID: 16988 RVA: 0x0013B1C6 File Offset: 0x001393C6
	public BodyPocketInventoryWidgetData BodyPocketInventoryWidgetData { get; private set; }

	// Token: 0x17000A0F RID: 2575
	// (get) Token: 0x0600425D RID: 16989 RVA: 0x0013B1CF File Offset: 0x001393CF
	// (set) Token: 0x0600425E RID: 16990 RVA: 0x0013B1D7 File Offset: 0x001393D7
	public UIInfoWidgetData InfoWidgetData { get; set; }

	// Token: 0x0600425F RID: 16991 RVA: 0x0013B1E0 File Offset: 0x001393E0
	public UIEmbalmWindowData(WgoData wgoData)
	{
		this.table = wgoData;
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
			Inventory inventory = new Inventory(this.bodyItem);
			this.BodyOrgansInventoryWidgetData = new BodyOrgansInventoryWidgetData(true, wgoData, MainGame.ZombieSystemData.GetZombie(this.bodyItem.UniqueId), inventory, null, null, null);
			this.BodyPocketInventoryWidgetData = new BodyPocketInventoryWidgetData(true, wgoData, MainGame.ZombieSystemData.GetZombie(this.bodyItem.UniqueId), inventory, null, null, new Action<UIItemCell>(this.OnItemCellPressPocket));
			this.BodyPocketInventoryWidgetData.FirstEmptyIsInteractable = true;
			return;
		}
		this.CorpseWidgetData = new UICorpseWidgetData(GameKey.ExtractBody);
		this.BodyPocketInventoryWidgetData = new BodyPocketInventoryWidgetData();
		this.BodyOrgansInventoryWidgetData = new BodyOrgansInventoryWidgetData();
	}

	// Token: 0x06004260 RID: 16992 RVA: 0x0013B390 File Offset: 0x00139590
	private void TakeBody()
	{
		if (this.IsEmpty)
		{
			return;
		}
		PlayerData playerData = MainGame.PlayerData;
		if (!playerData.HasFreeOverheadSlot)
		{
			MainGame.Instance.dropSystem.DropItem(this.bodyItem, this.table.WorldId, this.table.Position, null);
		}
		else
		{
			playerData.AddOverheadItem(this.bodyItem);
		}
		this.table.Inventory.RemoveItemFromInventoryByUID(this.bodyItem, -1);
		Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(this.table.UniqueId);
		if (wgoViewGlobal != null)
		{
			wgoViewGlobal.DrawWidgets();
		}
		LazyUI.GetWindow<UIEmbalmWindow>().Close();
	}

	// Token: 0x06004261 RID: 16993 RVA: 0x0013B42C File Offset: 0x0013962C
	private void OnItemCellPressPocket(UIItemCell itemCell)
	{
		if (itemCell.DisplayingItem == null || itemCell.DisplayingItem.IsEmpty)
		{
			LazyWindow<UIMultiInventoryWindowData> window = LazyUI.GetWindow<UIMultiInventoryWindow>();
			UIMultiInventoryWindowData uimultiInventoryWindowData = new UIMultiInventoryWindowData(MainGame.PlayerData, new Action<UIItemCell>(this.TryEmbalm), new Func<Item, bool>(this.CanEmbalm), true, null, null);
			window.Open(uimultiInventoryWindowData);
			this.currentMultiInventory = uimultiInventoryWindowData.MultiInventory;
		}
	}

	// Token: 0x06004262 RID: 16994 RVA: 0x0013B48C File Offset: 0x0013968C
	private void TryEmbalm(UIItemCell itemCell)
	{
		Item displayingItem = itemCell.DisplayingItem;
		if (!this.CanEmbalm(displayingItem))
		{
			return;
		}
		CraftDef autopsyCraftDef = GameBalance.GetAutopsyCraftDef(AutopsyTypeCraft.Embalm, displayingItem.id);
		if (autopsyCraftDef == null)
		{
			Debug.LogError(string.Format("Can not get insertion craft for embalm item [{0}]", displayingItem));
			return;
		}
		Item item = new Item(displayingItem.id, 1);
		CraftElement craftElement = new CraftElement(autopsyCraftDef.id, 1, new List<NeedItemData>(), new CraftParamsData(autopsyCraftDef.id, new GameRes()));
		craftElement.SetCustomItems(new List<Item> { item });
		this.table.CraftComponent.TryStartCraft(craftElement);
		this.currentMultiInventory.RemoveItemFromInventoryByUID(displayingItem, 1);
		LazyUI.GetWindow<UIMultiInventoryWindow>().Close();
		LazyUI.GetWindow<UIEmbalmWindow>().Close();
	}

	// Token: 0x06004263 RID: 16995 RVA: 0x0013B540 File Offset: 0x00139740
	private bool CanEmbalm(Item item)
	{
		return item != null && !item.IsEmpty && item.Definition.type == ItemType.Embalm && this.bodyItem != null && BodyPocketInventoryWidget.GetOccupiedSlotCount(this.bodyItem, this.zombieWgoData) < 6 && (!this.isZombie || this.zombieWgoData.CanAddItemToBody(item));
	}

	// Token: 0x040033C0 RID: 13248
	private Item bodyItem;

	// Token: 0x040033C1 RID: 13249
	private WgoData table;

	// Token: 0x040033C2 RID: 13250
	private ZombieWgoData zombieWgoData;

	// Token: 0x040033C3 RID: 13251
	private bool isZombie;

	// Token: 0x040033C4 RID: 13252
	private MultiInventory currentMultiInventory;
}
