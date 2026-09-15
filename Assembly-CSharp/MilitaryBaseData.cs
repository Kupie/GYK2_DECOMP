using System;
using System.Collections.Generic;
using LazyBearTechnology;
using Sirenix.Serialization;
using UnityEngine;

// Token: 0x0200046F RID: 1135
[Serializable]
public class MilitaryBaseData
{
	// Token: 0x17000510 RID: 1296
	// (get) Token: 0x06001DE5 RID: 7653 RVA: 0x0008C654 File Offset: 0x0008A854
	public bool IsMaxFighterContainers
	{
		get
		{
			return this.fighterContainers.Count >= 4;
		}
	}

	// Token: 0x17000511 RID: 1297
	// (get) Token: 0x06001DE6 RID: 7654 RVA: 0x0008C667 File Offset: 0x0008A867
	public SGuid FighterContainerMercenary
	{
		get
		{
			if (this.isMercenaryPayed)
			{
				return this.fighterContainerMercenary;
			}
			return null;
		}
	}

	// Token: 0x17000512 RID: 1298
	// (get) Token: 0x06001DE7 RID: 7655 RVA: 0x0008C679 File Offset: 0x0008A879
	// (set) Token: 0x06001DE8 RID: 7656 RVA: 0x0008C681 File Offset: 0x0008A881
	public bool IsMercenaryPayed
	{
		get
		{
			return this.isMercenaryPayed;
		}
		set
		{
			this.isMercenaryPayed = value;
			if (this.isMercenaryPayed)
			{
				WorldZoneData worldZoneDataById = MainGame.Instance.GameSave.WorldData.GetWorldZoneDataById("town_guard_barracks");
				if (worldZoneDataById == null)
				{
					return;
				}
				worldZoneDataById.NotifyWgoDataChanged();
			}
		}
	}

	// Token: 0x17000513 RID: 1299
	// (get) Token: 0x06001DE9 RID: 7657 RVA: 0x0008C6B5 File Offset: 0x0008A8B5
	// (set) Token: 0x06001DEA RID: 7658 RVA: 0x0008C6BD File Offset: 0x0008A8BD
	public string MercenariesPaymentId
	{
		get
		{
			return this.mercenariesPaymentId;
		}
		set
		{
			this.mercenariesPaymentId = value;
		}
	}

	// Token: 0x06001DEB RID: 7659 RVA: 0x0008C6C8 File Offset: 0x0008A8C8
	public void PrepareForGame()
	{
		if (this.fighterContainerMercenary != null && !this.fighterContainerMercenary.IsEmpty)
		{
			return;
		}
		WgoData wgoData = MainGame.Instance.GameSave.worldData.GetWgoData("fighter_container_mercenary");
		if (string.IsNullOrEmpty(this.mercenariesPaymentId))
		{
			this.mercenariesPaymentId = GameBalance.Me.mercenariesDefs[0].id;
		}
		if (wgoData == null)
		{
			Debug.LogError("Military Base Data: can not find mercenary fighter container");
			return;
		}
		for (int i = 1; i <= 4; i++)
		{
			string text = "npc_town_barracks_mercenary" + string.Format("_{0}", i);
			WgoData wgoData2 = MainGame.Instance.GameSave.worldData.GetWgoData(text);
			if (wgoData2 == null)
			{
				Debug.LogError("Military Base Data: can not find mercenary fighter with id " + text);
				return;
			}
			DockPointData nearestDockPointData = wgoData.GetNearestDockPointData(wgoData2.Position, DockPointData.Availability.OnlyNotOccupied);
			if (nearestDockPointData == null)
			{
				Debug.LogError(string.Concat(new string[] { "Military Base Data: no available dock point for mercenary fighter [", text, "] in container [", wgoData.id, "]" }));
			}
			else
			{
				nearestDockPointData.Occupy(wgoData2.UniqueId);
				wgoData2.takenDockPointsParentSGuid = wgoData.UniqueId;
			}
		}
		this.fighterContainerMercenary = wgoData.UniqueId;
	}

