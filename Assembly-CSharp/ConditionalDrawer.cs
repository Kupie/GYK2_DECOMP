using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003D6 RID: 982
public class ConditionalDrawer : MonoBehaviour
{
	// Token: 0x1700047C RID: 1148
	// (get) Token: 0x06001A13 RID: 6675 RVA: 0x0007A4D9 File Offset: 0x000786D9
	public List<ConditionalDrawerRule> Rules
	{
		get
		{
			return this.rules;
		}
	}

	// Token: 0x1700047D RID: 1149
	// (get) Token: 0x06001A14 RID: 6676 RVA: 0x0007A4E1 File Offset: 0x000786E1
	private PlayerWorkComponent PlayerWorkComp
	{
		get
		{
			PlayerController playerController = MainGame.PlayerController;
			if (playerController == null)
			{
				return null;
			}
			return playerController.PlayerWorkComponent;
		}
	}

	// Token: 0x1700047E RID: 1150
	// (get) Token: 0x06001A15 RID: 6677 RVA: 0x0007A4F4 File Offset: 0x000786F4
	private ConveyorComponent ConveyorComponent
	{
		get
		{
			WgoPart wgoPart = this.wgoPart;
			object obj;
			if (wgoPart == null)
			{
				obj = null;
			}
			else
			{
				Wgo wgo = wgoPart.Wgo;
				obj = ((wgo != null) ? wgo.Data : null);
			}
			ConveyorWgoData conveyorWgoData = obj as ConveyorWgoData;
			if (conveyorWgoData == null)
			{
				return null;
			}
			return conveyorWgoData.ConveyorComponent;
		}
	}

	// Token: 0x06001A16 RID: 6678 RVA: 0x0007A530 File Offset: 0x00078730
	public void Init(WgoPart wgoPart)
	{
		this.wgoPart = wgoPart;
		this.buildController = BuildController.Instance;
		if (this.rules.Count == 0)
		{
			return;
		}
		this.context = new ConditionalDrawerContext(wgoPart, this.buildController);
		this.BuildRulesIndex();
		this.SubscribeToEvents();
		this.EvaluateAll();
	}

	// Token: 0x06001A17 RID: 6679 RVA: 0x0007A581 File Offset: 0x00078781
	public void DeInit()
	{
		this.UnsubscribeFromEvents();
		this.context = null;
	}

	// Token: 0x06001A18 RID: 6680 RVA: 0x0007A590 File Offset: 0x00078790
	public void UpdateVisualsByConditions()
	{
		this.EvaluateAll();
	}

	// Token: 0x06001A19 RID: 6681 RVA: 0x0007A598 File Offset: 0x00078798
	public void AddRule(ConditionalDrawerRule rule)
	{
		this.rules.Add(rule);
		if (this.context != null)
		{
			this.BuildRulesIndex();
		}
	}

	// Token: 0x06001A1A RID: 6682 RVA: 0x0007A5B4 File Offset: 0x000787B4
	public bool RemoveRule(ConditionalDrawerRule rule)
	{
		bool flag = this.rules.Remove(rule);
		if (flag && this.context != null)
		{
			this.BuildRulesIndex();
		}
		return flag;
	}

	// Token: 0x06001A1B RID: 6683 RVA: 0x0007A5D4 File Offset: 0x000787D4
	public void SubscribeToParentCraftStatusChanged(WgoData wgoData)
	{
		if (this.rulesByEvent.ContainsKey(ConditionalEventType.CraftStatusChanged))
		{
			foreach (SGuid sguid in wgoData.WorkbenchParents)
			{
				WgoData wgoData2 = MainGame.Instance.GameSave.WorldData.GetWgoData(sguid);
				if (wgoData2 != null)
				{
					wgoData2.CraftComponent.OnStatusChanged += this.OnParentCraftStatusChanged;
				}
			}
		}
	}

