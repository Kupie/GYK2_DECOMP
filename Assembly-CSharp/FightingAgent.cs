using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using JetBrains.Annotations;
using LazyBearTechnology;
using LinqTools;
using Pathfinding;
using Pathfinding.RVO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x020002D5 RID: 725
public class FightingAgent : MonoBehaviour, IWgoCustomComponent<WCCD_ZombieAIAgent>
{
	// Token: 0x1400001D RID: 29
	// (add) Token: 0x060012A4 RID: 4772 RVA: 0x0005C268 File Offset: 0x0005A468
	// (remove) Token: 0x060012A5 RID: 4773 RVA: 0x0005C2A0 File Offset: 0x0005A4A0
	public event Action<bool> OnInitialized;

	// Token: 0x1400001E RID: 30
	// (add) Token: 0x060012A6 RID: 4774 RVA: 0x0005C2D8 File Offset: 0x0005A4D8
	// (remove) Token: 0x060012A7 RID: 4775 RVA: 0x0005C310 File Offset: 0x0005A510
	public event Action<FightingAgent, MobCommand> OnCommandCompleted;

	// Token: 0x17000310 RID: 784
	// (get) Token: 0x060012A8 RID: 4776 RVA: 0x0005C345 File Offset: 0x0005A545
	public Wgo Wgo
	{
		get
		{
			return this.wgo;
		}
	}

	// Token: 0x17000311 RID: 785
	// (get) Token: 0x060012A9 RID: 4777 RVA: 0x0005C34D File Offset: 0x0005A54D
	public RichAI_Custom RichAI
	{
		get
		{
			return this.richAI;
		}
	}

	// Token: 0x17000312 RID: 786
	// (get) Token: 0x060012AA RID: 4778 RVA: 0x0005C355 File Offset: 0x0005A555
	public MobCommand MobCommand
	{
		get
		{
			return this.mobCommand;
		}
	}

	// Token: 0x17000313 RID: 787
	// (get) Token: 0x060012AB RID: 4779 RVA: 0x0005C35D File Offset: 0x0005A55D
	public MobCommand PreviousCommand
	{
		get
		{
			return this.previousCommand;
		}
	}

	// Token: 0x17000314 RID: 788
	// (get) Token: 0x060012AC RID: 4780 RVA: 0x0005C365 File Offset: 0x0005A565
	public bool IsExecutingCommand
	{
		get
		{
			return this.isExecutingCommand;
		}
	}

	// Token: 0x17000315 RID: 789
	// (get) Token: 0x060012AD RID: 4781 RVA: 0x0005C36D File Offset: 0x0005A56D
	// (set) Token: 0x060012AE RID: 4782 RVA: 0x0005C375 File Offset: 0x0005A575
	public AgentsGroupBehaviourController ParentController
	{
		get
		{
			return this.parentController;
		}
		set
		{
			this.parentController = value;
		}
	}

	// Token: 0x17000316 RID: 790
	// (get) Token: 0x060012AF RID: 4783 RVA: 0x0005C37E File Offset: 0x0005A57E
	// (set) Token: 0x060012B0 RID: 4784 RVA: 0x0005C386 File Offset: 0x0005A586
	public AgentsGroupFlagController FlagController { get; set; }

	// Token: 0x17000317 RID: 791
	// (get) Token: 0x060012B1 RID: 4785 RVA: 0x0005C38F File Offset: 0x0005A58F
	public AttackComponent AttackComponent
	{
		get
		{
			return this.attackComponent;
		}
	}

	// Token: 0x17000318 RID: 792
	// (get) Token: 0x060012B2 RID: 4786 RVA: 0x0005C397 File Offset: 0x0005A597
	public MobCommand.CommandType AttackCommandType
	{
		get
		{
			return this.attackCommandType;
		}
	}

	// Token: 0x17000319 RID: 793
	// (get) Token: 0x060012B3 RID: 4787 RVA: 0x0005C39F File Offset: 0x0005A59F
	// (set) Token: 0x060012B4 RID: 4788 RVA: 0x0005C3A7 File Offset: 0x0005A5A7
	public ICombatEntity LastEntity { get; set; }

	// Token: 0x1700031A RID: 794
	// (get) Token: 0x060012B5 RID: 4789 RVA: 0x0005C3B0 File Offset: 0x0005A5B0
	public DecoyComponent DecoyComponent
	{
		get
		{
			return this.decoyComponent;
		}
	}

	// Token: 0x1700031B RID: 795
	// (get) Token: 0x060012B6 RID: 4790 RVA: 0x0005C3B8 File Offset: 0x0005A5B8
	// (set) Token: 0x060012B7 RID: 4791 RVA: 0x0005C3C0 File Offset: 0x0005A5C0
	public Weapon Weapon
	{
		get
		{
			return this.weapon;
		}
		set
		{
			this.weapon = value;
		}
	}

	// Token: 0x1700031C RID: 796
	// (get) Token: 0x060012B8 RID: 4792 RVA: 0x0005C3C9 File Offset: 0x0005A5C9
	public AgentAI AgentAI
	{
		get
		{
			return this.overrideAgentAI ?? this.agentAI;
		}
	}

	// Token: 0x1700031D RID: 797
	// (get) Token: 0x060012B9 RID: 4793 RVA: 0x0005C3DB File Offset: 0x0005A5DB
	public GoToDestinationModifier GoToDestinationModifier
	{
		get
		{
			if (!this.weapon || this.weapon.ItemDef.type != ItemType.Pike)
			{
				return null;
			}
			return new SpearDestinationModifier();
		}
	}

	// Token: 0x1700031E RID: 798
	// (get) Token: 0x060012BA RID: 4794 RVA: 0x0005C405 File Offset: 0x0005A605
	public DamageEffectComponent DamageEffectComponent
	{
		get
		{
			return this.damageEffectComponent;
		}
	}

	// Token: 0x1700031F RID: 799
	// (get) Token: 0x060012BB RID: 4795 RVA: 0x0005C40D File Offset: 0x0005A60D
	public FightingAgentSettings Settings
	{
		get
		{
			return this.settings;
		}
	}

	// Token: 0x17000320 RID: 800
	// (get) Token: 0x060012BC RID: 4796 RVA: 0x0005C415 File Offset: 0x0005A615
	public FighterDef FighterDef
	{
		get
		{
			return this.fighterDef;
		}
	}

	// Token: 0x17000321 RID: 801
	// (get) Token: 0x060012BD RID: 4797 RVA: 0x0005C41D File Offset: 0x0005A61D
	public bool IsValid
	{
		get
		{
			return this.isValid;
		}
	}

	// Token: 0x17000322 RID: 802
	// (get) Token: 0x060012BE RID: 4798 RVA: 0x0005C425 File Offset: 0x0005A625
	public bool IsUnderMainHeroPush
	{
		get
		{
			return this.mainHeroPushActiveUntil > Time.time;
		}
	}

