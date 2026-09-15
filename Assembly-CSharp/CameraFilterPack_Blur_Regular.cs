using System;
using UnityEngine;

// Token: 0x0200004D RID: 77
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Regular")]
public class CameraFilterPack_Blur_Regular : MonoBehaviour
{
	// Token: 0x1700004A RID: 74
	// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000C1DD File Offset: 0x0000A3DD
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

	// Token: 0x060001FA RID: 506 RVA: 0x0000C211 File Offset: 0x0000A411
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Blur_Regular");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060001FB RID: 507 RVA: 0x0000C234 File Offset: 0x0000A434
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
			this.material.SetFloat("_Level", (float)this.Level);
			this.material.SetVector("_Distance", this.Distance);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060001FC RID: 508 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060001FD RID: 509 RVA: 0x0000C306 File Offset: 0x0000A506
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400030E RID: 782
	public Shader SCShader;

	// Token: 0x0400030F RID: 783
	private float TimeX = 1f;

	// Token: 0x04000310 RID: 784
	private Vector4 ScreenResolution;

	// Token: 0x04000311 RID: 785
	private Material SCMaterial;

	// Token: 0x04000312 RID: 786
	[Range(1f, 16f)]
	public int Level = 4;

	// Token: 0x04000313 RID: 787
	public Vector2 Distance = new Vector2(30f, 0f);
}
