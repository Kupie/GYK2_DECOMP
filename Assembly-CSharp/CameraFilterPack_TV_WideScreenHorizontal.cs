using System;
using UnityEngine;

// Token: 0x02000107 RID: 263
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/WideScreenHorizontal")]
public class CameraFilterPack_TV_WideScreenHorizontal : MonoBehaviour
{
	// Token: 0x17000103 RID: 259
	// (get) Token: 0x06000663 RID: 1635 RVA: 0x0001E8AD File Offset: 0x0001CAAD
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

	// Token: 0x06000664 RID: 1636 RVA: 0x0001E8E1 File Offset: 0x0001CAE1
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_WideScreenHorizontal");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x0001E904 File Offset: 0x0001CB04
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
			this.material.SetFloat("_Value", this.Size);
			this.material.SetFloat("_Value2", this.Smooth);
			this.material.SetFloat("_Value3", this.StretchX);
			this.material.SetFloat("_Value4", this.StretchY);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x0001E9FC File Offset: 0x0001CBFC
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000829 RID: 2089
	public Shader SCShader;

	// Token: 0x0400082A RID: 2090
	private float TimeX = 1f;

	// Token: 0x0400082B RID: 2091
	private Vector4 ScreenResolution;

	// Token: 0x0400082C RID: 2092
	private Material SCMaterial;

	// Token: 0x0400082D RID: 2093
	[Range(0f, 0.8f)]
	public float Size = 0.55f;

	// Token: 0x0400082E RID: 2094
	[Range(0.001f, 0.4f)]
	public float Smooth = 0.01f;

	// Token: 0x0400082F RID: 2095
	[Range(0f, 10f)]
	private float StretchX = 1f;

	// Token: 0x04000830 RID: 2096
	[Range(0f, 10f)]
	private float StretchY = 1f;
}
