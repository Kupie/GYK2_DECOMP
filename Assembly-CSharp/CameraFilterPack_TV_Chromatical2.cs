using System;
using UnityEngine;

// Token: 0x020000F1 RID: 241
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/Chromatical2")]
public class CameraFilterPack_TV_Chromatical2 : MonoBehaviour
{
	// Token: 0x170000ED RID: 237
	// (get) Token: 0x060005DF RID: 1503 RVA: 0x0001CB11 File Offset: 0x0001AD11
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

	// Token: 0x060005E0 RID: 1504 RVA: 0x0001CB45 File Offset: 0x0001AD45
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_Chromatical2");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x0001CB68 File Offset: 0x0001AD68
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
			this.material.SetFloat("_Value", this.Aberration);
			this.material.SetFloat("_Value2", this.Value2);
			this.material.SetFloat("_Value3", this.Value3);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x0001CC60 File Offset: 0x0001AE60
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040007A8 RID: 1960
	public Shader SCShader;

	// Token: 0x040007A9 RID: 1961
	private float TimeX = 1f;

	// Token: 0x040007AA RID: 1962
	private Vector4 ScreenResolution;

	// Token: 0x040007AB RID: 1963
	private Material SCMaterial;

	// Token: 0x040007AC RID: 1964
	[Range(0f, 10f)]
	public float Aberration = 2f;

	// Token: 0x040007AD RID: 1965
	[Range(0f, 10f)]
	private float Value2 = 1f;

	// Token: 0x040007AE RID: 1966
	[Range(0f, 10f)]
	private float Value3 = 1f;

	// Token: 0x040007AF RID: 1967
	[Range(0f, 10f)]
	private float Value4 = 1f;
}
