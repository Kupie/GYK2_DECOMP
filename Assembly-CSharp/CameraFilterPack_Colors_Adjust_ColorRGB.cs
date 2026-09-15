using System;
using UnityEngine;

// Token: 0x02000052 RID: 82
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/ColorsAdjust/ColorRGB")]
public class CameraFilterPack_Colors_Adjust_ColorRGB : MonoBehaviour
{
	// Token: 0x1700004F RID: 79
	// (get) Token: 0x06000217 RID: 535 RVA: 0x0000CBF1 File Offset: 0x0000ADF1
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

	// Token: 0x06000218 RID: 536 RVA: 0x0000CC25 File Offset: 0x0000AE25
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Colors_Adjust_ColorRGB");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000219 RID: 537 RVA: 0x0000CC48 File Offset: 0x0000AE48
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
			this.material.SetFloat("_Value", this.Red);
			this.material.SetFloat("_Value2", this.Green);
			this.material.SetFloat("_Value3", this.Blue);
			this.material.SetFloat("_Value4", this.Brightness);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600021A RID: 538 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600021B RID: 539 RVA: 0x0000CD40 File Offset: 0x0000AF40
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000336 RID: 822
	public Shader SCShader;

	// Token: 0x04000337 RID: 823
	private float TimeX = 1f;

	// Token: 0x04000338 RID: 824
	private Vector4 ScreenResolution;

	// Token: 0x04000339 RID: 825
	private Material SCMaterial;

	// Token: 0x0400033A RID: 826
	[Range(-2f, 2f)]
	public float Red;

	// Token: 0x0400033B RID: 827
	[Range(-2f, 2f)]
	public float Green;

	// Token: 0x0400033C RID: 828
	[Range(-2f, 2f)]
	public float Blue;

	// Token: 0x0400033D RID: 829
	[Range(-1f, 1f)]
	public float Brightness;
}
