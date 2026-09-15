using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LazyBearTechnology;
using LinqTools;
using UnityEngine;

// Token: 0x0200028A RID: 650
[CreateAssetMenu(menuName = "GK2/Fighting/AIs/EnemyDefault")]
public class EnemyDefaultAI : AgentAI
{
	// Token: 0x170002BA RID: 698
	// (get) Token: 0x060010CE RID: 4302 RVA: 0x00054A33 File Offset: 0x00052C33
	protected virtual List<IEnemyDecisionStep> DefaultDecisionSteps
	{
		get
		{
			return new List<IEnemyDecisionStep>
			{
				new EnemyDefaultAI.LineEngagementDecisionStep(),
				new EnemyDefaultAI.CapturePointDecisionStep(),
				new EnemyDefaultAI.PrimaryTargetDecisionStep()
			};
		}
	}

	// Token: 0x060010CF RID: 4303 RVA: 0x00054A5C File Offset: 0x00052C5C
	[CanBeNull]
	public override MobCommand GetCommand(FightingAgent agent, Func<IEnumerable<ICombatEntity>> targets)
	{
		EnemyDefaultAI.EnemyDecisionContext enemyDecisionContext = this.BuildContext(agent, targets);
		IReadOnlyList<IEnemyDecisionStep> activeDecisionSteps = this.GetActiveDecisionSteps();
		for (int i = 0; i < activeDecisionSteps.Count; i++)
		{
			IEnemyDecisionStep enemyDecisionStep = activeDecisionSteps[i];
			if (enemyDecisionStep != null)
			{
				MobCommand mobCommand = enemyDecisionStep.TryCreateCommand(enemyDecisionContext);
				if (mobCommand != null)
				{
					return mobCommand;
				}
			}
		}
		return null;
	}

	// Token: 0x060010D0 RID: 4304 RVA: 0x00054AA6 File Offset: 0x00052CA6
	protected virtual EnemyDefaultAI.EnemyDecisionContext BuildContext(FightingAgent agent, Func<IEnumerable<ICombatEntity>> targets)
	{
		return new EnemyDefaultAI.EnemyDecisionContext(this, agent, this.WrapPotentialTargets(agent, targets), this.aggroDistance);
	}

	// Token: 0x060010D1 RID: 4305 RVA: 0x00054ABD File Offset: 0x00052CBD
	protected override bool IsTargetValid(ICombatEntity target, FightingAgent agent)
	{
		return target != null && base.IsTargetValid(target, agent) && (target.EntityType & this.ignoreEntityTypes) == LazyConsts.Fighting.EntityType.None;
	}

	// Token: 0x060010D2 RID: 4306 RVA: 0x00054AE0 File Offset: 0x00052CE0
	protected float DistToPos(FightingAgent agent, Vector3 pos)
	{
		return (agent.Wgo.Data.Position - pos).XZ().magnitude;
	}

	// Token: 0x060010D3 RID: 4307 RVA: 0x00054B10 File Offset: 0x00052D10
	protected bool IsOnCapturePoint(Vector3 pos, FightingCapturePoint point)
	{
		return (pos - point.transform.position).XZ().magnitude < point.Radius;
	}

	// Token: 0x060010D4 RID: 4308 RVA: 0x00054B44 File Offset: 0x00052D44
	private bool IsAgentInsideEpsilonCatchPoint(FightingAgent agent, FightingCapturePoint point)
	{
		float num = this.DistToPos(agent, point.transform.position);
		return num < point.Radius && num > point.Radius - 0.4f;
	}

	// Token: 0x060010D5 RID: 4309 RVA: 0x00054B80 File Offset: 0x00052D80
	private IReadOnlyList<IEnemyDecisionStep> GetActiveDecisionSteps()
	{
		if (this.customDecisionSteps != null)
		{
			for (int i = this.customDecisionSteps.Count - 1; i >= 0; i--)
			{
				if (this.customDecisionSteps[i] == null)
				{
					this.customDecisionSteps.RemoveAt(i);
				}
			}
			if (this.customDecisionSteps.Count > 0)
			{
				return this.customDecisionSteps;
			}
		}
		if (this.defaultDecisionSteps == null)
		{
			this.defaultDecisionSteps = this.DefaultDecisionSteps;
		}
		return this.defaultDecisionSteps;
	}