	// Token: 0x17000323 RID: 803
	// (get) Token: 0x060012BF RID: 4799 RVA: 0x0005C434 File Offset: 0x0005A634
	public bool CanReposition
	{
		get
		{
			return Time.time >= this.repositionBlockedUntil;
		}
	}

	// Token: 0x060012C0 RID: 4800 RVA: 0x0005C448 File Offset: 0x0005A648
	public void SetFacingDirection(Vector2 direction, bool instant = false)
	{
		Wgo wgo = this.wgo;
		if (((wgo != null) ? wgo.Data : null) == null || direction.sqrMagnitude < 0.0001f)
		{
			return;
		}
		float num = BasicNpcSteppedRotationPreset.Instance.ComputeAngle(direction);
		if (!instant && this.hasLockedFacing)
		{
			float num2 = Vector2.SignedAngle(Vector2.right, direction);
			float num3 = Mathf.Abs(Mathf.DeltaAngle(num2, this.lockedFacingAngle));
			if (Mathf.Abs(Mathf.DeltaAngle(num2, num)) + 12f >= num3)
			{
				return;
			}
		}
		this.hasLockedFacing = true;
		this.lockedFacingAngle = num;
		this.wgo.Data.direction.Value = new Vector2(Mathf.Cos(num * 0.017453292f), Mathf.Sin(num * 0.017453292f));
	}

	// Token: 0x060012C1 RID: 4801 RVA: 0x0005C501 File Offset: 0x0005A701
	private void ResetFacingHysteresis()
	{
		this.hasLockedFacing = false;
		this.lockedFacingAngle = 0f;
	}

	// Token: 0x060012C2 RID: 4802 RVA: 0x0005C515 File Offset: 0x0005A715
	private void ResetRepositionBudget()
	{
		this.repositionsUsed = 0;
		this.repositionBlockedUntil = 0f;
	}

	// Token: 0x060012C3 RID: 4803 RVA: 0x0005C52C File Offset: 0x0005A72C
	public void ConsumeReposition(int maxCount, float cooldown)
	{
		if (maxCount <= 0)
		{
			return;
		}
		int num = this.repositionsUsed + 1;
		this.repositionsUsed = num;
		if (num < maxCount)
		{
			return;
		}
		this.repositionsUsed = 0;
		this.repositionBlockedUntil = Time.time + cooldown;
	}

	// Token: 0x060012C4 RID: 4804 RVA: 0x0005C568 File Offset: 0x0005A768
	public void Init(Wgo wgo, AgentsGroupBehaviourController parentController)
	{
		this.StopCommandExecution(false);
		this.previousCommand = null;
		this.isDyingNow = false;
		this.deathElapsedTime = 0f;
		this.LastEntity = null;
		this.IsAnchoredAtDockPoint = false;
		this.SetNavmeshCutActive(false);
		this.ResetMainHeroPushState();
		this.movementPauseStartedAt = -1f;
		this.rvoLockedBeforeMovementPause = null;
		this.ResetFacingHysteresis();
		this.ResetRepositionBudget();
		if (this.attackComponent)
		{
			AttackComponent attackComponent = this.attackComponent;
			attackComponent.OnWeaponChanged = (Action)Delegate.Remove(attackComponent.OnWeaponChanged, new Action(this.UpdateArmorLayers));
		}
		if (this.subscribedAnimationEventReceiver != null)
		{
			this.subscribedAnimationEventReceiver.onEvent1.RemoveListener(new UnityAction(this.OnAnimationTriggerAttack));
			this.subscribedAnimationEventReceiver = null;
			this.wasAnimSubscribed = false;
		}
		this.wgo = wgo;
		this.parentController = parentController;
		this.attackComponent = base.GetComponentInChildren<AttackComponent>();
		if (!this.settings)
		{
			this.settings = LazySingletonSO<GlobalResources>.Instance.fighting.defaultFightingAgentSettings;
		}
		this.fighterDef = GameBalance.Me.GetData<FighterDef>(wgo.Id);
		if (this.fighterDef != null)
		{
			this.isValid = true;
			Action<bool> onInitialized = this.OnInitialized;
			if (onInitialized != null)
			{
				onInitialized(this.isValid);
			}
			if (this.attackComponent)
			{
				this.attackComponent.Init(wgo, this.fighterDef, null);
				AttackComponent attackComponent2 = this.attackComponent;
				attackComponent2.OnWeaponChanged = (Action)Delegate.Combine(attackComponent2.OnWeaponChanged, new Action(this.UpdateArmorLayers));
				this.attackComponent.animationComponent = base.GetComponentInChildren<AnimationComponent>();
				this.attackComponent.teamType = wgo.TeamType;
				if (this.attackComponent.teamType == LazyConsts.Fighting.TeamType.Player)
				{
					this.attackComponent.animationComponent.OnDeathAnimFinished += this.HandlePlayerDeathAnimFinished;
				}
				if (this.weapon)
				{
					this.attackComponent.EquipWeapon(this.weapon);
				}
			}
			if (!wgo.TryGetComponent<RichAI_Custom>(out this.richAI))
			{
				this.richAI = wgo.gameObject.AddComponent<RichAI_Custom>();
			}
			if (!wgo.TryGetComponent<Seeker>(out this.seeker))
			{
				this.seeker = wgo.gameObject.AddComponent<Seeker>();
			}
			if (!wgo.TryGetComponent<RVOController>(out this.rvoController))
			{
				this.rvoController = wgo.gameObject.AddComponent<RVOController>();
			}
			this.animationEventReceiver = wgo.MainWgoPart.GetComponentInChildren<AnimationEventReceiver>();
			TriggerColliderComponentLinker triggerColliderComponentLinker;
			if (wgo.MainWgoPart.TryGetComponent<TriggerColliderComponentLinker>(out triggerColliderComponentLinker))
			{
				triggerColliderComponentLinker.Component = wgo;
			}
			int num = this.fighterDef.hp.EvaluateInt(wgo);
			if (num > 0)
			{
				wgo.Data.HpComponent.SetCustomHpValue(num, true);
			}
			if (wgo.TryGetComponent<WgoMovementAdjustComponent>(out this.movementAdjustComponent))
			{
				this.movementAdjustComponent.SetAdjustmentActive(false);
				wgo.UpdatePosByData = false;
			}
			if (base.TryGetComponent<DecoyComponent>(out this.decoyComponent))
			{
				this.decoyComponent.Initialize(wgo);
			}
			wgo.Data.SetCustomDeathMoment();
			CinemachineCore.CameraUpdatedEvent.RemoveListener(new UnityAction<CinemachineBrain>(this.UpdatePos));
			CinemachineCore.CameraUpdatedEvent.AddListener(new UnityAction<CinemachineBrain>(this.UpdatePos));
			this.ConfigureAgent();
			if (!this.wasAnimSubscribed && this.animationEventReceiver != null)
			{
				this.wasAnimSubscribed = true;
				this.animationEventReceiver.onEvent1.AddListener(new UnityAction(this.OnAnimationTriggerAttack));
				this.subscribedAnimationEventReceiver = this.animationEventReceiver;
			}
			return;
		}
		Debug.LogError("[ZombieAgent] FighterDef not found for wgo: " + wgo.Id + ".");
		this.isValid = false;
		Action<bool> onInitialized2 = this.OnInitialized;
		if (onInitialized2 == null)
		{
			return;
		}
		onInitialized2(this.isValid);
	}

