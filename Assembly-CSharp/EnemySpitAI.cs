using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x02000299 RID: 665
[CreateAssetMenu(menuName = "GK2/Fighting/AIs/EnemySpit")]
public class EnemySpitAI : EnemyDefaultAI
{
	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x06001114 RID: 4372 RVA: 0x00056656 File Offset: 0x00054856
	protected override List<IEnemyDecisionStep> DefaultDecisionSteps
	{
		get
		{
			return new List<IEnemyDecisionStep>
			{
				new EnemySpitAI.SpitEngagementDecisionStep(),
				new EnemyDefaultAI.CapturePointDecisionStep()
			};
		}
	}

	// Token: 0x06001115 RID: 4373 RVA: 0x00056673 File Offset: 0x00054873
	public override MobCommand GetCommand(FightingAgent agent, Func<IEnumerable<ICombatEntity>> targets)
	{
		return base.GetCommand(agent, targets);
	}

	// Token: 0x06001116 RID: 4374 RVA: 0x00056680 File Offset: 0x00054880
	[CanBeNull]
	private MobCommand TryGetSpitEngagementCommand(EnemyDefaultAI.EnemyDecisionContext context)
	{
		if (!context.HasPlayerTargets)
		{
			this.ClearLockedTarget(context.Agent);
			return null;
		}
		float attackRange = EnemySpitAI.GetAttackRange(context.Agent);
		ICombatEntity combatEntity = this.ResolveEngagementTarget(context, attackRange);
		if (combatEntity == null)
		{
			this.ClearLockedTarget(context.Agent);
			return null;
		}
		if (this.IsTargetAttackable(context.Agent, combatEntity, attackRange))
		{
			this.LockTarget(context.Agent, combatEntity);
			return this.CreateSpitAttackCommand(context, combatEntity, attackRange);
		}
		this.ClearLockedTarget(context.Agent);
		return this.CreateApproachCommand(context, combatEntity, attackRange);
	}

	// Token: 0x06001117 RID: 4375 RVA: 0x0005670C File Offset: 0x0005490C
	[CanBeNull]
	private MobCommand CreateSpitAttackCommand(EnemyDefaultAI.EnemyDecisionContext context, ICombatEntity entity, float attackRange)
	{
		FightingAgent agent = context.Agent;
		Func<bool> func = () => !this.IsTargetAttackable(agent, entity, EnemySpitAI.GetAttackRange(agent));
		return new ZombieSpitAttackCommand(attackRange).WithDebugLogs(this.debugSpitAI).WithCustomStopCondition(func).ToTarget(entity);
	}

	// Token: 0x06001118 RID: 4376 RVA: 0x0005676C File Offset: 0x0005496C
	[CanBeNull]
	private MobCommand CreateApproachCommand(EnemyDefaultAI.EnemyDecisionContext context, ICombatEntity entity, float attackRange)
	{
		FightingAgent agent = context.Agent;
		if (!this.IsTargetAliveAndValid(agent, entity))
		{
			return null;
		}
		Func<bool> func = this.CreateApproachStopCondition(context, entity);
		Wgo wgo = entity as Wgo;
		base.ReleaseTakenDockPointIfDifferent(agent, wgo);
		DockPointData dockPoint = null;
		float num;
		if (wgo && base.TryGetDockPoint(agent, wgo, out dockPoint, DockPointData.Availability.OnlyNotOccupied, DockPointData.Filter.All, false, null))
		{
			num = base.DistToPos(agent, dockPoint.GetPosFrom(wgo.Data.Position));
			if (this.IsWithinAttackRange(agent, entity, attackRange))
			{
				if (!EnemySpitAI.HasDirectVisionToTarget(agent, entity))
				{
					return this.CreateGoToCommand(agent, entity, dockPoint, 0.06666668f, func);
				}
				return null;
			}
			else if ((num - 0.06666668f).More(0f, 0.0001f))
			{
				return new MobCommandGoTo(new CombatEntityDestinationModifier(dockPoint)).WithCustomTargetDestinationOffset(0.06666668f).WithCustomStopCondition(func, this.retargetDeltaTime, global::UnityEngine.Random.Range(0f, this.retargetDeltaTime)).WithCustomActionOnDestReached(delegate
				{
					agent.SetFacingDirection(dockPoint.Direction.ConvertToVector2XZ(), true);
					EnemySpitAI.TryAnchorAtTakenDockPoint(agent);
				})
					.ToTarget(wgo);
			}
		}
		num = base.DistToPos(agent, entity.CombatEntityPosition);
		float num2 = ((dockPoint != null) ? 0.06666668f : attackRange);
		DockPointData dockPointData;
		float num3;
		if (dockPoint == null && wgo != null && base.TryGetOverflowDockRing(agent, wgo, out dockPointData, out num3))
		{
			num2 = num3;
		}
		if (!EnemySpitAI.HasDirectVisionToTarget(agent, entity) && this.IsWithinAttackRange(agent, entity, attackRange))
		{
			num2 = 0.06666668f;
		}
		if ((num - num2).More(0f, 0.0001f))
		{
			return this.CreateGoToCommand(agent, entity, dockPoint, num2, func);
		}
		return null;
	}

