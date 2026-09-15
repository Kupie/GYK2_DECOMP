using System;
using System.Collections.Generic;

// Token: 0x02000155 RID: 341
public static class GardenTabletWorldIconLogic
{
	// Token: 0x06000813 RID: 2067 RVA: 0x00027B38 File Offset: 0x00025D38
	public static bool IsGardenTablet(string wgoId)
	{
		if (string.IsNullOrEmpty(wgoId))
		{
			return false;
		}
		if (GameBalance.Me != null)
		{
			if (GameBalance.Me.HasWgoIdByGroup("garden_tablets", wgoId))
			{
				return true;
			}
			if (GardenTabletWorldIconLogic.IsGardenTabletDef(GameBalance.Me.GetDataOrNull<WGODef>(wgoId)))
			{
				return true;
			}
		}
		return wgoId.StartsWith("garden_tablet_");
	}

	// Token: 0x06000814 RID: 2068 RVA: 0x00027B8F File Offset: 0x00025D8F
	public static bool IsGardenTablet(WgoData wgoData)
	{
		return wgoData != null && (GardenTabletWorldIconLogic.IsGardenTabletDef(wgoData.Definition) || GardenTabletWorldIconLogic.IsGardenTablet(wgoData.id));
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x00027BB0 File Offset: 0x00025DB0
	public static bool TryGetCropIdFromTabletWgoId(string tabletWgoId, out string cropId)
	{
		cropId = null;
		if (string.IsNullOrEmpty(tabletWgoId) || !tabletWgoId.StartsWith("garden_tablet_"))
		{
			return false;
		}
		cropId = tabletWgoId.Substring("garden_tablet_".Length);
		return !string.IsNullOrEmpty(cropId);
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x00027BE8 File Offset: 0x00025DE8
	public static bool TryGetCropIdFromPlotWgoId(string plotWgoId, out string cropId)
	{
		cropId = null;
		if (string.IsNullOrEmpty(plotWgoId) || GardenTabletWorldIconLogic.IsEmptyPlotId(plotWgoId))
		{
			return false;
		}
		string text = GardenTabletWorldIconLogic.StripStagePostfix(plotWgoId);
		if (text.StartsWith("garden_"))
		{
			cropId = text.Substring("garden_".Length);
		}
		else
		{
			if (!text.StartsWith("vineyard_"))
			{
				return false;
			}
			cropId = text.Substring("vineyard_".Length);
		}
		return !string.IsNullOrEmpty(cropId);
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x00027C5F File Offset: 0x00025E5F
	public static bool CropsMatch(string cropIdA, string cropIdB)
	{
		return !string.IsNullOrEmpty(cropIdA) && !string.IsNullOrEmpty(cropIdB) && GardenTabletWorldIconLogic.NormalizeCropId(cropIdA) == GardenTabletWorldIconLogic.NormalizeCropId(cropIdB);
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x00027C84 File Offset: 0x00025E84
	public static bool DoesTabletAffectPlot(string tabletWgoId, WgoData plot)
	{
		if (plot == null)
		{
			return false;
		}
		string id = plot.id;
		string attachedGardenTabletWgoId = GardenTabletWorldIconLogic.GetAttachedGardenTabletWgoId(plot);
		CraftComponent craftComponent = plot.CraftComponent;
		string text;
		if (craftComponent == null)
		{
			text = null;
		}
		else
		{
			CraftElementBase currentCraftElement = craftComponent.CurrentCraftElement;
			if (currentCraftElement == null)
			{
				text = null;
			}
			else
			{
				CraftDefBase def = currentCraftElement.Def;
				text = ((def != null) ? def.id : null);
			}
		}
		return GardenTabletWorldIconLogic.DoesTabletAffectPlot(tabletWgoId, id, attachedGardenTabletWgoId, text, GardenTabletWorldIconLogic.GetPlantOrderSeedItemId(plot));
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x00027CD8 File Offset: 0x00025ED8
	public static bool DoesTabletAffectPlot(string tabletWgoId, string plotWgoId, string attachedTabletWgoId = null, string currentGardenCraftId = null, string plantOrderSeedItemId = null)
	{
		string text;
		if (!GardenTabletWorldIconLogic.IsGardenTablet(tabletWgoId) || !GardenTabletWorldIconLogic.TryGetCropIdFromTabletWgoId(tabletWgoId, out text))
		{
			return false;
		}
		string text2;
		if (GardenTabletWorldIconLogic.TryGetCropIdFromPlotWgoId(plotWgoId, out text2))
		{
			return GardenTabletWorldIconLogic.CropsMatch(text, text2);
		}
		if (!GardenTabletWorldIconLogic.IsEmptyPlotId(plotWgoId))
		{
			return false;
		}
		string text3;
		if (GardenTabletWorldIconLogic.IsGardenTablet(attachedTabletWgoId) && GardenTabletWorldIconLogic.TryGetCropIdFromTabletWgoId(attachedTabletWgoId, out text3))
		{
			return GardenTabletWorldIconLogic.CropsMatch(text, text3);
		}
		string text4;
		if (GardenTabletWorldIconLogic.TryGetCropIdFromGardenCraftId(currentGardenCraftId, out text4))
		{
			return GardenTabletWorldIconLogic.CropsMatch(text, text4);
		}
		string text5;
		if (GardenTabletWorldIconLogic.TryGetCropIdFromSeedItemId(plantOrderSeedItemId, out text5))
		{
			return GardenTabletWorldIconLogic.CropsMatch(text, text5);
		}
		return GardenTabletWorldIconLogic.IsTabletCompatibleWithEmptyPlot(text, plotWgoId);
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x00027D5C File Offset: 0x00025F5C
	private static bool IsGardenTabletDef(WGODef def)
	{
		return def != null && def.wgoGroup == "garden_tablets";
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x00027D73 File Offset: 0x00025F73
	public static string NormalizeCropId(string cropId)
	{
		if (cropId == "flax")
		{
			return "linum";
		}
		if (cropId == "grape")
		{
			return "grapes";
		}
		return cropId;
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x00027D9C File Offset: 0x00025F9C
	private static bool IsEmptyPlotId(string plotWgoId)
	{
		if (string.IsNullOrEmpty(plotWgoId))
		{
			return false;
		}
		string text = GardenTabletWorldIconLogic.StripStagePostfix(plotWgoId);
		return text == "garden_empty" || text == "vineyard_empty" || text.StartsWith("garden_empty_") || text.StartsWith("vineyard_empty_");
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x00027DF0 File Offset: 0x00025FF0
	private static bool IsTabletCompatibleWithEmptyPlot(string tabletCrop, string emptyPlotWgoId)
	{
		bool flag = GardenTabletWorldIconLogic.IsVineyardTabletCrop(tabletCrop);
		string text = GardenTabletWorldIconLogic.StripStagePostfix(emptyPlotWgoId);
		if (text == "vineyard_empty" || text.StartsWith("vineyard_empty_"))
		{
			return flag;
		}
		return (text == "garden_empty" || text.StartsWith("garden_empty_")) && !flag;
	}

	// Token: 0x0600081E RID: 2078 RVA: 0x00027E48 File Offset: 0x00026048
	private static bool IsVineyardTabletCrop(string cropId)
	{
		string text = GardenTabletWorldIconLogic.NormalizeCropId(cropId);
		return text == "grapes" || text == "hop";
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x00027E78 File Offset: 0x00026078
	private static bool TryGetCropIdFromGardenCraftId(string craftId, out string cropId)
	{
		cropId = null;
		if (string.IsNullOrEmpty(craftId) || !craftId.StartsWith("garden_"))
		{
			return false;
		}
		string text;
		if (craftId.EndsWith("_planting"))
		{
			text = craftId.Substring(0, craftId.Length - "_planting".Length);
		}
		else
		{
			if (!craftId.EndsWith("_growing"))
			{
				return false;
			}
			text = craftId.Substring(0, craftId.Length - "_growing".Length);
		}
		return GardenTabletWorldIconLogic.TryGetCropIdFromPlotWgoId(text, out cropId);
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x00027EFC File Offset: 0x000260FC
	private static bool TryGetCropIdFromSeedItemId(string seedItemId, out string cropId)
	{
		cropId = null;
		if (string.IsNullOrEmpty(seedItemId))
		{
			return false;
		}
		string text = seedItemId.Split(':', StringSplitOptions.None)[0];
		if (!text.EndsWith("_seed"))
		{
			return false;
		}
		cropId = text.Substring(0, text.Length - "_seed".Length);
		return !string.IsNullOrEmpty(cropId);
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x00027F58 File Offset: 0x00026158
	private static string StripStagePostfix(string wgoId)
	{
		string text = wgoId;
		if (text.EndsWith("_place"))
		{
			text = text.Substring(0, text.Length - "_place".Length);
		}
		if (text.EndsWith("_ready"))
		{
			text = text.Substring(0, text.Length - "_ready".Length);
		}
		return text;
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x00027FB4 File Offset: 0x000261B4
	private static string GetAttachedGardenTabletWgoId(WgoData plot)
	{
		if (plot.AttachedWorkbenchExtensions != null)
		{
			MainGame instance = MainGame.Instance;
			bool flag;
			if (instance == null)
			{
				flag = null != null;
			}
			else
			{
				GameSave gameSave = instance.GameSave;
				flag = ((gameSave != null) ? gameSave.WorldData : null) != null;
			}
			if (flag)
			{
				foreach (SGuid sguid in plot.AttachedWorkbenchExtensions)
				{
					WgoData wgoData = MainGame.Instance.GameSave.WorldData.GetWgoData(sguid);
					if (GardenTabletWorldIconLogic.IsGardenTablet(wgoData))
					{
						return wgoData.id;
					}
				}
				return null;
			}
		}
		return null;
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x00028050 File Offset: 0x00026250
	private static string GetPlantOrderSeedItemId(WgoData plot)
	{
		WorldZoneData worldZoneData = plot.WorldZoneData;
		if (worldZoneData == null)
		{
			return null;
		}
		List<OrderBase> list = worldZoneData.FindOrdersByTarget(plot.UniqueId, typeof(PlantOrder));
		if (list == null || list.Count == 0)
		{
			return null;
		}
		Item item = list[0].Item;
		if (item == null || item.IsEmpty)
		{
			return null;
		}
		return item.id;
	}

	// Token: 0x04000A0C RID: 2572
	public const float PlotIconWorldYOffset = -0.5f;

	// Token: 0x04000A0D RID: 2573
	private const string TabletIdPrefix = "garden_tablet_";

	// Token: 0x04000A0E RID: 2574
	private const string ReadyPostfix = "_ready";

	// Token: 0x04000A0F RID: 2575
	private const string PlacePostfix = "_place";

	// Token: 0x04000A10 RID: 2576
	private const string PlantingCraftPostfix = "_planting";

	// Token: 0x04000A11 RID: 2577
	private const string GrowingCraftPostfix = "_growing";

	// Token: 0x04000A12 RID: 2578
	private const string SeedItemPostfix = "_seed";
}
