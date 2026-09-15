using System;
using UnityEngine;

// Token: 0x020000B5 RID: 181
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/Mirror")]
public class CameraFilterPack_FX_Mirror : MonoBehaviour
{
	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x06000466 RID: 1126 RVA: 0x00015FA6 File Offset: 0x000141A6
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

	// Token: 0x06000467 RID: 1127 RVA: 0x00015FDA File Offset: 0x000141DA
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_Mirror");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x00015FFC File Offset: 0x000141FC
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

	// Token: 0x06000469 RID: 1129 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x00016099 File Offset: 0x00014299
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040005E3 RID: 1507
	public Shader SCShader;

	// Token: 0x040005E4 RID: 1508
	private float TimeX = 1f;

	// Token: 0x040005E5 RID: 1509
	private Vector4 ScreenResolution;

	// Token: 0x040005E6 RID: 1510
	private Material SCMaterial;
}