	// Token: 0x060012C5 RID: 4805 RVA: 0x0005C90C File Offset: 0x0005AB0C
	public void DeInitForPool()
	{
		if (this.parentController != null)
		{
			this.parentController.RemoveAgent(this);
			this.parentController = null;
		}
		if (this.isExecutingCommand)
		{
			this.StopCommandExecution(false);
		}
		if (this.knockCoroutine != null)
		{
			base.StopCoroutine(this.knockCoroutine);
			this.knockCoroutine = null;
		}
		if (this.attackComponent)
		{
			AttackComponent attackComponent = this.attackComponent;
			attackComponent.OnWeaponChanged = (Action)Delegate.Remove(attackComponent.OnWeaponChanged, new Action(this.UpdateArmorLayers));
			if (this.attackComponent.animationComponent && this.attackComponent.teamType == LazyConsts.Fighting.TeamType.Player)
			{
				this.attackComponent.animationComponent.OnDeathAnimFinished -= this.HandlePlayerDeathAnimFinished;
			}
		}
		if (this.wasAnimSubscribed && this.animationEventReceiver != null)
		{
			this.animationEventReceiver.onEvent1.RemoveListener(new UnityAction(this.OnAnimationTriggerAttack));
		}
		this.wasAnimSubscribed = false;
		CinemachineCore.CameraUpdatedEvent.RemoveListener(new UnityAction<CinemachineBrain>(this.UpdatePos));
		this.mobCommand = null;
		this.previousCommand = null;
		this.isExecutingCommand = false;
		this.isKnockbackActive = false;
		this.isDyingNow = false;
		this.IsAnchoredAtDockPoint = false;
		this.SetNavmeshCutActive(false);
		this.ResetMainHeroPushState();
		this.movementPauseStartedAt = -1f;
		this.rvoLockedBeforeMovementPause = null;
		this.LastEntity = null;
		this.ResetFacingHysteresis();
		this.ResetRepositionBudget();
		this.overrideAgentAI = null;
		this.FlagController = null;
		this.weapon = null;
		this.attackCommandType = MobCommand.CommandType.ZombieMeleeAttack;
		this.isValid = false;
	}

	// Token: 0x060012C6 RID: 4806 RVA: 0x0005CAA6 File Offset: 0x0005ACA6
	private void HandlePlayerDeathAnimFinished()
	{
		if (this.wgo == null)
		{
			return;
		}
		this.wgo.Data.TriggerCustomDeathMoment();
		this.wgo.UpdateFlag(ChunkingIgnoreType.Animation, false);
	}

	// Token: 0x060012C7 RID: 4807 RVA: 0x0005CAD4 File Offset: 0x0005ACD4
	public void SetPathPenalties(List<PathfindingPenalty> penalties)
	{
		if (penalties == null)
		{
			return;
		}
		foreach (PathfindingPenalty pathfindingPenalty in penalties)
		{
			if (pathfindingPenalty.isTraversable)
			{
				this.seeker.tagEntryCosts[(int)pathfindingPenalty.tag.value] = pathfindingPenalty.penalty;
			}
			else
			{
				this.seeker.traversableTags &= ~(1 << (int)pathfindingPenalty.tag.value);
			}
		}
	}

	// Token: 0x060012C8 RID: 4808 RVA: 0x0005CB6C File Offset: 0x0005AD6C
	public void SetGraphMask(LazyConsts.Navigation.Graph graph)
	{
		if (this.seeker != null)
		{
			this.seeker.graphMask = GraphMask.FromGraphIndex((uint)graph);
		}
	}

	// Token: 0x060012C9 RID: 4809 RVA: 0x0005CB8D File Offset: 0x0005AD8D
	public void SetGraphMask(params LazyConsts.Navigation.Graph[] graphs)
	{
		if (this.seeker != null)
		{
			this.seeker.graphMask = NavigationGraphMaskUtils.ToGraphMask(graphs);
		}
	}

	// Token: 0x060012CA RID: 4810 RVA: 0x0005CBAE File Offset: 0x0005ADAE
	public void SetAgentAI(AgentAI ai)
	{
		this.overrideAgentAI = ai;
	}

	// Token: 0x060012CB RID: 4811 RVA: 0x0005CBB8 File Offset: 0x0005ADB8
	public void UpdateArmorLayers()
	{
		AnimationComponent.Layers layers = AnimationComponent.Layers.Armor;
		AttackComponent attackComponent = this.attackComponent;
		if (((attackComponent != null) ? attackComponent.weapon : null) != null)
		{
			layers = (this.attackComponent.IsRangedWeapon ? AnimationComponent.Layers.ArmorWithBow : AnimationComponent.Layers.ArmorWithPike);
		}
		AttackComponent attackComponent2 = this.attackComponent;
		if (attackComponent2 != null)
		{
			attackComponent2.animationComponent.ResetArmorLayers();
		}
		AttackComponent attackComponent3 = this.attackComponent;
		if (attackComponent3 != null)
		{
			attackComponent3.animationComponent.SetLayerWeight(layers, 1f);
		}
		AttackComponent attackComponent4 = this.attackComponent;
		if (attackComponent4 == null)
		{
			return;
		}
		attackComponent4.animationComponent.Animator.Update(0f);
	}

	// Token: 0x060012CC RID: 4812 RVA: 0x0005CC46 File Offset: 0x0005AE46
	public void SetCommand(MobCommand command)
	{
		if (this.isDyingNow)
		{
			return;
		}
		if (this.mobCommand != null)
		{
			this.previousCommand = this.mobCommand;
		}
		this.mobCommand = command;
		this.mobCommand.Init(this);
		this.StartCommandExecution();
	}

	// Token: 0x060012CD RID: 4813 RVA: 0x0005CC7E File Offset: 0x0005AE7E
	public void ClearCommand()
	{
		this.StopCommandExecution(false);
	}

	// Token: 0x060012CE RID: 4814 RVA: 0x0005CC87 File Offset: 0x0005AE87
	public void CustomUpdate(float deltaTime)
	{
		if (this.isDyingNow)
		{
			return;
		}
		this.CustomUpdateInner(deltaTime);
	}

