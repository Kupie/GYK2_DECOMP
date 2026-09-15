using System;
using UnityEngine;

// Token: 0x02000B1A RID: 2842
[DisallowMultipleComponent]
public class LightSourceGroup : DayNightLightBase
{
	// Token: 0x06004BD6 RID: 19414 RVA: 0x001669FB File Offset: 0x00164BFB
	private void Reset()
	{
		this.dayNightLight = base.GetComponent<DayNightLight>();
		this.lightFaker = base.GetComponent<LightFaker>();
		this.dayNightSprite = base.GetComponentInChildren<DayNightSprite>(true);
	}

	// Token: 0x06004BD7 RID: 19415 RVA: 0x00166A22 File Offset: 0x00164C22
	private void Awake()
	{
		this.ApplyLightMode(this.mode);
	}

	// Token: 0x06004BD8 RID: 19416 RVA: 0x00166A30 File Offset: 0x00164C30
	protected override void ApplyLightMode(DayNightLightBase.LightMode lightMode)
	{
		this.mode = lightMode;
		DayNightLight dayNightLight = this.dayNightLight;
		if (dayNightLight != null)
		{
			dayNightLight.ApplyLightModeInt((int)lightMode);
		}
		LightFaker lightFaker = this.lightFaker;
		if (lightFaker != null)
		{
			lightFaker.ApplyLightModeInt((int)lightMode);
		}
		DayNightSprite dayNightSprite = this.dayNightSprite;
		if (dayNightSprite == null)
		{
			return;
		}
		dayNightSprite.ApplyLightModeInt((int)lightMode);
	}

	// Token: 0x04003D26 RID: 15654
	[SerializeField]
	private DayNightLight dayNightLight;

	// Token: 0x04003D27 RID: 15655
	[SerializeField]
	private LightFaker lightFaker;

	// Token: 0x04003D28 RID: 15656
	[SerializeField]
	private DayNightSprite dayNightSprite;
}
