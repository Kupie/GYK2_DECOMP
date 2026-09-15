using System;
using UnityEngine;

// Token: 0x0200004B RID: 75
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Radial")]
public class CameraFilterPack_Blur_Radial : MonoBehaviour
{
	// Token: 0x17000048 RID: 72
	// (get) Token: 0x060001ED RID: 493 RVA: 0x0000BE8F File Offset: 0x0000A08F
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

	// Token: 0x060001EE RID: 494 RVA: 0x0000BEC3 File Offset: 0x0000A0C3
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Blur_Radial");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060001EF RID: 495 RVA: 0x0000BEE4 File Offset: 0x0000A0E4
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
			this.material.SetFloat("_Value", this.Intensity);
			this.material.SetFloat("_Value2", this.MovX);
			this.material.SetFloat("_Value3", this.MovY);
			this.material.SetFloat("_Value4", this.blurWidth);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x0000BFDC File Offset: 0x0000A1DC
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040002FE RID: 766
	public Shader SCShader;

	// Token: 0x040002FF RID: 767
	private float TimeX = 1f;

	// Token: 0x04000300 RID: 768
	private Vector4 ScreenResolution;

	// Token: 0x04000301 RID: 769
	private Material SCMaterial;

	// Token: 0x04000302 RID: 770
	[Range(-0.5f, 0.5f)]
	public float Intensity = 0.125f;

	// Token: 0x04000303 RID: 771
	[Range(-2f, 2f)]
	public float MovX = 0.5f;

	// Token: 0x04000304 RID: 772
	[Range(-2f, 2f)]
	public float MovY = 0.5f;

	// Token: 0x04000305 RID: 773
	[Range(0f, 10f)]
	private float blurWidth = 1f;
}
