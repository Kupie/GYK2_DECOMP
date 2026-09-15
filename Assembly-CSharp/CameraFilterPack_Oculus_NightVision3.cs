using System;
using UnityEngine;

// Token: 0x020000DA RID: 218
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Night Vision/Night Vision 3")]
public class CameraFilterPack_Oculus_NightVision3 : MonoBehaviour
{
	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x06000552 RID: 1362 RVA: 0x0001A47C File Offset: 0x0001867C
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

	// Token: 0x06000553 RID: 1363 RVA: 0x0001A4B0 File Offset: 0x000186B0
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Oculus_NightVision3");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x0001A4D4 File Offset: 0x000186D4
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
			this.material.SetFloat("_Greenness", this.Greenness);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x0001A583 File Offset: 0x00018783
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000702 RID: 1794
	public Shader SCShader;

	// Token: 0x04000703 RID: 1795
	private float TimeX = 1f;

	// Token: 0x04000704 RID: 1796
	private Vector4 ScreenResolution;

	// Token: 0x04000705 RID: 1797
	private Material SCMaterial;

	// Token: 0x04000706 RID: 1798
	[Range(0.2f, 2f)]
	public float Greenness = 1f;
}
