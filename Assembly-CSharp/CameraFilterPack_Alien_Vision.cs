using System;
using UnityEngine;

// Token: 0x02000019 RID: 25
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Alien/Vision")]
public class CameraFilterPack_Alien_Vision : MonoBehaviour
{
	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000088 RID: 136 RVA: 0x00005716 File Offset: 0x00003916
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

	// Token: 0x06000089 RID: 137 RVA: 0x0000574A File Offset: 0x0000394A
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Alien_Vision");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600008A RID: 138 RVA: 0x0000576C File Offset: 0x0000396C
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
			this.material.SetFloat("_Value", this.Therma_Variation);
			this.material.SetFloat("_Value2", this.Speed);
			this.material.SetFloat("_Value3", this.Burn);
			this.material.SetFloat("_Value4", this.SceneCut);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00005864 File Offset: 0x00003A64
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000116 RID: 278
	public Shader SCShader;

	// Token: 0x04000117 RID: 279
	private float TimeX = 1f;

	// Token: 0x04000118 RID: 280
	private Vector4 ScreenResolution;

	// Token: 0x04000119 RID: 281
	private Material SCMaterial;

	// Token: 0x0400011A RID: 282
	[Range(0f, 0.5f)]
	public float Therma_Variation = 0.5f;

	// Token: 0x0400011B RID: 283
	[Range(0f, 1f)]
	public float Speed = 0.5f;

	// Token: 0x0400011C RID: 284
	[Range(0f, 4f)]
	private float Burn;

	// Token: 0x0400011D RID: 285
	[Range(0f, 16f)]
	private float SceneCut = 1f;
}
