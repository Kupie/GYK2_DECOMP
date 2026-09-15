using System;
using UnityEngine;

// Token: 0x0200005F RID: 95
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/Chromatic_Aberration")]
public class CameraFilterPack_Color_Chromatic_Aberration : MonoBehaviour
{
	// Token: 0x1700005B RID: 91
	// (get) Token: 0x06000261 RID: 609 RVA: 0x0000E33C File Offset: 0x0000C53C
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

	// Token: 0x06000262 RID: 610 RVA: 0x0000E370 File Offset: 0x0000C570
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Color_Chromatic_Aberration");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000263 RID: 611 RVA: 0x0000E394 File Offset: 0x0000C594
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
			this.material.SetFloat("_Distortion", this.Offset);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000264 RID: 612 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000265 RID: 613 RVA: 0x0000E44A File Offset: 0x0000C64A
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040003B0 RID: 944
	public Shader SCShader;

	// Token: 0x040003B1 RID: 945
	private float TimeX = 1f;

	// Token: 0x040003B2 RID: 946
	private Vector4 ScreenResolution;

	// Token: 0x040003B3 RID: 947
	private Material SCMaterial;

	// Token: 0x040003B4 RID: 948
	[Range(-0.02f, 0.02f)]
	public float Offset = 0.02f;
}