	// Token: 0x06001A1C RID: 6684 RVA: 0x0007A658 File Offset: 0x00078858
	private void BuildRulesIndex()
	{
		this.rulesByEvent = new Dictionary<ConditionalEventType, List<ConditionalDrawerRule>>();
		foreach (object obj in Enum.GetValues(typeof(ConditionalEventType)))
		{
			ConditionalEventType conditionalEventType = (ConditionalEventType)obj;
			if (conditionalEventType != ConditionalEventType.None && conditionalEventType != ConditionalEventType.All)
			{
				List<ConditionalDrawerRule> list = new List<ConditionalDrawerRule>();
				foreach (ConditionalDrawerRule conditionalDrawerRule in this.rules)
				{
					if (conditionalDrawerRule.condition != null && conditionalDrawerRule.EventType.HasFlag(conditionalEventType))
					{
						list.Add(conditionalDrawerRule);
					}
				}
				if (list.Count > 0)
				{
					this.rulesByEvent[conditionalEventType] = list;
				}
			}
		}
	}

	// Token: 0x06001A1D RID: 6685 RVA: 0x0007A750 File Offset: 0x00078950
	private void EvaluateAll()
	{
		if (this.context == null)
		{
			return;
		}
		this.context.UpdateCachedValues(false);
		foreach (ConditionalDrawerRule conditionalDrawerRule in this.rules)
		{
			conditionalDrawerRule.Evaluate(this.context);
		}
	}

	// Token: 0x06001A1E RID: 6686 RVA: 0x0007A7BC File Offset: 0x000789BC
	private void EvaluateByEvent(ConditionalEventType eventType)
	{
		if (this.context == null)
		{
			return;
		}
		this.context.UpdateCachedValues(true);
		List<ConditionalDrawerRule> list;
		if (this.rulesByEvent.TryGetValue(eventType, out list))
		{
			foreach (ConditionalDrawerRule conditionalDrawerRule in list)
			{
				conditionalDrawerRule.Evaluate(this.context);
			}
		}
	}