	// Token: 0x060010D6 RID: 4310 RVA: 0x00054BF8 File Offset: 0x00052DF8
	private MobCommand TryGetLineEngagementCommand(EnemyDefaultAI.EnemyDecisionContext context)
	{
		if (context.ClosestAggroTarget != null)
		{
			return null;
		}
		ICombatEntity combatEntity = ((!context.IsRangedAttacker) ? context.NearestTargetOnLine : context.NearestTargetOnLineDirectVisible);
		if (combatEntity == null)
		{
			return null;
		}
		if (context.IsAgentOnNearestCapturePoint)
		{
			return null;
		}
		FightingCapturePoint nearestCapturePoint = context.NearestCapturePoint;
		return this.DoGoAndAttackLogic(context.Agent, combatEntity, this.CreateLineTargetStopCondition(context, combatEntity, nearestCapturePoint));
	}

	// Token: 0x060010D7 RID: 4311 RVA: 0x00054C59 File Offset: 0x00052E59
	private MobCommand TryGetCapturePointCommand(EnemyDefaultAI.EnemyDecisionContext context)
	{
		return this.DoCapturingPointLogic(context);
	}

	// Token: 0x060010D8 RID: 4312 RVA: 0x00054C64 File Offset: 0x00052E64
	protected virtual MobCommand TryEngagePrimaryTarget(EnemyDefaultAI.EnemyDecisionContext context)
	{
		if (!context.HasPlayerTargets)
		{
			return null;
		}
		ICombatEntity combatEntity = context.ClosestAggroTarget ?? (context.IsAgentOnNearestCapturePoint ? null : context.ClosestTarget);
		if (combatEntity == null)
		{
			return null;
		}
		return this.DoGoAndAttackLogic(context.Agent, combatEntity, this.CreatePrimaryTargetStopCondition(context, combatEntity));
	}

	// Token: 0x060010D9 RID: 4313 RVA: 0x00054CB6 File Offset: 0x00052EB6
	private Func<bool> CreateLineTargetStopCondition(EnemyDefaultAI.EnemyDecisionContext context, ICombatEntity targetOnLine, FightingCapturePoint capturePoint)
	{
		return delegate
		{
			if (!this.IsTargetValid(targetOnLine, context.Agent))
			{
				return true;
			}
			if (capturePoint)
			{
				if (this.DistToPos(context.Agent, capturePoint.transform.position) < this.DistToPos(context.Agent, targetOnLine.CombatEntityPosition))
				{
					return true;
				}
				MobCommandGoTo mobCommandGoTo = context.Agent.MobCommand as MobCommandGoTo;
				if (mobCommandGoTo != null)
				{
					ControlPointDestinationModifier controlPointDestinationModifier = mobCommandGoTo.DestinationModifier as ControlPointDestinationModifier;
					if (controlPointDestinationModifier != null)
					{
						if (this.DistToPos(context.Agent, controlPointDestinationModifier.GetCurrentTargetPosition()) < 0.1f)
						{
							return true;
						}
						goto IL_00FA;
					}
				}
				if (this.IsOnCapturePoint(context.Agent.Wgo.Data.Position, capturePoint))
				{
					return true;
				}
			}
			IL_00FA:
			FightingLine fightingLine = (context.Agent.ParentController ? context.Agent.ParentController.FightingLine : null);
			ICombatEntity combatEntity = this.FindFrontmostTargetOnLine(context.Agent, fightingLine, context.IsRangedAttacker, null);
			if (combatEntity != null && combatEntity != targetOnLine)
			{
				return true;
			}
			ICombatEntity closestTarget = AgentAI.GetClosestTarget(context.Agent.Wgo.Data.Position, context.PlayerTargets, context.Agent.Wgo.TeamType, context.AggroDistance, context.IsRangedAttacker);
			return (closestTarget != null && closestTarget != targetOnLine) || this.ShouldRetargetDockAssignment(context.Agent, targetOnLine);
		};
	}

