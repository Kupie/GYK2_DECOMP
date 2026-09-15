using System;
using UnityEngine;

// Token: 0x020000BB RID: 187
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/SuperDot")]
public class CameraFilterPack_FX_superDot : MonoBehaviour
{
	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x0600048A RID: 1162 RVA: 0x000167E5 File Offset: 0x000149E5
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

	// Token: 0x0600048B RID: 1163 RVA: 0x00016819 File Offset: 0x00014A19
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_superDot");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x0001683C File Offset: 0x00014A3C
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

	// Token: 0x0600048D RID: 1165 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x000168D9 File Offset: 0x00014AD9
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000606 RID: 1542
	public Shader SCShader;

	// Token: 0x04000607 RID: 1543
	private float TimeX = 1f;

	// Token: 0x04000608 RID: 1544
	private Vector4 ScreenResolution;

	// Token: 0x04000609 RID: 1545
	private Material SCMaterial;
}
