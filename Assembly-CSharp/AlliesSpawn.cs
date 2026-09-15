using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020002A5 RID: 677
public class AlliesSpawn : MonoBehaviour
{
	// Token: 0x170002CB RID: 715
	// (get) Token: 0x06001146 RID: 4422 RVA: 0x000570A5 File Offset: 0x000552A5
	public AgentsGroupFlagController FlagController
	{
		get
		{
			return this.flagController;
		}
	}

	// Token: 0x170002CC RID: 716
	// (get) Token: 0x06001147 RID: 4423 RVA: 0x000570AD File Offset: 0x000552AD
	public IReadOnlyList<WgoData> Fighters
	{
		get
		{
			return this.fighters;
		}
	}

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x06001148 RID: 4424 RVA: 0x000570B5 File Offset: 0x000552B5
	public int SquadSlotIndex
	{
		get
		{
			return this.squadSlotIndex;
		}
	}

	// Token: 0x06001149 RID: 4425 RVA: 0x000570C0 File Offset: 0x000552C0
	public void SpawnFromContainer(WgoData fighterContainer)
	{
		this.isMercenarySpawn = fighterContainer.id == "fighter_container_mercenary";
		this.squadSlotIndex = MainGame.Instance.GameSave.militaryBaseData.GetSquadSlotIndex(fighterContainer.UniqueId);
		WgoData wgoData;
		MainGame.Instance.GameSave.worldData.AddWgoData("flag_stand", this.flagStandPosition.position, MainGame.PlayerData.currentGameSceneId, "", out wgoData, false);
		LazySingleton<FightingGameController>.Instance.AddTemporaryWgoData(wgoData);
		this.flagStand = GameScene.GetWgoViewGlobal(wgoData.UniqueId);
		this.flagStand.UpdateFlag(ChunkingIgnoreType.Fighting, true);
		FlagStandComponent componentInChildren = this.flagStand.GetComponentInChildren<FlagStandComponent>();
		WgoData wgoData2;
		MainGame.Instance.GameSave.worldData.AddWgoData("test_flag", componentInChildren.FlagPlacementPoint.transform.position, MainGame.PlayerData.currentGameSceneId, "", out wgoData2, false);
		LazySingleton<FightingGameController>.Instance.AddTemporaryWgoData(wgoData2);
		wgoData2.ApplyWgoPartState(GameScene.GetWgoViewGlobal(SGuid.Parse(fighterContainer.GameResStr.Get("fighters_flag", ""))).Data.MainWgoPartData.variationId, -1);
		this.flag = GameScene.GetWgoViewGlobal(wgoData2.UniqueId);
		this.flag.UpdateFlag(ChunkingIgnoreType.Fighting, true);
		this.flagController = this.flag.GetComponentInChildren<AgentsGroupFlagController>();
		AgentsGroupBehaviourController componentInChildren2 = this.flag.GetComponentInChildren<AgentsGroupBehaviourController>();
		if (componentInChildren2 != null)
		{
			componentInChildren2.TargetTeam = LazyConsts.Fighting.TeamType.WildZombie;
		}
		this.flagController.Init();
		this.flagController.SetEnabled(true);
		List<DockPointData> dockPoints = fighterContainer.MainWgoPartData.GetDockPoints(DockPointData.Availability.OnlyOccupied, DockPointData.Filter.All);
		for (int i = 0; i < dockPoints.Count; i++)
		{
			if (this.isMercenarySpawn)
			{
				this.SpawnMercenary(dockPoints[i], this.alliesSpawnPositions[i]);
			}
			else
			{
				this.TrySpawnZombie(dockPoints[i], this.alliesSpawnPositions[i]);
			}
		}
		componentInChildren.AttachFlag(this.flag, false);
		this.flagStand.Data.GameResStr.Set("flag_stand_sguid", this.flag.Data.UniqueId.ToString());
	}

	// Token: 0x0600114A RID: 4426 RVA: 0x000572F0 File Offset: 0x000554F0
	public void Clear()
	{
		foreach (WgoData wgoData in this.fighters)
		{
			if (wgoData != null)
			{
				if (this.isMercenarySpawn)
				{
					MainGame.Instance.GameSave.militaryBaseData.ReturnFighterToContainer(wgoData);
				}
				else
				{
					MainGame.Instance.GameSave.worldData.RemoveWgoDataFromGameScene(wgoData, true);
				}
			}
		}
		this.fighters.Clear();
		if (this.flagController)
		{
			this.flagController.DeInit();
		}
		if (this.flag)
		{
			this.flag.UpdateFlag(ChunkingIgnoreType.Fighting, false);
			MainGame.WorldData.RemoveWgoDataFromGameScene(this.flag.Data, true);
		}
		if (this.flagStand)
		{
			this.flagStand.UpdateFlag(ChunkingIgnoreType.Fighting, false);
			MainGame.WorldData.RemoveWgoDataFromGameScene(this.flagStand.Data, true);
		}
		MainGame.Instance.GameSave.militaryBaseData.fighters.Clear();
		this.flagController = null;
		this.flag = null;
		this.flagStand = null;
		this.squadSlotIndex = -1;
	}

