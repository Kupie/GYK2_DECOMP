using System;
using UnityEngine;

// Token: 0x02000112 RID: 274
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/Plasma")]
public class CameraFilterPack_Vision_Plasma : MonoBehaviour
{
	// Token: 0x1700010E RID: 270
	// (get) Token: 0x060006A5 RID: 1701 RVA: 0x0001FAFF File Offset: 0x0001DCFF
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

	// Token: 0x060006A6 RID: 1702 RVA: 0x0001FB33 File Offset: 0x0001DD33
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Vision_Plasma");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x0001FB54 File Offset: 0x0001DD54
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
			this.material.SetFloat("_Value", this.Value);
			this.material.SetFloat("_Value2", this.Value2);
			this.material.SetFloat("_Value3", this.Intensity);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x0001FC4C File Offset: 0x0001DE4C
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000880 RID: 2176
	public Shader SCShader;

	// Token: 0x04000881 RID: 2177
	private float TimeX = 1f;

	// Token: 0x04000882 RID: 2178
	private Vector4 ScreenResolution;

	// Token: 0x04000883 RID: 2179
	private Material SCMaterial;

	// Token: 0x04000884 RID: 2180
	[Range(-2f, 2f)]
	public float Value = 0.6f;

	// Token: 0x04000885 RID: 2181
	[Range(-2f, 2f)]
	public float Value2 = 0.2f;

	// Token: 0x04000886 RID: 2182
	[Range(0f, 60f)]
	public float Intensity = 15f;

	// Token: 0x04000887 RID: 2183
	[Range(0f, 10f)]
	private float Value4 = 1f;
}
