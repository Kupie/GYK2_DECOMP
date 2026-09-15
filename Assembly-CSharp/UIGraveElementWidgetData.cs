using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x020009E2 RID: 2530
public class UIGraveElementWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000A51 RID: 2641
	// (get) Token: 0x060043CA RID: 17354 RVA: 0x00142527 File Offset: 0x00140727
	// (set) Token: 0x060043CB RID: 17355 RVA: 0x0014252F File Offset: 0x0014072F
	public Item GraveElementItem { get; private set; }

	// Token: 0x17000A52 RID: 2642
	// (get) Token: 0x060043CC RID: 17356 RVA: 0x00142538 File Offset: 0x00140738
	// (set) Token: 0x060043CD RID: 17357 RVA: 0x00142540 File Offset: 0x00140740
	public bool IsEmpty { get; private set; }

	// Token: 0x17000A53 RID: 2643
	// (get) Token: 0x060043CE RID: 17358 RVA: 0x00142549 File Offset: 0x00140749
	// (set) Token: 0x060043CF RID: 17359 RVA: 0x00142551 File Offset: 0x00140751
	public GraveElementType GraveElementType { get; private set; }

	// Token: 0x17000A54 RID: 2644
	// (get) Token: 0x060043D0 RID: 17360 RVA: 0x0014255A File Offset: 0x0014075A
	// (set) Token: 0x060043D1 RID: 17361 RVA: 0x00142562 File Offset: 0x00140762
	public int Quality { get; private set; }

	// Token: 0x17000A55 RID: 2645
	// (get) Token: 0x060043D2 RID: 17362 RVA: 0x0014256B File Offset: 0x0014076B
	// (set) Token: 0x060043D3 RID: 17363 RVA: 0x00142573 File Offset: 0x00140773
	public string EmptyDescriptionLocale { get; private set; }

	// Token: 0x17000A56 RID: 2646
	// (get) Token: 0x060043D4 RID: 17364 RVA: 0x0014257C File Offset: 0x0014077C
	public bool HasRequiredTool
	{
		get
		{
			this.requiredTool = ItemType.None;
			return !(this.grave == null) && MainGame.PlayerController.HasToolForWork(this.grave.Data, out this.requiredTool);
		}
	}

	// Token: 0x17000A57 RID: 2647
	// (get) Token: 0x060043D5 RID: 17365 RVA: 0x001425B0 File Offset: 0x001407B0
	public ItemType RequiredTool
	{
		get
		{
			return this.requiredTool;
		}
	}

	// Token: 0x060043D6 RID: 17366 RVA: 0x001425B8 File Offset: 0x001407B8
	public UIGraveElementWidgetData()
	{
		this.IsEmpty = false;
	}

	// Token: 0x060043D7 RID: 17367 RVA: 0x001425C8 File Offset: 0x001407C8
	public UIGraveElementWidgetData(Wgo grave, GraveElementType graveElementType)
	{
		this.IsEmpty = true;
		this.grave = grave;
		this.GraveElementType = graveElementType;
		this.TryGetGravePartItem();
		GraveElementType graveElementType2 = this.GraveElementType;
		if (graveElementType2 != GraveElementType.Top)
		{
			if (graveElementType2 == GraveElementType.Bot)
			{
				this.EmptyDescriptionLocale = "ui_fence_empty";
			}
		}
		else
		{
			this.EmptyDescriptionLocale = "ui_tombstone_empty";
		}
		this.Quality = (this.IsEmpty ? 0 : this.GraveElementItem.Definition.quality);
		this.onElementClicked = new Action(this.OnGraveElementClicked);
	}

	// Token: 0x060043D8 RID: 17368 RVA: 0x00142654 File Offset: 0x00140854
	private void TryGetGravePartItem()
	{
		foreach (Item item in this.grave.Data.Inventory.Data.Inventory)
		{
			GraveElementType graveElementType = this.GraveElementType;
			if (graveElementType != GraveElementType.Top)
			{
				if (graveElementType == GraveElementType.Bot)
				{
					if (item.Definition.itemGroupIds.Contains("gravebot"))
					{
						this.GraveElementItem = item;
						this.IsEmpty = false;
						break;
					}
				}
			}
			else if (item.Definition.itemGroupIds.Contains("gravetop"))
			{
				this.GraveElementItem = item;
				this.IsEmpty = false;
				break;
			}
		}
	}

	// Token: 0x060043D9 RID: 17369 RVA: 0x00142714 File Offset: 0x00140914
	private void OnGraveElementClicked()
	{
		if (this.IsEmpty)
		{
			LazyWindow<UIMultiInventoryWindowData> window = LazyUI.GetWindow<UIMultiInventoryWindow>();
			UIMultiInventoryWindowData uimultiInventoryWindowData = new UIMultiInventoryWindowData(MainGame.PlayerData, new Action<UIItemCell>(this.OnGraveItemSelected), new Func<Item, bool>(this.IsGraveItem), true, null, null);
			window.Open(uimultiInventoryWindowData);
			return;
		}
		this.OpenGravePartRemoveWindow();
	}

	// Token: 0x060043DA RID: 17370 RVA: 0x00142764 File Offset: 0x00140964
	private void OnGraveItemSelected(UIItemCell itemCell)
	{
		UIMultiInventoryWindow window = LazyUI.GetWindow<UIMultiInventoryWindow>();
		UIGraveWindow window2 = LazyUI.GetWindow<UIGraveWindow>();
		bool flag = false;
		if (this.grave.Data.CraftComponent.HasCraftsByBalance)
		{
			foreach (CraftDefBase craftDefBase in this.grave.Data.CraftComponent.AvailableCrafts)
			{
				using (List<NeedItemData>.Enumerator enumerator2 = craftDefBase.needItems.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.id == itemCell.DisplayingItem.id)
						{
							this.grave.Data.CraftComponent.TryStartCraft(new CraftElement(craftDefBase, new CraftParamsData(craftDefBase.id, this.grave.Data, CraftParamsData.CraftParamsType.Common, -1)));
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		window2.Close();
		window.Close();
	}

	// Token: 0x060043DB RID: 17371 RVA: 0x0014288C File Offset: 0x00140A8C
	private bool IsGraveItem(Item item)
	{
		if (item == null)
		{
			return false;
		}
		GraveElementType graveElementType = this.GraveElementType;
		if (graveElementType != GraveElementType.Top)
		{
			return graveElementType == GraveElementType.Bot && item.Definition.itemGroupIds.Contains("gravebot");
		}
		return item.Definition.itemGroupIds.Contains("gravetop");
	}

	// Token: 0x060043DC RID: 17372 RVA: 0x001428DC File Offset: 0x00140ADC
	private void OpenGravePartRemoveWindow()
	{
		CraftDef craftDef = UIBaseCraftWindowData.FindGravePartRemoveCraft(this.grave.Data.CraftComponent, this.GraveElementItem);
		if (craftDef == null)
		{
			return;
		}
		UICraftSelectionWindowData uicraftSelectionWindowData = new UICraftSelectionWindowData(this.grave.Data, craftDef, new Action<CraftDef, List<NeedItemData>, CraftParamsData, int>(this.OnGravePartRemoveConfirmed), new Action<CraftDef, List<NeedItemData>, CraftParamsData, int>(this.OnGravePartRemoveConfirmed));
		uicraftSelectionWindowData.IsGravePartRemove = true;
		LazyUI.GetWindow<UICraftSelectionWindow>().Open(uicraftSelectionWindowData);
	}

	// Token: 0x060043DD RID: 17373 RVA: 0x00142945 File Offset: 0x00140B45
	private void OnGravePartRemoveConfirmed(CraftDef craftDefinition, List<NeedItemData> selectedNeedItems, CraftParamsData craftParams, int craftsCount)
	{
		this.OnCraftPressed(new CraftElement(craftDefinition.id, craftsCount, selectedNeedItems, craftParams));
	}

	// Token: 0x060043DE RID: 17374 RVA: 0x0014295C File Offset: 0x00140B5C
	private void OnCraftPressed(CraftElement craftElement)
	{
		if (this.grave.Data.CraftComponent.IsStarted || this.grave.Data.CraftComponent.IsQueueDelayed)
		{
			return;
		}
		if (this.grave.Data.CraftComponent.TryStartCraft(craftElement))
		{
			LazyUI.GetWindow<UIGraveWindow>().Close();
		}
	}

	// Token: 0x040034EA RID: 13546
	public Action onElementClicked;

	// Token: 0x040034F0 RID: 13552
	private Wgo grave;

	// Token: 0x040034F1 RID: 13553
	private ItemType requiredTool;
}
