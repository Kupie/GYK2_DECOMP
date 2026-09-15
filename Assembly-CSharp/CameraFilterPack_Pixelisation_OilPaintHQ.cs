using System;
using UnityEngine;

// Token: 0x020000E1 RID: 225
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Pixelisation/OilPaintHQ")]
public class CameraFilterPack_Pixelisation_OilPaintHQ : MonoBehaviour
{
	// Token: 0x170000DD RID: 221
	// (get) Token: 0x0600057E RID: 1406 RVA: 0x0001B0DE File Offset: 0x000192DE
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

	// Token: 0x0600057F RID: 1407 RVA: 0x0001B112 File Offset: 0x00019312
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Pixelisation_OilPaintHQ");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x0001B134 File Offset: 0x00019334
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

	// Token: 0x06000581 RID: 1409 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x0001B1EA File Offset: 0x000193EA
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000736 RID: 1846
	public Shader SCShader;

	// Token: 0x04000737 RID: 1847
	private float TimeX = 1f;

	// Token: 0x04000738 RID: 1848
	private Vector4 ScreenResolution;

	// Token: 0x04000739 RID: 1849
	private Material SCMaterial;

	// Token: 0x0400073A RID: 1850
	[Range(0f, 5f)]
	public float Value = 2f;
}
