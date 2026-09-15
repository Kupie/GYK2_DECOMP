using System;
using UnityEngine;

// Token: 0x020000A5 RID: 165
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/DarkMatter")]
public class CameraFilterPack_FX_DarkMatter : MonoBehaviour
{
	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x06000406 RID: 1030 RVA: 0x00014827 File Offset: 0x00012A27
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

	// Token: 0x06000407 RID: 1031 RVA: 0x0001485B File Offset: 0x00012A5B
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_DarkMatter");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000408 RID: 1032 RVA: 0x0001487C File Offset: 0x00012A7C
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.TimeX += Time.deltaTime;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetFloat("_Value", this.Speed);
			this.material.SetFloat("_Value2", this.Intensity);
			this.material.SetFloat("_Value3", this.PosX);
			this.material.SetFloat("_Value4", this.PosY);
			this.material.SetFloat("_Value5", this.Zoom);
			this.material.SetFloat("_Value6", this.DarkIntensity);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x000149A0 File Offset: 0x00012BA0
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000577 RID: 1399
	public Shader SCShader;

	// Token: 0x04000578 RID: 1400
	private float TimeX = 1f;

	// Token: 0x04000579 RID: 1401
	private Vector4 ScreenResolution;

	// Token: 0x0400057A RID: 1402
	private Material SCMaterial;

	// Token: 0x0400057B RID: 1403
	[Range(-10f, 10f)]
	public float Speed = 0.8f;

	// Token: 0x0400057C RID: 1404
	[Range(0f, 1f)]
	public float Intensity = 1f;

	// Token: 0x0400057D RID: 1405
	[Range(-1f, 2f)]
	public float PosX = 0.5f;

	// Token: 0x0400057E RID: 1406
	[Range(-1f, 2f)]
	public float PosY = 0.5f;

	// Token: 0x0400057F RID: 1407
	[Range(-2f, 2f)]
	public float Zoom = 0.33f;

	// Token: 0x04000580 RID: 1408
	[Range(0f, 5f)]
	public float DarkIntensity = 2f;
}
