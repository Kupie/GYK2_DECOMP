using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003FF RID: 1023
[Serializable]
public class ChanceOutputItem
{
	// Token: 0x170004A7 RID: 1191
	// (get) Token: 0x06001AB5 RID: 6837 RVA: 0x0007C29F File Offset: 0x0007A49F
	public bool HasMinValueExpression
	{
		get
		{
			return this.minValue.HasExpression;
		}
	}

	// Token: 0x06001AB6 RID: 6838 RVA: 0x0007C2AC File Offset: 0x0007A4AC
	public List<ItemCount> MakePreOutput(WgoData wgoData, float resultForQualityRoll = 0f)
	{
		if (!this.chance.HasExpression && !this.HasMinValueExpression)
		{
			return this.MakePreOutputSingle(wgoData, resultForQualityRoll);
		}
		if (!this.chance.HasExpression && this.HasMinValueExpression)
		{
			return this.MakePreOutputRandomAmount(wgoData, resultForQualityRoll);
		}
		return this.MakePreOutputChance(wgoData, resultForQualityRoll);
	}

	// Token: 0x06001AB7 RID: 6839 RVA: 0x0007C2FD File Offset: 0x0007A4FD
	public List<ItemCount> MakePreOutputAsGroupItem(WgoData wgoData, float resultForQualityRoll = 0f)
	{
		if (!this.HasMinValueExpression)
		{
			return this.MakePreOutputSingle(wgoData, resultForQualityRoll);
		}
		return this.MakePreOutputRandomAmount(wgoData, resultForQualityRoll);
	}

	// Token: 0x06001AB8 RID: 6840 RVA: 0x0007C318 File Offset: 0x0007A518
	private List<ItemCount> MakePreOutputSingle(WgoData wgoData, float resultForQualityRoll = 0f)
	{
		return this.MakePreOutputItemsListFromCount(this.count.EvaluateInt(wgoData), resultForQualityRoll, ChanceOutputItem.IsGardenGrowingCraft(wgoData));
	}

	// Token: 0x06001AB9 RID: 6841 RVA: 0x0007C333 File Offset: 0x0007A533
	private List<ItemCount> MakePreOutputRandomAmount(WgoData wgoData, float resultForQualityRoll = 0f)
	{
		return this.MakePreOutputItemsListFromCount(global::UnityEngine.Random.Range(this.minValue.EvaluateInt(wgoData), this.maxValue.EvaluateInt(wgoData) + 1), resultForQualityRoll, ChanceOutputItem.IsGardenGrowingCraft(wgoData));
	}

	// Token: 0x06001ABA RID: 6842 RVA: 0x0007C361 File Offset: 0x0007A561
	private List<ItemCount> MakePreOutputChance(WgoData wgoData, float resultForQualityRoll = 0f)
	{
		if (global::UnityEngine.Random.Range(0f, 1f) > this.chance.EvaluateFloat(wgoData))
		{
			return new List<ItemCount>();
		}
		if (!this.HasMinValueExpression)
		{
			return this.MakePreOutputSingle(wgoData, resultForQualityRoll);
		}
		return this.MakePreOutputRandomAmount(wgoData, resultForQualityRoll);
	}

	// Token: 0x06001ABB RID: 6843 RVA: 0x0007C3A0 File Offset: 0x0007A5A0
	private static bool IsGardenGrowingCraft(WgoData wgoData)
	{
		if (wgoData == null)
		{
			return false;
		}
		CraftComponent craftComponent = wgoData.CraftComponent;
		CraftParamsData.CraftParamsType? craftParamsType;
		if (craftComponent == null)
		{
			craftParamsType = null;
		}
		else
		{
			CraftElementBase currentCraftElement = craftComponent.CurrentCraftElement;
			craftParamsType = ((currentCraftElement != null) ? new CraftParamsData.CraftParamsType?(currentCraftElement.ParamsData.craftParamsType) : null);
		}
		CraftParamsData.CraftParamsType? craftParamsType2 = craftParamsType;
		CraftParamsData.CraftParamsType craftParamsType3 = CraftParamsData.CraftParamsType.GardenGrowing;
		return (craftParamsType2.GetValueOrDefault() == craftParamsType3) & (craftParamsType2 != null);
	}

	// Token: 0x06001ABC RID: 6844 RVA: 0x0007C400 File Offset: 0x0007A600
	private List<ItemCount> MakePreOutputItemsListFromCount(int amount, float resultForQualityRoll = 0f, bool forceCommonOutputRule = false)
	{
		List<ItemCount> list = new List<ItemCount>();
		if (this.isStarGroup && !forceCommonOutputRule)
		{
			if (resultForQualityRoll == 0f)
			{
				return list;
			}
			int num = (int)resultForQualityRoll;
			Debug.Log(string.Format("Made Common craft output as star item [{0}] with quality [{1}]", this.id + ":" + num.ToString(), num));
			for (int i = 0; i < amount; i++)
			{
				this.AddOutputItemToList(list, num);
			}
		}
		else
		{
			Debug.Log("Made craft output as common item [" + this.id + "]");
			this.FillPreOutputFromAmount(ref list, this.id, amount, GameBalance.Me.GetData<ItemDef>(this.id).stackCount);
		}
		return list;
	}

	// Token: 0x06001ABD RID: 6845 RVA: 0x0007C4AC File Offset: 0x0007A6AC
	private void AddOutputItemToList(List<ItemCount> outputItems, int resultQuality)
	{
		ItemDef itemDef = GameBalance.Me.GetData<ItemDef>(this.id + ":" + resultQuality.ToString());
		int num = outputItems.FindIndex((ItemCount x) => x.itemId == itemDef.id && x.count < itemDef.stackCount);
		if (num != -1)
		{
			outputItems[num].count++;
			return;
		}
		outputItems.Add(new ItemCount(itemDef.id, 1));
	}

	// Token: 0x06001ABE RID: 6846 RVA: 0x0007C529 File Offset: 0x0007A729
	private void FillPreOutputFromAmount(ref List<ItemCount> outputItems, string itemId, int amount, int stackCount)
	{
		while (amount > 0)
		{
			if (amount >= stackCount)
			{
				outputItems.Add(new ItemCount(itemId, stackCount));
				amount -= stackCount;
			}
			else
			{
				outputItems.Add(new ItemCount(itemId, amount));
				amount = -1;
			}
		}
	}

	// Token: 0x040019E2 RID: 6626
	public string id;

	// Token: 0x040019E3 RID: 6627
	public string outputGroupId;

	// Token: 0x040019E4 RID: 6628
	public LazyExpression count = new LazyExpression();

	// Token: 0x040019E5 RID: 6629
	public LazyExpression minValue = new LazyExpression();

	// Token: 0x040019E6 RID: 6630
	public LazyExpression maxValue = new LazyExpression();

	// Token: 0x040019E7 RID: 6631
	public LazyExpression chance = new LazyExpression();

	// Token: 0x040019E8 RID: 6632
	public bool isStarGroup;
}
