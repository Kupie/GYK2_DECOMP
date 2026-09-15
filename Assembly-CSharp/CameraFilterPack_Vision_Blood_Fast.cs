using System;
using UnityEngine;

// Token: 0x0200010E RID: 270
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/Blood_Fast")]
public class CameraFilterPack_Vision_Blood_Fast : MonoBehaviour
{
	// Token: 0x1700010A RID: 266
	// (get) Token: 0x0600068D RID: 1677 RVA: 0x0001F459 File Offset: 0x0001D659
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

	// Token: 0x0600068E RID: 1678 RVA: 0x0001F48D File Offset: 0x0001D68D
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Vision_Blood_Fast");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x0001F4B0 File Offset: 0x0001D6B0
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
			this.material.SetFloat("_Value", this.HoleSize);
			this.material.SetFloat("_Value2", this.HoleSmooth);
			this.material.SetFloat("_Value3", this.Color1);
			this.material.SetFloat("_Value4", this.Color2);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x0001F5A8 File Offset: 0x0001D7A8
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000860 RID: 2144
	public Shader SCShader;

	// Token: 0x04000861 RID: 2145
	private float TimeX = 1f;

	// Token: 0x04000862 RID: 2146
	private Vector4 ScreenResolution;

	// Token: 0x04000863 RID: 2147
	private Material SCMaterial;

	// Token: 0x04000864 RID: 2148
	[Range(0.01f, 1f)]
	public float HoleSize = 0.6f;

	// Token: 0x04000865 RID: 2149
	[Range(-1f, 1f)]
	public float HoleSmooth = 0.3f;

	// Token: 0x04000866 RID: 2150
	[Range(-2f, 2f)]
	public float Color1 = 0.2f;

	// Token: 0x04000867 RID: 2151
	[Range(-2f, 2f)]
	public float Color2 = 0.9f;
}
