using System;
using System.Collections.Generic;
using LazyBearTechnology;
using LinqTools;
using UnityEngine;

// Token: 0x02000295 RID: 661
[CreateAssetMenu(menuName = "GK2/Fighting/AIs/EnemyLeglessZombieJump")]
public class EnemyLeglessZombieJumpAI : EnemyWithDoorAI
{
	// Token: 0x06001100 RID: 4352 RVA: 0x00055C9C File Offset: 0x00053E9C
	public override MobCommand GetCommand(FightingAgent agent, Func<IEnumerable<ICombatEntity>> targets)
	{
		MobCommand mobCommand;
		if (this.TryCreateJumpCommand(agent, targets, out mobCommand))
		{
			return mobCommand;
		}
		MobCommand command = base.GetCommand(agent, targets);
		MobCommandGoTo mobCommandGoTo = command as MobCommandGoTo;
		if (mobCommandGoTo != null)
		{
			mobCommandGoTo.WithCustomStopCondition(() => this.ShouldInterruptGoToForJump(agent, targets), this.retargetDeltaTime, global::UnityEngine.Random.Range(0f, this.retargetDeltaTime));
		}
		return command;
	}

	// Token: 0x06001101 RID: 4353 RVA: 0x00055D24 File Offset: 0x00053F24
	protected override EnemyDefaultAI.EnemyDecisionContext BuildContext(FightingAgent agent, Func<IEnumerable<ICombatEntity>> targets)
	{
		EnemyLeglessZombieJumpAI.<>c__DisplayClass18_0 CS$<>8__locals1 = new EnemyLeglessZombieJumpAI.<>c__DisplayClass18_0();
		CS$<>8__locals1.targets = targets;
		CS$<>8__locals1.capturePoint = null;
		FightingLine fightingLine = (agent.ParentController ? agent.ParentController.FightingLine : null);
		if (fightingLine != null)
		{
			EnemyLeglessZombieJumpAI.<>c__DisplayClass18_0 CS$<>8__locals2 = CS$<>8__locals1;
			FightingSector fightingSector = fightingLine.FindNearestEnemySectorBy(LazyConsts.Fighting.TeamType.WildZombie);
			CS$<>8__locals2.capturePoint = ((fightingSector != null) ? fightingSector.point : null);
		}
		if (!CS$<>8__locals1.capturePoint && LazySingleton<FightingGameController>.Instance != null && LazySingleton<FightingGameController>.Instance.CurrentLevel != null)
		{
			CS$<>8__locals1.capturePoint = LazySingleton<FightingGameController>.Instance.CurrentLevel.BaseCapturePoint;
		}
		if (CS$<>8__locals1.capturePoint != null && (agent.Wgo.Data.Position - CS$<>8__locals1.capturePoint.transform.position).XZ().magnitude < CS$<>8__locals1.capturePoint.Radius)
		{
			Func<IEnumerable<ICombatEntity>> func = delegate
			{
				IEnumerable<ICombatEntity> enumerable = CS$<>8__locals1.targets();
				Func<ICombatEntity, bool> func2;
				if ((func2 = CS$<>8__locals1.<>9__1) == null)
				{
					func2 = (CS$<>8__locals1.<>9__1 = (ICombatEntity t) => (t.CombatEntityPosition - CS$<>8__locals1.capturePoint.transform.position).XZ().magnitude < CS$<>8__locals1.capturePoint.Radius);
				}
				return enumerable.Where(func2);
			};
			return new EnemyDefaultAI.EnemyDecisionContext(this, agent, this.WrapPotentialTargets(agent, func), this.aggroDistance);
		}
		return base.BuildContext(agent, this.WrapPotentialTargets(agent, CS$<>8__locals1.targets));
	}

