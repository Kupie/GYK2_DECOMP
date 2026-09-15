using System;
using UnityEngine;

// Token: 0x02000083 RID: 131
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Halftone")]
public class CameraFilterPack_Drawing_Halftone : MonoBehaviour
{
	// Token: 0x1700007F RID: 127
	// (get) Token: 0x06000339 RID: 825 RVA: 0x000116B9 File Offset: 0x0000F8B9
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

	// Token: 0x0600033A RID: 826 RVA: 0x000116ED File Offset: 0x0000F8ED
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Halftone");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600033B RID: 827 RVA: 0x00011710 File Offset: 0x0000F910
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
			this.material.SetFloat("_Distortion", this.Threshold);
			this.material.SetFloat("_DotSize", this.DotSize);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600033C RID: 828 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600033D RID: 829 RVA: 0x000117AC File Offset: 0x0000F9AC
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000497 RID: 1175
	public Shader SCShader;

	// Token: 0x04000498 RID: 1176
	private float TimeX = 1f;

	// Token: 0x04000499 RID: 1177
	private Material SCMaterial;

	// Token: 0x0400049A RID: 1178
	[Range(0f, 1f)]
	public float Threshold = 0.6f;

	// Token: 0x0400049B RID: 1179
	[Range(1f, 16f)]
	public float DotSize = 4f;
}
