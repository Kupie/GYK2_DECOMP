using System;
using UnityEngine;

// Token: 0x020000C6 RID: 198
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Gradients/Rainbow")]
public class CameraFilterPack_Gradients_Rainbow : MonoBehaviour
{
	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x060004CC RID: 1228 RVA: 0x000179DE File Offset: 0x00015BDE
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

	// Token: 0x060004CD RID: 1229 RVA: 0x00017A12 File Offset: 0x00015C12
	private void Start()
	{
		this.SCShader = Shader.Find(this.ShaderName);
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x00017A34 File Offset: 0x00015C34
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

	// Token: 0x060004CF RID: 1231 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x00017B00 File Offset: 0x00015D00
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000654 RID: 1620
	public Shader SCShader;

	// Token: 0x04000655 RID: 1621
	private string ShaderName = "CameraFilterPack/Gradients_Rainbow";

	// Token: 0x04000656 RID: 1622
	private float TimeX = 1f;

	// Token: 0x04000657 RID: 1623
	private Vector4 ScreenResolution;

	// Token: 0x04000658 RID: 1624
	private Material SCMaterial;

	// Token: 0x04000659 RID: 1625
	[Range(0f, 1f)]
	public float Switch = 1f;

	// Token: 0x0400065A RID: 1626
	[Range(0f, 1f)]
	public float Fade = 1f;
}
