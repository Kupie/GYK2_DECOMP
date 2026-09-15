using System;
using UnityEngine;

// Token: 0x02000546 RID: 1350
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class RopeMeshView : MonoBehaviour
{
	// Token: 0x1700059D RID: 1437
	// (get) Token: 0x0600229D RID: 8861 RVA: 0x000A25CE File Offset: 0x000A07CE
	public MeshRenderer MeshRenderer
	{
		get
		{
			return this.meshRenderer;
		}
	}

	// Token: 0x1700059E RID: 1438
	// (get) Token: 0x0600229E RID: 8862 RVA: 0x000A25D6 File Offset: 0x000A07D6
	public Vector3 MeshScale
	{
		get
		{
			return base.transform.localScale;
		}
	}

	// Token: 0x0600229F RID: 8863 RVA: 0x000A25E3 File Offset: 0x000A07E3
	private void Awake()
	{
		this.InitializeComponents();
	}

	// Token: 0x060022A0 RID: 8864 RVA: 0x000A25EB File Offset: 0x000A07EB
	private void InitializeComponents()
	{
		this.meshRenderer = base.GetComponent<MeshRenderer>();
		this.materialPropertyBlock = new MaterialPropertyBlock();
	}

	// Token: 0x060022A1 RID: 8865 RVA: 0x000A2604 File Offset: 0x000A0804
	public void EnsureInitialized()
	{
		if (this.meshRenderer == null || this.materialPropertyBlock == null)
		{
			this.InitializeComponents();
		}
	}

	// Token: 0x060022A2 RID: 8866 RVA: 0x000A2624 File Offset: 0x000A0824
	public void AlignToEndPoint(Vector3 localEndPoint)
	{
		Vector3 vector = new Vector3(localEndPoint.x, 0f, localEndPoint.z);
		if (vector.sqrMagnitude < 0.0001f)
		{
			base.transform.localRotation = Quaternion.identity;
			return;
		}
		Quaternion quaternion = Quaternion.FromToRotation(Vector3.left, vector.normalized);
		base.transform.localRotation = Quaternion.Euler(0f, quaternion.eulerAngles.y, 0f);
	}

	// Token: 0x060022A3 RID: 8867 RVA: 0x000A26A0 File Offset: 0x000A08A0
	public void UpdateRopeVisuals(Vector2 startPos2D, Vector2 endPos2D, float looseness, float thickness, Color ropeColor, float emissionIntensity, float gravityStrength, int textureResolution, bool antiAliasing)
	{
		if (this.meshRenderer == null || this.materialPropertyBlock == null)
		{
			return;
		}
		this.materialPropertyBlock.SetVector(this.startPointProp, startPos2D);
		this.materialPropertyBlock.SetVector(this.endPointProp, endPos2D);
		this.materialPropertyBlock.SetFloat(this.loosenessProp, looseness);
		this.materialPropertyBlock.SetFloat(this.thicknessProp, thickness);
		this.materialPropertyBlock.SetColor(this.ropeColorProp, ropeColor);
		this.materialPropertyBlock.SetFloat(this.emissionIntensityProp, emissionIntensity);
		this.materialPropertyBlock.SetFloat(this.gravityStrengthProp, gravityStrength);
		this.materialPropertyBlock.SetFloat(this.resolutionProp, (float)textureResolution);
		this.materialPropertyBlock.SetInt(this.antiAliasingProp, antiAliasing ? 1 : 0);
		this.materialPropertyBlock.SetFloat(this.pixelScaleProp, 2f);
		this.meshRenderer.SetPropertyBlock(this.materialPropertyBlock);
	}

	// Token: 0x060022A4 RID: 8868 RVA: 0x000A27A4 File Offset: 0x000A09A4
	public void UpdateRopeParameters(float looseness, float thickness, Color ropeColor, float emissionIntensity, float gravityStrength)
	{
		if (this.meshRenderer == null || this.materialPropertyBlock == null)
		{
			return;
		}
		this.materialPropertyBlock.SetFloat(this.loosenessProp, looseness);
		this.materialPropertyBlock.SetFloat(this.thicknessProp, thickness);
		this.materialPropertyBlock.SetColor(this.ropeColorProp, ropeColor);
		this.materialPropertyBlock.SetFloat(this.emissionIntensityProp, emissionIntensity);
		this.materialPropertyBlock.SetFloat(this.gravityStrengthProp, gravityStrength);
		this.meshRenderer.SetPropertyBlock(this.materialPropertyBlock);
	}

	// Token: 0x060022A5 RID: 8869 RVA: 0x000A2838 File Offset: 0x000A0A38
	public void UpdateQualitySettings(int textureResolution, bool antiAliasing)
	{
		if (this.meshRenderer == null || this.materialPropertyBlock == null)
		{
			return;
		}
		this.materialPropertyBlock.SetFloat(this.resolutionProp, (float)textureResolution);
		this.materialPropertyBlock.SetInt(this.antiAliasingProp, antiAliasing ? 1 : 0);
		this.materialPropertyBlock.SetFloat(this.pixelScaleProp, 2f);
		this.meshRenderer.SetPropertyBlock(this.materialPropertyBlock);
	}

	// Token: 0x04001F47 RID: 8007
	private MeshRenderer meshRenderer;

	// Token: 0x04001F48 RID: 8008
	private MaterialPropertyBlock materialPropertyBlock;

	// Token: 0x04001F49 RID: 8009
	private readonly int startPointProp = Shader.PropertyToID("_StartPoint");

	// Token: 0x04001F4A RID: 8010
	private readonly int endPointProp = Shader.PropertyToID("_EndPoint");

	// Token: 0x04001F4B RID: 8011
	private readonly int loosenessProp = Shader.PropertyToID("_Looseness");

	// Token: 0x04001F4C RID: 8012
	private readonly int thicknessProp = Shader.PropertyToID("_Thickness");

	// Token: 0x04001F4D RID: 8013
	private readonly int ropeColorProp = Shader.PropertyToID("_RopeColor");

	// Token: 0x04001F4E RID: 8014
	private readonly int emissionIntensityProp = Shader.PropertyToID("_EmissionIntensity");

	// Token: 0x04001F4F RID: 8015
	private readonly int gravityStrengthProp = Shader.PropertyToID("_GravityStrength");

	// Token: 0x04001F50 RID: 8016
	private readonly int resolutionProp = Shader.PropertyToID("_Resolution");

	// Token: 0x04001F51 RID: 8017
	private readonly int antiAliasingProp = Shader.PropertyToID("_AntiAliasing");

	// Token: 0x04001F52 RID: 8018
	private readonly int pixelScaleProp = Shader.PropertyToID("_PixelScale");
}