	// Token: 0x06001119 RID: 4377 RVA: 0x0005694C File Offset: 0x00054B4C
	private MobCommand CreateGoToCommand(FightingAgent agent, ICombatEntity entity, DockPointData dockPoint, float destinationOffset, Func<bool> goToStopCondition)
	{
		return new MobCommandGoTo(new CombatEntityDestinationModifier(dockPoint)).WithCustomTargetDestinationOffset(destinationOffset).WithCustomStopCondition(goToStopCondition, this.retargetDeltaTime, global::UnityEngine.Random.Range(0f, this.retargetDeltaTime)).WithCustomActionOnDestReached(delegate
		{
			EnemySpitAI.TryAnchorAtTakenDockPoint(agent);
		})
			.ToTarget(entity);
	}

	// Token: 0x0600111A RID: 4378 RVA: 0x000569AC File Offset: 0x00054BAC
	private Func<bool> CreateApproachStopCondition(EnemyDefaultAI.EnemyDecisionContext context, ICombatEntity chosenEntity)
	{
		return delegate
		{
			FightingAgent agent = context.Agent;
			if (!this.IsTargetAliveAndValid(agent, chosenEntity))
			{
				return true;
			}
			float attackRange = EnemySpitAI.GetAttackRange(agent);
			if (this.IsTargetAttackable(agent, chosenEntity, attackRange))
			{
				return true;
			}
			ICombatEntity combatEntity = this.FindClosestApproachTarget(context);
			return (combatEntity != null && combatEntity.CombatEntityUID != chosenEntity.CombatEntityUID) || this.ShouldRetargetDockAssignment(agent, chosenEntity);
		};
	}

	// Token: 0x0600111B RID: 4379 RVA: 0x000569D4 File Offset: 0x00054BD4
	[CanBeNull]
	private ICombatEntity ResolveEngagementTarget(EnemyDefaultAI.EnemyDecisionContext context, float attackRange)
	{
		FightingAgent agent = context.Agent;
		SGuid uniqueId = agent.Wgo.Data.UniqueId;
		SGuid sguid;
		if (this.lockedTargetByAgent.TryGetValue(uniqueId, out sguid) && !SGuid.IsNullOrEmpty(sguid))
		{
			ICombatEntity combatEntity = EnemySpitAI.FindTargetById(context.PlayerTargets, sguid);
			if (this.IsTargetAttackable(agent, combatEntity, attackRange))
			{
				return combatEntity;
			}
			this.lockedTargetByAgent.Remove(uniqueId);
		}
		ICombatEntity combatEntity2 = this.FindClosestAttackableTarget(context, attackRange);
		if (combatEntity2 != null)
		{
			return combatEntity2;
		}
		return this.FindClosestApproachTarget(context);
	}