	// Token: 0x06001102 RID: 4354 RVA: 0x00055E50 File Offset: 0x00054050
	private bool TryCreateJumpCommand(FightingAgent agent, Func<IEnumerable<ICombatEntity>> targets, out MobCommand command)
	{
		command = null;
		if (agent == null || targets == null)
		{
			return false;
		}
		SGuid uniqueId = agent.Wgo.Data.UniqueId;
		int num;
		this.jumpCountByAgent.TryGetValue(uniqueId, out num);
		if (num >= this.maxJumpCount)
		{
			return false;
		}
		if (this.IsJumpOnCooldown(agent))
		{
			return false;
		}
		this.CleanupExpiredLandingReservations();
		ICombatEntity combatEntity;
		Vector3 vector;
		if (!this.TryGetNearestLandableJumpTarget(agent, targets, out combatEntity, out vector))
		{
			this.StartJumpCooldown(agent);
			return false;
		}
		this.ReserveLandingPosition(agent, vector);
		this.StartJumpCooldown(agent);
		this.jumpCountByAgent[uniqueId] = num + 1;
		command = new LeglessZombieJumpCommand(vector, this.jumpDuration, this.jumpArcHeight, this.GetRandomHeightCurve())
		{
			TargetEntity = combatEntity,
			customPosition = vector
		};
		return true;
	}

	// Token: 0x06001103 RID: 4355 RVA: 0x00055F08 File Offset: 0x00054108
	private bool TryGetNearestLandableJumpTarget(FightingAgent agent, Func<IEnumerable<ICombatEntity>> targets, out ICombatEntity nearestTarget, out Vector3 landingPosition)
	{
		nearestTarget = null;
		landingPosition = default(Vector3);
		IEnumerable<ICombatEntity> enumerable = targets();
		if (enumerable == null)
		{
			return false;
		}
		float num = float.PositiveInfinity;
		Vector3 position = agent.Wgo.Data.Position;
		foreach (ICombatEntity combatEntity in enumerable)
		{
			if (combatEntity != null && combatEntity.CombatEntityHpComponent.Hp > 0 && this.IsTargetValid(combatEntity, agent) && this.HasNonZeroMovementSpeed(combatEntity))
			{
				float combatEntityDistance = combatEntity.GetCombatEntityDistance(position, agent.Wgo.TeamType);
				Vector3 vector;
				if ((this.minJumpDistance < 0f || combatEntityDistance >= this.minJumpDistance) && combatEntityDistance <= this.lookingForJumpSearchRadius && combatEntityDistance < num && this.TryFindLandingPosition(agent, combatEntity, out vector))
				{
					num = combatEntityDistance;
					nearestTarget = combatEntity;
					landingPosition = vector;
				}
			}
		}
		return nearestTarget != null;
	}

	// Token: 0x06001104 RID: 4356 RVA: 0x00056004 File Offset: 0x00054204
	private bool HasNonZeroMovementSpeed(ICombatEntity target)
	{
		global::UnityEngine.Object @object = target as global::UnityEngine.Object;
		if (@object != null)
		{
			Wgo wgo = @object as Wgo;
			if (wgo != null)
			{
				FighterDef data = GameBalance.Me.GetData<FighterDef>(wgo.Id);
				return data != null && data.mvtSpeed > 0f;
			}
		}
		global::UnityEngine.Object object2 = target as global::UnityEngine.Object;
		return object2 != null && object2 is PlayerPhysicalBody;
	}

	// Token: 0x06001105 RID: 4357 RVA: 0x00056060 File Offset: 0x00054260
	private bool TryFindLandingPosition(FightingAgent agent, ICombatEntity target, out Vector3 landingPosition)
	{
		landingPosition = default(Vector3);
		Vector3 combatEntityPosition = target.CombatEntityPosition;
		Vector2 vector = (combatEntityPosition - agent.Wgo.Data.Position).XZ2().normalized;
		if (vector == Vector2.zero)
		{
			vector = agent.Wgo.Data.direction.Value;
		}
		Vector3 vector2 = combatEntityPosition + new Vector3(vector.x, 0f, vector.y) * this.landingOffsetFromTarget;
		Vector3 vector3;
		if (!SpecialPhysicsCastUtils.GetLandPositionByCapsule(vector2, vector, this.landingLookDistance, this.landingCapsuleRadius, this.landingCapsuleHeight, out vector3, 45f, 4))
		{
			return false;
		}
		if (!this.IsValidLandingPosition(agent, vector2, vector3))
		{
			return false;
		}
		landingPosition = vector3;
		return true;
	}

