using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000262 RID: 610
[Serializable]
public class ConveyorCraftElement : CraftElement
{
	// Token: 0x17000272 RID: 626
	// (get) Token: 0x06000F67 RID: 3943 RVA: 0x0004EE8E File Offset: 0x0004D08E
	public override int TotalProgressTicks
	{
		get
		{
			return Mathf.CeilToInt((float)this.totalProgressTicks / (float)this.outputCount);
		}
	}

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x06000F68 RID: 3944 RVA: 0x0004EEA4 File Offset: 0x0004D0A4
	public override bool IsStarted
	{
		get
		{
			return this.isStarted || this.currentOutputIndex > 0;
		}
	}

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x06000F69 RID: 3945 RVA: 0x0004EEB9 File Offset: 0x0004D0B9
	public override bool CaBeFinished
	{
		get
		{
			return this.currentOutputIndex == this.outputCount - 1;
		}
	}

	// Token: 0x06000F6A RID: 3946 RVA: 0x0004EECB File Offset: 0x0004D0CB
	public ConveyorCraftElement(string craftId, int count, CraftParamsData craftParamsData)
		: base(craftId, count, craftParamsData)
	{
	}

	// Token: 0x06000F6B RID: 3947 RVA: 0x0004EED6 File Offset: 0x0004D0D6
	public ConveyorCraftElement(string craftId, int count, List<NeedItemData> requirements, CraftParamsData paramsData)
		: base(craftId, count, requirements, paramsData)
	{
	}

	// Token: 0x06000F6C RID: 3948 RVA: 0x0004EEE3 File Offset: 0x0004D0E3
	public ConveyorCraftElement(CraftDefBase definition)
		: base(definition)
	{
	}

	// Token: 0x06000F6D RID: 3949 RVA: 0x0004EEEC File Offset: 0x0004D0EC
	public ConveyorCraftElement(CraftDefBase definition, CraftParamsData craftParamsData)
		: base(definition, craftParamsData)
	{
	}

	// Token: 0x06000F6E RID: 3950 RVA: 0x0004EEF6 File Offset: 0x0004D0F6
	protected ConveyorCraftElement(CraftElement other, int count = 1)
		: base(other, count)
	{
	}

	// Token: 0x06000F6F RID: 3951 RVA: 0x0004EF00 File Offset: 0x0004D100
	public override CraftElementBase Clone(int count = 1)
	{
		return new ConveyorCraftElement(this, count);
	}

	// Token: 0x06000F70 RID: 3952 RVA: 0x0004EF0C File Offset: 0x0004D10C
	public override void Start(ICraftable craftable)
	{
		base.Start(craftable);
		this.outputCount = 0;
		foreach (ItemCount itemCount in this.preOutputItems)
		{
			this.outputCount += itemCount.count;
		}
	}

	// Token: 0x06000F71 RID: 3953 RVA: 0x0004EF7C File Offset: 0x0004D17C
	public override void Update(int deltaTicks)
	{
		this.currentProgressTicks += deltaTicks;
		this.succeededProgressTicks += deltaTicks;
		this.craftable.OnSuccessfulTicksChange(this.succeededProgressTicks - deltaTicks, this.succeededProgressTicks, this);
		base.NotifyProgressChanged();
	}

	// Token: 0x06000F72 RID: 3954 RVA: 0x0004EFBA File Offset: 0x0004D1BA
	public override CraftStatus CanFinishCraft(ICraftable craftableObject)
	{
		if (!craftableObject.CraftableObjectCraftInventory.CanAddItemsToInventory(this.preOutputItems))
		{
			return CraftStatus.NotEnoughSpaceInWgo;
		}
		return CraftStatus.OK;
	}

	// Token: 0x06000F73 RID: 3955 RVA: 0x0004EFD4 File Offset: 0x0004D1D4
	public override void UpdateCountOnFinish()
	{
		this.currentOutputIndex++;
		if (this.currentOutputIndex >= this.outputCount)
		{
			int count = base.Count;
			base.Count = count - 1;
			this.currentOutputIndex = 0;
		}
	}

	// Token: 0x06000F74 RID: 3956 RVA: 0x0004F014 File Offset: 0x0004D214
	public override List<Item> MakeOutput()
	{
		List<Item> list = OutputItems.MakeOutput(this.preOutputItems);
		List<Item> list2 = new List<Item>();
		int num = 0;
		foreach (Item item in list)
		{
			num += item.Count;
			if (num > this.currentOutputIndex)
			{
				list2.Add(item.Split(1, false));
				break;
			}
		}
		return list2;
	}

	// Token: 0x06000F75 RID: 3957 RVA: 0x0004F090 File Offset: 0x0004D290
	public override CraftStatus CanStartCraft(ICraftable craftableObject, MultiInventory multiInventory = null)
	{
		if (this.currentOutputIndex > 0)
		{
			return CraftStatus.OK;
		}
		if (multiInventory == null)
		{
			multiInventory = craftableObject.GetCraftableMultiInventory(false);
		}
		if (this.requirements.Count > 0 && !multiInventory.HasItemsById(this.requirements, craftableObject as WgoData))
		{
			return CraftStatus.NotEnoughResources;
		}
		if (base.Def.needItemsFromWgo.Count > 0 && !craftableObject.CraftableObjectInventory.Data.HasItemsWithIds(base.Def.needItemsFromWgo, 1, null))
		{
			return CraftStatus.NotEnoughResources;
		}
		if (base.Def.addItemsToWgoOnStart.HasOutputItems && !craftableObject.CraftableObjectInventory.CanAddItemsToInventory(this.preToWgoOnStartItems))
		{
			return CraftStatus.NotEnoughSpaceInWgo;
		}
		return CraftStatus.OK;
	}

	// Token: 0x06000F76 RID: 3958 RVA: 0x0004F134 File Offset: 0x0004D334
	public override void Finish()
	{
		base.Finish();
		this.currentProgressTicks = 0;
		this.succeededProgressTicks = 0;
		this.failedProgressTicks = 0;
	}

	// Token: 0x04001246 RID: 4678
	[SerializeField]
	private int outputCount;

	// Token: 0x04001247 RID: 4679
	[SerializeField]
	private int currentOutputIndex;
}
