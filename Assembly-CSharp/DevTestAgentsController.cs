using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x020002D4 RID: 724
public class DevTestAgentsController : LazySingleton<DevTestAgentsController>
{
	// Token: 0x1700030D RID: 781
	// (get) Token: 0x06001298 RID: 4760 RVA: 0x0005BF1E File Offset: 0x0005A11E
	private GameScene GameScene
	{
		get
		{
			return MainGame.PlayerController.CurrentGameScene;
		}
	}

	// Token: 0x1700030E RID: 782
	// (get) Token: 0x06001299 RID: 4761 RVA: 0x0005BF2A File Offset: 0x0005A12A
	public IReadOnlyList<FightingAgent> TestAgents
	{
		get
		{
			return this.testAgents;
		}
	}

	// Token: 0x1700030F RID: 783
	// (get) Token: 0x0600129A RID: 4762 RVA: 0x0005BF32 File Offset: 0x0005A132
	public int AgentsCount
	{
		get
		{
			return this.testAgents.Count;
		}
	}

	// Token: 0x0600129B RID: 4763 RVA: 0x0005BF3F File Offset: 0x0005A13F
	protected override void Awake()
	{
		base.Awake();
		this.InitializeController();
		this.LoadDefaultAI();
	}

	// Token: 0x0600129C RID: 4764 RVA: 0x0005BF54 File Offset: 0x0005A154
	private void InitializeController()
	{
		if (this.agentsController == null)
		{
			GameObject gameObject = new GameObject("DevTestAgentsController_GroupController");
			gameObject.transform.SetParent(base.transform);
			this.agentsController = gameObject.AddComponent<AgentsGroupBehaviourController>();
			this.agentsController.TargetTeam = LazyConsts.Fighting.TeamType.Player;
		}
	}

	// Token: 0x0600129D RID: 4765 RVA: 0x0005BFA4 File Offset: 0x0005A1A4
	private void LoadDefaultAI()
	{
		if (this.aiLoaded || this.defaultTestAI != null)
		{
			return;
		}
		this.defaultTestAI = Addressables.LoadAssetAsync<DevTestPlayerAttackAI>("Dev Test Player Attack AI.asset").WaitForCompletion();
		this.aiLoaded = true;
		if (this.defaultTestAI == null)
		{
			Debug.LogWarning("[DevTestAgentsController] Failed to load default AI from: Dev Test Player Attack AI.asset");
		}
	}

	// Token: 0x0600129E RID: 4766 RVA: 0x0005C000 File Offset: 0x0005A200
	public FightingAgent SpawnTestAgent(string fighterDefId, Vector3 position, AgentAI ai = null)
	{
		if (string.IsNullOrEmpty(fighterDefId))
		{
			Debug.LogError("[DevTestAgentsController] Cannot spawn agent: fighterDefId is null or empty");
			return null;
		}
		string currentGameSceneId = MainGame.PlayerData.currentGameSceneId;
		WgoData wgoData = new WgoData(fighterDefId, position, currentGameSceneId);
		Wgo wgo = this.GameScene.AddWgoData(wgoData, false);
		if (wgo == null)
		{
			Debug.LogError("[DevTestAgentsController] Failed to spawn WGO with id: " + fighterDefId);
			return null;
		}
		FightingAgent fightingAgent = this.agentsController.AddWgoAsAgent(wgo, null, true);
		fightingAgent.SetGraphMask(LazyConsts.Navigation.Graph.RuinedTemple);
		AgentAI agentAI = ((ai != null) ? ai : this.defaultTestAI);
		if (agentAI != null)
		{
			fightingAgent.SetAgentAI(agentAI);
		}
		fightingAgent.SetPosition(position, true, true);
		this.testAgents.Add(fightingAgent);
		this.testWgos.Add(wgo);
		wgo.IsActiveCombatant = true;
		Debug.Log(string.Format("[DevTestAgentsController] Spawned test agent: {0} at {1}", fighterDefId, position));
		return fightingAgent;
	}

	// Token: 0x0600129F RID: 4767 RVA: 0x0005C0DC File Offset: 0x0005A2DC
	public void ClearAllTestAgents()
	{
		for (int i = this.testAgents.Count - 1; i >= 0; i--)
		{
			this.RemoveTestAgent(i);
		}
		this.testAgents.Clear();
		this.testWgos.Clear();
		Debug.Log("[DevTestAgentsController] Cleared all test agents");
	}

	// Token: 0x060012A0 RID: 4768 RVA: 0x0005C128 File Offset: 0x0005A328
	public void RemoveTestAgent(int index)
	{
		if (index < 0 || index >= this.testAgents.Count)
		{
			return;
		}
		FightingAgent fightingAgent = this.testAgents[index];
		Wgo wgo = this.testWgos[index];
		if (fightingAgent != null)
		{
			fightingAgent.ClearCommand();
			if (this.agentsController != null)
			{
				this.agentsController.RemoveAgent(wgo.Data.UniqueId);
			}
		}
		if (wgo != null)
		{
			SGuid uniqueId = wgo.Data.UniqueId;
			MainGame.Instance.GameSave.worldData.RemoveWgoDataFromGameScene(uniqueId);
			global::UnityEngine.Object.Destroy(wgo.gameObject);
		}
		this.testAgents.RemoveAt(index);
		this.testWgos.RemoveAt(index);
	}

	// Token: 0x060012A1 RID: 4769 RVA: 0x0005C1E4 File Offset: 0x0005A3E4
	public void RemoveTestAgent(FightingAgent agent)
	{
		int num = this.testAgents.IndexOf(agent);
		if (num >= 0)
		{
			this.RemoveTestAgent(num);
		}
	}

	// Token: 0x060012A2 RID: 4770 RVA: 0x0005C20C File Offset: 0x0005A40C
	private void Update()
	{
		if (this.testAgents.Count == 0 || this.agentsController == null)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		this.agentsController.CustomUpdate(deltaTime);
	}

	// Token: 0x0400142F RID: 5167
	private const string DEFAULT_AI_ADDRESSABLE_PATH = "Dev Test Player Attack AI.asset";

	// Token: 0x04001430 RID: 5168
	[SerializeField]
	private DevTestPlayerAttackAI defaultTestAI;

	// Token: 0x04001431 RID: 5169
	private AgentsGroupBehaviourController agentsController;

	// Token: 0x04001432 RID: 5170
	private List<FightingAgent> testAgents = new List<FightingAgent>();

	// Token: 0x04001433 RID: 5171
	private List<Wgo> testWgos = new List<Wgo>();

	// Token: 0x04001434 RID: 5172
	private bool aiLoaded;
}
