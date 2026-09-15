using System;
using UnityEngine;

// Token: 0x0200010D RID: 269
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/Blood")]
public class CameraFilterPack_Vision_Blood : MonoBehaviour
{
	// Token: 0x17000109 RID: 265
	// (get) Token: 0x06000687 RID: 1671 RVA: 0x0001F2B0 File Offset: 0x0001D4B0
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

	// Token: 0x06000688 RID: 1672 RVA: 0x0001F2E4 File Offset: 0x0001D4E4
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Vision_Blood");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000689 RID: 1673 RVA: 0x0001F308 File Offset: 0x0001D508
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

	// Token: 0x0600068A RID: 1674 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600068B RID: 1675 RVA: 0x0001F400 File Offset: 0x0001D600
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000858 RID: 2136
	public Shader SCShader;

	// Token: 0x04000859 RID: 2137
	private float TimeX = 1f;

	// Token: 0x0400085A RID: 2138
	private Vector4 ScreenResolution;

	// Token: 0x0400085B RID: 2139
	private Material SCMaterial;

	// Token: 0x0400085C RID: 2140
	[Range(0.01f, 1f)]
	public float HoleSize = 0.6f;

	// Token: 0x0400085D RID: 2141
	[Range(-1f, 1f)]
	public float HoleSmooth = 0.3f;

	// Token: 0x0400085E RID: 2142
	[Range(-2f, 2f)]
	public float Color1 = 0.2f;

	// Token: 0x0400085F RID: 2143
	[Range(-2f, 2f)]
	public float Color2 = 0.9f;
}
