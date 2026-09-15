using System;
using UnityEngine;

// Token: 0x020007C5 RID: 1989
[ExecuteAlways]
public class TimelineAnimator : MonoBehaviour
{
	// Token: 0x170007B6 RID: 1974
	// (get) Token: 0x06003323 RID: 13091 RVA: 0x000F65CA File Offset: 0x000F47CA
	public Animator Animator
	{
		get
		{
			if (!(this.animationComponent != null))
			{
				return null;
			}
			return this.animationComponent.Animator;
		}
	}

	// Token: 0x06003324 RID: 13092 RVA: 0x000F65E7 File Offset: 0x000F47E7
	private void OnEnable()
	{
		this.FindComponents();
	}

	// Token: 0x06003325 RID: 13093 RVA: 0x000F65EF File Offset: 0x000F47EF
	private void FindComponents()
	{
		this.animationComponent = base.GetComponent<AnimationComponentBase>();
		if (this.animationComponent == null)
		{
			this.animationComponent = base.GetComponentInChildren<AnimationComponentBase>();
		}
	}

	// Token: 0x06003326 RID: 13094 RVA: 0x000F6617 File Offset: 0x000F4817
	public void SetAnimationState(global::AnimationState state)
	{
		if (this.animationComponent == null)
		{
			this.FindComponents();
		}
		if (this.animationComponent == null)
		{
			return;
		}
		if (this.animationComponent.GetState() != state)
		{
			this.animationComponent.SetState(state);
		}
	}

	// Token: 0x06003327 RID: 13095 RVA: 0x000F6656 File Offset: 0x000F4856
	public global::AnimationState GetState()
	{
		if (this.animationComponent == null)
		{
			this.FindComponents();
		}
		if (!(this.animationComponent != null))
		{
			return global::AnimationState.Idle;
		}
		return this.animationComponent.GetState();
	}

	// Token: 0x06003328 RID: 13096 RVA: 0x000F6687 File Offset: 0x000F4887
	public void SetDirection(Direction direction)
	{
		if (this.animationComponent == null)
		{
			this.FindComponents();
		}
		if (this.animationComponent == null)
		{
			return;
		}
		this.animationComponent.SetDirection(direction);
	}

	// Token: 0x06003329 RID: 13097 RVA: 0x000F66B8 File Offset: 0x000F48B8
	public float GetDirectionAngle()
	{
		if (this.animationComponent == null || this.animationComponent.Animator == null)
		{
			return 0f;
		}
		return this.animationComponent.Animator.GetFloat(AnimationComponentBase.idDirectionAnimator);
	}

	// Token: 0x0600332A RID: 13098 RVA: 0x000F66F6 File Offset: 0x000F48F6
	public void SetDirectionAngle(float angle)
	{
		if (this.animationComponent == null || this.animationComponent.Animator == null)
		{
			return;
		}
		this.animationComponent.Animator.SetFloat(AnimationComponentBase.idDirectionAnimator, angle);
	}

	// Token: 0x0600332B RID: 13099 RVA: 0x000F6730 File Offset: 0x000F4930
	public void SetAnimatorFloat(string floatName, float value)
	{
		if (this.animationComponent == null)
		{
			this.FindComponents();
		}
		if (this.animationComponent == null || this.animationComponent.Animator == null)
		{
			return;
		}
		if (this.animationComponent.Animator.GetFloat(floatName) != value)
		{
			this.animationComponent.Animator.SetFloat(floatName, value);
		}
	}

	// Token: 0x0600332C RID: 13100 RVA: 0x000F679C File Offset: 0x000F499C
	public void UpdateAnimator(float deltaTime)
	{
		if (this.animationComponent == null)
		{
			this.FindComponents();
		}
		if (this.animationComponent != null && this.animationComponent.Animator != null && !Application.isPlaying)
		{
			this.animationComponent.Animator.Update(deltaTime);
		}
	}

	// Token: 0x040028EB RID: 10475
	private AnimationComponentBase animationComponent;
}
