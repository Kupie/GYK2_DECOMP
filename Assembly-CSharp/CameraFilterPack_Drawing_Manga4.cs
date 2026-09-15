using System;
using UnityEngine;

// Token: 0x02000089 RID: 137
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Manga4")]
public class CameraFilterPack_Drawing_Manga4 : MonoBehaviour
{
	// Token: 0x17000085 RID: 133
	// (get) Token: 0x0600035D RID: 861 RVA: 0x00011DF2 File Offset: 0x0000FFF2
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

	// Token: 0x0600035E RID: 862 RVA: 0x00011E26 File Offset: 0x00010026
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Manga4");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600035F RID: 863 RVA: 0x00011E48 File Offset: 0x00010048
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
			this.material.SetFloat("_DotSize", this.DotSize);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000360 RID: 864 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000361 RID: 865 RVA: 0x00011ECE File Offset: 0x000100CE
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040004B4 RID: 1204
	public Shader SCShader;

	// Token: 0x040004B5 RID: 1205
	private float TimeX = 1f;

	// Token: 0x040004B6 RID: 1206
	private Material SCMaterial;

	// Token: 0x040004B7 RID: 1207
	[Range(1f, 8f)]
	public float DotSize = 4.72f;
}
