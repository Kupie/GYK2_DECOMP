using System;
using UnityEngine;

// Token: 0x020004F8 RID: 1272
[ExecuteAlways]
public class DayNightLight : DayNightLightBase
{
	// Token: 0x0600211B RID: 8475 RVA: 0x0009C4F4 File Offset: 0x0009A6F4
	private float NightLight()
	{
		return Mathf.Lerp(this.intensity, 0f, Shader.GetGlobalFloat(GlobalShaderParameters.idSunLight));
	}

	// Token: 0x0600211C RID: 8476 RVA: 0x0009C510 File Offset: 0x0009A710
	private float DayLight()
	{
		return Mathf.Lerp(0f, this.intensity, Shader.GetGlobalFloat(GlobalShaderParameters.idSunLight));
	}

	// Token: 0x0600211D RID: 8477 RVA: 0x0009C52C File Offset: 0x0009A72C
	private float StaticLight()
	{
		return this.intensity;
	}

	// Token: 0x0600211E RID: 8478 RVA: 0x0009C534 File Offset: 0x0009A734
	private void Awake()
	{
		this.light = base.GetComponent<Light>();
		this.CacheAuthoredShadows();
		this.ApplyLightMode(this.mode);
	}

	// Token: 0x0600211F RID: 8479 RVA: 0x0009C554 File Offset: 0x0009A754
	private void OnEnable()
	{
		if (this.light == null)
		{
			this.light = base.GetComponent<Light>();
		}
		this.CacheAuthoredShadows();
		if (Application.isPlaying)
		{
			SwitchLightPolicy.OnLocalLightShadowPolicyChanged += this.ApplyLocalShadowPolicy;
			this.ApplyLocalShadowPolicy();
		}
		this.ApplyLightMode(this.mode);
	}

	// Token: 0x06002120 RID: 8480 RVA: 0x0009C5AB File Offset: 0x0009A7AB
	private void OnDisable()
	{
		SwitchLightPolicy.OnLocalLightShadowPolicyChanged -= this.ApplyLocalShadowPolicy;
		this.RestoreAuthoredShadowsIfNeeded();
	}

	// Token: 0x06002121 RID: 8481 RVA: 0x0009C5C4 File Offset: 0x0009A7C4
	private void RestoreAuthoredShadowsIfNeeded()
	{
		if (!this.authoredShadowsCached || this.light == null)
		{
			return;
		}
		if (base.gameObject.activeInHierarchy && PlatformUnityShadowPresets.Get(PlatformFeatures.GetActiveUnityShadowPreset()).localLightShadows != null)
		{
			return;
		}
		this.light.shadows = this.authoredShadows;
	}

	// Token: 0x06002122 RID: 8482 RVA: 0x0009C620 File Offset: 0x0009A820
	private void Update()
	{
		if (this.lightFunc == null || !this.light)
		{
			return;
		}
		float num = this.lightFunc();
		SwitchLightPolicy.ApplyRealLightEnabled(this.light, num);
		if (Mathf.Abs(this.light.intensity - num) > 0.0001f)
		{
			this.light.intensity = num;
		}
		Action<float> onLightIntensityChanged = this.OnLightIntensityChanged;
		if (onLightIntensityChanged == null)
		{
			return;
		}
		onLightIntensityChanged(num / this.intensity);
	}

	// Token: 0x06002123 RID: 8483 RVA: 0x0009C698 File Offset: 0x0009A898
	protected override void ApplyLightMode(DayNightLightBase.LightMode mode)
	{
		this.mode = mode;
		switch (mode)
		{
		case DayNightLightBase.LightMode.Night:
			this.lightFunc = new Func<float>(this.NightLight);
			break;
		case DayNightLightBase.LightMode.Day:
			this.lightFunc = new Func<float>(this.DayLight);
			break;
		case DayNightLightBase.LightMode.Static:
			this.lightFunc = new Func<float>(this.StaticLight);
			break;
		}
		this.Update();
		this.SyncLightFakerMode();
	}

	// Token: 0x06002124 RID: 8484 RVA: 0x0009C706 File Offset: 0x0009A906
	private void CacheAuthoredShadows()
	{
		if (this.authoredShadowsCached || this.light == null)
		{
			return;
		}
		this.authoredShadows = this.light.shadows;
		this.authoredShadowsCached = true;
	}

	// Token: 0x06002125 RID: 8485 RVA: 0x0009C737 File Offset: 0x0009A937
	private void ApplyLocalShadowPolicy()
	{
		if (this.light == null)
		{
			return;
		}
		SwitchLightPolicy.ApplyLocalLightShadowPolicy(this.light, this.authoredShadows);
	}

	// Token: 0x06002126 RID: 8486 RVA: 0x0009C75C File Offset: 0x0009A95C
	private void SyncLightFakerMode()
	{
		LightFaker lightFaker;
		if (!base.TryGetComponent<LightFaker>(out lightFaker) || lightFaker.mode == this.mode)
		{
			return;
		}
		lightFaker.ApplyLightModeInt((int)this.mode);
	}

	// Token: 0x04001DB9 RID: 7609
	public Action<float> OnLightIntensityChanged;

	// Token: 0x04001DBA RID: 7610
	public float intensity = 1f;

	// Token: 0x04001DBB RID: 7611
	private Light light;

	// Token: 0x04001DBC RID: 7612
	private Func<float> lightFunc;

	// Token: 0x04001DBD RID: 7613
	private LightShadows authoredShadows;

	// Token: 0x04001DBE RID: 7614
	private bool authoredShadowsCached;
}
