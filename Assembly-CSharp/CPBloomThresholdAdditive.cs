using System;
using UnityEngine;

// Token: 0x02000B1F RID: 2847
[Serializable]
public class CPBloomThresholdAdditive : ControllableParameter
{
	// Token: 0x06004BFA RID: 19450 RVA: 0x00166E43 File Offset: 0x00165043
	public override void Init()
	{
		CameraSystem.Instance.MainCamera.AddAdditionalBloomThresholdGetter(() => this.effectiveValue);
	}

	// Token: 0x06004BFB RID: 19451 RVA: 0x00166E60 File Offset: 0x00165060
	public override void OnDisable()
	{
		this.effectiveValue = 0f;
	}

	// Token: 0x06004BFC RID: 19452 RVA: 0x00166E6D File Offset: 0x0016506D
	public override void UpdateParameter(float v, WeatherComponent weatherComponent)
	{
		this.effectiveValue = Mathf.Lerp(0f, this.value, v);
		CameraSystem.Instance.MainCamera.UpdateBloomThreshold();
	}

	// Token: 0x04003D31 RID: 15665
	[Range(-1f, 1f)]
	public float value;

	// Token: 0x04003D32 RID: 15666
	private float effectiveValue;
}
