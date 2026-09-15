using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000938 RID: 2360
public class CharMainPageWidgetData : LazyWidgetDataBase
{
	// Token: 0x1700095D RID: 2397
	// (get) Token: 0x06003E34 RID: 15924 RVA: 0x00129330 File Offset: 0x00127530
	// (set) Token: 0x06003E35 RID: 15925 RVA: 0x00129338 File Offset: 0x00127538
	public PlayerData PlayerData { get; private set; }

	// Token: 0x1700095E RID: 2398
	// (get) Token: 0x06003E36 RID: 15926 RVA: 0x00129341 File Offset: 0x00127541
	// (set) Token: 0x06003E37 RID: 15927 RVA: 0x00129349 File Offset: 0x00127549
	public MultiInventoryWidgetData MultiInventoryWidgetData { get; private set; }

	// Token: 0x1700095F RID: 2399
	// (get) Token: 0x06003E38 RID: 15928 RVA: 0x00129352 File Offset: 0x00127552
	// (set) Token: 0x06003E39 RID: 15929 RVA: 0x0012935A File Offset: 0x0012755A
	public ToolBeltInventoryWidgetData ToolBeltInventoryWidgetData { get; private set; }

	// Token: 0x17000960 RID: 2400
	// (get) Token: 0x06003E3A RID: 15930 RVA: 0x00129363 File Offset: 0x00127563
	// (set) Token: 0x06003E3B RID: 15931 RVA: 0x0012936B File Offset: 0x0012756B
	public PerksWidgetData PerksWidgetData { get; private set; }

	// Token: 0x17000961 RID: 2401
	// (get) Token: 0x06003E3C RID: 15932 RVA: 0x00129374 File Offset: 0x00127574
	// (set) Token: 0x06003E3D RID: 15933 RVA: 0x0012937C File Offset: 0x0012757C
	public PerksWidgetData BuffsWidgetData { get; private set; }

	// Token: 0x17000962 RID: 2402
	// (get) Token: 0x06003E3E RID: 15934 RVA: 0x00129385 File Offset: 0x00127585
	// (set) Token: 0x06003E3F RID: 15935 RVA: 0x0012938D File Offset: 0x0012758D
	public MoneyWidgetData MoneyWidgetData { get; private set; }

	// Token: 0x17000963 RID: 2403
	// (get) Token: 0x06003E40 RID: 15936 RVA: 0x00129396 File Offset: 0x00127596
	public BagInventoryWidgetData BagInventoryWidgetData
	{
		get
		{
			return this.playerInventoryUIItemOpHandler.BagInventoryWidgetData;
		}
	}

	// Token: 0x17000964 RID: 2404
	// (get) Token: 0x06003E41 RID: 15937 RVA: 0x001293A3 File Offset: 0x001275A3
	public bool IsBagShown
	{
		get
		{
			return this.playerInventoryUIItemOpHandler.IsBagShown;
		}
	}

	// Token: 0x17000965 RID: 2405
	// (get) Token: 0x06003E42 RID: 15938 RVA: 0x001293B0 File Offset: 0x001275B0
	// (set) Token: 0x06003E43 RID: 15939 RVA: 0x001293B8 File Offset: 0x001275B8
	public Action OnMoveAllSimilarItemFromPlayerToBag { get; private set; }

	// Token: 0x17000966 RID: 2406
	// (get) Token: 0x06003E44 RID: 15940 RVA: 0x001293C1 File Offset: 0x001275C1
	// (set) Token: 0x06003E45 RID: 15941 RVA: 0x001293CE File Offset: 0x001275CE
	public Action<Item> OnBagHide
	{
		get
		{
			return this.playerInventoryUIItemOpHandler.OnBagHide;
		}
		set
		{
			this.playerInventoryUIItemOpHandler.OnBagHide = value;
		}
	}

	// Token: 0x17000967 RID: 2407
	// (get) Token: 0x06003E46 RID: 15942 RVA: 0x001293DC File Offset: 0x001275DC
	// (set) Token: 0x06003E47 RID: 15943 RVA: 0x001293E9 File Offset: 0x001275E9
	public Action<Item> OnBagShow
	{
		get
		{
			return this.playerInventoryUIItemOpHandler.OnBagShow;
		}
		set
		{
			this.playerInventoryUIItemOpHandler.OnBagShow = value;
		}
	}

	// Token: 0x17000968 RID: 2408
	// (get) Token: 0x06003E48 RID: 15944 RVA: 0x001293F7 File Offset: 0x001275F7
	// (set) Token: 0x06003E49 RID: 15945 RVA: 0x001293FF File Offset: 0x001275FF
	public Action OnHideBagPressed { get; private set; }

