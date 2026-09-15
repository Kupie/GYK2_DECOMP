using System;
using UnityEngine;

// Token: 0x0200009F RID: 159
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Film/ColorPerfection")]
public class CameraFilterPack_Film_ColorPerfection : MonoBehaviour
{
	// Token: 0x1700009C RID: 156
	// (get) Token: 0x060003E2 RID: 994 RVA: 0x00013F5E File Offset: 0x0001215E
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

	// Token: 0x060003E3 RID: 995 RVA: 0x00013F92 File Offset: 0x00012192
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Film_ColorPerfection");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x00013FB4 File Offset: 0x000121B4
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
			this.material.SetFloat("_Value2", this.Value2);
			this.material.SetFloat("_Value3", this.Value3);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060003E5 RID: 997 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x000140AC File Offset: 0x000122AC
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000551 RID: 1361
	public Shader SCShader;

	// Token: 0x04000552 RID: 1362
	private float TimeX = 1f;

	// Token: 0x04000553 RID: 1363
	private Vector4 ScreenResolution;

	// Token: 0x04000554 RID: 1364
	private Material SCMaterial;

	// Token: 0x04000555 RID: 1365
	[Range(0f, 4f)]
	public float Gamma = 0.55f;

	// Token: 0x04000556 RID: 1366
	[Range(0f, 10f)]
	private float Value2 = 1f;

	// Token: 0x04000557 RID: 1367
	[Range(0f, 10f)]
	private float Value3 = 1f;

	// Token: 0x04000558 RID: 1368
	[Range(0f, 10f)]
	private float Value4 = 1f;
}
