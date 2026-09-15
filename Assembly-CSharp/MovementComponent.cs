using System;
using System.Collections.Generic;
using LazyBearTechnology;
using Pathfinding;
using UnityEngine;

// Token: 0x02000363 RID: 867
[Serializable]
public class MovementComponent
{
	// Token: 0x170003EF RID: 1007
	// (get) Token: 0x06001713 RID: 5907 RVA: 0x0006D92A File Offset: 0x0006BB2A
	// (set) Token: 0x06001714 RID: 5908 RVA: 0x0006D934 File Offset: 0x0006BB34
	private int CurrentMovementDataIndex
	{
		get
		{
			return this.currentMovementDataIndex;
		}
		set
		{
			if (value < 0 || value >= this.movementData.Count)
			{
				return;
			}
			this.currentMovementDataIndex = value;
			this.currentMovementData = this.movementData[this.currentMovementDataIndex];
			this.partialPathCount = this.currentMovementData.worldPath.Count - 1;
		}
	}

	// Token: 0x170003F0 RID: 1008
	// (get) Token: 0x06001715 RID: 5909 RVA: 0x0006D98A File Offset: 0x0006BB8A
	// (set) Token: 0x06001716 RID: 5910 RVA: 0x0006D992 File Offset: 0x0006BB92
	private int CurrentPathTargetIndex
	{
		get
		{
			return this.currentPathTargetIndex;
		}
		set
		{
			if (!this.IsValidPathTargetIndex(value))
			{
				return;
			}
			this.currentPathTargetIndex = value;
			this.currentPathTarget = this.movementData[this.currentMovementDataIndex].worldPath[this.currentPathTargetIndex];
		}
	}

	// Token: 0x170003F1 RID: 1009
	// (get) Token: 0x06001717 RID: 5911 RVA: 0x0006D9CC File Offset: 0x0006BBCC
	public string OnPathCompleteEvent
	{
		get
		{
			return this.onPathCompleteEvent;
		}
	}

	// Token: 0x170003F2 RID: 1010
	// (get) Token: 0x06001718 RID: 5912 RVA: 0x0006D9D4 File Offset: 0x0006BBD4
	public MovementComponent.CompletionState Completion
	{
		get
		{
			return this.completionState;
		}
	}

	// Token: 0x170003F3 RID: 1011
	// (get) Token: 0x06001719 RID: 5913 RVA: 0x0006D9DC File Offset: 0x0006BBDC
	public bool IsMoving
	{
		get
		{
			return this.status == MovementComponent.Status.Moving;
		}
	}

	// Token: 0x0600171A RID: 5914 RVA: 0x0006D9E7 File Offset: 0x0006BBE7
	public bool ShouldRegisterInMovementSystem()
	{
		return this.IsMoving;
	}

	// Token: 0x170003F4 RID: 1012
	// (get) Token: 0x0600171B RID: 5915 RVA: 0x0006D9EF File Offset: 0x0006BBEF
	public MovementComponent.DestinationType TypeDestination
	{
		get
		{
			return this.destinationType;
		}
	}

	// Token: 0x170003F5 RID: 1013
	// (get) Token: 0x0600171C RID: 5916 RVA: 0x0006D9F7 File Offset: 0x0006BBF7
	// (set) Token: 0x0600171D RID: 5917 RVA: 0x0006D9FF File Offset: 0x0006BBFF
	public bool IsPaused
	{
		get
		{
			return this.isPaused;
		}
		set
		{
			this.isPaused = value;
		}
	}

	// Token: 0x0600171E RID: 5918 RVA: 0x0006DA08 File Offset: 0x0006BC08
	public void Init(IMovable movableObject)
	{
		this.assignedMovableObject = movableObject;
		if (this.IsMoving)
		{
			this.UpdatePathData();
		}
		MovementComponent.Status status = this.status;
		if (status == MovementComponent.Status.PathBuilding)
		{
			this.DoPathCalculations();
			return;
		}
		if (status != MovementComponent.Status.Moving)
		{
			return;
		}
		this.UpdatePathData();
	}

	// Token: 0x0600171F RID: 5919 RVA: 0x0006DA47 File Offset: 0x0006BC47
	public void SetCallbacks(Action<float> onStartAction = null, Action onFinishAction = null, Action<Vector2> onDirectionChange = null)
	{
		this.onStartAction = onStartAction;
		this.onFinishAction = onFinishAction;
		this.onDirectionChange = onDirectionChange;
		if (this.IsMoving)
		{
			Action<float> action = this.onStartAction;
			if (action == null)
			{
				return;
			}
			action(this.speed);
		}
	}

	// Token: 0x06001720 RID: 5920 RVA: 0x0006DA7C File Offset: 0x0006BC7C
	public void SetOnPathLengthReady(Action<float> callback)
	{
		this.onPathLengthReady = callback;
	}

	// Token: 0x06001721 RID: 5921 RVA: 0x0006DA85 File Offset: 0x0006BC85
	public void DeInit()
	{
		this.onStartAction = null;
		this.onFinishAction = null;
		this.onDirectionChange = null;
		this.onPathLengthReady = null;
	}

	// Token: 0x06001722 RID: 5922 RVA: 0x0006DAA3 File Offset: 0x0006BCA3
	public void Update(float deltaTime)
	{
		if (this.isPaused || this.status != MovementComponent.Status.Moving)
		{
			return;
		}
		Action<float> action = this.moveMethod;
		if (action == null)
		{
			return;
		}
		action(deltaTime);
	}

