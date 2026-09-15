using System;
using UnityEngine;

// Token: 0x02000086 RID: 134
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Manga")]
public class CameraFilterPack_Drawing_Manga : MonoBehaviour
{
	// Token: 0x17000082 RID: 130
	// (get) Token: 0x0600034B RID: 843 RVA: 0x00011AB5 File Offset: 0x0000FCB5
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

	// Token: 0x0600034C RID: 844 RVA: 0x00011AE9 File Offset: 0x0000FCE9
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Manga");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600034D RID: 845 RVA: 0x00011B0C File Offset: 0x0000FD0C
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

	// Token: 0x0600034E RID: 846 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600034F RID: 847 RVA: 0x00011B92 File Offset: 0x0000FD92
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040004A8 RID: 1192
	public Shader SCShader;

	// Token: 0x040004A9 RID: 1193
	private float TimeX = 1f;

	// Token: 0x040004AA RID: 1194
	private Material SCMaterial;

	// Token: 0x040004AB RID: 1195
	[Range(1f, 8f)]
	public float DotSize = 4.72f;
}
