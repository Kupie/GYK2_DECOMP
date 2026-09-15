using System;
using UnityEngine;

// Token: 0x020000C3 RID: 195
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Gradients/Fire")]
public class CameraFilterPack_Gradients_FireGradient : MonoBehaviour
{
	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x060004BA RID: 1210 RVA: 0x0001758E File Offset: 0x0001578E
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

	// Token: 0x060004BB RID: 1211 RVA: 0x000175C2 File Offset: 0x000157C2
	private void Start()
	{
		this.SCShader = Shader.Find(this.ShaderName);
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x000175E4 File Offset: 0x000157E4
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

	// Token: 0x060004BD RID: 1213 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x000176B0 File Offset: 0x000158B0
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400063F RID: 1599
	public Shader SCShader;

	// Token: 0x04000640 RID: 1600
	private string ShaderName = "CameraFilterPack/Gradients_FireGradient";

	// Token: 0x04000641 RID: 1601
	private float TimeX = 1f;

	// Token: 0x04000642 RID: 1602
	private Vector4 ScreenResolution;

	// Token: 0x04000643 RID: 1603
	private Material SCMaterial;

	// Token: 0x04000644 RID: 1604
	[Range(0f, 1f)]
	public float Switch = 1f;

	// Token: 0x04000645 RID: 1605
	[Range(0f, 1f)]
	public float Fade = 1f;
}
