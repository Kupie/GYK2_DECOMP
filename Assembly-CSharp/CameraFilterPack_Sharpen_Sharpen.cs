using System;
using UnityEngine;

// Token: 0x020000E6 RID: 230
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Sharpen/Sharpen")]
public class CameraFilterPack_Sharpen_Sharpen : MonoBehaviour
{
	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x0600059D RID: 1437 RVA: 0x0001B826 File Offset: 0x00019A26
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

	// Token: 0x0600059E RID: 1438 RVA: 0x0001B85A File Offset: 0x00019A5A
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Sharpen_Sharpen");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x0001B87C File Offset: 0x00019A7C
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
			this.material.SetFloat("_Value2", this.Value2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x0001B948 File Offset: 0x00019B48
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000757 RID: 1879
	public Shader SCShader;

	// Token: 0x04000758 RID: 1880
	[Range(0.001f, 100f)]
	public float Value = 4f;

	// Token: 0x04000759 RID: 1881
	[Range(0.001f, 32f)]
	public float Value2 = 1f;

	// Token: 0x0400075A RID: 1882
	private float TimeX = 1f;

	// Token: 0x0400075B RID: 1883
	private Vector4 ScreenResolution;

	// Token: 0x0400075C RID: 1884
	private Material SCMaterial;
}
