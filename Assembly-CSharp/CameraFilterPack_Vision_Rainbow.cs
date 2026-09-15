using System;
using UnityEngine;

// Token: 0x02000114 RID: 276
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/Rainbow")]
public class CameraFilterPack_Vision_Rainbow : MonoBehaviour
{
	// Token: 0x17000110 RID: 272
	// (get) Token: 0x060006B1 RID: 1713 RVA: 0x0001FE4D File Offset: 0x0001E04D
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

	// Token: 0x060006B2 RID: 1714 RVA: 0x0001FE81 File Offset: 0x0001E081
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Vision_Rainbow");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060006B3 RID: 1715 RVA: 0x0001FEA4 File Offset: 0x0001E0A4
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
			this.material.SetFloat("_Value", this.Speed);
			this.material.SetFloat("_Value2", this.PosX);
			this.material.SetFloat("_Value3", this.PosY);
			this.material.SetFloat("_Value4", this.Colors);
			this.material.SetFloat("_Value5", this.Vision);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x0001FFB2 File Offset: 0x0001E1B2
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000890 RID: 2192
	public Shader SCShader;

	// Token: 0x04000891 RID: 2193
	private float TimeX = 1f;

	// Token: 0x04000892 RID: 2194
	private Vector4 ScreenResolution;

	// Token: 0x04000893 RID: 2195
	private Material SCMaterial;

	// Token: 0x04000894 RID: 2196
	[Range(0f, 10f)]
	public float Speed = 1f;

	// Token: 0x04000895 RID: 2197
	[Range(0f, 1f)]
	public float PosX = 0.5f;

	// Token: 0x04000896 RID: 2198
	[Range(0f, 1f)]
	public float PosY = 0.5f;

	// Token: 0x04000897 RID: 2199
	[Range(0f, 5f)]
	public float Colors = 0.5f;

	// Token: 0x04000898 RID: 2200
	[Range(0f, 1f)]
	public float Vision = 0.5f;
}
