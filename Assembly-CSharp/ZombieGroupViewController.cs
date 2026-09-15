using System;
using System.Collections.Generic;
using LazyBearTechnology;
using Pathfinding.RVO;
using UnityEngine;

// Token: 0x020002F2 RID: 754
[RequireComponent(typeof(RVOSimulator))]
public class ZombieGroupViewController : LazySingleton<ZombieGroupViewController>
{
	// Token: 0x17000369 RID: 873
	// (get) Token: 0x060013EE RID: 5102 RVA: 0x0005BF1E File Offset: 0x0005A11E
	private GameScene GameScene
	{
		get
		{
			return MainGame.PlayerController.CurrentGameScene;
		}
	}

	// Token: 0x1700036A RID: 874
	// (get) Token: 0x060013EF RID: 5103 RVA: 0x0005272A File Offset: 0x0005092A
	private FightingGameController FgController
	{
		get
		{
			return LazySingleton<FightingGameController>.Instance;
		}
	}

	// Token: 0x1700036B RID: 875
	// (get) Token: 0x060013F0 RID: 5104 RVA: 0x00061AAC File Offset: 0x0005FCAC
	private AgentsGroupBehaviourController AgentsController
	{
		get
		{
			return this.FgController.BaseDefenseAgentsController;
		}
	}

	// Token: 0x060013F1 RID: 5105 RVA: 0x00061ABC File Offset: 0x0005FCBC
	public void SpawnZombiesOnGrid(int amount, Vector3 gridRightDownPos)
	{
		string text = this.ResolveSpawnGameSceneId();
		Vector3 vector = this.ResolveGridOrigin(gridRightDownPos);
		int num = Mathf.CeilToInt(Mathf.Sqrt((float)amount));
		for (int i = 0; i < amount; i++)
		{
			int num2 = i / num;
			int num3 = -i % num;
			Vector3 vector2 = vector + new Vector3((float)num3 * 0.484f, 0f, (float)num2 * 0.484f);
			WgoData wgoData = new WgoData(this.zombieWgoId, vector2, text);
			Wgo wgo = this.GameScene.AddWgoData(wgoData, false);
			ZombieGroupViewController.ConfigureDevSpawnNavGraph(this.AgentsController.AddWgoAsAgent(wgo, null, true));
		}
	}

	// Token: 0x060013F2 RID: 5106 RVA: 0x00061B54 File Offset: 0x0005FD54
	private static void ConfigureDevSpawnNavGraph(FightingAgent agent)
	{
		if (agent == null)
		{
			return;
		}
		agent.SetGraphMask(new LazyConsts.Navigation.Graph[]
		{
			LazyConsts.Navigation.Graph.RuinedTemple,
			LazyConsts.Navigation.Graph.Fighting_Recast
		});
	}

	// Token: 0x060013F3 RID: 5107 RVA: 0x00061B76 File Offset: 0x0005FD76
	public Wgo SpawnZombie(WgoData wgoData)
	{
		return this.GameScene.AddWgoData(wgoData, false);
	}

	// Token: 0x060013F4 RID: 5108 RVA: 0x00061B85 File Offset: 0x0005FD85
	public void Init(Wgo zombie, List<PathfindingPenalty> penalties = null)
	{
		this.AgentsController.AddWgoAsAgent(zombie, penalties, true);
	}

	// Token: 0x060013F5 RID: 5109 RVA: 0x00061B98 File Offset: 0x0005FD98
	public void DespawnZombies(int amount)
	{
		if (amount <= 0)
		{
			return;
		}
		int agentsCount = this.AgentsController.AgentsCount;
		amount = Mathf.Min(amount, agentsCount);
		for (int i = agentsCount - 1; i >= agentsCount - amount; i--)
		{
			FightingAgent fightingAgent = this.AgentsController.Agents[i];
			if (!(fightingAgent == null))
			{
				Wgo wgo = fightingAgent.Wgo;
				SGuid uniqueId = wgo.Data.UniqueId;
				MainGame.Instance.GameSave.worldData.RemoveWgoDataFromGameScene(uniqueId);
				this.AgentsController.RemoveAgent(uniqueId);
				global::UnityEngine.Object.Destroy(wgo.gameObject);
			}
		}
	}

