using System;
using UnityEngine;

// Token: 0x020000F6 RID: 246
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/Movie Noise")]
public class CameraFilterPack_TV_MovieNoise : MonoBehaviour
{
	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x060005FD RID: 1533 RVA: 0x0001D254 File Offset: 0x0001B454
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

	// Token: 0x060005FE RID: 1534 RVA: 0x0001D288 File Offset: 0x0001B488
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_MovieNoise");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x0001D2AC File Offset: 0x0001B4AC
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
			this.material.SetFloat("_Fade", this.Fade);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x0001D362 File Offset: 0x0001B562
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040007C9 RID: 1993
	public Shader SCShader;

	// Token: 0x040007CA RID: 1994
	private float TimeX = 1f;

	// Token: 0x040007CB RID: 1995
	private Vector4 ScreenResolution;

	// Token: 0x040007CC RID: 1996
	private Material SCMaterial;

	// Token: 0x040007CD RID: 1997
	[Range(0.0001f, 1f)]
	public float Fade = 0.01f;
}
