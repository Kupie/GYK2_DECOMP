using System;
using UnityEngine;

// Token: 0x02000106 RID: 262
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/WideScreenCircle")]
public class CameraFilterPack_TV_WideScreenCircle : MonoBehaviour
{
	// Token: 0x17000102 RID: 258
	// (get) Token: 0x0600065D RID: 1629 RVA: 0x0001E706 File Offset: 0x0001C906
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

	// Token: 0x0600065E RID: 1630 RVA: 0x0001E73A File Offset: 0x0001C93A
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_WideScreenCircle");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x0001E75C File Offset: 0x0001C95C
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

	// Token: 0x06000660 RID: 1632 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x0001E854 File Offset: 0x0001CA54
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000821 RID: 2081
	public Shader SCShader;

	// Token: 0x04000822 RID: 2082
	private float TimeX = 1f;

	// Token: 0x04000823 RID: 2083
	private Vector4 ScreenResolution;

	// Token: 0x04000824 RID: 2084
	private Material SCMaterial;

	// Token: 0x04000825 RID: 2085
	[Range(0f, 0.8f)]
	public float Size = 0.55f;

	// Token: 0x04000826 RID: 2086
	[Range(0.01f, 0.4f)]
	public float Smooth = 0.01f;

	// Token: 0x04000827 RID: 2087
	[Range(0f, 10f)]
	private float StretchX = 1f;

	// Token: 0x04000828 RID: 2088
	[Range(0f, 10f)]
	private float StretchY = 1f;
}
