using System;
using UnityEngine;

// Token: 0x020000E9 RID: 233
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/80s")]
public class CameraFilterPack_TV_80 : MonoBehaviour
{
	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x060005AF RID: 1455 RVA: 0x0001BC76 File Offset: 0x00019E76
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

	// Token: 0x060005B0 RID: 1456 RVA: 0x0001BCAA File Offset: 0x00019EAA
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_80");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x0001BCCC File Offset: 0x00019ECC
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
			this.material.SetFloat("_Fade", this.Fade);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x0001BD82 File Offset: 0x00019F82
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400076A RID: 1898
	public Shader SCShader;

	// Token: 0x0400076B RID: 1899
	private float TimeX = 1f;

	// Token: 0x0400076C RID: 1900
	private Vector4 ScreenResolution;

	// Token: 0x0400076D RID: 1901
	private Material SCMaterial;

	// Token: 0x0400076E RID: 1902
	[Range(0f, 1f)]
	public float Fade = 1f;
}