	// Token: 0x06001A1F RID: 6687 RVA: 0x0007A834 File Offset: 0x00078A34
	private void SubscribeToEvents()
	{
		WgoPart wgoPart = this.wgoPart;
		bool flag;
		if (wgoPart == null)
		{
			flag = null != null;
		}
		else
		{
			Wgo wgo = wgoPart.Wgo;
			flag = ((wgo != null) ? wgo.Data : null) != null;
		}
		if (!flag)
		{
			return;
		}
		WgoData data = this.wgoPart.Wgo.Data;
		Inventory inventory = data.Inventory;
		if (this.rulesByEvent.ContainsKey(ConditionalEventType.GameResChanged))
		{
			data.OnGameResChanged += this.OnGameResChanged;
		}
		if (this.rulesByEvent.ContainsKey(ConditionalEventType.ItemsChanged))
		{
			inventory.OnItemsAdd += this.OnItemsChanged;
			inventory.OnItemsRemove += this.OnItemsChanged;
		}
		if (this.rulesByEvent.ContainsKey(ConditionalEventType.CraftProgressChanged))
		{
			data.CraftComponent.OnCraftCurProgressNormalizedChanged += this.OnCraftProgressChanged;
		}
		if (this.rulesByEvent.ContainsKey(ConditionalEventType.CraftStatusChanged))
		{
			data.CraftComponent.OnStatusChanged += this.OnCraftStatusChanged;
		}
		if (this.rulesByEvent.ContainsKey(ConditionalEventType.WorkStateChanged))
		{
			PlayerWorkComponent playerWorkComp = this.PlayerWorkComp;
			playerWorkComp.OnInteractionUpdate = (Action)Delegate.Combine(playerWorkComp.OnInteractionUpdate, new Action(this.OnWorkStateChanged));
			data.OnWorkerChanged += this.OnWorkStateChanged;
			data.OnToolTickApply += this.OnWorkStateChanged;
			data.CraftComponent.OnStatusChanged += this.OnWorkStateChanged;
		}
		if (this.rulesByEvent.ContainsKey(ConditionalEventType.ConveyorChanged) && this.ConveyorComponent != null)
		{
			this.ConveyorComponent.OnConnected += this.OnConveyorChanged;
			this.ConveyorComponent.OnDisconnected += this.OnConveyorChanged;
		}
		if (this.rulesByEvent.ContainsKey(ConditionalEventType.ConveyorSystemChanged))
		{
			MainGame.Instance.conveyorSystem.ConveyorWorldZone.OnWgoDataAdded += this.OnConveyorSystemChanged;
			MainGame.Instance.conveyorSystem.ConveyorWorldZone.OnWgoDataRemoved += this.OnConveyorSystemChanged;
		}
		FightingAgent fightingAgent;
		if (this.rulesByEvent.ContainsKey(ConditionalEventType.FightingAgentChanged) && this.wgoPart.TryGetComponent<FightingAgent>(out fightingAgent))
		{
			fightingAgent.OnInitialized += this.OnFightingAgentInitialized;
		}
		if (this.rulesByEvent.ContainsKey(ConditionalEventType.BuildingModeChanged) && this.buildController != null)
		{
			this.buildController.OnBuildModeStateChanged += this.OnBuildModeStateChanged;
		}
		if (this.rulesByEvent.ContainsKey(ConditionalEventType.InteractableStateChanged) && data != null)
		{
			data.OnInteractableStateChanged += this.OnInteractableStateChanged;
		}
		if (this.rulesByEvent.ContainsKey(ConditionalEventType.ParentWorkCondition) && data != null)
		{
			foreach (SGuid sguid in data.WorkbenchParents)
			{
				WgoData wgoData = MainGame.Instance.GameSave.WorldData.GetWgoData(sguid);
				if (wgoData != null)
				{
					wgoData.CraftComponent.OnStatusChanged += this.OnParentWorkStatusChanged;
					wgoData.OnToolTickApply += this.OnParentWorkStatusChanged;
				}
			}
		}
		ZombieWgoData zombieWgoData = data as ZombieWgoData;
		if (zombieWgoData != null)
		{
			zombieWgoData.OnCaretakerStateChanged += this.OnZombieCaretakerStateChanged;
		}
		foreach (DockPointData dockPointData in data.MainWgoPartData.DockPointDataList)
		{
			dockPointData.OnOccupiedStatusChanged += this.OnOccupiedDockPointStatusChanged;
		}
		data.OnTakenDockPointChanged += this.OnTakenDockPointChanged;
		if (data.HpComponent != null)
		{
			data.HpComponent.OnHpChanged += this.OnHpChanged;
		}
	}

