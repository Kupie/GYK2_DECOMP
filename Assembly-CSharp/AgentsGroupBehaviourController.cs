using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020002C4 RID: 708
public class AgentsGroupBehaviourController : MonoBehaviour, IWgoCustomComponent<WCCD_AgentsGroupBehaviourController>
{
	// Token: 0x170002FB RID: 763
	// (get) Token: 0x06001229 RID: 4649 RVA: 0x0005A45B File Offset: 0x0005865B
	public int AgentsCount
	{
		get
		{
			return this.agents.Count;
		}
	}

	// Token: 0x170002FC RID: 764
	// (get) Token: 0x0600122A RID: 4650 RVA: 0x0005A468 File Offset: 0x00058668
	public IReadOnlyList<FightingAgent> Agents
	{
		get
		{
			return this.agents;
		}
	}

	// Token: 0x170002FD RID: 765
	// (get) Token: 0x0600122B RID: 4651 RVA: 0x0005A470 File Offset: 0x00058670
	public IReadOnlyCollection<FightingAgent> AvailableAgents
	{
		get
		{
			return this.availableAgents.Values;
		}
	}

	// Token: 0x170002FE RID: 766
	// (get) Token: 0x0600122C RID: 4652 RVA: 0x0005A47D File Offset: 0x0005867D
	public IReadOnlyCollection<FightingAgent> BusyAgents
	{
		get
		{
			return this.busyAgents.Values;
		}
	}

	// Token: 0x170002FF RID: 767
	// (get) Token: 0x0600122D RID: 4653 RVA: 0x0005A48A File Offset: 0x0005868A
	// (set) Token: 0x0600122E RID: 4654 RVA: 0x0005A492 File Offset: 0x00058692
	public FightingLine FightingLine
	{
		get
		{
			return this.fightingLine;
		}
		set
		{
			this.fightingLine = value;
		}
	}

	// Token: 0x17000300 RID: 768
	// (get) Token: 0x0600122F RID: 4655 RVA: 0x0005A49C File Offset: 0x0005869C
	private IReadOnlyList<ICombatEntity> Targets
	{
		get
		{
			if (this.targetLineIdx != -1)
			{
				IEnumerable<ICombatEntity> targets = LazySingleton<FightingGameController>.Instance.TargetsDatabase.GetTargets(this.targetLineIdx, -1);
				if (targets != null && targets.Any<ICombatEntity>())
				{
					return targets.Where((ICombatEntity t) => t.TeamType == this.targetTeam).ToList<ICombatEntity>();
				}
			}
			return LazySingleton<FightingGameController>.Instance.TargetsDatabase.GetTargetsByTeam(this.targetTeam);
		}
	}

	// Token: 0x17000301 RID: 769
	// (get) Token: 0x06001230 RID: 4656 RVA: 0x0005A501 File Offset: 0x00058701
	// (set) Token: 0x06001231 RID: 4657 RVA: 0x0005A509 File Offset: 0x00058709
	public LazyConsts.Fighting.TeamType TargetTeam
	{
		get
		{
			return this.targetTeam;
		}
		set
		{
			this.targetTeam = value;
		}
	}

	// Token: 0x06001232 RID: 4658 RVA: 0x0005A512 File Offset: 0x00058712
	public void Init()
	{
		MainGame.PlayerController.CurrentGameScene.GameSceneData.OnWgoDataPreRemove += this.HandleZombieRemoved;
	}

	// Token: 0x06001233 RID: 4659 RVA: 0x0005A534 File Offset: 0x00058734
	public void DeInit()
	{
		GameScene gameScene;
		if (!MainGame.PlayerController.TryGetCurrentGameScene(out gameScene))
		{
			return;
		}
		if (gameScene.GameSceneData == null)
		{
			return;
		}
		gameScene.GameSceneData.OnWgoDataPreRemove -= this.HandleZombieRemoved;
	}

