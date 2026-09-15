using System;
using UnityEngine;

// Token: 0x02000ABD RID: 2749
[CreateAssetMenu(fileName = "LightEnvironmentPreset", menuName = "GK2/Light/LightEnvironmentPreset")]
public class LightEnvironmentPreset : ScriptableObject
{
	// Token: 0x17000B43 RID: 2883
	// (get) Token: 0x06004A6A RID: 19050 RVA: 0x0015EE96 File Offset: 0x0015D096
	public Texture LutTexture
	{
		get
		{
			if (!this.lutIsLerped)
			{
				return this.lutTexture;
			}
			return this.GetLerpedLut();
		}
	}

	// Token: 0x06004A6B RID: 19051 RVA: 0x00002318 File Offset: 0x00000518
	private void ApplyPreset()
	{
	}

	// Token: 0x06004A6C RID: 19052 RVA: 0x0015EEB0 File Offset: 0x0015D0B0
	public static void Lerp(LightEnvironmentPreset dest, LightEnvironmentPreset p1, LightEnvironmentPreset p2, float v)
	{
		if (dest == null || p1 == null || p2 == null)
		{
			return;
		}
		dest.sunLightRotation = LightEnvironmentPreset.Lerp(p2.e_sunLightRotation, p1.sunLightRotation, p2.sunLightRotation, v);
		dest.sunLightColor = LightEnvironmentPreset.Lerp(p2.e_sunLightColor, p1.sunLightColor, p2.sunLightColor, v);
		dest.sunLightIntensity = LightEnvironmentPreset.Lerp(p2.e_sunLightIntensity, p1.sunLightIntensity, p2.sunLightIntensity, v);
		dest.sunLightShadowStrength = LightEnvironmentPreset.Lerp(p2.e_sunLightShadowStrength, p1.sunLightShadowStrength, p2.sunLightShadowStrength, v);
		dest.sunLightAmount = LightEnvironmentPreset.Lerp(p2.e_sunLightAmount, p1.sunLightAmount, p2.sunLightAmount, v);
		if (p2.e_sunLightIntensityLim)
		{
			dest.sunLightIntensity = Mathf.Min(dest.sunLightIntensity, p2.sunLightIntensityLim);
		}
		if (p2.e_sunLightShadowStrengthLim)
		{
			dest.sunLightShadowStrength = Mathf.Min(dest.sunLightShadowStrength, p2.sunLightShadowStrengthLim);
		}
		dest.backLightColor = LightEnvironmentPreset.Lerp(p2.e_backLightColor, p1.backLightColor, p2.backLightColor, v);
		dest.backLightIntensity = LightEnvironmentPreset.Lerp(p2.e_backLightIntensity, p1.backLightIntensity, p2.backLightIntensity, v);
		dest.gspBacklightColor = LightEnvironmentPreset.Lerp(p2.e_gspBacklightColor, p1.gspBacklightColor, p2.gspBacklightColor, v);
		dest.gspMaxLightBurn = LightEnvironmentPreset.Lerp(p2.e_gspMaxLightBurn, p1.gspMaxLightBurn, p2.gspMaxLightBurn, v);
		dest.ambientLightColor = LightEnvironmentPreset.Lerp(p2.e_ambientLightColor, p1.ambientLightColor, p2.ambientLightColor, v);
		Texture texture = ((p1.lutIsLerped && p2.e_lutTexture) ? p1.GetLerpedLut() : p1.lutTexture);
		if (texture == null)
		{
			dest.lutIsLerped = false;
			dest.lutTexture = p2.lutTexture;
		}
		else if (p2.lutTexture == null)
		{
			dest.lutIsLerped = false;
			dest.lutTexture = texture;
		}
		else
		{
			dest.lutIsLerped = true;
			dest.lutTexture = texture;
			dest.lutTexture2 = p2.lutTexture;
			dest.lutLerpFactor = v;
		}
		dest.additiveBloomThreshold = LightEnvironmentPreset.Lerp(p2.e_additiveBloom, p1.additiveBloomThreshold, p2.additiveBloomThreshold, v);
	}

	// Token: 0x06004A6D RID: 19053 RVA: 0x0015F0DD File Offset: 0x0015D2DD
	private static Color Lerp(bool enable, Color c1, Color c2, float v)
	{
		if (!enable)
		{
			return c1;
		}
		return Color.Lerp(c1, c2, v * c2.a);
	}

	// Token: 0x06004A6E RID: 19054 RVA: 0x0015F0F3 File Offset: 0x0015D2F3
	private static float Lerp(bool enable, float a1, float a2, float v)
	{
		if (!enable)
		{
			return a1;
		}
		return Mathf.Lerp(a1, a2, v);
	}

	// Token: 0x06004A6F RID: 19055 RVA: 0x0015F102 File Offset: 0x0015D302
	private static Vector3 Lerp(bool enable, Vector3 a1, Vector3 a2, float v)
	{
		if (!enable)
		{
			return a1;
		}
		return Vector3.Lerp(a1, a2, v);
	}

