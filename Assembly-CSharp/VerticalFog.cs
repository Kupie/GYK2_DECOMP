using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001CF RID: 463
public class VerticalFog : MonoBehaviour
{
	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x06000BCE RID: 3022 RVA: 0x0003B7CE File Offset: 0x000399CE
	public static VerticalFog GlobalInstance
	{
		get
		{
			return VerticalFog.globalInstance;
		}
	}

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x06000BCF RID: 3023 RVA: 0x0003B7D5 File Offset: 0x000399D5
	public Color Color
	{
		get
		{
			if (!this.colorIsGradient)
			{
				return this.color;
			}
			return this.colorGradient.Evaluate(EnvironmentEngine.Instance.timeOfDay);
		}
	}

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x06000BD0 RID: 3024 RVA: 0x0003B7FB File Offset: 0x000399FB
	public Color AdditionalColor
	{
		get
		{
			if (!this.additionalColorIsGradient)
			{
				return this.additionalFogColor;
			}
			return this.additionalColorGradient.Evaluate(EnvironmentEngine.Instance.timeOfDay);
		}
	}

	// Token: 0x170001FC RID: 508
	// (get) Token: 0x06000BD1 RID: 3025 RVA: 0x0003B821 File Offset: 0x00039A21
	public WeatherComponent WeatherComponent
	{
		get
		{
			if (!this.weatherComponent)
			{
				this.weatherComponent = base.GetComponent<WeatherComponent>();
			}
			return this.weatherComponent;
		}
	}

	// Token: 0x06000BD2 RID: 3026 RVA: 0x0003B842 File Offset: 0x00039A42
	private void Awake()
	{
		if (this.isGlobalController && VerticalFog.globalInstance == null)
		{
			VerticalFog.globalInstance = this;
		}
	}

	// Token: 0x06000BD3 RID: 3027 RVA: 0x0003B860 File Offset: 0x00039A60
	private void OnEnable()
	{
		Debug.Log("#dbg# Enabling " + base.name, this);
		if (!this.isGlobalController)
		{
			VerticalFog.secondaryFogs.AddIfNotContains(this);
			Debug.Log("#dbg# Added " + base.name + " to GlobalInstance.secondaryFogs", this);
		}
	}

	// Token: 0x06000BD4 RID: 3028 RVA: 0x0003B8B1 File Offset: 0x00039AB1
	private void OnDisable()
	{
		if (!this.isGlobalController)
		{
			VerticalFog.secondaryFogs.Remove(this);
		}
	}

	// Token: 0x06000BD5 RID: 3029 RVA: 0x0003B8C7 File Offset: 0x00039AC7
	private void OnDestroy()
	{
		Debug.Log("#dbg# Destroying " + base.name, this);
	}

	// Token: 0x06000BD6 RID: 3030 RVA: 0x0003B8DF File Offset: 0x00039ADF
	public void ApplyFogParameters()
	{
		this.ApplyFogParametersWithIntensity(1f);
	}

	// Token: 0x06000BD7 RID: 3031 RVA: 0x0003B8EC File Offset: 0x00039AEC
	public void ApplyFogParametersWithIntensity(float presetIntensity)
	{
		if (!this.isGlobalController)
		{
			return;
		}
		if (this.isGlobalController && this.colorIsGradient)
		{
			Debug.LogError("Global vertical fog can't use a color gradient!");
		}
		Shader.SetGlobalFloat(VerticalFog.PropVertFog, (float)(this.fogEnabled ? 1 : 0));
		float num = 0f;
		if (!this.fogEnabled)
		{
			Shader.DisableKeyword("VERT_FOG");
		}
		else
		{
			Shader.EnableKeyword("VERT_FOG");
			this.fogPosition += Time.deltaTime * this.noiseAnimSpeed;
			Shader.SetGlobalColor(VerticalFog.PropFogColor, this.Color);
			Shader.SetGlobalFloat(VerticalFog.PropFogLevel, this.level);
			Shader.SetGlobalFloat(VerticalFog.PropFogHeight, this.height);
			num = this.intensity * presetIntensity * VerticalFogGlobalK.GlobalFogCoefficient;
			Shader.SetGlobalFloat(VerticalFog.PropFogIntensity, num);
			Shader.SetGlobalFloat(VerticalFog.PropFogPower, this.power);
			Shader.SetGlobalFloat(VerticalFog.PropFogScale, this.scale);
			Shader.SetGlobalFloat(VerticalFog.PropFogXScale, this.xscale);
			Shader.SetGlobalFloat(VerticalFog.PropNoiseIntensity, this.noiseIntensity);
			Shader.SetGlobalFloat(VerticalFog.PropFogPosition, this.fogPosition);
			Shader.SetGlobalFloat(VerticalFog.PropFogAmbientIntensity, this.ambientInfluence);
			Shader.SetGlobalColor(VerticalFog.PropFogAdditionalLight, this.additionalFogColor);
		}
		HBAOColorManager.UpdateColorFromFog(this, num);
	}

