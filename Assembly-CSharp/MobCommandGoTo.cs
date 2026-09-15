using System;
using UnityEngine;

// Token: 0x020002EB RID: 747
public class MobCommandGoTo : MobCommand
{
	// Token: 0x17000361 RID: 865
	// (get) Token: 0x060013A7 RID: 5031 RVA: 0x0005F889 File Offset: 0x0005DA89
	public GoToDestinationModifier DestinationModifier
	{
		get
		{
			return this.destinationModifier;
		}
	}

	// Token: 0x17000362 RID: 866
	// (get) Token: 0x060013A8 RID: 5032 RVA: 0x0005F891 File Offset: 0x0005DA91
	private RichAI_Custom RichAI
	{
		get
		{
			return this.agent.RichAI;
		}
	}

	// Token: 0x17000363 RID: 867
	// (get) Token: 0x060013A9 RID: 5033 RVA: 0x0005F89E File Offset: 0x0005DA9E
	private string DestinationModifierType
	{
		get
		{
			GoToDestinationModifier goToDestinationModifier = this.destinationModifier;
			if (goToDestinationModifier == null)
			{
				return null;
			}
			return goToDestinationModifier.GetType().Name;
		}
	}

	// Token: 0x17000364 RID: 868
	// (get) Token: 0x060013AA RID: 5034 RVA: 0x0005F8B8 File Offset: 0x0005DAB8
	private GoToDestinationModifierDebugInfo DestinationModifierDebug
	{
		get
		{
			GoToDestinationModifier goToDestinationModifier = this.destinationModifier;
			if (goToDestinationModifier == null)
			{
				return default(GoToDestinationModifierDebugInfo);
			}
			return goToDestinationModifier.GetDebugInfo();
		}
	}

	// Token: 0x17000365 RID: 869
	// (get) Token: 0x060013AB RID: 5035 RVA: 0x0005F8DE File Offset: 0x0005DADE
	private Vector3 PathfindingTargetPosition
	{
		get
		{
			return this.targetPos;
		}
	}

	// Token: 0x17000366 RID: 870
	// (get) Token: 0x060013AC RID: 5036 RVA: 0x0005F8E6 File Offset: 0x0005DAE6
	private float SmoothedMoveSpeed
	{
		get
		{
			return this.smoothedMoveSpeed;
		}
	}

	// Token: 0x060013AD RID: 5037 RVA: 0x0005F8F0 File Offset: 0x0005DAF0
	public MobCommandGoTo(GoToDestinationModifier goToDestinationModifier)
		: base(MobCommand.CommandType.GoTo)
	{
		this.destinationModifier = goToDestinationModifier;
	}

	// Token: 0x060013AE RID: 5038 RVA: 0x0005F98C File Offset: 0x0005DB8C
	public override void Init(FightingAgent agent)
	{
		base.Init(agent);
		this.prevPosition = base.Wgo.Data.Position;
		this.lastRepathPosition = base.Wgo.Data.Position;
		this.lastRepathTime = Time.time;
		this.previousRotation = this.RichAI.rotation;
		this.destinationModifier.Init(agent);
		this.cachedMaxSpeed = this.RichAI.maxSpeed;
		this.isTransitioningPath = false;
		this.pathTransitionTime = 0f;
		this.pathTransitionVelocity = Vector3.zero;
		this.animationStillTime = 0f;
		this.smoothedMoveSpeed = 0f;
		this.lastPathProgressRemaining = -1f;
		this.lastPathProgressTime = Time.time;
	}

	// Token: 0x060013AF RID: 5039 RVA: 0x0005FA50 File Offset: 0x0005DC50
	public override bool IsTheSameCommand(MobCommand other)
	{
		MobCommandGoTo mobCommandGoTo = other as MobCommandGoTo;
		if (mobCommandGoTo != null)
		{
			Vector3 currentTargetPosition = this.destinationModifier.GetCurrentTargetPosition();
			Vector3 currentTargetPosition2 = mobCommandGoTo.destinationModifier.GetCurrentTargetPosition();
			return (currentTargetPosition - currentTargetPosition2).XZ().magnitude < 0.01f;
		}
		return base.IsTheSameCommand(other);
	}

