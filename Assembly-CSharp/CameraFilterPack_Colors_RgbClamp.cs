using System;
using UnityEngine;

// Token: 0x0200005C RID: 92
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/RgbClamp")]
public class CameraFilterPack_Colors_RgbClamp : MonoBehaviour
{
	// Token: 0x17000058 RID: 88
	// (get) Token: 0x0600024F RID: 591 RVA: 0x0000DEA1 File Offset: 0x0000C0A1
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

	// Token: 0x06000250 RID: 592 RVA: 0x0000DED5 File Offset: 0x0000C0D5
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Colors_RgbClamp");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000251 RID: 593 RVA: 0x0000DEF8 File Offset: 0x0000C0F8
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
			this.material.SetFloat("_Value", this.Red_Start);
			this.material.SetFloat("_Value2", this.Red_End);
			this.material.SetFloat("_Value3", this.Green_Start);
			this.material.SetFloat("_Value4", this.Green_End);
			this.material.SetFloat("_Value5", this.Blue_Start);
			this.material.SetFloat("_Value6", this.Blue_End);
			this.material.SetFloat("_Value7", this.RGB_Start);
			this.material.SetFloat("_Value8", this.RGB_End);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000252 RID: 594 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000253 RID: 595 RVA: 0x0000E048 File Offset: 0x0000C248
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000399 RID: 921
	public Shader SCShader;

	// Token: 0x0400039A RID: 922
	private float TimeX = 1f;

	// Token: 0x0400039B RID: 923
	private Vector4 ScreenResolution;

	// Token: 0x0400039C RID: 924
	private Material SCMaterial;

	// Token: 0x0400039D RID: 925
	[Range(0f, 1f)]
	public float Red_Start;

	// Token: 0x0400039E RID: 926
	[Range(0f, 1f)]
	public float Red_End = 1f;

	// Token: 0x0400039F RID: 927
	[Range(0f, 1f)]
	public float Green_Start;

	// Token: 0x040003A0 RID: 928
	[Range(0f, 1f)]
	public float Green_End = 1f;

	// Token: 0x040003A1 RID: 929
	[Range(0f, 1f)]
	public float Blue_Start;

	// Token: 0x040003A2 RID: 930
	[Range(0f, 1f)]
	public float Blue_End = 1f;

	// Token: 0x040003A3 RID: 931
	[Range(0f, 1f)]
	public float RGB_Start;

	// Token: 0x040003A4 RID: 932
	[Range(0f, 1f)]
	public float RGB_End = 1f;
}
