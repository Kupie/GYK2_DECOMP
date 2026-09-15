using System;
using UnityEngine;

// Token: 0x020005F8 RID: 1528
public abstract class ConveyorAnimatable : MonoBehaviour
{
	// Token: 0x170006A0 RID: 1696
	// (get) Token: 0x06002935 RID: 10549
	public abstract ConveyorAnimatableType Type { get; }

	// Token: 0x170006A1 RID: 1697
	// (get) Token: 0x06002936 RID: 10550 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public virtual bool IsValid
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06002937 RID: 10551 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void CreateCache()
	{
	}

	// Token: 0x06002938 RID: 10552 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void RestoreCache()
	{
	}

	// Token: 0x06002939 RID: 10553 RVA: 0x00002318 File Offset: 0x00000518
	protected virtual void OnEnable()
	{
	}

	// Token: 0x0600293A RID: 10554 RVA: 0x00002318 File Offset: 0x00000518
	protected virtual void OnDisable()
	{
	}
}
