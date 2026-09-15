using System;
using UnityEngine;

// Token: 0x02000065 RID: 101
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/RGB")]
public class CameraFilterPack_Color_RGB : MonoBehaviour
{
	// Token: 0x17000061 RID: 97
	// (get) Token: 0x06000285 RID: 645 RVA: 0x0000EB1A File Offset: 0x0000CD1A
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

	// Token: 0x06000286 RID: 646 RVA: 0x0000EB4E File Offset: 0x0000CD4E
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Color_RGB");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000287 RID: 647 RVA: 0x0000EB70 File Offset: 0x0000CD70
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
			this.material.SetColor("_ColorRGB", this.ColorRGB);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000288 RID: 648 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000289 RID: 649 RVA: 0x0000EC26 File Offset: 0x0000CE26
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040003D0 RID: 976
	public Shader SCShader;

	// Token: 0x040003D1 RID: 977
	private float TimeX = 1f;

	// Token: 0x040003D2 RID: 978
	private Vector4 ScreenResolution;

	// Token: 0x040003D3 RID: 979
	private Material SCMaterial;

	// Token: 0x040003D4 RID: 980
	public Color ColorRGB = new Color(1f, 1f, 1f);
}
