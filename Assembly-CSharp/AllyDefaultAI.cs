using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

// Token: 0x0200027C RID: 636
[CreateAssetMenu(menuName = "GK2/Fighting/AIs/AllyDefault")]
public class AllyDefaultAI : AgentAI
{
	// Token: 0x0600107C RID: 4220 RVA: 0x00052FEC File Offset: 0x000511EC
	private float DistToEnemy(FightingAgent agent, ICombatEntity enemy)
	{
		return (agent.Wgo.Data.Position - enemy.CombatEntityPosition).XZ().magnitude;
	}

	// Token: 0x0600107D RID: 4221 RVA: 0x00053024 File Offset: 0x00051224
	private bool TryDistToFlag(FightingAgent agent, out float dist)
	{
		dist = 0f;
		if (!agent.FlagController || !agent.FlagController.FlagWgo)
		{
			return false;
		}
		dist = (agent.FlagController.FlagWgo.Data.Position - agent.Wgo.Data.Position).XZ().magnitude;
		return true;
	}

	// Token: 0x0600107E RID: 4222 RVA: 0x00053094 File Offset: 0x00051294
	private bool IsAgentInsideRangeFlag(FightingAgent agent)
	{
		float num;
		return this.TryDistToFlag(agent, out num) && num < this.followDistance;
	}

	// Token: 0x0600107F RID: 4223 RVA: 0x000530B8 File Offset: 0x000512B8
	private bool HasEnemyInAttackRange(AllyDefaultAI.AllyDecisionContext context)
	{
		for (int i = 0; i < context.EnemyTargets.Count; i++)
		{
			if (this.DistToEnemy(context.Agent, context.EnemyTargets[i]) < context.AttackRange)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001080 RID: 4224 RVA: 0x00053104 File Offset: 0x00051304
	public override MobCommand GetCommand(FightingAgent agent, Func<IEnumerable<ICombatEntity>> targets)
	{
		AgentsGroupFlagController flagController = agent.FlagController;
		if (!flagController)
		{
			Debug.LogError("FlagController is not set, but should", this);
			return null;
		}
		AllyDefaultAI.AllyDecisionContext allyDecisionContext = new AllyDefaultAI.AllyDecisionContext(this, agent, this.WrapPotentialTargets(agent, targets), flagController, this.followDistance, this.attackDistance, this.aggroDistance, this.deAggroDistance);
		IReadOnlyList<IAllyDecisionStep> activeDecisionSteps = this.GetActiveDecisionSteps();
		for (int i = 0; i < activeDecisionSteps.Count; i++)
		{
			IAllyDecisionStep allyDecisionStep = activeDecisionSteps[i];
			if (allyDecisionStep != null)
			{
				MobCommand mobCommand = allyDecisionStep.TryCreateCommand(allyDecisionContext);
				if (mobCommand != null)
				{
					return mobCommand;
				}
			}
		}
		return null;
	}

	// Token: 0x06001081 RID: 4225 RVA: 0x00053190 File Offset: 0x00051390
	private IReadOnlyList<IAllyDecisionStep> GetActiveDecisionSteps()
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
			this.defaultDecisionSteps = new List<IAllyDecisionStep>
			{
				new AllyDefaultAI.CombatDecisionStep(),
				new AllyDefaultAI.CapturePointDecisionStep(),
				new AllyDefaultAI.FlagDockingDecisionStep(),
				new AllyDefaultAI.FlagFollowDecisionStep()
			};
		}
		return this.defaultDecisionSteps;
	}

	// Token: 0x06001082 RID: 4226 RVA: 0x00053234 File Offset: 0x00051434
	private MobCommand TryGetCombatCommand(AllyDefaultAI.AllyDecisionContext context)
	{
		if (context.Agent.IsUnderMainHeroPush)
		{
			return null;
		}
		FightingCapturePoint nearestAllyCapturePoint = this.GetNearestAllyCapturePoint(context.Agent);
		ICombatEntity combatEntity = ((nearestAllyCapturePoint != null) ? AllyDefaultAI.GetClosestEnemyOnCapturePoint(nearestAllyCapturePoint, context.Agent) : null);
		bool flag = combatEntity != null && this.DistToEnemy(context.Agent, combatEntity) <= context.AggroDistance;
		ICombatEntity combatEntity2 = (flag ? combatEntity : null);
		if (combatEntity2 == null)
		{
			if (!context.HasEnemyTargets)
			{
				return null;
			}
			combatEntity2 = AgentAI.GetClosestTarget(context.Agent.Wgo.Data.Position, context.EnemyTargets, context.Agent.Wgo.TeamType, context.AggroDistance, context.IsRangedAttacker);
		}
		if (combatEntity2 == null)
		{
			return null;
		}
		if (context.FlagController.IsSetAtPoint)
		{
			if (!context.Agent.IsAnchoredAtDockPoint)
			{
				return null;
			}
			return this.TryAttackEnemy(context, combatEntity2, flag);
		}
		else
		{
			if (!flag)
			{
				MobCommand mobCommand = this.TryKeepAgentCloseToFlag(context, combatEntity2);
				if (mobCommand != null)
				{
					return mobCommand;
				}
				MobCommand mobCommand2 = this.TryKiteAwayFromEnemy(context, combatEntity2, flag);
				if (mobCommand2 != null)
				{
					return mobCommand2;
				}
			}
			MobCommand mobCommand3 = this.TryChaseEnemy(context, combatEntity2, flag);
			if (mobCommand3 != null)
			{
				return mobCommand3;
			}
			return this.TryAttackEnemy(context, combatEntity2, flag);
		}
	}

