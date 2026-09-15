using System;
using UnityEngine;

// Token: 0x02000048 RID: 72
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/GaussianBlur")]
public class CameraFilterPack_Blur_GaussianBlur : MonoBehaviour
{
	// Token: 0x17000045 RID: 69
	// (get) Token: 0x060001DB RID: 475 RVA: 0x0000BA2F File Offset: 0x00009C2F
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

	// Token: 0x060001DC RID: 476 RVA: 0x0000BA63 File Offset: 0x00009C63
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Blur_GaussianBlur");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060001DD RID: 477 RVA: 0x0000BA84 File Offset: 0x00009C84
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
			this.material.SetFloat("_Distortion", this.Size);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060001DE RID: 478 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060001DF RID: 479 RVA: 0x0000BB33 File Offset: 0x00009D33
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040002EC RID: 748
	public Shader SCShader;

	// Token: 0x040002ED RID: 749
	private float TimeX = 1f;

	// Token: 0x040002EE RID: 750
	[Range(1f, 16f)]
	public float Size = 10f;

	// Token: 0x040002EF RID: 751
	private Vector4 ScreenResolution;

	// Token: 0x040002F0 RID: 752
	private Material SCMaterial;
}
