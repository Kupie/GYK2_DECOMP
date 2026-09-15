using System;
using UnityEngine;

// Token: 0x02000061 RID: 97
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/Contrast")]
public class CameraFilterPack_Color_Contrast : MonoBehaviour
{
	// Token: 0x1700005D RID: 93
	// (get) Token: 0x0600026D RID: 621 RVA: 0x0000E608 File Offset: 0x0000C808
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

	// Token: 0x0600026E RID: 622 RVA: 0x0000E63C File Offset: 0x0000C83C
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Color_Contrast");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600026F RID: 623 RVA: 0x0000E660 File Offset: 0x0000C860
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.TimeX += Time.deltaTime;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			this.material.SetFloat("_Contrast", this.Contrast);
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000270 RID: 624 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000271 RID: 625 RVA: 0x0000E716 File Offset: 0x0000C916
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040003BC RID: 956
	public Shader SCShader;

	// Token: 0x040003BD RID: 957
	private float TimeX = 1f;

	// Token: 0x040003BE RID: 958
	private Vector4 ScreenResolution;

	// Token: 0x040003BF RID: 959
	private Material SCMaterial;

	// Token: 0x040003C0 RID: 960
	[Range(0f, 10f)]
	public float Contrast = 4.5f;
}
