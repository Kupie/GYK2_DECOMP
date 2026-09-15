using System;
using UnityEngine;

// Token: 0x020000AC RID: 172
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/Funk")]
public class CameraFilterPack_FX_Funk : MonoBehaviour
{
	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x06000430 RID: 1072 RVA: 0x00015465 File Offset: 0x00013665
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

	// Token: 0x06000431 RID: 1073 RVA: 0x00015499 File Offset: 0x00013699
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_Funk");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x000154BC File Offset: 0x000136BC
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

	// Token: 0x06000433 RID: 1075 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x00015559 File Offset: 0x00013759
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040005B5 RID: 1461
	public Shader SCShader;

	// Token: 0x040005B6 RID: 1462
	private float TimeX = 1f;

	// Token: 0x040005B7 RID: 1463
	private Vector4 ScreenResolution;

	// Token: 0x040005B8 RID: 1464
	private Material SCMaterial;
}
