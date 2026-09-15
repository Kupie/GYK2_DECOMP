using System;
using UnityEngine;

// Token: 0x02000080 RID: 128
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Crosshatch")]
public class CameraFilterPack_Drawing_Crosshatch : MonoBehaviour
{
	// Token: 0x1700007C RID: 124
	// (get) Token: 0x06000327 RID: 807 RVA: 0x00011216 File Offset: 0x0000F416
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

	// Token: 0x06000328 RID: 808 RVA: 0x0001124A File Offset: 0x0000F44A
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Crosshatch");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000329 RID: 809 RVA: 0x0001126C File Offset: 0x0000F46C
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
			this.material.SetFloat("_Distortion", this.Width);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600032A RID: 810 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600032B RID: 811 RVA: 0x00011322 File Offset: 0x0000F522
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000482 RID: 1154
	public Shader SCShader;

	// Token: 0x04000483 RID: 1155
	private float TimeX = 1f;

	// Token: 0x04000484 RID: 1156
	private Vector4 ScreenResolution;

	// Token: 0x04000485 RID: 1157
	private Material SCMaterial;

	// Token: 0x04000486 RID: 1158
	[Range(1f, 10f)]
	public float Width = 2f;
}