	// Token: 0x06001723 RID: 5923 RVA: 0x0006DAC8 File Offset: 0x0006BCC8
	public MovementComponent.StartPathResult StartPath(Vector3 endPos, DockPointData.Baked endDockPointData, string startWorldId, string destinationWorldId, MovementType movementType = MovementType.Recast, float speed = 1.5f, string onPathCompleteEvent = "", Action onFinish = null, Seeker customSeeker = null)
	{
		MovementComponent.StartPathResult startPathResult = this.StartPath(endPos, startWorldId, destinationWorldId, movementType, speed, onPathCompleteEvent, onFinish, customSeeker, MovementComponent.DestinationType.DockPoint);
		if (startPathResult == MovementComponent.StartPathResult.Started)
		{
			this.endDockPointData = endDockPointData;
		}
		return startPathResult;
	}

	// Token: 0x06001724 RID: 5924 RVA: 0x0006DAF4 File Offset: 0x0006BCF4
	public MovementComponent.StartPathResult StartPath(NPCPointOfInterestData targetPoint, float speed = 1.5f)
	{
		GDPointData gdpointData = targetPoint.GDPointData;
		if (gdpointData == null)
		{
			return MovementComponent.StartPathResult.IncorrectMovementType;
		}
		MovementComponent.StartPathResult startPathResult = this.StartPath(gdpointData.Position, gdpointData.GameSceneDataId, gdpointData.GameSceneDataId, MovementType.GDGraph, speed, string.Empty, null, null, MovementComponent.DestinationType.PointOfInterest);
		if (startPathResult == MovementComponent.StartPathResult.Started)
		{
			this.targetPointOfInterest = targetPoint;
		}
		return startPathResult;
	}

	// Token: 0x06001725 RID: 5925 RVA: 0x0006DB3C File Offset: 0x0006BD3C
	public MovementComponent.StartPathResult StartPath(Vector3 endPos, string startWorldId, string destinationWorldId, MovementType movementType = MovementType.Recast, float speed = 1.5f, string onPathCompleteEvent = "", Action onFinish = null, Seeker customSeeker = null, MovementComponent.DestinationType destinationType = MovementComponent.DestinationType.Position)
	{
		if (this.TryFinishAlreadyAtDestination(endPos, movementType, onFinish))
		{
			return MovementComponent.StartPathResult.AlreadyAtDestinationPoint;
		}
		if (movementType == MovementType.None || movementType == MovementType.WorldZone)
		{
			Debug.LogError(string.Format("MC: [{0}] cannot start path with movement type [{1}]", this.assignedMovableObject.MovableObjectId, movementType));
			if (onFinish != null)
			{
				onFinish();
			}
			return MovementComponent.StartPathResult.IncorrectMovementType;
		}
		this.completionState = MovementComponent.CompletionState.None;
		this.PrepareForNewPath();
		this.movementData.Clear();
		this.startPos = this.assignedMovableObject.MovablePosition;
		this.endPos = endPos;
		this.endWorldId = destinationWorldId;
		this.movementType = movementType;
		this.startWorldId = startWorldId;
		this.currentWorldId = this.startWorldId;
		this.speed = speed;
		this.onPathCompleteEvent = onPathCompleteEvent;
		this.onFinishPath = onFinish;
		this.seeker = customSeeker;
		this.destinationType = destinationType;
		this.DoPathCalculations();
		this.assignedMovableObject.OnPathStart();
		return MovementComponent.StartPathResult.Started;
	}

	// Token: 0x06001726 RID: 5926 RVA: 0x0006DC1B File Offset: 0x0006BE1B
	public MovementComponent.StartPathResult StartPath(Vector3 endPos, DockPointData.Baked endDockPointData, LazyConsts.Navigation.Graph graph, string startWorldId, float speed = 1.5f, string onPathCompleteEvent = "", Action onFinish = null)
	{
		return this.StartPath(endPos, endDockPointData, NavigationGraphMaskUtils.ToGraphMask(graph), startWorldId, speed, onPathCompleteEvent, onFinish);
	}

	// Token: 0x06001727 RID: 5927 RVA: 0x0006DC33 File Offset: 0x0006BE33
	public MovementComponent.StartPathResult StartPath(Vector3 endPos, DockPointData.Baked endDockPointData, GraphMask graphMask, string startWorldId, float speed = 1.5f, string onPathCompleteEvent = "", Action onFinish = null)
	{
		MovementComponent.StartPathResult startPathResult = this.StartPath(endPos, graphMask, startWorldId, speed, onPathCompleteEvent, onFinish, MovementComponent.DestinationType.DockPoint);
		if (startPathResult == MovementComponent.StartPathResult.Started)
		{
			this.endDockPointData = endDockPointData;
		}
		return startPathResult;
	}

	// Token: 0x06001728 RID: 5928 RVA: 0x0006DC50 File Offset: 0x0006BE50
	public MovementComponent.StartPathResult StartPath(Vector3 endPos, LazyConsts.Navigation.Graph graph, string startWorldId, float speed = 1.5f, string onPathCompleteEvent = "", Action onFinish = null, MovementComponent.DestinationType destinationType = MovementComponent.DestinationType.Position)
	{
		return this.StartPath(endPos, NavigationGraphMaskUtils.ToGraphMask(graph), startWorldId, speed, onPathCompleteEvent, onFinish, destinationType);
	}

