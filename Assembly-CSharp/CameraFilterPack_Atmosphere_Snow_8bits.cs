using System;
using UnityEngine;

// Token: 0x0200001F RID: 31
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Pixel/Snow_8bits")]
public class CameraFilterPack_Atmosphere_Snow_8bits : MonoBehaviour
{
	// Token: 0x1700001D RID: 29
	// (get) Token: 0x060000AC RID: 172 RVA: 0x00006419 File Offset: 0x00004619
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

	// Token: 0x060000AD RID: 173 RVA: 0x0000644D File Offset: 0x0000464D
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Atmosphere_Snow_8bits");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00006470 File Offset: 0x00004670
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
			this.material.SetFloat("_Value", this.Threshold);
			this.material.SetFloat("_Value2", this.Size);
			this.material.SetFloat("_Value3", this.DirectionX);
			this.material.SetFloat("_Value4", this.Fade);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x00006568 File Offset: 0x00004768
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400015A RID: 346
	public Shader SCShader;

	// Token: 0x0400015B RID: 347
	private float TimeX = 1f;

	// Token: 0x0400015C RID: 348
	private Vector4 ScreenResolution;

	// Token: 0x0400015D RID: 349
	private Material SCMaterial;

	// Token: 0x0400015E RID: 350
	[Range(0.9f, 2f)]
	public float Threshold = 1f;

	// Token: 0x0400015F RID: 351
	[Range(8f, 256f)]
	public float Size = 64f;

	// Token: 0x04000160 RID: 352
	[Range(-0.5f, 0.5f)]
	public float DirectionX;

	// Token: 0x04000161 RID: 353
	[Range(0f, 1f)]
	public float Fade = 1f;
}