	// Token: 0x060012CF RID: 4815 RVA: 0x0005CC9C File Offset: 0x0005AE9C
	private void CustomUpdateInner(float deltaTime)
	{
		if (this.isKnockbackActive)
		{
			this.wgo.Data.Position = this.richAI.position;
		}
		if (this.richAI != null && this.richAI.IsMovementPaused)
		{
			return;
		}
		if (this.isExecutingCommand && this.mobCommand != null)
		{
			this.mobCommand.OnUpdate(deltaTime);
			if (this.IsPushableByMainHero && this.IsMovable && !(this.mobCommand is MobCommandGoTo))
			{
				this.TickMainHeroPushState(false);
				if (this.TryApplyRvoPushDisplacement(false, deltaTime))
				{
					this.TickMainHeroPushState(true);
					this.InterruptCommandDueToPlayerPush();
					WgoPart mainWgoPart = this.wgo.MainWgoPart;
					if (mainWgoPart == null)
					{
						return;
					}
					AnimationComponentBase animationComponent = mainWgoPart.AnimationComponent;
					if (animationComponent == null)
					{
						return;
					}
					animationComponent.SetState(global::AnimationState.Walk);
					return;
				}
				else if (this.richAI.simulateMovement)
				{
					this.SyncAlliedWgoPositionFromRichAI();
				}
			}
			return;
		}
		if (this.IsAnchoredAtDockPoint)
		{
			this.RVO_Locked = true;
			AnimationComponentBase animationComponent2 = this.wgo.MainWgoPart.AnimationComponent;
			if (animationComponent2 == null)
			{
				return;
			}
			animationComponent2.SetState(global::AnimationState.Idle);
			return;
		}
		else
		{
			if (this.IsPushableByMainHero && this.IsMovable)
			{
				this.TickMainHeroPushState(false);
				bool flag = this.TryApplyRvoPushDisplacement(true, deltaTime);
				if (flag)
				{
					this.TickMainHeroPushState(true);
				}
				else if (this.richAI.simulateMovement)
				{
					this.SyncAlliedWgoPositionFromRichAI();
				}
				if (flag)
				{
					return;
				}
			}
			WgoPart mainWgoPart2 = this.wgo.MainWgoPart;
			if (mainWgoPart2 == null)
			{
				return;
			}
			AnimationComponentBase animationComponent3 = mainWgoPart2.AnimationComponent;
			if (animationComponent3 == null)
			{
				return;
			}
			animationComponent3.SetState(global::AnimationState.Idle);
			return;
		}
	}

	// Token: 0x060012D0 RID: 4816 RVA: 0x0005CE03 File Offset: 0x0005B003
	public IEnumerator SetPositionWithDelayedMovementLock(Vector3 position)
	{
		this.RVO_Locked = true;
		this.TeleportToNavmesh(position, true);
		yield return null;
		this.RVO_Locked = false;
		yield break;
	}

	// Token: 0x060012D1 RID: 4817 RVA: 0x0005CE19 File Offset: 0x0005B019
	public void TeleportToNavmesh(Vector3 position, bool clearPath = false)
	{
		this.richAI.Teleport(position, clearPath);
		this.wgo.Data.Position = this.richAI.position;
	}

	// Token: 0x060012D2 RID: 4818 RVA: 0x0005CE43 File Offset: 0x0005B043
	public void SetPosition(Vector3 position, bool stopCurrentCommand = true, bool snapToNavmesh = true)
	{
		if (stopCurrentCommand)
		{
			this.StopCommandExecution(false);
		}
		if (snapToNavmesh)
		{
			this.TeleportToNavmesh(position, false);
			return;
		}
		this.wgo.Data.Position = position;
	}

	// Token: 0x060012D3 RID: 4819 RVA: 0x0005CE6C File Offset: 0x0005B06C
	public void OnAnimationTriggerAttack()
	{
		ZombieMeleeAttackCommand zombieMeleeAttackCommand = this.MobCommand as ZombieMeleeAttackCommand;
		if (zombieMeleeAttackCommand != null)
		{
			zombieMeleeAttackCommand.HandleAttackHit();
		}
	}

	// Token: 0x060012D4 RID: 4820 RVA: 0x0005CE90 File Offset: 0x0005B090
	public void StopCommandExecution(bool reportAlsoAsCompletion = false)
	{
		if (!this.isExecutingCommand)
		{
			return;
		}
		if (this.mobCommand is ZombieAttackCommand)
		{
			AttackComponent attackComponent = this.attackComponent;
			if (attackComponent != null)
			{
				attackComponent.DetachAnimationFinishedCallback();
			}
		}
		this.mobCommand.OnFinish();
		this.isExecutingCommand = false;
		this.LastEntity = this.mobCommand.TargetEntity;
		this.previousCommand = this.mobCommand;
		this.mobCommand = null;
		this.richAI.destination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		if (reportAlsoAsCompletion)
		{
			Action<FightingAgent, MobCommand> onCommandCompleted = this.OnCommandCompleted;
			if (onCommandCompleted == null)
			{
				return;
			}
			onCommandCompleted(this, this.previousCommand);
		}
	}

	// Token: 0x060012D5 RID: 4821 RVA: 0x0005CF34 File Offset: 0x0005B134
	public void AssignWeapon(ItemDef weaponDef)
	{
		if (weaponDef == null)
		{
			this.weapon = null;
			this.attackCommandType = MobCommand.CommandType.None;
			return;
		}
		this.attackCommandType = MobCommand.CommandType.None;
		ItemType type = weaponDef.type;
		if (type != ItemType.None)
		{
			if (type != ItemType.Bow)
			{
				if (type == ItemType.Pike)
				{
					this.weapon = base.GetComponentsInChildren<Weapon>(true).First((Weapon c) => c.name.Contains("Pike"));
					if (this.weapon)
					{
						this.attackCommandType = MobCommand.CommandType.ZombiePikeAttack;
					}
				}
			}
			else
			{
				this.weapon = base.GetComponentInChildren<BowWeapon>(true);
				if (this.weapon)
				{
					this.attackCommandType = MobCommand.CommandType.ZombieBowAttack;
				}
			}
		}
		else
		{
			this.weapon = null;
			this.attackCommandType = MobCommand.CommandType.None;
		}
		if (this.weapon)
		{
			this.attackComponent.EquipWeapon(this.weapon);
		}
	}

	// Token: 0x060012D6 RID: 4822 RVA: 0x0005D006 File Offset: 0x0005B206
	public void DoKnockback(Vector3 dir, float knockDuration)
	{
		if (this.knockCoroutine != null)
		{
			base.StopCoroutine(this.knockCoroutine);
		}
		this.knockCoroutine = base.StartCoroutine(this.KnockRoutine(dir, knockDuration));
	}