	// Token: 0x060013B0 RID: 5040 RVA: 0x0005FAA0 File Offset: 0x0005DCA0
	public override void OnStart()
	{
		this.RichAI.SetPath(null, true);
		this.agent.RVO_Locked = false;
		this.agent.RVO_Enabled = true;
		this.agent.SetNavmeshCutActive(false);
		this.agent.IsAnchoredAtDockPoint = false;
		this.agent.RichAI.simulateMovement = false;
		this.agent.TeleportToNavmesh(base.Wgo.Data.Position, false);
		this.prevPosition = this.RichAI.position;
		this.lastRepathPosition = this.RichAI.position;
		this.lastPathProgressRemaining = -1f;
		this.lastPathProgressTime = Time.time;
		this.animationStillTime = 0f;
		this.smoothedMoveSpeed = 0f;
		if (this.cachedMaxSpeed > 0f)
		{
			this.RichAI.maxSpeed = this.cachedMaxSpeed;
			try
			{
				base.Wgo.MainWgoPart.AnimationComponent.SetWalkAnimationSpeedMultiplier(1f);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				throw;
			}
		}
		this.destinationModifier.OnStart();
		this.StartNewPathCalculation();
		base.Wgo.MainWgoPart.AnimationComponent.SetState(global::AnimationState.Walk);
		this.cachedRvoControllerPriority = this.agent.RVO_Priority;
	}

	// Token: 0x060013B1 RID: 5041 RVA: 0x0005FBF0 File Offset: 0x0005DDF0
	public override void OnUpdate(float deltaTime)
	{
		if (this.RichAI.IsMovementPaused || this.agent.RVO_Locked)
		{
			return;
		}
		this.destinationModifier.OnUpdate(deltaTime);
		if (!this.destinationModifier.IsValid)
		{
			this.OnCameToDestination();
			return;
		}
		float num = this.cachedRvoControllerPriority;
		Vector3 currentTargetPosition = this.destinationModifier.GetCurrentTargetPosition();
		if (this.customStopCondition != null)
		{
			if (this.customConditionUpdateTime > 0f)
			{
				this.customConditionUpdateAccumulatedTime += deltaTime;
				if (this.customConditionUpdateAccumulatedTime > this.customConditionUpdateTime)
				{
					if (this.customStopCondition())
					{
						this.OnCameToDestination();
						return;
					}
					this.customConditionUpdateAccumulatedTime = 0f;
				}
			}
			else if (this.customStopCondition())
			{
				this.OnCameToDestination();
				return;
			}
		}
		Vector3 position = this.RichAI.position;
		float magnitude = (currentTargetPosition - position).XZ().magnitude;
		if (this.destinationModifier.IsStucked)
		{
			this.OnCameToDestination();
			return;
		}
		float magnitude2 = (currentTargetPosition - this.targetPos).XZ().magnitude;
		float num2 = (this.prevPosition - position).XZ().magnitude / deltaTime;
		this.adaptiveSensitivityMultiplier = Mathf.Clamp(1f + num2 * 0.1f, 0.5f, 2f);
		float num3 = this.repathSensitivity * this.adaptiveSensitivityMultiplier;
		bool flag = magnitude2.EqualsOrMore(num3, 1E-05f);
		bool flag2 = false;
		if (flag && Time.time - this.lastRepathTime > this.repathCooldown && (position - this.lastRepathPosition).XZ().magnitude > num3 * 0.5f)
		{
			this.lastRepathTime = Time.time;
			this.lastRepathPosition = position;
			this.DoRepath();
			flag2 = true;
		}
		if ((magnitude - this.destinationCompletionOffset).EqualsOrLess(0f, 0.0001f) || magnitude < this.destinationCompletionOffset)
		{
			this.OnCameToDestination();
			return;
		}
		float currentPathDistance = this.GetCurrentPathDistance(magnitude);
		float num4 = currentPathDistance;
		if (currentPathDistance > 100f)
		{
			num4 %= this.maxRvoPriorityOnDestReached;
		}
		num += Mathf.Lerp(this.maxRvoPriorityOnDestReached, 0f, (100f - currentPathDistance) / 100f);
		if (currentPathDistance < this.pathCompletionOffset)
		{
			Vector3 vector = currentTargetPosition - position;
			vector.y = 0f;
			float magnitude3 = vector.magnitude;
			float num5 = this.destinationCompletionOffset * 0.5f;
			bool flag3 = magnitude3 < this.pathCompletionOffset;
			if (flag3 && magnitude3 > num5)
			{
				Vector3 vector2 = position + vector.normalized * (magnitude3 - num5);
				this.RichAI.FinalizeMovement(vector2, this.RichAI.rotation);
				this.UpdateMovementAndAnimation(deltaTime, vector2, new Quaternion?(this.RichAI.rotation));
			}
			if (flag3)
			{
				this.OnCameToDestination();
				return;
			}
			if (!flag2 && magnitude2 > this.destinationCompletionOffset && Time.time - this.lastRepathTime > this.repathCooldown)
			{
				this.lastRepathTime = Time.time;
				this.lastRepathPosition = position;
				this.DoRepath();
				flag2 = true;
			}
		}
		num += 0.1f;
		this.agent.RVO_Priority = num;
		Vector3 position2 = this.RichAI.position;
		Vector3 vector3;
		Quaternion quaternion;
		this.RichAI.MovementUpdate(deltaTime, out vector3, out quaternion);
		vector3 = this.ApplyRvoFunnelFallbackIfNeeded(position2, vector3, deltaTime);
		if (!flag2)
		{
			flag2 = this.TryRepathIfPathProgressStalled();
		}
		if (!flag2 && this.RichAI.RequiredRepath)
		{
			this.lastRepathTime = Time.time;
			this.lastRepathPosition = position;
			this.DoRepath();
		}
		if (this.isTransitioningPath)
		{
			this.pathTransitionTime += deltaTime;
			float num6 = Mathf.Clamp01(this.pathTransitionTime / this.pathTransitionDuration);
			vector3 = Vector3.Lerp(position + this.pathTransitionVelocity * deltaTime, vector3, num6);
			quaternion = Quaternion.Slerp(this.previousRotation, quaternion, num6);
			if (num6 >= 1f)
			{
				this.isTransitioningPath = false;
				this.pathTransitionTime = 0f;
				this.pathTransitionVelocity = Vector3.zero;
				this.agent.RVO_Priority = this.cachedRvoControllerPriority;
			}
		}
		this.RichAI.FinalizeMovement(vector3, quaternion);
		Vector3 position3 = this.RichAI.position;
		this.UpdateMovementAndAnimation(deltaTime, position3, new Quaternion?(quaternion));
		this.previousRotation = quaternion;
	}