	// Token: 0x06000BD8 RID: 3032 RVA: 0x0003BA34 File Offset: 0x00039C34
	public void GlobalFogUpdate()
	{
		if (!this.isGlobalController)
		{
			Debug.LogError("Calling GlobalFogUpdate() for a non-global controller.");
			return;
		}
		this.fogEnabled = false;
		Color color = new Color(0f, 0f, 0f, 0f);
		Color color2 = new Color(0f, 0f, 0f, 0f);
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		float num9 = 0f;
		float num10 = 0f;
		int num11 = 0;
		VerticalFog verticalFog = null;
		foreach (VerticalFog verticalFog2 in VerticalFog.secondaryFogs)
		{
			if (verticalFog2.priorityFog && verticalFog2.fogEnabled && verticalFog2.intensity > 0f)
			{
				verticalFog = verticalFog2;
				break;
			}
		}
		VerticalFog verticalFog3 = null;
		VerticalFog verticalFog4 = null;
		foreach (VerticalFog verticalFog5 in VerticalFog.secondaryFogs)
		{
			if (verticalFog5.fogEnabled && verticalFog5.WeatherComponent)
			{
				if (verticalFog5.WeatherComponent.Animating == WeatherComponent.AnimationType.FadeIn)
				{
					verticalFog3 = verticalFog5;
				}
				else if (verticalFog5.WeatherComponent.Animating == WeatherComponent.AnimationType.FadeOut)
				{
					verticalFog4 = verticalFog5;
				}
			}
		}
		if (verticalFog3 && verticalFog4 && verticalFog == null)
		{
			this.fogEnabled = true;
			float num12 = verticalFog3.WeatherComponent.intensity;
			this.color = Color.Lerp(verticalFog4.Color, verticalFog3.Color, num12);
			this.level = Mathf.Lerp(verticalFog4.level, verticalFog3.level, num12);
			this.height = Mathf.Lerp(verticalFog4.height, verticalFog3.height, num12);
			this.power = Mathf.Lerp(verticalFog4.power, verticalFog3.power, num12);
			this.scale = Mathf.Lerp(verticalFog4.scale, verticalFog3.scale, num12);
			this.xscale = Mathf.Lerp(verticalFog4.xscale, verticalFog3.xscale, num12);
			this.noiseIntensity = Mathf.Lerp(verticalFog4.noiseIntensity, verticalFog3.noiseIntensity, num12);
			this.noiseAnimSpeed = Mathf.Lerp(verticalFog4.noiseAnimSpeed, verticalFog3.noiseAnimSpeed, num12);
			this.ambientInfluence = Mathf.Lerp(verticalFog4.ambientInfluence, verticalFog3.ambientInfluence, num12);
			this.additionalFogColor = Color.Lerp(verticalFog4.AdditionalColor, verticalFog3.AdditionalColor, num12);
			this.intensity = Mathf.Lerp(verticalFog4.intensity, verticalFog3.intensity, num12);
			this.ApplyFogParameters();
			return;
		}
		foreach (VerticalFog verticalFog6 in VerticalFog.secondaryFogs)
		{
			if (!(verticalFog != null) || verticalFog6.priorityFog)
			{
				float num13 = verticalFog6.intensity;
				if (verticalFog6.WeatherComponent)
				{
					num13 *= verticalFog6.WeatherComponent.intensity;
				}
				if (verticalFog6.fogEnabled && verticalFog6.gameObject.activeSelf && num13 != 0f)
				{
					this.fogEnabled = true;
					num9 += num13;
					num10 += num13 * num13;
					num11++;
					color += verticalFog6.Color * num13;
					num += verticalFog6.level * num13;
					num2 += verticalFog6.height * num13;
					num3 += verticalFog6.power * num13;
					num4 += verticalFog6.scale * num13;
					num5 += verticalFog6.xscale * num13;
					num6 += verticalFog6.noiseIntensity * num13;
					num7 += verticalFog6.noiseAnimSpeed * num13;
					num8 += verticalFog6.ambientInfluence * num13;
					color2 += verticalFog6.AdditionalColor * num13;
				}
			}
		}
		if (this.fogEnabled && num9 > 0f)
		{
			this.color = color / num9;
			this.level = num / num9;
			this.height = num2 / num9;
			this.intensity = Mathf.Min(1f, num9);
			this.power = num3 / num9;
			this.scale = num4 / num9;
			this.xscale = num5 / num9;
			this.noiseIntensity = num6 / num9;
			this.noiseAnimSpeed = num7 / num9;
			this.additionalFogColor = color2 / num9;
			this.ambientInfluence = num8 / num9;
		}
		this.ApplyFogParameters();
	}