	// Token: 0x06001DEC RID: 7660 RVA: 0x0008C80C File Offset: 0x0008AA0C
	public void AddFightBuilding(WgoData building)
	{
		LazySingleton<FightingGameController>.Instance.BaseDefenseAgentsController.AddWgoAsAgent(GameScene.GetWgoViewGlobal(building.UniqueId), null, false);
		string text = building.id.Replace("_fight", "_pre");
		foreach (SGuid sguid in this.baseBuildings)
		{
			WgoData wgoData = MainGame.Instance.GameSave.worldData.GetWgoData(sguid);
			if (wgoData.id == text)
			{
				this.baseBuildings.Remove(sguid);
				List<MilitaryBaseData.MilitaryBaseFightBuilding> list = this.fightBuildings;
				Vector3 position = wgoData.Position;
				SGuid uniqueId = building.UniqueId;
				WgoPartData mainWgoPartData = wgoData.MainWgoPartData;
				string text2 = ((mainWgoPartData != null) ? mainWgoPartData.variationId : null);
				WgoPartData mainWgoPartData2 = wgoData.MainWgoPartData;
				list.Add(new MilitaryBaseData.MilitaryBaseFightBuilding(position, uniqueId, text2, (mainWgoPartData2 != null) ? mainWgoPartData2.rotationIndex : (-1)));
				MainGame.Instance.GameSave.worldData.RemoveWgoDataFromGameScene(sguid);
				return;
			}
		}
		this.fightBuildings.Add(new MilitaryBaseData.MilitaryBaseFightBuilding(building.Position, building.UniqueId, null, -1));
		if (this.fightingBuildingsSpawnedNotFromBase == null)
		{
			this.fightingBuildingsSpawnedNotFromBase = new HashSet<SGuid>();
		}
		this.fightingBuildingsSpawnedNotFromBase.Add(building.UniqueId);
	}

	// Token: 0x06001DED RID: 7661 RVA: 0x0008C95C File Offset: 0x0008AB5C
	public void RemoveFightBuilding(WgoData building)
	{
		LazySingleton<FightingGameController>.Instance.BaseDefenseAgentsController.RemoveAgent(building.UniqueId);
		if (this.fightingBuildingsSpawnedNotFromBase != null && this.fightingBuildingsSpawnedNotFromBase.Remove(building.UniqueId))
		{
			return;
		}
		MilitaryBaseData.MilitaryBaseFightBuilding militaryBaseFightBuilding = this.fightBuildings.Find((MilitaryBaseData.MilitaryBaseFightBuilding x) => x.uniqueId == building.UniqueId);
		if (militaryBaseFightBuilding == null)
		{
			Debug.LogError(string.Format("Military Base Data: can not find fight building [{0}] to return to base", building.UniqueId));
			return;
		}
		WgoData wgoData = new WgoData(building.id.Replace("_fight", "_pre"), militaryBaseFightBuilding.basePosition, "RuinedTemple");
		if (!string.IsNullOrEmpty(militaryBaseFightBuilding.variationId) || militaryBaseFightBuilding.rotationIndex != -1)
		{
			wgoData.MainWgoPartData.variationId = militaryBaseFightBuilding.variationId;
			wgoData.MainWgoPartData.rotationIndex = militaryBaseFightBuilding.rotationIndex;
		}
		MainGame.Instance.GameSave.worldData.AddWgoData(wgoData);
		this.baseBuildings.Add(wgoData.UniqueId);
	}

	// Token: 0x06001DEE RID: 7662 RVA: 0x0008CA70 File Offset: 0x0008AC70
	public List<Inventory> CreateBaseBuildingsInventory()
	{
		Inventory inventory = Inventory.Create(99, false, null, null, "");
		foreach (SGuid sguid in this.baseBuildings)
		{
			string id = MainGame.Instance.GameSave.worldData.GetWgoData(sguid).id;
			inventory.AddItemToInventory(new Item(id, 1), null, false);
		}
		return new List<Inventory> { inventory };
	}

	// Token: 0x06001DEF RID: 7663 RVA: 0x0008CB04 File Offset: 0x0008AD04
	public void RemoveFighterFromContainer(WgoData fighter)
	{
		WgoData wgoData = MainGame.WorldData.GetWgoData(fighter.takenDockPointsParentSGuid);
		int occupiedDockPointIndex = wgoData.MainWgoPartData.GetOccupiedDockPointIndex(fighter.UniqueId);
		DockPointData occupiedDockPointBy = wgoData.MainWgoPartData.GetOccupiedDockPointBy(fighter.UniqueId);
		MilitaryBaseData.MilitaryBaseFighter militaryBaseFighter = new MilitaryBaseData.MilitaryBaseFighter(wgoData.UniqueId, fighter.UniqueId, occupiedDockPointIndex, fighter.direction.Value);
		fighter.takenDockPointsParentSGuid = null;
		occupiedDockPointBy.UnOccupy();
		this.fighters.Add(militaryBaseFighter);
		MainGame.WorldData.RemoveWgoDataFromGameScene(fighter, true);
		fighter.IsInteractable = false;
	}