	// Token: 0x060013B2 RID: 5042 RVA: 0x0006003C File Offset: 0x0005E23C
	private Vector3 ApplyRvoFunnelFallbackIfNeeded(Vector3 positionBeforeUpdate, Vector3 nextPosition, float deltaTime)
	{
		if (!this.agent.RVO_Enabled)
		{
			return nextPosition;
		}
		float magnitude = this.RichAI.desiredVelocityWithoutLocalAvoidance.magnitude;
		if (magnitude < 0.05f)
		{
			return nextPosition;
		}
		float magnitude2 = (nextPosition - positionBeforeUpdate).XZ().magnitude;
		float num = magnitude * deltaTime;
		if (magnitude2 >= num * 0.05f)
		{
			return nextPosition;
		}
		Vector3 vector = this.RichAI.desiredVelocityWithoutLocalAvoidance * deltaTime;
		return positionBeforeUpdate + vector;
	}

	// Token: 0x060013B3 RID: 5043 RVA: 0x000600B4 File Offset: 0x0005E2B4
	private bool TryRepathIfPathProgressStalled()
	{
		if (!this.RichAI.hasPath || this.RichAI.pathPending)
		{
			return false;
		}
		if (this.agent.RVO_NeighbourCount == 0)
		{
			return false;
		}
		float remainingDistance = this.RichAI.remainingDistance;
		if (float.IsNaN(remainingDistance) || float.IsInfinity(remainingDistance) || remainingDistance < 0f)
		{
			return false;
		}
		if (this.RichAI.desiredVelocityWithoutLocalAvoidance.magnitude < 0.05f)
		{
			return false;
		}
		if (this.lastPathProgressRemaining < 0f || remainingDistance < this.lastPathProgressRemaining - 0.05f)
		{
			this.lastPathProgressRemaining = remainingDistance;
			this.lastPathProgressTime = Time.time;
			return false;
		}
		if (Time.time - this.lastPathProgressTime < this.RichAI.stuckThreshold)
		{
			return false;
		}
		if (Time.time - this.lastRepathTime < this.RichAI.repathCooldownDuration)
		{
			return false;
		}
		this.lastRepathTime = Time.time;
		this.lastRepathPosition = this.RichAI.position;
		this.lastPathProgressTime = Time.time;
		this.DoRepath();
		return true;
	}

