using System;
using UnityEngine;

// Token: 0x020000C2 RID: 194
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Gradients/Electric")]
public class CameraFilterPack_Gradients_ElectricGradient : MonoBehaviour
{
	// Token: 0x170000BF RID: 191
	// (get) Token: 0x060004B4 RID: 1204 RVA: 0x0001741E File Offset: 0x0001561E
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

	// Token: 0x060004B5 RID: 1205 RVA: 0x00017452 File Offset: 0x00015652
	private void Start()
	{
		this.SCShader = Shader.Find(this.ShaderName);
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x00017474 File Offset: 0x00015674
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

	// Token: 0x060004B7 RID: 1207 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x00017540 File Offset: 0x00015740
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000638 RID: 1592
	public Shader SCShader;

	// Token: 0x04000639 RID: 1593
	private string ShaderName = "CameraFilterPack/Gradients_ElectricGradient";

	// Token: 0x0400063A RID: 1594
	private float TimeX = 1f;

	// Token: 0x0400063B RID: 1595
	private Vector4 ScreenResolution;

	// Token: 0x0400063C RID: 1596
	private Material SCMaterial;

	// Token: 0x0400063D RID: 1597
	[Range(0f, 1f)]
	public float Switch = 1f;

	// Token: 0x0400063E RID: 1598
	[Range(0f, 1f)]
	public float Fade = 1f;
}
