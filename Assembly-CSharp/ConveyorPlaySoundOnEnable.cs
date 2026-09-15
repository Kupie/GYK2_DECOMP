using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200011D RID: 285
public class ConveyorPlaySoundOnEnable : PlaySoundOnEnable
{
	// Token: 0x17000117 RID: 279
	// (get) Token: 0x060006E8 RID: 1768 RVA: 0x00020C60 File Offset: 0x0001EE60
	public float MassMultiplier
	{
		get
		{
			return this.massMultiplier;
		}
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x00020C68 File Offset: 0x0001EE68
	protected override void OnEnable()
	{
		LazySingleton<ConveyorSoundSystem>.Instance.Register(this);
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x00020C75 File Offset: 0x0001EE75
	protected override void OnDisable()
	{
		LazySingleton<ConveyorSoundSystem>.Instance.Unregister(this);
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x00020C75 File Offset: 0x0001EE75
	protected override void OnDestroy()
	{
		LazySingleton<ConveyorSoundSystem>.Instance.Unregister(this);
	}

	// Token: 0x040008D5 RID: 2261
	[SerializeField]
	private float massMultiplier = 1f;
}
