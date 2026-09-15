using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LazyBearTechnology;
using LinqTools;
using Pathfinding;
using UnityEngine;

// Token: 0x020002E5 RID: 741
public class SpearDestinationModifier : GoToDestinationModifier
{
	// Token: 0x06001372 RID: 4978 RVA: 0x0005EA08 File Offset: 0x0005CC08
	private static List<SpearDestinationModifier.SideWithOwner> CreateEmptySides()
	{
		return new List<SpearDestinationModifier.SideWithOwner>
		{
			new SpearDestinationModifier.SideWithOwner(Direction.Right, SGuid.Empty),
			new SpearDestinationModifier.SideWithOwner(Direction.Up, SGuid.Empty),
			new SpearDestinationModifier.SideWithOwner(Direction.Left, SGuid.Empty),
			new SpearDestinationModifier.SideWithOwner(Direction.Down, SGuid.Empty)
		};
	}

	// Token: 0x1700035B RID: 859
	// (get) Token: 0x06001373 RID: 4979 RVA: 0x0005EA5E File Offset: 0x0005CC5E
	public override float CustomDestinationOffset
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x1700035C RID: 860
	// (get) Token: 0x06001374 RID: 4980 RVA: 0x0005EA65 File Offset: 0x0005CC65
	private float PikeAttackOffset
	{
		get
		{
			return PikeCombatGeometry.ApproachDistance(base.Agent);
		}
	}

	// Token: 0x06001375 RID: 4981 RVA: 0x0005EA72 File Offset: 0x0005CC72
	public static void ClearAttachedEntities()
	{
		SpearDestinationModifier.attachedEntities.Clear();
	}

	// Token: 0x06001376 RID: 4982 RVA: 0x0005EA7E File Offset: 0x0005CC7E
	public override void OnStart()
	{
		base.OnStart();
		this.positionSelected = false;
		this.positionDwellElapsed = 0f;
		this.SelectBestAttackPosition();
	}

	// Token: 0x06001377 RID: 4983 RVA: 0x0005EAA0 File Offset: 0x0005CCA0
	public override void OnUpdate(float deltaTime)
	{
		base.OnUpdate(deltaTime);
		if (!this.positionSelected)
		{
			return;
		}
		this.positionDwellElapsed += deltaTime;
		if (this.positionDwellElapsed < 0.3f)
		{
			return;
		}
		if ((base.Agent.MobCommand.Position - this.targetPositionWhenSelected).XZ().magnitude > 0.4f)
		{
			this.positionSelected = false;
			this.SelectBestAttackPosition();
		}
	}

	// Token: 0x06001378 RID: 4984 RVA: 0x0005EB15 File Offset: 0x0005CD15
	public override bool TryGetTargetPositionForPathfinding(out ICombatEntity combatEntity, out Vector3 position)
	{
		combatEntity = null;
		position = this.GetCurrentTargetPosition();
		return true;
	}

	// Token: 0x06001379 RID: 4985 RVA: 0x0005EB28 File Offset: 0x0005CD28
	public override Vector3 GetCurrentTargetPosition()
	{
		Vector3 position = base.Agent.MobCommand.Position;
		return position + (this.positionSelected ? this.selectedAttackPositionLocal : this.GetClosestSlotOffset(position));
	}

	// Token: 0x0600137A RID: 4986 RVA: 0x0005EB64 File Offset: 0x0005CD64
	private Vector3 GetClosestSlotOffset(Vector3 targetPos)
	{
		Vector2 vector = (base.Wgo.Data.Position - targetPos).XZ2();
		return ((Mathf.Abs(vector.x) >= Mathf.Abs(vector.y)) ? ((vector.x >= 0f) ? Vector3.right : Vector3.left) : ((vector.y >= 0f) ? Vector3.forward : Vector3.back)) * this.PikeAttackOffset;
	}

	// Token: 0x0600137B RID: 4987 RVA: 0x0005EBE4 File Offset: 0x0005CDE4
	public override void OnReachedDestination()
	{
		base.OnReachedDestination();
		this.TryFreeOwnedSide(base.Agent.Wgo);
		ICombatEntity targetEntity = base.Agent.MobCommand.TargetEntity;
		if (targetEntity == null)
		{
			return;
		}
		targetEntity.OnOtherCombatTargetReachedToMe(base.Wgo);
	}

	// Token: 0x0600137C RID: 4988 RVA: 0x0005EC1D File Offset: 0x0005CE1D
	public override void OnFinish()
	{
		base.OnFinish();
		this.TryFreeOwnedSide(base.Agent.Wgo);
	}

