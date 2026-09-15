using System;
using System.Collections.Generic;

// Token: 0x02000445 RID: 1093
public static class ZombieDeliveryIndication
{
	// Token: 0x06001CC5 RID: 7365 RVA: 0x00086895 File Offset: 0x00084A95
	public static bool IsCaretakerStation(WgoData wgoData)
	{
		return ((wgoData != null) ? wgoData.Definition : null) != null && wgoData.Definition.interactionType == WGODef.InteractionType.Station;
	}

	// Token: 0x06001CC6 RID: 7366 RVA: 0x000868B6 File Offset: 0x00084AB6
	public static bool ShouldShowNoCaretakerAssigned(WgoData wgoData)
	{
		return ZombieDeliveryIndication.IsCaretakerStation(wgoData) && !wgoData.isTempObject && !wgoData.IsHidden && wgoData.Worker == null;
	}

	// Token: 0x06001CC7 RID: 7367 RVA: 0x000868DC File Offset: 0x00084ADC
	public static bool IsCraftStalledWithoutCaretaker(WgoData workbench)
	{
		if (((workbench != null) ? workbench.Definition : null) == null || workbench.isTempObject || workbench.IsHidden)
		{
			return false;
		}
		ZombieWgoData zombieWgoData = workbench.Worker as ZombieWgoData;
		if (zombieWgoData == null || zombieWgoData.ZombieType != ZombieType.Crafter)
		{
			return false;
		}
		CraftElementBase currentCraftElement = workbench.CraftComponent.CurrentCraftElement;
		if (currentCraftElement != null && !currentCraftElement.IsStarted)
		{
			CraftDef craftDef = currentCraftElement.Def as CraftDef;
			if (craftDef != null && !craftDef.isConveyorCraft)
			{
				if (!(zombieWgoData.CrafterCurrentOrder is DeliveryOrder))
				{
					return false;
				}
				WorldZoneData worldZoneData = workbench.WorldZoneData;
				return worldZoneData != null && !ZombieDeliveryIndication.HasCaretakerInZone(worldZoneData);
			}
		}
		return false;
	}

	// Token: 0x06001CC8 RID: 7368 RVA: 0x00086978 File Offset: 0x00084B78
	public static bool HasCaretakerInZone(WorldZoneData zone)
	{
		if (zone == null)
		{
			return false;
		}
		foreach (SGuid sguid in MainGame.ZombieSystemData.zombieOnSceneWgoIds)
		{
			ZombieWgoData zombie = MainGame.ZombieSystemData.GetZombie(sguid);
			if (zombie != null && zombie.ZombieType == ZombieType.Caretaker)
			{
				WgoData attachedWgoData = zombie.AttachedWgoData;
				WorldZoneData worldZoneData = ((attachedWgoData != null) ? attachedWgoData.WorldZoneData : null) ?? zombie.WorldZoneData;
				if (worldZoneData != null && worldZoneData.id == zone.id)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001CC9 RID: 7369 RVA: 0x00086A24 File Offset: 0x00084C24
	public static string GetStationIconId()
	{
		if (!string.IsNullOrEmpty(ZombieDeliveryIndication.cachedStationIconId))
		{
			return ZombieDeliveryIndication.cachedStationIconId;
		}
		GameBalance me = GameBalance.Me;
		WGODef wgodef = ((me != null) ? me.GetDataOrNull<WGODef>("zombie_supplier_station") : null);
		BuildingDef buildingDef;
		if (wgodef != null && wgodef.TryGetBuildingDefForWgo(out buildingDef))
		{
			ZombieDeliveryIndication.cachedStationIconId = buildingDef.BuildResultIcon;
		}
		else
		{
			ZombieDeliveryIndication.cachedStationIconId = "i_b_blueprint_placeholder";
		}
		return ZombieDeliveryIndication.cachedStationIconId;
	}

	// Token: 0x06001CCA RID: 7370 RVA: 0x00086A84 File Offset: 0x00084C84
	public static void RedrawCrafterWorkbenchesInZone(WorldZoneData zone)
	{
		if (((zone != null) ? zone.wgoDataList : null) == null)
		{
			return;
		}
		List<SGuid> wgoDataList = zone.wgoDataList;
		for (int i = 0; i < wgoDataList.Count; i++)
		{
			WgoData wgoData = MainGame.WorldData.GetWgoData(wgoDataList[i]);
			ZombieWgoData zombieWgoData = ((wgoData != null) ? wgoData.Worker : null) as ZombieWgoData;
			if (zombieWgoData != null && zombieWgoData.ZombieType == ZombieType.Crafter)
			{
				Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(wgoData.UniqueId);
				if (wgoViewGlobal != null)
				{
					wgoViewGlobal.DrawWidgets();
				}
			}
		}
	}

	// Token: 0x04001ACB RID: 6859
	private static string cachedStationIconId;
}
