using System;
using System.Collections.Generic;

// Token: 0x02000559 RID: 1369
public static class CraftDefExtensions
{
	// Token: 0x0600231F RID: 8991 RVA: 0x000A42A8 File Offset: 0x000A24A8
	public static bool IsOneTimeCraft(this CraftDefBase craftDef)
	{
		CraftDef craftDef2 = craftDef as CraftDef;
		bool flag;
		if (craftDef2 == null)
		{
			SurveyDef surveyDef = craftDef as SurveyDef;
			flag = surveyDef != null && surveyDef.isOneTimeCraft;
		}
		else
		{
			flag = craftDef2.isOneTimeCraft;
		}
		return flag;
	}

	// Token: 0x06002320 RID: 8992 RVA: 0x000A42E0 File Offset: 0x000A24E0
	public static bool ShouldSkipDuplicateOneTimeCraft(bool incomingIsStarted, bool existingIsStarted)
	{
		return !incomingIsStarted || existingIsStarted;
	}

	// Token: 0x06002321 RID: 8993 RVA: 0x000A42E8 File Offset: 0x000A24E8
	public static bool CanActuallyStartCraft(this CraftDef craftDef, WgoData wgoData)
	{
		CraftParamsData craftParamsData = new CraftParamsData(craftDef.id, wgoData, CraftParamsData.CraftParamsType.Common, -1);
		if (!craftDef.isStarCraft)
		{
			IWorker worker = wgoData.Worker;
			int? num = ((worker != null) ? new int?(worker.GetMasteryLevelForTalentBranch(wgoData.Definition.talent, craftDef)) : null);
			int talentLock = craftDef.talentLock;
			if ((num.GetValueOrDefault() < talentLock) & (num != null))
			{
				return false;
			}
		}
		if (craftDef.needItems == null || craftDef.needItems.Count == 0 || wgoData.Definition.conveyorType == ConveyorElementType.Workbench)
		{
			return craftDef.CanActuallyStartCraftWithNeeds(new List<NeedItemData>(), craftParamsData, wgoData);
		}
		MultiInventory craftableMultiInventory = wgoData.GetCraftableMultiInventory(false);
		List<List<NeedItemData>> list = CraftDefExtensions.ExpandNeedItemCombinations(craftDef.needItems, craftableMultiInventory, wgoData);
		if (list.Count == 0)
		{
			return false;
		}
		foreach (List<NeedItemData> list2 in list)
		{
			if (craftDef.CanActuallyStartCraftWithNeeds(list2, craftParamsData, wgoData))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002322 RID: 8994 RVA: 0x000A43FC File Offset: 0x000A25FC
	private static List<List<NeedItemData>> ExpandNeedItemCombinations(List<NeedItemData> needItems, MultiInventory multiInventory, WgoData wgoData)
	{
		List<List<NeedItemData>> list = new List<List<NeedItemData>>();
		foreach (NeedItemData needItemData in needItems)
		{
			List<NeedItemData> needItemVariants = CraftDefExtensions.GetNeedItemVariants(needItemData, multiInventory, wgoData);
			if (needItemVariants.Count == 0)
			{
				return new List<List<NeedItemData>>();
			}
			if (list.Count == 0)
			{
				using (List<NeedItemData>.Enumerator enumerator2 = needItemVariants.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						NeedItemData needItemData2 = enumerator2.Current;
						list.Add(new List<NeedItemData>
						{
							new NeedItemData(needItemData2.id, needItemData2.count)
						});
					}
					continue;
				}
			}
			List<List<NeedItemData>> list2 = new List<List<NeedItemData>>();
			foreach (List<NeedItemData> list3 in list)
			{
				foreach (NeedItemData needItemData3 in needItemVariants)
				{
					list2.Add(new List<NeedItemData>(list3)
					{
						new NeedItemData(needItemData3.id, needItemData3.count)
					});
				}
			}
			list = list2;
		}
		return list;
	}

	// Token: 0x06002323 RID: 8995 RVA: 0x000A45A8 File Offset: 0x000A27A8
	private static List<NeedItemData> GetNeedItemVariants(NeedItemData needItemData, MultiInventory multiInventory, WgoData wgoData)
	{
		if (!needItemData.IsGroup)
		{
			return new List<NeedItemData>
			{
				new NeedItemData(needItemData.id, needItemData.count)
			};
		}
		List<ItemDef> list;
		if (!needItemData.TryGetGroupItemDefs(out list) || list == null || list.Count == 0)
		{
			return new List<NeedItemData>();
		}
		List<NeedItemData> list2 = new List<NeedItemData>();
		List<NeedItemData> list3 = new List<NeedItemData>();
		foreach (ItemDef itemDef in list)
		{
			NeedItemData needItemData2 = new NeedItemData(itemDef.id, needItemData.count);
			list2.Add(needItemData2);
			if (multiInventory != null && multiInventory.HasItemQuantity(needItemData2.Id, needItemData2.GetCount(wgoData)))
			{
				list3.Add(needItemData2);
			}
		}
		if (list3.Count > 0)
		{
			return list3;
		}
		return new List<NeedItemData> { list2[0] };
	}

	// Token: 0x06002324 RID: 8996 RVA: 0x000A4694 File Offset: 0x000A2894
	private static bool CanActuallyStartCraftWithNeeds(this CraftDef craftDef, List<NeedItemData> needItemDatas, CraftParamsData paramsData, WgoData wgoData)
	{
		paramsData.RecalculateParams(needItemDatas, wgoData.Worker);
		CraftElement craftElement = new CraftElement(craftDef.id, 1, needItemDatas, paramsData);
		return wgoData.CraftComponent.GetStartCraftStatus(craftElement, null) == CraftStatus.OK;
	}

	// Token: 0x06002325 RID: 8997 RVA: 0x000A46D0 File Offset: 0x000A28D0
	public static bool CanActuallyStartInstantCraft(this CraftDef craftDef, List<NeedItemData> needItemDatas, WgoData wgoData)
	{
		CraftParamsData craftParamsData = new CraftParamsData(craftDef.id, wgoData, CraftParamsData.CraftParamsType.Common, -1);
		craftParamsData.RecalculateParams(needItemDatas, wgoData.Worker);
		CraftElement craftElement = new CraftElement(craftDef.id, 1, needItemDatas, craftParamsData);
		craftElement.DoBeforeStartCalculations(wgoData);
		return wgoData.CraftComponent.GetStartCraftStatus(craftElement, null) == CraftStatus.OK && craftElement.CanFinishCraft(wgoData) == CraftStatus.OK;
	}
}