	// Token: 0x06001DF0 RID: 7664 RVA: 0x0008CB90 File Offset: 0x0008AD90
	public void ReturnFighterToContainer(WgoData fighter)
	{
		MainGame.WorldData.RemoveWgoDataFromGameScene(fighter, true);
		MilitaryBaseData.MilitaryBaseFighter militaryBaseFighter = this.fighters.Find((MilitaryBaseData.MilitaryBaseFighter x) => x.uniqueId == fighter.UniqueId);
		WgoData wgoData = MainGame.WorldData.GetWgoData(militaryBaseFighter.containerParent);
		DockPointData dockPointByIndex = wgoData.MainWgoPartData.GetDockPointByIndex(militaryBaseFighter.dockPointIndex);
		dockPointByIndex.Occupy(fighter.UniqueId);
		fighter.WorldId = "RuinedTemple";
		fighter.HpComponent.RestoreFullHp();
		fighter.Position = wgoData.GetDockPointDataWorldPosition(dockPointByIndex);
		fighter.takenDockPointsParentSGuid = militaryBaseFighter.containerParent;
		fighter.direction.Value = militaryBaseFighter.baseDirection;
		MainGame.WorldData.AddWgoData(fighter);
		fighter.PrepareForGame();
		this.fighters.Remove(militaryBaseFighter);
		fighter.IsInteractable = true;
	}

	// Token: 0x06001DF1 RID: 7665 RVA: 0x0008CC94 File Offset: 0x0008AE94
	public void AddBaseBuilding(WgoData building)
	{
		if (this.baseBuildings.Contains(building.UniqueId))
		{
			return;
		}
		this.baseBuildings.Add(building.UniqueId);
	}

	// Token: 0x06001DF2 RID: 7666 RVA: 0x0008CCBB File Offset: 0x0008AEBB
	public void RemoveBaseBuilding(WgoData building)
	{
		if (!this.baseBuildings.Contains(building.UniqueId))
		{
			return;
		}
		this.baseBuildings.Remove(building.UniqueId);
	}

	// Token: 0x06001DF3 RID: 7667 RVA: 0x0008CCE3 File Offset: 0x0008AEE3
	public void AddFighterContainer(SGuid fightingPlace)
	{
		if (this.fighterContainers.Contains(fightingPlace))
		{
			return;
		}
		this.fighterContainers.Add(fightingPlace);
	}

	// Token: 0x06001DF4 RID: 7668 RVA: 0x0008CD00 File Offset: 0x0008AF00
	public void UpgradeFighterContainers()
	{
		if (this.IsMaxFighterContainers)
		{
			return;
		}
		int num = this.fighterContainers.Count + 1;
		WgoData wgoData = MainGame.Instance.GameSave.worldData.GetWgoData("fighter_container" + string.Format("{0}_{1}", "_place", num));
		if (wgoData == null)
		{
			Debug.LogError("Military Base Data: can not find upgradable fighter container");
			return;
		}
		MainGame.WorldData.ChangeWgoData(wgoData, "fighter_container");
		this.AddFighterContainer(wgoData.UniqueId);
		MainGame.WorldData.GetWgoData(SGuid.Parse(wgoData.GameResStr.Get("fighters_flag", ""))).IsHidden = false;
	}

	// Token: 0x06001DF5 RID: 7669 RVA: 0x0008CDAC File Offset: 0x0008AFAC
	public void ReturnFightBuildingsToBase()
	{
		foreach (MilitaryBaseData.MilitaryBaseFightBuilding militaryBaseFightBuilding in this.fightBuildings)
		{
			WgoData wgoData = MainGame.Instance.GameSave.worldData.GetWgoData(militaryBaseFightBuilding.uniqueId);
			if (wgoData != null)
			{
				this.RemoveFightBuilding(wgoData);
				MainGame.Instance.GameSave.worldData.RemoveWgoDataFromGameScene(wgoData, true);
			}
		}
		this.fightBuildings.Clear();
	}

