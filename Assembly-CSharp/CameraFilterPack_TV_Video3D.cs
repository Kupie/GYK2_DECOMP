using System;
using UnityEngine;

// Token: 0x02000102 RID: 258
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/Video3D")]
public class CameraFilterPack_TV_Video3D : MonoBehaviour
{
	// Token: 0x170000FE RID: 254
	// (get) Token: 0x06000645 RID: 1605 RVA: 0x0001E25D File Offset: 0x0001C45D
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

	// Token: 0x06000646 RID: 1606 RVA: 0x0001E291 File Offset: 0x0001C491
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_Video3D");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000647 RID: 1607 RVA: 0x0001E2B4 File Offset: 0x0001C4B4
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

	// Token: 0x06000648 RID: 1608 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x0001E351 File Offset: 0x0001C551
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400080E RID: 2062
	public Shader SCShader;

	// Token: 0x0400080F RID: 2063
	private float TimeX = 1f;

	// Token: 0x04000810 RID: 2064
	private Vector4 ScreenResolution;

	// Token: 0x04000811 RID: 2065
	private Material SCMaterial;
}
