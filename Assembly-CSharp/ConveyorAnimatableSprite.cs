using System;
using UnityEngine;

// Token: 0x020005FB RID: 1531
public class ConveyorAnimatableSprite : ConveyorAnimatable
{
	// Token: 0x170006A3 RID: 1699
	// (get) Token: 0x06002943 RID: 10563 RVA: 0x000C28B1 File Offset: 0x000C0AB1
	public override bool IsValid
	{
		get
		{
			return this.sprite != null && !string.IsNullOrEmpty(this.spriteNameWithoutIdx);
		}
	}

	// Token: 0x170006A4 RID: 1700
	// (get) Token: 0x06002944 RID: 10564 RVA: 0x00028294 File Offset: 0x00026494
	public override ConveyorAnimatableType Type
	{
		get
		{
			return ConveyorAnimatableType.Sprite;
		}
	}

	// Token: 0x06002945 RID: 10565 RVA: 0x000C28D4 File Offset: 0x000C0AD4
	protected override void OnEnable()
	{
		if (!this.IsValid)
		{
			return;
		}
		if (this.targetType == ConveyorSystemAnimatorType.Cell)
		{
			ConveyorAnimator componentInParent = base.GetComponentInParent<ConveyorAnimator>();
			if (componentInParent != null)
			{
				componentInParent.AddAnimatable(this);
			}
			return;
		}
		Debug.LogError(string.Format("ConveyorAnimatableSprite: {0} is not supported", this.targetType));
	}

	// Token: 0x06002946 RID: 10566 RVA: 0x000C2924 File Offset: 0x000C0B24
	protected override void OnDisable()
	{
		if (!this.IsValid)
		{
			return;
		}
		if (this.targetType == ConveyorSystemAnimatorType.Cell)
		{
			ConveyorAnimator componentInParent = base.GetComponentInParent<ConveyorAnimator>();
			if (componentInParent != null)
			{
				componentInParent.RemoveAnimatable(this);
			}
			return;
		}
		Debug.LogError(string.Format("ConveyorAnimatableSprite: {0} is not supported", this.targetType));
	}

	// Token: 0x04002242 RID: 8770
	public ConveyorSystemAnimatorType targetType;

	// Token: 0x04002243 RID: 8771
	public string spriteNameWithoutIdx;

	// Token: 0x04002244 RID: 8772
	public GenericSprite sprite;
}