	// Token: 0x060010DA RID: 4314 RVA: 0x00054CE4 File Offset: 0x00052EE4
	protected Func<bool> CreatePrimaryTargetStopCondition(EnemyDefaultAI.EnemyDecisionContext context, ICombatEntity chosenEntity)
	{
		return delegate
		{
			if (!this.IsTargetValid(chosenEntity, context.Agent))
			{
				return true;
			}
			MobCommandGoTo mobCommandGoTo = context.Agent.MobCommand as MobCommandGoTo;
			if (mobCommandGoTo != null)
			{
				ControlPointDestinationModifier controlPointDestinationModifier = mobCommandGoTo.DestinationModifier as ControlPointDestinationModifier;
				if (controlPointDestinationModifier != null)
				{
					if (this.DistToPos(context.Agent, controlPointDestinationModifier.GetCurrentTargetPosition()) < 0.1f)
					{
						return true;
					}
					goto IL_00A1;
				}
			}
			if (this.IsOnCapturePoint(context.Agent.Wgo.Data.Position, context.NearestCapturePoint))
			{
				return true;
			}
			IL_00A1:
			return AgentAI.GetClosestTarget(context.Agent.Wgo.Data.Position, context.PlayerTargets, context.Agent.Wgo.TeamType, context.AggroDistance, context.IsRangedAttacker) != chosenEntity || this.ShouldRetargetDockAssignment(context.Agent, chosenEntity);
		};
	}

	// Token: 0x060010DB RID: 4315 RVA: 0x00054D0C File Offset: 0x00052F0C
	protected bool ShouldRetargetDockAssignment(FightingAgent agent, ICombatEntity chosenEntity)
	{
		Wgo wgo = chosenEntity as Wgo;
		WgoData wgoData = ((wgo != null) ? wgo.Data : null);
		if (wgoData == null)
		{
			MobCommandGoTo mobCommandGoTo = agent.MobCommand as MobCommandGoTo;
			if (mobCommandGoTo != null)
			{
				CombatEntityDestinationModifier combatEntityDestinationModifier = mobCommandGoTo.DestinationModifier as CombatEntityDestinationModifier;
				if (combatEntityDestinationModifier != null)
				{
					wgoData = combatEntityDestinationModifier.TargeObj;
				}
			}
		}
		if (((wgoData != null) ? wgoData.MainWgoPartData : null) == null)
		{
			return false;
		}
		SGuid combatEntityUID = agent.Wgo.CombatEntityUID;
		if (wgoData.MainWgoPartData.GetOccupiedDockPointBy(combatEntityUID) != null)
		{
			return false;
		}
		ZombieMeleeAttackCommand zombieMeleeAttackCommand = agent.MobCommand as ZombieMeleeAttackCommand;
		if (zombieMeleeAttackCommand != null && zombieMeleeAttackCommand.CustomDockPoint != null && !zombieMeleeAttackCommand.CustomDockPoint.IsOccupied)
		{
			return false;
		}
		MobCommandGoTo mobCommandGoTo2 = agent.MobCommand as MobCommandGoTo;
		if (mobCommandGoTo2 != null)
		{
			CombatEntityDestinationModifier combatEntityDestinationModifier2 = mobCommandGoTo2.DestinationModifier as CombatEntityDestinationModifier;
			if (combatEntityDestinationModifier2 != null && combatEntityDestinationModifier2.TargetDockPoint != null)
			{
				return combatEntityDestinationModifier2.TargetDockPoint.IsOccupied && !combatEntityDestinationModifier2.TargetDockPoint.IsOccupiedBy(combatEntityUID);
			}
		}
		return base.HasAnyAvailableDockPointOnRecast(wgoData, DockPointData.Filter.All);
	}

