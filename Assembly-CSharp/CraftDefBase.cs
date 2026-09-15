using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020001E4 RID: 484
[Serializable]
public class CraftDefBase : BalanceBaseObject
{
	// Token: 0x1700020F RID: 527
	// (get) Token: 0x06000C2F RID: 3119 RVA: 0x0003DDF0 File Offset: 0x0003BFF0
	public ItemDef FuelItemDef
	{
		get
		{
			return GameBalance.Me.GetData<ItemDef>(this.fuelItemDefId);
		}
	}

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x06000C30 RID: 3120 RVA: 0x00028294 File Offset: 0x00026494
	public virtual AutopsyTypeCraft AutopsyType
	{
		get
		{
			return AutopsyTypeCraft.None;
		}
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x0003DE02 File Offset: 0x0003C002
	public virtual OutputPreview GetOutputPreview(WgoData wgoData = null)
	{
		return this.outputItems.GetOutputPreview(this.id, wgoData);
	}

	// Token: 0x06000C32 RID: 3122 RVA: 0x0003DE18 File Offset: 0x0003C018
	public void SetIsFuelRelated()
	{
		ItemDef itemDef;
		this.isFuelCraft = this.ContainsFuelItem(this.addItemsToWgoOnFinish.chanceOutputItems, out itemDef);
		if (!this.isFuelCraft)
		{
			this.isFuelCraft = this.ContainsFuelItem(this.addItemsToWgoOnFinish.groupChanceOutputItems, out itemDef);
		}
		if (this.isFuelCraft)
		{
			this.fuelItemDefId = itemDef.id;
			foreach (string text in this.craftsIn)
			{
				WGODef data = GameBalance.Me.GetData<WGODef>(text);
				if (data != null)
				{
					data.isFuelContainer = true;
				}
				else
				{
					Debug.LogError(string.Concat(new string[] { "CraftDef [", this.id, "]. Can not find craft's in wgo [", text, "]." }));
				}
			}
		}
	}

	// Token: 0x06000C33 RID: 3123 RVA: 0x0003DF04 File Offset: 0x0003C104
	protected bool ContainsFuelItem(List<ChanceOutputItem> chanceOutputItems, out ItemDef fuelItemDef)
	{
		fuelItemDef = null;
		foreach (ChanceOutputItem chanceOutputItem in chanceOutputItems)
		{
			ItemDef data = GameBalance.Me.GetData<ItemDef>(chanceOutputItem.id);
			if (data.isFuel)
			{
				fuelItemDef = data;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000C34 RID: 3124 RVA: 0x0003DF74 File Offset: 0x0003C174
	protected bool ContainsFuelItem(List<GroupChanceOutputItem> groupChanceOutputItems, out ItemDef fuelItemDef)
	{
		fuelItemDef = null;
		for (int i = 0; i < groupChanceOutputItems.Count; i++)
		{
			if (this.ContainsFuelItem(groupChanceOutputItems[i].chanceItems, out fuelItemDef))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04000D9B RID: 3483
	[AutoParse("crafts_in")]
	public List<string> craftsIn = new List<string>();

	// Token: 0x04000D9C RID: 3484
	[AutoParse("need_extension")]
	public string extensionNeedId;

	// Token: 0x04000D9D RID: 3485
	[AutoParse("is_auto")]
	public bool isAuto;

	// Token: 0x04000D9E RID: 3486
	[AutoParse("is_hidden")]
	public bool isHidden;

	// Token: 0x04000D9F RID: 3487
	[AutoParse("need_item")]
	public List<NeedItemData> needItems = new List<NeedItemData>();

	// Token: 0x04000DA0 RID: 3488
	[AutoParse("needs_from_wgo")]
	public List<NeedItemData> needItemsFromWgo = new List<NeedItemData>();

	// Token: 0x04000DA1 RID: 3489
	public bool haveStarCraftOutput;

	// Token: 0x04000DA2 RID: 3490
	public bool isStarCraft;

	// Token: 0x04000DA3 RID: 3491
	public bool isFuelCraft;

	// Token: 0x04000DA4 RID: 3492
	public bool isAutopsyCraft;

	// Token: 0x04000DA5 RID: 3493
	public bool isPocketExtractCraft;

	// Token: 0x04000DA6 RID: 3494
	[SerializeField]
	private string fuelItemDefId;

	// Token: 0x04000DA7 RID: 3495
	[AutoParse("duration")]
	public LazyExpression duration = new LazyExpression();

	// Token: 0x04000DA8 RID: 3496
	[AutoParse("energy")]
	public LazyExpression energyPerTick = new LazyExpression();

	// Token: 0x04000DA9 RID: 3497
	[AutoParse("insanity")]
	public LazyExpression insanityPerTick = new LazyExpression();

	// Token: 0x04000DAA RID: 3498
	[AutoParse("insanity_lock")]
	public LazyExpression insanityLock = new LazyExpression();

	// Token: 0x04000DAB RID: 3499
	[AutoParse("difficulty")]
	public int talentLock = 1;

	// Token: 0x04000DAC RID: 3500
	[AutoParse("is_auto_finish")]
	public bool isAutoFinish;

	// Token: 0x04000DAD RID: 3501
	[AutoParse("linked_perks")]
	public List<string> linkedPerks = new List<string>();

	// Token: 0x04000DAE RID: 3502
	[AutoParse("out_item")]
	public OutputItems outputItems = new OutputItems();

	// Token: 0x04000DAF RID: 3503
	[AutoParse("set_out_param_on_start_to_wgo")]
	public GameRes setWgoParamsOnStart = new GameRes();

	// Token: 0x04000DB0 RID: 3504
	[AutoParse("set_out_param_on_finish_to_wgo")]
	public GameRes setWgoParamsOnFinish = new GameRes();

	// Token: 0x04000DB1 RID: 3505
	[AutoParse("add_out_param_on_start_to_wgo")]
	public GameRes addWgoParamsOnStart = new GameRes();

	// Token: 0x04000DB2 RID: 3506
	[AutoParse("add_out_param_on_finish_to_wgo")]
	public GameRes addWgoParamsOnFinish = new GameRes();

	// Token: 0x04000DB3 RID: 3507
	[AutoParse("add_items_to_wgo_on_start")]
	public OutputItems addItemsToWgoOnStart = new OutputItems();

	// Token: 0x04000DB4 RID: 3508
	[AutoParse("add_items_to_wgo_on_finish")]
	public OutputItems addItemsToWgoOnFinish = new OutputItems();

	// Token: 0x04000DB5 RID: 3509
	public bool autoFinishAutoCraft;
}
