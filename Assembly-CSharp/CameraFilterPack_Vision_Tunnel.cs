using System;
using UnityEngine;

// Token: 0x02000116 RID: 278
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/Tunnel")]
public class CameraFilterPack_Vision_Tunnel : MonoBehaviour
{
	// Token: 0x17000112 RID: 274
	// (get) Token: 0x060006BD RID: 1725 RVA: 0x000202F4 File Offset: 0x0001E4F4
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

	// Token: 0x060006BE RID: 1726 RVA: 0x00020328 File Offset: 0x0001E528
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Vision_Tunnel");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x0002034C File Offset: 0x0001E54C
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
			this.material.SetFloat("_Value", this.Value);
			this.material.SetFloat("_Value2", this.Value2);
			this.material.SetFloat("_Value3", this.Intensity);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060006C0 RID: 1728 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x00020444 File Offset: 0x0001E644
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040008A9 RID: 2217
	public Shader SCShader;

	// Token: 0x040008AA RID: 2218
	private float TimeX = 1f;

	// Token: 0x040008AB RID: 2219
	private Vector4 ScreenResolution;

	// Token: 0x040008AC RID: 2220
	private Material SCMaterial;

	// Token: 0x040008AD RID: 2221
	[Range(0f, 1f)]
	public float Value = 0.6f;

	// Token: 0x040008AE RID: 2222
	[Range(0f, 1f)]
	public float Value2 = 0.4f;

	// Token: 0x040008AF RID: 2223
	[Range(0f, 1f)]
	public float Intensity = 1f;

	// Token: 0x040008B0 RID: 2224
	[Range(0f, 10f)]
	private float Value4 = 1f;
}
