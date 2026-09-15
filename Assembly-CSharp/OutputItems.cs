using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000402 RID: 1026
[Serializable]
public class OutputItems : IAutoParsable
{
	// Token: 0x170004A8 RID: 1192
	// (get) Token: 0x06001AC5 RID: 6853 RVA: 0x0007C6C3 File Offset: 0x0007A8C3
	public bool HasOutputItems
	{
		get
		{
			return this.chanceOutputItems.Count > 0 || this.groupChanceOutputItems.Count > 0;
		}
	}

	// Token: 0x170004A9 RID: 1193
	// (get) Token: 0x06001AC6 RID: 6854 RVA: 0x0007C6E3 File Offset: 0x0007A8E3
	public bool HasFirstChanceItemWithMinValueExpression
	{
		get
		{
			return this.chanceOutputItems.Count > 0 && this.chanceOutputItems[0].HasMinValueExpression;
		}
	}

	// Token: 0x06001AC7 RID: 6855 RVA: 0x0007C706 File Offset: 0x0007A906
	public void Add(ChanceOutputItem chanceOutputItem, GameBalance gameBalance)
	{
		if (gameBalance == null)
		{
			Debug.LogError("Error adding chance output item. Current parsable Game balance is null.");
			return;
		}
		chanceOutputItem.isStarGroup = gameBalance.starGroupItemsCache.ContainsKey(chanceOutputItem.id);
		this.AddChanceOutputItem(chanceOutputItem);
	}

	// Token: 0x06001AC8 RID: 6856 RVA: 0x0007C73C File Offset: 0x0007A93C
	public void AddChanceOutputItem(ChanceOutputItem chanceOutputItem)
	{
		if (string.IsNullOrEmpty(chanceOutputItem.outputGroupId))
		{
			this.chanceOutputItems.Add(chanceOutputItem);
			return;
		}
		int num = this.groupChanceOutputItems.FindIndex((GroupChanceOutputItem x) => x.outputGroupId == chanceOutputItem.outputGroupId);
		if (num == -1)
		{
			GroupChanceOutputItem groupChanceOutputItem = new GroupChanceOutputItem();
			groupChanceOutputItem.outputGroupId = chanceOutputItem.outputGroupId;
			groupChanceOutputItem.chanceItems.Add(chanceOutputItem);
			this.groupChanceOutputItems.Add(groupChanceOutputItem);
			return;
		}
		this.groupChanceOutputItems[num].chanceItems.Add(chanceOutputItem);
	}

	// Token: 0x06001AC9 RID: 6857 RVA: 0x0007C7E8 File Offset: 0x0007A9E8
	public List<ItemCount> MakePreOutput(ICraftable craftable, float resultForQualityRoll = 0f)
	{
		WgoData wgoData = craftable as WgoData;
		List<ItemCount> list = new List<ItemCount>();
		foreach (ChanceOutputItem chanceOutputItem in this.chanceOutputItems)
		{
			list.AddRange(chanceOutputItem.MakePreOutput(wgoData, resultForQualityRoll));
		}
		foreach (GroupChanceOutputItem groupChanceOutputItem in this.groupChanceOutputItems)
		{
			List<ItemCount> list2 = groupChanceOutputItem.MakePreOutput(wgoData, resultForQualityRoll);
			if (list2 != null)
			{
				list.AddRange(list2);
			}
		}
		return list;
	}

	// Token: 0x06001ACA RID: 6858 RVA: 0x0007C8A0 File Offset: 0x0007AAA0
	public static List<Item> MakeOutput(List<ItemCount> preOutputItems)
	{
		List<Item> list = new List<Item>();
		foreach (ItemCount itemCount in preOutputItems)
		{
			list.Add(new Item(itemCount.itemId, itemCount.count));
		}
		return list;
	}

	// Token: 0x06001ACB RID: 6859 RVA: 0x0007C908 File Offset: 0x0007AB08
	public bool HasOutput()
	{
		return this.chanceOutputItems.Count != 0 || this.groupChanceOutputItems.Count != 0;
	}

	// Token: 0x06001ACC RID: 6860 RVA: 0x0007C928 File Offset: 0x0007AB28
	public OutputPreview GetOutputPreview(string craftId, WgoData wgoData = null)
	{
		if (this.chanceOutputItems.Count > 0)
		{
			return this.TryFormOutputPreview(craftId, this.chanceOutputItems[0], wgoData);
		}
		if (this.groupChanceOutputItems.Count > 0)
		{
			return this.TryFormOutputPreview(craftId, this.groupChanceOutputItems[0].chanceItems[0], wgoData);
		}
		return null;
	}

	// Token: 0x06001ACD RID: 6861 RVA: 0x0007C988 File Offset: 0x0007AB88
	public override string ToString()
	{
		string text = string.Empty;
		if (this.chanceOutputItems.Count > 0)
		{
			for (int i = 0; i < this.chanceOutputItems.Count; i++)
			{
				if (i > 0)
				{
					text += ";";
				}
				string text2 = (this.chanceOutputItems[i].count.HasExpression ? this.chanceOutputItems[i].count.ToString() : this.chanceOutputItems[i].minValue.ToString());
				text = text + this.chanceOutputItems[i].id + "=" + text2;
			}
		}
		else if (this.groupChanceOutputItems.Count > 0)
		{
			for (int j = 0; j < this.chanceOutputItems.Count; j++)
			{
				if (j > 0)
				{
					text += ";";
				}
				string text3 = (this.groupChanceOutputItems[j].chanceItems[0].count.HasExpression ? this.groupChanceOutputItems[j].chanceItems[0].count.ToString() : this.groupChanceOutputItems[j].chanceItems[0].minValue.ToString());
				text = text + this.groupChanceOutputItems[j].chanceItems[0].id + "=" + text3;
			}
		}
		return text;
	}

	// Token: 0x06001ACE RID: 6862 RVA: 0x0007CB10 File Offset: 0x0007AD10
	private OutputPreview TryFormOutputPreview(string craftId, ChanceOutputItem chanceOutputItem, WgoData wgoData = null)
	{
		int num = 0;
		if (chanceOutputItem.count.HasExpression)
		{
			num = ((wgoData != null) ? chanceOutputItem.count.EvaluateInt(wgoData) : chanceOutputItem.count.EvaluateInt());
		}
		else if (chanceOutputItem.minValue.HasExpression)
		{
			num = ((wgoData != null) ? chanceOutputItem.minValue.EvaluateInt(wgoData) : chanceOutputItem.minValue.EvaluateInt());
		}
		OutputPreview outputPreview;
		if (chanceOutputItem.isStarGroup)
		{
			outputPreview = new OutputPreview(craftId, chanceOutputItem.id, true, num, 0, "");
		}
		else
		{
			outputPreview = new OutputPreview(craftId, chanceOutputItem.id, false, num, -1, "");
		}
		return outputPreview;
	}

	// Token: 0x040019EC RID: 6636
	public List<ChanceOutputItem> chanceOutputItems = new List<ChanceOutputItem>();

	// Token: 0x040019ED RID: 6637
	public List<GroupChanceOutputItem> groupChanceOutputItems = new List<GroupChanceOutputItem>();
}
