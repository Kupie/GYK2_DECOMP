using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200055A RID: 1370
public static class CraftStatusIconHelper
{
	// Token: 0x06002326 RID: 8998 RVA: 0x000A472C File Offset: 0x000A292C
	public static Sprite GetCraftStatusIcon(CraftStatus craftStatus, ItemType itemType = ItemType.None, string talentId = "")
	{
		switch (craftStatus)
		{
		case CraftStatus.NotEnoughResources:
			return LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("craft_status_not_enough_items", null);
		case CraftStatus.DoesntHaveRequiredTool:
			return LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("icon_" + itemType.ToString().ToLower() + "_not_equipped", null);
		case CraftStatus.NotEnoughSpaceInWgo:
			return LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("craft_status_not_enough_space", null);
		case CraftStatus.NotEnoughMastery:
			return LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("icon_" + talentId + "_not_enough", null);
		case CraftStatus.NotEnoughSpaceInMultiInventory:
			return LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("craft_status_not_enough_space", null);
		case CraftStatus.NoExtension:
			return LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("craft_status_no_extension", null);
		}
		return LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("craft_status_not_enough_items", null);
	}

	// Token: 0x06002327 RID: 8999 RVA: 0x000A4825 File Offset: 0x000A2A25
	public static Sprite GetPlantingDigStatusIcon()
	{
		return LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("icon_shovel_equipped", null);
	}

	// Token: 0x04001F97 RID: 8087
	private const string NOT_ENOUGH_RESOURCES_ICON_NAME = "craft_status_not_enough_items";

	// Token: 0x04001F98 RID: 8088
	private const string NOT_ENOUGH_SPACE_IN_WGO_ICON_NAME = "craft_status_not_enough_space";

	// Token: 0x04001F99 RID: 8089
	private const string NO_EXTENSION_ICON_NAME = "craft_status_no_extension";

	// Token: 0x04001F9A RID: 8090
	private const string PLANTING_DIG_ICON_NAME = "icon_shovel_equipped";
}
