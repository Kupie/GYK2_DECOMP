using System;
using UnityEngine;

// Token: 0x020000C0 RID: 192
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Gradients/Ansi")]
public class CameraFilterPack_Gradients_Ansi : MonoBehaviour
{
	// Token: 0x170000BD RID: 189
	// (get) Token: 0x060004A8 RID: 1192 RVA: 0x0001713C File Offset: 0x0001533C
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

	// Token: 0x060004A9 RID: 1193 RVA: 0x00017170 File Offset: 0x00015370
	private void Start()
	{
		this.SCShader = Shader.Find(this.ShaderName);
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x00017194 File Offset: 0x00015394
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

	// Token: 0x060004AB RID: 1195 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x00017260 File Offset: 0x00015460
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400062A RID: 1578
	public Shader SCShader;

	// Token: 0x0400062B RID: 1579
	private string ShaderName = "CameraFilterPack/Gradients_Ansi";

	// Token: 0x0400062C RID: 1580
	private float TimeX = 1f;

	// Token: 0x0400062D RID: 1581
	private Vector4 ScreenResolution;

	// Token: 0x0400062E RID: 1582
	private Material SCMaterial;

	// Token: 0x0400062F RID: 1583
	[Range(0f, 1f)]
	public float Switch = 1f;

	// Token: 0x04000630 RID: 1584
	[Range(0f, 1f)]
	public float Fade = 1f;
}
