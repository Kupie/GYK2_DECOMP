using System;
using UnityEngine;

// Token: 0x020000DD RID: 221
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Old Film/Cutting 1")]
public class CameraFilterPack_OldFilm_Cutting1 : MonoBehaviour
{
	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x06000566 RID: 1382 RVA: 0x0001AAAE File Offset: 0x00018CAE
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

	// Token: 0x06000567 RID: 1383 RVA: 0x0001AAE2 File Offset: 0x00018CE2
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_OldFilm1") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/OldFilm_Cutting1");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x0001AB18 File Offset: 0x00018D18
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
			this.material.SetFloat("_Value", this.Luminosity);
			this.material.SetFloat("_Value2", 1f - this.Vignette);
			this.material.SetFloat("_Value3", this.Negative);
			this.material.SetFloat("_Speed", this.Speed);
			this.material.SetTexture("_MainTex2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x0001ABFF File Offset: 0x00018DFF
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000719 RID: 1817
	public Shader SCShader;

	// Token: 0x0400071A RID: 1818
	private float TimeX = 1f;

	// Token: 0x0400071B RID: 1819
	[Range(0f, 10f)]
	public float Speed = 1f;

	// Token: 0x0400071C RID: 1820
	[Range(0f, 2f)]
	public float Luminosity = 1.5f;

	// Token: 0x0400071D RID: 1821
	[Range(0f, 1f)]
	public float Vignette = 1f;

	// Token: 0x0400071E RID: 1822
	[Range(0f, 2f)]
	public float Negative;

	// Token: 0x0400071F RID: 1823
	private Material SCMaterial;

	// Token: 0x04000720 RID: 1824
	private Texture2D Texture2;
}