	// Token: 0x06004A70 RID: 19056 RVA: 0x0015F114 File Offset: 0x0015D314
	private Texture GetLerpedLut()
	{
		if (this.lutRT == null)
		{
			this.lutRT = RenderTexture.GetTemporary(this.lutTexture.width, this.lutTexture.height);
		}
		if (this.lutLerpMaterial == null)
		{
			this.lutLerpMaterial = new Material(Shader.Find("Custom/TextureLerp"));
		}
		this.lutLerpMaterial.SetTexture(LightEnvironmentPreset.idMainTex, this.lutTexture);
		this.lutLerpMaterial.SetTexture(LightEnvironmentPreset.idSecondaryTex, this.lutTexture2);
		this.lutLerpMaterial.SetFloat(LightEnvironmentPreset.idLerpFactor, this.lutLerpFactor);
		Graphics.Blit(null, this.lutRT, this.lutLerpMaterial);
		return this.lutRT;
	}

	// Token: 0x04003A29 RID: 14889
	[Header("")]
	[Space(5f)]
	[SerializeField]
	private bool e_sunLightRotation = true;

	// Token: 0x04003A2A RID: 14890
	public Vector3 sunLightRotation;

	// Token: 0x04003A2B RID: 14891
	[SerializeField]
	private bool e_sunLightColor = true;

	// Token: 0x04003A2C RID: 14892
	public Color sunLightColor;

	// Token: 0x04003A2D RID: 14893
	[SerializeField]
	private bool e_sunLightIntensity = true;

	// Token: 0x04003A2E RID: 14894
	public float sunLightIntensity;

	// Token: 0x04003A2F RID: 14895
	[SerializeField]
	private bool e_sunLightIntensityLim;

	// Token: 0x04003A30 RID: 14896
	public float sunLightIntensityLim;

	// Token: 0x04003A31 RID: 14897
	[SerializeField]
	private bool e_sunLightShadowStrength = true;

	// Token: 0x04003A32 RID: 14898
	public float sunLightShadowStrength;

	// Token: 0x04003A33 RID: 14899
	[SerializeField]
	private bool e_sunLightShadowStrengthLim;

	// Token: 0x04003A34 RID: 14900
	public float sunLightShadowStrengthLim;

	// Token: 0x04003A35 RID: 14901
	[SerializeField]
	private bool e_sunLightAmount = true;

	// Token: 0x04003A36 RID: 14902
	public float sunLightAmount;

	// Token: 0x04003A37 RID: 14903
	[Header("")]
	[Space(15f)]
	[SerializeField]
	private bool e_backLightColor = true;

	// Token: 0x04003A38 RID: 14904
	[Space(10f)]
	public Color backLightColor;

	// Token: 0x04003A39 RID: 14905
	[SerializeField]
	private bool e_backLightIntensity = true;

	// Token: 0x04003A3A RID: 14906
	public float backLightIntensity;

	// Token: 0x04003A3B RID: 14907
	[Header("")]
	[Space(15f)]
	[SerializeField]
	private bool e_gspBacklightColor = true;

	// Token: 0x04003A3C RID: 14908
	[Space(10f)]
	public Color gspBacklightColor;

	// Token: 0x04003A3D RID: 14909
	[SerializeField]
	private bool e_gspMaxLightBurn = true;

	// Token: 0x04003A3E RID: 14910
	public float gspMaxLightBurn = 1f;

	// Token: 0x04003A3F RID: 14911
	[Header("")]
	[Space(15f)]
	[SerializeField]
	private bool e_ambientLightColor = true;

	// Token: 0x04003A40 RID: 14912
	[Space(10f)]
	[ColorUsage(true, true)]
	public Color ambientLightColor;

	// Token: 0x04003A41 RID: 14913
	[Header("")]
	[Space(15f)]
	[SerializeField]
	private bool e_lutTexture = true;

	// Token: 0x04003A42 RID: 14914
	[Space(10f)]
	private Texture lutTexture;

	// Token: 0x04003A43 RID: 14915
	[Header("")]
	[Space(15f)]
	[SerializeField]
	private bool e_additiveBloom;

	// Token: 0x04003A44 RID: 14916
	[Space(10f)]
	[Range(-1f, 1f)]
	public float additiveBloomThreshold;

	// Token: 0x04003A45 RID: 14917
	private bool lutIsLerped;

	// Token: 0x04003A46 RID: 14918
	private Texture lutTexture2;

	// Token: 0x04003A47 RID: 14919
	private float lutLerpFactor;

	// Token: 0x04003A48 RID: 14920
	private RenderTexture lutRT;

	// Token: 0x04003A49 RID: 14921
	private Material lutLerpMaterial;

	// Token: 0x04003A4A RID: 14922
	private static readonly int idMainTex = Shader.PropertyToID("_MainTex");

	// Token: 0x04003A4B RID: 14923
	private static readonly int idSecondaryTex = Shader.PropertyToID("_SecondaryTex");

	// Token: 0x04003A4C RID: 14924
	private static readonly int idLerpFactor = Shader.PropertyToID("_LerpFactor");
}
