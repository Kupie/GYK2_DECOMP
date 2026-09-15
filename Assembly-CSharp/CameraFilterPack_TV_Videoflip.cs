using System;
using UnityEngine;

// Token: 0x02000103 RID: 259
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/Videoflip")]
public class CameraFilterPack_TV_Videoflip : MonoBehaviour
{
	// Token: 0x170000FF RID: 255
	// (get) Token: 0x0600064B RID: 1611 RVA: 0x0001E37E File Offset: 0x0001C57E
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

	// Token: 0x0600064C RID: 1612 RVA: 0x0001E3B2 File Offset: 0x0001C5B2
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_Videoflip");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x0001E3D4 File Offset: 0x0001C5D4
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

	// Token: 0x0600064E RID: 1614 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x0001E471 File Offset: 0x0001C671
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000812 RID: 2066
	public Shader SCShader;

	// Token: 0x04000813 RID: 2067
	private float TimeX = 1f;

	// Token: 0x04000814 RID: 2068
	private Vector4 ScreenResolution;

	// Token: 0x04000815 RID: 2069
	private Material SCMaterial;
}
