using System;
using UnityEngine;

// Token: 0x02000042 RID: 66
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Bloom")]
public class CameraFilterPack_Blur_Bloom : MonoBehaviour
{
	// Token: 0x1700003F RID: 63
	// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000B0B5 File Offset: 0x000092B5
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

	// Token: 0x060001B8 RID: 440 RVA: 0x0000B0E9 File Offset: 0x000092E9
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Blur_Bloom");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x0000B10C File Offset: 0x0000930C
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
			this.material.SetFloat("_Amount", this.Amount);
			this.material.SetFloat("_Glow", this.Glow);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060001BA RID: 442 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060001BB RID: 443 RVA: 0x0000B1D1 File Offset: 0x000093D1
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040002C1 RID: 705
	public Shader SCShader;

	// Token: 0x040002C2 RID: 706
	private float TimeX = 1f;

	// Token: 0x040002C3 RID: 707
	private Vector4 ScreenResolution;

	// Token: 0x040002C4 RID: 708
	private Material SCMaterial;

	// Token: 0x040002C5 RID: 709
	[Range(0f, 10f)]
	public float Amount = 4.5f;

	// Token: 0x040002C6 RID: 710
	[Range(0f, 1f)]
	public float Glow = 0.5f;
}
