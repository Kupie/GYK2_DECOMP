using System;
using UnityEngine;

// Token: 0x02000093 RID: 147
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Toon")]
public class CameraFilterPack_Drawing_Toon : MonoBehaviour
{
	// Token: 0x1700008F RID: 143
	// (get) Token: 0x06000399 RID: 921 RVA: 0x00012F7A File Offset: 0x0001117A
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

	// Token: 0x0600039A RID: 922 RVA: 0x00012FAE File Offset: 0x000111AE
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Toon");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600039B RID: 923 RVA: 0x00012FD0 File Offset: 0x000111D0
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

	// Token: 0x0600039C RID: 924 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600039D RID: 925 RVA: 0x0001306C File Offset: 0x0001126C
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000509 RID: 1289
	public Shader SCShader;

	// Token: 0x0400050A RID: 1290
	private Material SCMaterial;

	// Token: 0x0400050B RID: 1291
	private float TimeX = 1f;

	// Token: 0x0400050C RID: 1292
	[Range(0f, 2f)]
	public float Threshold = 1f;

	// Token: 0x0400050D RID: 1293
	[Range(0f, 8f)]
	public float DotSize = 1f;
}