	// Token: 0x06001729 RID: 5929 RVA: 0x0006DC68 File Offset: 0x0006BE68
	public MovementComponent.StartPathResult StartPath(Vector3 endPos, GraphMask graphMask, string startWorldId, float speed = 1.5f, string onPathCompleteEvent = "", Action onFinish = null, MovementComponent.DestinationType destinationType = MovementComponent.DestinationType.Position)
	{
		if (this.TryFinishAlreadyAtDestination(endPos, MovementType.WorldZone, onFinish))
		{
			return MovementComponent.StartPathResult.AlreadyAtDestinationPoint;
		}
		this.completionState = MovementComponent.CompletionState.None;
		this.destinationType = destinationType;
		this.graphMask = graphMask;
		this.movementType = MovementType.WorldZone;
		this.PrepareForNewPath();
		this.movementData.Clear();
		this.startPos = this.assignedMovableObject.MovablePosition;
		this.endPos = endPos;
		this.startWorldId = startWorldId;
		this.endWorldId = startWorldId;
		this.currentWorldId = this.startWorldId;
		this.speed = speed;
		this.onPathCompleteEvent = onPathCompleteEvent;
		this.onFinishPath = onFinish;
		this.DoPathCalculations();
		this.assignedMovableObject.OnPathStart();
		return MovementComponent.StartPathResult.Started;
	}

	// Token: 0x0600172A RID: 5930 RVA: 0x0006DD10 File Offset: 0x0006BF10
	public void ForceStop()
	{
		Action action = this.onFinishAction;
		if (action != null)
		{
			action();
		}
		this.onFinishPath = null;
		this.onPathLengthReady = null;
		this.status = MovementComponent.Status.None;
		this.completionState = MovementComponent.CompletionState.Canceled;
		MainGame.Instance.movementSystem.RemoveMovingObject(this);
		this.assignedMovableObject.OnPathComplete(this);
	}

	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x0600172B RID: 5931 RVA: 0x0006DD68 File Offset: 0x0006BF68
	private bool UsesExactRequestedDestination
	{
		get
		{
			MovementType movementType = this.movementType;
			return movementType == MovementType.Direct || movementType == MovementType.GDGraph;
		}
	}

	// Token: 0x0600172C RID: 5932 RVA: 0x0006DD90 File Offset: 0x0006BF90
	private bool TryFinishAlreadyAtDestination(Vector3 destination, MovementType type, Action onFinish)
	{
		if (type == MovementType.Direct || type == MovementType.GDGraph)
		{
			if (!this.IsAtExactDestinationXZ(destination))
			{
				return false;
			}
		}
		else if ((this.assignedMovableObject.MovablePosition - destination).sqrMagnitude >= 0.010000001f)
		{
			return false;
		}
		if (onFinish != null)
		{
			onFinish();
		}
		return true;
	}

	// Token: 0x0600172D RID: 5933 RVA: 0x0006DDDC File Offset: 0x0006BFDC
	private bool IsAtExactDestinationXZ(Vector3 destination)
	{
		return (destination - this.assignedMovableObject.MovablePosition).XZ().sqrMagnitude <= 1.0000001E-06f;
	}

	// Token: 0x0600172E RID: 5934 RVA: 0x0006DE11 File Offset: 0x0006C011
	private void SnapToExactDestinationIfNeeded()
	{
		if (!this.UsesExactRequestedDestination)
		{
			return;
		}
		this.assignedMovableObject.MovablePositionWithoutDirectionChange = this.endPos;
	}

	// Token: 0x0600172F RID: 5935 RVA: 0x0006DE2D File Offset: 0x0006C02D
	private bool IsOnFinalDestinationWaypoint()
	{
		return this.currentPathTargetIndex == this.partialPathCount && this.currentMovementDataIndex == this.movementDataCount;
	}

	// Token: 0x06001730 RID: 5936 RVA: 0x0006DE50 File Offset: 0x0006C050
	private void AppendExactDestinationWaypoint(List<Vector3> worldPath)
	{
		if (this.movementType != MovementType.GDGraph)
		{
			worldPath.Add(this.endPos);
			return;
		}
		if (worldPath.Count == 0)
		{
			worldPath.Add(this.endPos);
			return;
		}
		if ((worldPath[worldPath.Count - 1] - this.endPos).sqrMagnitude <= 0.010000001f)
		{
			worldPath[worldPath.Count - 1] = this.endPos;
			return;
		}
		worldPath.Add(this.endPos);
	}

	// Token: 0x06001731 RID: 5937 RVA: 0x0006DED4 File Offset: 0x0006C0D4
	private void PrepareForNewPath()
	{
		this.pathRequestId += 1U;
		MainGame instance = MainGame.Instance;
		if (instance != null)
		{
			MovementSystem movementSystem = instance.movementSystem;
			if (movementSystem != null)
			{
				movementSystem.RemoveMovingObject(this);
			}
		}
		this.currentMovementDataIndex = 0;
		this.currentPathTargetIndex = 0;
		this.partialPathCount = 0;
		this.moveMethod = null;
	}

	// Token: 0x06001732 RID: 5938 RVA: 0x0006DF28 File Offset: 0x0006C128
	private bool IsValidPathTargetIndex(int index)
	{
		return index >= 0 && this.currentMovementDataIndex >= 0 && this.currentMovementDataIndex < this.movementData.Count && index < this.movementData[this.currentMovementDataIndex].worldPath.Count;
	}

	// Token: 0x06001733 RID: 5939 RVA: 0x0006DF75 File Offset: 0x0006C175
	private void OnPathCalculatedIfCurrent(uint requestId, Path p)
	{
		if (requestId != this.pathRequestId || this.status != MovementComponent.Status.PathBuilding)
		{
			return;
		}
		this.OnPathCalculated(p);
	}