	// Token: 0x06001083 RID: 4227 RVA: 0x00053360 File Offset: 0x00051560
	private MobCommand TryKeepAgentCloseToFlag(AllyDefaultAI.AllyDecisionContext context, ICombatEntity enemy = null)
	{
		if (context.Agent.IsUnderMainHeroPush)
		{
			return null;
		}
		if (!context.HasValidFlag)
		{
			return null;
		}
		bool flag = this.IsPikeman(context.Agent);
		if (enemy != null && this.pikemenHoldFrontline && flag)
		{
			if (AllyDefaultAI.CanPikeStrike(context.Agent, enemy))
			{
				return null;
			}
			if (this.DistToEnemy(context.Agent, enemy) <= context.AttackRange)
			{
				return null;
			}
			Vector3 flagEdgePositionTowards = this.GetFlagEdgePositionTowards(context, enemy.CombatEntityPosition);
			if (this.DistToPos(context.Agent, flagEdgePositionTowards) <= 0.4f)
			{
				return null;
			}
			return new MobCommandGoTo(new CombatEntityDestinationModifier(null)).ToPosition(flagEdgePositionTowards);
		}
		else
		{
			if (this.IsAgentInsideRangeFlag(context.Agent))
			{
				return null;
			}
			return new MobCommandGoTo(new CombatEntityDestinationModifier(null)).WithCustomTargetDestinationOffset(context.FollowDistance).ToTarget(context.FlagController.FlagWgo);
		}
	}

	// Token: 0x06001084 RID: 4228 RVA: 0x00053440 File Offset: 0x00051640
	private Vector3 GetFlagEdgePositionTowards(AllyDefaultAI.AllyDecisionContext context, Vector3 towardsPosition)
	{
		Vector3 position = context.FlagController.FlagWgo.Data.Position;
		Vector2 vector = (towardsPosition - position).XZ2();
		if (vector.sqrMagnitude <= 0.0001f)
		{
			return position;
		}
		return position + vector.normalized.XZ() * context.FollowDistance;
	}

	// Token: 0x06001085 RID: 4229 RVA: 0x0005349F File Offset: 0x0005169F
	private bool IsPikeman(FightingAgent agent)
	{
		return agent.AttackCommandType == MobCommand.CommandType.ZombiePikeAttack;
	}

	// Token: 0x06001086 RID: 4230 RVA: 0x000534AA File Offset: 0x000516AA
	private bool IsArcher(FightingAgent agent)
	{
		return agent.AttackCommandType == MobCommand.CommandType.ZombieBowAttack;
	}

	// Token: 0x06001087 RID: 4231 RVA: 0x000534B5 File Offset: 0x000516B5
	private static bool CanPikeStrike(FightingAgent agent, ICombatEntity enemy)
	{
		return enemy != null && PikeCombatGeometry.IsAlignedForStrike(agent, enemy.CombatEntityPosition, 0.4f);
	}

	// Token: 0x06001088 RID: 4232 RVA: 0x000534D0 File Offset: 0x000516D0
	private MobCommand TryKiteAwayFromEnemy(AllyDefaultAI.AllyDecisionContext context, ICombatEntity enemy, bool defendCapturePoint)
	{
		if (context.Agent.IsUnderMainHeroPush)
		{
			return null;
		}
		if (this.archerKiteDistance <= 0f || !this.IsArcher(context.Agent))
		{
			return null;
		}
		if (!context.Agent.CanReposition)
		{
			return null;
		}
		float num = this.DistToEnemy(context.Agent, enemy);
		if (num >= this.archerKiteDistance)
		{
			return null;
		}
		Vector3 position = context.Agent.Wgo.Data.Position;
		Vector2 vector = (position - enemy.CombatEntityPosition).XZ2();
		if (vector.sqrMagnitude <= 0.0001f)
		{
			return null;
		}
		Vector2 normalized = vector.normalized;
		float num2 = this.archerKiteDistance - num;
		Vector3 combatEntityPosition = enemy.CombatEntityPosition;
		Vector3 vector2 = default(Vector3);
		float num3 = float.NegativeInfinity;
		bool flag = false;
		for (int i = 0; i < AllyDefaultAI.ArcherKiteYawOffsetsDeg.Length; i++)
		{
			Vector2 vector3 = AllyDefaultAI.RotateXZ(normalized, AllyDefaultAI.ArcherKiteYawOffsetsDeg[i]);
			Vector3 vector4 = AllyDefaultAI.ClampToFlagFollowRadius(context, position + vector3.XZ() * num2);
			Vector3 vector5;
			GraphNode graphNode;
			if (this.TryProjectKitePointOntoGraph(context.Agent, vector4, out vector5, out graphNode))
			{
				vector4 = vector5;
				float num4 = this.DistToPos(context.Agent, vector4);
				if (num4 >= 0.75f)
				{
					float magnitude = (vector4 - combatEntityPosition).XZ().magnitude;
					if (magnitude >= num + 0.35f && !this.IsArcherKiteSlotOccupied(context, vector4) && this.IsKitePointReachable(context.Agent, graphNode))
					{
						float num5 = magnitude * 10f - num4;
						if (num5 > num3)
						{
							num3 = num5;
							vector2 = vector4;
							flag = true;
						}
					}
				}
			}
		}
		if (!flag)
		{
			return null;
		}
		context.Agent.ConsumeReposition(this.archerMaxRepositions, this.archerRepositionCooldown);
		Vector3 retreatPosition = vector2;
		return new MobCommandGoTo(new CombatEntityDestinationModifier(null)).WithCustomStopCondition(() => this.DistToEnemy(context.Agent, enemy) >= this.archerKiteDistance || this.DistToPos(context.Agent, retreatPosition) <= 0.75f || this.ShouldStopFlagBoundCombat(context, defendCapturePoint), -1f, 0f).ToPosition(retreatPosition);
	}

