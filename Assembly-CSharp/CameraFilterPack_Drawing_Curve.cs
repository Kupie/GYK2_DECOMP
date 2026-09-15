using System;
using UnityEngine;

// Token: 0x02000081 RID: 129
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Curve")]
public class CameraFilterPack_Drawing_Curve : MonoBehaviour
{
	// Token: 0x1700007D RID: 125
	// (get) Token: 0x0600032D RID: 813 RVA: 0x0001135A File Offset: 0x0000F55A
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

	// Token: 0x0600032E RID: 814 RVA: 0x0001138E File Offset: 0x0000F58E
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Curve");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600032F RID: 815 RVA: 0x000113B0 File Offset: 0x0000F5B0
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
			this.material.SetFloat("_Value", this.Size);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000330 RID: 816 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000331 RID: 817 RVA: 0x00011466 File Offset: 0x0000F666
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000487 RID: 1159
	public Shader SCShader;

	// Token: 0x04000488 RID: 1160
	private float TimeX = 1f;

	// Token: 0x04000489 RID: 1161
	private Vector4 ScreenResolution;

	// Token: 0x0400048A RID: 1162
	private Material SCMaterial;

	// Token: 0x0400048B RID: 1163
	[Range(3f, 5f)]
	public float Size = 1f;
}
