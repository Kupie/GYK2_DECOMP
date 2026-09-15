using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x020007BD RID: 1981
[Serializable]
public class AnimationControlBehaviour : PlayableBehaviour
{
	// Token: 0x060032F6 RID: 13046 RVA: 0x000F5AEC File Offset: 0x000F3CEC
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		TimelineAnimator timelineAnimator = playerData as TimelineAnimator;
		if (timelineAnimator == null)
		{
			return;
		}
		if (!this.isInitialStateSaved)
		{
			this.savedTrackBinding = timelineAnimator;
			this.isInitialStateSaved = true;
			this.initialAnimationState = this.savedTrackBinding.GetState();
			this.initialDirectionAngle = this.savedTrackBinding.GetDirectionAngle();
			if (!string.IsNullOrEmpty(this.floatParameterName) && this.savedTrackBinding.Animator != null)
			{
				this.hasInitialFloatValue = true;
				this.initialFloatValue = this.savedTrackBinding.Animator.GetFloat(this.floatParameterName);
			}
		}
		timelineAnimator.SetAnimationState(this.animationState);
		if (this.overrideDirection)
		{
			timelineAnimator.SetDirection(this.direction);
		}
		if (!string.IsNullOrEmpty(this.floatParameterName))
		{
			timelineAnimator.SetAnimatorFloat(this.floatParameterName, this.floatParameterValue);
		}
		timelineAnimator.UpdateAnimator(info.deltaTime);
	}

	// Token: 0x060032F7 RID: 13047 RVA: 0x000F5BD4 File Offset: 0x000F3DD4
	public override void OnPlayableDestroy(Playable playable)
	{
		if (Application.isPlaying || this.savedTrackBinding == null || !this.isInitialStateSaved)
		{
			return;
		}
		this.savedTrackBinding.SetAnimationState(this.initialAnimationState);
		this.savedTrackBinding.SetDirectionAngle(this.initialDirectionAngle);
		if (this.hasInitialFloatValue)
		{
			this.savedTrackBinding.SetAnimatorFloat(this.floatParameterName, this.initialFloatValue);
		}
		this.savedTrackBinding.UpdateAnimator(0f);
		this.isInitialStateSaved = false;
		this.savedTrackBinding = null;
	}

	// Token: 0x040028C5 RID: 10437
	public global::AnimationState animationState;

	// Token: 0x040028C6 RID: 10438
	public bool overrideDirection;

	// Token: 0x040028C7 RID: 10439
	public Direction direction;

	// Token: 0x040028C8 RID: 10440
	public string floatParameterName;

	// Token: 0x040028C9 RID: 10441
	public float floatParameterValue;

	// Token: 0x040028CA RID: 10442
	private TimelineAnimator savedTrackBinding;

	// Token: 0x040028CB RID: 10443
	private bool isInitialStateSaved;

	// Token: 0x040028CC RID: 10444
	private global::AnimationState initialAnimationState;

	// Token: 0x040028CD RID: 10445
	private float initialDirectionAngle;

	// Token: 0x040028CE RID: 10446
	private float initialFloatValue;

	// Token: 0x040028CF RID: 10447
	private bool hasInitialFloatValue;
}
