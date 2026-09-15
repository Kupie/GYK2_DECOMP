using System;
using UnityEngine;

// Token: 0x020000C5 RID: 197
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Gradients/Neon")]
public class CameraFilterPack_Gradients_NeonGradient : MonoBehaviour
{
	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x060004C6 RID: 1222 RVA: 0x0001786E File Offset: 0x00015A6E
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

	// Token: 0x060004C7 RID: 1223 RVA: 0x000178A2 File Offset: 0x00015AA2
	private void Start()
	{
		this.SCShader = Shader.Find(this.ShaderName);
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x000178C4 File Offset: 0x00015AC4
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

	// Token: 0x060004C9 RID: 1225 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x00017990 File Offset: 0x00015B90
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400064D RID: 1613
	public Shader SCShader;

	// Token: 0x0400064E RID: 1614
	private string ShaderName = "CameraFilterPack/Gradients_NeonGradient";

	// Token: 0x0400064F RID: 1615
	private float TimeX = 1f;

	// Token: 0x04000650 RID: 1616
	private Vector4 ScreenResolution;

	// Token: 0x04000651 RID: 1617
	private Material SCMaterial;

	// Token: 0x04000652 RID: 1618
	[Range(0f, 1f)]
	public float Switch = 1f;

	// Token: 0x04000653 RID: 1619
	[Range(0f, 1f)]
	public float Fade = 1f;
}
