using System;
using UnityEngine;

// Token: 0x020000FD RID: 253
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/RGB Display")]
public class CameraFilterPack_TV_Rgb : MonoBehaviour
{
	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x06000627 RID: 1575 RVA: 0x0001DB1A File Offset: 0x0001BD1A
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

	// Token: 0x06000628 RID: 1576 RVA: 0x0001DB4E File Offset: 0x0001BD4E
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_Rgb");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x0001DB70 File Offset: 0x0001BD70
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
			this.material.SetFloat("_Distortion", this.Distortion);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600062A RID: 1578 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x0001DC26 File Offset: 0x0001BE26
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040007ED RID: 2029
	public Shader SCShader;

	// Token: 0x040007EE RID: 2030
	private Vector4 ScreenResolution;

	// Token: 0x040007EF RID: 2031
	private float TimeX = 1f;

	// Token: 0x040007F0 RID: 2032
	[Range(0.01f, 4f)]
	public float Distortion = 1f;

	// Token: 0x040007F1 RID: 2033
	private Material SCMaterial;
}
