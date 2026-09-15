using System;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class AudioAnimationParamsReceiver : MonoBehaviour
{
	// Token: 0x060006D4 RID: 1748 RVA: 0x000208D5 File Offset: 0x0001EAD5
	public void EnableAdditionalStepSound()
	{
		if (!this.animationComponent)
		{
			return;
		}
		this.animationComponent.UseAdditionalStepSound = true;
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x000208F1 File Offset: 0x0001EAF1
	public void DisableAdditionalStepSound()
	{
		if (!this.animationComponent)
		{
			return;
		}
		this.animationComponent.UseAdditionalStepSound = false;
	}

	// Token: 0x060006D6 RID: 1750 RVA: 0x0002090D File Offset: 0x0001EB0D
	public void OnAnimationStepCompleted()
	{
		this.EnsureAnimationComponent();
		AnimationComponentBase animationComponentBase = this.animationComponent;
		if (animationComponentBase == null)
		{
			return;
		}
		animationComponentBase.OnAnimationStepCompleted();
	}

	// Token: 0x060006D7 RID: 1751 RVA: 0x00020925 File Offset: 0x0001EB25
	private void Awake()
	{
		this.EnsureAnimationComponent();
	}

	// Token: 0x060006D8 RID: 1752 RVA: 0x0002092D File Offset: 0x0001EB2D
	private void EnsureAnimationComponent()
	{
		if (!this.animationComponent)
		{
			this.animationComponent = base.GetComponentInParent<AnimationComponentBase>();
		}
	}

	// Token: 0x040008C7 RID: 2247
	private AnimationComponentBase animationComponent;
}