	// Token: 0x060010DC RID: 4316 RVA: 0x00054E00 File Offset: 0x00053000
	private MobCommand DoGoAndAttackLogic(FightingAgent agent, ICombatEntity entity, Func<bool> stopCondition)
	{
		if (!this.IsTargetValid(entity, agent))
		{
			return null;
		}
		Wgo wgo = entity as Wgo;
		base.ReleaseTakenDockPointIfDifferent(agent, wgo);
		FighterDef fighterDef = agent.FighterDef;
		float num = ((fighterDef != null) ? fighterDef.atkRange.EvaluateFloat() : this.attackDistance);
		FighterDef fighterDef2 = agent.FighterDef;
		int num3;
		if (fighterDef2 == null)
		{
			Weapon weapon = agent.Weapon;
			int? num2;
			if (weapon == null)
			{
				num2 = null;
			}
			else
			{
				ItemDef itemDef = weapon.ItemDef;
				num2 = ((itemDef != null) ? new int?(itemDef.damage.EvaluateInt(agent.Wgo)) : null);
			}
			num3 = num2 ?? this.zombieMeleeDamage;
		}
		else
		{
			num3 = fighterDef2.atkDamage.EvaluateInt(agent.Wgo);
		}
		int num4 = num3;
		float num5 = -1f;
		DockPointData dockPoint = null;
		if (wgo && base.TryGetDockPoint(agent, wgo, out dockPoint, DockPointData.Availability.OnlyNotOccupied, DockPointData.Filter.All, false, null))
		{
			num5 = this.DistToPos(agent, dockPoint.GetPosFrom(wgo.Data.Position));
			if ((num5 - 0.06666668f).More(0f, 0.0001f))
			{
				return new MobCommandGoTo(new CombatEntityDestinationModifier(dockPoint)).WithCustomTargetDestinationOffset(0.06666668f).WithCustomStopCondition(stopCondition, this.retargetDeltaTime, global::UnityEngine.Random.Range(0f, this.retargetDeltaTime)).WithCustomActionOnDestReached(delegate
				{
					agent.SetFacingDirection(dockPoint.Direction.ConvertToVector2XZ(), true);
					EnemyDefaultAI.TryAnchorAtTakenDockPoint(agent);
				})
					.ToTarget(wgo);
			}
		}
		DockPointData dockPointData = null;
		float num6 = num;
		if (dockPoint != null)
		{
			num6 = 0.06666668f;
		}
		else
		{
			num5 = this.DistToPos(agent, entity.CombatEntityPosition);
			float num7;
			if (wgo != null && base.TryGetOverflowDockRing(agent, wgo, out dockPointData, out num7))
			{
				num6 = num7;
			}
		}
		float num8 = Mathf.Max(num, num6);
		if ((num5 - num6).More(0f, 0.0001f))
		{
			return new MobCommandGoTo(new CombatEntityDestinationModifier(dockPoint)).WithCustomTargetDestinationOffset(num6).WithCustomStopCondition(stopCondition, this.retargetDeltaTime, global::UnityEngine.Random.Range(0f, this.retargetDeltaTime)).WithCustomActionOnDestReached(delegate
			{
				EnemyDefaultAI.TryAnchorAtTakenDockPoint(agent);
			})
				.ToTarget(entity);
		}
		if ((num5 - num8).Less(0f, 0.0001f))
		{
			if (dockPoint != null)
			{
				base.TryClaimDockPoint(agent, wgo, dockPoint);
			}
			return new ZombieMeleeAttackCommand((dockPoint != null) ? num : Mathf.Max(num, num6)).WithCustomDockPoint(dockPoint ?? dockPointData).WithDamage(num4).WithCustomStopCondition(stopCondition)
				.ToTarget(entity);
		}
		return null;
	}

	// Token: 0x060010DD RID: 4317 RVA: 0x000550D0 File Offset: 0x000532D0
	[CanBeNull]
	protected ICombatEntity FindFrontmostTargetOnLine(FightingAgent agent, [CanBeNull] FightingLine fightingLine, bool requireDirectVisibility, Func<ICombatEntity, bool> additionalFilter = null)
	{
		if (!(fightingLine == null))
		{
			FightingGameController instance = LazySingleton<FightingGameController>.Instance;
			if (((instance != null) ? instance.TargetsDatabase : null) != null)
			{
				int num = int.MinValue;
				ICombatEntity combatEntity = null;
				float num2 = float.MaxValue;
				foreach (TargetInfo targetInfo in LazySingleton<FightingGameController>.Instance.TargetsDatabase.AllTargets)
				{
					if (targetInfo.LineId == fightingLine.lineIdx && targetInfo.Team == LazyConsts.Fighting.TeamType.Player && this.IsTargetValid(targetInfo.entity, agent) && (additionalFilter == null || additionalFilter(targetInfo.entity)) && (!requireDirectVisibility || AgentAI.TryLineCastByRecast(agent.Wgo.Data.Position, targetInfo.entity.CombatEntityPosition)) && targetInfo.SectorId >= num)
					{
						float combatEntityDistance = targetInfo.entity.GetCombatEntityDistance(agent.Wgo.Data.Position, agent.Wgo.TeamType);
						if (targetInfo.SectorId > num)
						{
							num = targetInfo.SectorId;
							combatEntity = targetInfo.entity;
							num2 = combatEntityDistance;
						}
						else if (combatEntityDistance < num2)
						{
							combatEntity = targetInfo.entity;
							num2 = combatEntityDistance;
						}
					}
				}
				return combatEntity;
			}
		}
		return null;
	}

