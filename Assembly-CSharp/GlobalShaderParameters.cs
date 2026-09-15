using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000B17 RID: 2839
[ExecuteInEditMode]
public class GlobalShaderParameters : LazySingleton<GlobalShaderParameters>
{
	// Token: 0x06004BA0 RID: 19360 RVA: 0x00165A15 File Offset: 0x00163C15
	private new void Awake()
	{
		this.ApplyShaderParameters();
	}

	// Token: 0x06004BA1 RID: 19361 RVA: 0x00165A20 File Offset: 0x00163C20
	public void ApplyShaderParameters()
	{
		Shader.SetGlobalColor(GlobalShaderParameters.idBacklightColor, this.backlightColor);
		Shader.SetGlobalFloat(GlobalShaderParameters.idBacklightContrast, this.backlightContrast);
		Shader.SetGlobalFloat(GlobalShaderParameters.idLightSpread, this.lightSpread);
		Shader.SetGlobalFloat(GlobalShaderParameters.idLightMaxBurn, this.lightMaxBurn);
		Shader.SetGlobalFloat(GlobalShaderParameters.idLightFocus, this.lightFocus);
		Shader.SetGlobalFloat(GlobalShaderParameters.idSunLight, this.sunLight);
		Shader.SetGlobalFloat(GlobalShaderParameters.idTimeOfDay, EnvironmentEngine.Instance.timeOfDay);
	}

	// Token: 0x04003CF6 RID: 15606
	private static readonly int idBacklightColor = Shader.PropertyToID("_BacklightColor");

	// Token: 0x04003CF7 RID: 15607
	private static readonly int idBacklightContrast = Shader.PropertyToID("_BacklightContrast");

	// Token: 0x04003CF8 RID: 15608
	private static readonly int idLightSpread = Shader.PropertyToID("_LightSpread");

	// Token: 0x04003CF9 RID: 15609
	private static readonly int idLightMaxBurn = Shader.PropertyToID("_LightMaxBurn");

	// Token: 0x04003CFA RID: 15610
	private static readonly int idLightFocus = Shader.PropertyToID("_LightFocus");

	// Token: 0x04003CFB RID: 15611
	public static readonly int idSunLight = Shader.PropertyToID("_SunLight");

	// Token: 0x04003CFC RID: 15612
	public static readonly int idTimeOfDay = Shader.PropertyToID("_TimeOfDay");

	// Token: 0x04003CFD RID: 15613
	public static readonly int idWindValue = Shader.PropertyToID("_WindValue");

	// Token: 0x04003CFE RID: 15614
	[Header("Backlight")]
	public Color backlightColor = new Color(0.2f, 0.2f, 0.6f, 0f);

	// Token: 0x04003CFF RID: 15615
	[Range(0f, 5f)]
	public float backlightContrast = 1f;

	// Token: 0x04003D00 RID: 15616
	[Space(10f)]
	[Header("Light overburn limit")]
	[Range(0f, 5f)]
	public float lightSpread = 1f;

	// Token: 0x04003D01 RID: 15617
	[Range(0.01f, 10f)]
	public float lightMaxBurn = 3f;

	// Token: 0x04003D02 RID: 15618
	[Range(0f, 5f)]
	public float lightFocus = 1f;

	// Token: 0x04003D03 RID: 15619
	[Space(10f)]
	[Header("Global params")]
	[Range(0f, 1f)]
	public float sunLight = 1f;
}
