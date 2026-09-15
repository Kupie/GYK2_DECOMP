using System;
using UnityEngine;

// Token: 0x020000FA RID: 250
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Old Film/Old_Movie_2")]
public class CameraFilterPack_TV_Old_Movie_2 : MonoBehaviour
{
	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x06000615 RID: 1557 RVA: 0x0001D706 File Offset: 0x0001B906
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

	// Token: 0x06000616 RID: 1558 RVA: 0x0001D73A File Offset: 0x0001B93A
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_Old_Movie_2");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x0001D75C File Offset: 0x0001B95C
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
			this.material.SetFloat("_Value", this.FramePerSecond);
			this.material.SetFloat("_Value2", this.Contrast);
			this.material.SetFloat("_Value3", this.Burn);
			this.material.SetFloat("_Value4", this.SceneCut);
			this.material.SetFloat("_Fade", this.Fade);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x0001D86A File Offset: 0x0001BA6A
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040007DB RID: 2011
	public Shader SCShader;

	// Token: 0x040007DC RID: 2012
	private float TimeX = 1f;

	// Token: 0x040007DD RID: 2013
	private Vector4 ScreenResolution;

	// Token: 0x040007DE RID: 2014
	private Material SCMaterial;

	// Token: 0x040007DF RID: 2015
	[Range(1f, 60f)]
	public float FramePerSecond = 15f;

	// Token: 0x040007E0 RID: 2016
	[Range(0f, 5f)]
	public float Contrast = 1f;

	// Token: 0x040007E1 RID: 2017
	[Range(0f, 4f)]
	public float Burn;

	// Token: 0x040007E2 RID: 2018
	[Range(0f, 16f)]
	public float SceneCut = 1f;

	// Token: 0x040007E3 RID: 2019
	[Range(0f, 1f)]
	public float Fade = 1f;
}
