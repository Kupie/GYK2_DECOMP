using System;
using LazyBearTechnology;

// Token: 0x02000919 RID: 2329
public class InspirationWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000938 RID: 2360
	// (get) Token: 0x06003D61 RID: 15713 RVA: 0x001257D0 File Offset: 0x001239D0
	public int CurrentLevel
	{
		get
		{
			if (this.InspirationData != null)
			{
				return this.InspirationData.curLevel;
			}
			return this.InspirationDef.lvl;
		}
	}

	// Token: 0x17000939 RID: 2361
	// (get) Token: 0x06003D62 RID: 15714 RVA: 0x001257F4 File Offset: 0x001239F4
	public int CurrentLevelFrame
	{
		get
		{
			InspirationDef inspirationDef = null;
			if (this.InspirationData != null)
			{
				inspirationDef = InspirationDef.GetDataForLevel(this.InspirationData.id, this.InspirationData.curLevel);
			}
			if (inspirationDef == null)
			{
				inspirationDef = this.InspirationDef;
			}
			if (inspirationDef.lvlFrame > 0)
			{
				return inspirationDef.lvlFrame;
			}
			return inspirationDef.lvl;
		}
	}

	// Token: 0x1700093A RID: 2362
	// (get) Token: 0x06003D63 RID: 15715 RVA: 0x00125847 File Offset: 0x00123A47
	public string IdWithoutLevel
	{
		get
		{
			if (this.InspirationData != null)
			{
				return this.InspirationData.id;
			}
			return this.InspirationDef.idWithoutLvl;
		}
	}

	// Token: 0x1700093B RID: 2363
	// (get) Token: 0x06003D64 RID: 15716 RVA: 0x00125868 File Offset: 0x00123A68
	public int Price
	{
		get
		{
			if (this.InspirationData != null)
			{
				return InspirationDef.GetDataForLevel(this.InspirationData.id, this.InspirationData.curLevel).completionPrice;
			}
			return this.InspirationDef.completionPrice;
		}
	}

	// Token: 0x1700093C RID: 2364
	// (get) Token: 0x06003D65 RID: 15717 RVA: 0x0012589E File Offset: 0x00123A9E
	// (set) Token: 0x06003D66 RID: 15718 RVA: 0x001258A6 File Offset: 0x00123AA6
	public Action OnPress { get; private set; }

	// Token: 0x1700093D RID: 2365
	// (get) Token: 0x06003D67 RID: 15719 RVA: 0x001258AF File Offset: 0x00123AAF
	// (set) Token: 0x06003D68 RID: 15720 RVA: 0x001258B7 File Offset: 0x00123AB7
	public InspirationData InspirationData { get; private set; }

	// Token: 0x1700093E RID: 2366
	// (get) Token: 0x06003D69 RID: 15721 RVA: 0x001258C0 File Offset: 0x00123AC0
	// (set) Token: 0x06003D6A RID: 15722 RVA: 0x001258C8 File Offset: 0x00123AC8
	public InspirationDef InspirationDef { get; private set; }

	// Token: 0x1700093F RID: 2367
	// (get) Token: 0x06003D6B RID: 15723 RVA: 0x001258D1 File Offset: 0x00123AD1
	// (set) Token: 0x06003D6C RID: 15724 RVA: 0x001258D9 File Offset: 0x00123AD9
	public string TalentExpPointIconId { get; private set; }

	// Token: 0x06003D6D RID: 15725 RVA: 0x001258E4 File Offset: 0x00123AE4
	public InspirationWidgetData(InspirationData inspirationData, string talentExpPointIconId = null)
	{
		this.InspirationData = inspirationData;
		this.InspirationDef = GameBalance.Me.GetData<InspirationDef>(inspirationData.id + string.Format("_{0}", inspirationData.curLevel));
		this.TalentExpPointIconId = talentExpPointIconId;
		this.OnPress = new Action(this.OnInspirationPress);
	}

	// Token: 0x06003D6E RID: 15726 RVA: 0x00125947 File Offset: 0x00123B47
	public InspirationWidgetData(InspirationDef inspirationDef)
	{
		this.InspirationDef = inspirationDef;
		this.OnPress = null;
	}

	// Token: 0x06003D6F RID: 15727 RVA: 0x00125960 File Offset: 0x00123B60
	private void OnInspirationPress()
	{
		if (this.InspirationData == null)
		{
			return;
		}
		if (!MainGame.PlayerData.inventory.Data.HasItemQuantityInInventory("faith", this.Price))
		{
			return;
		}
		MainGame.PlayerData.inventory.Data.RemoveItemFromInventoryById("faith", this.Price, null, null, false);
		MainGame.Instance.GameSave.talentSystemData.PurchaseInspiration(this.InspirationData.id);
	}
}
