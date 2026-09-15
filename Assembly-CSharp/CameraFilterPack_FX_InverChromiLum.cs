using System;
using UnityEngine;

// Token: 0x020000B4 RID: 180
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/InverChromiLum")]
public class CameraFilterPack_FX_InverChromiLum : MonoBehaviour
{
	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x06000460 RID: 1120 RVA: 0x00015E86 File Offset: 0x00014086
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

	// Token: 0x06000461 RID: 1121 RVA: 0x00015EBA File Offset: 0x000140BA
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_InverChromiLum");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x00015EDC File Offset: 0x000140DC
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
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x00015F79 File Offset: 0x00014179
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040005DF RID: 1503
	public Shader SCShader;

	// Token: 0x040005E0 RID: 1504
	private float TimeX = 1f;

	// Token: 0x040005E1 RID: 1505
	private Vector4 ScreenResolution;

	// Token: 0x040005E2 RID: 1506
	private Material SCMaterial;
}
