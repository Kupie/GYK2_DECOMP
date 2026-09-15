using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200042B RID: 1067
public class ConveyorSystem : ICustomUpdatable
{
	// Token: 0x14000046 RID: 70
	// (add) Token: 0x06001C2A RID: 7210 RVA: 0x00083544 File Offset: 0x00081744
	// (remove) Token: 0x06001C2B RID: 7211 RVA: 0x0008357C File Offset: 0x0008177C
	public event Action OnUpdated;

	// Token: 0x170004E2 RID: 1250
	// (get) Token: 0x06001C2C RID: 7212 RVA: 0x000835B1 File Offset: 0x000817B1
	private static ConveyorSystemData Data
	{
		get
		{
			return MainGame.Instance.GameSave.conveyorSystemData;
		}
	}

	// Token: 0x170004E3 RID: 1251
	// (get) Token: 0x06001C2D RID: 7213 RVA: 0x000835C2 File Offset: 0x000817C2
	public WorldZoneData ConveyorWorldZone
	{
		get
		{
			return MainGame.Instance.GameSave.worldData.GetWorldZoneDataById("conveyor");
		}
	}

	// Token: 0x170004E4 RID: 1252
	// (get) Token: 0x06001C2E RID: 7214 RVA: 0x000835E0 File Offset: 0x000817E0
	public bool HasEnoughPower
	{
		get
		{
			WorldZoneData conveyorWorldZone = this.ConveyorWorldZone;
			return conveyorWorldZone == null || conveyorWorldZone.GetTotalQuality(WorldZoneWgoQualityType.ConveyorPowerSource) >= conveyorWorldZone.GetTotalQuality(WorldZoneWgoQualityType.ConveyorCells);
		}
	}

	// Token: 0x170004E5 RID: 1253
	// (get) Token: 0x06001C2F RID: 7215 RVA: 0x0008360C File Offset: 0x0008180C
	// (set) Token: 0x06001C30 RID: 7216 RVA: 0x00083618 File Offset: 0x00081818
	public bool IsPaused
	{
		get
		{
			return ConveyorSystem.Data.isPaused;
		}
		set
		{
			Debug.Log(string.Format("Set ConveyorSystem IsPaused to [{0}] ", value));
			ConveyorSystem.Data.isPaused = value;
			if (this.soundsArePlayingThisUpdate && value)
			{
				LazySingleton<ConveyorSoundSystem>.Instance.PauseSounds();
			}
		}
	}

	// Token: 0x170004E6 RID: 1254
	// (get) Token: 0x06001C31 RID: 7217 RVA: 0x0008364E File Offset: 0x0008184E
	// (set) Token: 0x06001C32 RID: 7218 RVA: 0x00083656 File Offset: 0x00081856
	public float UpdateInterval { get; private set; }

	// Token: 0x06001C33 RID: 7219 RVA: 0x0008365F File Offset: 0x0008185F
	public void Init()
	{
		this.UpdateInterval = ConstDef.Get("conveyor_system_update_interval").FloatValue;
		this.SubscribeToZonePowerChanges();
	}

