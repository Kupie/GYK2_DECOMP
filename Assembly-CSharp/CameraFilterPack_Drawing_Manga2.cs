using System;
using UnityEngine;

// Token: 0x02000087 RID: 135
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Manga2")]
public class CameraFilterPack_Drawing_Manga2 : MonoBehaviour
{
	// Token: 0x17000083 RID: 131
	// (get) Token: 0x06000351 RID: 849 RVA: 0x00011BCA File Offset: 0x0000FDCA
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

	// Token: 0x06000352 RID: 850 RVA: 0x00011BFE File Offset: 0x0000FDFE
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Manga2");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000353 RID: 851 RVA: 0x00011C20 File Offset: 0x0000FE20
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
			this.material.SetFloat("_DotSize", this.DotSize);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000354 RID: 852 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000355 RID: 853 RVA: 0x00011CA6 File Offset: 0x0000FEA6
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040004AC RID: 1196
	public Shader SCShader;

	// Token: 0x040004AD RID: 1197
	private float TimeX = 1f;

	// Token: 0x040004AE RID: 1198
	private Material SCMaterial;

	// Token: 0x040004AF RID: 1199
	[Range(1f, 8f)]
	public float DotSize = 4.72f;
}