	// Token: 0x060012D7 RID: 4823 RVA: 0x0005D030 File Offset: 0x0005B230
	public void PlayDying()
	{
		if (this.isDyingNow)
		{
			return;
		}
		this.isDyingNow = true;
		if (this.attackComponent && this.attackComponent.teamType == LazyConsts.Fighting.TeamType.Player)
		{
			this.wgo.UpdateFlag(ChunkingIgnoreType.Animation, true);
			this.attackComponent.animationComponent.SetState(global::AnimationState.Death);
			this.attackComponent.animationComponent.CancelBowAimLoop();
			return;
		}
		foreach (string text in this.deathEffects)
		{
			WorldFX.Spawn(base.transform.position, text, null, default(Vector3));
		}
		if (this.damageEffectComponent && this.damageEffectComponent.Settings)
		{
			FightEffectsManager fightEffectsManager = LazySingleton<FightingGameController>.Instance.FightEffectsManager;
			if (this.damageEffectComponent.Settings.spawnBloodPaddle)
			{
				fightEffectsManager.SpawnBloodPaddle(this.wgo.Data.Position, Direction.None);
			}
			if (this.damageEffectComponent.Settings.spawnBonesDecals && global::UnityEngine.Random.value < LazySingletonSO<GlobalResources>.Instance.fighting.bonesDecalSpawnProbability)
			{
				fightEffectsManager.bonesDecalsCollection.SpawnDecal(base.transform.position, Direction.None, this.customDeathEffectId);
			}
			if (this.damageEffectComponent.Settings.spawnGutsDecals && global::UnityEngine.Random.value < LazySingletonSO<GlobalResources>.Instance.fighting.gutsDecalSpawnProbability)
			{
				fightEffectsManager.gutsDecalsCollection.SpawnDecal(base.transform.position, Direction.None, "");
			}
		}
		this.wgo.Data.HandleDeath();
		this.wgo.Data.TriggerCustomDeathMoment();
	}

	// Token: 0x060012D8 RID: 4824 RVA: 0x0005D1F4 File Offset: 0x0005B3F4
	public WCCD_ZombieAIAgent OnSave()
	{
		return new WCCD_ZombieAIAgent
		{
			aiRef = this.aiRef
		};
	}

	// Token: 0x060012D9 RID: 4825 RVA: 0x0005D207 File Offset: 0x0005B407
	public void OnLoad(WCCD_ZombieAIAgent data)
	{
		this.SetStrategyReference(data.aiRef);
	}

	// Token: 0x060012DA RID: 4826 RVA: 0x00002318 File Offset: 0x00000518
	public void OnUnload()
	{
	}

	// Token: 0x060012DB RID: 4827 RVA: 0x0005D215 File Offset: 0x0005B415
	private void SetStrategyReference(AssetReferenceT<AgentAI> newReference)
	{
		this.aiRef = newReference;
		this.agentAI = AddressableUtils.LoadAssetReferenceSync<AgentAI>(this.aiRef, ref this.agentAIHandle);
	}

	// Token: 0x17000324 RID: 804
	// (get) Token: 0x060012DC RID: 4828 RVA: 0x0005D235 File Offset: 0x0005B435
	// (set) Token: 0x060012DD RID: 4829 RVA: 0x0005D23D File Offset: 0x0005B43D
	public bool IsAnchoredAtDockPoint { get; set; }

	// Token: 0x17000325 RID: 805
	// (get) Token: 0x060012DE RID: 4830 RVA: 0x0005D246 File Offset: 0x0005B446
	public bool IsPushableByMainHero
	{
		get
		{
			return this.wgo != null && this.wgo.TeamType == LazyConsts.Fighting.TeamType.Player && !this.IsAnchoredAtDockPoint;
		}
	}

