using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000209 RID: 521
[Serializable]
public class SurveyDef : CraftDefBase
{
	// Token: 0x17000230 RID: 560
	// (get) Token: 0x06000CA8 RID: 3240 RVA: 0x0003FBC5 File Offset: 0x0003DDC5
	public NeedItemData SurveyedItem
	{
		get
		{
			if (this.needItems.Count <= 0)
			{
				return null;
			}
			return this.needItems[0];
		}
	}

	// Token: 0x06000CA9 RID: 3241 RVA: 0x0003FBE4 File Offset: 0x0003DDE4
	public bool IsSurveyForItem(ItemDef itemDef)
	{
		if (itemDef == null || this.SurveyedItem == null)
		{
			return false;
		}
		switch (this.SurveyedItem.groupType)
		{
		case ItemGroup.None:
			return this.SurveyedItem.id == itemDef.id;
		case ItemGroup.Common:
			return itemDef.itemGroupIds.Contains(this.SurveyedItem.id);
		case ItemGroup.Star:
			return itemDef.qualityType == ItemDef.QualityType.Star && itemDef.id.Split(':', StringSplitOptions.None)[0] == this.SurveyedItem.id;
		default:
			return false;
		}
	}

	// Token: 0x06000CAA RID: 3242 RVA: 0x0003FC7C File Offset: 0x0003DE7C
	public List<ItemDef> GetSurveyedItemDefs()
	{
		List<ItemDef> list = new List<ItemDef>();
		if (this.SurveyedItem == null)
		{
			return list;
		}
		switch (this.SurveyedItem.groupType)
		{
		case ItemGroup.None:
		{
			ItemDef dataOrNull = GameBalance.Me.GetDataOrNull<ItemDef>(this.SurveyedItem.id);
			if (dataOrNull != null)
			{
				list.Add(dataOrNull);
			}
			break;
		}
		case ItemGroup.Common:
		{
			List<ItemDef> list2;
			if (GameBalance.Me.groupItemsCache.TryGetValue(this.SurveyedItem.id, out list2))
			{
				list.AddRange(list2);
			}
			break;
		}
		case ItemGroup.Star:
		{
			List<ItemDef> list3;
			if (GameBalance.Me.starGroupItemsCache.TryGetValue(this.SurveyedItem.id, out list3))
			{
				list.AddRange(list3);
			}
			break;
		}
		}
		return list;
	}

	// Token: 0x06000CAB RID: 3243 RVA: 0x0003FD2C File Offset: 0x0003DF2C
	public override OutputPreview GetOutputPreview(WgoData wgoData = null)
	{
		if (this.outputPreview == null)
		{
			string text = this.id.Replace("surv:", string.Empty);
			ItemDef itemDef = GameBalance.Me.GetDataOrNull<ItemDef>(text);
			if (itemDef == null)
			{
				List<ItemDef> surveyedItemDefs = this.GetSurveyedItemDefs();
				itemDef = ((surveyedItemDefs.Count > 0) ? surveyedItemDefs[0] : null);
			}
			string text2 = ((itemDef == null) ? ("i_" + text) : itemDef.iconId);
			this.outputPreview = new OutputPreview(this.id, "", false, 1, -1, text2);
		}
		return this.outputPreview;
	}

	// Token: 0x04000EE8 RID: 3816
	[AutoParse("surveyed_at_start")]
	public bool surveyedAtStart;

	// Token: 0x04000EE9 RID: 3817
	[AutoParse("on_craft_end_expressions")]
	public List<LazyExpression> onCraftEndExpressions = new List<LazyExpression>();

	// Token: 0x04000EEA RID: 3818
	[AutoParse("tech_r")]
	public int techRed;

	// Token: 0x04000EEB RID: 3819
	[AutoParse("tech_g")]
	public int techGreen;

	// Token: 0x04000EEC RID: 3820
	[AutoParse("tech_b")]
	public int techBlue;

	// Token: 0x04000EED RID: 3821
	public bool isOneTimeCraft;

	// Token: 0x04000EEE RID: 3822
	public bool isScienceFuelCraft;

	// Token: 0x04000EEF RID: 3823
	private OutputPreview outputPreview;
}
