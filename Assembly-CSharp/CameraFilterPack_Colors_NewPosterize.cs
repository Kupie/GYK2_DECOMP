using System;
using UnityEngine;

// Token: 0x0200005B RID: 91
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/NewPosterize")]
public class CameraFilterPack_Colors_NewPosterize : MonoBehaviour
{
	// Token: 0x17000057 RID: 87
	// (get) Token: 0x06000249 RID: 585 RVA: 0x0000DCFB File Offset: 0x0000BEFB
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

	// Token: 0x0600024A RID: 586 RVA: 0x0000DD2F File Offset: 0x0000BF2F
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Colors_NewPosterize");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600024B RID: 587 RVA: 0x0000DD50 File Offset: 0x0000BF50
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
			this.material.SetFloat("_Value", this.Gamma);
			this.material.SetFloat("_Value2", this.Colors);
			this.material.SetFloat("_Value3", this.Green_Mod);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600024C RID: 588 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600024D RID: 589 RVA: 0x0000DE48 File Offset: 0x0000C048
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000391 RID: 913
	public Shader SCShader;

	// Token: 0x04000392 RID: 914
	private float TimeX = 1f;

	// Token: 0x04000393 RID: 915
	private Vector4 ScreenResolution;

	// Token: 0x04000394 RID: 916
	private Material SCMaterial;

	// Token: 0x04000395 RID: 917
	[Range(0f, 2f)]
	public float Gamma = 1f;

	// Token: 0x04000396 RID: 918
	[Range(0f, 16f)]
	public float Colors = 11f;

	// Token: 0x04000397 RID: 919
	[Range(-1f, 1f)]
	public float Green_Mod = 1f;

	// Token: 0x04000398 RID: 920
	[Range(0f, 10f)]
	private float Value4 = 1f;
}
