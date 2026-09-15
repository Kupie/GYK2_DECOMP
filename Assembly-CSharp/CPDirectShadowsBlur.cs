using System;
using System.Collections.Generic;
using LazyBearTechnology;
using PI.NGSS;
using UnityEngine;

// Token: 0x02000B22 RID: 2850
[Serializable]
public class CPDirectShadowsBlur : ControllableParameter
{
	// Token: 0x17000B66 RID: 2918
	// (get) Token: 0x06004C07 RID: 19463 RVA: 0x001670FC File Offset: 0x001652FC
	private static NGSS_Directional NgssDirectional
	{
		get
		{
			if (!CPDirectShadowsBlur.ngssDirectional)
			{
				CPDirectShadowsBlur.ngssDirectional = global::UnityEngine.Object.FindFirstObjectByType<NGSS_Directional>();
				if (!CPDirectShadowsBlur.ngssDirectional)
				{
					Debug.LogError("No NGSS Directional component found.");
					return null;
				}
				CPDirectShadowsBlur.initialBlurValue = CPDirectShadowsBlur.ngssDirectional.NGSS_PCSS_SOFTNESS_NEAR;
			}
			if (!CPDirectShadowsBlur.ngssInitialized)
			{
				CPDirectShadowsBlur.Initialize();
			}
			return CPDirectShadowsBlur.ngssDirectional;
		}
	}

	// Token: 0x06004C08 RID: 19464 RVA: 0x00167158 File Offset: 0x00165358
	public static void Initialize()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		CPDirectShadowsBlur.ngssInitialized = true;
		if (LightsSystem.Instance.NgssFeatureEnabled)
		{
			NGSS_Directional ngss = CPDirectShadowsBlur.NgssDirectional;
			if (ngss != null)
			{
				ngss.enabled = false;
				LazyTimer.AddTimer(0f, delegate
				{
					if (ngss)
					{
						ngss.enabled = true;
					}
				}, null);
			}
		}
	}

	// Token: 0x06004C09 RID: 19465 RVA: 0x001671C4 File Offset: 0x001653C4
	public override void UpdateParameter(float v, WeatherComponent weatherComponent)
	{
		if (CPDirectShadowsBlur.NgssDirectional == null)
		{
			return;
		}
		float num = v * this.additionalBlur;
		CPDirectShadowsBlur.values[this] = num;
	}

	// Token: 0x06004C0A RID: 19466 RVA: 0x001671F4 File Offset: 0x001653F4
	public static void ApplyParameters()
	{
		if (CPDirectShadowsBlur.NgssDirectional == null)
		{
			return;
		}
		float ngss_PCSS_SOFTNESS_NEAR = CPDirectShadowsBlur.NgssDirectional.NGSS_PCSS_SOFTNESS_NEAR;
		float num = 0f;
		foreach (float num2 in CPDirectShadowsBlur.values.Values)
		{
			num = Mathf.Max(num, num2);
		}
		CPDirectShadowsBlur.NgssDirectional.NGSS_PCSS_SOFTNESS_NEAR = CPDirectShadowsBlur.initialBlurValue + num;
		CPDirectShadowsBlur.NgssDirectional.UpdateParameters();
	}

	// Token: 0x04003D38 RID: 15672
	[Range(0f, 3f)]
	public float additionalBlur = 2f;

	// Token: 0x04003D39 RID: 15673
	private static Dictionary<CPDirectShadowsBlur, float> values = new Dictionary<CPDirectShadowsBlur, float>();

	// Token: 0x04003D3A RID: 15674
	private static float initialBlurValue;

	// Token: 0x04003D3B RID: 15675
	private static bool ngssInitialized = false;

	// Token: 0x04003D3C RID: 15676
	private static NGSS_Directional ngssDirectional;
}