	// Token: 0x0600114B RID: 4427 RVA: 0x0005742C File Offset: 0x0005562C
	private void SpawnMercenary(DockPointData occupiedDockPoint, Transform spawPosition)
	{
		WgoData wgoData = MainGame.WorldData.GetWgoData(occupiedDockPoint.OccupiedBy);
		MainGame.Instance.GameSave.militaryBaseData.RemoveFighterFromContainer(wgoData);
		wgoData.WorldId = MainGame.PlayerData.currentGameSceneId;
		wgoData.Position = spawPosition.position;
		MainGame.Instance.GameSave.worldData.AddWgoData(wgoData);
		wgoData.PrepareForGame();
		Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(wgoData.UniqueId);
		wgoViewGlobal.UpdateFlag(ChunkingIgnoreType.Fighting, true);
		FightingAgent fightingAgent = this.flagController.AgentsController.AddWgoAsAgent(wgoViewGlobal, null, true);
		if (!fightingAgent)
		{
			Debug.LogError(string.Format("Failed to initialize fighting agent for mercenary [{0}]", wgoViewGlobal.Data.UniqueId));
			return;
		}
		this.AssignCommonFighterWeapon(fightingAgent, wgoViewGlobal.Data);
		fightingAgent.FlagController = this.flagController;
		this.fighters.Add(wgoViewGlobal.Data);
	}

	// Token: 0x0600114C RID: 4428 RVA: 0x0005750C File Offset: 0x0005570C
	private void TrySpawnZombie(DockPointData occupiedDockPoint, Transform spawPosition)
	{
		ZombieWgoData zombieWgoData = MainGame.WorldData.GetWgoData(occupiedDockPoint.OccupiedBy) as ZombieWgoData;
		if (!this.IsSpawnableZombie(zombieWgoData))
		{
			return;
		}
		ZombieWgoData zombieWgoData2 = zombieWgoData.CreateFighterFromThis(spawPosition.position, MainGame.PlayerData.currentGameSceneId);
		MainGame.Instance.GameSave.worldData.AddWgoData(zombieWgoData2);
		zombieWgoData2.PrepareForGame();
		Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(zombieWgoData2.UniqueId);
		wgoViewGlobal.UpdateFlag(ChunkingIgnoreType.Fighting, true);
		wgoViewGlobal.InitZombieFighter();
		FightingAgent fightingAgent = this.flagController.AgentsController.AddWgoAsAgent(wgoViewGlobal, null, true);
		if (!fightingAgent)
		{
			Debug.LogError(string.Format("Failed to initialize fighting agent for zombie [{0}]", wgoViewGlobal.Data.UniqueId));
			return;
		}
		this.AssignZombieWeapon(fightingAgent, zombieWgoData);
		fightingAgent.FlagController = this.flagController;
		this.fighters.Add(wgoViewGlobal.Data);
	}

	// Token: 0x0600114D RID: 4429 RVA: 0x000575E4 File Offset: 0x000557E4
	private bool IsSpawnableZombie(ZombieWgoData zombieData)
	{
		Inventory inventory = new Inventory(zombieData.ZombieItem);
		Item itemByGroupId = inventory.GetItemByGroupId("weapon");
		Item itemByType = inventory.GetItemByType(ItemType.BodyArmor);
		return !itemByGroupId.IsEmpty && !itemByType.IsEmpty;
	}

	// Token: 0x0600114E RID: 4430 RVA: 0x00057624 File Offset: 0x00055824
	private void AssignZombieWeapon(FightingAgent fightingAgent, ZombieWgoData zombieData)
	{
		Item itemByGroupId = new Inventory(zombieData.ZombieItem).GetItemByGroupId("weapon");
		ItemType type = itemByGroupId.Definition.type;
		if (type == ItemType.Bow || type == ItemType.Pike)
		{
			fightingAgent.AssignWeapon(itemByGroupId.Definition);
		}
	}

	// Token: 0x0600114F RID: 4431 RVA: 0x0005766C File Offset: 0x0005586C
	private void AssignCommonFighterWeapon(FightingAgent fightingAgent, WgoData fighter)
	{
		Item itemByGroupId = fighter.Inventory.GetItemByGroupId("weapon");
		ItemType type = itemByGroupId.Definition.type;
		if (type == ItemType.Bow || type == ItemType.Pike)
		{
			fightingAgent.AssignWeapon(itemByGroupId.Definition);
		}
	}

	// Token: 0x04001345 RID: 4933
	public Transform flagStandPosition;

	// Token: 0x04001346 RID: 4934
	public List<Transform> alliesSpawnPositions;

	// Token: 0x04001347 RID: 4935
	private Wgo flagStand;

	// Token: 0x04001348 RID: 4936
	private List<WgoData> fighters = new List<WgoData>();

	// Token: 0x04001349 RID: 4937
	private Wgo flag;

	// Token: 0x0400134A RID: 4938
	private AgentsGroupFlagController flagController;

	// Token: 0x0400134B RID: 4939
	private bool isMercenarySpawn;

	// Token: 0x0400134C RID: 4940
	private int squadSlotIndex = -1;
}