	// Token: 0x060010DE RID: 4318 RVA: 0x00055224 File Offset: 0x00053424
	private static void TryAnchorAtTakenDockPoint(FightingAgent agent)
	{
		SGuid sguid;
		if (agent == null)
		{
			sguid = null;
		}
		else
		{
			Wgo wgo = agent.Wgo;
			if (wgo == null)
			{
				sguid = null;
			}
			else
			{
				WgoData data = wgo.Data;
				sguid = ((data != null) ? data.takenDockPointsParentSGuid : null);
			}
		}
		if (!SGuid.IsNullOrEmpty(sguid))
		{
			agent.IsAnchoredAtDockPoint = true;
		}
	}

	// Token: 0x060010DF RID: 4319 RVA: 0x00055258 File Offset: 0x00053458
	private MobCommand DoCapturingPointLogic(EnemyDefaultAI.EnemyDecisionContext context)
	{
		EnemyDefaultAI.<>c__DisplayClass29_0 CS$<>8__locals1 = new EnemyDefaultAI.<>c__DisplayClass29_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.context = context;
		if (CS$<>8__locals1.context.ClosestAggroTarget != null)
		{
			return null;
		}
		CS$<>8__locals1.capturePoint = CS$<>8__locals1.context.NearestCapturePoint;
		if (!CS$<>8__locals1.capturePoint)
		{
			return null;
		}
		CS$<>8__locals1.playerTargets = CS$<>8__locals1.context.PlayerTargets;
		EnemyDefaultAI.<>c__DisplayClass29_0 CS$<>8__locals2 = CS$<>8__locals1;
		Func<IEnumerable<ICombatEntity>> playerTargets = CS$<>8__locals1.playerTargets;
		CS$<>8__locals2.targets = ((playerTargets != null) ? playerTargets() : null);
		CS$<>8__locals1.agent = CS$<>8__locals1.context.Agent;
		if (CS$<>8__locals1.capturePoint.OwnedByTeam != CS$<>8__locals1.agent.Wgo.TeamType)
		{
			bool flag = this.IsOnCapturePoint(CS$<>8__locals1.agent.Wgo.Data.Position, CS$<>8__locals1.capturePoint);
			MobCommandGoTo mobCommandGoTo = CS$<>8__locals1.agent.MobCommand as MobCommandGoTo;
			if (mobCommandGoTo != null)
			{
				ControlPointDestinationModifier controlPointDestinationModifier = mobCommandGoTo.DestinationModifier as ControlPointDestinationModifier;
				if (controlPointDestinationModifier != null)
				{
					if (this.DistToPos(CS$<>8__locals1.agent, controlPointDestinationModifier.GetCurrentTargetPosition()) >= 0.1f)
					{
						return null;
					}
					flag = true;
				}
			}
			if (!flag)
			{
				return new MobCommandGoTo(new ControlPointDestinationModifier(CS$<>8__locals1.capturePoint)).WithCustomStopCondition(() => AgentAI.GetClosestTarget(CS$<>8__locals1.agent.Wgo.Data.Position, CS$<>8__locals1.playerTargets, CS$<>8__locals1.agent.Wgo.TeamType, CS$<>8__locals1.<>4__this.aggroDistance, CS$<>8__locals1.context.IsRangedAttacker) != null || CS$<>8__locals1.capturePoint.OwnedByTeam == LazyConsts.Fighting.TeamType.WildZombie, this.retargetDeltaTime, global::UnityEngine.Random.Range(0f, this.retargetDeltaTime)).ToPosition(CS$<>8__locals1.capturePoint.gameObject.transform.position);
			}
			ICombatEntity entityInsideSector = AgentAI.GetClosestTarget(CS$<>8__locals1.capturePoint.transform.position, CS$<>8__locals1.targets, CS$<>8__locals1.context.Agent.Wgo.TeamType, this.aggroDistance, false);
			if (entityInsideSector != null && this.IsAgentInsideEpsilonCatchPoint(CS$<>8__locals1.agent, CS$<>8__locals1.capturePoint))
			{
				return this.DoGoAndAttackLogic(CS$<>8__locals1.agent, entityInsideSector, () => !CS$<>8__locals1.<>4__this.IsOnCapturePoint(entityInsideSector.CombatEntityPosition, CS$<>8__locals1.capturePoint) || AgentAI.GetClosestTarget(CS$<>8__locals1.capturePoint.transform.position, CS$<>8__locals1.targets, CS$<>8__locals1.context.Agent.Wgo.TeamType, CS$<>8__locals1.<>4__this.aggroDistance, false) != entityInsideSector);
			}
		}
		return null;
	}

