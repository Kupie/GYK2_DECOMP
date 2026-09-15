using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LazyBearTechnology;
using Pathfinding;
using UnityEngine;

// Token: 0x02000276 RID: 630
[Serializable]
public abstract class AgentAI : ScriptableObject
{
	// Token: 0x170002AB RID: 683
	// (get) Token: 0x0600105F RID: 4191 RVA: 0x0005272A File Offset: 0x0005092A
	public FightingGameController FightingGameController
	{
		get
		{
			return LazySingleton<FightingGameController>.Instance;
		}
	}

	// Token: 0x06001060 RID: 4192
	public abstract MobCommand GetCommand(FightingAgent agent, Func<IEnumerable<ICombatEntity>> targets);

	// Token: 0x06001061 RID: 4193 RVA: 0x00052734 File Offset: 0x00050934
	public static bool TryLineCastByRecast(Vector3 start, Vector3 end)
	{
		AgentAI.TargetSelectionBuffers.SingleLinecastDestinations.Clear();
		AgentAI.TargetSelectionBuffers.SingleLinecastDestinations.Add(end);
		AgentAI.TargetSelectionBuffers.SingleLinecastVisibility.Clear();
		SpecialPhysicsCastUtils.GetLinecastVisibility(start, AgentAI.TargetSelectionBuffers.SingleLinecastDestinations, 256, AgentAI.TargetSelectionBuffers.SingleLinecastVisibility, 30);
		return AgentAI.TargetSelectionBuffers.SingleLinecastVisibility.Count > 0 && AgentAI.TargetSelectionBuffers.SingleLinecastVisibility[0];
	}

	// Token: 0x06001062 RID: 4194 RVA: 0x00052791 File Offset: 0x00050991
	[CanBeNull]
	protected static ICombatEntity GetClosestTarget(Vector3 position, Func<IEnumerable<ICombatEntity>> targets, LazyConsts.Fighting.TeamType teamType, float maxDistance = float.PositiveInfinity, bool targetMustBeDirectlyVisible = false)
	{
		if (targets == null)
		{
			return null;
		}
		return AgentAI.GetClosestTarget(position, targets(), teamType, maxDistance, targetMustBeDirectlyVisible);
	}

	// Token: 0x06001063 RID: 4195 RVA: 0x000527A8 File Offset: 0x000509A8
	[CanBeNull]
	protected static ICombatEntity GetClosestTarget(Vector3 position, IEnumerable<ICombatEntity> targets, LazyConsts.Fighting.TeamType teamType, float maxDistance = float.PositiveInfinity, bool targetMustBeDirectlyVisible = false)
	{
		AgentAI.TargetSelectionBuffers.Targets.Clear();
		AgentAI.TargetSelectionBuffers.TargetDistances.Clear();
		AgentAI.TargetSelectionBuffers.TargetPriorities.Clear();
		foreach (ICombatEntity combatEntity in targets)
		{
			if (combatEntity.CombatEntityHpComponent.Hp != 0)
			{
				float combatEntityDistance = combatEntity.GetCombatEntityDistance(position, teamType);
				if (combatEntityDistance < maxDistance)
				{
					AgentAI.TargetSelectionBuffers.Targets.Add(combatEntity);
					AgentAI.TargetSelectionBuffers.TargetDistances.Add(combatEntityDistance);
					AgentAI.TargetSelectionBuffers.TargetPriorities.Add(combatEntity.AttackPriority);
				}
			}
		}
		if (AgentAI.TargetSelectionBuffers.Targets.Count == 0)
		{
			return null;
		}
		if (targetMustBeDirectlyVisible)
		{
			AgentAI.TargetSelectionBuffers.LinecastDestinations.Clear();
			for (int i = 0; i < AgentAI.TargetSelectionBuffers.Targets.Count; i++)
			{
				AgentAI.TargetSelectionBuffers.LinecastDestinations.Add(AgentAI.TargetSelectionBuffers.Targets[i].CombatEntityPosition + Vector3.up * 0.5f);
			}
			AgentAI.TargetSelectionBuffers.LinecastVisibility.Clear();
			SpecialPhysicsCastUtils.GetLinecastVisibility(position + Vector3.up * 0.5f, AgentAI.TargetSelectionBuffers.LinecastDestinations, 256, AgentAI.TargetSelectionBuffers.LinecastVisibility, 30);
		}
		ICombatEntity combatEntity2 = null;
		float num = float.PositiveInfinity;
		int num2 = -1;
		for (int j = 0; j < AgentAI.TargetSelectionBuffers.Targets.Count; j++)
		{
			if (!targetMustBeDirectlyVisible || (j < AgentAI.TargetSelectionBuffers.LinecastVisibility.Count && AgentAI.TargetSelectionBuffers.LinecastVisibility[j]))
			{
				int num3 = AgentAI.TargetSelectionBuffers.TargetPriorities[j];
				float num4 = AgentAI.TargetSelectionBuffers.TargetDistances[j];
				if (num3 > num2 || (num3 == num2 && num4 < num))
				{
					combatEntity2 = AgentAI.TargetSelectionBuffers.Targets[j];
					num = num4;
					num2 = num3;
				}
			}
		}
		return combatEntity2;
	}

