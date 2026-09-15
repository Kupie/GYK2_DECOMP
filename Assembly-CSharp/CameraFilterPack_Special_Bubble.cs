using System;
using UnityEngine;

// Token: 0x020000E7 RID: 231
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Special/Bubble")]
public class CameraFilterPack_Special_Bubble : MonoBehaviour
{
	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x060005A3 RID: 1443 RVA: 0x0001B98B File Offset: 0x00019B8B
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

	// Token: 0x060005A4 RID: 1444 RVA: 0x0001B9BF File Offset: 0x00019BBF
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Special_Bubble");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x0001B9E0 File Offset: 0x00019BE0
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
			this.material.SetFloat("_Value", this.X);
			this.material.SetFloat("_Value2", this.Y);
			this.material.SetFloat("_Value3", this.Rate);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x0001BAD8 File Offset: 0x00019CD8
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400075D RID: 1885
	public Shader SCShader;

	// Token: 0x0400075E RID: 1886
	private float TimeX = 1f;

	// Token: 0x0400075F RID: 1887
	private Vector4 ScreenResolution;

	// Token: 0x04000760 RID: 1888
	private Material SCMaterial;

	// Token: 0x04000761 RID: 1889
	[Range(-4f, 4f)]
	public float X = 0.5f;

	// Token: 0x04000762 RID: 1890
	[Range(-4f, 4f)]
	public float Y = 0.5f;

	// Token: 0x04000763 RID: 1891
	[Range(0f, 5f)]
	public float Rate = 1f;

	// Token: 0x04000764 RID: 1892
	[Range(0f, 10f)]
	private float Value4 = 1f;
}
