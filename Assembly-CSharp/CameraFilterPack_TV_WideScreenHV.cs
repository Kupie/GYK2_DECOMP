using System;
using UnityEngine;

// Token: 0x02000108 RID: 264
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/WideScreenHV")]
public class CameraFilterPack_TV_WideScreenHV : MonoBehaviour
{
	// Token: 0x17000104 RID: 260
	// (get) Token: 0x06000669 RID: 1641 RVA: 0x0001EA55 File Offset: 0x0001CC55
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

	// Token: 0x0600066A RID: 1642 RVA: 0x0001EA89 File Offset: 0x0001CC89
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_WideScreenHV");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x0001EAAC File Offset: 0x0001CCAC
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

	// Token: 0x0600066C RID: 1644 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x0001EBA4 File Offset: 0x0001CDA4
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000831 RID: 2097
	public Shader SCShader;

	// Token: 0x04000832 RID: 2098
	private float TimeX = 1f;

	// Token: 0x04000833 RID: 2099
	private Vector4 ScreenResolution;

	// Token: 0x04000834 RID: 2100
	private Material SCMaterial;

	// Token: 0x04000835 RID: 2101
	[Range(0f, 0.8f)]
	public float Size = 0.55f;

	// Token: 0x04000836 RID: 2102
	[Range(0.001f, 0.4f)]
	public float Smooth = 0.01f;

	// Token: 0x04000837 RID: 2103
	[Range(0f, 10f)]
	private float StretchX = 1f;

	// Token: 0x04000838 RID: 2104
	[Range(0f, 10f)]
	private float StretchY = 1f;
}