	// Token: 0x040012F3 RID: 4851
	private const float EPSILON = 0.4f;

	// Token: 0x040012F4 RID: 4852
	private const float TINY_EPSILON = 0.0001f;

	// Token: 0x040012F5 RID: 4853
	public float attackDistance = 1f;

	// Token: 0x040012F6 RID: 4854
	[Range(0f, 10f)]
	public float retargetDeltaTime = 1f;

	// Token: 0x040012F7 RID: 4855
	public int zombieMeleeDamage = 1;

	// Token: 0x040012F8 RID: 4856
	public LazyConsts.Fighting.EntityType ignoreEntityTypes;

	// Token: 0x040012F9 RID: 4857
	[SerializeReference]
	private List<IEnemyDecisionStep> customDecisionSteps = new List<IEnemyDecisionStep>();

	// Token: 0x040012FA RID: 4858
	private List<IEnemyDecisionStep> defaultDecisionSteps;

	// Token: 0x0200028B RID: 651
	[Serializable]
	protected sealed class LineEngagementDecisionStep : IEnemyDecisionStep, IAgentDecisionStep<EnemyDefaultAI.EnemyDecisionContext>
	{
		// Token: 0x060010E1 RID: 4321 RVA: 0x0005549B File Offset: 0x0005369B
		public MobCommand TryCreateCommand(EnemyDefaultAI.EnemyDecisionContext context)
		{
			if (context.Owner == null)
			{
				return null;
			}
			return context.Owner.TryGetLineEngagementCommand(context);
		}
	}

	// Token: 0x0200028C RID: 652
	[Serializable]
	protected sealed class CapturePointDecisionStep : IEnemyDecisionStep, IAgentDecisionStep<EnemyDefaultAI.EnemyDecisionContext>
	{
		// Token: 0x060010E3 RID: 4323 RVA: 0x000554BB File Offset: 0x000536BB
		public MobCommand TryCreateCommand(EnemyDefaultAI.EnemyDecisionContext context)
		{
			if (context.Owner == null)
			{
				return null;
			}
			return context.Owner.TryGetCapturePointCommand(context);
		}
	}

	// Token: 0x0200028D RID: 653
	[Serializable]
	protected sealed class PrimaryTargetDecisionStep : IEnemyDecisionStep, IAgentDecisionStep<EnemyDefaultAI.EnemyDecisionContext>
	{
		// Token: 0x060010E5 RID: 4325 RVA: 0x000554DB File Offset: 0x000536DB
		public MobCommand TryCreateCommand(EnemyDefaultAI.EnemyDecisionContext context)
		{
			if (context.Owner == null)
			{
				return null;
			}
			return context.Owner.TryEngagePrimaryTarget(context);
		}
	}

