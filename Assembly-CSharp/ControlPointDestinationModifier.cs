using System;
using Pathfinding;
using UnityEngine;

// Token: 0x020002E2 RID: 738
public class ControlPointDestinationModifier : GoToDestinationModifier
{
	// Token: 0x17000348 RID: 840
	// (get) Token: 0x0600134C RID: 4940 RVA: 0x0005E5AE File Offset: 0x0005C7AE
	protected FightingCapturePoint TargetControlPoint
	{
		get
		{
			return this.targetControlPoint;
		}
	}

	// Token: 0x17000349 RID: 841
	// (get) Token: 0x0600134D RID: 4941 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	protected virtual bool AcceptWaitingSlot
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700034A RID: 842
	// (get) Token: 0x0600134E RID: 4942 RVA: 0x0005E5B8 File Offset: 0x0005C7B8
	public override bool IsStucked
	{
		get
		{
			return this.hasResolvedPosition && (this.targetPosition - base.Wgo.Data.Position).XZ().magnitude < 0.05f;
		}
	}

	// Token: 0x1700034B RID: 843
	// (get) Token: 0x0600134F RID: 4943 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public override bool ShouldAnchorOnArrival
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001350 RID: 4944 RVA: 0x0005E5FE File Offset: 0x0005C7FE
	public ControlPointDestinationModifier(FightingCapturePoint capturePoint)
	{
		this.targetControlPoint = capturePoint;
	}

	// Token: 0x06001351 RID: 4945 RVA: 0x0005E60D File Offset: 0x0005C80D
	public override Vector3 GetCurrentTargetPosition()
	{
		if (!this.hasResolvedPosition)
		{
			this.ResolveTargetPosition();
		}
		return this.targetPosition;
	}

	// Token: 0x06001352 RID: 4946 RVA: 0x0005E623 File Offset: 0x0005C823
	public override bool TryGetTargetPositionForPathfinding(out ICombatEntity combatEntity, out Vector3 position)
	{
		combatEntity = null;
		if (!this.hasResolvedPosition)
		{
			this.ResolveTargetPosition();
		}
		position = this.targetPosition;
		return true;
	}

	// Token: 0x06001353 RID: 4947 RVA: 0x0005E643 File Offset: 0x0005C843
	public override void OnFinish()
	{
		this.hasResolvedPosition = false;
		if (this.targetControlPoint)
		{
			this.targetControlPoint.ReleaseSlot(base.Wgo.Data.UniqueId);
		}
		base.OnFinish();
	}

	// Token: 0x06001354 RID: 4948 RVA: 0x0005E67C File Offset: 0x0005C87C
	private void ResolveTargetPosition()
	{
		Vector3 vector;
		bool flag;
		if (this.targetControlPoint.TryReserveSlot(base.Wgo.Data.UniqueId, out vector, out flag, this.AcceptWaitingSlot))
		{
			this.targetPosition = vector;
		}
		else
		{
			this.targetPosition = this.GetAvailablePositionFromRecast();
		}
		this.hasResolvedPosition = true;
	}

	// Token: 0x06001355 RID: 4949 RVA: 0x0005E6CC File Offset: 0x0005C8CC
	private Vector3 GetAvailablePositionFromRecast()
	{
		Vector3 position = this.targetControlPoint.transform.position;
		float radius = this.targetControlPoint.Radius;
		RecastGraph recastGraph = AstarPath.active.graphs[12] as RecastGraph;
		NearestNodeConstraint walkable = NearestNodeConstraint.Walkable;
		walkable.graphMask = GraphMask.FromGraph(recastGraph);
		for (int i = 0; i < 24; i++)
		{
			Vector2 vector = global::UnityEngine.Random.insideUnitCircle * radius;
			Vector3 vector2 = new Vector3(position.x + vector.x, position.y, position.z + vector.y);
			GraphNode graphNode = recastGraph.PointOnNavmesh(vector2, walkable);
			if (graphNode != null)
			{
				Vector3 vector3 = (Vector3)graphNode.position;
				float num = vector3.x - position.x;
				float num2 = vector3.z - position.z;
				if (num * num + num2 * num2 <= radius * radius)
				{
					return vector3;
				}
			}
		}
		return AstarPath.active.GetNearest(position, walkable).position;
	}

	// Token: 0x06001356 RID: 4950 RVA: 0x0005E7CC File Offset: 0x0005C9CC
	public override GoToDestinationModifierDebugInfo GetDebugInfo()
	{
		GoToDestinationModifierDebugInfo debugInfo = base.GetDebugInfo();
		debugInfo.CapturePointName = (this.targetControlPoint ? this.targetControlPoint.name : null);
		debugInfo.ReservedSlotPosition = this.targetPosition;
		return debugInfo;
	}

	// Token: 0x0400149D RID: 5277
	private const float DISTANCE_TO_STOP_AS_STUCKED = 0.05f;

	// Token: 0x0400149E RID: 5278
	private FightingCapturePoint targetControlPoint;

	// Token: 0x0400149F RID: 5279
	private Vector3 targetPosition;

	// Token: 0x040014A0 RID: 5280
	private bool hasResolvedPosition;
}
