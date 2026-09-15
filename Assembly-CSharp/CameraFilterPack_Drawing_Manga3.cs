using System;
using UnityEngine;

// Token: 0x02000088 RID: 136
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Manga3")]
public class CameraFilterPack_Drawing_Manga3 : MonoBehaviour
{
	// Token: 0x17000084 RID: 132
	// (get) Token: 0x06000357 RID: 855 RVA: 0x00011CDE File Offset: 0x0000FEDE
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

	// Token: 0x06000358 RID: 856 RVA: 0x00011D12 File Offset: 0x0000FF12
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Manga3");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000359 RID: 857 RVA: 0x00011D34 File Offset: 0x0000FF34
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

	// Token: 0x0600035A RID: 858 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600035B RID: 859 RVA: 0x00011DBA File Offset: 0x0000FFBA
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040004B0 RID: 1200
	public Shader SCShader;

	// Token: 0x040004B1 RID: 1201
	private float TimeX = 1f;

	// Token: 0x040004B2 RID: 1202
	private Material SCMaterial;

	// Token: 0x040004B3 RID: 1203
	[Range(1f, 8f)]
	public float DotSize = 4.72f;
}
