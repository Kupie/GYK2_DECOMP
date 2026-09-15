using System;
using UnityEngine;

// Token: 0x02000099 RID: 153
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Edge/Sobel")]
public class CameraFilterPack_Edge_Sobel : MonoBehaviour
{
	// Token: 0x17000095 RID: 149
	// (get) Token: 0x060003BD RID: 957 RVA: 0x000136BF File Offset: 0x000118BF
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

	// Token: 0x060003BE RID: 958 RVA: 0x000136F3 File Offset: 0x000118F3
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Edge_Sobel");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060003BF RID: 959 RVA: 0x00013714 File Offset: 0x00011914
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
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x000137B1 File Offset: 0x000119B1
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000527 RID: 1319
	public Shader SCShader;

	// Token: 0x04000528 RID: 1320
	private float TimeX = 1f;

	// Token: 0x04000529 RID: 1321
	private Vector4 ScreenResolution;

	// Token: 0x0400052A RID: 1322
	private Material SCMaterial;
}