	// Token: 0x0600137D RID: 4989 RVA: 0x0005EC38 File Offset: 0x0005CE38
	private void TryFreeOwnedSide(ICombatEntity combatEntity)
	{
		if (base.Agent.MobCommand.TargetEntity == null)
		{
			return;
		}
		List<SpearDestinationModifier.SideWithOwner> list;
		if (!SpearDestinationModifier.attachedEntities.TryGetValue(base.Agent.MobCommand.TargetEntity.CombatEntityUID, out list))
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			SpearDestinationModifier.SideWithOwner sideWithOwner = list[i];
			if (sideWithOwner.Owner == combatEntity.CombatEntityUID)
			{
				sideWithOwner.Owner = SGuid.Empty;
				list[i] = sideWithOwner;
				break;
			}
		}
		if (list.All((SpearDestinationModifier.SideWithOwner x) => x.Owner == SGuid.Empty))
		{
			SpearDestinationModifier.attachedEntities.Remove(base.Agent.MobCommand.TargetEntity.CombatEntityUID);
		}
	}

	// Token: 0x0600137E RID: 4990 RVA: 0x0005ED08 File Offset: 0x0005CF08
	private void SetNewOwner(List<SpearDestinationModifier.SideWithOwner> list, SGuid owner, int idx)
	{
		SpearDestinationModifier.SideWithOwner sideWithOwner = list[idx];
		sideWithOwner.Owner = owner;
		list[idx] = sideWithOwner;
	}

	// Token: 0x0600137F RID: 4991 RVA: 0x0005ED30 File Offset: 0x0005CF30
	[CanBeNull]
	private List<SpearDestinationModifier.SideWithOwner> GetOrCreateListWithOwners()
	{
		if (base.Agent.MobCommand.TargetEntity == null)
		{
			return null;
		}
		List<SpearDestinationModifier.SideWithOwner> list;
		if (SpearDestinationModifier.attachedEntities.TryGetValue(base.Agent.MobCommand.TargetEntity.CombatEntityUID, out list))
		{
			return list;
		}
		list = (SpearDestinationModifier.attachedEntities[base.Agent.MobCommand.TargetEntity.CombatEntityUID] = SpearDestinationModifier.CreateEmptySides());
		return list;
	}

	// Token: 0x06001380 RID: 4992 RVA: 0x0005EDA0 File Offset: 0x0005CFA0
	private void SelectBestAttackPosition()
	{
		Vector3 position = base.Agent.MobCommand.Position;
		Vector3 position2 = base.Wgo.Data.Position;
		this.TryFreeOwnedSide(base.Agent.Wgo);
		float pikeAttackOffset = this.PikeAttackOffset;
		Vector3[] array = new Vector3[]
		{
			position + Vector3.right * pikeAttackOffset,
			position + Vector3.forward * pikeAttackOffset,
			position + Vector3.left * pikeAttackOffset,
			position + Vector3.back * pikeAttackOffset
		};
		List<SpearDestinationModifier.SideWithOwner> orCreateListWithOwners = this.GetOrCreateListWithOwners();
		bool flag;
		if (base.Agent.MobCommand.TargetEntity != null)
		{
			if (orCreateListWithOwners == null)
			{
				flag = false;
			}
			else
			{
				flag = orCreateListWithOwners.Any((SpearDestinationModifier.SideWithOwner x) => x.Owner == SGuid.Empty);
			}
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		Vector3 vector;
		int num;
		if (!this.TryPickSlot(array, orCreateListWithOwners, flag2, position2, true, out vector, out num) && !this.TryPickSlot(array, orCreateListWithOwners, flag2, position2, false, out vector, out num))
		{
			vector = position;
			num = -1;
		}
		if (num >= 0)
		{
			this.SetNewOwner(orCreateListWithOwners, base.Agent.Wgo.CombatEntityUID, num);
		}
		this.selectedAttackPositionLocal = vector - position;
		this.targetPositionWhenSelected = position;
		this.positionSelected = true;
		this.positionDwellElapsed = 0f;
	}

	// Token: 0x06001381 RID: 4993 RVA: 0x0005EF0C File Offset: 0x0005D10C
	private bool TryPickSlot(Vector3[] attackPositions, List<SpearDestinationModifier.SideWithOwner> sideWithOwnersLocal, bool hasAtLeastOneFreeSide, Vector3 agentPos, bool requireReachable, out Vector3 bestPosition, out int bestSideIndex)
	{
		bestPosition = default(Vector3);
		bestSideIndex = -1;
		float num = float.MaxValue;
		for (int i = 0; i < attackPositions.Length; i++)
		{
			SpearDestinationModifier.SideWithOwner sideWithOwner = ((sideWithOwnersLocal != null) ? sideWithOwnersLocal[i] : default(SpearDestinationModifier.SideWithOwner));
			if (!hasAtLeastOneFreeSide || !(sideWithOwner.Owner != SGuid.Empty))
			{
				Vector3 vector = attackPositions[i];
				if (!requireReachable || this.TryGetReachableSlotPosition(vector, out vector))
				{
					float magnitude = (vector - agentPos).XZ().magnitude;
					if (magnitude < num)
					{
						num = magnitude;
						bestPosition = vector;
						bestSideIndex = i;
					}
				}
			}
		}
		return bestSideIndex >= 0;
	}

	// Token: 0x06001382 RID: 4994 RVA: 0x0005EFB4 File Offset: 0x0005D1B4
	private bool TryGetReachableSlotPosition(Vector3 candidate, out Vector3 onGraph)
	{
		onGraph = candidate;
		FightingGameController instance = LazySingleton<FightingGameController>.Instance;
		RecastGraph recastGraph = ((instance != null) ? instance.RecastGraph : null);
		if (recastGraph == null || AstarPath.active == null)
		{
			return true;
		}
		NearestNodeConstraint walkable = NearestNodeConstraint.Walkable;
		walkable.graphMask = GraphMask.FromGraph(recastGraph);
		walkable.distanceMetric = DistanceMetric.ClosestAsSeenFromAbove();
		GraphNode graphNode = recastGraph.PointOnNavmesh(candidate, walkable);
		if (graphNode == null || !graphNode.Walkable)
		{
			return false;
		}
		NNInfo nearest = recastGraph.GetNearest(base.Wgo.Data.Position, walkable);
		if (nearest.node == null || !nearest.node.Walkable)
		{
			return false;
		}
		if (!PathUtilities.IsPathPossible(nearest.node, graphNode))
		{
			return false;
		}
		onGraph = candidate;
		return true;
	}

	// Token: 0x06001383 RID: 4995 RVA: 0x0005F068 File Offset: 0x0005D268
	public override GoToDestinationModifierDebugInfo GetDebugInfo()
	{
		GoToDestinationModifierDebugInfo debugInfo = base.GetDebugInfo();
		debugInfo.SpearPositionSelected = this.positionSelected;
		debugInfo.SpearAttackOffsetLocal = this.selectedAttackPositionLocal;
		debugInfo.SpearTargetPositionWhenSelected = this.targetPositionWhenSelected;
		return debugInfo;
	}

	// Token: 0x040014B4 RID: 5300
	private const float POSITION_DWELL = 0.3f;

	// Token: 0x040014B5 RID: 5301
	private Vector3 selectedAttackPositionLocal;

	// Token: 0x040014B6 RID: 5302
	private Vector3 targetPositionWhenSelected;

	// Token: 0x040014B7 RID: 5303
	private bool positionSelected;

	// Token: 0x040014B8 RID: 5304
	private float positionDwellElapsed;

	// Token: 0x040014B9 RID: 5305
	private static Dictionary<SGuid, List<SpearDestinationModifier.SideWithOwner>> attachedEntities = new Dictionary<SGuid, List<SpearDestinationModifier.SideWithOwner>>();

	// Token: 0x020002E6 RID: 742
	private struct SideWithOwner
	{
		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06001386 RID: 4998 RVA: 0x0005F0B8 File Offset: 0x0005D2B8
		// (set) Token: 0x06001387 RID: 4999 RVA: 0x0005F0C0 File Offset: 0x0005D2C0
		public Direction Direction { readonly get; private set; }

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06001388 RID: 5000 RVA: 0x0005F0C9 File Offset: 0x0005D2C9
		// (set) Token: 0x06001389 RID: 5001 RVA: 0x0005F0D1 File Offset: 0x0005D2D1
		public SGuid Owner { readonly get; set; }

		// Token: 0x0600138A RID: 5002 RVA: 0x0005F0DA File Offset: 0x0005D2DA
		public SideWithOwner(Direction direction, SGuid owner)
		{
			this.Direction = direction;
			this.Owner = owner;
		}
	}
}
