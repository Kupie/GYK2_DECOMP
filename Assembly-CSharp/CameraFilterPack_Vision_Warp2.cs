using System;
using UnityEngine;

// Token: 0x02000118 RID: 280
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/Warp2")]
public class CameraFilterPack_Vision_Warp2 : MonoBehaviour
{
	// Token: 0x17000114 RID: 276
	// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00020645 File Offset: 0x0001E845
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

	// Token: 0x060006CA RID: 1738 RVA: 0x00020679 File Offset: 0x0001E879
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Vision_Warp2");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060006CB RID: 1739 RVA: 0x0002069C File Offset: 0x0001E89C
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

	// Token: 0x060006CC RID: 1740 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x00020794 File Offset: 0x0001E994
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040008B9 RID: 2233
	public Shader SCShader;

	// Token: 0x040008BA RID: 2234
	private float TimeX = 1f;

	// Token: 0x040008BB RID: 2235
	private Vector4 ScreenResolution;

	// Token: 0x040008BC RID: 2236
	private Material SCMaterial;

	// Token: 0x040008BD RID: 2237
	[Range(0f, 1f)]
	public float Value = 0.5f;

	// Token: 0x040008BE RID: 2238
	[Range(0f, 1f)]
	public float Value2 = 0.2f;

	// Token: 0x040008BF RID: 2239
	[Range(-1f, 2f)]
	public float Intensity = 1f;

	// Token: 0x040008C0 RID: 2240
	[Range(0f, 10f)]
	private float Value4 = 1f;
}
