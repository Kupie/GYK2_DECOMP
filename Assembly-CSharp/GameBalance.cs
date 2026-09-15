using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000229 RID: 553
[Serializable]
public class GameBalance : GameBalanceBase
{
	// Token: 0x17000248 RID: 584
	// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x000410CD File Offset: 0x0003F2CD
	public static GameBalance Me
	{
		get
		{
			if (GameBalance.instance)
			{
				return GameBalance.instance;
			}
			GameBalance.LoadGameBalance();
			return GameBalance.instance;
		}
	}

	// Token: 0x06000CF7 RID: 3319 RVA: 0x000410EB File Offset: 0x0003F2EB
	public static void LoadGameBalance()
	{
		GameBalance.instance = Resources.Load<GameBalance>("GameBalance");
		if (GameBalance.instance == null)
		{
			Debug.LogError("Game data load failed");
			return;
		}
		GameBalanceBase.Instance = GameBalance.instance;
		GameBalance.instance.InitCache();
	}

	// Token: 0x06000CF8 RID: 3320 RVA: 0x00041128 File Offset: 0x0003F328
	public override void InitCache()
	{
		base.InitCache();
		this.InitAlchemyCache();
		this.InitTalentExpLevels();
		this.InitInspirationLevels();
		this.FillItemsSortingOrder();
		this.CreateCraftCache();
		this.CreateAutopsyCraftsCache();
		this.CreateCraftGroupsCache();
		this.CreateCraftInItemsCache();
		this.CreateBuildCache();
		this.CreateNewVendorProductsCache();
		this.CreateQuestsCache();
		this.SetTalentIdsForInstruments();
		this.CreateGardenCraftsPerItemCache();
		this.CreateConveyorCache();
		this.CreateWorkbenchExtensionsCache();
		this.CreateFightersCache();
		this.CreateCustomBuildAreaIdToWgoIdsCache();
		this.CreateGardenGrowingCraftsCache();
		this.CreateQuestDefByFinishPhraseCache();
		this.CreateWgoIdsByGroupCache();
	}

	// Token: 0x06000CF9 RID: 3321 RVA: 0x000411B4 File Offset: 0x0003F3B4
	private void CreateCraftCache()
	{
		this.craftsInCache.Clear();
		foreach (CraftDef craftDef in this.craftDefs)
		{
			craftDef.SetIsFuelRelated();
			foreach (string text in craftDef.craftsIn)
			{
				if (!this.craftsInCache.ContainsKey(text))
				{
					this.craftsInCache.Add(text, new List<CraftDefBase>());
				}
				this.craftsInCache[text].Add(craftDef);
			}
		}
	}

	// Token: 0x06000CFA RID: 3322 RVA: 0x00041280 File Offset: 0x0003F480
	private void CreateAutopsyCraftsCache()
	{
		this.autopsyCraftsByTypeAndItemCache.Clear();
		foreach (CraftDef craftDef in this.craftDefs)
		{
			if (craftDef.autopsyTypeCraft != AutopsyTypeCraft.None)
			{
				Dictionary<string, CraftDef> dictionary;
				if (!this.autopsyCraftsByTypeAndItemCache.TryGetValue(craftDef.autopsyTypeCraft, out dictionary))
				{
					dictionary = new Dictionary<string, CraftDef>();
					this.autopsyCraftsByTypeAndItemCache.Add(craftDef.autopsyTypeCraft, dictionary);
				}
				if (!dictionary.TryAdd(craftDef.autopsyItemId, craftDef))
				{
					Debug.LogError(string.Format("Craft [{0}] duplicates autopsy craft [{1}] of type [{2}] for item [{3}].", new object[]
					{
						craftDef.id,
						dictionary[craftDef.autopsyItemId].id,
						craftDef.autopsyTypeCraft,
						craftDef.autopsyItemId
					}));
				}
			}
		}
	}