	// Token: 0x06001C34 RID: 7220 RVA: 0x0008367C File Offset: 0x0008187C
	public void CustomUpdate(float deltaTime)
	{
		this.RefreshNoPowerIconsIfNeeded();
		if (this.IsPaused)
		{
			return;
		}
		ConveyorSystem.Data.timer += deltaTime;
		if (this.ConveyorWorldZone == null)
		{
			return;
		}
		for (int i = ConveyorSystem.Data.workbenchElements.Count - 1; i >= 0; i--)
		{
			if (ConveyorSystem.Data.workbenchElements[i].WgoData.CraftComponent.HasPreFinishUpdate)
			{
				ConveyorSystem.Data.workbenchElements[i].WgoData.CraftComponent.PreFinishUpdate(deltaTime);
			}
		}
		if (!this.HasEnoughPower)
		{
			ConveyorSystem.Data.timer = 0f;
			return;
		}
		for (int j = ConveyorSystem.Data.zombieCraftActivities.Count - 1; j >= 0; j--)
		{
			ConveyorSystem.Data.zombieCraftActivities[j].Update(deltaTime);
		}
		for (int k = ConveyorSystem.Data.workbenchElements.Count - 1; k >= 0; k--)
		{
			ConveyorComponent conveyorComponent = ConveyorSystem.Data.workbenchElements[k];
			if (conveyorComponent.WgoData.Definition.isAutoCrafter && conveyorComponent.WgoData.CraftComponent.HasCraftsInQueue && conveyorComponent.WgoData.CraftComponent.Status != CraftComponentStatus.WaitingForOutputDrop)
			{
				conveyorComponent.WgoData.CraftComponent.Update(deltaTime);
			}
		}
		if (ConveyorSystem.Data.timer >= this.UpdateInterval)
		{
			if (ConveyorSystem.Data.graphEndElements.Count == 0)
			{
				this.ReconstructConveyorsCache();
			}
			foreach (ConveyorComponent conveyorComponent2 in ConveyorSystem.Data.conveyorComponents)
			{
				conveyorComponent2.CurrentVisitState = VisitState.NotVisited;
				conveyorComponent2.wasPerformedItemTransfer = false;
				conveyorComponent2.ClearInAndOutItemDatas();
			}
			foreach (ConveyorWorkbenchComponent conveyorWorkbenchComponent in ConveyorSystem.Data.workbenchElements)
			{
				conveyorWorkbenchComponent.DoJobIn();
			}
			foreach (ConveyorComponent conveyorComponent3 in ConveyorSystem.Data.graphEndElements)
			{
				if (conveyorComponent3.CurrentVisitState == VisitState.NotVisited)
				{
					conveyorComponent3.DoJob(null);
				}
			}
			foreach (ConveyorWorkbenchComponent conveyorWorkbenchComponent2 in ConveyorSystem.Data.workbenchElements)
			{
				conveyorWorkbenchComponent2.DoJobOut();
			}
			foreach (ConveyorSplitterComponent conveyorSplitterComponent in ConveyorSystem.Data.splitterElements)
			{
				conveyorSplitterComponent.SwitchDirection();
			}
			ConveyorSystem.Data.timer = 0f;
			Action onUpdated = this.OnUpdated;
			if (onUpdated != null)
			{
				onUpdated();
			}
			LazySingleton<ConveyorSystemAnimationOrchestrator>.Instance.SetState("Out");
			this.soundsArePlayingThisUpdate = true;
			LazySingleton<ConveyorSoundSystem>.Instance.PauseSounds();
			LazySingleton<ConveyorSoundSystem>.Instance.PlaySounds();
		}
	}

	// Token: 0x06001C35 RID: 7221 RVA: 0x000839BC File Offset: 0x00081BBC
	public void AddConveyorObject(ConveyorComponent conveyorComponent)
	{
		if (ConveyorSystem.Data.conveyorComponents.Contains(conveyorComponent))
		{
			return;
		}
		ConveyorSystem.Data.conveyorComponents.Add(conveyorComponent);
		this.ReconstructConveyorsCache();
		this.RefreshNoPowerIconsIfNeeded();
	}

	// Token: 0x06001C36 RID: 7222 RVA: 0x000839ED File Offset: 0x00081BED
	public void RemoveConveyorObject(ConveyorComponent conveyorComponent)
	{
		ConveyorSystem.Data.conveyorComponents.Remove(conveyorComponent);
		this.ReconstructConveyorsCache();
		this.RefreshNoPowerIconsIfNeeded();
	}

	// Token: 0x06001C37 RID: 7223 RVA: 0x00083A0C File Offset: 0x00081C0C
	public void AddWorker(ZombieCraftActivity craftActivity)
	{
		if (this.TryGetCraftActivity(craftActivity.Zombie) != null)
		{
			return;
		}
		ConveyorSystem.Data.zombieCraftActivities.Add(craftActivity);
	}

	// Token: 0x06001C38 RID: 7224 RVA: 0x00083A30 File Offset: 0x00081C30
	public void RemoveWorker(ZombieCraftActivity craftActivity)
	{
		ZombieCraftActivity zombieCraftActivity = this.TryGetCraftActivity(craftActivity.Zombie);
		if (zombieCraftActivity == null)
		{
			return;
		}
		ConveyorSystem.Data.zombieCraftActivities.Remove(zombieCraftActivity);
	}

	// Token: 0x06001C39 RID: 7225 RVA: 0x00083A60 File Offset: 0x00081C60
	public ZombieCraftActivity TryGetCraftActivity(ZombieWgoData zombieWgoData)
	{
		foreach (ZombieCraftActivity zombieCraftActivity in ConveyorSystem.Data.zombieCraftActivities)
		{
			if (zombieCraftActivity.Zombie.UniqueId.Guid == zombieWgoData.UniqueId.Guid)
			{
				return zombieCraftActivity;
			}
		}
		return null;
	}