	// Token: 0x06001DF6 RID: 7670 RVA: 0x0008CE40 File Offset: 0x0008B040
	public int GetSquadSlotIndex(SGuid containerId)
	{
		if (SGuid.IsNullOrEmpty(containerId))
		{
			return -1;
		}
		if (containerId == this.fighterContainerMercenary)
		{
			return 0;
		}
		int num = this.fighterContainers.IndexOf(containerId);
		if (num < 0)
		{
			return -1;
		}
		return num + 1;
	}

	// Token: 0x06001DF7 RID: 7671 RVA: 0x0008CE80 File Offset: 0x0008B080
	public bool ContainsFighter(WgoData wgoData, bool checkMercenaries = false)
	{
		if (SGuid.IsNullOrEmpty(wgoData.takenDockPointsParentSGuid))
		{
			return false;
		}
		foreach (SGuid sguid in this.fighterContainers)
		{
			if (wgoData.takenDockPointsParentSGuid == sguid)
			{
				return true;
			}
		}
		return checkMercenaries && wgoData.takenDockPointsParentSGuid == this.fighterContainerMercenary;
	}

	// Token: 0x04001B77 RID: 7031
	private const string MILITARY_BASE_SCENE_ID = "RuinedTemple";

	// Token: 0x04001B78 RID: 7032
	private const string BASE_POSTFIX = "_pre";

	// Token: 0x04001B79 RID: 7033
	private const string FIGHT_POSTFIX = "_fight";

	// Token: 0x04001B7A RID: 7034
	private const string FIGHTER_CONTAINER_PLACE_POSTFIX = "_place";

	// Token: 0x04001B7B RID: 7035
	public List<SGuid> baseBuildings = new List<SGuid>();

	// Token: 0x04001B7C RID: 7036
	public List<MilitaryBaseData.MilitaryBaseFightBuilding> fightBuildings = new List<MilitaryBaseData.MilitaryBaseFightBuilding>();

	// Token: 0x04001B7D RID: 7037
	public List<SGuid> fighterContainers = new List<SGuid>();

	// Token: 0x04001B7E RID: 7038
	public List<SGuid> fighterContainersSelectedForFight = new List<SGuid>();

	// Token: 0x04001B7F RID: 7039
	public List<MilitaryBaseData.MilitaryBaseFighter> fighters = new List<MilitaryBaseData.MilitaryBaseFighter>();

	// Token: 0x04001B80 RID: 7040
	[SerializeField]
	private bool isMercenaryPayed;

	// Token: 0x04001B81 RID: 7041
	[SerializeField]
	private SGuid fighterContainerMercenary;

	// Token: 0x04001B82 RID: 7042
	[OdinSerialize]
	private HashSet<SGuid> fightingBuildingsSpawnedNotFromBase = new HashSet<SGuid>();

	// Token: 0x04001B83 RID: 7043
	[SerializeField]
	private string mercenariesPaymentId;

	// Token: 0x02000470 RID: 1136
	[Serializable]
	public class MilitaryBaseFightBuilding
	{
		// Token: 0x06001DF9 RID: 7673 RVA: 0x0008CF5D File Offset: 0x0008B15D
		public MilitaryBaseFightBuilding(Vector3 position, SGuid uniqueId, string variationId = null, int rotationIndex = -1)
		{
			this.basePosition = position;
			this.uniqueId = uniqueId;
			this.variationId = variationId;
			this.rotationIndex = rotationIndex;
		}

		// Token: 0x04001B84 RID: 7044
		public Vector3 basePosition;

		// Token: 0x04001B85 RID: 7045
		public SGuid uniqueId;

		// Token: 0x04001B86 RID: 7046
		public string variationId;

		// Token: 0x04001B87 RID: 7047
		public int rotationIndex = -1;
	}

	// Token: 0x02000471 RID: 1137
	[Serializable]
	public class MilitaryBaseFighter
	{
		// Token: 0x06001DFA RID: 7674 RVA: 0x0008CF89 File Offset: 0x0008B189
		public MilitaryBaseFighter(SGuid containerParent, SGuid uniqueId, int dockPointIndex, Vector2 baseDirection)
		{
			this.containerParent = containerParent;
			this.uniqueId = uniqueId;
			this.dockPointIndex = dockPointIndex;
			this.baseDirection = baseDirection;
		}

		// Token: 0x04001B88 RID: 7048
		public SGuid containerParent;

		// Token: 0x04001B89 RID: 7049
		public SGuid uniqueId;

		// Token: 0x04001B8A RID: 7050
		public int dockPointIndex;

		// Token: 0x04001B8B RID: 7051
		public Vector2 baseDirection;
	}
}