	// Token: 0x17000326 RID: 806
	// (get) Token: 0x060012DF RID: 4831 RVA: 0x0005D270 File Offset: 0x0005B470
	private bool IsMovable
	{
		get
		{
			Wgo wgo = this.wgo;
			bool? flag;
			if (wgo == null)
			{
				flag = null;
			}
			else
			{
				WgoData data = wgo.Data;
				if (data == null)
				{
					flag = null;
				}
				else
				{
					WGODef definition = data.Definition;
					flag = ((definition != null) ? new bool?(definition.isMovable) : null);
				}
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}
	}

	// Token: 0x17000327 RID: 807
	// (get) Token: 0x060012E0 RID: 4832 RVA: 0x0005D2CC File Offset: 0x0005B4CC
	// (set) Token: 0x060012E1 RID: 4833 RVA: 0x0005D2D9 File Offset: 0x0005B4D9
	public float RVO_Priority
	{
		get
		{
			return this.rvoController.priority;
		}
		set
		{
			this.rvoController.priority = value;
		}
	}

	// Token: 0x17000328 RID: 808
	// (get) Token: 0x060012E2 RID: 4834 RVA: 0x0005D2E7 File Offset: 0x0005B4E7
	// (set) Token: 0x060012E3 RID: 4835 RVA: 0x0005D2F4 File Offset: 0x0005B4F4
	public bool RVO_Locked
	{
		get
		{
			return this.rvoController.locked;
		}
		set
		{
			this.rvoController.locked = value;
		}
	}

	// Token: 0x17000329 RID: 809
	// (get) Token: 0x060012E4 RID: 4836 RVA: 0x0005D302 File Offset: 0x0005B502
	// (set) Token: 0x060012E5 RID: 4837 RVA: 0x0005D30F File Offset: 0x0005B50F
	public Vector3 RVO_Velocity
	{
		get
		{
			return this.rvoController.velocity;
		}
		set
		{
			this.rvoController.velocity = value;
		}
	}

	// Token: 0x1700032A RID: 810
	// (get) Token: 0x060012E6 RID: 4838 RVA: 0x0005D31D File Offset: 0x0005B51D
	// (set) Token: 0x060012E7 RID: 4839 RVA: 0x0005D33A File Offset: 0x0005B53A
	public bool RVO_Enabled
	{
		get
		{
			return this.rvoController != null && this.rvoController.enabled;
		}
		set
		{
			this.rvoController.enabled = value;
		}
	}

	// Token: 0x1700032B RID: 811
	// (get) Token: 0x060012E8 RID: 4840 RVA: 0x0005D348 File Offset: 0x0005B548
	public bool RVO_AvoidingAnyAgents
	{
		get
		{
			return this.rvoController != null && this.rvoController.AvoidingAnyAgents;
		}
	}

	// Token: 0x1700032C RID: 812
	// (get) Token: 0x060012E9 RID: 4841 RVA: 0x0005D365 File Offset: 0x0005B565
	public float RVO_CalculatedSpeed
	{
		get
		{
			RVOController rvocontroller = this.rvoController;
			if (((rvocontroller != null) ? rvocontroller.rvoAgent : null) == null)
			{
				return 0f;
			}
			return this.rvoController.rvoAgent.CalculatedSpeed;
		}
	}

	// Token: 0x1700032D RID: 813
	// (get) Token: 0x060012EA RID: 4842 RVA: 0x0005D391 File Offset: 0x0005B591
	public int RVO_NeighbourCount
	{
		get
		{
			RVOController rvocontroller = this.rvoController;
			if (((rvocontroller != null) ? rvocontroller.rvoAgent : null) == null)
			{
				return 0;
			}
			return this.rvoController.rvoAgent.NeighbourCount;
		}
	}

	// Token: 0x060012EB RID: 4843 RVA: 0x0005D3BC File Offset: 0x0005B5BC
	public float RVO_GetMovementDeltaMagnitude(Vector3 position, float deltaTime)
	{
		if (this.rvoController == null || !this.rvoController.enabled || deltaTime <= 0f)
		{
			return 0f;
		}
		return this.rvoController.CalculateMovementDelta(position, deltaTime).XZ().magnitude;
	}

	// Token: 0x060012EC RID: 4844 RVA: 0x0005D40C File Offset: 0x0005B60C
	public void RVO_SetPlayerAvoidance(FightingAgent agent, bool isAvoiding)
	{
		if (isAvoiding)
		{
			this.rvoController.collidesWith |= RVOLayer.Layer2;
			return;
		}
		this.rvoController.collidesWith &= (RVOLayer)(-5);
	}

	// Token: 0x060012ED RID: 4845 RVA: 0x0005D439 File Offset: 0x0005B639
	public void RvoStopAt(Vector3 position)
	{
		if (this.rvoController == null)
		{
			return;
		}
		this.rvoController.SetTarget(position, 0f, 0f, position);
	}

	// Token: 0x060012EE RID: 4846 RVA: 0x0005D464 File Offset: 0x0005B664
	public void ProvideReturnDamageLogic(AttackContext ctxHitMe)
	{
		int num = (this.fighterDef.retDamage.HasExpression ? this.fighterDef.retDamage.EvaluateInt() : 0);
		if (num == 0 || ctxHitMe.isReturnDamage)
		{
			return;
		}
		ItemDef weaponDef = ctxHitMe.weaponDef;
		if (!(((weaponDef != null) ? new ItemType?(weaponDef.type) : null) != ItemType.Spit))
		{
			return;
		}
		ICombatEntity combatEntity = this.wgo;
		LazyConsts.Fighting.TeamType teamType = this.wgo.TeamType;
		FighterDef fighterDef = this.fighterDef;
		Weapon weapon = this.weapon;
		ItemDef itemDef = ((weapon != null) ? weapon.ItemDef : null);
		Vector3 position = this.wgo.Data.Position;
		Vector3 vector = ctxHitMe.hitPosition - this.wgo.Data.Position;
		int num2 = ((num > 0) ? num : (-1));
		AttackContext attackContext = new AttackContext(combatEntity, teamType, fighterDef, itemDef, position, vector, default(Vector3), num2, true, null, null);
		global::UnityEngine.Object @object = ctxHitMe.attacker as global::UnityEngine.Object;
		if (@object != null && @object)
		{
			AttackComponent attackersAttackComponent = ctxHitMe.attackersAttackComponent;
			if (attackersAttackComponent == null)
			{
				return;
			}
			attackersAttackComponent.DoHit(ctxHitMe.attacker, attackContext);
		}
	}

	// Token: 0x060012EF RID: 4847 RVA: 0x0005D57C File Offset: 0x0005B77C
	public void AssignWeaponFromInventory()
	{
		Item itemByGroupId = this.wgo.Data.Inventory.GetItemByGroupId("weapon");
		if (itemByGroupId == null)
		{
			Debug.LogError("PreSet Fighter [" + this.wgo.Id + "] has no weapon in inventory", this);
			return;
		}
		ItemType type = itemByGroupId.Definition.type;
		if (type == ItemType.Bow || type == ItemType.Pike)
		{
			this.AssignWeapon(itemByGroupId.Definition);
		}
	}

	// Token: 0x060012F0 RID: 4848 RVA: 0x0005D5EC File Offset: 0x0005B7EC
	public void PauseMovement()
	{
		if (this.richAI != null && this.richAI.IsMovementPaused)
		{
			return;
		}
		RichAI_Custom richAI_Custom = this.richAI;
		if (richAI_Custom != null)
		{
			richAI_Custom.SetMovementPaused(true);
		}
		this.movementPauseStartedAt = Time.time;
		if (this.rvoController != null)
		{
			this.rvoLockedBeforeMovementPause = new bool?(this.rvoController.locked);
			this.RVO_Locked = true;
			Vector3 vector = ((this.richAI != null) ? this.richAI.position : this.wgo.Data.Position);
			this.RvoStopAt(vector);
		}
	}

	// Token: 0x060012F1 RID: 4849 RVA: 0x0005D690 File Offset: 0x0005B890
	public void UnpauseMovement()
	{
		if (this.richAI != null && !this.richAI.IsMovementPaused && this.rvoLockedBeforeMovementPause == null)
		{
			return;
		}
		float num = ((this.movementPauseStartedAt >= 0f) ? (Time.time - this.movementPauseStartedAt) : 0f);
		this.movementPauseStartedAt = -1f;
		if (this.rvoLockedBeforeMovementPause != null)
		{
			this.RVO_Locked = this.rvoLockedBeforeMovementPause.Value;
			this.rvoLockedBeforeMovementPause = null;
		}
		RichAI_Custom richAI_Custom = this.richAI;
		if (richAI_Custom != null)
		{
			richAI_Custom.SetMovementPaused(false);
		}
		if (num <= 0f)
		{
			return;
		}
		if (this.mainHeroPushActiveUntil > 0f)
		{
			this.mainHeroPushActiveUntil += num;
		}
		MobCommand mobCommand = this.mobCommand;
		if (mobCommand == null)
		{
			return;
		}
		mobCommand.CompensatePause(num);
	}

	// Token: 0x060012F2 RID: 4850 RVA: 0x0005D763 File Offset: 0x0005B963
	public void SetNavmeshCutActive(bool active)
	{
		if (!this.navmeshCut)
		{
			return;
		}
		this.navmeshCut.enabled = active;
		this.navmeshCut.ForceUpdate();
	}

	// Token: 0x060012F3 RID: 4851 RVA: 0x0005D78C File Offset: 0x0005B98C
	private void StartCommandExecution()
	{
		if (this.isExecutingCommand)
		{
			this.StopCommandExecution(false);
		}
		MobCommand mobCommand = this.mobCommand;
		if (mobCommand == null)
		{
			return;
		}
		this.isExecutingCommand = true;
		mobCommand.OnStart();
	}

	// Token: 0x060012F4 RID: 4852 RVA: 0x0005D7C0 File Offset: 0x0005B9C0
	private IEnumerator KnockRoutine(Vector3 totalDisplacement, float knockDuration)
	{
		this.isKnockbackActive = true;
		float t = 0f;
		while (t < knockDuration)
		{
			if (!MainGame.IsGamePaused)
			{
				float deltaTime = Time.deltaTime;
				float num = this.forceFalloff.Evaluate(t / knockDuration);
				this.richAI.Move(totalDisplacement * num * deltaTime / knockDuration);
				t += deltaTime;
			}
			yield return null;
		}
		this.isKnockbackActive = false;
		yield break;
	}

	// Token: 0x060012F5 RID: 4853 RVA: 0x0005D7E0 File Offset: 0x0005B9E0
	private void TickMainHeroPushState(bool pushedThisFrame)
	{
		if (!this.IsPushableByMainHero)
		{
			this.ResetMainHeroPushState();
			return;
		}
		if (this.IsInMainHeroPushProximity() || pushedThisFrame)
		{
			this.BeginMainHeroPushPriorityOverride();
			this.mainHeroPushActiveUntil = Time.time + this.settings.mainHeroPushHoldTime;
			return;
		}
		if (!this.IsUnderMainHeroPush)
		{
			this.EndMainHeroPushPriorityOverride();
		}
	}

	// Token: 0x060012F6 RID: 4854 RVA: 0x0005D832 File Offset: 0x0005BA32
	private void BeginMainHeroPushPriorityOverride()
	{
		if (this.rvoPriorityBeforeMainHeroPush != null)
		{
			return;
		}
		this.rvoPriorityBeforeMainHeroPush = new float?(this.RVO_Priority);
		this.RVO_Priority = this.settings.mainHeroPushAllyRvoPriority;
	}

	// Token: 0x060012F7 RID: 4855 RVA: 0x0005D864 File Offset: 0x0005BA64
	private void EndMainHeroPushPriorityOverride()
	{
		if (this.rvoPriorityBeforeMainHeroPush == null)
		{
			return;
		}
		this.RVO_Priority = this.rvoPriorityBeforeMainHeroPush.Value;
		this.rvoPriorityBeforeMainHeroPush = null;
	}

	// Token: 0x060012F8 RID: 4856 RVA: 0x0005D891 File Offset: 0x0005BA91
	private void ResetMainHeroPushState()
	{
		this.mainHeroPushActiveUntil = 0f;
		this.EndMainHeroPushPriorityOverride();
	}

	// Token: 0x060012F9 RID: 4857 RVA: 0x0005D8A4 File Offset: 0x0005BAA4
	private void SyncAlliedWgoPositionFromRichAI()
	{
		if (this.IsPushableByMainHero && this.IsMovable && !(this.richAI == null))
		{
			Wgo wgo = this.wgo;
			if (((wgo != null) ? wgo.Data : null) != null)
			{
				Vector3 vector = this.richAI.ApplySeekerGraphSnap(this.richAI.position);
				if ((this.wgo.Data.Position - vector).XZ().sqrMagnitude < 0.0001f)
				{
					return;
				}
				this.wgo.Data.Position = vector;
				return;
			}
		}
	}

	// Token: 0x060012FA RID: 4858 RVA: 0x0005D938 File Offset: 0x0005BB38
	private bool TryApplyRvoPushDisplacement(bool updateAnimation, float deltaTime)
	{
		if (!this.RVO_Enabled || deltaTime <= 0f)
		{
			return false;
		}
		if (this.IsPushableByMainHero)
		{
			if (!this.IsInMainHeroPushProximity())
			{
				return false;
			}
			if (this.RVO_Locked)
			{
				this.RVO_Locked = false;
			}
		}
		else if (this.RVO_Locked)
		{
			return false;
		}
		if (!this.richAI.simulateMovement)
		{
			this.RvoStopAt(this.richAI.position);
		}
		Vector3 position = this.richAI.position;
		Vector3 vector = this.rvoController.CalculateMovementDelta(position, deltaTime);
		if (vector.sqrMagnitude < 1E-06f)
		{
			return false;
		}
		Vector3 vector2 = this.richAI.ApplySeekerGraphSnap(position + vector);
		this.wgo.Data.Position = vector2;
		this.RvoStopAt(vector2);
		this.SetFacingDirection(this.rvoController.velocity.XZ2(), false);
		if (updateAnimation)
		{
			AnimationComponentBase animationComponent = this.wgo.MainWgoPart.AnimationComponent;
			if (animationComponent != null)
			{
				animationComponent.SetState(global::AnimationState.Walk);
			}
		}
		return true;
	}

	// Token: 0x060012FB RID: 4859 RVA: 0x0005DA30 File Offset: 0x0005BC30
	private bool IsInMainHeroPushProximity()
	{
		PlayerController playerController = MainGame.PlayerController;
		ICombatEntity combatEntity = ((playerController != null) ? playerController.PhysicalBody : null);
		if (combatEntity != null)
		{
			Wgo wgo = this.wgo;
			if (((wgo != null) ? wgo.Data : null) != null)
			{
				float num = this.rvoController.radius + 0.3f + 0.15f;
				return (combatEntity.CombatEntityPosition - this.wgo.Data.Position).XZ().sqrMagnitude <= num * num;
			}
		}
		return false;
	}

	// Token: 0x060012FC RID: 4860 RVA: 0x0005DAB0 File Offset: 0x0005BCB0
	private void InterruptCommandDueToPlayerPush()
	{
		if (!this.isExecutingCommand)
		{
			return;
		}
		if (this.mobCommand is ZombieAttackCommand)
		{
			AttackComponent attackComponent = this.attackComponent;
			if (attackComponent != null)
			{
				attackComponent.CancelAttack();
			}
		}
		this.StopCommandExecution(true);
	}

	// Token: 0x060012FD RID: 4861 RVA: 0x0005DAE0 File Offset: 0x0005BCE0
	private void ConfigureAgent()
	{
		this.richAI.autoRepath.mode = AutoRepathPolicy.Mode.Never;
		this.richAI.updatePosition = false;
		this.richAI.updateRotation = false;
		this.richAI.radius = this.settings.aiPathRadius;
		this.richAI.height = this.settings.aiPathHeight;
		this.richAI.funnelSimplification = true;
		this.richAI.slowWhenNotFacingTarget = this.settings.slowWhenNotFacingTarget;
		this.richAI.preventMovingBackwards = this.settings.slowWhenNotFacingTarget && this.settings.preventMovingBackwards;
		this.richAI.endReachedDistance = 0.15f;
		this.richAI.slowdownTime = 0.7f;
		this.richAI.wallForce = this.settings.wallForce;
		this.richAI.wallDist = this.settings.wallDist;
		this.richAI.rotationSpeed = 360f;
		this.richAI.rvoDensityBehavior.enabled = false;
		this.richAI.rvoDensityBehavior.returnAfterBeingPushedAway = true;
		this.richAI.rvoDensityBehavior.densityThreshold = this.settings.rvoDensityBehaviorDensityThreshold;
		this.richAI.gravity = Vector3.zero;
		this.richAI.maxSpeed = this.fighterDef.mvtSpeed;
		this.richAI.acceleration = this.fighterDef.mvtAcceleration;
		this.seeker.graphMask = GraphMask.FromGraphIndex(12U);
		this.rvoController.layer = RVOLayer.DefaultAgent;
		this.rvoController.collidesWith = (RVOLayer)5;
		this.rvoController.agentTimeHorizon = this.settings.rvoAgentTimeHorizon;
		this.rvoController.obstacleTimeHorizon = this.settings.rvoAgentObstacleTimeHorizon;
		this.rvoController.maxNeighbours = this.settings.rvoMaxNeighbours;
		this.rvoController.priority = this.settings.defaultRvoPriority;
	}

	// Token: 0x060012FE RID: 4862 RVA: 0x0005DCE1 File Offset: 0x0005BEE1
	private void Awake()
	{
		if (this.autoBakeAsset)
		{
			this.agentAI = AddressableUtils.LoadAssetReferenceSync<AgentAI>(this.aiRef, ref this.agentAIHandle);
		}
		else
		{
			this.agentAI = this.editorAI;
		}
		this.damageEffectComponent = base.GetComponent<DamageEffectComponent>();
	}

	// Token: 0x060012FF RID: 4863 RVA: 0x0005DD1C File Offset: 0x0005BF1C
	private void OnDestroy()
	{
		RichAI_Custom richAI_Custom = this.richAI;
		if (richAI_Custom != null)
		{
			richAI_Custom.SetPath(null, false);
		}
		CinemachineCore.CameraUpdatedEvent.RemoveListener(new UnityAction<CinemachineBrain>(this.UpdatePos));
		if (this.subscribedAnimationEventReceiver != null)
		{
			this.subscribedAnimationEventReceiver.onEvent1.RemoveListener(new UnityAction(this.OnAnimationTriggerAttack));
		}
		AddressableUtils.ReleaseAssetReference<AgentAI>(ref this.agentAIHandle);
	}

	// Token: 0x06001300 RID: 4864 RVA: 0x0005DD88 File Offset: 0x0005BF88
	private void UpdatePos(CinemachineBrain brain)
	{
		RichAI_Custom richAI_Custom = this.richAI;
		if (richAI_Custom != null && !richAI_Custom.GroundSnapEnabled)
		{
			this.wgo.transform.position = this.wgo.Data.Position;
			return;
		}
		if (this.movementAdjustComponent != null && this.IsMovable)
		{
			this.movementAdjustComponent.UpdatePos(this.wgo.Data.Position);
		}
	}

	// Token: 0x04001437 RID: 5175
	[SerializeField]
	private Wgo wgo;

	// Token: 0x04001438 RID: 5176
	[SerializeField]
	private FighterDef fighterDef;

	// Token: 0x04001439 RID: 5177
	[SerializeField]
	private bool isExecutingCommand;

	// Token: 0x0400143A RID: 5178
	[SerializeField]
	private FightingAgentSettings settings;

	// Token: 0x0400143B RID: 5179
	[Space]
	[SerializeField]
	[CanBeNull]
	private Weapon weapon;

	// Token: 0x0400143C RID: 5180
	[SerializeField]
	private AttackComponent attackComponent;

	// Token: 0x0400143D RID: 5181
	private MobCommand.CommandType attackCommandType = MobCommand.CommandType.ZombieMeleeAttack;

	// Token: 0x0400143E RID: 5182
	[SerializeField]
	private AssetReferenceT<AgentAI> aiRef;

	// Token: 0x0400143F RID: 5183
	[SerializeField]
	private bool autoBakeAsset;

	// Token: 0x04001440 RID: 5184
	[SerializeField]
	private AgentAI editorAI;

	// Token: 0x04001441 RID: 5185
	[SerializeField]
	[CanBeNull]
	private NavmeshCut navmeshCut;

	// Token: 0x04001442 RID: 5186
	[Header("Death")]
	private float deathDuration = 1f;

	// Token: 0x04001443 RID: 5187
	private float deathElapsedTime;

	// Token: 0x04001444 RID: 5188
	private bool isDyingNow;

	// Token: 0x04001445 RID: 5189
	public string customDeathEffectId = "";

	// Token: 0x04001446 RID: 5190
	public List<string> deathEffects = new List<string>();

	// Token: 0x04001447 RID: 5191
	private AnimationCurve forceFalloff = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

	// Token: 0x04001448 RID: 5192
	private float knockDuration = 0.5f;

	// Token: 0x04001449 RID: 5193
	private Coroutine knockCoroutine;

	// Token: 0x0400144A RID: 5194
	private bool isKnockbackActive;

	// Token: 0x0400144B RID: 5195
	private RichAI_Custom richAI;

	// Token: 0x0400144C RID: 5196
	private Seeker seeker;

	// Token: 0x0400144D RID: 5197
	private RVOController rvoController;

	// Token: 0x0400144E RID: 5198
	private MobCommand mobCommand;

	// Token: 0x0400144F RID: 5199
	private MobCommand previousCommand;

	// Token: 0x04001450 RID: 5200
	private AnimationEventReceiver animationEventReceiver;

	// Token: 0x04001451 RID: 5201
	private AnimationEventReceiver subscribedAnimationEventReceiver;

	// Token: 0x04001452 RID: 5202
	private WgoMovementAdjustComponent movementAdjustComponent;

	// Token: 0x04001453 RID: 5203
	private DecoyComponent decoyComponent;

	// Token: 0x04001454 RID: 5204
	private AgentAI agentAI;

	// Token: 0x04001455 RID: 5205
	private AgentAI overrideAgentAI;

	// Token: 0x04001456 RID: 5206
	private AsyncOperationHandle<AgentAI> agentAIHandle;

	// Token: 0x04001457 RID: 5207
	[CanBeNull]
	private DamageEffectComponent damageEffectComponent;

	// Token: 0x04001458 RID: 5208
	private AgentsGroupBehaviourController parentController;

	// Token: 0x04001459 RID: 5209
	private bool wasAnimSubscribed;

	// Token: 0x0400145A RID: 5210
	private bool isValid;

	// Token: 0x0400145B RID: 5211
	private float mainHeroPushActiveUntil;

	// Token: 0x0400145C RID: 5212
	private float? rvoPriorityBeforeMainHeroPush;

	// Token: 0x0400145D RID: 5213
	private float movementPauseStartedAt = -1f;

	// Token: 0x0400145E RID: 5214
	private bool? rvoLockedBeforeMovementPause;

	// Token: 0x0400145F RID: 5215
	private bool hasLockedFacing;

	// Token: 0x04001460 RID: 5216
	private float lockedFacingAngle;

	// Token: 0x04001461 RID: 5217
	private const float FACING_HYSTERESIS_DEGREES = 12f;

	// Token: 0x04001462 RID: 5218
	private int repositionsUsed;

	// Token: 0x04001463 RID: 5219
	private float repositionBlockedUntil;

	// Token: 0x04001467 RID: 5223
	private const float ALLIED_WGO_RICHAI_SYNC_THRESHOLD_SQR = 0.0001f;
}