	// Token: 0x06001C3A RID: 7226 RVA: 0x00083ADC File Offset: 0x00081CDC
	public void Clear()
	{
		for (int i = ConveyorSystem.Data.conveyorComponents.Count - 1; i >= 0; i--)
		{
			ConveyorComponent conveyorComponent = ConveyorSystem.Data.conveyorComponents[i];
			if (conveyorComponent.WgoData.GetGameResInt("conveyor_build_is_not_removable") <= 0)
			{
				for (int j = conveyorComponent.WgoData.AttachedWorkbenchExtensions.Count - 1; j >= 0; j--)
				{
					MainGame.WorldData.RemoveWgoDataFromGameScene(conveyorComponent.WgoData.AttachedWorkbenchExtensions[j]);
				}
				MainGame.Instance.GameSave.worldData.RemoveWgoDataFromGameScene(conveyorComponent.WgoData, true);
				if (conveyorComponent.WgoData.Worker != null)
				{
					ZombieWgoData zombieWgoData = conveyorComponent.WgoData.Worker as ZombieWgoData;
					if (zombieWgoData == null)
					{
						goto IL_01DB;
					}
					MainGame.WorldData.RemoveWgoDataFromGameScene(zombieWgoData.UniqueId);
					MainGame.Instance.GameSave.zombieSystemData.zombieOnSceneWgoIds.Remove(zombieWgoData.UniqueId);
					MainGame.Instance.GameSave.zombieSystemData.Cache.Remove(zombieWgoData.UniqueId.Guid);
				}
				if (conveyorComponent is ConveyorPowerSourceComponent)
				{
					List<DockPointData> dockPoints = conveyorComponent.WgoData.MainWgoPartData.GetDockPoints(DockPointData.Availability.OnlyOccupied, DockPointData.Filter.OnlyZombie);
					if (dockPoints.Count > 0)
					{
						foreach (DockPointData dockPointData in dockPoints)
						{
							SGuid occupiedBy = dockPointData.OccupiedBy;
							ZombieWgoData zombie = MainGame.Instance.GameSave.zombieSystemData.GetZombie(occupiedBy);
							MainGame.WorldData.RemoveWgoDataFromGameScene(zombie.UniqueId);
							MainGame.Instance.GameSave.zombieSystemData.zombieOnSceneWgoIds.Remove(zombie.UniqueId);
							MainGame.Instance.GameSave.zombieSystemData.Cache.Remove(zombie.UniqueId.Guid);
						}
					}
				}
			}
			IL_01DB:;
		}
		ConveyorSystem.Data.conveyorComponents.Clear();
		ConveyorSystem.Data.workbenchElements.Clear();
		ConveyorSystem.Data.splitterElements.Clear();
		ConveyorSystem.Data.zombieCraftActivities.Clear();
		ConveyorSystem.Data.graphStartElements.Clear();
		ConveyorSystem.Data.graphEndElements.Clear();
		LazySingleton<ConveyorSoundSystem>.Instance.PauseSounds();
	}

	// Token: 0x06001C3B RID: 7227 RVA: 0x00083D44 File Offset: 0x00081F44
	private void UpdateStartElements()
	{
		ConveyorSystem.Data.graphStartElements.Clear();
		foreach (ConveyorComponent conveyorComponent in ConveyorSystem.Data.conveyorComponents)
		{
			if (conveyorComponent.ParentsData.Count == 0)
			{
				ConveyorSystem.Data.graphStartElements.Add(conveyorComponent);
			}
		}
	}

	// Token: 0x06001C3C RID: 7228 RVA: 0x00083DC0 File Offset: 0x00081FC0
	private void UpdateEndElements()
	{
		ConveyorSystem.Data.graphEndElements.Clear();
		foreach (ConveyorComponent conveyorComponent in ConveyorSystem.Data.conveyorComponents)
		{
			conveyorComponent.CurrentVisitState = VisitState.NotVisited;
			conveyorComponent.wasPerformedItemTransfer = false;
		}
		foreach (ConveyorComponent conveyorComponent2 in ConveyorSystem.Data.graphStartElements)
		{
			if (conveyorComponent2.CurrentVisitState == VisitState.NotVisited)
			{
				conveyorComponent2.GetEndElement(ref ConveyorSystem.Data.graphEndElements);
			}
		}
		ConveyorSystem.Data.graphEndElements.Reverse();
	}

