using System;
using UnityEngine;

// Token: 0x020005FA RID: 1530
public class ConveyorAnimatableItem : ConveyorAnimatable
{
	// Token: 0x170006A2 RID: 1698
	// (get) Token: 0x0600293C RID: 10556 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public override ConveyorAnimatableType Type
	{
		get
		{
			return ConveyorAnimatableType.Item;
		}
	}

	// Token: 0x0600293D RID: 10557 RVA: 0x000C27AB File Offset: 0x000C09AB
	public override void CreateCache()
	{
		this.cachedPosition = base.transform.localPosition;
		this.cachedRotation = base.transform.localEulerAngles;
		this.cachedScale = base.transform.localScale;
	}

	// Token: 0x0600293E RID: 10558 RVA: 0x000C27E0 File Offset: 0x000C09E0
	public override void RestoreCache()
	{
		base.transform.localPosition = this.cachedPosition;
		base.transform.localEulerAngles = this.cachedRotation;
		base.transform.localScale = this.cachedScale;
	}

	// Token: 0x0600293F RID: 10559 RVA: 0x000C2818 File Offset: 0x000C0A18
	protected override void OnEnable()
	{
		if (this.parentWgoPart == null)
		{
			return;
		}
		ConveyorAnimator componentInParent = base.GetComponentInParent<ConveyorAnimator>();
		if (componentInParent != null)
		{
			componentInParent.AddAnimatable(this);
		}
		this.CreateCache();
	}

	// Token: 0x06002940 RID: 10560 RVA: 0x000C2854 File Offset: 0x000C0A54
	protected override void OnDisable()
	{
		if (this.parentWgoPart == null)
		{
			return;
		}
		ConveyorAnimator componentInParent = base.GetComponentInParent<ConveyorAnimator>();
		if (componentInParent != null)
		{
			componentInParent.RemoveAnimatable(this);
		}
		this.RestoreCache();
	}

	// Token: 0x06002941 RID: 10561 RVA: 0x000C288D File Offset: 0x000C0A8D
	private void Awake()
	{
		if (this.parentWgoPart == null)
		{
			this.parentWgoPart = base.GetComponentInParent<WgoPart>();
		}
	}

	// Token: 0x0400223E RID: 8766
	public WgoPart parentWgoPart;

	// Token: 0x0400223F RID: 8767
	private Vector3 cachedPosition;

	// Token: 0x04002240 RID: 8768
	private Vector3 cachedRotation;

	// Token: 0x04002241 RID: 8769
	private Vector3 cachedScale;
}
