using System;
using UnityEngine;

// Token: 0x02000104 RID: 260
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/Vignetting")]
public class CameraFilterPack_TV_Vignetting : MonoBehaviour
{
	// Token: 0x17000100 RID: 256
	// (get) Token: 0x06000651 RID: 1617 RVA: 0x0001E49E File Offset: 0x0001C69E
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

	// Token: 0x06000652 RID: 1618 RVA: 0x0001E4D2 File Offset: 0x0001C6D2
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_Vignetting");
		this.Vignette = Resources.Load("CameraFilterPack_TV_Vignetting1") as Texture2D;
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x0001E508 File Offset: 0x0001C708
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.material.SetTexture("Vignette", this.Vignette);
			this.material.SetFloat("_Vignetting", this.Vignetting);
			this.material.SetFloat("_Vignetting2", this.VignettingFull);
			this.material.SetColor("_VignettingColor", this.VignettingColor);
			this.material.SetFloat("_VignettingDirt", this.VignettingDirt);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000655 RID: 1621 RVA: 0x0001E5A6 File Offset: 0x0001C7A6
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000816 RID: 2070
	public Shader SCShader;

	// Token: 0x04000817 RID: 2071
	private Material SCMaterial;

	// Token: 0x04000818 RID: 2072
	private Texture2D Vignette;

	// Token: 0x04000819 RID: 2073
	[Range(0f, 1f)]
	public float Vignetting = 1f;

	// Token: 0x0400081A RID: 2074
	[Range(0f, 1f)]
	public float VignettingFull;

	// Token: 0x0400081B RID: 2075
	[Range(0f, 1f)]
	public float VignettingDirt;

	// Token: 0x0400081C RID: 2076
	public Color VignettingColor = new Color(0f, 0f, 0f, 1f);
}
