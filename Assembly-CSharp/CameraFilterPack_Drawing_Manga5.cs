using System;
using UnityEngine;

// Token: 0x0200008A RID: 138
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Manga5")]
public class CameraFilterPack_Drawing_Manga5 : MonoBehaviour
{
	// Token: 0x17000086 RID: 134
	// (get) Token: 0x06000363 RID: 867 RVA: 0x00011F06 File Offset: 0x00010106
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

	// Token: 0x06000364 RID: 868 RVA: 0x00011F3A File Offset: 0x0001013A
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Manga5");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000365 RID: 869 RVA: 0x00011F5C File Offset: 0x0001015C
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

	// Token: 0x06000366 RID: 870 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000367 RID: 871 RVA: 0x00011FE2 File Offset: 0x000101E2
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040004B8 RID: 1208
	public Shader SCShader;

	// Token: 0x040004B9 RID: 1209
	private float TimeX = 1f;

	// Token: 0x040004BA RID: 1210
	private Material SCMaterial;

	// Token: 0x040004BB RID: 1211
	[Range(1f, 8f)]
	public float DotSize = 4.72f;
}
