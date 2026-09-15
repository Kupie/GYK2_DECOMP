using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x020009DB RID: 2523
public class UIGardenBedWindowData : LazyWidgetDataBase
{
	// Token: 0x17000A3B RID: 2619
	// (get) Token: 0x06004387 RID: 17287 RVA: 0x00141716 File Offset: 0x0013F916
	// (set) Token: 0x06004388 RID: 17288 RVA: 0x0014171E File Offset: 0x0013F91E
	public CraftElement CraftElement { get; private set; }

	// Token: 0x17000A3C RID: 2620
	// (get) Token: 0x06004389 RID: 17289 RVA: 0x00141727 File Offset: 0x0013F927
	// (set) Token: 0x0600438A RID: 17290 RVA: 0x0014172F File Offset: 0x0013F92F
	public CraftDef CraftDefinition { get; private set; }

	// Token: 0x17000A3D RID: 2621
	// (get) Token: 0x0600438B RID: 17291 RVA: 0x00141738 File Offset: 0x0013F938
	// (set) Token: 0x0600438C RID: 17292 RVA: 0x00141740 File Offset: 0x0013F940
	public WgoData WgoData { get; private set; }

	// Token: 0x17000A3E RID: 2622
	// (get) Token: 0x0600438D RID: 17293 RVA: 0x00141749 File Offset: 0x0013F949
	// (set) Token: 0x0600438E RID: 17294 RVA: 0x00141751 File Offset: 0x0013F951
	public UIInfoWidgetData UIInfoWidgetData { get; private set; }

	// Token: 0x17000A3F RID: 2623
	// (get) Token: 0x0600438F RID: 17295 RVA: 0x0014175A File Offset: 0x0013F95A
	// (set) Token: 0x06004390 RID: 17296 RVA: 0x00141762 File Offset: 0x0013F962
	public List<PerkData> GardenPerks { get; private set; }

	// Token: 0x17000A40 RID: 2624
	// (get) Token: 0x06004391 RID: 17297 RVA: 0x0014176B File Offset: 0x0013F96B
	// (set) Token: 0x06004392 RID: 17298 RVA: 0x00141773 File Offset: 0x0013F973
	public bool IsGrowing { get; private set; }

	// Token: 0x06004393 RID: 17299 RVA: 0x0014177C File Offset: 0x0013F97C
	public UIGardenBedWindowData(WgoData wgoData)
	{
		this.WgoData = wgoData;
		this.CraftElement = ((wgoData.CraftComponent.CurrentCraftElement == null) ? null : (wgoData.CraftComponent.CurrentCraftElement as CraftElement));
		CraftElement craftElement = this.CraftElement;
		this.CraftDefinition = ((craftElement != null) ? craftElement.Definition : null);
		this.UIInfoWidgetData = new UIInfoWidgetData(wgoData, null, true);
		this.UIInfoWidgetData.ForcedWorker = MainGame.PlayerController;
		this.GardenPerks = new List<PerkData>();
		this.IsGrowing = this.CraftElement != null;
		foreach (PerkData perkData in this.WgoData.ActivePerks)
		{
			if (perkData.Definition.IsFertilizerPerk)
			{
				this.GardenPerks.Add(perkData);
			}
		}
	}

	// Token: 0x06004394 RID: 17300 RVA: 0x0014186C File Offset: 0x0013FA6C
	public void UpdateData()
	{
		this.CraftElement = ((this.WgoData.CraftComponent.CurrentCraftElement == null) ? null : (this.WgoData.CraftComponent.CurrentCraftElement as CraftElement));
		this.IsGrowing = this.CraftElement != null;
		CraftElement craftElement = this.CraftElement;
		this.CraftDefinition = ((craftElement != null) ? craftElement.Definition : null);
		this.UIInfoWidgetData = new UIInfoWidgetData(this.WgoData, null, true);
		this.UIInfoWidgetData.ForcedWorker = MainGame.PlayerController;
		this.GardenPerks = new List<PerkData>();
		foreach (PerkData perkData in this.WgoData.ActivePerks)
		{
			if (perkData.Definition.IsFertilizerPerk)
			{
				this.GardenPerks.Add(perkData);
			}
		}
	}

	// Token: 0x06004395 RID: 17301 RVA: 0x0014195C File Offset: 0x0013FB5C
	public void TrySetWorker()
	{
		if (this.WgoData.Worker == null)
		{
			this.wasPlayerSetAsWorker = true;
			this.WgoData.TrySetWorker(MainGame.PlayerController, null);
		}
	}

	// Token: 0x06004396 RID: 17302 RVA: 0x00141984 File Offset: 0x0013FB84
	public void TryRemoveWorker()
	{
		if (this.wasPlayerSetAsWorker)
		{
			this.wasPlayerSetAsWorker = false;
			this.WgoData.ClearWorker();
		}
	}

	// Token: 0x040034B0 RID: 13488
	private bool isSubscribedToDataChanges;

	// Token: 0x040034B1 RID: 13489
	private bool wasPlayerSetAsWorker;
}
