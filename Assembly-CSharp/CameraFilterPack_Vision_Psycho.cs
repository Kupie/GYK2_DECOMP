using System;
using UnityEngine;

// Token: 0x02000113 RID: 275
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/Psycho")]
public class CameraFilterPack_Vision_Psycho : MonoBehaviour
{
	// Token: 0x1700010F RID: 271
	// (get) Token: 0x060006AB RID: 1707 RVA: 0x0001FCA5 File Offset: 0x0001DEA5
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

	// Token: 0x060006AC RID: 1708 RVA: 0x0001FCD9 File Offset: 0x0001DED9
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Vision_Psycho");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x0001FCFC File Offset: 0x0001DEFC
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
			this.material.SetFloat("_Value", this.HoleSize);
			this.material.SetFloat("_Value2", this.HoleSmooth);
			this.material.SetFloat("_Value3", this.Color1);
			this.material.SetFloat("_Value4", this.Color2);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x0001FDF4 File Offset: 0x0001DFF4
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000888 RID: 2184
	public Shader SCShader;

	// Token: 0x04000889 RID: 2185
	private float TimeX = 1f;

	// Token: 0x0400088A RID: 2186
	private Vector4 ScreenResolution;

	// Token: 0x0400088B RID: 2187
	private Material SCMaterial;

	// Token: 0x0400088C RID: 2188
	[Range(0.01f, 1f)]
	public float HoleSize = 0.6f;

	// Token: 0x0400088D RID: 2189
	[Range(-1f, 1f)]
	public float HoleSmooth = 0.3f;

	// Token: 0x0400088E RID: 2190
	[Range(-2f, 2f)]
	public float Color1 = 0.2f;

	// Token: 0x0400088F RID: 2191
	[Range(-2f, 2f)]
	public float Color2 = 0.9f;
}