	// Token: 0x06001734 RID: 5940 RVA: 0x0006DF94 File Offset: 0x0006C194
	private void FindPathRecastGraph(OnPathDelegate callback = null)
	{
		int num = GraphHelper.Instance.SceneGraphsData.GetRecastGraphIndexByWorldId(this.startWorldId)[0];
		LazyConsts.Navigation.Graph graph = (LazyConsts.Navigation.Graph)num;
		if (graph == LazyConsts.Navigation.Graph.None)
		{
			this.onPathLengthReady = null;
			return;
		}
		if (!this.IsReachable(this.startPos, this.endPos, num))
		{
			Debug.LogError(this.GetPathCalculationError());
			this.onPathLengthReady = null;
			return;
		}
		this.status = MovementComponent.Status.PathBuilding;
		uint requestId = this.pathRequestId;
		this.wholePath = new ABPath();
		this.wholePath.path = new List<GraphNode>();
		this.wholePath.vectorPath = new List<Vector3>();
		this.useVectorPathAsSource = true;
		LazySingleton<GlobalNavigationManager>.Instance.CalculatePath(graph, this.startPos, this.endPos, delegate(Path p)
		{
			this.OnPathCalculatedIfCurrent(requestId, p);
		});
	}

	// Token: 0x06001735 RID: 5941 RVA: 0x0006E068 File Offset: 0x0006C268
	private void FindPathGDGraph(OnPathDelegate callback = null)
	{
		this.status = MovementComponent.Status.PathBuilding;
		uint requestId = this.pathRequestId;
		this.wholePath = new ABPath();
		this.wholePath.path = new List<GraphNode>();
		this.wholePath.vectorPath = new List<Vector3>();
		ABPath abpath = ABPath.Construct(this.startPos, this.endPos, delegate(Path p)
		{
			if (requestId != this.pathRequestId)
			{
				return;
			}
			this.OnPathCalculated(p);
		});
		abpath.calculatePartial = true;
		abpath.traversalConstraint.graphMask = new GraphMask((uint)GraphHelper.Instance.SceneGraphsData.GDPointGraphMask);
		AstarPath.StartPath(abpath, false, false);
	}

	// Token: 0x06001736 RID: 5942 RVA: 0x0006E10C File Offset: 0x0006C30C
	private void FindPathWorldZone()
	{
		this.status = MovementComponent.Status.PathBuilding;
		uint requestId = this.pathRequestId;
		this.wholePath = new ABPath();
		this.wholePath.path = new List<GraphNode>();
		this.wholePath.vectorPath = new List<Vector3>();
		this.useVectorPathAsSource = true;
		LazySingleton<GlobalNavigationManager>.Instance.CalculatePath(this.graphMask, this.startPos, this.endPos, delegate(Path p)
		{
			this.OnPathCalculatedIfCurrent(requestId, p);
		});
	}

	// Token: 0x06001737 RID: 5943 RVA: 0x0006E194 File Offset: 0x0006C394
	private bool IsReachedDestinationPoint()
	{
		if (!this.IsOnFinalDestinationWaypoint())
		{
			return false;
		}
		if (!(this.UsesExactRequestedDestination ? this.IsAtExactDestinationXZ(this.currentPathTarget) : (Vector3.Distance(this.currentPathTarget, this.assignedMovableObject.MovablePosition) < 0.1f)))
		{
			return false;
		}
		this.OnDestinationReachSucceed();
		return true;
	}

	// Token: 0x06001738 RID: 5944 RVA: 0x0006E1EC File Offset: 0x0006C3EC
	private bool TransitToNextWorld()
	{
		if (Vector3.Distance(this.currentPathTarget, this.assignedMovableObject.MovablePosition) < 0.1f && this.currentPathTargetIndex == this.partialPathCount && this.currentMovementDataIndex != this.movementDataCount && !string.IsNullOrEmpty(this.currentMovementData.transitionWorldId))
		{
			this.assignedMovableObject.OnTransitionReached(this.currentWorldId, this.movementData[this.currentMovementDataIndex].transitionWorldId);
			this.currentWorldId = this.currentMovementData.transitionWorldId;
			int num = this.CurrentMovementDataIndex;
			this.CurrentMovementDataIndex = num + 1;
			this.assignedMovableObject.MovablePositionWithoutDirectionChange = this.currentMovementData.worldPath[0];
			this.CurrentPathTargetIndex = ((this.currentMovementData.worldPath.Count > 1) ? 1 : 0);
			return true;
		}
		return false;
	}

	// Token: 0x06001739 RID: 5945 RVA: 0x0006E2D4 File Offset: 0x0006C4D4
	private bool IsReachedNextWaypoint()
	{
		if (Vector3.Distance(this.currentPathTarget, this.assignedMovableObject.MovablePosition) < 0.1f && this.currentPathTargetIndex < this.partialPathCount)
		{
			int num = this.CurrentPathTargetIndex;
			this.CurrentPathTargetIndex = num + 1;
			return true;
		}
		return false;
	}

	// Token: 0x0600173A RID: 5946 RVA: 0x0006E31F File Offset: 0x0006C51F
	private bool IsAtCurrentPathTarget()
	{
		return Vector3.Distance(this.currentPathTarget, this.assignedMovableObject.MovablePosition) < 0.1f;
	}