	// Token: 0x060013B4 RID: 5044 RVA: 0x000601C4 File Offset: 0x0005E3C4
	private void UpdateMovementAndAnimation(float deltaTime, Vector3 currentPosition, Quaternion? facingRotation)
	{
		WgoPart mainWgoPart = base.Wgo.MainWgoPart;
		AnimationComponentBase animationComponentBase = ((mainWgoPart != null) ? mainWgoPart.AnimationComponent : null);
		if (facingRotation != null)
		{
			float num = Mathf.Max(deltaTime, 0.0001f);
			float num2 = (currentPosition - this.prevPosition).XZ().magnitude / num;
			float num3 = 1f - Mathf.Exp(-num / 0.08f);
			this.smoothedMoveSpeed = Mathf.Lerp(this.smoothedMoveSpeed, num2, num3);
			this.prevPosition = currentPosition;
			base.Wgo.Data.Position = currentPosition;
		}
		if (animationComponentBase != null && this.customAnimationState != global::AnimationState.Walk)
		{
			animationComponentBase.SetState(this.customAnimationState);
			if ((animationComponentBase.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f).EqualsTo(1f, 0.05f))
			{
				this.customAnimationState = global::AnimationState.Walk;
				animationComponentBase.SetState(global::AnimationState.Walk);
			}
			return;
		}
		if (facingRotation == null || animationComponentBase == null)
		{
			return;
		}
		bool flag = this.smoothedMoveSpeed >= 0.12f;
		bool flag2 = this.RichAI.hasPath && this.GetCurrentPathDistance(0f) > this.destinationCompletionOffset;
		bool flag3 = this.isTransitioningPath || this.RichAI.pathPending || flag2 || this.RichAI.desiredVelocityWithoutLocalAvoidance.magnitude >= 0.05f;
		if (flag || flag3)
		{
			this.animationStillTime = 0f;
		}
		else
		{
			this.animationStillTime += deltaTime;
		}
		if (flag3 || flag || this.animationStillTime < 0.12f)
		{
			animationComponentBase.SetState(global::AnimationState.Walk);
			Quaternion value = facingRotation.Value;
			base.SetFacingDirection(new Vector2(Mathf.Sin(value.eulerAngles.y * 0.017453292f), Mathf.Cos(value.eulerAngles.y * 0.017453292f)), false);
			return;
		}
		animationComponentBase.SetState(global::AnimationState.Idle);
	}

	// Token: 0x060013B5 RID: 5045 RVA: 0x000603D0 File Offset: 0x0005E5D0
	private float GetCurrentPathDistance(float fallbackDistance)
	{
		if (!this.RichAI.hasPath)
		{
			return fallbackDistance;
		}
		float remainingDistance = this.RichAI.remainingDistance;
		if (float.IsNaN(remainingDistance) || float.IsInfinity(remainingDistance) || remainingDistance < 0f)
		{
			return fallbackDistance;
		}
		return remainingDistance;
	}

	// Token: 0x060013B6 RID: 5046 RVA: 0x00060414 File Offset: 0x0005E614
	public override void OnFinish()
	{
		this.RichAI.SetPath(null, true);
		this.agent.TeleportToNavmesh(base.Wgo.Data.Position, false);
		this.agent.RvoStopAt(base.Wgo.Data.Position);
		this.agent.RVO_Enabled = !this.agent.IsAnchoredAtDockPoint;
		if (this.cachedMaxSpeed > 0f)
		{
			this.RichAI.maxSpeed = this.cachedMaxSpeed;
			WgoPart mainWgoPart = base.Wgo.MainWgoPart;
			if (mainWgoPart != null)
			{
				AnimationComponentBase animationComponent = mainWgoPart.AnimationComponent;
				if (animationComponent != null)
				{
					animationComponent.SetWalkAnimationSpeedMultiplier(1f);
				}
			}
		}
		WgoPart mainWgoPart2 = base.Wgo.MainWgoPart;
		if (mainWgoPart2 != null)
		{
			AnimationComponentBase animationComponent2 = mainWgoPart2.AnimationComponent;
			if (animationComponent2 != null)
			{
				animationComponent2.SetState(global::AnimationState.Idle);
			}
		}
		this.agent.RVO_Priority = this.cachedRvoControllerPriority;
		this.agent.RVO_Locked = this.agent.IsAnchoredAtDockPoint;
		GoToDestinationModifier goToDestinationModifier = this.destinationModifier;
		if (goToDestinationModifier != null)
		{
			goToDestinationModifier.OnFinish();
		}
		this.agent.RichAI.simulateMovement = true;
	}