	// Token: 0x06001106 RID: 4358 RVA: 0x00056124 File Offset: 0x00054324
	private bool ShouldInterruptGoToForJump(FightingAgent agent, Func<IEnumerable<ICombatEntity>> targets)
	{
		if (agent == null || targets == null)
		{
			return false;
		}
		if (this.IsJumpOnCooldown(agent))
		{
			return false;
		}
		SGuid uniqueId = agent.Wgo.Data.UniqueId;
		int num;
		this.jumpCountByAgent.TryGetValue(uniqueId, out num);
		if (num >= this.maxJumpCount)
		{
			return false;
		}
		this.CleanupExpiredLandingReservations();
		ICombatEntity combatEntity;
		Vector3 vector;
		return this.TryGetNearestLandableJumpTarget(agent, targets, out combatEntity, out vector);
	}

	// Token: 0x06001107 RID: 4359 RVA: 0x00056188 File Offset: 0x00054388
	private bool IsValidLandingPosition(FightingAgent agent, Vector3 searchCenter, Vector3 candidate)
	{
		if ((candidate - searchCenter).XZ().magnitude > this.landingLookDistance + 0.1f)
		{
			return false;
		}
		if ((candidate - agent.Wgo.Data.Position).XZ().magnitude < 0.2f)
		{
			return false;
		}
		if (this.IsLandingReservedByAnotherAgent(agent, candidate))
		{
			return false;
		}
		Vector3 vector = new Vector3(agent.Settings.aiPathRadius, 0.9f, agent.Settings.aiPathRadius);
		return !Physics.CheckBox(candidate + Vector3.up * vector.y, vector, Quaternion.identity, 16843009, QueryTriggerInteraction.Ignore);
	}

	// Token: 0x06001108 RID: 4360 RVA: 0x00056240 File Offset: 0x00054440
	private bool IsJumpOnCooldown(FightingAgent agent)
	{
		SGuid uniqueId = agent.Wgo.Data.UniqueId;
		float num;
		if (!this.jumpCooldownByAgent.TryGetValue(uniqueId, out num))
		{
			return false;
		}
		if (Time.time >= num)
		{
			this.jumpCooldownByAgent.Remove(uniqueId);
			return false;
		}
		return true;
	}

	// Token: 0x06001109 RID: 4361 RVA: 0x00056288 File Offset: 0x00054488
	private void StartJumpCooldown(FightingAgent agent)
	{
		this.jumpCooldownByAgent[agent.Wgo.Data.UniqueId] = Time.time + this.jumpCooldown;
	}

	// Token: 0x0600110A RID: 4362 RVA: 0x000562B4 File Offset: 0x000544B4
	private AnimationCurve GetRandomHeightCurve()
	{
		if (this.jumpHeightCurves == null || this.jumpHeightCurves.Count == 0)
		{
			return null;
		}
		for (int i = this.jumpHeightCurves.Count - 1; i >= 0; i--)
		{
			if (this.jumpHeightCurves[i] == null)
			{
				this.jumpHeightCurves.RemoveAt(i);
			}
		}
		if (this.jumpHeightCurves.Count == 0)
		{
			return null;
		}
		int num = global::UnityEngine.Random.Range(0, this.jumpHeightCurves.Count);
		return this.jumpHeightCurves[num];
	}