	// Token: 0x06003E4A RID: 15946 RVA: 0x00129408 File Offset: 0x00127608
	public CharMainPageWidgetData(GameSave gameSave)
	{
		CharMainPageWidgetData <>4__this = this;
		this.PlayerData = gameSave.playerData;
		this.MultiInventoryWidgetData = new MultiInventoryWidgetData(MultiInventoryWidgetMode.Default);
		this.playerInventoryUIItemOpHandler = new PlayerInventoryUIItemOpHandler(gameSave.playerData, this.MultiInventoryWidgetData);
		this.ToolBeltInventoryWidgetData = new ToolBeltInventoryWidgetData(gameSave.playerData.toolBeltInventory, delegate(UIItemCell _)
		{
			LazyAudio.PlayAndForget("gui_hover_light");
		}, null, new Action<UIItemCell>(this.playerInventoryUIItemOpHandler.TryUnEquipItem), new Action<UIItemCell>(this.playerInventoryUIItemOpHandler.TryUnEquipItem), null, new Func<Item, bool>(this.playerInventoryUIItemOpHandler.ToolBeltItemsAvailabilityCondition), null, ItemRelatedWidgetState.Default);
		List<InventoryWidgetDataBase> widgetsDataForInventory = InventoryWidgetDataHelper.GetWidgetsDataForInventory(gameSave.playerData.inventory, delegate(UIItemCell _)
		{
			LazyAudio.PlayAndForget("gui_hover_light");
		}, null, new Action<UIItemCell>(this.playerInventoryUIItemOpHandler.OnPlayerInvItemPressed), new Action<UIItemCell>(this.playerInventoryUIItemOpHandler.OnPlayerInvItemPressed2), new Action<UIItemCell>(this.playerInventoryUIItemOpHandler.OnPlayerInventoryPressedDown), new Func<Item, bool>(this.playerInventoryUIItemOpHandler.PlayerItemsAvailabilityCondition), true, false, ItemRelatedWidgetState.Default, ItemRelatedWidgetState.Default);
		if (widgetsDataForInventory.Count > 0)
		{
			widgetsDataForInventory[0].CustomItemSelectedCondition = new Func<Item, bool>(this.playerInventoryUIItemOpHandler.IsShownBag);
		}
		this.MultiInventoryWidgetData.AddRange(widgetsDataForInventory);
		this.OnHideBagPressed = new Action(this.playerInventoryUIItemOpHandler.HideBag);
		this.OnMoveAllSimilarItemFromPlayerToBag = delegate
		{
			<>4__this.BagInventoryWidgetData.Inventory.TakeAllItemsExistingInMeFromOtherInventory(<>4__this.MultiInventoryWidgetData.SelectedWidgetData.Inventory, true, true);
		};
		this.MultiInventoryWidgetData.GetMoveAllSimilarTargetInventory = delegate
		{
			BagInventoryWidgetData bagInventoryWidgetData = <>4__this.BagInventoryWidgetData;
			if (bagInventoryWidgetData == null)
			{
				return null;
			}
			return bagInventoryWidgetData.Inventory;
		};
		if (gameSave.playerData.CurrentWorldZoneData != null)
		{
			this.MultiInventoryWidgetData.AddRange(InventoryWidgetDataHelper.GetWidgetsDataForMultiInventory(new MultiInventory(gameSave.playerData.CurrentWorldZoneData, null, false), null, null, null, null, null, new Func<Item, bool>(this.playerInventoryUIItemOpHandler.PlayerItemsAvailabilityCondition), true, false, ItemRelatedWidgetState.Disabled, ItemRelatedWidgetState.Disabled));
		}
		this.PerksWidgetData = new PerksWidgetData(gameSave, new List<PerkType> { PerkType.Default });
		this.BuffsWidgetData = new PerksWidgetData(gameSave, new List<PerkType> { PerkType.Buff });
		this.MoneyWidgetData = new MoneyWidgetData();
		this.MoneyWidgetData.Money = () => gameSave.playerData.GetResInt("money");
	}

	// Token: 0x06003E4B RID: 15947 RVA: 0x00129678 File Offset: 0x00127878
	public void HideBag()
	{
		PlayerInventoryUIItemOpHandler playerInventoryUIItemOpHandler = this.playerInventoryUIItemOpHandler;
		if (playerInventoryUIItemOpHandler == null)
		{
			return;
		}
		playerInventoryUIItemOpHandler.HideBag();
	}

	// Token: 0x040030FF RID: 12543
	private PlayerInventoryUIItemOpHandler playerInventoryUIItemOpHandler;
}