	// Token: 0x06001064 RID: 4196 RVA: 0x00052970 File Offset: 0x00050B70
	protected bool TryGetDockPoint(FightingAgent agent, Wgo wgo, out DockPointData dockPoint, DockPointData.Availability availability = DockPointData.Availability.OnlyNotOccupied, DockPointData.Filter filter = DockPointData.Filter.OnlyNotZombie, bool getRandomInsteadOfNearest = false, Func<DockPointData, Vector3, bool> additionalCheck = null)
	{
		AgentAI.<>c__DisplayClass11_0 CS$<>8__locals1 = new AgentAI.<>c__DisplayClass11_0();
		CS$<>8__locals1.additionalCheck = additionalCheck;
		CS$<>8__locals1.<>4__this = this;
		dockPoint = null;
		if (!wgo.HasAnyDockPoint)
		{
			return false;
		}
		if (!SGuid.IsNullOrEmpty(agent.Wgo.Data.takenDockPointsParentSGuid) && agent.Wgo.Data.takenDockPointsParentSGuid == wgo.Data.UniqueId)
		{
			dockPoint = wgo.Data.MainWgoPartData.GetOccupiedDockPointBy(agent.Wgo.Data.UniqueId);
		}
		if (dockPoint == null)
		{
			dockPoint = ((!getRandomInsteadOfNearest) ? wgo.Data.MainWgoPartData.GetNearestDockPoint(wgo.Data, agent.Wgo.Data.Position, availability, filter, new Func<DockPointData, Vector3, bool>(CS$<>8__locals1.<TryGetDockPoint>g__OutOfGraphCheck|0)) : wgo.Data.MainWgoPartData.GetDockPoints(availability, filter).GetRandom<DockPointData>());
		}
		return dockPoint != null;
	}