	// Token: 0x0600110B RID: 4363 RVA: 0x00056338 File Offset: 0x00054538
	private bool IsLandingReservedByAnotherAgent(FightingAgent agent, Vector3 candidate)
	{
		float num = Mathf.Max(this.reservedLandingRadius, this.landingCapsuleRadius);
		float num2 = num * num;
		SGuid uniqueId = agent.Wgo.Data.UniqueId;
		foreach (KeyValuePair<SGuid, EnemyLeglessZombieJumpAI.LandingReservation> keyValuePair in this.landingReservationsByAgent)
		{
			if (!(keyValuePair.Key == uniqueId) && (keyValuePair.Value.Position - candidate).XZ().sqrMagnitude <= num2)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600110C RID: 4364 RVA: 0x000563E4 File Offset: 0x000545E4
	private void ReserveLandingPosition(FightingAgent agent, Vector3 position)
	{
		float num = Mathf.Max(0.1f, this.reservedLandingLifetime);
		this.landingReservationsByAgent[agent.Wgo.Data.UniqueId] = new EnemyLeglessZombieJumpAI.LandingReservation
		{
			Position = position,
			ExpiresAt = Time.time + num
		};
	}

	// Token: 0x0600110D RID: 4365 RVA: 0x0005643C File Offset: 0x0005463C
	private void CleanupExpiredLandingReservations()
	{
		if (this.landingReservationsByAgent.Count == 0)
		{
			return;
		}
		this.reservationKeysToRemove.Clear();
		foreach (KeyValuePair<SGuid, EnemyLeglessZombieJumpAI.LandingReservation> keyValuePair in this.landingReservationsByAgent)
		{
			if (Time.time >= keyValuePair.Value.ExpiresAt)
			{
				this.reservationKeysToRemove.Add(keyValuePair.Key);
			}
		}
		for (int i = 0; i < this.reservationKeysToRemove.Count; i++)
		{
			this.landingReservationsByAgent.Remove(this.reservationKeysToRemove[i]);
		}
	}

	// Token: 0x04001317 RID: 4887
	[Header("Jump Limits")]
	public int maxJumpCount = 1;

	// Token: 0x04001318 RID: 4888
	[Header("Jump")]
	[Tooltip("Minimum distance to target required to start jump. Negative value disables this check.")]
	public float minJumpDistance = -1f;

	// Token: 0x04001319 RID: 4889
	public float lookingForJumpSearchRadius = 2.5f;

	// Token: 0x0400131A RID: 4890
	public float jumpCooldown = 3f;

	// Token: 0x0400131B RID: 4891
	public float jumpDuration = 0.35f;

	// Token: 0x0400131C RID: 4892
	public float jumpArcHeight = 0.8f;

	// Token: 0x0400131D RID: 4893
	[Header("Landing Search")]
	[Tooltip("Distance from the target to offset the landing search center. Helps the agent land further away.")]
	public float landingOffsetFromTarget = 1f;

	// Token: 0x0400131E RID: 4894
	public float landingLookDistance = 1.5f;

	// Token: 0x0400131F RID: 4895
	public float landingCapsuleRadius = 0.35f;

	// Token: 0x04001320 RID: 4896
	public float landingCapsuleHeight = 1.4f;

	// Token: 0x04001321 RID: 4897
	[Header("Landing Reservation")]
	public float reservedLandingRadius = 0.6f;

	// Token: 0x04001322 RID: 4898
	public float reservedLandingLifetime = 1.2f;

	// Token: 0x04001323 RID: 4899
	[Tooltip("Predefined jump height curves in normalized time [0..1]. One random curve is selected for each jump.")]
	public List<AnimationCurve> jumpHeightCurves = new List<AnimationCurve>();

	// Token: 0x04001324 RID: 4900
	private readonly Dictionary<SGuid, float> jumpCooldownByAgent = new Dictionary<SGuid, float>();

	// Token: 0x04001325 RID: 4901
	private readonly Dictionary<SGuid, int> jumpCountByAgent = new Dictionary<SGuid, int>();

	// Token: 0x04001326 RID: 4902
	private readonly Dictionary<SGuid, EnemyLeglessZombieJumpAI.LandingReservation> landingReservationsByAgent = new Dictionary<SGuid, EnemyLeglessZombieJumpAI.LandingReservation>();

	// Token: 0x04001327 RID: 4903
	private readonly List<SGuid> reservationKeysToRemove = new List<SGuid>();

	// Token: 0x02000296 RID: 662
	private struct LandingReservation
	{
		// Token: 0x04001328 RID: 4904
		public Vector3 Position;

		// Token: 0x04001329 RID: 4905
		public float ExpiresAt;
	}
}