	// Token: 0x0600173B RID: 5947 RVA: 0x0006E340 File Offset: 0x0006C540
	private bool ApplyMovementStepTowardTarget(ref float remainingStep, bool requireExactArrival = false)
	{
		bool flag = (requireExactArrival ? this.IsAtExactDestinationXZ(this.currentPathTarget) : this.IsAtCurrentPathTarget());
		if (remainingStep <= 0f || flag)
		{
			return flag;
		}
		Vector3 movablePosition = this.assignedMovableObject.MovablePosition;
		Vector3 vector = (requireExactArrival ? (this.currentPathTarget - movablePosition).XZ() : (this.currentPathTarget - movablePosition));
		float sqrMagnitude = vector.sqrMagnitude;
		float magnitude = vector.magnitude;
		Vector3 vector2 = ((magnitude > 0.0001f) ? (vector / magnitude) : Vector3.zero);
		Vector3 vector3;
		if ((remainingStep - magnitude).EqualsOrMore(0f, 1E-05f))
		{
			vector3 = (requireExactArrival ? (movablePosition + vector) : this.currentPathTarget);
			remainingStep -= magnitude;
		}
		else
		{
			vector3 = movablePosition + remainingStep * vector2;
			remainingStep = 0f;
		}
		this.prevDirection = this.assignedMovableObject.MovableDirection;
		if (sqrMagnitude > 0.010000001f)
		{
			Vector2 vector4 = new Vector2(vector2.x, vector2.z);
			this.assignedMovableObject.MovableDirection = vector4;
			this.CheckDirectionChanged(this.prevDirection, vector4);
			this.assignedMovableObject.MovablePosition = vector3;
		}
		else
		{
			this.assignedMovableObject.MovablePositionWithoutDirectionChange = vector3;
		}
		if (!requireExactArrival)
		{
			return this.IsAtCurrentPathTarget();
		}
		return this.IsAtExactDestinationXZ(this.currentPathTarget);
	}

	// Token: 0x0600173C RID: 5948 RVA: 0x0006E490 File Offset: 0x0006C690
	private bool ProcessPathTransitionsAtCurrentTarget(out bool pathFinished)
	{
		pathFinished = false;
		if (!((this.UsesExactRequestedDestination && this.IsOnFinalDestinationWaypoint()) ? this.IsAtExactDestinationXZ(this.currentPathTarget) : this.IsAtCurrentPathTarget()))
		{
			return false;
		}
		if (this.TransitToNextPoint())
		{
			return true;
		}
		if (this.IsReachedNextWaypoint())
		{
			return true;
		}
		if (this.IsReachedDestinationPoint())
		{
			pathFinished = true;
			return false;
		}
		return this.TransitToNextWorld();
	}

	// Token: 0x0600173D RID: 5949 RVA: 0x0006E4F4 File Offset: 0x0006C6F4
	private bool IsReachedDestinationDirect()
	{
		if (!this.IsAtExactDestinationXZ(this.endPos))
		{
			return false;
		}
		this.OnDestinationReachSucceed();
		return true;
	}

	// Token: 0x0600173E RID: 5950 RVA: 0x0006E510 File Offset: 0x0006C710
	private void MoveDirect(float deltaTime)
	{
		if (this.IsReachedDestinationDirect())
		{
			return;
		}
		Vector3 movablePosition = this.assignedMovableObject.MovablePosition;
		Vector3 vector = (this.endPos - movablePosition).XZ();
		float magnitude = vector.magnitude;
		float num = deltaTime * this.speed;
		if (magnitude <= 0.0001f)
		{
			this.OnDestinationReachSucceed();
			return;
		}
		Vector2 vector2 = vector.XZ2() / magnitude;
		this.prevDirection = this.assignedMovableObject.MovableDirection;
		this.assignedMovableObject.MovableDirection = vector2;
		this.CheckDirectionChanged(this.prevDirection, vector2);
		if ((num - magnitude).EqualsOrMore(0f, 1E-05f))
		{
			this.assignedMovableObject.MovablePosition = movablePosition + vector;
			this.OnDestinationReachSucceed();
			return;
		}
		this.assignedMovableObject.MovablePosition = movablePosition + num * vector2.XZ();
	}

	// Token: 0x0600173F RID: 5951 RVA: 0x0006E5EC File Offset: 0x0006C7EC
	private void MoveAStar(float deltaTime)
	{
		if (this.status != MovementComponent.Status.Moving || this.movementData.Count == 0)
		{
			return;
		}
		float num = deltaTime * this.speed;
		int num2 = 0;
		while (num2 < 64 && num.More(0f, 1E-05f))
		{
			bool flag = this.UsesExactRequestedDestination && this.IsOnFinalDestinationWaypoint();
			if ((!this.IsAtCurrentPathTarget() || (flag && !this.IsAtExactDestinationXZ(this.currentPathTarget))) && !this.ApplyMovementStepTowardTarget(ref num, flag))
			{
				break;
			}
			bool flag2;
			if (!this.ProcessPathTransitionsAtCurrentTarget(out flag2))
			{
				return;
			}
			num2++;
		}
	}

	// Token: 0x06001740 RID: 5952 RVA: 0x0006E67C File Offset: 0x0006C87C
	private void OnPathCalculated(Path p)
	{
		if (this.status != MovementComponent.Status.PathBuilding)
		{
			return;
		}
		Vector3 vector = this.endPos;
		if (p.CompleteState != PathCompleteState.Complete)
		{
			if (this.movementType == MovementType.GDGraph && this.TryMoveWithoutFullGDPath(p))
			{
				return;
			}
			Debug.LogError(this.GetPathCalculationError());
			this.OnDestinationReachFailed();
			return;
		}
		else
		{
			MovementType movementType = this.movementType;
			if (movementType == MovementType.WorldZone || movementType == MovementType.Recast)
			{
				List<Vector3> vectorPath = p.vectorPath;
				this.endPos = vectorPath[vectorPath.Count - 1];
			}
			this.wholePath.path.AddRange(p.path);
			this.wholePath.vectorPath = new List<Vector3>(p.vectorPath);
			this.CreateMovementData();
			this.NotifyPathLengthReady(p.GetTotalLength());
			Action<float> action = this.onStartAction;
			if (action == null)
			{
				return;
			}
			action(this.speed);
			return;
		}
	}

