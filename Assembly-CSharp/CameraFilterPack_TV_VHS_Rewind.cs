using System;
using UnityEngine;

// Token: 0x02000101 RID: 257
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/VHS/VHS_Rewind")]
public class CameraFilterPack_TV_VHS_Rewind : MonoBehaviour
{
	// Token: 0x170000FD RID: 253
	// (get) Token: 0x0600063F RID: 1599 RVA: 0x0001E0B6 File Offset: 0x0001C2B6
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

	// Token: 0x06000640 RID: 1600 RVA: 0x0001E0EA File Offset: 0x0001C2EA
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_VHS_Rewind");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x0001E10C File Offset: 0x0001C30C
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
			this.material.SetFloat("_Value", this.Cryptage);
			this.material.SetFloat("_Value2", this.Parasite);
			this.material.SetFloat("_Value3", this.Parasite2);
			this.material.SetFloat("_Value4", this.WhiteParasite);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x0001E204 File Offset: 0x0001C404
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000806 RID: 2054
	public Shader SCShader;

	// Token: 0x04000807 RID: 2055
	private float TimeX = 1f;

	// Token: 0x04000808 RID: 2056
	private Vector4 ScreenResolution;

	// Token: 0x04000809 RID: 2057
	private Material SCMaterial;

	// Token: 0x0400080A RID: 2058
	[Range(0f, 1f)]
	public float Cryptage = 1f;

	// Token: 0x0400080B RID: 2059
	[Range(-20f, 20f)]
	public float Parasite = 9f;

	// Token: 0x0400080C RID: 2060
	[Range(-20f, 20f)]
	public float Parasite2 = 12f;

	// Token: 0x0400080D RID: 2061
	[Range(0f, 1f)]
	private float WhiteParasite = 1f;
}