	// Token: 0x06001C3D RID: 7229 RVA: 0x00083E94 File Offset: 0x00082094
	private void UpdateElements()
	{
		ConveyorSystem.Data.workbenchElements.Clear();
		ConveyorSystem.Data.splitterElements.Clear();
		foreach (ConveyorComponent conveyorComponent in ConveyorSystem.Data.conveyorComponents)
		{
			ConveyorWorkbenchComponent conveyorWorkbenchComponent = conveyorComponent as ConveyorWorkbenchComponent;
			if (conveyorWorkbenchComponent != null)
			{
				ConveyorSystem.Data.workbenchElements.Add(conveyorWorkbenchComponent);
			}
			else
			{
				ConveyorSplitterComponent conveyorSplitterComponent = conveyorComponent as ConveyorSplitterComponent;
				if (conveyorSplitterComponent != null)
				{
					ConveyorSystem.Data.splitterElements.Add(conveyorSplitterComponent);
				}
			}
		}
	}

	// Token: 0x06001C3E RID: 7230 RVA: 0x00083F38 File Offset: 0x00082138
	private void ReconstructConveyorsCache()
	{
		if (this.isReconstructLocked)
		{
			return;
		}
		this.UpdateStartElements();
		this.UpdateEndElements();
		this.UpdateElements();
	}

	// Token: 0x06001C3F RID: 7231 RVA: 0x00083F58 File Offset: 0x00082158
	private void SubscribeToZonePowerChanges()
	{
		if (this.zonePowerChangesSubscribed)
		{
			return;
		}
		WorldZoneData conveyorWorldZone = this.ConveyorWorldZone;
		if (conveyorWorldZone == null)
		{
			return;
		}
		conveyorWorldZone.OnWgoDataAdded += this.OnConveyorZoneMembershipChanged;
		conveyorWorldZone.OnWgoDataRemoved += this.OnConveyorZoneMembershipChanged;
		conveyorWorldZone.OnWgoDataChanged += this.OnConveyorZonePowerPossiblyChanged;
		conveyorWorldZone.OnWgoDataToCustomQualityAdded += this.OnConveyorZoneMembershipChanged;
		conveyorWorldZone.OnWgoDataFromCustomQualityRemoved += this.OnConveyorZoneMembershipChanged;
		this.zonePowerChangesSubscribed = true;
	}

	// Token: 0x06001C40 RID: 7232 RVA: 0x00083FDA File Offset: 0x000821DA
	private void OnConveyorZoneMembershipChanged(WgoData _)
	{
		this.RefreshNoPowerIconsIfNeeded();
	}

	// Token: 0x06001C41 RID: 7233 RVA: 0x00083FDA File Offset: 0x000821DA
	private void OnConveyorZonePowerPossiblyChanged()
	{
		this.RefreshNoPowerIconsIfNeeded();
	}

	// Token: 0x06001C42 RID: 7234 RVA: 0x00083FE4 File Offset: 0x000821E4
	private void RefreshNoPowerIconsIfNeeded()
	{
		this.SubscribeToZonePowerChanges();
		if (this.ConveyorWorldZone == null)
		{
			return;
		}
		bool hasEnoughPower = this.HasEnoughPower;
		bool? flag = this.lastHasEnoughPower;
		bool flag2 = hasEnoughPower;
		if ((flag.GetValueOrDefault() == flag2) & (flag != null))
		{
			return;
		}
		this.lastHasEnoughPower = new bool?(hasEnoughPower);
		ConveyorSystem.RedrawConveyorWidgets();
	}

	// Token: 0x06001C43 RID: 7235 RVA: 0x00084038 File Offset: 0x00082238
	private static void RedrawConveyorWidgets()
	{
		List<ConveyorComponent> conveyorComponents = ConveyorSystem.Data.conveyorComponents;
		for (int i = 0; i < conveyorComponents.Count; i++)
		{
			ConveyorComponent conveyorComponent = conveyorComponents[i];
			if (((conveyorComponent != null) ? conveyorComponent.WgoData : null) != null)
			{
				Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(conveyorComponent.WgoData.UniqueId);
				if (wgoViewGlobal != null)
				{
					wgoViewGlobal.DrawWidgets();
				}
			}
		}
	}

	// Token: 0x04001AA7 RID: 6823
	public bool isReconstructLocked;

	// Token: 0x04001AA8 RID: 6824
	private bool soundsArePlayingThisUpdate;

	// Token: 0x04001AA9 RID: 6825
	private bool? lastHasEnoughPower;

	// Token: 0x04001AAA RID: 6826
	private bool zonePowerChangesSubscribed;
}