	// Token: 0x06001234 RID: 4660 RVA: 0x0005A570 File Offset: 0x00058770
	public void AddPathCalculation(PathCalculationData data)
	{
		if (data == null)
		{
			return;
		}
		int num;
		if (this.pathCalculationsByGuid.TryGetValue(data.agentGuid.Guid, out num) && num >= 0 && num < this.pathCalculationDataList.Count && this.pathCalculationDataList[num].agentGuid.Guid == data.agentGuid.Guid)
		{
			this.pathCalculationDataList[num] = data;
			return;
		}
		int num2 = this.pathCalculationDataList.FindIndex((PathCalculationData p) => p.agentGuid.Guid == data.agentGuid.Guid);
		if (num2 >= 0)
		{
			this.pathCalculationDataList[num2] = data;
			this.pathCalculationsByGuid[data.agentGuid.Guid] = num2;
			return;
		}
		this.pathCalculationDataList.Add(data);
		this.pathCalculationsByGuid[data.agentGuid.Guid] = this.pathCalculationDataList.Count - 1;
	}

	// Token: 0x06001235 RID: 4661 RVA: 0x0005A688 File Offset: 0x00058888
	public void RemovePathCalculation(Guid agentGuid)
	{
		int num = -1;
		int num2;
		if (this.pathCalculationsByGuid.TryGetValue(agentGuid, out num2) && num2 >= 0 && num2 < this.pathCalculationDataList.Count && this.pathCalculationDataList[num2].agentGuid.Guid == agentGuid)
		{
			num = num2;
		}
		if (num < 0)
		{
			num = this.pathCalculationDataList.FindIndex((PathCalculationData p) => p.agentGuid.Guid == agentGuid);
		}
		if (num < 0)
		{
			return;
		}
		this.pathCalculationDataList.RemoveAt(num);
		this.RebuildPathCalculationsByGuid();
	}

	// Token: 0x06001236 RID: 4662 RVA: 0x0005A724 File Offset: 0x00058924
	public FightingAgent AddWgoAsAgent(Wgo wgo, List<PathfindingPenalty> penalties = null, bool snapToNavmesh = true)
	{
		if (!wgo)
		{
			Debug.LogError("[AgentsGroupBehaviourController] Cannot add null Wgo as agent.");
			return null;
		}
		WgoPart wgoPart = wgo.MainWgoPart;
		if (!wgoPart || wgoPart.Wgo != wgo)
		{
			wgoPart = wgo.GetComponentInChildren<WgoPart>(true);
		}
		if (!wgoPart)
		{
			Debug.LogError(string.Format("[AgentsGroupBehaviourController] Main WgoPart is missing for [{0}] [{1}]", wgo.Id, wgo.Data.UniqueId));
			return null;
		}
		FightingAgent fightingAgent;
		if (!wgoPart.TryGetComponent<FightingAgent>(out fightingAgent))
		{
			fightingAgent = wgoPart.gameObject.AddComponent<FightingAgent>();
		}
		fightingAgent.Init(wgo, this);
		fightingAgent.SetPathPenalties(penalties);
		fightingAgent.SetPosition(wgo.Data.Position, true, snapToNavmesh);
		this.AddAgent(fightingAgent);
		return fightingAgent;
	}

	// Token: 0x06001237 RID: 4663 RVA: 0x0005A7D4 File Offset: 0x000589D4
	public void AddAgent(FightingAgent agent)
	{
		AgentsGroupBehaviourController parentController = agent.ParentController;
		if (parentController != null)
		{
			parentController.RemoveAgent(agent);
		}
		agent.ParentController = this;
		agent.OnCommandCompleted += this.HandleAgentCommandCompleted;
		this.agents.Add(agent);
		this.agentsByIds.Add(agent.Wgo.Data.UniqueId, agent);
		this.availableAgents.Add(agent.Wgo.Data.UniqueId, agent);
		AgentAI agentAI;
		if (this.agentAIByEntityTypeMask.TryGetAgentAI(agent.Wgo.EntityType, out agentAI))
		{
			agent.SetAgentAI(agentAI);
		}
	}

	// Token: 0x06001238 RID: 4664 RVA: 0x0005A874 File Offset: 0x00058A74
	public void RemoveAgent(int idx)
	{
		FightingAgent fightingAgent = this.agents[idx];
		fightingAgent.ParentController = null;
		SGuid uniqueId = fightingAgent.Wgo.Data.UniqueId;
		fightingAgent.OnCommandCompleted -= this.HandleAgentCommandCompleted;
		this.agents.RemoveAt(idx);
		this.agentsByIds.Remove(uniqueId);
		this.availableAgents.Remove(uniqueId);
		this.busyAgents.Remove(uniqueId);
		AgentAI agentAI;
		if (this.agentAIByEntityTypeMask.TryGetAgentAI(fightingAgent.Wgo.EntityType, out agentAI))
		{
			fightingAgent.SetAgentAI(null);
		}
	}

