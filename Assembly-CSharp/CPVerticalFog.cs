using System;
using UnityEngine;

// Token: 0x02000B29 RID: 2857
[Serializable]
public class CPVerticalFog : ControllableParameter
{
	// Token: 0x06004C1B RID: 19483 RVA: 0x0016744A File Offset: 0x0016564A
	public override void UpdateParameter(float v, WeatherComponent weatherComponent)
	{
		if (this.verticalFog.isGlobalController)
		{
			Debug.LogError("Global Vertical Fog controller cannot be controlled by a controllable parameter.", this.verticalFog);
			return;
		}
		this.verticalFog.fogEnabled = v > 0f;
		this.verticalFog.ApplyFogParametersWithIntensity(v);
	}

	// Token: 0x04003D44 RID: 15684
	public VerticalFog verticalFog;
}
