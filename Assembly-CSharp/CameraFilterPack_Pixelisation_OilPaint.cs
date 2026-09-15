using System;
using UnityEngine;

// Token: 0x020000E0 RID: 224
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Pixelisation/OilPaint")]
public class CameraFilterPack_Pixelisation_OilPaint : MonoBehaviour
{
	// Token: 0x170000DC RID: 220
	// (get) Token: 0x06000578 RID: 1400 RVA: 0x0001AF99 File Offset: 0x00019199
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

	// Token: 0x06000579 RID: 1401 RVA: 0x0001AFCD File Offset: 0x000191CD
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Pixelisation_OilPaint");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x0001AFF0 File Offset: 0x000191F0
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
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x0001B0A6 File Offset: 0x000192A6
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000731 RID: 1841
	public Shader SCShader;

	// Token: 0x04000732 RID: 1842
	private float TimeX = 1f;

	// Token: 0x04000733 RID: 1843
	private Vector4 ScreenResolution;

	// Token: 0x04000734 RID: 1844
	private Material SCMaterial;

	// Token: 0x04000735 RID: 1845
	[Range(0f, 5f)]
	public float Value = 1f;
}