	// Token: 0x06001089 RID: 4233 RVA: 0x00053730 File Offset: 0x00051930
	private static Vector3 ClampToFlagFollowRadius(AllyDefaultAI.AllyDecisionContext context, Vector3 position)
	{
		if (!context.HasValidFlag)
		{
			return position;
		}
		Vector3 position2 = context.FlagController.FlagWgo.Data.Position;
		Vector2 vector = (position - position2).XZ2();
		if (vector.magnitude <= context.FollowDistance)
		{
			return position;
		}
		return position2 + vector.normalized.XZ() * context.FollowDistance;
	}

	// Token: 0x0600108A RID: 4234 RVA: 0x0005379C File Offset: 0x0005199C
	private static Vector2 RotateXZ(Vector2 v, float degrees)
	{
		float num = degrees * 0.017453292f;
		float num2 = Mathf.Cos(num);
		float num3 = Mathf.Sin(num);
		return new Vector2(v.x * num2 - v.y * num3, v.x * num3 + v.y * num2);
	}

	// Token: 0x0600108B RID: 4235 RVA: 0x000537E4 File Offset: 0x000519E4
	private bool TryProjectKitePointOntoGraph(FightingAgent agent, Vector3 candidate, out Vector3 onGraph, out GraphNode node)
	{
		onGraph = default(Vector3);
		node = null;
		FightingGameController fightingGameController = base.FightingGameController;
		RecastGraph recastGraph = ((fightingGameController != null) ? fightingGameController.RecastGraph : null);
		if (recastGraph == null || AstarPath.active == null)
		{
			return false;
		}
		NearestNodeConstraint walkable = NearestNodeConstraint.Walkable;
		walkable.graphMask = GraphMask.FromGraph(recastGraph);
		walkable.distanceMetric = DistanceMetric.ClosestAsSeenFromAbove();
		NNInfo nearest = recastGraph.GetNearest(candidate, walkable);
		if (nearest.node == null || !nearest.node.Walkable)
		{
			return false;
		}
		if ((nearest.position - candidate).XZ().sqrMagnitude > 0.25f)
		{
			return false;
		}
		onGraph = nearest.position;
		node = nearest.node;
		if (agent.FlagController && agent.FlagController.FlagWgo)
		{
			Vector3 position = agent.FlagController.FlagWgo.Data.Position;
			Vector2 vector = (onGraph - position).XZ2();
			if (vector.magnitude > this.followDistance)
			{
				Vector3 vector2 = position + vector.normalized.XZ() * this.followDistance;
				NNInfo nearest2 = recastGraph.GetNearest(vector2, walkable);
				if (nearest2.node == null || !nearest2.node.Walkable)
				{
					return false;
				}
				if ((nearest2.position - vector2).XZ().sqrMagnitude > 0.25f)
				{
					return false;
				}
				onGraph = nearest2.position;
				node = nearest2.node;
			}
		}
		return true;
	}

	// Token: 0x0600108C RID: 4236 RVA: 0x00053978 File Offset: 0x00051B78
	private bool IsKitePointReachable(FightingAgent agent, GraphNode destNode)
	{
		if (destNode == null || AstarPath.active == null)
		{
			return false;
		}
		FightingGameController fightingGameController = base.FightingGameController;
		RecastGraph recastGraph = ((fightingGameController != null) ? fightingGameController.RecastGraph : null);
		if (recastGraph == null)
		{
			return false;
		}
		NearestNodeConstraint walkable = NearestNodeConstraint.Walkable;
		walkable.graphMask = GraphMask.FromGraph(recastGraph);
		walkable.distanceMetric = DistanceMetric.ClosestAsSeenFromAbove();
		NNInfo nearest = recastGraph.GetNearest(agent.Wgo.Data.Position, walkable);
		return nearest.node != null && nearest.node.Walkable && PathUtilities.IsPathPossible(nearest.node, destNode);
	}

