using System;
using UnityEngine;

// Token: 0x0200007F RID: 127
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Comics")]
public class CameraFilterPack_Drawing_Comics : MonoBehaviour
{
	// Token: 0x1700007B RID: 123
	// (get) Token: 0x06000321 RID: 801 RVA: 0x00011101 File Offset: 0x0000F301
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

	// Token: 0x06000322 RID: 802 RVA: 0x00011135 File Offset: 0x0000F335
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Comics");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000323 RID: 803 RVA: 0x00011158 File Offset: 0x0000F358
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
			this.material.SetFloat("_DotSize", this.DotSize);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000324 RID: 804 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000325 RID: 805 RVA: 0x000111DE File Offset: 0x0000F3DE
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400047E RID: 1150
	public Shader SCShader;

	// Token: 0x0400047F RID: 1151
	private float TimeX = 1f;

	// Token: 0x04000480 RID: 1152
	private Material SCMaterial;

	// Token: 0x04000481 RID: 1153
	[Range(0f, 1f)]
	public float DotSize = 0.5f;
}
