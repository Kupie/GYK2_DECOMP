using System;
using UnityEngine;

// Token: 0x0200025E RID: 606
[RequireComponent(typeof(Collider))]
public abstract class ColliderTriggerComponentBase : MonoBehaviour
{
	// Token: 0x06000F55 RID: 3925 RVA: 0x0004ECD3 File Offset: 0x0004CED3
	protected void Init(ColliderTriggerDataBase onEnter, ColliderTriggerDataBase onExit)
	{
		this.onEnterBase = onEnter;
		this.onExitBase = onExit;
		this.collider = base.GetComponent<Collider>();
		this.collider.isTrigger = true;
	}

	// Token: 0x06000F56 RID: 3926 RVA: 0x0004ECFB File Offset: 0x0004CEFB
	private void OnTriggerEnter(Collider other)
	{
		if (this.isPlayerInside)
		{
			return;
		}
		this.isPlayerInside = true;
		this.onEnterBase.TrySetTrigger();
		this.onExitBase.TryResetTrigger();
	}

	// Token: 0x06000F57 RID: 3927 RVA: 0x0004ED23 File Offset: 0x0004CF23
	private void OnTriggerExit(Collider other)
	{
		if (!this.isPlayerInside)
		{
			return;
		}
		this.isPlayerInside = false;
		this.onExitBase.TrySetTrigger();
		this.onEnterBase.TryResetTrigger();
	}

	// Token: 0x04001237 RID: 4663
	private ColliderTriggerDataBase onEnterBase;

	// Token: 0x04001238 RID: 4664
	private ColliderTriggerDataBase onExitBase;

	// Token: 0x04001239 RID: 4665
	private bool isPlayerInside;

	// Token: 0x0400123A RID: 4666
	private Collider collider;
}
