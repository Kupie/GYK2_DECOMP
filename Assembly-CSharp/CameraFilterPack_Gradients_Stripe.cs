using System;
using UnityEngine;

// Token: 0x020000C7 RID: 199
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Gradients/Stripe")]
public class CameraFilterPack_Gradients_Stripe : MonoBehaviour
{
	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x060004D2 RID: 1234 RVA: 0x00017B4E File Offset: 0x00015D4E
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

	// Token: 0x060004D3 RID: 1235 RVA: 0x00017B82 File Offset: 0x00015D82
	private void Start()
	{
		this.SCShader = Shader.Find(this.ShaderName);
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x00017BA4 File Offset: 0x00015DA4
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

	// Token: 0x060004D5 RID: 1237 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x00017C70 File Offset: 0x00015E70
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400065B RID: 1627
	public Shader SCShader;

	// Token: 0x0400065C RID: 1628
	private string ShaderName = "CameraFilterPack/Gradients_Stripe";

	// Token: 0x0400065D RID: 1629
	private float TimeX = 1f;

	// Token: 0x0400065E RID: 1630
	private Vector4 ScreenResolution;

	// Token: 0x0400065F RID: 1631
	private Material SCMaterial;

	// Token: 0x04000660 RID: 1632
	[Range(0f, 1f)]
	public float Switch = 1f;

	// Token: 0x04000661 RID: 1633
	[Range(0f, 1f)]
	public float Fade = 1f;
}
