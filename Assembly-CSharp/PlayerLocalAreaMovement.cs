using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

// Token: 0x02000395 RID: 917
public class PlayerLocalAreaMovement : MonoBehaviour
{
	// Token: 0x1700042D RID: 1069
	// (get) Token: 0x0600188D RID: 6285 RVA: 0x00074491 File Offset: 0x00072691
	public MovementType MovementType
	{
		get
		{
			return this.movementType;
		}
	}

	// Token: 0x1700042E RID: 1070
	// (get) Token: 0x0600188E RID: 6286 RVA: 0x00074499 File Offset: 0x00072699
	public PlayerLocalAreaMovement.MovementStatus Status
	{
		get
		{
			return this.movementStatus;
		}
	}

	// Token: 0x1700042F RID: 1071
	// (get) Token: 0x0600188F RID: 6287 RVA: 0x000744A1 File Offset: 0x000726A1
	public Seeker Seeker
	{
		get
		{
			return this.seeker;
		}
	}

	// Token: 0x17000430 RID: 1072
	// (get) Token: 0x06001890 RID: 6288 RVA: 0x000744A9 File Offset: 0x000726A9
	public bool IsMovementStarted
	{
		get
		{
			return this.movementStatus == PlayerLocalAreaMovement.MovementStatus.CalcPath || this.movementStatus == PlayerLocalAreaMovement.MovementStatus.MovingToTarget;
		}
	}

	// Token: 0x06001891 RID: 6289 RVA: 0x000744BF File Offset: 0x000726BF
	private void Awake()
	{
		this.seeker = base.GetComponent<Seeker>();
		this.seeker.graphMask = new GraphMask(8U);
	}

	// Token: 0x06001892 RID: 6290 RVA: 0x000744DE File Offset: 0x000726DE
	public void Init(PlayerPhysicalBody playerPhysics)
	{
		this.playerPhysics = playerPhysics;
	}

	// Token: 0x06001893 RID: 6291 RVA: 0x000744E8 File Offset: 0x000726E8
	public void StartMovement(Vector3 endPosition, Direction arrivalFacingDirection = Direction.None)
	{
		PlayerLocalAreaMovement.<>c__DisplayClass22_0 CS$<>8__locals1 = new PlayerLocalAreaMovement.<>c__DisplayClass22_0();
		CS$<>8__locals1.<>4__this = this;
		PlayerLocalAreaMovement.<>c__DisplayClass22_0 CS$<>8__locals2 = CS$<>8__locals1;
		int num = this.pathRequestVersion + 1;
		this.pathRequestVersion = num;
		CS$<>8__locals2.requestVersion = num;
		this.arrivalFacingDirection = arrivalFacingDirection;
		this.seeker.graphMask = new GraphMask(8U);
		this.movementStatus = PlayerLocalAreaMovement.MovementStatus.CalcPath;
		this.startPosition = base.transform.position;
		this.endPosition = endPosition;
		this.RescanPlayerGraph();
		if (!this.IsReachable(endPosition))
		{
			Debug.LogError("Can not calculate path");
			this.StopMovement(true);
			return;
		}
		this.seeker.StartPath(this.startPosition, endPosition, delegate(Path path)
		{
			CS$<>8__locals1.<>4__this.OnPathFound(path, CS$<>8__locals1.requestVersion);
		});
	}

	// Token: 0x06001894 RID: 6292 RVA: 0x00074590 File Offset: 0x00072790
	public void StopMovement(bool force = false)
	{
		if (this.movementType == MovementType.Recast || force)
		{
			this.pathRequestVersion++;
			this.movementStatus = PlayerLocalAreaMovement.MovementStatus.NotMoving;
			this.playerPhysics.SetNonKinematicFlag(PlayerDynamicType.ByMovementComponent, true);
			this.currentPath = null;
			this.currentWaypointIndex = 0;
			this.movementType = MovementType.None;
			this.arrivalFacingDirection = Direction.None;
			this.playerPhysics.PlayerView.PlayerAnimation.SetState(global::AnimationState.Idle);
		}
	}

