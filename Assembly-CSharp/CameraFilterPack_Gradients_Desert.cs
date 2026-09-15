using System;
using UnityEngine;

// Token: 0x020000C1 RID: 193
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Gradients/Desert")]
public class CameraFilterPack_Gradients_Desert : MonoBehaviour
{
	// Token: 0x170000BE RID: 190
	// (get) Token: 0x060004AE RID: 1198 RVA: 0x000172AE File Offset: 0x000154AE
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

	// Token: 0x060004AF RID: 1199 RVA: 0x000172E2 File Offset: 0x000154E2
	private void Start()
	{
		this.SCShader = Shader.Find(this.ShaderName);
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x00017304 File Offset: 0x00015504
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
			this.material.SetFloat("_Value", this.Switch);
			this.material.SetFloat("_Value2", this.Fade);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x000173D0 File Offset: 0x000155D0
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000631 RID: 1585
	public Shader SCShader;

	// Token: 0x04000632 RID: 1586
	private string ShaderName = "CameraFilterPack/Gradients_Desert";

	// Token: 0x04000633 RID: 1587
	private float TimeX = 1f;

	// Token: 0x04000634 RID: 1588
	private Vector4 ScreenResolution;

	// Token: 0x04000635 RID: 1589
	private Material SCMaterial;

	// Token: 0x04000636 RID: 1590
	[Range(0f, 1f)]
	public float Switch = 1f;

	// Token: 0x04000637 RID: 1591
	[Range(0f, 1f)]
	public float Fade = 1f;
}