	// Token: 0x04000CD7 RID: 3287
	public bool isGlobalController;

	// Token: 0x04000CD8 RID: 3288
	private static VerticalFog globalInstance = null;

	// Token: 0x04000CD9 RID: 3289
	[NonSerialized]
	public static List<VerticalFog> secondaryFogs = new List<VerticalFog>();

	// Token: 0x04000CDA RID: 3290
	[Space(20f)]
	public bool fogEnabled;

	// Token: 0x04000CDB RID: 3291
	public bool priorityFog;

	// Token: 0x04000CDC RID: 3292
	[Space]
	public bool colorIsGradient;

	// Token: 0x04000CDD RID: 3293
	[SerializeField]
	private Color color;

	// Token: 0x04000CDE RID: 3294
	public Gradient colorGradient = new Gradient();

	// Token: 0x04000CDF RID: 3295
	[Range(-10f, 30f)]
	public float level;

	// Token: 0x04000CE0 RID: 3296
	[Range(0f, 20f)]
	public float height;

	// Token: 0x04000CE1 RID: 3297
	[Range(0f, 20f)]
	public float power = 1f;

	// Token: 0x04000CE2 RID: 3298
	[Range(0f, 1f)]
	public float intensity;

	// Token: 0x04000CE3 RID: 3299
	[Space(10f)]
	[Range(0f, 1f)]
	public float ambientInfluence = 0.5f;

	// Token: 0x04000CE4 RID: 3300
	public bool additionalColorIsGradient;

	// Token: 0x04000CE5 RID: 3301
	public Color additionalFogColor = Color.black;

	// Token: 0x04000CE6 RID: 3302
	public Gradient additionalColorGradient = new Gradient();

	// Token: 0x04000CE7 RID: 3303
	[Space(10f)]
	[Range(0f, 3f)]
	public float scale = 1f;

	// Token: 0x04000CE8 RID: 3304
	[Range(0f, 3f)]
	public float xscale = 1f;

	// Token: 0x04000CE9 RID: 3305
	[Range(0f, 1f)]
	public float noiseIntensity = 1f;

	// Token: 0x04000CEA RID: 3306
	[Range(-1f, 1f)]
	public float noiseAnimSpeed = 1f;

	// Token: 0x04000CEB RID: 3307
	private float fogPosition;

	// Token: 0x04000CEC RID: 3308
	private static readonly int PropVertFog = Shader.PropertyToID("_VertFog");

	// Token: 0x04000CED RID: 3309
	private static readonly int PropFogColor = Shader.PropertyToID("_FogColor");

	// Token: 0x04000CEE RID: 3310
	private static readonly int PropFogLevel = Shader.PropertyToID("_FogLevel");

	// Token: 0x04000CEF RID: 3311
	private static readonly int PropFogHeight = Shader.PropertyToID("_FogHeight");

	// Token: 0x04000CF0 RID: 3312
	private static readonly int PropFogPower = Shader.PropertyToID("_FogPower");

	// Token: 0x04000CF1 RID: 3313
	private static readonly int PropFogIntensity = Shader.PropertyToID("_FogIntensity");

	// Token: 0x04000CF2 RID: 3314
	private static readonly int PropFogScale = Shader.PropertyToID("_FogScale");

	// Token: 0x04000CF3 RID: 3315
	private static readonly int PropFogXScale = Shader.PropertyToID("_FogXScale");

	// Token: 0x04000CF4 RID: 3316
	private static readonly int PropFogPosition = Shader.PropertyToID("_FogPosition");

	// Token: 0x04000CF5 RID: 3317
	private static readonly int PropNoiseIntensity = Shader.PropertyToID("_FogNoiseIntensity");

	// Token: 0x04000CF6 RID: 3318
	private static readonly int PropNoiseAnimSpeed = Shader.PropertyToID("_FogNoiseAnimSpeed");

	// Token: 0x04000CF7 RID: 3319
	private static readonly int PropFogAmbientIntensity = Shader.PropertyToID("_FogAmbientIntensity");

	// Token: 0x04000CF8 RID: 3320
	private static readonly int PropFogAdditionalLight = Shader.PropertyToID("_FogAdditionalLight");

	// Token: 0x04000CF9 RID: 3321
	private WeatherComponent weatherComponent;
}
