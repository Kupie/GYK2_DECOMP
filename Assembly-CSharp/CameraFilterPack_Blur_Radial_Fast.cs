using System;
using UnityEngine;

// Token: 0x0200004C RID: 76
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Radial_Fast")]
public class CameraFilterPack_Blur_Radial_Fast : MonoBehaviour
{
	// Token: 0x17000049 RID: 73
	// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000C035 File Offset: 0x0000A235
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

	// Token: 0x060001F4 RID: 500 RVA: 0x0000C069 File Offset: 0x0000A269
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Blur_Radial_Fast");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x0000C08C File Offset: 0x0000A28C
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
			this.material.SetFloat("_Value", this.Intensity);
			this.material.SetFloat("_Value2", this.MovX);
			this.material.SetFloat("_Value3", this.MovY);
			this.material.SetFloat("_Value4", this.blurWidth);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x0000C184 File Offset: 0x0000A384
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000306 RID: 774
	public Shader SCShader;

	// Token: 0x04000307 RID: 775
	private float TimeX = 1f;

	// Token: 0x04000308 RID: 776
	private Vector4 ScreenResolution;

	// Token: 0x04000309 RID: 777
	private Material SCMaterial;

	// Token: 0x0400030A RID: 778
	[Range(-0.5f, 0.5f)]
	public float Intensity = 0.125f;

	// Token: 0x0400030B RID: 779
	[Range(-2f, 2f)]
	public float MovX = 0.5f;

	// Token: 0x0400030C RID: 780
	[Range(-2f, 2f)]
	public float MovY = 0.5f;

	// Token: 0x0400030D RID: 781
	[Range(0f, 10f)]
	private float blurWidth = 1f;
}
