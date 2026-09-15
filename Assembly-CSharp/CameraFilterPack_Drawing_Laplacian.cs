using System;
using UnityEngine;

// Token: 0x02000084 RID: 132
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Laplacian")]
public class CameraFilterPack_Drawing_Laplacian : MonoBehaviour
{
	// Token: 0x17000080 RID: 128
	// (get) Token: 0x0600033F RID: 831 RVA: 0x000117EF File Offset: 0x0000F9EF
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

	// Token: 0x06000340 RID: 832 RVA: 0x00011823 File Offset: 0x0000FA23
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Laplacian");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000341 RID: 833 RVA: 0x00011844 File Offset: 0x0000FA44
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

	// Token: 0x06000342 RID: 834 RVA: 0x0000CFB4 File Offset: 0x0000B1B4
	private void Update()
	{
		bool isPlaying = Application.isPlaying;
	}

	// Token: 0x06000343 RID: 835 RVA: 0x000118E1 File Offset: 0x0000FAE1
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400049C RID: 1180
	public Shader SCShader;

	// Token: 0x0400049D RID: 1181
	private float TimeX = 1f;

	// Token: 0x0400049E RID: 1182
	private Vector4 ScreenResolution;

	// Token: 0x0400049F RID: 1183
	private Material SCMaterial;
}
