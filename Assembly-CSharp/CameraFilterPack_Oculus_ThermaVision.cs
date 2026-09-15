using System;
using UnityEngine;

// Token: 0x020000DC RID: 220
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/ThermaVision")]
public class CameraFilterPack_Oculus_ThermaVision : MonoBehaviour
{
	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x06000560 RID: 1376 RVA: 0x0001A911 File Offset: 0x00018B11
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

	// Token: 0x06000561 RID: 1377 RVA: 0x0001A945 File Offset: 0x00018B45
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Oculus_ThermaVision");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x0001A968 File Offset: 0x00018B68
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
			this.material.SetFloat("_Value", this.Therma_Variation);
			this.material.SetFloat("_Value2", this.Contrast);
			this.material.SetFloat("_Value3", this.Burn);
			this.material.SetFloat("_Value4", this.SceneCut);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x0001AA60 File Offset: 0x00018C60
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000711 RID: 1809
	public Shader SCShader;

	// Token: 0x04000712 RID: 1810
	private float TimeX = 1f;

	// Token: 0x04000713 RID: 1811
	private Vector4 ScreenResolution;

	// Token: 0x04000714 RID: 1812
	private Material SCMaterial;

	// Token: 0x04000715 RID: 1813
	[Range(0f, 1f)]
	public float Therma_Variation = 0.5f;

	// Token: 0x04000716 RID: 1814
	[Range(0f, 8f)]
	private float Contrast = 3f;

	// Token: 0x04000717 RID: 1815
	[Range(0f, 4f)]
	private float Burn;

	// Token: 0x04000718 RID: 1816
	[Range(0f, 16f)]
	private float SceneCut = 1f;
}
