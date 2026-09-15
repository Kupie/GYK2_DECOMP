using System;
using UnityEngine;

// Token: 0x020000E3 RID: 227
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Pixel/Pixelisation")]
public class CameraFilterPack_Pixel_Pixelisation : MonoBehaviour
{
	// Token: 0x170000DF RID: 223
	// (get) Token: 0x0600058A RID: 1418 RVA: 0x0001B3A5 File Offset: 0x000195A5
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

	// Token: 0x0600058B RID: 1419 RVA: 0x0001B3D9 File Offset: 0x000195D9
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Pixel_Pixelisation");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x0001B3FC File Offset: 0x000195FC
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.material.SetFloat("_Val", this._Pixelisation);
			this.material.SetFloat("_Val2", this._SizeX);
			this.material.SetFloat("_Val3", this._SizeY);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x0001B46E File Offset: 0x0001966E
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000743 RID: 1859
	public Shader SCShader;

	// Token: 0x04000744 RID: 1860
	[Range(0.6f, 120f)]
	public float _Pixelisation = 8f;

	// Token: 0x04000745 RID: 1861
	[Range(0.6f, 120f)]
	public float _SizeX = 1f;

	// Token: 0x04000746 RID: 1862
	[Range(0.6f, 120f)]
	public float _SizeY = 1f;

	// Token: 0x04000747 RID: 1863
	private Material SCMaterial;
}