	// Token: 0x0600111C RID: 4380 RVA: 0x00056A54 File Offset: 0x00054C54
	[CanBeNull]
	private ICombatEntity FindClosestAttackableTarget(EnemyDefaultAI.EnemyDecisionContext context, float attackRange)
	{
		FightingAgent agent = context.Agent;
		Func<IEnumerable<ICombatEntity>> playerTargets = context.PlayerTargets;
		IEnumerable<ICombatEntity> enumerable = ((playerTargets != null) ? playerTargets() : null);
		if (enumerable == null)
		{
			return null;
		}
		ICombatEntity combatEntity = null;
		float num = float.MaxValue;
		int num2 = -1;
		foreach (ICombatEntity combatEntity2 in enumerable)
		{
			if (this.IsTargetAttackable(agent, combatEntity2, attackRange))
			{
				float num3 = base.DistToPos(agent, combatEntity2.CombatEntityPosition);
				int attackPriority = combatEntity2.AttackPriority;
				if (attackPriority > num2 || (attackPriority == num2 && num3 < num))
				{
					combatEntity = combatEntity2;
					num = num3;
					num2 = attackPriority;
				}
			}
		}
		return combatEntity;
	}

	// Token: 0x0600111D RID: 4381 RVA: 0x00056B08 File Offset: 0x00054D08
	[CanBeNull]
	private ICombatEntity FindClosestApproachTarget(EnemyDefaultAI.EnemyDecisionContext context)
	{
		FightingAgent agent = context.Agent;
		if (this.IsTargetAliveAndValid(agent, context.ClosestAggroTarget))
		{
			return context.ClosestAggroTarget;
		}
		if (context.IsAgentOnNearestCapturePoint)
		{
			return null;
		}
		ICombatEntity closestTarget = context.ClosestTarget;
		if (!this.IsTargetAliveAndValid(agent, closestTarget))
		{
			return null;
		}
		if (base.DistToPos(agent, closestTarget.CombatEntityPosition) > this.deAggroDistance)
		{
			return null;
		}
		return closestTarget;
	}

	// Token: 0x0600111E RID: 4382 RVA: 0x00056B6C File Offset: 0x00054D6C
	private bool IsTargetAttackable(FightingAgent agent, ICombatEntity entity, float attackRange)
	{
		return this.IsTargetAliveAndValid(agent, entity) && this.IsWithinAttackRange(agent, entity, attackRange) && (EnemySpitAI.IsBarricadeTarget(entity) || EnemySpitAI.HasDirectVisionToTarget(agent, entity));
	}

	// Token: 0x0600111F RID: 4383 RVA: 0x00056B98 File Offset: 0x00054D98
	private static bool IsBarricadeTarget(ICombatEntity entity)
	{
		return FightingWgoTarget.IsBarricadeOrTower(entity);
	}

	// Token: 0x06001120 RID: 4384 RVA: 0x00056BA0 File Offset: 0x00054DA0
	private bool IsWithinAttackRange(FightingAgent agent, ICombatEntity entity, float attackRange)
	{
		return base.DistToPos(agent, entity.CombatEntityPosition) <= attackRange + 0.06666668f;
	}

	// Token: 0x06001121 RID: 4385 RVA: 0x00056BBB File Offset: 0x00054DBB
	private bool IsTargetAliveAndValid(FightingAgent agent, ICombatEntity entity)
	{
		return this.IsTargetValid(entity, agent) && entity.CombatEntityHpComponent != null && entity.CombatEntityHpComponent.Hp > 0;
	}