	// Token: 0x06001895 RID: 6293 RVA: 0x00074600 File Offset: 0x00072800
	public void CustomUpdate(float delta)
	{
		if (this.currentPath != null)
		{
			Vector3 vector = this.playerPhysics.transform.position;
			float num = delta * 3.3f;
			while (this.currentWaypointIndex < this.currentPath.Count)
			{
				Vector3 vector2 = this.currentPath[this.currentWaypointIndex];
				Vector3 normalized = (vector2 - vector).normalized;
				float magnitude = (vector - vector2).magnitude;
				if (num <= magnitude)
				{
					vector += num * normalized;
					this.playerPhysics.MoveByPosition(vector, normalized.XZ2(), true);
					return;
				}
				num -= magnitude;
				vector = vector2;
				this.currentWaypointIndex++;
				if (this.currentWaypointIndex == this.currentPath.Count)
				{
					this.playerPhysics.SetPosition(vector);
				}
			}
			this.ApplyArrivalFacing();
			this.StopMovement(false);
			return;
		}
	}

	// Token: 0x06001896 RID: 6294 RVA: 0x000746EC File Offset: 0x000728EC
	public bool IsReachable(Vector3 position)
	{
		GridGraph gridGraph = (GridGraph)AstarPath.active.data.graphs[3];
		NNInfo nearest = gridGraph.GetNearest(base.transform.position, NNConstraint.Walkable);
		NNInfo nearest2 = gridGraph.GetNearest(position);
		return PlayerColliderTester.IsPositionReachable(nearest2.position) && (PathUtilities.IsPathPossible(nearest.node, nearest2.node) || (base.transform.position - position).sqrMagnitude < 0.01f);
	}

	// Token: 0x06001897 RID: 6295 RVA: 0x00074771 File Offset: 0x00072971
	private void ApplyArrivalFacing()
	{
		if (this.arrivalFacingDirection == Direction.None)
		{
			return;
		}
		this.playerPhysics.SetFacingDirection(this.arrivalFacingDirection.ConvertToVector2XZ());
	}

	// Token: 0x06001898 RID: 6296 RVA: 0x00074794 File Offset: 0x00072994
	private void OnPathFound(Path p, int requestVersion)
	{
		if (requestVersion != this.pathRequestVersion || this.movementStatus != PlayerLocalAreaMovement.MovementStatus.CalcPath)
		{
			return;
		}
		if (p.CompleteState != PathCompleteState.Complete || p.path.Count == 0)
		{
			Debug.LogError("Can not calculate path");
			this.StopMovement(true);
			return;
		}
		this.movementStatus = PlayerLocalAreaMovement.MovementStatus.MovingToTarget;
		this.playerPhysics.SetNonKinematicFlag(PlayerDynamicType.ByMovementComponent, false);
		this.movementType = MovementType.Recast;
		this.currentPath = p.vectorPath;
		List<Vector3> list = this.currentPath;
		list[list.Count - 1] = this.endPosition;
		this.currentWaypointIndex = 0;
	}

	// Token: 0x06001899 RID: 6297 RVA: 0x00074824 File Offset: 0x00072A24
	public void RescanPlayerGraph()
	{
		GridGraph gridGraph = (GridGraph)AstarPath.active.data.graphs[3];
		int num = Mathf.RoundToInt(44f);
		gridGraph.center = VisualConsts.GetRoundedPosXYZ(base.transform.position, 2, 10);
		gridGraph.collision.diameter = 2.8f;
		gridGraph.SetDimensions(num, num, 0.1f);
		gridGraph.Scan();
	}

	// Token: 0x04001815 RID: 6165
	private const float PLAYER_AI_MOVEMENT_SPEED = 3.3f;

	// Token: 0x04001816 RID: 6166
	private MovementType movementType;

	// Token: 0x04001817 RID: 6167
	private Vector3 endPosition;

	// Token: 0x04001818 RID: 6168
	private Vector3 startPosition;

	// Token: 0x04001819 RID: 6169
	private List<Vector3> currentPath;

	// Token: 0x0400181A RID: 6170
	private int currentWaypointIndex;

	// Token: 0x0400181B RID: 6171
	private Seeker seeker;

	// Token: 0x0400181C RID: 6172
	private PlayerPhysicalBody playerPhysics;

	// Token: 0x0400181D RID: 6173
	private PlayerLocalAreaMovement.MovementStatus movementStatus;

	// Token: 0x0400181E RID: 6174
	private Direction arrivalFacingDirection;

	// Token: 0x0400181F RID: 6175
	private int pathRequestVersion;

	// Token: 0x02000396 RID: 918
	public enum MovementStatus
	{
		// Token: 0x04001821 RID: 6177
		NotMoving,
		// Token: 0x04001822 RID: 6178
		CalcPath,
		// Token: 0x04001823 RID: 6179
		MovingToTarget
	}
}
