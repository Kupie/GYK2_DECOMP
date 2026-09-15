using System;
using UnityEngine;

// Token: 0x020000DF RID: 223
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Pixelisation/Dot")]
public class CameraFilterPack_Pixelisation_Dot : MonoBehaviour
{
	// Token: 0x170000DB RID: 219
	// (get) Token: 0x06000572 RID: 1394 RVA: 0x0001ADF3 File Offset: 0x00018FF3
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

	// Token: 0x06000573 RID: 1395 RVA: 0x0001AE27 File Offset: 0x00019027
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Pixelisation_Dot");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x0001AE48 File Offset: 0x00019048
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
			this.material.SetFloat("_Value", this.Size);
			this.material.SetFloat("_Value2", this.LightBackGround);
			this.material.SetFloat("_Value3", this.Speed);
			this.material.SetFloat("_Value4", this.Size2);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x0001AF40 File Offset: 0x00019140
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000729 RID: 1833
	public Shader SCShader;

	// Token: 0x0400072A RID: 1834
	private float TimeX = 1f;

	// Token: 0x0400072B RID: 1835
	private Vector4 ScreenResolution;

	// Token: 0x0400072C RID: 1836
	private Material SCMaterial;

	// Token: 0x0400072D RID: 1837
	[Range(0.0001f, 0.5f)]
	public float Size = 0.005f;

	// Token: 0x0400072E RID: 1838
	[Range(0f, 1f)]
	public float LightBackGround = 0.3f;

	// Token: 0x0400072F RID: 1839
	[Range(0f, 10f)]
	private float Speed = 1f;

	// Token: 0x04000730 RID: 1840
	[Range(0f, 10f)]
	private float Size2 = 1f;
}