	// Token: 0x0600108D RID: 4237 RVA: 0x00053A0C File Offset: 0x00051C0C
	private bool IsArcherKiteSlotOccupied(AllyDefaultAI.AllyDecisionContext context, Vector3 candidate)
	{
		AgentsGroupFlagController flagController = context.FlagController;
		IReadOnlyList<FightingAgent> readOnlyList;
		if (flagController == null)
		{
			readOnlyList = null;
		}
		else
		{
			AgentsGroupBehaviourController agentsController = flagController.AgentsController;
			readOnlyList = ((agentsController != null) ? agentsController.Agents : null);
		}
		IReadOnlyList<FightingAgent> readOnlyList2 = readOnlyList;
		if (readOnlyList2 == null)
		{
			return false;
		}
		SGuid uniqueId = context.Agent.Wgo.Data.UniqueId;
		float num = 0.80999994f;
		float num2 = 0.45562494f;
		for (int i = 0; i < readOnlyList2.Count; i++)
		{
			FightingAgent fightingAgent = readOnlyList2[i];
			if (!(fightingAgent == null) && !(fightingAgent.Wgo == null) && !(fightingAgent.Wgo.Data.UniqueId == uniqueId) && this.IsArcher(fightingAgent))
			{
				MobCommandGoTo mobCommandGoTo = fightingAgent.MobCommand as MobCommandGoTo;
				if (mobCommandGoTo != null && mobCommandGoTo.DestinationModifier != null && (mobCommandGoTo.DestinationModifier.GetCurrentTargetPosition() - candidate).XZ().sqrMagnitude < num)
				{
					return true;
				}
				if ((fightingAgent.Wgo.Data.Position - candidate).XZ().sqrMagnitude < num2)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600108E RID: 4238 RVA: 0x00053B34 File Offset: 0x00051D34
	private MobCommand TryChaseEnemy(AllyDefaultAI.AllyDecisionContext context, ICombatEntity enemy, bool defendCapturePoint = false)
	{
		float num = this.DistToEnemy(context.Agent, enemy);
		if (this.IsPikeman(context.Agent))
		{
			if (AllyDefaultAI.CanPikeStrike(context.Agent, enemy))
			{
				return null;
			}
		}
		else if (num <= context.AttackRange)
		{
			return null;
		}
		if (!defendCapturePoint && !this.IsAgentInsideRangeFlag(context.Agent))
		{
			return null;
		}
		if (!defendCapturePoint && this.IsAgentInsideEpsilonZoneFlag(context.Agent) && this.IsEnemyOutwardBeyondFlagRim(context, enemy))
		{
			return null;
		}
		GoToDestinationModifier goToDestinationModifier = context.Agent.GoToDestinationModifier ?? new CombatEntityDestinationModifier(null);
		float num2 = ((goToDestinationModifier.CustomDestinationOffset > 0f) ? goToDestinationModifier.CustomDestinationOffset : context.AttackRange);
		return new MobCommandGoTo(goToDestinationModifier).WithCustomTargetDestinationOffset(num2).WithCustomStopCondition(() => this.ShouldStopFlagBoundCombat(context, defendCapturePoint), -1f, 0f).ToTarget(enemy);
	}

	// Token: 0x0600108F RID: 4239 RVA: 0x00053C5C File Offset: 0x00051E5C
	private bool IsEnemyOutwardBeyondFlagRim(AllyDefaultAI.AllyDecisionContext context, ICombatEntity enemy)
	{
		if (!context.HasValidFlag)
		{
			return true;
		}
		Vector3 position = context.FlagController.FlagWgo.Data.Position;
		Vector2 vector = (context.Agent.Wgo.Data.Position - position).XZ2();
		Vector2 vector2 = (enemy.CombatEntityPosition - position).XZ2();
		return vector2.magnitude >= context.FollowDistance - 0.4f && (vector.sqrMagnitude <= 0.0001f || vector2.sqrMagnitude <= 0.0001f || Vector2.Dot(vector.normalized, vector2.normalized) > 0.25f);
	}

	// Token: 0x06001090 RID: 4240 RVA: 0x00053D10 File Offset: 0x00051F10
	private MobCommand TryAttackEnemy(AllyDefaultAI.AllyDecisionContext context, ICombatEntity enemy, bool defendCapturePoint = false)
	{
		if (this.DistToEnemy(context.Agent, enemy) >= context.AttackRange)
		{
			return null;
		}
		switch (context.Agent.AttackCommandType)
		{
		case MobCommand.CommandType.ZombieMeleeAttack:
			return new ZombieMeleeAttackCommand(context.AttackRange).WithCustomStopCondition(() => this.ShouldStopFlagBoundCombat(context, defendCapturePoint)).ToTarget(enemy);
		case MobCommand.CommandType.ZombieBowAttack:
			return new ZombieBowAttackCommand().WithCustomStopCondition(() => this.ShouldStopFlagBoundCombat(context, defendCapturePoint)).ToTarget(enemy);
		case MobCommand.CommandType.ZombiePikeAttack:
			if (!AllyDefaultAI.CanPikeStrike(context.Agent, enemy))
			{
				return null;
			}
			return new ZombiePikeAttackCommand().WithCustomStopCondition(() => this.ShouldStopFlagBoundCombat(context, defendCapturePoint)).ToTarget(enemy);
		default:
			return null;
		}
	}

	// Token: 0x06001091 RID: 4241 RVA: 0x00053DF8 File Offset: 0x00051FF8
	private bool ShouldStopFlagBoundCombat(AllyDefaultAI.AllyDecisionContext context, bool defendCapturePoint = false)
	{
		if (!context.HasValidFlag)
		{
			return true;
		}
		if (defendCapturePoint)
		{
			FightingCapturePoint nearestAllyCapturePoint = this.GetNearestAllyCapturePoint(context.Agent);
			ICombatEntity combatEntity = ((nearestAllyCapturePoint != null) ? AllyDefaultAI.GetClosestEnemyOnCapturePoint(nearestAllyCapturePoint, context.Agent) : null);
			return combatEntity == null || this.DistToEnemy(context.Agent, combatEntity) > context.DeAggroDistance;
		}
		return !context.FlagController.IsSetAtPoint && !this.IsAgentInsideRangeFlag(context.Agent);
	}

	// Token: 0x06001092 RID: 4242 RVA: 0x00053E78 File Offset: 0x00052078
	private MobCommand TryGetFlagFollowCommand(AllyDefaultAI.AllyDecisionContext context)
	{
		if (context.Agent.IsUnderMainHeroPush)
		{
			return null;
		}
		if (!context.HasValidFlag)
		{
			return null;
		}
		if (context.FlagController.IsSetAtPoint)
		{
			return null;
		}
		if (this.HasEnemyInAttackRange(context))
		{
			return null;
		}
		float num;
		if (!this.TryDistToFlag(context.Agent, out num) || num <= context.FollowDistance)
		{
			return null;
		}
		return new MobCommandGoTo(new CombatEntityDestinationModifier(null)).WithCustomTargetDestinationOffset(context.FollowDistance).ToTarget(context.FlagController.FlagWgo);
	}

	// Token: 0x06001093 RID: 4243 RVA: 0x00053F00 File Offset: 0x00052100
	private MobCommand TryGetFlagDockingCommand(AllyDefaultAI.AllyDecisionContext context)
	{
		if (!context.FlagController || !context.FlagController.IsSetAtPoint)
		{
			return null;
		}
		if (context.Agent.IsUnderMainHeroPush)
		{
			return null;
		}
		if (context.Agent.IsAnchoredAtDockPoint)
		{
			return null;
		}
		return this.DoDockToFlagPlacedObjectLogic(context);
	}

	// Token: 0x06001094 RID: 4244 RVA: 0x00053F54 File Offset: 0x00052154
	private MobCommand DoDockToFlagPlacedObjectLogic(AllyDefaultAI.AllyDecisionContext context)
	{
		FightingAgent agent = context.Agent;
		Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(agent.FlagController.AttachedSGuid);
		DockPointTag agentTagToLookFor = ((agent.Settings.useDockPointPrioritizationByWeapon && agent.Weapon) ? agent.FighterDef.TargetFilterDockPointTag(agent.Wgo.Data) : DockPointTag.None);
		bool hasTagInFlagTargetObj = false;
		if (agentTagToLookFor != DockPointTag.None)
		{
			using (List<DockPointData>.Enumerator enumerator = wgoViewGlobal.Data.MainWgoPartData.GetDockPoints(DockPointData.Availability.OnlyNotOccupied, DockPointData.Filter.OnlyNotZombie).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.BakedData.DockPointTag == agentTagToLookFor)
					{
						hasTagInFlagTargetObj = true;
						break;
					}
				}
			}
		}
		DockPointData dockPoint;
		if (!wgoViewGlobal || !base.TryGetDockPoint(agent, wgoViewGlobal, out dockPoint, DockPointData.Availability.OnlyNotOccupied, DockPointData.Filter.OnlyNotZombie, false, (DockPointData data, Vector3 position) => this.AllyDockPointCheck(hasTagInFlagTargetObj ? agentTagToLookFor : DockPointTag.None, data, position)))
		{
			return null;
		}
		if (SGuid.IsNullOrEmpty(agent.Wgo.Data.takenDockPointsParentSGuid))
		{
			dockPoint.Occupy(agent.Wgo.Data.UniqueId);
			agent.Wgo.Data.takenDockPointsParentSGuid = wgoViewGlobal.Data.UniqueId;
			return new MobCommandGoTo(new CombatEntityDestinationModifier(dockPoint)).WithCustomTargetDestinationOffset(0.06666668f).WithCustomActionOnDestReached(delegate
			{
				AllyDefaultAI.OnAgentDocked(agent, dockPoint);
			}).ToTarget(wgoViewGlobal);
		}
		float magnitude = (wgoViewGlobal.Data.GetDockPointDataWorldPosition(dockPoint) - agent.Wgo.Data.Position).XZ().magnitude;
		if (magnitude < 0.06666668f || (magnitude - 0.06666668f).EqualsTo(0f, 0.0001f))
		{
			AllyDefaultAI.OnAgentDocked(agent, dockPoint);
			return null;
		}
		return new MobCommandGoTo(new CombatEntityDestinationModifier(dockPoint)).WithCustomTargetDestinationOffset(0.06666668f).WithCustomActionOnDestReached(delegate
		{
			AllyDefaultAI.OnAgentDocked(agent, dockPoint);
		}).ToTarget(wgoViewGlobal);
	}

	// Token: 0x06001095 RID: 4245 RVA: 0x000541B4 File Offset: 0x000523B4
	private static void OnAgentDocked(FightingAgent agent, DockPointData dockPoint)
	{
		agent.SetFacingDirection(dockPoint.Direction.ConvertToVector2XZ(), true);
		agent.IsAnchoredAtDockPoint = true;
		agent.SetNavmeshCutActive(true);
	}

	// Token: 0x06001096 RID: 4246 RVA: 0x000541D6 File Offset: 0x000523D6
	private bool AllyDockPointCheck(DockPointTag tag, DockPointData data, Vector3 parentPos)
	{
		return tag == DockPointTag.None || data.BakedData.DockPointTag == tag;
	}

	// Token: 0x06001097 RID: 4247 RVA: 0x000541EC File Offset: 0x000523EC
	private bool IsAgentInsideEpsilonZoneFlag(FightingAgent agent)
	{
		float num;
		return this.TryDistToFlag(agent, out num) && num < this.followDistance && num > this.followDistance - 0.4f;
	}

	// Token: 0x06001098 RID: 4248 RVA: 0x00054220 File Offset: 0x00052420
	private float DistToPos(FightingAgent agent, Vector3 pos)
	{
		return (agent.Wgo.Data.Position - pos).XZ().magnitude;
	}

	// Token: 0x06001099 RID: 4249 RVA: 0x00054250 File Offset: 0x00052450
	private bool IsOnCapturePoint(Vector3 pos, FightingCapturePoint point)
	{
		return point && point.IsOnCapturePoint(pos);
	}

	// Token: 0x0600109A RID: 4250 RVA: 0x00054264 File Offset: 0x00052464
	private FightingCapturePoint GetNearestAllyCapturePoint(FightingAgent agent)
	{
		AgentsGroupFlagController flagController = agent.FlagController;
		FightingCapturePoint fightingCapturePoint = ((flagController != null) ? flagController.CapturePoint : null);
		if (fightingCapturePoint)
		{
			return fightingCapturePoint;
		}
		AgentsGroupBehaviourController parentController = agent.ParentController;
		FightingLine fightingLine = ((parentController != null) ? parentController.FightingLine : null);
		if (fightingLine == null)
		{
			return null;
		}
		FightingSector fightingSector = fightingLine.FindNearestEnemySectorBy(LazyConsts.Fighting.TeamType.Player);
		if (fightingSector == null)
		{
			return null;
		}
		return fightingSector.point;
	}

	// Token: 0x0600109B RID: 4251 RVA: 0x000542B8 File Offset: 0x000524B8
	private MobCommand TryGetCapturePointCommand(AllyDefaultAI.AllyDecisionContext context)
	{
		if (context.Agent.IsUnderMainHeroPush)
		{
			return null;
		}
		if (context.FlagController.IsSetAtPoint)
		{
			return null;
		}
		FightingCapturePoint capturePoint = this.GetNearestAllyCapturePoint(context.Agent);
		if (!capturePoint || capturePoint.LockedForCapture)
		{
			return null;
		}
		if (AllyDefaultAI.HasEnemyOnCapturePoint(capturePoint))
		{
			return null;
		}
		if (!AllyDefaultAI.ShouldAllyContestCapturePoint(capturePoint))
		{
			return null;
		}
		FightingAgent agent = context.Agent;
		bool flag = this.IsOnCapturePoint(agent.Wgo.Data.Position, capturePoint);
		if (!flag)
		{
			MobCommandGoTo mobCommandGoTo = agent.MobCommand as MobCommandGoTo;
			if (mobCommandGoTo != null && mobCommandGoTo.DestinationModifier is AllyCapturePointDestinationModifier)
			{
				return null;
			}
		}
		if (!flag)
		{
			return new MobCommandGoTo(new AllyCapturePointDestinationModifier(capturePoint)).WithCustomStopCondition(() => this.ShouldStopAllyCaptureGoTo(context, capturePoint), this.retargetDeltaTime, global::UnityEngine.Random.Range(0f, this.retargetDeltaTime)).ToPosition(capturePoint.transform.position);
		}
		return new MobCommandFlagCapture(capturePoint).WithCustomStopCondition(() => this.ShouldStopAllyFlagCapture(context, capturePoint));
	}

	// Token: 0x0600109C RID: 4252 RVA: 0x00054408 File Offset: 0x00052608
	private bool ShouldStopAllyCaptureGoTo(AllyDefaultAI.AllyDecisionContext context, FightingCapturePoint capturePoint)
	{
		return capturePoint == null || AllyDefaultAI.IsAllyCaptureContestComplete(capturePoint) || AllyDefaultAI.HasEnemyOnCapturePoint(capturePoint);
	}

	// Token: 0x0600109D RID: 4253 RVA: 0x00054425 File Offset: 0x00052625
	private bool ShouldStopAllyFlagCapture(AllyDefaultAI.AllyDecisionContext context, FightingCapturePoint capturePoint)
	{
		return capturePoint == null || AllyDefaultAI.IsAllyCaptureContestComplete(capturePoint) || !this.IsOnCapturePoint(context.Agent.Wgo.Data.Position, capturePoint) || AllyDefaultAI.HasEnemyOnCapturePoint(capturePoint);
	}

	// Token: 0x0600109E RID: 4254 RVA: 0x00054463 File Offset: 0x00052663
	private static bool ShouldAllyContestCapturePoint(FightingCapturePoint capturePoint)
	{
		return capturePoint && (capturePoint.OwnedByTeam != LazyConsts.Fighting.TeamType.Player || !capturePoint.CurrentProgress.EqualsOrMore(1f, 1E-05f));
	}

	// Token: 0x0600109F RID: 4255 RVA: 0x00054491 File Offset: 0x00052691
	private static bool IsAllyCaptureContestComplete(FightingCapturePoint capturePoint)
	{
		return capturePoint != null && capturePoint.OwnedByTeam == LazyConsts.Fighting.TeamType.Player && capturePoint.CurrentProgress.EqualsOrMore(1f, 1E-05f);
	}

	// Token: 0x060010A0 RID: 4256 RVA: 0x000544BC File Offset: 0x000526BC
	private static bool HasEnemyOnCapturePoint(FightingCapturePoint capturePoint)
	{
		if (!capturePoint)
		{
			return false;
		}
		for (int i = 0; i < capturePoint.enemies.Count; i++)
		{
			ICombatEntity combatEntity = capturePoint.enemies[i];
			if (combatEntity != null && combatEntity.IsActiveCombatant)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060010A1 RID: 4257 RVA: 0x00054504 File Offset: 0x00052704
	private static ICombatEntity GetClosestEnemyOnCapturePoint(FightingCapturePoint capturePoint, FightingAgent agent)
	{
		if (!capturePoint || capturePoint.enemies.Count == 0)
		{
			return null;
		}
		Vector3 position = agent.Wgo.Data.Position;
		ICombatEntity combatEntity = null;
		float num = float.MaxValue;
		for (int i = 0; i < capturePoint.enemies.Count; i++)
		{
			ICombatEntity combatEntity2 = capturePoint.enemies[i];
			if (combatEntity2 != null && combatEntity2.IsActiveCombatant)
			{
				float sqrMagnitude = (position - combatEntity2.CombatEntityPosition).XZ().sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					combatEntity = combatEntity2;
				}
			}
		}
		return combatEntity;
	}

	// Token: 0x040012C4 RID: 4804
	private const float EPSILON = 0.4f;

	// Token: 0x040012C5 RID: 4805
	private const float ARCHER_KITE_MIN_MOVE = 0.75f;

	// Token: 0x040012C6 RID: 4806
	private const float ARCHER_KITE_MIN_GAIN = 0.35f;

	// Token: 0x040012C7 RID: 4807
	private const float ARCHER_KITE_SLOT_RADIUS = 0.9f;

	// Token: 0x040012C8 RID: 4808
	private const float ARCHER_KITE_MAX_SNAP = 0.5f;

	// Token: 0x040012C9 RID: 4809
	private static readonly float[] ArcherKiteYawOffsetsDeg = new float[] { 0f, 45f, -45f, 90f, -90f };

	// Token: 0x040012CA RID: 4810
	public float attackDistance = 0.1f;

	// Token: 0x040012CB RID: 4811
	[Range(0.4f, 20f)]
	public float followDistance = 2f;

	// Token: 0x040012CC RID: 4812
	[Range(0f, 10f)]
	public float retargetDeltaTime = 1f;

	// Token: 0x040012CD RID: 4813
	public bool pikemenHoldFrontline = true;

	// Token: 0x040012CE RID: 4814
	[Range(0f, 20f)]
	public float archerKiteDistance = 3f;

	// Token: 0x040012CF RID: 4815
	public int archerMaxRepositions = 2;

	// Token: 0x040012D0 RID: 4816
	public float archerRepositionCooldown = 4f;

	// Token: 0x040012D1 RID: 4817
	[SerializeReference]
	private List<IAllyDecisionStep> customDecisionSteps = new List<IAllyDecisionStep>();

	// Token: 0x040012D2 RID: 4818
	private List<IAllyDecisionStep> defaultDecisionSteps;

	// Token: 0x0200027D RID: 637
	[Serializable]
	private sealed class CombatDecisionStep : IAllyDecisionStep, IAgentDecisionStep<AllyDefaultAI.AllyDecisionContext>
	{
		// Token: 0x060010A4 RID: 4260 RVA: 0x00054617 File Offset: 0x00052817
		public MobCommand TryCreateCommand(AllyDefaultAI.AllyDecisionContext context)
		{
			if (context.Owner == null)
			{
				return null;
			}
			return context.Owner.TryGetCombatCommand(context);
		}
	}

	// Token: 0x0200027E RID: 638
	[Serializable]
	private sealed class CapturePointDecisionStep : IAllyDecisionStep, IAgentDecisionStep<AllyDefaultAI.AllyDecisionContext>
	{
		// Token: 0x060010A6 RID: 4262 RVA: 0x00054637 File Offset: 0x00052837
		public MobCommand TryCreateCommand(AllyDefaultAI.AllyDecisionContext context)
		{
			if (context.Owner == null)
			{
				return null;
			}
			return context.Owner.TryGetCapturePointCommand(context);
		}
	}

	// Token: 0x0200027F RID: 639
	[Serializable]
	private sealed class FlagFollowDecisionStep : IAllyDecisionStep, IAgentDecisionStep<AllyDefaultAI.AllyDecisionContext>
	{
		// Token: 0x060010A8 RID: 4264 RVA: 0x00054657 File Offset: 0x00052857
		public MobCommand TryCreateCommand(AllyDefaultAI.AllyDecisionContext context)
		{
			if (context.Owner == null)
			{
				return null;
			}
			return context.Owner.TryGetFlagFollowCommand(context);
		}
	}

	// Token: 0x02000280 RID: 640
	[Serializable]
	private sealed class FlagDockingDecisionStep : IAllyDecisionStep, IAgentDecisionStep<AllyDefaultAI.AllyDecisionContext>
	{
		// Token: 0x060010AA RID: 4266 RVA: 0x00054677 File Offset: 0x00052877
		public MobCommand TryCreateCommand(AllyDefaultAI.AllyDecisionContext context)
		{
			if (context.Owner == null)
			{
				return null;
			}
			return context.Owner.TryGetFlagDockingCommand(context);
		}
	}

	// Token: 0x02000281 RID: 641
	public readonly struct AllyDecisionContext
	{
		// Token: 0x060010AC RID: 4268 RVA: 0x00054698 File Offset: 0x00052898
		public AllyDecisionContext(AllyDefaultAI owner, FightingAgent agent, Func<IEnumerable<ICombatEntity>> potentialTargets, AgentsGroupFlagController flagController, float followDistance, float defaultAttackDistance, float aggroDistance, float deAggroDistance)
		{
			this.owner = owner;
			this.agent = agent;
			this.flagController = flagController;
			this.followDistance = followDistance;
			this.aggroDistance = aggroDistance;
			this.deAggroDistance = deAggroDistance;
			this.potentialTargets = potentialTargets;
			this.isRangedAttacker = agent.AttackComponent.IsRangedWeapon;
			this.attackRange = (agent.AttackComponent.weapon ? ((float)agent.FighterDef.atkRange.EvaluateInt(agent.Wgo)) : defaultAttackDistance);
			this.enemyTargets = new List<ICombatEntity>();
			if (potentialTargets == null)
			{
				return;
			}
			foreach (ICombatEntity combatEntity in potentialTargets())
			{
				if (combatEntity != null && combatEntity.TeamType != agent.Wgo.TeamType)
				{
					this.enemyTargets.Add(combatEntity);
				}
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x060010AD RID: 4269 RVA: 0x00054788 File Offset: 0x00052988
		public AllyDefaultAI Owner
		{
			get
			{
				return this.owner;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x060010AE RID: 4270 RVA: 0x00054790 File Offset: 0x00052990
		public FightingAgent Agent
		{
			get
			{
				return this.agent;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x060010AF RID: 4271 RVA: 0x00054798 File Offset: 0x00052998
		public bool HasValidFlag
		{
			get
			{
				return this.flagController && this.flagController.FlagWgo;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x060010B0 RID: 4272 RVA: 0x000547B9 File Offset: 0x000529B9
		public AgentsGroupFlagController FlagController
		{
			get
			{
				return this.flagController;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x060010B1 RID: 4273 RVA: 0x000547C1 File Offset: 0x000529C1
		public IReadOnlyList<ICombatEntity> EnemyTargets
		{
			get
			{
				return this.enemyTargets;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x060010B2 RID: 4274 RVA: 0x000547C9 File Offset: 0x000529C9
		public Func<IEnumerable<ICombatEntity>> PotentialTargets
		{
			get
			{
				return this.potentialTargets;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x060010B3 RID: 4275 RVA: 0x000547D1 File Offset: 0x000529D1
		public float AttackRange
		{
			get
			{
				return this.attackRange;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x060010B4 RID: 4276 RVA: 0x000547D9 File Offset: 0x000529D9
		public float FollowDistance
		{
			get
			{
				return this.followDistance;
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x060010B5 RID: 4277 RVA: 0x000547E1 File Offset: 0x000529E1
		public float AggroDistance
		{
			get
			{
				return this.aggroDistance;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x060010B6 RID: 4278 RVA: 0x000547E9 File Offset: 0x000529E9
		public float DeAggroDistance
		{
			get
			{
				return this.deAggroDistance;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x060010B7 RID: 4279 RVA: 0x000547F1 File Offset: 0x000529F1
		public bool HasEnemyTargets
		{
			get
			{
				return this.enemyTargets.Count > 0;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x060010B8 RID: 4280 RVA: 0x00054801 File Offset: 0x00052A01
		public bool IsRangedAttacker
		{
			get
			{
				return this.isRangedAttacker;
			}
		}

		// Token: 0x040012D3 RID: 4819
		private readonly AllyDefaultAI owner;

		// Token: 0x040012D4 RID: 4820
		private readonly FightingAgent agent;

		// Token: 0x040012D5 RID: 4821
		private readonly AgentsGroupFlagController flagController;

		// Token: 0x040012D6 RID: 4822
		private readonly List<ICombatEntity> enemyTargets;

		// Token: 0x040012D7 RID: 4823
		private readonly Func<IEnumerable<ICombatEntity>> potentialTargets;

		// Token: 0x040012D8 RID: 4824
		private readonly float attackRange;

		// Token: 0x040012D9 RID: 4825
		private readonly float followDistance;

		// Token: 0x040012DA RID: 4826
		private readonly float aggroDistance;

		// Token: 0x040012DB RID: 4827
		private readonly float deAggroDistance;

		// Token: 0x040012DC RID: 4828
		private readonly bool isRangedAttacker;
	}
}
