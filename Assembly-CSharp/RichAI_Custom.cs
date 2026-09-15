using System;
using Pathfinding;
using UnityEngine;

// Token: 0x0200032D RID: 813
public class RichAI_Custom : RichAI
{
	// Token: 0x170003BA RID: 954
	// (get) Token: 0x060015AA RID: 5546 RVA: 0x000697B2 File Offset: 0x000679B2
	// (set) Token: 0x060015AB RID: 5547 RVA: 0x000697BA File Offset: 0x000679BA
	public bool GroundSnapEnabled { get; set; } = true;

	// Token: 0x170003BB RID: 955
	// (get) Token: 0x060015AC RID: 5548 RVA: 0x000697C3 File Offset: 0x000679C3
	// (set) Token: 0x060015AD RID: 5549 RVA: 0x000697CB File Offset: 0x000679CB
	public bool RequiredRepath { get; private set; }

	// Token: 0x170003BC RID: 956
	// (get) Token: 0x060015AE RID: 5550 RVA: 0x000697D4 File Offset: 0x000679D4
	// (set) Token: 0x060015AF RID: 5551 RVA: 0x000697DC File Offset: 0x000679DC
	public bool IsMovementPaused { get; private set; }

	// Token: 0x060015B0 RID: 5552 RVA: 0x000697E5 File Offset: 0x000679E5
	public void SetMovementPaused(bool paused)
	{
		this.IsMovementPaused = paused;
	}

	// Token: 0x060015B1 RID: 5553 RVA: 0x000697EE File Offset: 0x000679EE
	protected override void OnUpdate(float dt)
	{
		if (this.IsMovementPaused)
		{
			return;
		}
		base.OnUpdate(dt);
	}

	// Token: 0x060015B2 RID: 5554 RVA: 0x00069800 File Offset: 0x00067A00
	public override void SearchPath()
	{
		base.SearchPath();
		this.RequiredRepath = false;
	}

	// Token: 0x060015B3 RID: 5555 RVA: 0x00069810 File Offset: 0x00067A10
	protected override Vector3 ClampToNavmesh(Vector3 position, out bool positionChanged)
	{
		if (!this.GroundSnapEnabled)
		{
			return base.ClampToNavmesh(position, out positionChanged);
		}
		position = base.ClampToNavmesh(position, out positionChanged);
		float y = position.y;
		NNInfo nearestOnCurrentFloor = this.GetNearestOnCurrentFloor(position);
		if (nearestOnCurrentFloor.node == null)
		{
			return position;
		}
		Vector2 vector = this.movementPlane.ToPlane(nearestOnCurrentFloor.position - position);
		float sqrMagnitude = vector.sqrMagnitude;
		if (sqrMagnitude > 1.0000001E-06f)
		{
			this.velocity2D -= vector * Vector2.Dot(vector, this.velocity2D) / sqrMagnitude;
			position += this.movementPlane.ToWorld(vector, 0f);
			positionChanged = true;
		}
		float y2 = nearestOnCurrentFloor.position.y;
		position.y = y2;
		if (!positionChanged)
		{
			positionChanged = !y2.EqualsTo(y, 0.001f);
		}
		return position;
	}

	// Token: 0x060015B4 RID: 5556 RVA: 0x000698EB File Offset: 0x00067AEB
	protected override Vector3 ClampPositionToGraph(Vector3 newPosition)
	{
		return this.SnapToSeekerGraph(newPosition);
	}

	// Token: 0x060015B5 RID: 5557 RVA: 0x000698F4 File Offset: 0x00067AF4
	public Vector3 SnapToSeekerGraph(Vector3 worldPos)
	{
		if (!this.GroundSnapEnabled)
		{
			return worldPos;
		}
		NNInfo nearestOnCurrentFloor = this.GetNearestOnCurrentFloor(worldPos);
		if (nearestOnCurrentFloor.node == null)
		{
			return worldPos;
		}
		float num;
		this.movementPlane.ToPlane(worldPos, out num);
		return this.movementPlane.ToWorld(this.movementPlane.ToPlane(nearestOnCurrentFloor.position), num);
	}