	// Token: 0x06001065 RID: 4197 RVA: 0x00052A5C File Offset: 0x00050C5C
	protected bool HasAnyAvailableDockPointOnRecast(WgoData wgoData, DockPointData.Filter filter)
	{
		if (((wgoData != null) ? wgoData.MainWgoPartData : null) == null)
		{
			return false;
		}
		RecastGraph recastGraph = this.FightingGameController.RecastGraph;
		if (recastGraph == null)
		{
			return false;
		}
		Vector3 position = wgoData.Position;
		using (List<DockPointData>.Enumerator enumerator = wgoData.MainWgoPartData.GetDockPoints(DockPointData.Availability.OnlyNotOccupied, filter).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.IsOnRecast(position, recastGraph))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001066 RID: 4198 RVA: 0x00052AE8 File Offset: 0x00050CE8
	protected bool TryGetOverflowDockRing(FightingAgent agent, Wgo wgoTarget, out DockPointData nearestZombieDock, out float approachOffsetFromCenter)
	{
		nearestZombieDock = null;
		approachOffsetFromCenter = 0f;
		bool flag;
		if (agent == null)
		{
			flag = null != null;
		}
		else
		{
			Wgo wgo = agent.Wgo;
			flag = ((wgo != null) ? wgo.Data : null) != null;
		}
		if (flag)
		{
			bool flag2;
			if (wgoTarget == null)
			{
				flag2 = null != null;
			}
			else
			{
				WgoData data = wgoTarget.Data;
				flag2 = ((data != null) ? data.MainWgoPartData : null) != null;
			}
			if (flag2)
			{
				nearestZombieDock = wgoTarget.Data.MainWgoPartData.GetNearestDockPoint(wgoTarget.Data, agent.Wgo.Data.Position, DockPointData.Availability.All, DockPointData.Filter.OnlyZombie, null);
				if (nearestZombieDock == null)
				{
					return false;
				}
				float magnitude = nearestZombieDock.BakedData.Position.XZ().magnitude;
				float num = ((agent.Settings != null) ? (agent.Settings.aiPathRadius * 2f) : 0.4f);
				approachOffsetFromCenter = magnitude + num;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001067 RID: 4199 RVA: 0x00052BB0 File Offset: 0x00050DB0
	protected void ReleaseTakenDockPointIfDifferent(FightingAgent agent, Wgo targetWgo)
	{
		bool flag;
		if (agent == null)
		{
			flag = null != null;
		}
		else
		{
			Wgo wgo = agent.Wgo;
			flag = ((wgo != null) ? wgo.Data : null) != null;
		}
		if (!flag || SGuid.IsNullOrEmpty(agent.Wgo.Data.takenDockPointsParentSGuid))
		{
			return;
		}
		if (targetWgo != null && agent.Wgo.Data.takenDockPointsParentSGuid == targetWgo.Data.UniqueId)
		{
			return;
		}
		MainGame instance = MainGame.Instance;
		WgoData wgoData;
		if (instance == null)
		{
			wgoData = null;
		}
		else
		{
			GameSave gameSave = instance.GameSave;
			if (gameSave == null)
			{
				wgoData = null;
			}
			else
			{
				WorldData worldData = gameSave.WorldData;
				wgoData = ((worldData != null) ? worldData.GetWgoData(agent.Wgo.Data.takenDockPointsParentSGuid) : null);
			}
		}
		WgoData wgoData2 = wgoData;
		if (wgoData2 != null)
		{
			WgoPartData mainWgoPartData = wgoData2.MainWgoPartData;
			if (mainWgoPartData != null)
			{
				mainWgoPartData.TryFreeDockPoint(wgoData2.UniqueId, agent.Wgo.Data.UniqueId);
			}
		}
		agent.Wgo.Data.takenDockPointsParentSGuid = SGuid.Empty;
		agent.IsAnchoredAtDockPoint = false;
	}

	// Token: 0x06001068 RID: 4200 RVA: 0x00052C9C File Offset: 0x00050E9C
	protected void TryClaimDockPoint(FightingAgent agent, Wgo targetWgo, DockPointData dockPoint)
	{
		bool flag;
		if (agent == null)
		{
			flag = null != null;
		}
		else
		{
			Wgo wgo = agent.Wgo;
			flag = ((wgo != null) ? wgo.Data : null) != null;
		}
		if (!flag || ((targetWgo != null) ? targetWgo.Data : null) == null || dockPoint == null)
		{
			return;
		}
		SGuid combatEntityUID = agent.Wgo.CombatEntityUID;
		if (dockPoint.IsOccupied && !dockPoint.IsOccupiedBy(combatEntityUID))
		{
			return;
		}
		if (!dockPoint.IsOccupiedBy(combatEntityUID))
		{
			dockPoint.Occupy(combatEntityUID);
		}
		agent.Wgo.Data.takenDockPointsParentSGuid = targetWgo.Data.UniqueId;
	}

	// Token: 0x06001069 RID: 4201 RVA: 0x00052D1E File Offset: 0x00050F1E
	protected IEnumerable<ICombatEntity> FilterTargets(IEnumerable<ICombatEntity> targets, FightingAgent agent)
	{
		foreach (ICombatEntity combatEntity in targets)
		{
			if (this.IsTargetValid(combatEntity, agent))
			{
				yield return combatEntity;
			}
		}
		IEnumerator<ICombatEntity> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x0600106A RID: 4202 RVA: 0x00052D3C File Offset: 0x00050F3C
	[CanBeNull]
	protected virtual Func<IEnumerable<ICombatEntity>> WrapPotentialTargets(FightingAgent agent, [CanBeNull] Func<IEnumerable<ICombatEntity>> targets)
	{
		if (targets == null)
		{
			return null;
		}
		return delegate
		{
			IEnumerable<ICombatEntity> enumerable = targets();
			if (enumerable != null)
			{
				return this.FilterTargets(enumerable, agent);
			}
			return Array.Empty<ICombatEntity>();
		};
	}

	// Token: 0x0600106B RID: 4203 RVA: 0x00052D7A File Offset: 0x00050F7A
	protected virtual bool IsTargetValid(ICombatEntity target, FightingAgent agent)
	{
		return target != null;
	}

	// Token: 0x040012AA RID: 4778
	protected const float Y_LINECAST_OFFSET = 0.5f;

	// Token: 0x040012AB RID: 4779
	protected const float DOCKING_DISTANCE = 0.06666668f;

	// Token: 0x040012AC RID: 4780
	private const float OVERFLOW_DOCK_RING_PADDING_FALLBACK = 0.4f;

	// Token: 0x040012AD RID: 4781
	public float aggroDistance = 10f;

	// Token: 0x040012AE RID: 4782
	public float deAggroDistance = 15f;

	// Token: 0x02000277 RID: 631
	private static class TargetSelectionBuffers
	{
		// Token: 0x040012AF RID: 4783
		public static readonly List<Vector3> LinecastDestinations = new List<Vector3>();

		// Token: 0x040012B0 RID: 4784
		public static readonly List<bool> LinecastVisibility = new List<bool>();

		// Token: 0x040012B1 RID: 4785
		public static readonly List<Vector3> SingleLinecastDestinations = new List<Vector3>(1);

		// Token: 0x040012B2 RID: 4786
		public static readonly List<bool> SingleLinecastVisibility = new List<bool>(1);

		// Token: 0x040012B3 RID: 4787
		public static readonly List<ICombatEntity> Targets = new List<ICombatEntity>();

		// Token: 0x040012B4 RID: 4788
		public static readonly List<float> TargetDistances = new List<float>();

		// Token: 0x040012B5 RID: 4789
		public static readonly List<int> TargetPriorities = new List<int>();
	}
}
