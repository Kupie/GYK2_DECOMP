using System;
using UnityEngine;

// Token: 0x020001B6 RID: 438
[Serializable]
public class NPCPointOfInterestAnimationConfiguration
{
	// Token: 0x170001CA RID: 458
	// (get) Token: 0x06000B21 RID: 2849 RVA: 0x0003820E File Offset: 0x0003640E
	public string TriggerId
	{
		get
		{
			return this.triggerId;
		}
	}

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x06000B22 RID: 2850 RVA: 0x00038216 File Offset: 0x00036416
	public float MinDelay
	{
		get
		{
			return this.minDelay;
		}
	}

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x06000B23 RID: 2851 RVA: 0x0003821E File Offset: 0x0003641E
	public float MaxDelay
	{
		get
		{
			return this.maxDelay;
		}
	}

	// Token: 0x04000C66 RID: 3174
	[SerializeField]
	private string triggerId;

	// Token: 0x04000C67 RID: 3175
	[SerializeField]
	private float minDelay;

	// Token: 0x04000C68 RID: 3176
	[SerializeField]
	private float maxDelay;
}
