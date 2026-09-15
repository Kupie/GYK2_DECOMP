using System;
using UnityEngine;

// Token: 0x020000E2 RID: 226
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Pixelisation/Pixelisation_Sweater")]
public class CameraFilterPack_Pixelisation_Sweater : MonoBehaviour
{
	// Token: 0x170000DE RID: 222
	// (get) Token: 0x06000584 RID: 1412 RVA: 0x0001B222 File Offset: 0x00019422
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

	// Token: 0x06000585 RID: 1413 RVA: 0x0001B256 File Offset: 0x00019456
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_Sweater") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/Pixelisation_Sweater");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x0001B28C File Offset: 0x0001948C
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
			this.material.SetFloat("_Fade", this.Fade);
			this.material.SetFloat("_SweaterSize", this.SweaterSize);
			this.material.SetFloat("_Intensity", this._Intensity);
			this.material.SetTexture("Texture2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x0001B357 File Offset: 0x00019557
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400073B RID: 1851
	public Shader SCShader;

	// Token: 0x0400073C RID: 1852
	private float TimeX = 1f;

	// Token: 0x0400073D RID: 1853
	private Vector4 ScreenResolution;

	// Token: 0x0400073E RID: 1854
	private Material SCMaterial;

	// Token: 0x0400073F RID: 1855
	[Range(16f, 128f)]
	public float SweaterSize = 64f;

	// Token: 0x04000740 RID: 1856
	[Range(0f, 2f)]
	public float _Intensity = 1.4f;

	// Token: 0x04000741 RID: 1857
	[Range(0f, 1f)]
	public float Fade = 1f;

	// Token: 0x04000742 RID: 1858
	private Texture2D Texture2;
}
