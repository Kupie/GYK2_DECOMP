using System;
using UnityEngine;

// Token: 0x0200008B RID: 139
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Manga_Color")]
public class CameraFilterPack_Drawing_Manga_Color : MonoBehaviour
{
	// Token: 0x17000087 RID: 135
	// (get) Token: 0x06000369 RID: 873 RVA: 0x0001201A File Offset: 0x0001021A
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

	// Token: 0x0600036A RID: 874 RVA: 0x0001204E File Offset: 0x0001024E
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Manga_Color");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600036B RID: 875 RVA: 0x00012070 File Offset: 0x00010270
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

	// Token: 0x0600036C RID: 876 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600036D RID: 877 RVA: 0x000120F6 File Offset: 0x000102F6
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040004BC RID: 1212
	public Shader SCShader;

	// Token: 0x040004BD RID: 1213
	private float TimeX = 1f;

	// Token: 0x040004BE RID: 1214
	private Material SCMaterial;

	// Token: 0x040004BF RID: 1215
	[Range(1f, 8f)]
	public float DotSize = 1.6f;

	// Token: 0x040004C0 RID: 1216
	public static float ChangeDotSize;
}
