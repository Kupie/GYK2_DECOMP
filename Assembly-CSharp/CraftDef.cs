using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001E2 RID: 482
[Serializable]
public class CraftDef : CraftDefBase
{
	// Token: 0x1700020C RID: 524
	// (get) Token: 0x06000C1B RID: 3099 RVA: 0x0003CF8F File Offset: 0x0003B18F
	public bool IsMultipleCraftsDisabled
	{
		get
		{
			return this.isMulticraftDisabled || this.isOneTimeCraft;
		}
	}

	// Token: 0x1700020D RID: 525
	// (get) Token: 0x06000C1C RID: 3100 RVA: 0x0003CFA1 File Offset: 0x0003B1A1
	public override AutopsyTypeCraft AutopsyType
	{
		get
		{
			return this.autopsyTypeCraft;
		}
	}

	// Token: 0x1700020E RID: 526
	// (get) Token: 0x06000C1D RID: 3101 RVA: 0x0003CFAC File Offset: 0x0003B1AC
	public string Description
	{
		get
		{
			string text = LLBase.L(this.description);
			if (this.id.EndsWith("_boost"))
			{
				text += "\n";
				for (int i = 0; i < this.addWgoParamsOnStart.List.Count; i++)
				{
					GameResAtom gameResAtom = this.addWgoParamsOnStart.List[i];
					string type = gameResAtom.type;
					if (type == "rune_r" || type == "rune_b" || type == "rune_g")
					{
						text += gameResAtom.ToFormattedString(true, null, false, true, null);
					}
				}
			}
			return text;
		}
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x0003D054 File Offset: 0x0003B254
	public CraftDef Copy()
	{
		return new CraftDef
		{
			id = this.id,
			description = this.description,
			craftsIn = this.craftsIn,
			isHidden = this.isHidden,
			needItems = this.needItems,
			needItemsFromWgo = this.needItemsFromWgo,
			isStarCraft = this.isStarCraft,
			isFuelCraft = this.isFuelCraft,
			haveStarCraftOutput = this.haveStarCraftOutput,
			duration = this.duration,
			energyPerTick = this.energyPerTick,
			insanityPerTick = this.insanityPerTick,
			insanityLock = this.insanityLock,
			talentLock = this.talentLock,
			linkedPerks = this.linkedPerks,
			insanityLock = this.insanityLock,
			outputItems = this.outputItems,
			setWgoParamsOnStart = this.setWgoParamsOnStart,
			setWgoParamsOnFinish = this.setWgoParamsOnFinish,
			addWgoParamsOnStart = this.addWgoParamsOnStart,
			addWgoParamsOnFinish = this.addWgoParamsOnFinish,
			addItemsToWgoOnStart = this.addItemsToWgoOnStart,
			addItemsToWgoOnFinish = this.addItemsToWgoOnFinish,
			isAddToQueueDisabled = this.isAddToQueueDisabled,
			skipQueue = this.skipQueue,
			bronzeLevel = this.bronzeLevel,
			silverLevel = this.silverLevel,
			goldLevel = this.goldLevel,
			isNeedsUnlock = this.isNeedsUnlock,
			isOneTimeCraft = this.isOneTimeCraft,
			isForcedDifficultCraft = this.isForcedDifficultCraft,
			isObjDestroyCraft = this.isObjDestroyCraft,
			isMulticraftDisabled = this.isMulticraftDisabled,
			customItemTypeAction = this.customItemTypeAction,
			transferDestinationStart = this.transferDestinationStart,
			destinationItemStart = this.destinationItemStart,
			transferDestinationEnd = this.transferDestinationEnd,
			destinationItemEnd = this.destinationItemEnd,
			transferNeedsToDestinationOnStart = this.transferNeedsToDestinationOnStart,
			transferNeedsToDestinationOnFinish = this.transferNeedsToDestinationOnFinish,
			needItemsDurabilityUse = this.needItemsDurabilityUse,
			needItemsDurabilityUseIndex = this.needItemsDurabilityUseIndex,
			durabilityUseItem = this.durabilityUseItem,
			hasDurabilityUseItem = this.hasDurabilityUseItem,
			dropFromWgoItemsStart = this.dropFromWgoItemsStart,
			dropFromWgoItemsEnd = this.dropFromWgoItemsEnd,
			removeItemsFromWgo = this.removeItemsFromWgo,
			autopsyTypeCraft = this.autopsyTypeCraft,
			autopsyItemId = this.autopsyItemId,
			techRed = this.techRed,
			techGreen = this.techGreen,
			techBlue = this.techBlue,
			replaceWgoId = this.replaceWgoId,
			transferDataOnReplace = this.transferDataOnReplace,
			executeOnReplace = this.executeOnReplace,
			worldFxOnReplace = this.worldFxOnReplace,
			globalScriptOnCraftEnd = this.globalScriptOnCraftEnd,
			customCraftResultIcon = this.customCraftResultIcon,
			iconId = this.iconId,
			onCraftAddQueueExpressions = this.onCraftAddQueueExpressions,
			onCraftStartExpressions = this.onCraftStartExpressions,
			onCraftEndExpressions = this.onCraftEndExpressions,
			gameresPerSuccessfulProgress = this.gameresPerSuccessfulProgress
		};
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x0003D35C File Offset: 0x0003B55C
	public string GetCraftResultIcon(WgoData wgoData = null)
	{
		if (AlchemyMixDef.IsUnknownMixResult(this.id))
		{
			return "i_slot-question";
		}
		if (this.customCraftResultIcon != null && this.customCraftResultIcon.HasExpression)
		{
			string text = this.customCraftResultIcon.Evaluate(wgoData);
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
		}
		ItemDef itemDef = this.TryGetResultingItemDef(false);
		if (itemDef != null)
		{
			return itemDef.iconId;
		}
		OutputPreview outputPreviewFromItemsOrLinkedGrowing = this.GetOutputPreviewFromItemsOrLinkedGrowing(wgoData);
		return ((outputPreviewFromItemsOrLinkedGrowing != null) ? outputPreviewFromItemsOrLinkedGrowing.IconId : null) ?? string.Empty;
	}

	// Token: 0x06000C20 RID: 3104 RVA: 0x0003D3D8 File Offset: 0x0003B5D8
	private OutputPreview GetOutputPreviewFromItems(WgoData wgoData = null)
	{
		OutputPreview outputPreview = (this.isFuelCraft ? this.addItemsToWgoOnFinish.GetOutputPreview(this.id, wgoData) : base.GetOutputPreview(wgoData));
		if (outputPreview == null)
		{
			outputPreview = this.addItemsToWgoOnFinish.GetOutputPreview(this.id, wgoData);
		}
		return outputPreview;
	}

	// Token: 0x06000C21 RID: 3105 RVA: 0x0003D420 File Offset: 0x0003B620
	private OutputPreview GetOutputPreviewFromItemsOrLinkedGrowing(WgoData wgoData = null)
	{
		OutputPreview outputPreviewFromItems = this.GetOutputPreviewFromItems(wgoData);
		if (outputPreviewFromItems != null)
		{
			return outputPreviewFromItems;
		}
		return this.TryGetLinkedGardenGrowingOutputPreview(wgoData);
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x0003D444 File Offset: 0x0003B644
	private OutputPreview TryGetLinkedGardenGrowingOutputPreview(WgoData wgoData)
	{
		if (string.IsNullOrEmpty(this.id) || !this.id.Contains("_planting"))
		{
			return null;
		}
		string text = this.id.Replace("_planting", "_growing");
		if (text == this.id || GameBalance.Me == null)
		{
			return null;
		}
		CraftDef craftDef;
		if (!GameBalance.Me.gardenGrowingCrafts.TryGetValue(text, out craftDef) || craftDef == null)
		{
			return null;
		}
		OutputPreview outputPreviewFromItems = craftDef.GetOutputPreviewFromItems(wgoData);
		if (outputPreviewFromItems == null)
		{
			return null;
		}
		return new OutputPreview(this.id, outputPreviewFromItems.itemId, outputPreviewFromItems.isStarOutput, outputPreviewFromItems.count, outputPreviewFromItems.quality, outputPreviewFromItems.customIconId);
	}

	// Token: 0x06000C23 RID: 3107 RVA: 0x0003D4F4 File Offset: 0x0003B6F4
	public override OutputPreview GetOutputPreview(WgoData wgoData = null)
	{
		OutputPreview outputPreview = this.GetOutputPreviewFromItemsOrLinkedGrowing(wgoData);
		if (outputPreview == null)
		{
			outputPreview = new OutputPreview(this.id, "", this.isStarCraft, 1, this.isStarCraft ? 0 : (-1), this.id);
		}
		if (this.customCraftResultIcon != null && this.customCraftResultIcon.HasExpression)
		{
			string text = this.customCraftResultIcon.Evaluate(wgoData);
			if (!string.IsNullOrEmpty(text))
			{
				outputPreview.customIconId = text;
			}
		}
		ItemDef itemDef = this.TryGetResultingItemDef(false);
		if (!this.isStarCraft && itemDef != null && itemDef.qualityType == ItemDef.QualityType.Star)
		{
			outputPreview.quality = itemDef.quality;
		}
		if (AlchemyMixDef.IsUnknownMixResult(this.id))
		{
			outputPreview.customIconId = "i_slot-question";
			outputPreview.itemId = string.Empty;
			outputPreview.quality = -1;
		}
		return outputPreview;
	}

	// Token: 0x06000C24 RID: 3108 RVA: 0x0003D5BC File Offset: 0x0003B7BC
	public ItemDef TryGetResultingItemDef(bool tryGetFromPreview = true)
	{
		if (!this.<TryGetResultingItemDef>g__IsCachedNullOrEmpty|60_0())
		{
			return this.cachedResultingItemDef;
		}
		if (this.outputItems.chanceOutputItems.Count == 0 && this.outputItems.groupChanceOutputItems.Count == 0)
		{
			if (this.dropFromWgoItemsEnd.Count > 0)
			{
				NeedItemData needItemData = this.dropFromWgoItemsEnd[0];
				switch (needItemData.groupType)
				{
				case ItemGroup.None:
					this.cachedResultingItemDef = GameBalance.Me.GetDataOrNull<ItemDef>(needItemData.id);
					break;
				case ItemGroup.Common:
					this.cachedResultingItemDef = GameBalance.Me.groupItemsCache[needItemData.id][0];
					break;
				case ItemGroup.Star:
					this.cachedResultingItemDef = GameBalance.Me.starGroupItemsCache[needItemData.id][0];
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			if (this.<TryGetResultingItemDef>g__IsCachedNullOrEmpty|60_0() && this.removeItemsFromWgo.Count > 0)
			{
				NeedItemData needItemData2 = this.removeItemsFromWgo[0];
				switch (needItemData2.groupType)
				{
				case ItemGroup.None:
					this.cachedResultingItemDef = GameBalance.Me.GetDataOrNull<ItemDef>(needItemData2.id);
					break;
				case ItemGroup.Common:
					this.cachedResultingItemDef = GameBalance.Me.groupItemsCache[needItemData2.id][0];
					break;
				case ItemGroup.Star:
					this.cachedResultingItemDef = GameBalance.Me.starGroupItemsCache[needItemData2.id][0];
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}
		else if (this.outputItems.chanceOutputItems.Count > 0)
		{
			if (this.outputItems.chanceOutputItems[0].isStarGroup)
			{
				this.cachedResultingItemDef = GameBalance.Me.starGroupItemsCache[this.outputItems.chanceOutputItems[0].id][0];
			}
			else
			{
				this.cachedResultingItemDef = GameBalance.Me.GetDataOrNull<ItemDef>(this.outputItems.chanceOutputItems[0].id);
			}
		}
		else if (this.outputItems.groupChanceOutputItems.Count > 0)
		{
			if (this.outputItems.groupChanceOutputItems[0].chanceItems[0].isStarGroup)
			{
				this.cachedResultingItemDef = GameBalance.Me.starGroupItemsCache[this.outputItems.groupChanceOutputItems[0].chanceItems[0].id][0];
			}
			else
			{
				this.cachedResultingItemDef = GameBalance.Me.GetDataOrNull<ItemDef>(this.outputItems.groupChanceOutputItems[0].chanceItems[0].id);
			}
		}
		if (tryGetFromPreview && this.<TryGetResultingItemDef>g__IsCachedNullOrEmpty|60_0())
		{
			this.cachedResultingItemDef = GameBalance.Me.GetData<ItemDef>(this.GetOutputPreview(null).itemId);
		}
		return this.cachedResultingItemDef;
	}

	// Token: 0x06000C25 RID: 3109 RVA: 0x0003D8B4 File Offset: 0x0003BAB4
	public List<ItemDef> GetPossibleResultingItemDefs(bool tryGetFromPreview = true)
	{
		if (this.cachedPossibleResultingItemDefs != null && this.cachedPossibleResultingItemDefs.Count > 0)
		{
			return this.cachedPossibleResultingItemDefs;
		}
		List<ItemDef> list = new List<ItemDef>();
		CraftDef.CollectPossibleResultingItemDefsFromOutput(this.outputItems, list);
		if (list.Count == 0)
		{
			CraftDef.CollectPossibleResultingItemDefsFromNeeds(this.dropFromWgoItemsEnd, list);
		}
		if (list.Count == 0)
		{
			CraftDef.CollectPossibleResultingItemDefsFromNeeds(this.removeItemsFromWgo, list);
		}
		if (list.Count == 0)
		{
			ItemDef itemDef = this.TryGetResultingItemDef(tryGetFromPreview);
			if (itemDef != null && !string.IsNullOrEmpty(itemDef.id))
			{
				list.Add(itemDef);
			}
		}
		if (list.Count > 0)
		{
			this.cachedPossibleResultingItemDefs = list;
		}
		return list;
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x0003D950 File Offset: 0x0003BB50
	private static void CollectPossibleResultingItemDefsFromOutput(OutputItems items, List<ItemDef> result)
	{
		if (items == null)
		{
			return;
		}
		for (int i = 0; i < items.chanceOutputItems.Count; i++)
		{
			CraftDef.AddPossibleResultingItemDef(items.chanceOutputItems[i], result);
		}
		for (int j = 0; j < items.groupChanceOutputItems.Count; j++)
		{
			List<ChanceOutputItem> chanceItems = items.groupChanceOutputItems[j].chanceItems;
			for (int k = 0; k < chanceItems.Count; k++)
			{
				CraftDef.AddPossibleResultingItemDef(chanceItems[k], result);
			}
		}
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x0003D9D0 File Offset: 0x0003BBD0
	private static void AddPossibleResultingItemDef(ChanceOutputItem chanceOutputItem, List<ItemDef> result)
	{
		if (chanceOutputItem.isStarGroup)
		{
			List<ItemDef> list;
			if (GameBalance.Me.starGroupItemsCache.TryGetValue(chanceOutputItem.id, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					result.Add(list[i]);
				}
			}
			return;
		}
		ItemDef dataOrNull = GameBalance.Me.GetDataOrNull<ItemDef>(chanceOutputItem.id);
		if (dataOrNull != null)
		{
			result.Add(dataOrNull);
		}
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x0003DA38 File Offset: 0x0003BC38
	private static void CollectPossibleResultingItemDefsFromNeeds(List<NeedItemData> needItems, List<ItemDef> result)
	{
		if (needItems == null || needItems.Count == 0)
		{
			return;
		}
		NeedItemData needItemData = needItems[0];
		switch (needItemData.groupType)
		{
		case ItemGroup.None:
		{
			ItemDef dataOrNull = GameBalance.Me.GetDataOrNull<ItemDef>(needItemData.id);
			if (dataOrNull != null)
			{
				result.Add(dataOrNull);
				return;
			}
			break;
		}
		case ItemGroup.Common:
		{
			List<ItemDef> list;
			if (GameBalance.Me.groupItemsCache.TryGetValue(needItemData.id, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					result.Add(list[i]);
				}
				return;
			}
			break;
		}
		case ItemGroup.Star:
		{
			List<ItemDef> list2;
			if (GameBalance.Me.starGroupItemsCache.TryGetValue(needItemData.id, out list2))
			{
				for (int j = 0; j < list2.Count; j++)
				{
					result.Add(list2[j]);
				}
				return;
			}
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x0003DB18 File Offset: 0x0003BD18
	public string GetBoostRunesAsString()
	{
		Vector3Int boostRunesAsVector3Int = this.GetBoostRunesAsVector3Int();
		StringBuilder stringBuilder = new StringBuilder();
		int x = boostRunesAsVector3Int.x;
		int y = boostRunesAsVector3Int.y;
		int z = boostRunesAsVector3Int.z;
		if (x > 0)
		{
			stringBuilder.Append(string.Format("{0}{1}", "rune_r".FontIcon(), x));
		}
		if (y > 0)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(" ");
			}
			stringBuilder.Append(string.Format("{0}{1}", "rune_g".FontIcon(), y));
		}
		if (z > 0)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(" ");
			}
			stringBuilder.Append(string.Format("{0}{1}", "rune_b".FontIcon(), z));
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000C2A RID: 3114 RVA: 0x0003DBEC File Offset: 0x0003BDEC
	public Vector3Int GetBoostRunesAsVector3Int()
	{
		if (this.id.EndsWith("_boost"))
		{
			if (this.cachedBoostRunesAsVector3Int != default(Vector3Int))
			{
				return this.cachedBoostRunesAsVector3Int;
			}
			this.cachedBoostRunesAsVector3Int = new Vector3Int(this.addWgoParamsOnStart.GetInt("rune_r"), this.addWgoParamsOnStart.GetInt("rune_g"), this.addWgoParamsOnStart.GetInt("rune_b"));
		}
		return this.cachedBoostRunesAsVector3Int;
	}

	// Token: 0x06000C2B RID: 3115 RVA: 0x0003DC6C File Offset: 0x0003BE6C
	public int GetQualityForGardenProgressTick(int tick)
	{
		int num = -1;
		foreach (GameResPerProgress gameResPerProgress in this.gameresPerSuccessfulProgress)
		{
			if (gameResPerProgress.sucessfulProgressTick == tick)
			{
				if (gameResPerProgress.gameRes.GetInt("crop") > 0)
				{
					num = 0;
					break;
				}
				if (gameResPerProgress.gameRes.GetInt("crop_b") > 0)
				{
					num = 1;
					break;
				}
				if (gameResPerProgress.gameRes.GetInt("crop_s") > 0)
				{
					num = 2;
					break;
				}
				if (gameResPerProgress.gameRes.GetInt("crop_g") > 0)
				{
					num = 3;
					break;
				}
			}
		}
		return num;
	}

	// Token: 0x06000C2D RID: 3117 RVA: 0x0003DDBE File Offset: 0x0003BFBE
	[CompilerGenerated]
	private bool <TryGetResultingItemDef>g__IsCachedNullOrEmpty|60_0()
	{
		return this.cachedResultingItemDef == null || string.IsNullOrEmpty(this.cachedResultingItemDef.id);
	}

	// Token: 0x04000D69 RID: 3433
	[SerializeField]
	private string description;

	// Token: 0x04000D6A RID: 3434
	[AutoParse("skip_queue")]
	public bool skipQueue;

	// Token: 0x04000D6B RID: 3435
	[AutoParse("disable_add_to_queue")]
	public bool isAddToQueueDisabled;

	// Token: 0x04000D6C RID: 3436
	[AutoParse("bronze_level")]
	public int bronzeLevel;

	// Token: 0x04000D6D RID: 3437
	[AutoParse("silver_level")]
	public int silverLevel;

	// Token: 0x04000D6E RID: 3438
	[AutoParse("gold_level")]
	public int goldLevel;

	// Token: 0x04000D6F RID: 3439
	[AutoParse("is_needs_unlock")]
	public bool isNeedsUnlock;

	// Token: 0x04000D70 RID: 3440
	[AutoParse("is_one_time_craft")]
	public bool isOneTimeCraft;

	// Token: 0x04000D71 RID: 3441
	[AutoParse("is_forced_difficult_craft")]
	public bool isForcedDifficultCraft;

	// Token: 0x04000D72 RID: 3442
	[AutoParse("is_obj_destroy_craft")]
	public bool isObjDestroyCraft;

	// Token: 0x04000D73 RID: 3443
	[AutoParse("disable_multicraft")]
	public bool isMulticraftDisabled;

	// Token: 0x04000D74 RID: 3444
	[AutoParse("do_not_show_in_tooltips")]
	public bool doNotShowInTooltips;

	// Token: 0x04000D75 RID: 3445
	[AutoParse("is_conveyor_craft")]
	public bool isConveyorCraft;

	// Token: 0x04000D76 RID: 3446
	[AutoParse("tab_id")]
	public string tabId;

	// Token: 0x04000D77 RID: 3447
	[AutoParse("custom_action")]
	public ItemType customItemTypeAction;

	// Token: 0x04000D78 RID: 3448
	[AutoParse("transfer_destination_start")]
	public TransferDestination transferDestinationStart;

	// Token: 0x04000D79 RID: 3449
	[AutoParse("destination_item_start")]
	public string destinationItemStart;

	// Token: 0x04000D7A RID: 3450
	[AutoParse("transfer_destination_end")]
	public TransferDestination transferDestinationEnd;

	// Token: 0x04000D7B RID: 3451
	[AutoParse("destination_item_end")]
	public string destinationItemEnd;

	// Token: 0x04000D7C RID: 3452
	[AutoParse("transfer_needs_on_start")]
	public bool transferNeedsToDestinationOnStart;

	// Token: 0x04000D7D RID: 3453
	[AutoParse("transfer_needs_on_finish")]
	public bool transferNeedsToDestinationOnFinish;

	// Token: 0x04000D7E RID: 3454
	[AutoParse("use_dur_of_needs")]
	public float needItemsDurabilityUse;

	// Token: 0x04000D7F RID: 3455
	[AutoParse("use_dur_of_needs_idx")]
	public int needItemsDurabilityUseIndex = -1;

	// Token: 0x04000D80 RID: 3456
	public NeedItemData durabilityUseItem;

	// Token: 0x04000D81 RID: 3457
	public bool hasDurabilityUseItem;

	// Token: 0x04000D82 RID: 3458
	[AutoParse("drop_from_wgo_start")]
	public List<NeedItemData> dropFromWgoItemsStart = new List<NeedItemData>();

	// Token: 0x04000D83 RID: 3459
	[AutoParse("drop_from_wgo_end")]
	public List<NeedItemData> dropFromWgoItemsEnd = new List<NeedItemData>();

	// Token: 0x04000D84 RID: 3460
	[AutoParse("autopsy_type_craft")]
	public AutopsyTypeCraft autopsyTypeCraft;

	// Token: 0x04000D85 RID: 3461
	public string autopsyItemId;

	// Token: 0x04000D86 RID: 3462
	[AutoParse("remove_from_wgo")]
	public List<NeedItemData> removeItemsFromWgo = new List<NeedItemData>();

	// Token: 0x04000D87 RID: 3463
	[AutoParse("tech_r")]
	[LazyExpressionPureValueType(PureValueType.Float)]
	public LazyExpression techRed = new LazyExpression();

	// Token: 0x04000D88 RID: 3464
	[AutoParse("tech_g")]
	[LazyExpressionPureValueType(PureValueType.Float)]
	public LazyExpression techGreen = new LazyExpression();

	// Token: 0x04000D89 RID: 3465
	[AutoParse("tech_b")]
	[LazyExpressionPureValueType(PureValueType.Float)]
	public LazyExpression techBlue = new LazyExpression();

	// Token: 0x04000D8A RID: 3466
	[AutoParse("replace_to_wgo")]
	public string replaceWgoId;

	// Token: 0x04000D8B RID: 3467
	[AutoParse("transfer_data")]
	public bool transferDataOnReplace;

	// Token: 0x04000D8C RID: 3468
	[AutoParse("expression_on_replace")]
	public List<LazyExpression> executeOnReplace = new List<LazyExpression>();

	// Token: 0x04000D8D RID: 3469
	[AutoParse("fx_on_replace")]
	public string worldFxOnReplace;

	// Token: 0x04000D8E RID: 3470
	[AutoParse("end_script")]
	public string globalScriptOnCraftEnd;

	// Token: 0x04000D8F RID: 3471
	[AutoParse("custom_craft_result_icon")]
	[LazyExpressionPureValueType(PureValueType.String)]
	public LazyExpression customCraftResultIcon = new LazyExpression();

	// Token: 0x04000D90 RID: 3472
	public string iconId;

	// Token: 0x04000D91 RID: 3473
	[AutoParse("on_craft_add_queue_expressions")]
	public List<LazyExpression> onCraftAddQueueExpressions = new List<LazyExpression>();

	// Token: 0x04000D92 RID: 3474
	[AutoParse("on_craft_start_expressions")]
	public List<LazyExpression> onCraftStartExpressions = new List<LazyExpression>();

	// Token: 0x04000D93 RID: 3475
	[AutoParse("on_craft_end_expressions")]
	public List<LazyExpression> onCraftEndExpressions = new List<LazyExpression>();

	// Token: 0x04000D94 RID: 3476
	public List<GameResPerProgress> gameresPerSuccessfulProgress;

	// Token: 0x04000D95 RID: 3477
	[AutoParse("zombie_speed_item_modificator")]
	public GameRes zombieSpeedItemModificators = new GameRes();

	// Token: 0x04000D96 RID: 3478
	private ItemDef cachedResultingItemDef;

	// Token: 0x04000D97 RID: 3479
	private List<ItemDef> cachedPossibleResultingItemDefs;

	// Token: 0x04000D98 RID: 3480
	private Vector3Int cachedBoostRunesAsVector3Int;
}