	// Token: 0x060013B7 RID: 5047 RVA: 0x0006052D File Offset: 0x0005E72D
	public override void CompensatePause(float pausedFor)
	{
		if (pausedFor <= 0f)
		{
			return;
		}
		this.lastRepathTime += pausedFor;
		this.lastPathProgressTime += pausedFor;
	}

	// Token: 0x060013B8 RID: 5048 RVA: 0x00060554 File Offset: 0x0005E754
	public MobCommandGoTo WithCustomTargetDestinationOffset(float offset)
	{
		this.destinationCompletionOffset = offset;
		return this;
	}

	// Token: 0x060013B9 RID: 5049 RVA: 0x00060560 File Offset: 0x0005E760
	public MobCommandGoTo WithCustomStopCondition(Func<bool> condition, float customUpdateTime = -1f, float initialCounter = 0f)
	{
		Func<bool> previousStopCondition = this.customStopCondition;
		this.customStopCondition = ((previousStopCondition == null) ? condition : (() => previousStopCondition() || condition()));
		if (previousStopCondition == null)
		{
			this.customConditionUpdateTime = customUpdateTime;
			this.customConditionUpdateAccumulatedTime = Mathf.Clamp(initialCounter, 0f, customUpdateTime);
		}
		else if (customUpdateTime > 0f)
		{
			this.customConditionUpdateTime = ((this.customConditionUpdateTime > 0f) ? Mathf.Min(this.customConditionUpdateTime, customUpdateTime) : customUpdateTime);
		}
		return this;
	}

	// Token: 0x060013BA RID: 5050 RVA: 0x000605F6 File Offset: 0x0005E7F6
	public MobCommandGoTo WithCustomActionOnDestReached(Action action)
	{
		this.action = action;
		return this;
	}

	// Token: 0x060013BB RID: 5051 RVA: 0x00060600 File Offset: 0x0005E800
	public void SetCustomAnimationState(global::AnimationState animationState)
	{
		if (this.customAnimationState != animationState)
		{
			this.customAnimationState = animationState;
		}
	}

	// Token: 0x060013BC RID: 5052 RVA: 0x00060614 File Offset: 0x0005E814
	private void OnCameToDestination()
	{
		float magnitude = (this.destinationModifier.GetCurrentTargetPosition() - base.Wgo.Data.Position).XZ().magnitude;
		if ((magnitude - this.destinationCompletionOffset).EqualsTo(0f, 0.0001f) || magnitude < this.destinationCompletionOffset)
		{
			this.destinationModifier.OnReachedDestination();
			if (this.destinationModifier.ShouldAnchorOnArrival)
			{
				this.agent.IsAnchoredAtDockPoint = true;
			}
		}
		Action action = this.action;
		if (action != null)
		{
			action();
		}
		this.agent.StopCommandExecution(true);
	}

	// Token: 0x060013BD RID: 5053 RVA: 0x000606B4 File Offset: 0x0005E8B4
	private void DoRepath()
	{
		if (this.RichAI.hasPath && this.RichAI.velocity.sqrMagnitude > 0.01f)
		{
			if (!this.isTransitioningPath)
			{
				this.pathTransitionVelocity = this.RichAI.velocity;
				this.isTransitioningPath = true;
				this.pathTransitionTime = 0f;
			}
			else
			{
				this.pathTransitionVelocity = this.RichAI.velocity;
			}
		}
		else if (!this.isTransitioningPath)
		{
			this.agent.RVO_Priority = this.cachedRvoControllerPriority;
		}
		this.StartNewPathCalculation();
	}