	// Token: 0x06001239 RID: 4665 RVA: 0x0005A90C File Offset: 0x00058B0C
	public void RemoveAgent(SGuid guid)
	{
		FightingAgent fightingAgent;
		if (!this.agentsByIds.TryGetValue(guid, out fightingAgent))
		{
			return;
		}
		this.agentsByIds.Remove(guid);
		this.availableAgents.Remove(guid);
		this.busyAgents.Remove(guid);
		if (fightingAgent)
		{
			this.agents.Remove(fightingAgent);
			fightingAgent.OnCommandCompleted -= this.HandleAgentCommandCompleted;
		}
	}

	// Token: 0x0600123A RID: 4666 RVA: 0x0005A978 File Offset: 0x00058B78
	public void RemoveAgent(FightingAgent agent)
	{
		if (!agent)
		{
			return;
		}
		bool flag = false;
		for (int i = this.pathCalculationDataList.Count - 1; i >= 0; i--)
		{
			PathCalculationData pathCalculationData = this.pathCalculationDataList[i];
			if (!(((pathCalculationData != null) ? pathCalculationData.richAI : null) != agent.RichAI))
			{
				this.pathCalculationDataList.RemoveAt(i);
				flag = true;
			}
		}
		if (flag)
		{
			this.RebuildPathCalculationsByGuid();
		}
		agent.OnCommandCompleted -= this.HandleAgentCommandCompleted;
		if (agent.ParentController == this)
		{
			agent.ParentController = null;
		}
		this.agents.RemoveAll((FightingAgent a) => a == agent);
		AgentsGroupBehaviourController.RemoveAgentMappingsFor(agent, this.agentsByIds);
		AgentsGroupBehaviourController.RemoveAgentMappingsFor(agent, this.availableAgents);
		AgentsGroupBehaviourController.RemoveAgentMappingsFor(agent, this.busyAgents);
	}

	// Token: 0x0600123B RID: 4667 RVA: 0x0005AA7C File Offset: 0x00058C7C
	public void RemoveAllAgents()
	{
		for (int i = 0; i < this.agents.Count; i++)
		{
			FightingAgent fightingAgent = this.agents[i];
			if (fightingAgent)
			{
				fightingAgent.OnCommandCompleted -= this.HandleAgentCommandCompleted;
				if (fightingAgent.ParentController == this)
				{
					fightingAgent.ParentController = null;
				}
			}
		}
		this.agents.Clear();
		this.agentsByIds.Clear();
		this.availableAgents.Clear();
		this.busyAgents.Clear();
		this.pathCalculationDataList.Clear();
		this.pathCalculationsByGuid.Clear();
	}

	// Token: 0x0600123C RID: 4668 RVA: 0x0005AB20 File Offset: 0x00058D20
	public void TrySetCommand(SGuid agentSGuid)
	{
		FightingAgent fightingAgent;
		if (this.availableAgents.TryGetValue(agentSGuid, out fightingAgent))
		{
			if (fightingAgent.IsExecutingCommand)
			{
				return;
			}
			this.OrderNewCommand(fightingAgent);
		}
	}

	// Token: 0x0600123D RID: 4669 RVA: 0x0005AB4D File Offset: 0x00058D4D
	public void CustomUpdate(float deltaTime)
	{
		this.UpdatePathCalculations();
		this.UpdateAgentsState(deltaTime);
		this.UpdateCommands();
	}

