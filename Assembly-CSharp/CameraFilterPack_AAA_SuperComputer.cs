using System;
using UnityEngine;

// Token: 0x02000015 RID: 21
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/AAA/Super Computer")]
public class CameraFilterPack_AAA_SuperComputer : MonoBehaviour
{
	// Token: 0x17000013 RID: 19
	// (get) Token: 0x06000070 RID: 112 RVA: 0x00004EC6 File Offset: 0x000030C6
	private Material material
	{
		get
		{
			if (this.SCMaterial == null)
			{
				this.SCMaterial = new Material(this.SCShader);
				this.SCMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.SCMaterial;
		}
	}

	// Token: 0x06000071 RID: 113 RVA: 0x00004EFA File Offset: 0x000030FA
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/AAA_Super_Computer");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00004F1C File Offset: 0x0000311C
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.TimeX += Time.deltaTime / 4f;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetFloat("_Value", this.ShapeFormula);
			this.material.SetFloat("_Value2", this.Shape);
			this.material.SetFloat("_PositionX", this.center.x);
			this.material.SetFloat("_PositionY", this.center.y);
			this.material.SetFloat("_Radius", this.Radius);
			this.material.SetFloat("_BorderSize", this._BorderSize);
			this.material.SetColor("_BorderColor", this._BorderColor);
			this.material.SetFloat("_AlphaHexa", this._AlphaHexa);
			this.material.SetFloat("_SpotSize", this._SpotSize);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00005092 File Offset: 0x00003292
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040000EE RID: 238
	public Shader SCShader;

	// Token: 0x040000EF RID: 239
	[Range(0f, 1f)]
	public float _AlphaHexa = 1f;

	// Token: 0x040000F0 RID: 240
	private float TimeX = 1f;

	// Token: 0x040000F1 RID: 241
	private Vector4 ScreenResolution;

	// Token: 0x040000F2 RID: 242
	private Material SCMaterial;

	// Token: 0x040000F3 RID: 243
	[Range(-20f, 20f)]
	public float ShapeFormula = 10f;

	// Token: 0x040000F4 RID: 244
	[Range(0f, 6f)]
	public float Shape = 1f;

	// Token: 0x040000F5 RID: 245
	[Range(-4f, 4f)]
	public float _BorderSize = 1f;

	// Token: 0x040000F6 RID: 246
	public Color _BorderColor = new Color(0f, 0.2f, 1f, 1f);

	// Token: 0x040000F7 RID: 247
	public float _SpotSize = 2.5f;

	// Token: 0x040000F8 RID: 248
	public Vector2 center = new Vector2(0f, 0f);

	// Token: 0x040000F9 RID: 249
	public float Radius = 0.77f;
}