	// Token: 0x06001741 RID: 5953 RVA: 0x0006E744 File Offset: 0x0006C944
	private bool TryMoveWithoutFullGDPath(Path p)
	{
		if (p.CompleteState == PathCompleteState.Partial && p.path.Count > 0 && p.vectorPath.Count > 0)
		{
			List<GraphNode> path = p.path;
			if (!this.EndsInDestinationWorld(path[path.Count - 1]))
			{
				return false;
			}
			this.useVectorPathAsSource = false;
			this.wholePath.path.AddRange(p.path);
			this.wholePath.vectorPath = new List<Vector3>(p.vectorPath);
			float totalLength = p.GetTotalLength();
			List<Vector3> vectorPath = p.vectorPath;
			this.NotifyPathLengthReady(totalLength + Vector3.Distance(vectorPath[vectorPath.Count - 1], this.endPos));
		}
		else
		{
			if (this.startWorldId != this.endWorldId)
			{
				return false;
			}
			this.useVectorPathAsSource = true;
			this.wholePath.vectorPath = new List<Vector3>();
			this.NotifyPathLengthReady(Vector3.Distance(this.startPos, this.endPos));
		}
		this.CreateMovementData();
		Action<float> action = this.onStartAction;
		if (action != null)
		{
			action(this.speed);
		}
		return true;
	}

	// Token: 0x06001742 RID: 5954 RVA: 0x0006E85C File Offset: 0x0006CA5C
	private bool EndsInDestinationWorld(GraphNode node)
	{
		GDPointNode gdpointNode = node as GDPointNode;
		if (gdpointNode == null || gdpointNode.GdPointData == null)
		{
			return false;
		}
		string gameSceneDataId = gdpointNode.GdPointData.GameSceneDataId;
		return string.IsNullOrEmpty(gameSceneDataId) || gameSceneDataId == this.endWorldId;
	}

	// Token: 0x06001743 RID: 5955 RVA: 0x0006E8A0 File Offset: 0x0006CAA0
	private bool TransitToNextPoint()
	{
		if (Vector3.Distance(this.currentPathTarget, this.assignedMovableObject.MovablePosition) < 0.1f && this.currentPathTargetIndex == this.partialPathCount && this.currentMovementDataIndex != this.movementDataCount && this.currentMovementData.lastPointTransitToNextMovementData)
		{
			int num = this.CurrentMovementDataIndex;
			this.CurrentMovementDataIndex = num + 1;
			this.CurrentPathTargetIndex = ((this.currentMovementData.worldPath.Count > 1) ? 1 : 0);
			this.UpdatePathData();
			Vector3 movablePosition = this.assignedMovableObject.MovablePosition;
			this.assignedMovableObject.MovablePositionWithoutDirectionChange = this.currentMovementData.worldPath[0];
			this.assignedMovableObject.OnTeleportToTransitPoint(movablePosition, this.assignedMovableObject.MovablePosition);
			return true;
		}
		return false;
	}

	// Token: 0x06001744 RID: 5956 RVA: 0x0006E974 File Offset: 0x0006CB74
	private void CreateMovementData()
	{
		this.movementData.Clear();
		int num = 0;
		this.movementData.Add(new MovementData
		{
			worldPath = new List<Vector3>()
		});
		this.movementData[num].worldPath.Add(this.startPos);
		if (!this.useVectorPathAsSource)
		{
			this.movementData[num].worldPath.Add((Vector3)this.wholePath.path[0].position);
			int i = 1;
			while (i < this.wholePath.path.Count)
			{
				GDPointNode gdpointNode = this.wholePath.path[i] as GDPointNode;
				if (gdpointNode == null)
				{
					goto IL_019C;
				}
				if (!string.IsNullOrEmpty(gdpointNode.GdPointData.GameSceneDataIdToTransit))
				{
					this.movementData[num].transitionWorldId = gdpointNode.GdPointData.GameSceneDataIdToTransit;
					this.movementData[num].worldPath.Add((Vector3)gdpointNode.position);
					this.movementData.Add(new MovementData
					{
						worldPath = new List<Vector3>()
					});
					num++;
				}
				else
				{
					if (!string.IsNullOrEmpty(gdpointNode.GdPointData.GameSceneDataIdToTransit) || !gdpointNode.GdPointData.IsTransitPoint || string.IsNullOrEmpty(gdpointNode.GdPointData.TransitToGdPointId))
					{
						goto IL_019C;
					}
					this.movementData[num].lastPointTransitToNextMovementData = true;
					this.movementData[num].worldPath.Add((Vector3)gdpointNode.position);
					this.movementData.Add(new MovementData
					{
						worldPath = new List<Vector3>()
					});
					num++;
				}
				IL_01CD:
				i++;
				continue;
				IL_019C:
				this.movementData[num].worldPath.Add((Vector3)this.wholePath.path[i].position);
				goto IL_01CD;
			}
		}
		else
		{
			this.movementData[num].worldPath.AddRange(this.wholePath.vectorPath);
		}
		this.AppendExactDestinationWaypoint(this.movementData[num].worldPath);
		this.status = MovementComponent.Status.Moving;
		this.currentMovementDataIndex = 0;
		this.currentPathTargetIndex = 0;
		this.UpdatePathData();
		MainGame.Instance.movementSystem.AddMovingObject(this);
	}