	// Token: 0x0200028E RID: 654
	public readonly struct EnemyDecisionContext
	{
		// Token: 0x060010E7 RID: 4327 RVA: 0x000554FC File Offset: 0x000536FC
		public EnemyDecisionContext(EnemyDefaultAI owner, FightingAgent agent, Func<IEnumerable<ICombatEntity>> potentialTargets, float aggroDistance)
		{
			this.owner = owner;
			this.agent = agent;
			this.aggroDistance = aggroDistance;
			AttackComponent attackComponent = agent.AttackComponent;
			this.isRangedAttacker = attackComponent != null && attackComponent.IsRangedWeapon;
			this.playerTargets = potentialTargets;
			this.closestAggroTarget = AgentAI.GetClosestTarget(agent.Wgo.Data.Position, this.playerTargets, agent.Wgo.TeamType, aggroDistance, this.isRangedAttacker);
			this.closestTarget = AgentAI.GetClosestTarget(agent.Wgo.Data.Position, this.playerTargets, agent.Wgo.TeamType, float.PositiveInfinity, false);
			FightingLine fightingLine = (agent.ParentController ? agent.ParentController.FightingLine : null);
			this.nearestTargetOnLine = owner.FindFrontmostTargetOnLine(agent, fightingLine, false, null);
			this.nearestTargetOnLineDirectVisible = owner.FindFrontmostTargetOnLine(agent, fightingLine, true, null);
			FightingCapturePoint fightingCapturePoint;
			if (fightingLine == null)
			{
				fightingCapturePoint = null;
			}
			else
			{
				FightingSector fightingSector = fightingLine.FindNearestEnemySectorBy(LazyConsts.Fighting.TeamType.WildZombie);
				fightingCapturePoint = ((fightingSector != null) ? fightingSector.point : null);
			}
			FightingCapturePoint fightingCapturePoint2 = fightingCapturePoint;
			if (!fightingCapturePoint2)
			{
				FightingLevel currentLevel = LazySingleton<FightingGameController>.Instance.CurrentLevel;
				fightingCapturePoint2 = ((currentLevel != null) ? currentLevel.BaseCapturePoint : null);
			}
			this.nearestCapturePoint = fightingCapturePoint2;
			this.isAgentOnNearestCapturePoint = fightingCapturePoint2 && owner.IsOnCapturePoint(agent.Wgo.Data.Position, fightingCapturePoint2);
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x00055648 File Offset: 0x00053848
		public EnemyDecisionContext(EnemyDefaultAI owner, FightingAgent agent, Func<IEnumerable<ICombatEntity>> potentialTargets, float aggroDistance, ICombatEntity nearestTargetOnLine)
		{
			this.owner = owner;
			this.agent = agent;
			this.aggroDistance = aggroDistance;
			this.playerTargets = potentialTargets;
			AttackComponent attackComponent = agent.AttackComponent;
			this.isRangedAttacker = attackComponent != null && attackComponent.IsRangedWeapon;
			this.closestAggroTarget = AgentAI.GetClosestTarget(agent.Wgo.Data.Position, this.playerTargets, agent.Wgo.TeamType, aggroDistance, this.isRangedAttacker);
			this.closestTarget = AgentAI.GetClosestTarget(agent.Wgo.Data.Position, this.playerTargets, agent.Wgo.TeamType, float.PositiveInfinity, false);
			FightingLine fightingLine = (agent.ParentController ? agent.ParentController.FightingLine : null);
			this.nearestTargetOnLine = ((nearestTargetOnLine != null && owner.IsTargetValid(nearestTargetOnLine, agent)) ? nearestTargetOnLine : null);
			this.nearestTargetOnLineDirectVisible = (fightingLine ? owner.FindFrontmostTargetOnLine(agent, fightingLine, true, null) : null);
			FightingCapturePoint fightingCapturePoint;
			if (fightingLine == null)
			{
				fightingCapturePoint = null;
			}
			else
			{
				FightingSector fightingSector = fightingLine.FindNearestEnemySectorBy(LazyConsts.Fighting.TeamType.WildZombie);
				fightingCapturePoint = ((fightingSector != null) ? fightingSector.point : null);
			}
			FightingCapturePoint fightingCapturePoint2 = fightingCapturePoint;
			if (!fightingCapturePoint2)
			{
				FightingLevel currentLevel = LazySingleton<FightingGameController>.Instance.CurrentLevel;
				fightingCapturePoint2 = ((currentLevel != null) ? currentLevel.BaseCapturePoint : null);
			}
			this.nearestCapturePoint = fightingCapturePoint2;
			this.isAgentOnNearestCapturePoint = fightingCapturePoint2 && owner.IsOnCapturePoint(agent.Wgo.Data.Position, fightingCapturePoint2);
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x060010E9 RID: 4329 RVA: 0x000557A9 File Offset: 0x000539A9
		public EnemyDefaultAI Owner
		{
			get
			{
				return this.owner;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x060010EA RID: 4330 RVA: 0x000557B1 File Offset: 0x000539B1
		public FightingAgent Agent
		{
			get
			{
				return this.agent;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x060010EB RID: 4331 RVA: 0x000557B9 File Offset: 0x000539B9
		public Func<IEnumerable<ICombatEntity>> PlayerTargets
		{
			get
			{
				return this.playerTargets;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x060010EC RID: 4332 RVA: 0x000557C1 File Offset: 0x000539C1
		public float AggroDistance
		{
			get
			{
				return this.aggroDistance;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x060010ED RID: 4333 RVA: 0x000557CC File Offset: 0x000539CC
		public bool HasPlayerTargets
		{
			get
			{
				Func<IEnumerable<ICombatEntity>> func = this.playerTargets;
				bool? flag;
				if (func == null)
				{
					flag = null;
				}
				else
				{
					IEnumerable<ICombatEntity> enumerable = func();
					flag = ((enumerable != null) ? new bool?(enumerable.Any<ICombatEntity>()) : null);
				}
				bool? flag2 = flag;
				return flag2.GetValueOrDefault();
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x060010EE RID: 4334 RVA: 0x00055814 File Offset: 0x00053A14
		public ICombatEntity ClosestAggroTarget
		{
			get
			{
				return this.closestAggroTarget;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x060010EF RID: 4335 RVA: 0x0005581C File Offset: 0x00053A1C
		public ICombatEntity ClosestTarget
		{
			get
			{
				return this.closestTarget;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x060010F0 RID: 4336 RVA: 0x00055824 File Offset: 0x00053A24
		public ICombatEntity NearestTargetOnLine
		{
			get
			{
				return this.nearestTargetOnLine;
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x060010F1 RID: 4337 RVA: 0x0005582C File Offset: 0x00053A2C
		public ICombatEntity NearestTargetOnLineDirectVisible
		{
			get
			{
				return this.nearestTargetOnLineDirectVisible;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x060010F2 RID: 4338 RVA: 0x00055834 File Offset: 0x00053A34
		public FightingCapturePoint NearestCapturePoint
		{
			get
			{
				return this.nearestCapturePoint;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x060010F3 RID: 4339 RVA: 0x0005583C File Offset: 0x00053A3C
		public bool IsAgentOnNearestCapturePoint
		{
			get
			{
				return this.isAgentOnNearestCapturePoint;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x060010F4 RID: 4340 RVA: 0x00055844 File Offset: 0x00053A44
		public bool IsRangedAttacker
		{
			get
			{
				return this.isRangedAttacker;
			}
		}

		// Token: 0x040012FB RID: 4859
		private readonly EnemyDefaultAI owner;

		// Token: 0x040012FC RID: 4860
		private readonly FightingAgent agent;

		// Token: 0x040012FD RID: 4861
		private readonly Func<IEnumerable<ICombatEntity>> playerTargets;

		// Token: 0x040012FE RID: 4862
		private readonly float aggroDistance;

		// Token: 0x040012FF RID: 4863
		private readonly ICombatEntity closestAggroTarget;

		// Token: 0x04001300 RID: 4864
		private readonly ICombatEntity closestTarget;

		// Token: 0x04001301 RID: 4865
		private readonly ICombatEntity nearestTargetOnLine;

		// Token: 0x04001302 RID: 4866
		private readonly ICombatEntity nearestTargetOnLineDirectVisible;

		// Token: 0x04001303 RID: 4867
		private readonly FightingCapturePoint nearestCapturePoint;

		// Token: 0x04001304 RID: 4868
		private readonly bool isAgentOnNearestCapturePoint;

		// Token: 0x04001305 RID: 4869
		private readonly bool isRangedAttacker;
	}
}
