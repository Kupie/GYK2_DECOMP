using System;
using UnityEngine;

// Token: 0x020000A8 RID: 168
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/Dot_Circle")]
public class CameraFilterPack_FX_Dot_Circle : MonoBehaviour
{
	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x06000418 RID: 1048 RVA: 0x00014D78 File Offset: 0x00012F78
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

	// Token: 0x06000419 RID: 1049 RVA: 0x00014DAC File Offset: 0x00012FAC
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_Dot_Circle");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x00014DD0 File Offset: 0x00012FD0
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
			this.material.SetFloat("_Value", this.Value);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x00014E86 File Offset: 0x00013086
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000591 RID: 1425
	public Shader SCShader;

	// Token: 0x04000592 RID: 1426
	private float TimeX = 1f;

	// Token: 0x04000593 RID: 1427
	private Vector4 ScreenResolution;

	// Token: 0x04000594 RID: 1428
	private Material SCMaterial;

	// Token: 0x04000595 RID: 1429
	[Range(4f, 32f)]
	public float Value = 7f;
}