	// Token: 0x06001745 RID: 5957 RVA: 0x0006EBD0 File Offset: 0x0006CDD0
	private void DoPathCalculations()
	{
		switch (this.movementType)
		{
		case MovementType.Direct:
		{
			this.status = MovementComponent.Status.Moving;
			this.moveMethod = new Action<float>(this.MoveDirect);
			this.NotifyPathLengthReady(Vector3.Distance(this.startPos, this.endPos));
			Action<float> action = this.onStartAction;
			if (action != null)
			{
				action(this.speed);
			}
			MainGame.Instance.movementSystem.AddMovingObject(this);
			return;
		}
		case MovementType.Recast:
			this.FindPathRecastGraph(null);
			return;
		case MovementType.GDGraph:
			this.FindPathGDGraph(null);
			return;
		case MovementType.WorldZone:
			this.FindPathWorldZone();
			return;
		default:
			return;
		}
	}

	// Token: 0x06001746 RID: 5958 RVA: 0x0006EC6C File Offset: 0x0006CE6C
	private void UpdatePathData()
	{
		MovementType movementType = this.movementType;
		if (movementType != MovementType.Direct)
		{
			if (movementType - MovementType.Recast <= 2)
			{
				if (this.movementData.Count == 0 || this.currentMovementDataIndex < 0 || this.currentMovementDataIndex >= this.movementData.Count || !this.IsValidPathTargetIndex(this.currentPathTargetIndex))
				{
					this.moveMethod = null;
					return;
				}
				this.currentMovementData = this.movementData[this.currentMovementDataIndex];
				this.currentPathTarget = this.currentMovementData.worldPath[this.currentPathTargetIndex];
				this.movementDataCount = this.movementData.Count - 1;
				this.partialPathCount = this.currentMovementData.worldPath.Count - 1;
				this.moveMethod = new Action<float>(this.MoveAStar);
				return;
			}
		}
		else
		{
			this.moveMethod = new Action<float>(this.MoveDirect);
		}
	}

	// Token: 0x06001747 RID: 5959 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	private bool CheckPathCalculationState(Path p)
	{
		return true;
	}

	// Token: 0x06001748 RID: 5960 RVA: 0x0006ED54 File Offset: 0x0006CF54
	private string GetPathCalculationError()
	{
		switch (this.movementType)
		{
		case MovementType.Recast:
			return "MC: Cannot calculate recast graph path for [" + this.assignedMovableObject.MovableObjectId + "]";
		case MovementType.GDGraph:
			return string.Format("MC: Cannot calculate gd point graph path for [{0}] from [{1}] to [{2}]", this.assignedMovableObject.MovableObjectId, this.startPos, this.endPos);
		case MovementType.WorldZone:
			return string.Format("MC: Cannot calculate world zone path for [{0}] in graph mask: [{1}]", this.assignedMovableObject.MovableObjectId, this.graphMask);
		default:
			return string.Format("MC: Wrong path calculation for type [{0}]", this.movementType);
		}
	}