	// Token: 0x0600123E RID: 4670 RVA: 0x0005AB64 File Offset: 0x00058D64
	public void TryRetargetAgents()
	{
		using (List<FightingAgent>.Enumerator enumerator = this.agents.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				FightingAgent agent = enumerator.Current;
				ZombieAttackCommand zombieAttackCommand = agent.MobCommand as ZombieAttackCommand;
				if (zombieAttackCommand == null || !zombieAttackCommand.IsAttackAnimPlaying)
				{
					MobCommand newCommand = this.GetNewCommand(agent);
					if (agent.MobCommand != null && newCommand != null)
					{
						newCommand.Init(agent);
						if (!agent.MobCommand.IsTheSameCommand(newCommand))
						{
							agent.StopCommandExecution(false);
							this.pathCalculationDataList.ForEach(delegate(PathCalculationData p)
							{
								if (p.agentGuid == agent.Wgo.Data.UniqueId)
								{
									p.isOutDated = true;
								}
							});
							agent.SetCommand(newCommand);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600123F RID: 4671 RVA: 0x0005AC48 File Offset: 0x00058E48
	public void SetTargetLineIdx(int lineIdx)
	{
		this.targetLineIdx = lineIdx;
	}

	// Token: 0x06001240 RID: 4672 RVA: 0x0005AC54 File Offset: 0x00058E54
	private void UpdatePathCalculations()
	{
		if (this.pathCalculationDataList.Count == 0)
		{
			return;
		}
		int num = 0;
		bool flag = false;
		int num2 = 0;
		while (num2 < this.pathCalculationDataList.Count && num < 10)
		{
			PathCalculationData pathCalculationData = this.pathCalculationDataList[num2];
			if (pathCalculationData == null || pathCalculationData.isOutDated || pathCalculationData.richAI == null)
			{
				this.pathCalculationDataList.RemoveAt(num2);
				flag = true;
			}
			else if (pathCalculationData.richAI.pathPending)
			{
				num2++;
			}
			else
			{
				pathCalculationData.richAI.destination = pathCalculationData.Destination;
				pathCalculationData.richAI.SearchPath();
				this.pathCalculationDataList.RemoveAt(num2);
				flag = true;
				num++;
			}
		}
		if (flag)
		{
			this.RebuildPathCalculationsByGuid();
		}
	}

	// Token: 0x06001241 RID: 4673 RVA: 0x0005AD10 File Offset: 0x00058F10
	private void RebuildPathCalculationsByGuid()
	{
		this.pathCalculationsByGuid.Clear();
		for (int i = 0; i < this.pathCalculationDataList.Count; i++)
		{
			this.pathCalculationsByGuid[this.pathCalculationDataList[i].agentGuid.Guid] = i;
		}
	}

	// Token: 0x06001242 RID: 4674 RVA: 0x0005AD60 File Offset: 0x00058F60
	private void HandleZombieRemoved(WgoData wgoData)
	{
		this.RemoveAgent(wgoData.UniqueId);
	}

	// Token: 0x06001243 RID: 4675 RVA: 0x0005AD70 File Offset: 0x00058F70
	private void UpdateAgentsState(float deltaTime)
	{
		for (int i = this.agents.Count - 1; i >= 0; i--)
		{
			this.agents[i].CustomUpdate(deltaTime);
		}
	}

	// Token: 0x06001244 RID: 4676 RVA: 0x0005ADA8 File Offset: 0x00058FA8
	private void UpdateCommands()
	{
		List<FightingAgent> list = new List<FightingAgent>();
		foreach (FightingAgent fightingAgent in this.agents)
		{
			if (fightingAgent.Wgo.Data.HpComponent.Hp <= 0)
			{
				list.Add(fightingAgent);
			}
			else if (!fightingAgent.IsExecutingCommand)
			{
				this.OrderNewCommand(fightingAgent);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			this.RemoveAgent(list[i].Wgo.Data.UniqueId);
			list[i].ClearCommand();
			list[i].PlayDying();
			list.RemoveAt(i);
			i--;
		}
	}

	// Token: 0x06001245 RID: 4677 RVA: 0x0005AE7C File Offset: 0x0005907C
	private MobCommand GetNewCommand(FightingAgent agent)
	{
		if (!agent.AgentAI)
		{
			return null;
		}
		return agent.AgentAI.GetCommand(agent, () => this.Targets);
	}

	// Token: 0x06001246 RID: 4678 RVA: 0x0005AEA8 File Offset: 0x000590A8
	private void OrderNewCommand(FightingAgent agent)
	{
		if (agent.Wgo.Data.HpComponent.Hp <= 0)
		{
			agent.PlayDying();
			return;
		}
		MobCommand newCommand = this.GetNewCommand(agent);
		if (newCommand == null)
		{
			return;
		}
		agent.SetCommand(newCommand);
		this.busyAgents[agent.Wgo.Data.UniqueId] = agent;
	}

	// Token: 0x06001247 RID: 4679 RVA: 0x0005AF03 File Offset: 0x00059103
	private void HandleAgentCommandCompleted(FightingAgent agent, MobCommand command)
	{
		this.busyAgents.Remove(agent.Wgo.Data.UniqueId);
		this.OrderNewCommand(agent);
	}

	// Token: 0x06001248 RID: 4680 RVA: 0x0005AF28 File Offset: 0x00059128
	private static void RemoveAgentMappingsFor(FightingAgent agent, Dictionary<SGuid, FightingAgent> dictionary)
	{
		if (dictionary.Count == 0)
		{
			return;
		}
		List<SGuid> list = null;
		foreach (KeyValuePair<SGuid, FightingAgent> keyValuePair in dictionary)
		{
			if (!(keyValuePair.Value != agent))
			{
				if (list == null)
				{
					list = new List<SGuid>();
				}
				list.Add(keyValuePair.Key);
			}
		}
		if (list == null)
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			dictionary.Remove(list[i]);
		}
	}

	// Token: 0x06001249 RID: 4681 RVA: 0x0005AFC4 File Offset: 0x000591C4
	private void RetargetForThoseWhoAreGoingOnNewTargetAdded(ICombatEntity addedEntity)
	{
		foreach (FightingAgent fightingAgent in this.busyAgents.Values)
		{
			if (fightingAgent.MobCommand != null && fightingAgent.MobCommand.commandType == MobCommand.CommandType.GoTo && (addedEntity.CombatEntityPosition - fightingAgent.Wgo.CombatEntityPosition).magnitude < (fightingAgent.MobCommand.Position - fightingAgent.Wgo.CombatEntityPosition).magnitude)
			{
				fightingAgent.StopCommandExecution(false);
				this.OrderNewCommand(fightingAgent);
			}
		}
	}

	// Token: 0x0600124A RID: 4682 RVA: 0x0005B07C File Offset: 0x0005927C
	public WCCD_AgentsGroupBehaviourController OnSave()
	{
		return new WCCD_AgentsGroupBehaviourController
		{
			targetTeam = this.targetTeam
		};
	}

	// Token: 0x0600124B RID: 4683 RVA: 0x0005B08F File Offset: 0x0005928F
	public void OnLoad(WCCD_AgentsGroupBehaviourController data)
	{
		this.TargetTeam = data.targetTeam;
	}

	// Token: 0x0600124C RID: 4684 RVA: 0x0005B09D File Offset: 0x0005929D
	public void OnUnload()
	{
		this.TargetTeam = LazyConsts.Fighting.TeamType.Player;
	}

	// Token: 0x040013F5 RID: 5109
	private const int MAX_PATH_CALCULATIONS_PER_FRAME = 10;

	// Token: 0x040013F6 RID: 5110
	[SerializeField]
	[Space]
	private LazyConsts.Fighting.TeamType targetTeam;

	// Token: 0x040013F7 RID: 5111
	[SerializeField]
	private AgentAIEntityTypeMaskProvider agentAIByEntityTypeMask = new AgentAIEntityTypeMaskProvider();

	// Token: 0x040013F8 RID: 5112
	private List<PathCalculationData> pathCalculationDataList = new List<PathCalculationData>();

	// Token: 0x040013F9 RID: 5113
	private Dictionary<Guid, int> pathCalculationsByGuid = new Dictionary<Guid, int>();

	// Token: 0x040013FA RID: 5114
	private List<FightingAgent> agents = new List<FightingAgent>();

	// Token: 0x040013FB RID: 5115
	private Dictionary<SGuid, FightingAgent> agentsByIds = new Dictionary<SGuid, FightingAgent>();

	// Token: 0x040013FC RID: 5116
	private Dictionary<SGuid, FightingAgent> availableAgents = new Dictionary<SGuid, FightingAgent>();

	// Token: 0x040013FD RID: 5117
	private Dictionary<SGuid, FightingAgent> busyAgents = new Dictionary<SGuid, FightingAgent>();

	// Token: 0x040013FE RID: 5118
	[CanBeNull]
	[SerializeField]
	private FightingLine fightingLine;

	// Token: 0x040013FF RID: 5119
	private int targetLineIdx = -1;
}
