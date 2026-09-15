using System;
using System.Collections.Generic;
using System.Linq;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020009D6 RID: 2518
public class UIFishingWindowData : LazyWidgetDataBase
{
	// Token: 0x17000A36 RID: 2614
	// (get) Token: 0x06004345 RID: 17221 RVA: 0x0013FB51 File Offset: 0x0013DD51
	public Wgo Reservoir
	{
		get
		{
			return this.reservoir;
		}
	}

	// Token: 0x17000A37 RID: 2615
	// (get) Token: 0x06004346 RID: 17222 RVA: 0x0013FB59 File Offset: 0x0013DD59
	public ItemDef FishingRodDef
	{
		get
		{
			return this.fishingRodDef;
		}
	}

	// Token: 0x17000A38 RID: 2616
	// (get) Token: 0x06004347 RID: 17223 RVA: 0x0013FB61 File Offset: 0x0013DD61
	public List<Item> BaitItems
	{
		get
		{
			return this.baitItems;
		}
	}

	// Token: 0x17000A39 RID: 2617
	// (get) Token: 0x06004348 RID: 17224 RVA: 0x0013FB69 File Offset: 0x0013DD69
	public List<FishingDef> ReservoirFishings
	{
		get
		{
			return this.reservoirFishings;
		}
	}

	// Token: 0x06004349 RID: 17225 RVA: 0x0013FB71 File Offset: 0x0013DD71
	public UIFishingWindowData(Wgo reservoirWgo, ItemDef playerFishingRodDef)
	{
		this.reservoir = reservoirWgo;
		this.fishingRodDef = playerFishingRodDef;
		this.UpdateAvailableFishingAndBaits();
	}

	// Token: 0x0600434A RID: 17226 RVA: 0x0013FBA4 File Offset: 0x0013DDA4
	public void UpdateAvailableFishingAndBaits()
	{
		if (this.reservoir == null)
		{
			Debug.LogError("[UIFishingWindowData]: reservoir wgo is null");
			return;
		}
		if (this.reservoirFishings == null)
		{
			Debug.LogError("[UIFishingWindowData]: There's no available fishing for this reservoir wgo [" + this.reservoir.Data.id + "]");
			return;
		}
		this.reservoirFishings = GameBalance.Me.fishingDefs.FindAll((FishingDef x) => x.reservoirId.Equals(this.reservoir.Data.id));
		HashSet<string> hashSet = new HashSet<string>();
		for (int i = 0; i < this.reservoirFishings.Count; i++)
		{
			for (int j = 0; j < this.reservoirFishings[i].baitMod.List.Count; j++)
			{
				GameResAtom gameResAtom = this.reservoirFishings[i].baitMod.List[j];
				hashSet.Add(gameResAtom.type);
			}
		}
		hashSet.Add("no_bait");
		this.baitItems.Clear();
		List<string> list = hashSet.ToList<string>();
		for (int k = 0; k < list.Count; k++)
		{
			string text = list[k];
			if (text == "no_bait")
			{
				this.baitItems.Insert(0, new Item(text, 1));
			}
			else
			{
				this.baitItems.Add(new Item(text, MainGame.PlayerData.inventory.Data.GetTotalCountInInventory(text, null, false)));
			}
		}
	}

	// Token: 0x0400346F RID: 13423
	private Wgo reservoir;

	// Token: 0x04003470 RID: 13424
	private ItemDef fishingRodDef;

	// Token: 0x04003471 RID: 13425
	private List<FishingDef> reservoirFishings = new List<FishingDef>();

	// Token: 0x04003472 RID: 13426
	private List<Item> baitItems = new List<Item>();
}
