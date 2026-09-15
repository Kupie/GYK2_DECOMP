using System;
using UnityEngine;

// Token: 0x02000110 RID: 272
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/Drost")]
public class CameraFilterPack_Vision_Drost : MonoBehaviour
{
	// Token: 0x1700010C RID: 268
	// (get) Token: 0x06000699 RID: 1689 RVA: 0x0001F7A9 File Offset: 0x0001D9A9
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

	// Token: 0x0600069A RID: 1690 RVA: 0x0001F7DD File Offset: 0x0001D9DD
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Vision_Drost");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x0001F800 File Offset: 0x0001DA00
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
			this.material.SetFloat("_Value", this.Intensity);
			this.material.SetFloat("_Value2", this.Speed);
			this.material.SetFloat("_Value3", this.Value3);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x0001F8F8 File Offset: 0x0001DAF8
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000870 RID: 2160
	public Shader SCShader;

	// Token: 0x04000871 RID: 2161
	private float TimeX = 1f;

	// Token: 0x04000872 RID: 2162
	private Vector4 ScreenResolution;

	// Token: 0x04000873 RID: 2163
	private Material SCMaterial;

	// Token: 0x04000874 RID: 2164
	[Range(0f, 0.4f)]
	public float Intensity = 0.4f;

	// Token: 0x04000875 RID: 2165
	[Range(0f, 10f)]
	public float Speed = 1f;

	// Token: 0x04000876 RID: 2166
	[Range(0f, 10f)]
	private float Value3 = 1f;

	// Token: 0x04000877 RID: 2167
	[Range(0f, 10f)]
	private float Value4 = 1f;
}