	// Token: 0x06001122 RID: 4386 RVA: 0x00056BE1 File Offset: 0x00054DE1
	private static float GetAttackRange(FightingAgent agent)
	{
		bool flag;
		if (agent == null)
		{
			flag = null != null;
		}
		else
		{
			FighterDef fighterDef = agent.FighterDef;
			flag = ((fighterDef != null) ? fighterDef.atkRange : null) != null;
		}
		if (!flag)
		{
			return 0f;
		}
		return agent.FighterDef.atkRange.EvaluateFloat(agent.Wgo);
	}

	// Token: 0x06001123 RID: 4387 RVA: 0x00056C1C File Offset: 0x00054E1C
	[CanBeNull]
	private static ICombatEntity FindTargetById(Func<IEnumerable<ICombatEntity>> targets, SGuid targetId)
	{
		if (targets == null || SGuid.IsNullOrEmpty(targetId))
		{
			return null;
		}
		IEnumerable<ICombatEntity> enumerable = targets();
		if (enumerable == null)
		{
			return null;
		}
		foreach (ICombatEntity combatEntity in enumerable)
		{
			if (combatEntity != null && combatEntity.CombatEntityUID == targetId)
			{
				return combatEntity;
			}
		}
		return null;
	}

	// Token: 0x06001124 RID: 4388 RVA: 0x00056C90 File Offset: 0x00054E90
	private void LockTarget(FightingAgent agent, ICombatEntity entity)
	{
		if (((agent != null) ? agent.Wgo : null) == null || entity == null)
		{
			return;
		}
		this.lockedTargetByAgent[agent.Wgo.Data.UniqueId] = entity.CombatEntityUID;
	}

	// Token: 0x06001125 RID: 4389 RVA: 0x00056CCB File Offset: 0x00054ECB
	private void ClearLockedTarget(FightingAgent agent)
	{
		if (((agent != null) ? agent.Wgo : null) == null)
		{
			return;
		}
		this.lockedTargetByAgent.Remove(agent.Wgo.Data.UniqueId);
	}

	// Token: 0x06001126 RID: 4390 RVA: 0x00056CFE File Offset: 0x00054EFE
	private static bool HasDirectVisionToTarget(FightingAgent agent, ICombatEntity entity)
	{
		return AgentAI.TryLineCastByRecast(EnemySpitAI.GetLinecastOrigin(agent), entity.CombatEntityPosition + Vector3.up * 0.5f);
	}

	// Token: 0x06001127 RID: 4391 RVA: 0x00056D28 File Offset: 0x00054F28
	private static Vector3 GetLinecastOrigin(FightingAgent agent)
	{
		AttackComponent attackComponent = agent.AttackComponent;
		if ((attackComponent != null) ? attackComponent.weapon : null)
		{
			return agent.AttackComponent.weapon.transform.position;
		}
		return agent.Wgo.Data.Position + Vector3.up * 0.5f;
	}

	// Token: 0x06001128 RID: 4392 RVA: 0x00055224 File Offset: 0x00053424
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

	// Token: 0x04001330 RID: 4912
	private const float TINY_EPSILON = 0.0001f;

	// Token: 0x04001331 RID: 4913
	[Header("Debug")]
	[SerializeField]
	private bool debugSpitAI;

	// Token: 0x04001332 RID: 4914
	private readonly Dictionary<SGuid, SGuid> lockedTargetByAgent = new Dictionary<SGuid, SGuid>();

	// Token: 0x0200029A RID: 666
	[Serializable]
	private sealed class SpitEngagementDecisionStep : IEnemyDecisionStep, IAgentDecisionStep<EnemyDefaultAI.EnemyDecisionContext>
	{
		// Token: 0x0600112A RID: 4394 RVA: 0x00056D9C File Offset: 0x00054F9C
		public MobCommand TryCreateCommand(EnemyDefaultAI.EnemyDecisionContext context)
		{
			EnemySpitAI enemySpitAI = context.Owner as EnemySpitAI;
			if (enemySpitAI == null)
			{
				return null;
			}
			return enemySpitAI.TryGetSpitEngagementCommand(context);
		}
	}
}
