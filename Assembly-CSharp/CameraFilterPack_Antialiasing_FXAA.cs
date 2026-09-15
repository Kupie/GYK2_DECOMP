using System;
using UnityEngine;

// Token: 0x0200001A RID: 26
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Antialiasing/FXAA")]
public class CameraFilterPack_Antialiasing_FXAA : MonoBehaviour
{
	// Token: 0x17000018 RID: 24
	// (get) Token: 0x0600008E RID: 142 RVA: 0x000058B2 File Offset: 0x00003AB2
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

	// Token: 0x0600008F RID: 143 RVA: 0x000058E6 File Offset: 0x00003AE6
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Antialiasing_FXAA");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00005908 File Offset: 0x00003B08
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
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000091 RID: 145 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000092 RID: 146 RVA: 0x0000599E File Offset: 0x00003B9E
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400011E RID: 286
	public Shader SCShader;

	// Token: 0x0400011F RID: 287
	private float TimeX = 1f;

	// Token: 0x04000120 RID: 288
	private Vector4 ScreenResolution;

	// Token: 0x04000121 RID: 289
	private Material SCMaterial;
}