	// Token: 0x06000CFB RID: 3323 RVA: 0x00041370 File Offset: 0x0003F570
	private void CreateCraftInItemsCache()
	{
		foreach (CraftDef craftDef in this.craftDefs)
		{
			if (!craftDef.isHidden && !craftDef.id.StartsWith("rem_") && !craftDef.id.StartsWith("set_") && !craftDef.id.StartsWith("town_building_craft:"))
			{
				GameBalance.<>c__DisplayClass77_0 CS$<>8__locals1;
				CS$<>8__locals1.allPossibleItemsAsOutput = new List<string>();
				this.<CreateCraftInItemsCache>g__ScanOutputItems|77_0(craftDef.outputItems, ref CS$<>8__locals1);
				this.<CreateCraftInItemsCache>g__ScanOutputItems|77_0(craftDef.addItemsToWgoOnStart, ref CS$<>8__locals1);
				this.<CreateCraftInItemsCache>g__ScanOutputItems|77_0(craftDef.addItemsToWgoOnFinish, ref CS$<>8__locals1);
				this.<CreateCraftInItemsCache>g__ScanNeedItems|77_1(craftDef.dropFromWgoItemsEnd, ref CS$<>8__locals1);
				this.<CreateCraftInItemsCache>g__ScanNeedItems|77_1(craftDef.dropFromWgoItemsStart, ref CS$<>8__locals1);
				for (int i = 0; i < CS$<>8__locals1.allPossibleItemsAsOutput.Count; i++)
				{
					if (!this.craftInItemsCache.ContainsKey(CS$<>8__locals1.allPossibleItemsAsOutput[i]))
					{
						this.craftInItemsCache.Add(CS$<>8__locals1.allPossibleItemsAsOutput[i], new List<string>());
						this.craftInItemsCacheShownInTooltips.Add(CS$<>8__locals1.allPossibleItemsAsOutput[i], new List<string>());
					}
					for (int j = 0; j < craftDef.craftsIn.Count; j++)
					{
						if (!this.craftInItemsCache[CS$<>8__locals1.allPossibleItemsAsOutput[i]].Contains(craftDef.craftsIn[j]))
						{
							this.craftInItemsCache[CS$<>8__locals1.allPossibleItemsAsOutput[i]].Add(craftDef.craftsIn[j]);
							if (!craftDef.doNotShowInTooltips)
							{
								this.craftInItemsCacheShownInTooltips[CS$<>8__locals1.allPossibleItemsAsOutput[i]].Add(craftDef.craftsIn[j]);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06000CFC RID: 3324 RVA: 0x0004157C File Offset: 0x0003F77C
	private void AddCraftDefToCache(CraftDef craftDef)
	{
		GameBalance.<>c__DisplayClass78_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		craftDef.SetIsFuelRelated();
		foreach (string text in craftDef.craftsIn)
		{
			if (!this.craftsInCache.ContainsKey(text))
			{
				this.craftsInCache.Add(text, new List<CraftDefBase>());
			}
			this.craftsInCache[text].Add(craftDef);
		}
		if (craftDef.isHidden || craftDef.id.StartsWith("rem_") || craftDef.id.StartsWith("set_"))
		{
			return;
		}
		CS$<>8__locals1.allPossibleItemsAsOutput = new List<string>();
		this.<AddCraftDefToCache>g__ScanOutputItems|78_1(craftDef.outputItems, ref CS$<>8__locals1);
		this.<AddCraftDefToCache>g__ScanOutputItems|78_1(craftDef.addItemsToWgoOnStart, ref CS$<>8__locals1);
		this.<AddCraftDefToCache>g__ScanOutputItems|78_1(craftDef.addItemsToWgoOnFinish, ref CS$<>8__locals1);
		this.<AddCraftDefToCache>g__ScanNeedItems|78_2(craftDef.dropFromWgoItemsEnd, ref CS$<>8__locals1);
		this.<AddCraftDefToCache>g__ScanNeedItems|78_2(craftDef.dropFromWgoItemsStart, ref CS$<>8__locals1);
		for (int i = 0; i < CS$<>8__locals1.allPossibleItemsAsOutput.Count; i++)
		{
			if (!this.craftInItemsCache.ContainsKey(CS$<>8__locals1.allPossibleItemsAsOutput[i]))
			{
				this.craftInItemsCache.Add(CS$<>8__locals1.allPossibleItemsAsOutput[i], new List<string>());
				this.craftInItemsCacheShownInTooltips.Add(CS$<>8__locals1.allPossibleItemsAsOutput[i], new List<string>());
			}
			for (int j = 0; j < craftDef.craftsIn.Count; j++)
			{
				if (!this.craftInItemsCache[CS$<>8__locals1.allPossibleItemsAsOutput[i]].Contains(craftDef.craftsIn[j]))
				{
					this.craftInItemsCache[CS$<>8__locals1.allPossibleItemsAsOutput[i]].Add(craftDef.craftsIn[j]);
					if (!craftDef.doNotShowInTooltips)
					{
						this.craftInItemsCacheShownInTooltips[CS$<>8__locals1.allPossibleItemsAsOutput[i]].Add(craftDef.craftsIn[j]);
					}
				}
			}
		}
	}

	// Token: 0x06000CFD RID: 3325 RVA: 0x00041798 File Offset: 0x0003F998
	public static T GetCraftDef<T>(string craftId) where T : CraftDefBase
	{
		if (typeof(T) == typeof(SermonDef))
		{
			return GameBalance.GetSermonDef(craftId) as T;
		}
		if (typeof(T) == typeof(SurveyDef))
		{
			return GameBalance.GetSurveyDef(craftId) as T;
		}
		if (typeof(T) == typeof(AlchemyMixDef))
		{
			return GameBalance.GetAlchemyMixDef(craftId) as T;
		}
		if (typeof(T) == typeof(CraftDef))
		{
			return GameBalance.GetCraftDef(craftId) as T;
		}
		return GameBalance.GetCraftDefBase(craftId) as T;
	}

	// Token: 0x06000CFE RID: 3326 RVA: 0x00041868 File Offset: 0x0003FA68
	public static CraftDefBase GetCraftDefBase(string craftId)
	{
		CraftDefBase craftDefBase = GameBalance.Me.GetDataOrNull<CraftDef>(craftId);
		if (craftDefBase != null)
		{
			return craftDefBase;
		}
		craftDefBase = GameBalance.Me.GetDataOrNull<SermonDef>(craftId);
		if (craftDefBase != null)
		{
			return craftDefBase;
		}
		craftDefBase = GameBalance.Me.GetDataOrNull<SurveyDef>(craftId);
		if (craftDefBase != null)
		{
			return craftDefBase;
		}
		craftDefBase = GameBalance.GetAlchemyMixDef(craftId);
		if (craftDefBase != null)
		{
			return craftDefBase;
		}
		return GameBalance.GetAlchemyMixCraftDef(craftId);
	}

	// Token: 0x06000CFF RID: 3327 RVA: 0x000418BC File Offset: 0x0003FABC
	public static CraftDef GetCraftDef(string craftId)
	{
		if (craftId.StartsWith("mix"))
		{
			return GameBalance.GetAlchemyMixCraftDef(craftId);
		}
		return GameBalance.Me.GetData<CraftDef>(craftId);
	}

	// Token: 0x06000D00 RID: 3328 RVA: 0x000418E0 File Offset: 0x0003FAE0
	public static CraftDef GetAutopsyCraftDef(AutopsyTypeCraft autopsyType, string itemId = "")
	{
		Dictionary<string, CraftDef> dictionary;
		if (!GameBalance.Me.autopsyCraftsByTypeAndItemCache.TryGetValue(autopsyType, out dictionary))
		{
			return null;
		}
		if (autopsyType == AutopsyTypeCraft.PocketExtract)
		{
			using (Dictionary<string, CraftDef>.ValueCollection.Enumerator enumerator = dictionary.Values.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return null;
		}
		CraftDef craftDef;
		dictionary.TryGetValue(itemId, out craftDef);
		return craftDef;
	}

	// Token: 0x06000D01 RID: 3329 RVA: 0x00041958 File Offset: 0x0003FB58
	public static SurveyDef GetSurveyDef(string craftId)
	{
		return GameBalance.Me.GetData<SurveyDef>(craftId);
	}

	// Token: 0x06000D02 RID: 3330 RVA: 0x00041965 File Offset: 0x0003FB65
	public static SurveyDef GetSurveyDefOrNull(string craftId)
	{
		return GameBalance.Me.GetDataOrNull<SurveyDef>(craftId);
	}

	// Token: 0x06000D03 RID: 3331 RVA: 0x00041974 File Offset: 0x0003FB74
	public static SurveyDef GetSurveyDefForItemOrNull(string itemId)
	{
		SurveyDef surveyDefOrNull = GameBalance.GetSurveyDefOrNull("surv:" + itemId);
		if (surveyDefOrNull != null)
		{
			return surveyDefOrNull;
		}
		ItemDef dataOrNull = GameBalance.Me.GetDataOrNull<ItemDef>(itemId);
		if (dataOrNull == null)
		{
			return null;
		}
		foreach (SurveyDef surveyDef in GameBalance.Me.surveyDefs)
		{
			if (surveyDef.IsSurveyForItem(dataOrNull))
			{
				return surveyDef;
			}
		}
		return null;
	}

	// Token: 0x06000D04 RID: 3332 RVA: 0x00041A00 File Offset: 0x0003FC00
	public static SermonDef GetSermonDef(string craftId)
	{
		return GameBalance.Me.GetData<SermonDef>(craftId);
	}

	// Token: 0x06000D05 RID: 3333 RVA: 0x00041A10 File Offset: 0x0003FC10
	public static AlchemyMixDef GetAlchemyMixDef(string mixId)
	{
		AlchemyMixDef alchemyMixDef;
		if (GameBalance.Me.alchemyMixDefsCache.TryGetValue(mixId, out alchemyMixDef) && alchemyMixDef != null)
		{
			Debug.Log("#alch# GetAlchemyMixDef:[" + mixId + "] has cached");
			return alchemyMixDef;
		}
		AlchemyMixSourceDef alchemyMixSourceDef;
		if (!GameBalance.Me.alchemyMixSourcesByIdCache.TryGetValue(mixId, out alchemyMixSourceDef))
		{
			Debug.Log("#alch# GetAlchemyMixDef:[" + mixId + "] return null");
			return null;
		}
		AlchemyMixDef alchemyMixDef2 = GameBalance.Me.BuildAlchemyMixDef(alchemyMixSourceDef);
		Debug.Log(string.Format("#alch# GetAlchemyMixDef:[{0}] build new is null:[{1}]", mixId, alchemyMixDef2 == null));
		GameBalance.Me.alchemyMixDefsCache[mixId] = alchemyMixDef2;
		return alchemyMixDef2;
	}

	// Token: 0x06000D06 RID: 3334 RVA: 0x00041AB0 File Offset: 0x0003FCB0
	private void FillItemsSortingOrder()
	{
		for (int i = 0; i < this.itemDefs.Count; i++)
		{
			this.itemDefs[i].sortOrder = i;
		}
	}

	// Token: 0x06000D07 RID: 3335 RVA: 0x00041AE8 File Offset: 0x0003FCE8
	private void CreateCraftGroupsCache()
	{
		this.starGroupItemsCache.Clear();
		this.groupItemsCache.Clear();
		foreach (ItemDef itemDef in this.itemDefs)
		{
			for (int i = 0; i < itemDef.itemGroupIds.Count; i++)
			{
				if (!this.groupItemsCache.ContainsKey(itemDef.itemGroupIds[i]))
				{
					this.groupItemsCache.Add(itemDef.itemGroupIds[i], new List<ItemDef>());
				}
				this.groupItemsCache[itemDef.itemGroupIds[i]].Add(itemDef);
			}
			if (itemDef.qualityType == ItemDef.QualityType.Star)
			{
				string text = itemDef.id.Split(":", StringSplitOptions.None)[0];
				if (!this.starGroupItemsCache.ContainsKey(text))
				{
					this.starGroupItemsCache.Add(text, new List<ItemDef>());
				}
				this.starGroupItemsCache[text].Add(itemDef);
			}
		}
	}

	// Token: 0x06000D08 RID: 3336 RVA: 0x00041C08 File Offset: 0x0003FE08
	private void CreateBuildCache()
	{
		this.buildDefsInBuilder.Clear();
		this.buildableWgos.Clear();
		this.removableWgos.Clear();
		foreach (BuildingDef buildingDef in this.buildingDefs)
		{
			foreach (string text in buildingDef.buildsIn)
			{
				if (!this.buildDefsInBuilder.ContainsKey(text))
				{
					this.buildDefsInBuilder.Add(text, new List<BuildingDef>());
				}
				this.buildDefsInBuilder[text].Add(buildingDef);
			}
			switch (buildingDef.buildingMode)
			{
			case BuildingDef.BuildingMode.Place:
			case BuildingDef.BuildingMode.ConveyorPlace:
			case BuildingDef.BuildingMode.FightingPlace:
			case BuildingDef.BuildingMode.FightBuilding:
				this.buildableWgos.Add(buildingDef.wgoId, buildingDef);
				break;
			case BuildingDef.BuildingMode.Remove:
				this.removableWgos.Add(buildingDef.wgoId, buildingDef);
				break;
			}
		}
	}

	// Token: 0x06000D09 RID: 3337 RVA: 0x00041D34 File Offset: 0x0003FF34
	private void CreateNewVendorProductsCache()
	{
		foreach (VendorDef vendorDef in this.vendorDefs)
		{
			if (vendorDef.startTier != 3)
			{
				foreach (VendorTierData vendorTierData in vendorDef.tierDataList)
				{
					vendorTierData.newProducts.Clear();
					foreach (VendorProductData vendorProductData in vendorTierData.vendorProducts)
					{
						bool flag = false;
						foreach (VendorTierData vendorTierData2 in vendorDef.tierDataList)
						{
							if (vendorTierData2 == vendorTierData)
							{
								break;
							}
							using (List<VendorProductData>.Enumerator enumerator5 = vendorTierData2.vendorProducts.GetEnumerator())
							{
								while (enumerator5.MoveNext())
								{
									if (enumerator5.Current.itemId == vendorProductData.itemId)
									{
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
						if (!flag)
						{
							vendorTierData.newProducts.Add(vendorProductData.itemId);
						}
					}
				}
			}
		}
	}

	// Token: 0x06000D0A RID: 3338 RVA: 0x00041F14 File Offset: 0x00040114
	private void CreateQuestsCache()
	{
		this.questDefByReqPhrase.Clear();
		this.questsByQuestIdCache.Clear();
		this.questBrothersCache.Clear();
		foreach (QuestDef questDef in this.questDefs)
		{
			if (!string.IsNullOrEmpty(questDef.finishCheck.phrase))
			{
				this.questDefByReqPhrase.TryAdd(questDef.finishCheck.phrase, questDef);
			}
			List<QuestDef> list;
			if (this.questsByQuestIdCache.TryGetValue(questDef.id, out list))
			{
				list.Add(questDef);
			}
			else
			{
				this.questsByQuestIdCache.Add(questDef.id, new List<QuestDef> { questDef });
			}
		}
		this.CreateQuestBrothersCache();
	}

	// Token: 0x06000D0B RID: 3339 RVA: 0x00041FEC File Offset: 0x000401EC
	private void CreateQuestBrothersCache()
	{
		Dictionary<Vector2Int, List<string>> dictionary = new Dictionary<Vector2Int, List<string>>();
		foreach (QuestDef questDef in this.questDefs)
		{
			List<string> list;
			if (!dictionary.TryGetValue(questDef.TreePos, out list))
			{
				list = new List<string>();
				dictionary.Add(questDef.TreePos, list);
			}
			list.Add(questDef.id);
		}
		foreach (QuestDef questDef2 in this.questDefs)
		{
			HashSet<string> hashSet = new HashSet<string>(questDef2.brotherIds);
			foreach (string text in dictionary[questDef2.TreePos])
			{
				if (text != questDef2.id)
				{
					hashSet.Add(text);
				}
			}
			this.questBrothersCache.Add(questDef2.id, new List<string>(hashSet));
		}
	}

	// Token: 0x06000D0C RID: 3340 RVA: 0x00042134 File Offset: 0x00040334
	private void CreateGardenCraftsPerItemCache()
	{
		foreach (ItemDef itemDef in this.itemDefs)
		{
			if (itemDef.isSeed || itemDef.isFertilizer)
			{
				if (!this.gardenCraftsPerItemCache.ContainsKey(itemDef))
				{
					this.gardenCraftsPerItemCache.Add(itemDef, new List<CraftDef>());
				}
				foreach (CraftDefBase craftDefBase in this.craftDefs)
				{
					foreach (NeedItemData needItemData in craftDefBase.needItems)
					{
						switch (needItemData.groupType)
						{
						case ItemGroup.None:
							if (needItemData.id == itemDef.id)
							{
								this.gardenCraftsPerItemCache[itemDef].Add(craftDefBase as CraftDef);
							}
							break;
						case ItemGroup.Common:
							if (this.groupItemsCache[needItemData.id].Contains(itemDef))
							{
								this.gardenCraftsPerItemCache[itemDef].Add(craftDefBase as CraftDef);
							}
							break;
						case ItemGroup.Star:
							if (this.starGroupItemsCache[needItemData.id].Contains(itemDef))
							{
								this.gardenCraftsPerItemCache[itemDef].Add(craftDefBase as CraftDef);
							}
							break;
						}
					}
				}
			}
		}
	}

	// Token: 0x06000D0D RID: 3341 RVA: 0x00042314 File Offset: 0x00040514
	private void InitTalentExpLevels()
	{
		this.talentExpLevelsCache.Clear();
		this.yellow = new TalentExpLevelBalanceData("talent_yellow");
		this.green = new TalentExpLevelBalanceData("talent_green");
		this.red = new TalentExpLevelBalanceData("talent_red");
		this.orange = new TalentExpLevelBalanceData("talent_orange");
		this.blue = new TalentExpLevelBalanceData("talent_blue");
		this.talentExpLevelsCache.Add("talent_yellow", this.yellow);
		this.talentExpLevelsCache.Add("talent_green", this.green);
		this.talentExpLevelsCache.Add("talent_red", this.red);
		this.talentExpLevelsCache.Add("talent_orange", this.orange);
		this.talentExpLevelsCache.Add("talent_blue", this.blue);
		foreach (TalentExpLevelDef talentExpLevelDef in this.talentExpLevelDefs)
		{
			this.yellow.expLevels.Add(talentExpLevelDef.yellow);
			this.green.expLevels.Add(talentExpLevelDef.green);
			this.red.expLevels.Add(talentExpLevelDef.red);
			this.orange.expLevels.Add(talentExpLevelDef.orange);
			this.blue.expLevels.Add(talentExpLevelDef.blue);
		}
	}

	// Token: 0x06000D0E RID: 3342 RVA: 0x00042498 File Offset: 0x00040698
	private void InitAlchemyCache()
	{
		this.runtimeCraftDefsCacheAlchemy = new Dictionary<string, CraftDef>();
		this.alchemyMixDefsCache = new Dictionary<string, AlchemyMixDef>();
		this.alchemyMixSourcesByIdCache = new Dictionary<string, AlchemyMixSourceDef>();
		foreach (AlchemyMixSourceDef alchemyMixSourceDef in this.alchemyMixSourceDefs)
		{
			this.alchemyMixSourcesByIdCache[alchemyMixSourceDef.mixId] = alchemyMixSourceDef;
		}
	}

	// Token: 0x06000D0F RID: 3343 RVA: 0x00042518 File Offset: 0x00040718
	private void InitInspirationLevels()
	{
		this.inspirationLevels = new List<InspirationLevelData>();
		this.inspirationLevelsCache = new Dictionary<string, InspirationLevelData>();
		foreach (InspirationDef inspirationDef in this.inspirationDefs)
		{
			string idWithoutLvl = inspirationDef.idWithoutLvl;
			InspirationLevelData inspirationLevelData;
			if (this.inspirationLevelsCache.TryGetValue(idWithoutLvl, out inspirationLevelData))
			{
				inspirationLevelData.levels.Add(inspirationDef);
				inspirationLevelData.inspirationLocks.AddRange(inspirationDef.inpsirationLocks);
				inspirationLevelData.techLocks.AddRange(inspirationDef.techLocks);
				inspirationLevelData.questLocks.AddRange(inspirationDef.questLocks);
			}
			else
			{
				InspirationLevelData inspirationLevelData2 = new InspirationLevelData();
				inspirationLevelData2.id = idWithoutLvl;
				inspirationLevelData2.talentId = inspirationDef.talentId;
				inspirationLevelData2.levels.Add(inspirationDef);
				inspirationLevelData2.inspirationLocks.AddRange(inspirationDef.inpsirationLocks);
				inspirationLevelData2.techLocks.AddRange(inspirationDef.techLocks);
				inspirationLevelData2.questLocks.AddRange(inspirationDef.questLocks);
				this.inspirationLevels.Add(inspirationLevelData2);
				this.inspirationLevelsCache.Add(idWithoutLvl, inspirationLevelData2);
			}
		}
	}

	// Token: 0x06000D10 RID: 3344 RVA: 0x00042654 File Offset: 0x00040854
	private void SetTalentIdsForInstruments()
	{
		foreach (ItemDef itemDef in this.itemDefs)
		{
			GameBalanceBase me = GameBalance.Me;
			int type = (int)itemDef.type;
			ToolTypeDef dataOrNull = me.GetDataOrNull<ToolTypeDef>(type.ToString());
			if (dataOrNull != null)
			{
				itemDef.talentIds = dataOrNull.talentIds;
			}
		}
	}

	// Token: 0x06000D11 RID: 3345 RVA: 0x000426C8 File Offset: 0x000408C8
	public void CreateTownBuildingCrafts()
	{
		foreach (TownBuildingDef townBuildingDef in this.townBuildingDefs)
		{
			CraftDef craftDef = new CraftDef();
			craftDef.id = "town_building_craft:" + townBuildingDef.id;
			craftDef.craftsIn = townBuildingDef.craftsIn;
			craftDef.needItems = townBuildingDef.needItems;
			craftDef.duration = new LazyExpression();
			craftDef.isAuto = true;
			craftDef.autoFinishAutoCraft = true;
			craftDef.isNeedsUnlock = townBuildingDef.isNeedsUnlock;
			craftDef.onCraftEndExpressions = townBuildingDef.onCraftEndExpressions;
			craftDef.outputItems = townBuildingDef.dropItemsOnBuildingFinished;
			if (!townBuildingDef.dontPlaceBuildingOnWgo)
			{
				craftDef.onCraftEndExpressions.Insert(0, new LazyExpression("CreateTownBuilding()"));
			}
			else
			{
				craftDef.onCraftEndExpressions.Insert(0, new LazyExpression("DestroySignboard()"));
			}
			this.craftDefs.Add(craftDef);
		}
	}

	// Token: 0x06000D12 RID: 3346 RVA: 0x000427D0 File Offset: 0x000409D0
	private AlchemyMixDef BuildAlchemyMixDef(AlchemyMixSourceDef source)
	{
		AlchemyFormulaDef data = base.GetData<AlchemyFormulaDef>(source.formulaId);
		Vector3Int runesAsVector3Int = data.GetRunesAsVector3Int();
		List<string> list = new List<string>();
		if (!string.IsNullOrEmpty(source.ingredient1))
		{
			list.Add(source.ingredient1);
		}
		if (!string.IsNullOrEmpty(source.ingredient2))
		{
			list.Add(source.ingredient2);
		}
		if (!string.IsNullOrEmpty(source.ingredient3))
		{
			list.Add(source.ingredient3);
		}
		AlchemyMixDef alchemyMixDef = new AlchemyMixDef();
		alchemyMixDef.id = source.mixId;
		alchemyMixDef.ingredients = list.ToArray();
		alchemyMixDef.craftsIn = data.craftsIn;
		alchemyMixDef.duration = new LazyExpression(3.ToString());
		alchemyMixDef.isMulticraftDisabled = true;
		alchemyMixDef.talentLock = 0;
		alchemyMixDef.techRed = new LazyExpression(runesAsVector3Int.x.ToString());
		alchemyMixDef.techGreen = new LazyExpression(runesAsVector3Int.y.ToString());
		alchemyMixDef.techBlue = new LazyExpression(runesAsVector3Int.z.ToString());
		alchemyMixDef.outputItems = new OutputItems();
		ChanceOutputItem chanceOutputItem = new ChanceOutputItem();
		chanceOutputItem.id = data.id;
		chanceOutputItem.count = new LazyExpression(1.ToString());
		alchemyMixDef.outputItems.chanceOutputItems.Add(chanceOutputItem);
		alchemyMixDef.onCraftEndExpressions = data.onCraftEndExpressions;
		return alchemyMixDef;
	}

	// Token: 0x06000D13 RID: 3347 RVA: 0x00042930 File Offset: 0x00040B30
	public static CraftDef GetAlchemyMixCraftDef(string craftId)
	{
		CraftDef craftDef;
		if (GameBalance.Me.runtimeCraftDefsCacheAlchemy.TryGetValue(craftId, out craftDef))
		{
			return craftDef;
		}
		AlchemyMixDef alchemyMixDef = GameBalance.GetAlchemyMixDef(craftId);
		if (alchemyMixDef == null)
		{
			return null;
		}
		GameBalance.Me.CreateRelatedStuffForSingleMix(alchemyMixDef);
		CraftDef craftDef2;
		if (GameBalance.Me.runtimeCraftDefsCacheAlchemy.TryGetValue(craftId, out craftDef2))
		{
			return craftDef2;
		}
		string text = ((alchemyMixDef.outputItems != null && alchemyMixDef.outputItems.chanceOutputItems.Count > 0) ? alchemyMixDef.outputItems.chanceOutputItems[0].id : "<empty>");
		Debug.LogError(string.Concat(new string[] { "#alch# Failed to create alchemy workbench craft. craftId:[", craftId, "] outputId:[", text, "]" }));
		return null;
	}

	// Token: 0x06000D14 RID: 3348 RVA: 0x000429EC File Offset: 0x00040BEC
	public void CreateAlchemyMixingDefs()
	{
		List<ItemDef> list = new List<ItemDef>();
		List<CraftDef> list2 = new List<CraftDef>();
		this.alchemyMixSourceDefs.Clear();
		this.alchemyMixSourcesByIdCache.Clear();
		for (int i = 0; i < this.itemDefs.Count; i++)
		{
			ItemDef itemDef = this.itemDefs[i];
			if (itemDef.canBeUsedInAlchemy && (itemDef.runesRed.EvaluateInt() > 0 || itemDef.runesGreen.EvaluateInt() > 0 || itemDef.runesBlue.EvaluateInt() > 0))
			{
				list.Add(itemDef);
			}
		}
		for (int j = 0; j < this.craftDefs.Count; j++)
		{
			CraftDef craftDef = this.craftDefs[j];
			if (craftDef.id.EndsWith("_boost"))
			{
				list2.Add(craftDef);
			}
		}
		foreach (AlchemyFormulaDef alchemyFormulaDef in this.alchemyFormulaDefs)
		{
			Vector3Int runesAsVector3Int = alchemyFormulaDef.GetRunesAsVector3Int();
			for (int k = 0; k < list.Count; k++)
			{
				ItemDef itemDef2 = list[k];
				Vector3Int runesAsVector3Int2 = itemDef2.GetRunesAsVector3Int();
				if (runesAsVector3Int == runesAsVector3Int2)
				{
					this.<CreateAlchemyMixingDefs>g__TryAddSource|102_0(alchemyFormulaDef, new List<ItemDef> { itemDef2 }, null);
				}
				foreach (CraftDef craftDef2 in list2)
				{
					if (craftDef2.GetBoostRunesAsVector3Int() + runesAsVector3Int2 == runesAsVector3Int)
					{
						this.<CreateAlchemyMixingDefs>g__TryAddSource|102_0(alchemyFormulaDef, new List<ItemDef> { itemDef2 }, craftDef2);
					}
				}
				for (int l = 0; l < list.Count; l++)
				{
					ItemDef itemDef3 = list[l];
					Vector3Int runesAsVector3Int3 = itemDef3.GetRunesAsVector3Int();
					if (runesAsVector3Int2 + runesAsVector3Int3 == runesAsVector3Int)
					{
						this.<CreateAlchemyMixingDefs>g__TryAddSource|102_0(alchemyFormulaDef, new List<ItemDef> { itemDef2, itemDef3 }, null);
					}
					foreach (CraftDef craftDef3 in list2)
					{
						if (craftDef3.GetBoostRunesAsVector3Int() + runesAsVector3Int2 + runesAsVector3Int3 == runesAsVector3Int)
						{
							this.<CreateAlchemyMixingDefs>g__TryAddSource|102_0(alchemyFormulaDef, new List<ItemDef> { itemDef2, itemDef3 }, craftDef3);
						}
					}
					for (int m = 0; m < list.Count; m++)
					{
						ItemDef itemDef4 = list[m];
						Vector3Int runesAsVector3Int4 = itemDef4.GetRunesAsVector3Int();
						if (runesAsVector3Int2 + runesAsVector3Int3 + runesAsVector3Int4 == runesAsVector3Int)
						{
							this.<CreateAlchemyMixingDefs>g__TryAddSource|102_0(alchemyFormulaDef, new List<ItemDef> { itemDef2, itemDef3, itemDef4 }, null);
						}
						foreach (CraftDef craftDef4 in list2)
						{
							if (craftDef4.GetBoostRunesAsVector3Int() + runesAsVector3Int2 + runesAsVector3Int3 + runesAsVector3Int4 == runesAsVector3Int)
							{
								this.<CreateAlchemyMixingDefs>g__TryAddSource|102_0(alchemyFormulaDef, new List<ItemDef> { itemDef2, itemDef3, itemDef4 }, craftDef4);
							}
						}
					}
				}
			}
		}
		this.AddMixCraftsAliases();
	}

	// Token: 0x06000D15 RID: 3349 RVA: 0x00042DBC File Offset: 0x00040FBC
	private void CreateRelatedStuffForSingleMix(AlchemyMixDef cur)
	{
		CraftDef dataOrNull = base.GetDataOrNull<CraftDef>("default_alchemy_craft");
		string id = cur.outputItems.chanceOutputItems[0].id;
		CraftDef craftDef = base.GetDataOrNull<CraftDef>(cur.id);
		if (craftDef != null)
		{
			this.runtimeCraftDefsCacheAlchemy[cur.id] = craftDef;
			return;
		}
		if (dataOrNull == null)
		{
			Debug.LogError("#alch# Can't create alchemy workbench craft [" + cur.id + "], default_alchemy_craft is missing.");
			return;
		}
		craftDef = dataOrNull.Copy();
		craftDef.id = cur.id;
		craftDef.outputItems = new OutputItems();
		craftDef.talentLock = cur.talentLock;
		craftDef.techBlue = cur.techBlue;
		craftDef.techRed = cur.techRed;
		craftDef.techGreen = cur.techGreen;
		craftDef.tabId = id;
		craftDef.needItems = new List<NeedItemData>();
		string[] ingredients = cur.ingredients;
		for (int j = 0; j < ingredients.Length; j++)
		{
			string ingredient = ingredients[j];
			NeedItemData needItemData = craftDef.needItems.Find((NeedItemData i) => i.id == ingredient);
			if (needItemData != null)
			{
				needItemData.Reinitialize(needItemData.id, needItemData.GetCount(null) + 1);
			}
			else
			{
				craftDef.needItems.Add(new NeedItemData(ingredient, 1));
			}
		}
		craftDef.needItems.Add(new NeedItemData("alchemy_flask", 1));
		craftDef.outputItems.chanceOutputItems.Add(cur.outputItems.chanceOutputItems[0]);
		if (cur.id.EndsWith("_boost"))
		{
			CraftDef boostCraft = cur.BoostCraft;
			craftDef.craftsIn = new List<string>();
			craftDef.craftsIn.AddRange(boostCraft.craftsIn);
			craftDef.needItems.AddRange(boostCraft.needItems);
			if (boostCraft.talentLock > craftDef.talentLock)
			{
				craftDef.talentLock = boostCraft.talentLock;
			}
		}
		this.runtimeCraftDefsCacheAlchemy[craftDef.id] = craftDef;
		this.AddCraftDefToCache(craftDef);
	}

	// Token: 0x06000D16 RID: 3350 RVA: 0x00042FC0 File Offset: 0x000411C0
	public void AddMixCraftsAliases()
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		foreach (AlchemyMixSourceDef alchemyMixSourceDef in this.alchemyMixSourceDefs)
		{
			list.Add(alchemyMixSourceDef.mixId);
			list2.Add(alchemyMixSourceDef.formulaId);
		}
		LLBase.AddAliases(list, list2);
	}

	// Token: 0x06000D17 RID: 3351 RVA: 0x00043038 File Offset: 0x00041238
	private void FindCoordsForFreeTalentLevelUps()
	{
		for (int i = 0; i < this.talentDefs.Count; i++)
		{
			string id = this.talentDefs[i].id;
			Dictionary<Vector2, TalentLevelUpDef> dictionary = new Dictionary<Vector2, TalentLevelUpDef>();
			Queue<TalentLevelUpDef> queue = new Queue<TalentLevelUpDef>();
			foreach (TalentLevelUpDef talentLevelUpDef in this.talentLevelUpDefs)
			{
				if (!(talentLevelUpDef.talentId != id))
				{
					if (!talentLevelUpDef.isFreeCoordinates)
					{
						dictionary[talentLevelUpDef.TreePos] = talentLevelUpDef;
					}
					else
					{
						queue.Enqueue(talentLevelUpDef);
					}
				}
			}
			if (queue.Count == 0)
			{
				return;
			}
			for (float num = 0f; num < 5f; num += 0.5f)
			{
				for (float num2 = 0f; num2 < 6f; num2 += 0.5f)
				{
					Vector2 vector = new Vector2(num, num2);
					if (!dictionary.ContainsKey(vector))
					{
						dictionary[vector] = queue.Dequeue();
						if (queue.Count == 0)
						{
							return;
						}
					}
				}
			}
		}
	}

	// Token: 0x06000D18 RID: 3352 RVA: 0x00043160 File Offset: 0x00041360
	private void CreateConveyorCache()
	{
		this.conveyorWgosCache.Clear();
		foreach (WGODef wgodef in this.wgoDefs)
		{
			if (wgodef.conveyorType != ConveyorElementType.None)
			{
				this.conveyorWgosCache.Add(wgodef.id, wgodef);
			}
		}
	}

	// Token: 0x06000D19 RID: 3353 RVA: 0x000431D4 File Offset: 0x000413D4
	private void CreateWorkbenchExtensionsCache()
	{
		this.workbenchCraftsByParentAndExtensionCache.Clear();
		this.workbenchParentsByExtensionCache.Clear();
		this.workbenchExtensionsByParentCache.Clear();
		this.workbenchExtensionIdsCache.Clear();
		this.workbenchesWhichUseExtensions.Clear();
		foreach (WGODef wgodef in this.wgoDefs)
		{
			if (wgodef.attachedWorkbenchExtensionIds.Count > 0)
			{
				this.workbenchesWhichUseExtensions.Add(wgodef);
			}
			foreach (string text in wgodef.attachedWorkbenchExtensionIds)
			{
				this.workbenchExtensionIdsCache.Add(text);
				List<WGODef> list;
				if (this.workbenchParentsByExtensionCache.TryGetValue(text, out list))
				{
					if (!list.Contains(wgodef))
					{
						list.Add(wgodef);
					}
				}
				else
				{
					this.workbenchParentsByExtensionCache[text] = new List<WGODef> { wgodef };
				}
				HashSet<string> hashSet;
				if (!this.workbenchExtensionsByParentCache.TryGetValue(wgodef.id, out hashSet))
				{
					hashSet = new HashSet<string>();
					this.workbenchExtensionsByParentCache[wgodef.id] = hashSet;
				}
				hashSet.Add(text);
			}
		}
		foreach (CraftDef craftDef in this.craftDefs)
		{
			if (!string.IsNullOrEmpty(craftDef.extensionNeedId))
			{
				foreach (string text2 in craftDef.craftsIn)
				{
					Dictionary<string, List<CraftDef>> dictionary;
					if (!this.workbenchCraftsByParentAndExtensionCache.TryGetValue(text2, out dictionary))
					{
						dictionary = new Dictionary<string, List<CraftDef>>();
						this.workbenchCraftsByParentAndExtensionCache[text2] = dictionary;
					}
					List<CraftDef> list2;
					if (!dictionary.TryGetValue(craftDef.extensionNeedId, out list2))
					{
						list2 = new List<CraftDef>();
						dictionary[craftDef.extensionNeedId] = list2;
					}
					if (!list2.Contains(craftDef))
					{
						list2.Add(craftDef);
					}
				}
			}
		}
	}

	// Token: 0x06000D1A RID: 3354 RVA: 0x00043430 File Offset: 0x00041630
	public bool IsWorkbenchExtensionId(string extensionId)
	{
		return !string.IsNullOrEmpty(extensionId) && this.workbenchExtensionIdsCache.Contains(extensionId);
	}

	// Token: 0x06000D1B RID: 3355 RVA: 0x00043448 File Offset: 0x00041648
	public WGODef GetWorkbenchExtensionLogicDef(string wgoId)
	{
		if (string.IsNullOrEmpty(wgoId))
		{
			return null;
		}
		WGODef data = base.GetData<WGODef>(wgoId);
		if (data == null)
		{
			return null;
		}
		if (this.workbenchesWhichUseExtensions.Contains(data) || this.IsWorkbenchExtensionId(wgoId))
		{
			return data;
		}
		if (wgoId.EndsWith("_place", StringComparison.Ordinal))
		{
			int length = "_place".Length;
			string text = wgoId.Substring(0, wgoId.Length - length);
			WGODef data2 = base.GetData<WGODef>(text);
			if (data2 != null && (this.workbenchesWhichUseExtensions.Contains(data2) || this.IsWorkbenchExtensionId(text)))
			{
				return data2;
			}
		}
		return data;
	}

	// Token: 0x06000D1C RID: 3356 RVA: 0x000434D7 File Offset: 0x000416D7
	public bool TryGetParentWorkbenchDefsForExtension(string extensionId, out List<WGODef> parentWorkbenchDefs)
	{
		if (string.IsNullOrEmpty(extensionId))
		{
			parentWorkbenchDefs = null;
			return false;
		}
		return this.workbenchParentsByExtensionCache.TryGetValue(extensionId, out parentWorkbenchDefs);
	}

	// Token: 0x06000D1D RID: 3357 RVA: 0x000434F4 File Offset: 0x000416F4
	public bool IsExtensionAllowedForParentWorkbench(string parentWorkbenchId, string extensionId)
	{
		HashSet<string> hashSet;
		return !string.IsNullOrEmpty(parentWorkbenchId) && !string.IsNullOrEmpty(extensionId) && this.workbenchExtensionsByParentCache.TryGetValue(parentWorkbenchId, out hashSet) && hashSet.Contains(extensionId);
	}

	// Token: 0x06000D1E RID: 3358 RVA: 0x0004352C File Offset: 0x0004172C
	public HashSet<string> GetAllowedExtensionIdsForParentWorkbench(string parentWorkbenchId)
	{
		if (string.IsNullOrEmpty(parentWorkbenchId))
		{
			return new HashSet<string>();
		}
		HashSet<string> hashSet;
		if (this.workbenchExtensionsByParentCache.TryGetValue(parentWorkbenchId, out hashSet))
		{
			return new HashSet<string>(hashSet);
		}
		return new HashSet<string>();
	}

	// Token: 0x06000D1F RID: 3359 RVA: 0x00043564 File Offset: 0x00041764
	public List<CraftDef> GetWorkbenchExtensionCrafts(string parentWorkbenchId, string extensionId)
	{
		if (string.IsNullOrEmpty(parentWorkbenchId) || string.IsNullOrEmpty(extensionId))
		{
			return new List<CraftDef>();
		}
		Dictionary<string, List<CraftDef>> dictionary;
		List<CraftDef> list;
		if (this.workbenchCraftsByParentAndExtensionCache.TryGetValue(parentWorkbenchId, out dictionary) && dictionary.TryGetValue(extensionId, out list))
		{
			return new List<CraftDef>(list);
		}
		return new List<CraftDef>();
	}

	// Token: 0x06000D20 RID: 3360 RVA: 0x000435B0 File Offset: 0x000417B0
	public bool HasWgoIdByGroup(string wgoGroup, string wgoId)
	{
		List<string> list;
		return !string.IsNullOrEmpty(wgoGroup) && this.wgoIdsByGroup.TryGetValue(wgoGroup, out list) && list.Contains(wgoId);
	}

	// Token: 0x06000D21 RID: 3361 RVA: 0x000435E0 File Offset: 0x000417E0
	private void CreateFightersCache()
	{
		this.fighterWgoIdsCache.Clear();
		foreach (FighterDef fighterDef in this.fighterDefs)
		{
			this.fighterWgoIdsCache.Add(fighterDef.id);
		}
	}

	// Token: 0x06000D22 RID: 3362 RVA: 0x0004364C File Offset: 0x0004184C
	private void CreateCustomBuildAreaIdToWgoIdsCache()
	{
		this.customBuildAreaIdToWgoIds.Clear();
		foreach (BuildingDef buildingDef in this.buildingDefs)
		{
			if (!string.IsNullOrEmpty(buildingDef.customBuildAreaId))
			{
				if (!this.customBuildAreaIdToWgoIds.ContainsKey(buildingDef.customBuildAreaId))
				{
					this.customBuildAreaIdToWgoIds.Add(buildingDef.customBuildAreaId, new List<string>());
				}
				this.customBuildAreaIdToWgoIds[buildingDef.customBuildAreaId].Add(buildingDef.wgoId);
			}
		}
	}

	// Token: 0x06000D23 RID: 3363 RVA: 0x000436F8 File Offset: 0x000418F8
	private void CreateGardenGrowingCraftsCache()
	{
		this.gardenGrowingCrafts.Clear();
		foreach (CraftDef craftDef in this.craftDefs)
		{
			if (craftDef.id.StartsWith("garden_") && craftDef.id.Contains("_growing"))
			{
				this.gardenGrowingCrafts.TryAdd(craftDef.id, craftDef);
				if (craftDef.id.Contains("grape") || craftDef.id.Contains("hop"))
				{
					this.gardenGrowingCraftTypes.TryAdd(craftDef.id, CraftParamsData.GardenType.Vineyard);
				}
				else
				{
					this.gardenGrowingCraftTypes.TryAdd(craftDef.id, CraftParamsData.GardenType.None);
				}
			}
		}
	}

	// Token: 0x06000D24 RID: 3364 RVA: 0x000437D8 File Offset: 0x000419D8
	private void CreateQuestDefByFinishPhraseCache()
	{
		if (FinishPhrasesByWgoParser.PhrasesByWgoData == null)
		{
			return;
		}
		using (List<QuestDef>.Enumerator enumerator = this.questDefs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				QuestDef questDef = enumerator.Current;
				if (!string.IsNullOrEmpty(questDef.finishCheck.phrase) && FinishPhrasesByWgoParser.PhrasesByWgoData.finishPhrasesByWgo.Find((PhrasesByWgo x) => x.phrases.Contains(questDef.finishCheck.phrase)) != null)
				{
					this.questDefByFinishPhrase.TryAdd(questDef.finishCheck.phrase, questDef);
				}
			}
		}
	}

	// Token: 0x06000D25 RID: 3365 RVA: 0x00043894 File Offset: 0x00041A94
	private void CreateWgoIdsByGroupCache()
	{
		foreach (WGODef wgodef in this.wgoDefs)
		{
			if (!string.IsNullOrEmpty(wgodef.wgoGroup))
			{
				List<string> list;
				if (!this.wgoIdsByGroup.TryGetValue(wgodef.wgoGroup, out list))
				{
					this.wgoIdsByGroup.Add(wgodef.wgoGroup, new List<string> { wgodef.id });
				}
				else
				{
					list.Add(wgodef.id);
				}
			}
		}
	}

	// Token: 0x06000D27 RID: 3367 RVA: 0x00043C40 File Offset: 0x00041E40
	[CompilerGenerated]
	private void <CreateCraftInItemsCache>g__ScanOutputItems|77_0(OutputItems outputItems, ref GameBalance.<>c__DisplayClass77_0 A_2)
	{
		for (int i = 0; i < outputItems.chanceOutputItems.Count; i++)
		{
			this.<CreateCraftInItemsCache>g__AddChanceItemDef|77_2(outputItems.chanceOutputItems[i], ref A_2);
		}
		for (int j = 0; j < outputItems.groupChanceOutputItems.Count; j++)
		{
			for (int k = 0; k < outputItems.groupChanceOutputItems[j].chanceItems.Count; k++)
			{
				this.<CreateCraftInItemsCache>g__AddChanceItemDef|77_2(outputItems.groupChanceOutputItems[j].chanceItems[k], ref A_2);
			}
		}
	}

	// Token: 0x06000D28 RID: 3368 RVA: 0x00043CCC File Offset: 0x00041ECC
	[CompilerGenerated]
	private void <CreateCraftInItemsCache>g__AddChanceItemDef|77_2(ChanceOutputItem chanceOutputItem, ref GameBalance.<>c__DisplayClass77_0 A_2)
	{
		if (chanceOutputItem.isStarGroup)
		{
			for (int i = 0; i < this.starGroupItemsCache[chanceOutputItem.id].Count; i++)
			{
				A_2.allPossibleItemsAsOutput.Add(this.starGroupItemsCache[chanceOutputItem.id][i].id);
			}
			return;
		}
		A_2.allPossibleItemsAsOutput.Add(chanceOutputItem.id);
	}

	// Token: 0x06000D29 RID: 3369 RVA: 0x00043D3C File Offset: 0x00041F3C
	[CompilerGenerated]
	private void <CreateCraftInItemsCache>g__ScanNeedItems|77_1(List<NeedItemData> needItems, ref GameBalance.<>c__DisplayClass77_0 A_2)
	{
		for (int i = 0; i < needItems.Count; i++)
		{
			NeedItemData needItemData = needItems[i];
			switch (needItemData.groupType)
			{
			case ItemGroup.None:
				A_2.allPossibleItemsAsOutput.Add(needItemData.id);
				break;
			case ItemGroup.Common:
			{
				for (int j = 0; j < this.groupItemsCache[needItemData.id].Count; j++)
				{
					A_2.allPossibleItemsAsOutput.Add(this.groupItemsCache[needItemData.id][j].id);
				}
				break;
			}
			case ItemGroup.Star:
			{
				for (int k = 0; k < this.starGroupItemsCache[needItemData.id].Count; k++)
				{
					A_2.allPossibleItemsAsOutput.Add(this.starGroupItemsCache[needItemData.id][k].id);
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}

	// Token: 0x06000D2A RID: 3370 RVA: 0x00043E3C File Offset: 0x0004203C
	[CompilerGenerated]
	private void <AddCraftDefToCache>g__AddChanceItemDef|78_0(ChanceOutputItem chanceOutputItem, ref GameBalance.<>c__DisplayClass78_0 A_2)
	{
		if (chanceOutputItem.isStarGroup)
		{
			for (int i = 0; i < this.starGroupItemsCache[chanceOutputItem.id].Count; i++)
			{
				A_2.allPossibleItemsAsOutput.Add(this.starGroupItemsCache[chanceOutputItem.id][i].id);
			}
			return;
		}
		A_2.allPossibleItemsAsOutput.Add(chanceOutputItem.id);
	}

	// Token: 0x06000D2B RID: 3371 RVA: 0x00043EAC File Offset: 0x000420AC
	[CompilerGenerated]
	private void <AddCraftDefToCache>g__ScanOutputItems|78_1(OutputItems outputItems, ref GameBalance.<>c__DisplayClass78_0 A_2)
	{
		for (int i = 0; i < outputItems.chanceOutputItems.Count; i++)
		{
			this.<AddCraftDefToCache>g__AddChanceItemDef|78_0(outputItems.chanceOutputItems[i], ref A_2);
		}
		for (int j = 0; j < outputItems.groupChanceOutputItems.Count; j++)
		{
			for (int k = 0; k < outputItems.groupChanceOutputItems[j].chanceItems.Count; k++)
			{
				this.<AddCraftDefToCache>g__AddChanceItemDef|78_0(outputItems.groupChanceOutputItems[j].chanceItems[k], ref A_2);
			}
		}
	}

	// Token: 0x06000D2C RID: 3372 RVA: 0x00043F38 File Offset: 0x00042138
	[CompilerGenerated]
	private void <AddCraftDefToCache>g__ScanNeedItems|78_2(List<NeedItemData> needItems, ref GameBalance.<>c__DisplayClass78_0 A_2)
	{
		for (int i = 0; i < needItems.Count; i++)
		{
			NeedItemData needItemData = needItems[i];
			switch (needItemData.groupType)
			{
			case ItemGroup.None:
				A_2.allPossibleItemsAsOutput.Add(needItemData.id);
				break;
			case ItemGroup.Common:
			{
				for (int j = 0; j < this.groupItemsCache[needItemData.id].Count; j++)
				{
					A_2.allPossibleItemsAsOutput.Add(this.groupItemsCache[needItemData.id][j].id);
				}
				break;
			}
			case ItemGroup.Star:
			{
				for (int k = 0; k < this.starGroupItemsCache[needItemData.id].Count; k++)
				{
					A_2.allPossibleItemsAsOutput.Add(this.starGroupItemsCache[needItemData.id][k].id);
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}

	// Token: 0x06000D2D RID: 3373 RVA: 0x00044038 File Offset: 0x00042238
	[CompilerGenerated]
	private void <CreateAlchemyMixingDefs>g__TryAddSource|102_0(AlchemyFormulaDef formulaDef, List<ItemDef> ingredients, CraftDef boostCraft)
	{
		string text = AlchemyMixDef.MixId(ingredients.Select((ItemDef i) => i.id).ToArray<string>(), boostCraft);
		if (this.alchemyMixSourcesByIdCache.ContainsKey(text))
		{
			return;
		}
		AlchemyMixSourceDef alchemyMixSourceDef = new AlchemyMixSourceDef();
		alchemyMixSourceDef.mixId = text;
		alchemyMixSourceDef.formulaId = formulaDef.id;
		alchemyMixSourceDef.ingredient1 = ((ingredients.Count > 0) ? ingredients[0].id : "");
		alchemyMixSourceDef.ingredient2 = ((ingredients.Count > 1) ? ingredients[1].id : "");
		alchemyMixSourceDef.ingredient3 = ((ingredients.Count > 2) ? ingredients[2].id : "");
		this.alchemyMixSourceDefs.Add(alchemyMixSourceDef);
		this.alchemyMixSourcesByIdCache[text] = alchemyMixSourceDef;
	}

	// Token: 0x0400100D RID: 4109
	private const string GAME_BALANCE_ASSET_PATH = "Assets/Resources/GameBalance.asset";

	// Token: 0x0400100E RID: 4110
	[BalanceTab("Items", 4)]
	public List<ItemDef> itemDefs = new List<ItemDef>();

	// Token: 0x0400100F RID: 4111
	[BalanceTab("WGOs", -1)]
	public List<WGODef> wgoDefs = new List<WGODef>();

	// Token: 0x04001010 RID: 4112
	[BalanceTab("WSOs", -1)]
	public List<WSODef> wsoDefs = new List<WSODef>();

	// Token: 0x04001011 RID: 4113
	[BalanceTab("WGOGroups", -1)]
	public List<WgoGroupDef> wgoGroupDefs = new List<WgoGroupDef>();

	// Token: 0x04001012 RID: 4114
	[BalanceTab("ToolTypes", -1)]
	public List<ToolTypeDef> toolTypes = new List<ToolTypeDef>();

	// Token: 0x04001013 RID: 4115
	[BalanceTab("Fighters", -1)]
	public List<FighterDef> fighterDefs = new List<FighterDef>();

	// Token: 0x04001014 RID: 4116
	[BalanceTab("Crafts", -1)]
	public List<CraftDef> craftDefs = new List<CraftDef>();

	// Token: 0x04001015 RID: 4117
	[BalanceTab("Talents", -1)]
	public List<TalentDef> talentDefs = new List<TalentDef>();

	// Token: 0x04001016 RID: 4118
	[BalanceTab("TalentExpLevels", -1)]
	public List<TalentExpLevelDef> talentExpLevelDefs = new List<TalentExpLevelDef>();

	// Token: 0x04001017 RID: 4119
	[BalanceTab("Inspirations", -1)]
	public List<InspirationDef> inspirationDefs = new List<InspirationDef>();

	// Token: 0x04001018 RID: 4120
	[BalanceTab("Perks", -1)]
	public List<PerkDef> perkDefs = new List<PerkDef>();

	// Token: 0x04001019 RID: 4121
	[BalanceTab("WorldZones", -1)]
	public List<WorldZoneDef> worldZoneDefs = new List<WorldZoneDef>();

	// Token: 0x0400101A RID: 4122
	[BalanceTab("Buildings", -1)]
	public List<BuildingDef> buildingDefs = new List<BuildingDef>();

	// Token: 0x0400101B RID: 4123
	[BalanceTab("Sermons", -1)]
	public List<SermonDef> sermonDefs = new List<SermonDef>();

	// Token: 0x0400101C RID: 4124
	[BalanceTab("Survey", -1)]
	public List<SurveyDef> surveyDefs = new List<SurveyDef>();

	// Token: 0x0400101D RID: 4125
	[BalanceTab("SermonConfigs", -1)]
	public List<SermonConfigDef> sermonConfigDefs = new List<SermonConfigDef>();

	// Token: 0x0400101E RID: 4126
	[BalanceTab("Logics", -1)]
	public List<GameLogicDef> gameLogicsDefs = new List<GameLogicDef>();

	// Token: 0x0400101F RID: 4127
	[BalanceTab("Bodies", -1)]
	public List<BodyDef> bodyDefs = new List<BodyDef>();

	// Token: 0x04001020 RID: 4128
	[BalanceTab("Techs", -1)]
	public List<TechDef> techDefs = new List<TechDef>();

	// Token: 0x04001021 RID: 4129
	[BalanceTab("GameResSystem", -1)]
	public List<GameResSystemDef> gameResSystemDefs = new List<GameResSystemDef>();

	// Token: 0x04001022 RID: 4130
	[BalanceTab("Vendors", -1)]
	public List<VendorDef> vendorDefs = new List<VendorDef>();

	// Token: 0x04001023 RID: 4131
	[BalanceTab("VendorOrders", -1)]
	public List<VendorOrderDef> vendorOrderDefs = new List<VendorOrderDef>();

	// Token: 0x04001024 RID: 4132
	[BalanceTab("TalentsLevelUps", -1)]
	public List<TalentLevelUpDef> talentLevelUpDefs = new List<TalentLevelUpDef>();

	// Token: 0x04001025 RID: 4133
	[BalanceTab("Quests", -1)]
	public List<QuestDef> questDefs = new List<QuestDef>();

	// Token: 0x04001026 RID: 4134
	[BalanceTab("Alchemy_formulas", -1)]
	public List<AlchemyFormulaDef> alchemyFormulaDefs = new List<AlchemyFormulaDef>();

	// Token: 0x04001027 RID: 4135
	[BalanceTab("Fishing", -1)]
	public List<FishingDef> fishingDefs = new List<FishingDef>();

	// Token: 0x04001028 RID: 4136
	[BalanceTab("Consts", -1)]
	public List<ConstDef> constDefs = new List<ConstDef>();

	// Token: 0x04001029 RID: 4137
	[BalanceTab("TownBuildings", -1)]
	public List<TownBuildingDef> townBuildingDefs = new List<TownBuildingDef>();

	// Token: 0x0400102A RID: 4138
	[BalanceTab("Fights", -1)]
	public List<FightDef> fightDefinitions = new List<FightDef>();

	// Token: 0x0400102B RID: 4139
	[BalanceTab("PorterStations", -1)]
	public List<PorterStationDef> porterStationDefs = new List<PorterStationDef>();

	// Token: 0x0400102C RID: 4140
	[BalanceTab("Mercenaries", -1)]
	public List<MercenariesDef> mercenariesDefs = new List<MercenariesDef>();

	// Token: 0x0400102D RID: 4141
	[BalanceTab("Achievements", -1)]
	public List<AchievementDefinition> achievementDefs = new List<AchievementDefinition>();

	// Token: 0x0400102E RID: 4142
	public List<AlchemyMixSourceDef> alchemyMixSourceDefs = new List<AlchemyMixSourceDef>();

	// Token: 0x0400102F RID: 4143
	public TalentExpLevelBalanceData yellow = new TalentExpLevelBalanceData();

	// Token: 0x04001030 RID: 4144
	public TalentExpLevelBalanceData green = new TalentExpLevelBalanceData();

	// Token: 0x04001031 RID: 4145
	public TalentExpLevelBalanceData red = new TalentExpLevelBalanceData();

	// Token: 0x04001032 RID: 4146
	public TalentExpLevelBalanceData orange = new TalentExpLevelBalanceData();

	// Token: 0x04001033 RID: 4147
	public TalentExpLevelBalanceData blue = new TalentExpLevelBalanceData();

	// Token: 0x04001034 RID: 4148
	[NonSerialized]
	public Dictionary<string, List<ItemDef>> starGroupItemsCache = new Dictionary<string, List<ItemDef>>();

	// Token: 0x04001035 RID: 4149
	[NonSerialized]
	public Dictionary<string, List<ItemDef>> groupItemsCache = new Dictionary<string, List<ItemDef>>();

	// Token: 0x04001036 RID: 4150
	[NonSerialized]
	public Dictionary<string, TalentExpLevelBalanceData> talentExpLevelsCache = new Dictionary<string, TalentExpLevelBalanceData>();

	// Token: 0x04001037 RID: 4151
	[NonSerialized]
	public List<InspirationLevelData> inspirationLevels = new List<InspirationLevelData>();

	// Token: 0x04001038 RID: 4152
	[NonSerialized]
	public Dictionary<string, InspirationLevelData> inspirationLevelsCache = new Dictionary<string, InspirationLevelData>();

	// Token: 0x04001039 RID: 4153
	[NonSerialized]
	public Dictionary<string, List<CraftDefBase>> craftsInCache = new Dictionary<string, List<CraftDefBase>>();

	// Token: 0x0400103A RID: 4154
	[NonSerialized]
	public Dictionary<string, List<string>> craftInItemsCache = new Dictionary<string, List<string>>();

	// Token: 0x0400103B RID: 4155
	[NonSerialized]
	public Dictionary<string, List<string>> craftInItemsCacheShownInTooltips = new Dictionary<string, List<string>>();

	// Token: 0x0400103C RID: 4156
	[NonSerialized]
	public Dictionary<string, List<BuildingDef>> buildDefsInBuilder = new Dictionary<string, List<BuildingDef>>();

	// Token: 0x0400103D RID: 4157
	[NonSerialized]
	public Dictionary<string, BuildingDef> buildableWgos = new Dictionary<string, BuildingDef>();

	// Token: 0x0400103E RID: 4158
	[NonSerialized]
	public Dictionary<string, BuildingDef> removableWgos = new Dictionary<string, BuildingDef>();

	// Token: 0x0400103F RID: 4159
	[NonSerialized]
	public Dictionary<string, List<QuestDef>> questsByQuestIdCache = new Dictionary<string, List<QuestDef>>();

	// Token: 0x04001040 RID: 4160
	[NonSerialized]
	public Dictionary<string, List<string>> questBrothersCache = new Dictionary<string, List<string>>();

	// Token: 0x04001041 RID: 4161
	[NonSerialized]
	public Dictionary<string, QuestDef> questDefByReqPhrase = new Dictionary<string, QuestDef>();

	// Token: 0x04001042 RID: 4162
	[NonSerialized]
	public Dictionary<ItemDef, List<CraftDef>> gardenCraftsPerItemCache = new Dictionary<ItemDef, List<CraftDef>>();

	// Token: 0x04001043 RID: 4163
	[NonSerialized]
	public Dictionary<string, WGODef> conveyorWgosCache = new Dictionary<string, WGODef>();

	// Token: 0x04001044 RID: 4164
	[NonSerialized]
	public Dictionary<string, Dictionary<string, List<CraftDef>>> workbenchCraftsByParentAndExtensionCache = new Dictionary<string, Dictionary<string, List<CraftDef>>>();

	// Token: 0x04001045 RID: 4165
	[NonSerialized]
	public Dictionary<string, List<WGODef>> workbenchParentsByExtensionCache = new Dictionary<string, List<WGODef>>();

	// Token: 0x04001046 RID: 4166
	[NonSerialized]
	public Dictionary<string, HashSet<string>> workbenchExtensionsByParentCache = new Dictionary<string, HashSet<string>>();

	// Token: 0x04001047 RID: 4167
	[NonSerialized]
	public HashSet<string> workbenchExtensionIdsCache = new HashSet<string>();

	// Token: 0x04001048 RID: 4168
	[NonSerialized]
	public HashSet<WGODef> workbenchesWhichUseExtensions = new HashSet<WGODef>();

	// Token: 0x04001049 RID: 4169
	[NonSerialized]
	public HashSet<string> fighterWgoIdsCache = new HashSet<string>();

	// Token: 0x0400104A RID: 4170
	[NonSerialized]
	public Dictionary<string, List<string>> customBuildAreaIdToWgoIds = new Dictionary<string, List<string>>();

	// Token: 0x0400104B RID: 4171
	[NonSerialized]
	public Dictionary<AutopsyTypeCraft, Dictionary<string, CraftDef>> autopsyCraftsByTypeAndItemCache = new Dictionary<AutopsyTypeCraft, Dictionary<string, CraftDef>>();

	// Token: 0x0400104C RID: 4172
	[NonSerialized]
	public Dictionary<string, CraftDef> gardenGrowingCrafts = new Dictionary<string, CraftDef>();

	// Token: 0x0400104D RID: 4173
	[NonSerialized]
	public Dictionary<string, CraftParamsData.GardenType> gardenGrowingCraftTypes = new Dictionary<string, CraftParamsData.GardenType>();

	// Token: 0x0400104E RID: 4174
	[NonSerialized]
	public Dictionary<string, AlchemyMixDef> alchemyMixDefsCache = new Dictionary<string, AlchemyMixDef>();

	// Token: 0x0400104F RID: 4175
	[NonSerialized]
	public Dictionary<string, AlchemyMixSourceDef> alchemyMixSourcesByIdCache = new Dictionary<string, AlchemyMixSourceDef>();

	// Token: 0x04001050 RID: 4176
	[NonSerialized]
	public Dictionary<string, CraftDef> runtimeCraftDefsCacheAlchemy = new Dictionary<string, CraftDef>();

	// Token: 0x04001051 RID: 4177
	[NonSerialized]
	public Dictionary<string, QuestDef> questDefByFinishPhrase = new Dictionary<string, QuestDef>();

	// Token: 0x04001052 RID: 4178
	[NonSerialized]
	public Dictionary<string, List<string>> wgoIdsByGroup = new Dictionary<string, List<string>>();

	// Token: 0x04001053 RID: 4179
	private static GameBalance instance;
}
