using System;
using UnityEngine;

// Token: 0x02000117 RID: 279
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/Warp")]
public class CameraFilterPack_Vision_Warp : MonoBehaviour
{
	// Token: 0x17000113 RID: 275
	// (get) Token: 0x060006C3 RID: 1731 RVA: 0x0002049D File Offset: 0x0001E69D
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

	// Token: 0x060006C4 RID: 1732 RVA: 0x000204D1 File Offset: 0x0001E6D1
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Vision_Warp");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x000204F4 File Offset: 0x0001E6F4
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
			this.material.SetFloat("_Value3", this.Value3);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060006C7 RID: 1735 RVA: 0x000205EC File Offset: 0x0001E7EC
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040008B1 RID: 2225
	public Shader SCShader;

	// Token: 0x040008B2 RID: 2226
	private float TimeX = 1f;

	// Token: 0x040008B3 RID: 2227
	private Vector4 ScreenResolution;

	// Token: 0x040008B4 RID: 2228
	private Material SCMaterial;

	// Token: 0x040008B5 RID: 2229
	[Range(0f, 1f)]
	public float Value = 0.6f;

	// Token: 0x040008B6 RID: 2230
	[Range(0f, 1f)]
	public float Value2 = 0.6f;

	// Token: 0x040008B7 RID: 2231
	[Range(0f, 10f)]
	private float Value3 = 1f;

	// Token: 0x040008B8 RID: 2232
	[Range(0f, 10f)]
	private float Value4 = 1f;
}
