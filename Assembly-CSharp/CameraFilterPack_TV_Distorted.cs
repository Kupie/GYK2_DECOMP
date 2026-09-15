using System;
using UnityEngine;

// Token: 0x020000F3 RID: 243
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/Distorted")]
public class CameraFilterPack_TV_Distorted : MonoBehaviour
{
	// Token: 0x170000EF RID: 239
	// (get) Token: 0x060005EB RID: 1515 RVA: 0x0001CDFE File Offset: 0x0001AFFE
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

	// Token: 0x060005EC RID: 1516 RVA: 0x0001CE32 File Offset: 0x0001B032
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_Distorted");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x0001CE54 File Offset: 0x0001B054
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
			this.material.SetFloat("_Distortion", this.Distortion);
			this.material.SetFloat("_RGB", this.RGB);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x0001CEF0 File Offset: 0x0001B0F0
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040007B5 RID: 1973
	public Shader SCShader;

	// Token: 0x040007B6 RID: 1974
	private float TimeX = 1f;

	// Token: 0x040007B7 RID: 1975
	[Range(0f, 10f)]
	public float Distortion = 1f;

	// Token: 0x040007B8 RID: 1976
	[Range(-0.01f, 0.01f)]
	public float RGB = 0.002f;

	// Token: 0x040007B9 RID: 1977
	private Material SCMaterial;
}
