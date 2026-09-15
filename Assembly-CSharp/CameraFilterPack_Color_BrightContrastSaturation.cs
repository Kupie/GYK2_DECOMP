using System;
using UnityEngine;

// Token: 0x0200005E RID: 94
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/BrightContrastSaturation")]
public class CameraFilterPack_Color_BrightContrastSaturation : MonoBehaviour
{
	// Token: 0x1700005A RID: 90
	// (get) Token: 0x0600025B RID: 603 RVA: 0x0000E1B6 File Offset: 0x0000C3B6
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

	// Token: 0x0600025C RID: 604 RVA: 0x0000E1EA File Offset: 0x0000C3EA
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Color_BrightContrastSaturation");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600025D RID: 605 RVA: 0x0000E20C File Offset: 0x0000C40C
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.TimeX += Time.deltaTime;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			this.material.SetFloat("_Brightness", this.Brightness);
			this.material.SetFloat("_Saturation", this.Saturation);
			this.material.SetFloat("_Contrast", this.Contrast);
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600025E RID: 606 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600025F RID: 607 RVA: 0x0000E2EE File Offset: 0x0000C4EE
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040003A9 RID: 937
	public Shader SCShader;

	// Token: 0x040003AA RID: 938
	private float TimeX = 1f;

	// Token: 0x040003AB RID: 939
	private Vector4 ScreenResolution;

	// Token: 0x040003AC RID: 940
	private Material SCMaterial;

	// Token: 0x040003AD RID: 941
	[Range(0f, 10f)]
	public float Brightness = 2f;

	// Token: 0x040003AE RID: 942
	[Range(0f, 10f)]
	public float Saturation = 1.5f;

	// Token: 0x040003AF RID: 943
	[Range(0f, 10f)]
	public float Contrast = 1.5f;
}
