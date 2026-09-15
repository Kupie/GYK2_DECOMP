using System;
using UnityEngine;

// Token: 0x020001B7 RID: 439
[Serializable]
public class NPCPointOfInterestAnimationData
{
	// Token: 0x170001CD RID: 461
	// (get) Token: 0x06000B25 RID: 2853 RVA: 0x00038226 File Offset: 0x00036426
	// (set) Token: 0x06000B26 RID: 2854 RVA: 0x0003822E File Offset: 0x0003642E
	public float RemainingTimeToRoll
	{
		get
		{
			return this.remainingTimeToRoll;
		}
		set
		{
			this.remainingTimeToRoll = value;
		}
	}

	// Token: 0x170001CE RID: 462
	// (get) Token: 0x06000B27 RID: 2855 RVA: 0x00038237 File Offset: 0x00036437
	public SGuid WgoId
	{
		get
		{
			return this.wgoId;
		}
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x0003823F File Offset: 0x0003643F
	public NPCPointOfInterestAnimationData(WgoData wgoData, NPCPointOfInterestAnimationConfiguration configuration)
	{
		this.wgoId = wgoData.UniqueId;
		this.remainingTimeToRoll = global::UnityEngine.Random.Range(configuration.MinDelay, configuration.MaxDelay);
	}

	// Token: 0x04000C69 RID: 3177
	[SerializeField]
	private SGuid wgoId;

	// Token: 0x04000C6A RID: 3178
	[SerializeField]
	private float remainingTimeToRoll;
}
