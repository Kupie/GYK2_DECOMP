using System;
using UnityEngine;

// Token: 0x020000FE RID: 254
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/Tiles")]
public class CameraFilterPack_TV_Tiles : MonoBehaviour
{
	// Token: 0x170000FA RID: 250
	// (get) Token: 0x0600062D RID: 1581 RVA: 0x0001DC5E File Offset: 0x0001BE5E
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

	// Token: 0x0600062E RID: 1582 RVA: 0x0001DC92 File Offset: 0x0001BE92
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_Tiles");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x0001DCB4 File Offset: 0x0001BEB4
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
			this.material.SetFloat("_Value2", this.Intensity);
			this.material.SetFloat("_Value3", this.StretchX);
			this.material.SetFloat("_Value4", this.StretchY);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x0001DDAC File Offset: 0x0001BFAC
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040007F2 RID: 2034
	public Shader SCShader;

	// Token: 0x040007F3 RID: 2035
	private float TimeX = 1f;

	// Token: 0x040007F4 RID: 2036
	private Vector4 ScreenResolution;

	// Token: 0x040007F5 RID: 2037
	private Material SCMaterial;

	// Token: 0x040007F6 RID: 2038
	[Range(0.5f, 2f)]
	public float Size = 1f;

	// Token: 0x040007F7 RID: 2039
	[Range(0f, 10f)]
	public float Intensity = 4f;

	// Token: 0x040007F8 RID: 2040
	[Range(0f, 1f)]
	public float StretchX = 0.6f;

	// Token: 0x040007F9 RID: 2041
	[Range(0f, 1f)]
	public float StretchY = 0.4f;
}
