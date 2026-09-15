using System;
using UnityEngine;

// Token: 0x020000C9 RID: 201
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Gradients/Thermal")]
public class CameraFilterPack_Gradients_Therma : MonoBehaviour
{
	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x060004DE RID: 1246 RVA: 0x00017E2E File Offset: 0x0001602E
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

	// Token: 0x060004DF RID: 1247 RVA: 0x00017E62 File Offset: 0x00016062
	private void Start()
	{
		this.SCShader = Shader.Find(this.ShaderName);
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x00017E84 File Offset: 0x00016084
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

	// Token: 0x060004E1 RID: 1249 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x00017F50 File Offset: 0x00016150
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000669 RID: 1641
	public Shader SCShader;

	// Token: 0x0400066A RID: 1642
	private string ShaderName = "CameraFilterPack/Gradients_Therma";

	// Token: 0x0400066B RID: 1643
	private float TimeX = 1f;

	// Token: 0x0400066C RID: 1644
	private Vector4 ScreenResolution;

	// Token: 0x0400066D RID: 1645
	private Material SCMaterial;

	// Token: 0x0400066E RID: 1646
	[Range(0f, 1f)]
	public float Switch = 1f;

	// Token: 0x0400066F RID: 1647
	[Range(0f, 1f)]
	public float Fade = 1f;
}
