using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000B1C RID: 2844
public static class SwitchLightPolicy
{
	// Token: 0x140000D1 RID: 209
	// (add) Token: 0x06004BE5 RID: 19429 RVA: 0x00166BB8 File Offset: 0x00164DB8
	// (remove) Token: 0x06004BE6 RID: 19430 RVA: 0x00166BEC File Offset: 0x00164DEC
	public static event Action OnLocalLightShadowPolicyChanged;

	// Token: 0x17000B62 RID: 2914
	// (get) Token: 0x06004BE7 RID: 19431 RVA: 0x00166C20 File Offset: 0x00164E20
	public static bool IsSwitchPlatform
	{
		get
		{
			RuntimePlatform platform = Application.platform;
			return platform == RuntimePlatform.Switch || platform == RuntimePlatform.Switch2;
		}
	}

	// Token: 0x17000B63 RID: 2915
	// (get) Token: 0x06004BE8 RID: 19432 RVA: 0x00166C46 File Offset: 0x00164E46
	public static bool UseLightRT
	{
		get
		{
			return PlatformFeatures.Current.pointLightMode == PlatformPointLightMode.Faked;
		}
	}

	// Token: 0x17000B64 RID: 2916
	// (get) Token: 0x06004BE9 RID: 19433 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public static bool AllowRealPointLights
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06004BEA RID: 19434 RVA: 0x00166C55 File Offset: 0x00164E55
	public static bool ShouldDisableRealLight(float intensity)
	{
		return intensity < 0.001f;
	}

	// Token: 0x06004BEB RID: 19435 RVA: 0x00166C5F File Offset: 0x00164E5F
	public static void ApplyLightRTPolicy()
	{
		LightRTManager.ApplyPolicy();
	}

	// Token: 0x06004BEC RID: 19436 RVA: 0x00166C68 File Offset: 0x00164E68
	public static void ApplyRealLightEnabled(Light light, float intensity)
	{
		if (light == null)
		{
			return;
		}
		bool flag;
		if (SwitchLightPolicy.TryGetPlatformDependentPointLightEnabled(light, intensity, out flag))
		{
			light.enabled = flag;
			return;
		}
		light.enabled = SwitchLightPolicy.GetRealLightEnabled(intensity);
	}

	// Token: 0x06004BED RID: 19437 RVA: 0x00166C9E File Offset: 0x00164E9E
	public static void NotifyLocalLightShadowPolicyChanged()
	{
		Action onLocalLightShadowPolicyChanged = SwitchLightPolicy.OnLocalLightShadowPolicyChanged;
		if (onLocalLightShadowPolicyChanged == null)
		{
			return;
		}
		onLocalLightShadowPolicyChanged();
	}

	// Token: 0x06004BEE RID: 19438 RVA: 0x00166CB0 File Offset: 0x00164EB0
	public static void ApplyLocalLightShadowPolicy(Light light, LightShadows authoredShadows)
	{
		if (light == null || light.type == LightType.Directional)
		{
			return;
		}
		LightShadows lightShadows = PlatformUnityShadowPresets.Get(PlatformFeatures.GetActiveUnityShadowPreset()).localLightShadows ?? authoredShadows;
		if (light.shadows == lightShadows)
		{
			return;
		}
		light.shadows = lightShadows;
	}

	// Token: 0x06004BEF RID: 19439 RVA: 0x00166D05 File Offset: 0x00164F05
	private static bool GetRealLightEnabled(float intensity)
	{
		return !SwitchLightPolicy.UseLightRT && !SwitchLightPolicy.ShouldDisableRealLight(intensity);
	}

	// Token: 0x06004BF0 RID: 19440 RVA: 0x00166D1C File Offset: 0x00164F1C
	private static bool TryGetPlatformDependentPointLightEnabled(Light light, float intensity, out bool enabled)
	{
		enabled = false;
		if (light.type != LightType.Point)
		{
			return false;
		}
		LazyPlatformDependentElement lazyPlatformDependentElement;
		if (!light.TryGetComponent<LazyPlatformDependentElement>(out lazyPlatformDependentElement))
		{
			return false;
		}
		if (!lazyPlatformDependentElement.IsActive)
		{
			return true;
		}
		enabled = SwitchLightPolicy.GetRealLightEnabled(intensity);
		return true;
	}

	// Token: 0x04003D2E RID: 15662
	private const float INTENSITY_EPSILON = 0.001f;
}