	// Token: 0x06001A20 RID: 6688 RVA: 0x0007ABE8 File Offset: 0x00078DE8
	private void UnsubscribeFromEvents()
	{
		WgoPart wgoPart = this.wgoPart;
		bool flag;
		if (wgoPart == null)
		{
			flag = null != null;
		}
		else
		{
			Wgo wgo = wgoPart.Wgo;
			flag = ((wgo != null) ? wgo.Data : null) != null;
		}
		if (!flag)
		{
			return;
		}
		WgoData data = this.wgoPart.Wgo.Data;
		Inventory inventory = data.Inventory;
		data.OnGameResChanged -= this.OnGameResChanged;
		inventory.OnItemsAdd -= this.OnItemsChanged;
		inventory.OnItemsRemove -= this.OnItemsChanged;
		data.CraftComponent.OnCraftCurProgressNormalizedChanged -= this.OnCraftProgressChanged;
		data.CraftComponent.OnStatusChanged -= this.OnCraftStatusChanged;
		PlayerWorkComponent playerWorkComp = this.PlayerWorkComp;
		playerWorkComp.OnInteractionUpdate = (Action)Delegate.Remove(playerWorkComp.OnInteractionUpdate, new Action(this.OnWorkStateChanged));
		data.OnWorkerChanged -= this.OnWorkStateChanged;
		data.OnToolTickApply -= this.OnWorkStateChanged;
		data.CraftComponent.OnStatusChanged -= this.OnWorkStateChanged;
		foreach (SGuid sguid in data.WorkbenchParents)
		{
			WgoData wgoData = MainGame.Instance.GameSave.WorldData.GetWgoData(sguid);
			if (wgoData != null)
			{
				wgoData.CraftComponent.OnStatusChanged -= this.OnParentCraftStatusChanged;
			}
		}
		foreach (SGuid sguid2 in data.WorkbenchParents)
		{
			WgoData wgoData2 = MainGame.Instance.GameSave.WorldData.GetWgoData(sguid2);
			if (wgoData2 != null)
			{
				data.CraftComponent.OnStatusChanged -= this.OnParentWorkStatusChanged;
				wgoData2.OnToolTickApply -= this.OnParentWorkStatusChanged;
			}
		}
		if (this.ConveyorComponent != null)
		{
			this.ConveyorComponent.OnConnected -= this.OnConveyorChanged;
			this.ConveyorComponent.OnDisconnected -= this.OnConveyorChanged;
		}
		MainGame.Instance.conveyorSystem.ConveyorWorldZone.OnWgoDataAdded -= this.OnConveyorSystemChanged;
		MainGame.Instance.conveyorSystem.ConveyorWorldZone.OnWgoDataRemoved -= this.OnConveyorSystemChanged;
		FightingAgent fightingAgent;
		if (this.wgoPart.TryGetComponent<FightingAgent>(out fightingAgent))
		{
			fightingAgent.OnInitialized -= this.OnFightingAgentInitialized;
		}
		if (this.buildController != null)
		{
			this.buildController.OnBuildModeStateChanged -= this.OnBuildModeStateChanged;
		}
		if (data != null)
		{
			data.OnInteractableStateChanged -= this.OnInteractableStateChanged;
		}
		ZombieWgoData zombieWgoData = data as ZombieWgoData;
		if (zombieWgoData != null)
		{
			zombieWgoData.OnCaretakerStateChanged -= this.OnZombieCaretakerStateChanged;
		}
		foreach (DockPointData dockPointData in data.MainWgoPartData.DockPointDataList)
		{
			dockPointData.OnOccupiedStatusChanged -= this.OnOccupiedDockPointStatusChanged;
		}
		data.OnTakenDockPointChanged -= this.OnTakenDockPointChanged;
		if (data.HpComponent != null)
		{
			data.HpComponent.OnHpChanged -= this.OnHpChanged;
		}
	}

	// Token: 0x06001A21 RID: 6689 RVA: 0x0007AF58 File Offset: 0x00079158
	private void OnGameResChanged(string id)
	{
		this.EvaluateByEvent(ConditionalEventType.GameResChanged);
	}

	// Token: 0x06001A22 RID: 6690 RVA: 0x0007AF61 File Offset: 0x00079161
	private void OnItemsChanged(List<Item> items)
	{
		this.EvaluateByEvent(ConditionalEventType.ItemsChanged);
	}

	// Token: 0x06001A23 RID: 6691 RVA: 0x0007AF6A File Offset: 0x0007916A
	private void OnCraftProgressChanged(float progress)
	{
		this.EvaluateByEvent(ConditionalEventType.CraftProgressChanged);
	}

	// Token: 0x06001A24 RID: 6692 RVA: 0x0007AF73 File Offset: 0x00079173
	private void OnCraftStatusChanged(CraftComponentStatus status)
	{
		this.EvaluateByEvent(ConditionalEventType.CraftStatusChanged);
	}

	// Token: 0x06001A25 RID: 6693 RVA: 0x0007AF73 File Offset: 0x00079173
	private void OnParentCraftStatusChanged(CraftComponentStatus status)
	{
		this.EvaluateByEvent(ConditionalEventType.CraftStatusChanged);
	}

	// Token: 0x06001A26 RID: 6694 RVA: 0x0007AF7C File Offset: 0x0007917C
	private void OnParentWorkStatusChanged(CraftComponentStatus status)
	{
		this.EvaluateByEvent(ConditionalEventType.ParentWorkCondition);
	}