	// Token: 0x060013BE RID: 5054 RVA: 0x00060748 File Offset: 0x0005E948
	private void StartNewPathCalculation()
	{
		ICombatEntity combatEntity;
		if (this.destinationModifier.TryGetTargetPositionForPathfinding(out combatEntity, out this.targetPos))
		{
			if (combatEntity != null)
			{
				this.agent.ParentController.AddPathCalculation(new PathCalculationData(base.Wgo.Data.UniqueId, this.RichAI, combatEntity));
				return;
			}
			this.agent.ParentController.AddPathCalculation(new PathCalculationData(base.Wgo.Data.UniqueId, this.RichAI, this.targetPos));
		}
	}

	// Token: 0x040014D2 RID: 5330
	public const float DEST_ALMOST_PRECISE_OFFSET = 0.06666668f;

	// Token: 0x040014D3 RID: 5331
	private const float MOVEMENT_AGENT_RVO_PRIORITY_ADD = 0.1f;

	// Token: 0x040014D4 RID: 5332
	private const float MAX_DISTANCE_TO_TARGET_THRESHOLD = 100f;

	// Token: 0x040014D5 RID: 5333
	private GoToDestinationModifier destinationModifier;

	// Token: 0x040014D6 RID: 5334
	private Action action;

	// Token: 0x040014D7 RID: 5335
	private float destinationCompletionOffset = 0.06666668f;

	// Token: 0x040014D8 RID: 5336
	private float pathCompletionOffset = 0.3f;

	// Token: 0x040014D9 RID: 5337
	private float repathSensitivity = 0.5f;

	// Token: 0x040014DA RID: 5338
	private float lastRepathTime;

	// Token: 0x040014DB RID: 5339
	private float repathCooldown = 0.3f;

	// Token: 0x040014DC RID: 5340
	private float adaptiveSensitivityMultiplier = 1f;

	// Token: 0x040014DD RID: 5341
	private Vector3 lastRepathPosition;

	// Token: 0x040014DE RID: 5342
	private Vector3 pathTransitionVelocity;

	// Token: 0x040014DF RID: 5343
	private float pathTransitionTime;

	// Token: 0x040014E0 RID: 5344
	private float pathTransitionDuration = 0.25f;

	// Token: 0x040014E1 RID: 5345
	private bool isTransitioningPath;

	// Token: 0x040014E2 RID: 5346
	private Quaternion previousRotation;

	// Token: 0x040014E3 RID: 5347
	private Vector3 targetPos;

	// Token: 0x040014E4 RID: 5348
	private Vector3 prevPosition;

	// Token: 0x040014E5 RID: 5349
	private Func<bool> customStopCondition;

	// Token: 0x040014E6 RID: 5350
	private float customConditionUpdateTime = -1f;

	// Token: 0x040014E7 RID: 5351
	private float customConditionUpdateAccumulatedTime;

	// Token: 0x040014E8 RID: 5352
	private float cachedRvoControllerPriority;

	// Token: 0x040014E9 RID: 5353
	private global::AnimationState customAnimationState = global::AnimationState.Walk;

	// Token: 0x040014EA RID: 5354
	private float distanceToTargetWhenStartToApplyPreFinishRvoPriority = 5f;

	// Token: 0x040014EB RID: 5355
	private float maxRvoPriorityOnDestReached = 0.2f;

	// Token: 0x040014EC RID: 5356
	private float cachedMaxSpeed = -1f;

	// Token: 0x040014ED RID: 5357
	private const float MOVEMENT_WALK_SPEED_THRESHOLD = 0.12f;

	// Token: 0x040014EE RID: 5358
	private const float MOVEMENT_INTENT_SPEED_THRESHOLD = 0.05f;

	// Token: 0x040014EF RID: 5359
	private const float MOVEMENT_SPEED_SMOOTH_TIME = 0.08f;

	// Token: 0x040014F0 RID: 5360
	private const float IDLE_AFTER_STILL_TIME = 0.12f;

	// Token: 0x040014F1 RID: 5361
	private float animationStillTime;

	// Token: 0x040014F2 RID: 5362
	private float smoothedMoveSpeed;

	// Token: 0x040014F3 RID: 5363
	private float lastPathProgressRemaining = -1f;

	// Token: 0x040014F4 RID: 5364
	private float lastPathProgressTime;
}
