using System;
using UnityEngine;

// Token: 0x020000F7 RID: 247
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/Noise")]
public class CameraFilterPack_TV_Noise : MonoBehaviour
{
	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x06000603 RID: 1539 RVA: 0x0001D39A File Offset: 0x0001B59A
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

	// Token: 0x06000604 RID: 1540 RVA: 0x0001D3CE File Offset: 0x0001B5CE
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_Noise");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x0001D3F0 File Offset: 0x0001B5F0
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

	// Token: 0x06000606 RID: 1542 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x0001D4A6 File Offset: 0x0001B6A6
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040007CE RID: 1998
	public Shader SCShader;

	// Token: 0x040007CF RID: 1999
	private float TimeX = 1f;

	// Token: 0x040007D0 RID: 2000
	private Vector4 ScreenResolution;

	// Token: 0x040007D1 RID: 2001
	private Material SCMaterial;

	// Token: 0x040007D2 RID: 2002
	[Range(0.0001f, 1f)]
	public float Fade = 0.01f;
}
