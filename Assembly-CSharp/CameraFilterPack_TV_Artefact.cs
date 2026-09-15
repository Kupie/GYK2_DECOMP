using System;
using UnityEngine;

// Token: 0x020000ED RID: 237
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/Artefact")]
public class CameraFilterPack_TV_Artefact : MonoBehaviour
{
	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x060005C7 RID: 1479 RVA: 0x0001C253 File Offset: 0x0001A453
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

	// Token: 0x060005C8 RID: 1480 RVA: 0x0001C287 File Offset: 0x0001A487
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_Artefact");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x0001C2A8 File Offset: 0x0001A4A8
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
			this.material.SetFloat("_Colorisation", this.Colorisation);
			this.material.SetFloat("_Parasite", this.Parasite);
			this.material.SetFloat("_Noise", this.Noise);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x0001C38A File Offset: 0x0001A58A
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000784 RID: 1924
	public Shader SCShader;

	// Token: 0x04000785 RID: 1925
	private Vector4 ScreenResolution;

	// Token: 0x04000786 RID: 1926
	private float TimeX = 1f;

	// Token: 0x04000787 RID: 1927
	[Range(-10f, 10f)]
	public float Colorisation = 1f;

	// Token: 0x04000788 RID: 1928
	[Range(-10f, 10f)]
	public float Parasite = 1f;

	// Token: 0x04000789 RID: 1929
	[Range(-10f, 10f)]
	public float Noise = 1f;

	// Token: 0x0400078A RID: 1930
	private Material SCMaterial;
}
