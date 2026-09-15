using System;
using UnityEngine;

// Token: 0x020000D8 RID: 216
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Night Vision/Night Vision 1")]
public class CameraFilterPack_Oculus_NightVision1 : MonoBehaviour
{
	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x06000544 RID: 1348 RVA: 0x0001A011 File Offset: 0x00018211
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

	// Token: 0x06000545 RID: 1349 RVA: 0x0001A045 File Offset: 0x00018245
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Oculus_NightVision1");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x0001A068 File Offset: 0x00018268
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
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			this.material.SetFloat("_Vignette", this.Vignette);
			this.material.SetFloat("_Linecount", this.Linecount);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x0001A14A File Offset: 0x0001834A
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040006F4 RID: 1780
	public Shader SCShader;

	// Token: 0x040006F5 RID: 1781
	private float TimeX = 1f;

	// Token: 0x040006F6 RID: 1782
	private float Distortion = 1f;

	// Token: 0x040006F7 RID: 1783
	private Material SCMaterial;

	// Token: 0x040006F8 RID: 1784
	private Vector4 ScreenResolution;

	// Token: 0x040006F9 RID: 1785
	[Range(0f, 100f)]
	public float Vignette = 1.3f;

	// Token: 0x040006FA RID: 1786
	[Range(1f, 150f)]
	public float Linecount = 90f;
}
