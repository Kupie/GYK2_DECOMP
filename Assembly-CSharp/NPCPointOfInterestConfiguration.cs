using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001B9 RID: 441
[Serializable]
public class NPCPointOfInterestConfiguration
{
	// Token: 0x170001CF RID: 463
	// (get) Token: 0x06000B29 RID: 2857 RVA: 0x0003826A File Offset: 0x0003646A
	public string Id
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x06000B2A RID: 2858 RVA: 0x00038272 File Offset: 0x00036472
	public bool EnabledByDefault
	{
		get
		{
			return this.enabledByDefault;
		}
	}

	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x06000B2B RID: 2859 RVA: 0x0003827A File Offset: 0x0003647A
	public float StartWeight
	{
		get
		{
			return this.startWeight;
		}
	}

	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x06000B2C RID: 2860 RVA: 0x00038282 File Offset: 0x00036482
	public List<NPCPointOfInterestAnimationConfiguration> AnimationsForRoll
	{
		get
		{
			return this.animationsForRoll;
		}
	}

	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x06000B2D RID: 2861 RVA: 0x0003828A File Offset: 0x0003648A
	public NPCPointOfInterestAnimationType AnimationType
	{
		get
		{
			return this.animationType;
		}
	}

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x06000B2E RID: 2862 RVA: 0x00038292 File Offset: 0x00036492
	public string IdleTriggerId
	{
		get
		{
			return this.idleTriggerId;
		}
	}

	// Token: 0x04000C6F RID: 3183
	[SerializeField]
	private string id;

	// Token: 0x04000C70 RID: 3184
	[SerializeField]
	private bool enabledByDefault = true;

	// Token: 0x04000C71 RID: 3185
	[SerializeField]
	private NPCPointOfInterestAnimationType animationType;

	// Token: 0x04000C72 RID: 3186
	[SerializeField]
	private List<NPCPointOfInterestAnimationConfiguration> animationsForRoll = new List<NPCPointOfInterestAnimationConfiguration>();

	// Token: 0x04000C73 RID: 3187
	[SerializeField]
	private string idleTriggerId;

	// Token: 0x04000C74 RID: 3188
	[SerializeField]
	private float startWeight = 10f;
}