	// Token: 0x060015B6 RID: 5558 RVA: 0x0006994C File Offset: 0x00067B4C
	private NNInfo GetNearestOnCurrentFloor(Vector3 worldPos)
	{
		AstarPath active = AstarPath.active;
		if (active == null)
		{
			return NNInfo.Empty;
		}
		this.movementPlane.ToPlane(worldPos, out this.snapQueryElevation);
		if (this.currentFloorNodeFilter == null)
		{
			this.currentFloorNodeFilter = new Func<GraphNode, bool>(this.IsNodeOnCurrentFloor);
		}
		NearestNodeConstraint walkable = NearestNodeConstraint.Walkable;
		if (this.seeker)
		{
			walkable.graphMask = this.seeker.graphMask;
			walkable.tags = this.seeker.traversableTags;
		}
		walkable.distanceMetric = DistanceMetric.ClosestAsSeenFromAbove();
		walkable.filter = this.currentFloorNodeFilter;
		NNInfo nearest = active.GetNearest(worldPos, walkable);
		if (nearest.node == null)
		{
			return NNInfo.Empty;
		}
		float num;
		this.movementPlane.ToPlane(nearest.position, out num);
		if (Mathf.Abs(num - this.snapQueryElevation) > this.maxVerticalSnapDelta)
		{
			return NNInfo.Empty;
		}
		return nearest;
	}

	// Token: 0x060015B7 RID: 5559 RVA: 0x00069A34 File Offset: 0x00067C34
	private bool IsNodeOnCurrentFloor(GraphNode node)
	{
		float num;
		this.movementPlane.ToPlane((Vector3)node.position, out num);
		return Mathf.Abs(num - this.snapQueryElevation) <= this.maxVerticalSnapDelta;
	}

	// Token: 0x060015B8 RID: 5560 RVA: 0x00069A72 File Offset: 0x00067C72
	public Vector3 ApplySeekerGraphSnap(Vector3 worldPos)
	{
		worldPos = this.SnapToSeekerGraph(worldPos);
		this.prevPosition2 = this.prevPosition1;
		this.prevPosition1 = this.simulatedPosition;
		this.simulatedPosition = worldPos;
		return worldPos;
	}

	// Token: 0x060015B9 RID: 5561 RVA: 0x00069AA0 File Offset: 0x00067CA0
	protected override Vector3 UpdateTarget(RichFunnel fn)
	{
		this.nextCorners.Clear();
		bool flag;
		Vector3 vector = fn.Update(this.simulatedPosition, this.nextCorners, 2, out this.lastCorner, out flag);
		if (!this.RequiredRepath && flag && !this.waitingForPathCalculation)
		{
			this.RequiredRepath = true;
		}
		return vector;
	}

	// Token: 0x04001623 RID: 5667
	[Header("Custom Stuck Detection")]
	[Tooltip("Time in seconds after which the agent is considered stuck if it hasn't reached the next steering target.")]
	public float stuckThreshold = 2f;

	// Token: 0x04001624 RID: 5668
	[Tooltip("Cooldown after a forced repath to avoid frequent recalculations.")]
	public float repathCooldownDuration = 1f;

	// Token: 0x04001625 RID: 5669
	[Tooltip("Max elevation change (meters) allowed when snapping to the seeker recast graph. Larger jumps are treated as a different floor (e.g. cobblestones under a bridge) so the agent stays on the current floor instead of falling through navmesh holes.")]
	public float maxVerticalSnapDelta = 1.25f;

	// Token: 0x04001628 RID: 5672
	private float snapQueryElevation;

	// Token: 0x04001629 RID: 5673
	private Func<GraphNode, bool> currentFloorNodeFilter;
}
