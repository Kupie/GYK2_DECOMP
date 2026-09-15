using System;
using UnityEngine;

// Token: 0x0200004E RID: 78
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Steam")]
public class CameraFilterPack_Blur_Steam : MonoBehaviour
{
	// Token: 0x1700004B RID: 75
	// (get) Token: 0x060001FF RID: 511 RVA: 0x0000C34F File Offset: 0x0000A54F
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

	// Token: 0x06000200 RID: 512 RVA: 0x0000C383 File Offset: 0x0000A583
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Blur_Steam");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000201 RID: 513 RVA: 0x0000C3A4 File Offset: 0x0000A5A4
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
			this.material.SetFloat("_Radius", this.Radius);
			this.material.SetFloat("_Quality", this.Quality);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000202 RID: 514 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000203 RID: 515 RVA: 0x0000C469 File Offset: 0x0000A669
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000314 RID: 788
	public Shader SCShader;

	// Token: 0x04000315 RID: 789
	private float TimeX = 1f;

	// Token: 0x04000316 RID: 790
	private Vector4 ScreenResolution;

	// Token: 0x04000317 RID: 791
	private Material SCMaterial;

	// Token: 0x04000318 RID: 792
	[Range(0f, 1f)]
	public float Radius = 0.1f;

	// Token: 0x04000319 RID: 793
	[Range(0f, 1f)]
	public float Quality = 0.75f;
}