	// Token: 0x06001749 RID: 5961 RVA: 0x0006EDFC File Offset: 0x0006CFFC
	private void OnDestinationReachSucceed()
	{
		this.SnapToExactDestinationIfNeeded();
		this.useVectorPathAsSource = false;
		this.status = MovementComponent.Status.None;
		this.completionState = MovementComponent.CompletionState.Success;
		Action action = this.onFinishAction;
		if (action != null)
		{
			action();
		}
		Action action2 = this.onFinishPath;
		if (action2 != null)
		{
			action2();
		}
		if (this.status == MovementComponent.Status.None)
		{
			this.onFinishPath = null;
		}
		MainGame.Instance.movementSystem.RemoveMovingObject(this);
		if (this.destinationType == MovementComponent.DestinationType.DockPoint)
		{
			Vector2 vector = this.endDockPointData.Direction.ConvertToVector2XZ();
			this.assignedMovableObject.MovableDirection = vector;
			Action<Vector2> action3 = this.onDirectionChange;
			if (action3 != null)
			{
				action3(vector);
			}
		}
		this.assignedMovableObject.OnPathComplete(this);
		GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.WgoGoToFinished, this.assignedMovableObject.MovableObjectId);
	}

	// Token: 0x0600174A RID: 5962 RVA: 0x0006EEBB File Offset: 0x0006D0BB
	private void NotifyPathLengthReady(float length)
	{
		Action<float> action = this.onPathLengthReady;
		this.onPathLengthReady = null;
		if (action == null)
		{
			return;
		}
		action(length);
	}

	// Token: 0x0600174B RID: 5963 RVA: 0x0006EED8 File Offset: 0x0006D0D8
	private void OnDestinationReachFailed()
	{
		this.useVectorPathAsSource = false;
		this.status = MovementComponent.Status.None;
		this.completionState = MovementComponent.CompletionState.Fail;
		this.onPathLengthReady = null;
		Action action = this.onFinishAction;
		if (action != null)
		{
			action();
		}
		Action action2 = this.onFinishPath;
		if (action2 != null)
		{
			action2();
		}
		if (this.status == MovementComponent.Status.None)
		{
			this.onFinishPath = null;
		}
		MainGame.Instance.movementSystem.RemoveMovingObject(this);
		Debug.LogError(string.Format("MC: [{0}] cannot reach destination: requested [{1}] from [{2}] type [{3}] in graph mask: [{4}]", new object[]
		{
			this.assignedMovableObject.MovableObjectId,
			this.endPos,
			this.startPos,
			this.movementType,
			this.graphMask
		}));
		this.assignedMovableObject.OnPathComplete(this);
	}

	// Token: 0x0600174C RID: 5964 RVA: 0x0006EFA9 File Offset: 0x0006D1A9
	private void CheckDirectionChanged(Vector2 prevDir, Vector2 curDir)
	{
		if (!Vector2.Dot(prevDir.normalized, curDir.normalized).EqualsTo(1f, 0.1f))
		{
			Action<Vector2> action = this.onDirectionChange;
			if (action == null)
			{
				return;
			}
			action(curDir);
		}
	}

	// Token: 0x0600174D RID: 5965 RVA: 0x0006EFE0 File Offset: 0x0006D1E0
	private bool IsReachable(Vector3 startPos, Vector3 endPos, int graphIndex)
	{
		NavGraph navGraph = AstarPath.active.data.graphs[graphIndex];
		NNInfo nearest = navGraph.GetNearest(startPos);
		NNInfo nearest2 = navGraph.GetNearest(endPos);
		return PathUtilities.IsPathPossible(nearest.node, nearest2.node);
	}

	// Token: 0x04001700 RID: 5888
	private const float TARGET_OFFSET = 0.1f;

	// Token: 0x04001701 RID: 5889
	private const float TARGET_OFFSET_SQR = 0.010000001f;

	// Token: 0x04001702 RID: 5890
	private const float EXACT_DEST_OFFSET_SQR = 1.0000001E-06f;

	// Token: 0x04001703 RID: 5891
	private const int MAX_PATH_ADVANCE_ITERATIONS = 64;

	// Token: 0x04001704 RID: 5892
	[NonSerialized]
	private IMovable assignedMovableObject;

	// Token: 0x04001705 RID: 5893
	[SerializeField]
	private List<MovementData> movementData = new List<MovementData>();

	// Token: 0x04001706 RID: 5894
	[SerializeField]
	private MovementComponent.DestinationType destinationType;

	// Token: 0x04001707 RID: 5895
	[SerializeField]
	private MovementComponent.Status status;

	// Token: 0x04001708 RID: 5896
	[SerializeField]
	private MovementComponent.CompletionState completionState;

	// Token: 0x04001709 RID: 5897
	[SerializeField]
	private int currentMovementDataIndex;

	// Token: 0x0400170A RID: 5898
	[SerializeField]
	private int currentPathTargetIndex;

	// Token: 0x0400170B RID: 5899
	[SerializeField]
	private Vector3 startPos;

	// Token: 0x0400170C RID: 5900
	[SerializeField]
	private Vector3 endPos;

	// Token: 0x0400170D RID: 5901
	[SerializeField]
	private DockPointData.Baked endDockPointData;

	// Token: 0x0400170E RID: 5902
	[SerializeField]
	private NPCPointOfInterestData targetPointOfInterest;

	// Token: 0x0400170F RID: 5903
	[SerializeField]
	private string startWorldId;

	// Token: 0x04001710 RID: 5904
	[SerializeField]
	private string endWorldId;

	// Token: 0x04001711 RID: 5905
	[SerializeField]
	private string currentWorldId;

	// Token: 0x04001712 RID: 5906
	[SerializeField]
	private float speed;

	// Token: 0x04001713 RID: 5907
	[SerializeField]
	private MovementType movementType;

	// Token: 0x04001714 RID: 5908
	[SerializeField]
	private string onPathCompleteEvent;

	// Token: 0x04001715 RID: 5909
	[SerializeField]
	private GraphMask graphMask;

	// Token: 0x04001716 RID: 5910
	[SerializeField]
	private bool useVectorPathAsSource;

	// Token: 0x04001717 RID: 5911
	[SerializeField]
	private bool isPaused;

	// Token: 0x04001718 RID: 5912
	private Action onFinishPath;

	// Token: 0x04001719 RID: 5913
	private ABPath wholePath;

	// Token: 0x0400171A RID: 5914
	private Seeker seeker;

	// Token: 0x0400171B RID: 5915
	private Vector3 currentPathTarget;

	// Token: 0x0400171C RID: 5916
	private MovementData currentMovementData;

	// Token: 0x0400171D RID: 5917
	private int movementDataCount;

	// Token: 0x0400171E RID: 5918
	private int partialPathCount;

	// Token: 0x0400171F RID: 5919
	private Action<float> moveMethod;

	// Token: 0x04001720 RID: 5920
	private Vector2 prevDirection;

	// Token: 0x04001721 RID: 5921
	private Action<float> onStartAction;

	// Token: 0x04001722 RID: 5922
	private Action onFinishAction;

	// Token: 0x04001723 RID: 5923
	private Action<Vector2> onDirectionChange;

	// Token: 0x04001724 RID: 5924
	private Action<float> onPathLengthReady;

	// Token: 0x04001725 RID: 5925
	private uint pathRequestId;

	// Token: 0x02000364 RID: 868
	public enum Status
	{
		// Token: 0x04001727 RID: 5927
		None,
		// Token: 0x04001728 RID: 5928
		PathBuilding,
		// Token: 0x04001729 RID: 5929
		Moving
	}

	// Token: 0x02000365 RID: 869
	public enum CompletionState
	{
		// Token: 0x0400172B RID: 5931
		None,
		// Token: 0x0400172C RID: 5932
		Success,
		// Token: 0x0400172D RID: 5933
		Fail,
		// Token: 0x0400172E RID: 5934
		Canceled
	}

	// Token: 0x02000366 RID: 870
	public enum StartPathResult
	{
		// Token: 0x04001730 RID: 5936
		Started,
		// Token: 0x04001731 RID: 5937
		AlreadyAtDestinationPoint,
		// Token: 0x04001732 RID: 5938
		IncorrectMovementType
	}

	// Token: 0x02000367 RID: 871
	public enum DestinationType
	{
		// Token: 0x04001734 RID: 5940
		Position,
		// Token: 0x04001735 RID: 5941
		DockPoint,
		// Token: 0x04001736 RID: 5942
		PointOfInterest
	}
}
