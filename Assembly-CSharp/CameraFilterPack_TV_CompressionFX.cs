using System;
using UnityEngine;

// Token: 0x020000F2 RID: 242
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Glitch/Compression FX")]
public class CameraFilterPack_TV_CompressionFX : MonoBehaviour
{
	// Token: 0x170000EE RID: 238
	// (get) Token: 0x060005E5 RID: 1509 RVA: 0x0001CCB9 File Offset: 0x0001AEB9
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

	// Token: 0x060005E6 RID: 1510 RVA: 0x0001CCED File Offset: 0x0001AEED
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_CompressionFX");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x0001CD10 File Offset: 0x0001AF10
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
			this.material.SetFloat("_Parasite", this.Parasite);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x0001CDC6 File Offset: 0x0001AFC6
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040007B0 RID: 1968
	public Shader SCShader;

	// Token: 0x040007B1 RID: 1969
	private Vector4 ScreenResolution;

	// Token: 0x040007B2 RID: 1970
	private float TimeX = 1f;

	// Token: 0x040007B3 RID: 1971
	[Range(-10f, 10f)]
	public float Parasite = 1f;

	// Token: 0x040007B4 RID: 1972
	private Material SCMaterial;
}