	// Token: 0x06001A27 RID: 6695 RVA: 0x0007AF7C File Offset: 0x0007917C
	private void OnParentWorkStatusChanged(bool isFirstHit)
	{
		this.EvaluateByEvent(ConditionalEventType.ParentWorkCondition);
	}

	// Token: 0x06001A28 RID: 6696 RVA: 0x0007AF89 File Offset: 0x00079189
	private void OnWorkStateChanged()
	{
		this.EvaluateByEvent(ConditionalEventType.WorkStateChanged);
	}

	// Token: 0x06001A29 RID: 6697 RVA: 0x0007AF89 File Offset: 0x00079189
	private void OnWorkStateChanged(bool isFirstHit)
	{
		this.EvaluateByEvent(ConditionalEventType.WorkStateChanged);
	}

	// Token: 0x06001A2A RID: 6698 RVA: 0x0007AF89 File Offset: 0x00079189
	private void OnWorkStateChanged(CraftComponentStatus status)
	{
		this.EvaluateByEvent(ConditionalEventType.WorkStateChanged);
	}

	// Token: 0x06001A2B RID: 6699 RVA: 0x0007AF93 File Offset: 0x00079193
	private void OnConveyorChanged()
	{
		this.EvaluateByEvent(ConditionalEventType.ConveyorChanged);
	}

	// Token: 0x06001A2C RID: 6700 RVA: 0x0007AF9D File Offset: 0x0007919D
	private void OnFightingAgentInitialized(bool isValid)
	{
		this.EvaluateByEvent(ConditionalEventType.FightingAgentChanged);
	}

	// Token: 0x06001A2D RID: 6701 RVA: 0x0007AFA7 File Offset: 0x000791A7
	private void OnBuildModeStateChanged(bool isActive)
	{
		this.EvaluateByEvent(ConditionalEventType.BuildingModeChanged);
	}

	// Token: 0x06001A2E RID: 6702 RVA: 0x0007AFB4 File Offset: 0x000791B4
	private void OnZombieCaretakerStateChanged()
	{
		this.EvaluateByEvent(ConditionalEventType.CaretakerStateChanged);
		this.EvaluateByEvent(ConditionalEventType.ZombieWorkerStateChanged);
	}

	// Token: 0x06001A2F RID: 6703 RVA: 0x0007AFCC File Offset: 0x000791CC
	private void OnOccupiedDockPointStatusChanged()
	{
		this.EvaluateByEvent(ConditionalEventType.DockPointStatusChanged);
	}

	// Token: 0x06001A30 RID: 6704 RVA: 0x0007AFD9 File Offset: 0x000791D9
	private void OnTakenDockPointChanged()
	{
		this.EvaluateByEvent(ConditionalEventType.TakenDockPointChanged);
	}

	// Token: 0x06001A31 RID: 6705 RVA: 0x0007AFE6 File Offset: 0x000791E6
	private void OnHpChanged(HPComponent hpComponent)
	{
		this.EvaluateByEvent(ConditionalEventType.HPChanged);
	}

	// Token: 0x06001A32 RID: 6706 RVA: 0x0007AFF3 File Offset: 0x000791F3
	private void OnInteractableStateChanged(bool state)
	{
		this.EvaluateByEvent(ConditionalEventType.InteractableStateChanged);
	}

	// Token: 0x06001A33 RID: 6707 RVA: 0x0007B000 File Offset: 0x00079200
	private void OnConveyorSystemChanged(WgoData wgoData)
	{
		this.EvaluateByEvent(ConditionalEventType.ConveyorSystemChanged);
	}

	// Token: 0x04001961 RID: 6497
	[SerializeField]
	[SerializeReference]
	private List<ConditionalDrawerRule> rules = new List<ConditionalDrawerRule>();

	// Token: 0x04001962 RID: 6498
	private ConditionalDrawerContext context;

	// Token: 0x04001963 RID: 6499
	private Dictionary<ConditionalEventType, List<ConditionalDrawerRule>> rulesByEvent;

	// Token: 0x04001964 RID: 6500
	private WgoPart wgoPart;

	// Token: 0x04001965 RID: 6501
	private BuildController buildController;
}
