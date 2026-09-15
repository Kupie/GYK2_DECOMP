using System;
using UnityEngine;

// Token: 0x0200005D RID: 93
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/Threshold")]
public class CameraFilterPack_Colors_Threshold : MonoBehaviour
{
	// Token: 0x17000059 RID: 89
	// (get) Token: 0x06000255 RID: 597 RVA: 0x0000E0A1 File Offset: 0x0000C2A1
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

	// Token: 0x06000256 RID: 598 RVA: 0x0000E0D5 File Offset: 0x0000C2D5
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Colors_Threshold");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000257 RID: 599 RVA: 0x0000E0F8 File Offset: 0x0000C2F8
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
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000258 RID: 600 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000259 RID: 601 RVA: 0x0000E17E File Offset: 0x0000C37E
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040003A5 RID: 933
	public Shader SCShader;

	// Token: 0x040003A6 RID: 934
	private float TimeX = 1f;

	// Token: 0x040003A7 RID: 935
	[Range(0f, 1f)]
	public float Threshold = 0.3f;

	// Token: 0x040003A8 RID: 936
	private Material SCMaterial;
}