	// Token: 0x060013F6 RID: 5110 RVA: 0x00061C28 File Offset: 0x0005FE28
	public void DespawnZombie(Wgo zombie)
	{
		if (zombie == null)
		{
			return;
		}
		SGuid uniqueId = zombie.Data.UniqueId;
		MainGame.Instance.GameSave.worldData.RemoveWgoDataFromGameScene(uniqueId);
		FightingAgent component = zombie.GetComponent<FightingAgent>();
		this.AgentsController.RemoveAgent(component.Wgo.Data.UniqueId);
		global::UnityEngine.Object.Destroy(zombie.gameObject);
	}

	// Token: 0x060013F7 RID: 5111 RVA: 0x00061C90 File Offset: 0x0005FE90
	public void DespawnAllZombies()
	{
		foreach (FightingAgent fightingAgent in this.AgentsController.Agents)
		{
			WgoData wgoData = ((fightingAgent != null) ? fightingAgent.Wgo.Data : null);
			if (wgoData != null)
			{
				SGuid uniqueId = wgoData.UniqueId;
				MainGame.Instance.GameSave.worldData.RemoveWgoDataFromGameScene(uniqueId);
			}
		}
		this.AgentsController.RemoveAllAgents();
	}

	// Token: 0x060013F8 RID: 5112 RVA: 0x00061D18 File Offset: 0x0005FF18
	public void PlaceAllToStartOnGrid(Vector3 gridRightDownPos)
	{
		if (this.AgentsController.AgentsCount == 0)
		{
			return;
		}
		Vector3 vector = this.ResolveGridOrigin(gridRightDownPos);
		int num = Mathf.CeilToInt(Mathf.Sqrt((float)this.AgentsController.AgentsCount));
		for (int i = 0; i < this.AgentsController.Agents.Count; i++)
		{
			if (!(this.AgentsController.Agents[i] == null))
			{
				int num2 = i / num;
				int num3 = -i % num;
				Vector3 vector2 = vector + new Vector3((float)num3 * 0.484f, 0f, (float)num2 * 0.484f);
				this.AgentsController.Agents[i].SetPosition(vector2, true, true);
			}
		}
	}

	// Token: 0x060013F9 RID: 5113 RVA: 0x00061DCD File Offset: 0x0005FFCD
	private string ResolveSpawnGameSceneId()
	{
		if (!(MainGame.PlayerData.currentGameSceneId == "Dev_ZombieFigtersTest"))
		{
			return MainGame.PlayerData.currentGameSceneId;
		}
		return "Dev_ZombieFigtersTest";
	}

	// Token: 0x060013FA RID: 5114 RVA: 0x00061DF8 File Offset: 0x0005FFF8
	private Vector3 ResolveGridOrigin(Vector3 defaultGridRightDownPos)
	{
		if (MainGame.PlayerData.currentGameSceneId != "Dev_ZombieFigtersTest")
		{
			Vector2 vector = MainGame.PlayerData.Direction * 4f;
			return MainGame.PlayerData.position.Value + new Vector3(vector.x, 0f, vector.y);
		}
		return defaultGridRightDownPos;
	}

	// Token: 0x060013FB RID: 5115 RVA: 0x00061E5C File Offset: 0x0006005C
	private void Update()
	{
		if (MainGame.IsGamePaused)
		{
			return;
		}
		this.AgentsController.CustomUpdate(Time.deltaTime);
	}

	// Token: 0x0400150B RID: 5387
	private const float ZOMBIE_RADIUS = 0.22f;

	// Token: 0x0400150C RID: 5388
	private const string DEV_ZOMBIE_FIGHTERS_TEST_SCENE = "Dev_ZombieFigtersTest";

	// Token: 0x0400150D RID: 5389
	private const float PLAYER_SPAWN_OFFSET = 4f;

	// Token: 0x0400150E RID: 5390
	[Space]
	[SerializeField]
	private string zombieWgoId = "zmb_wild_01";
}
