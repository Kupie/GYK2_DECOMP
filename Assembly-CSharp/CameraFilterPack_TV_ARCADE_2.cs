using System;
using UnityEngine;

// Token: 0x020000EB RID: 235
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/ARCADE_2")]
public class CameraFilterPack_TV_ARCADE_2 : MonoBehaviour
{
	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x060005BB RID: 1467 RVA: 0x0001BEDA File Offset: 0x0001A0DA
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

	// Token: 0x060005BC RID: 1468 RVA: 0x0001BF0E File Offset: 0x0001A10E
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_ARCADE_2");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x0001BF30 File Offset: 0x0001A130
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
			this.material.SetFloat("_Value", this.Interferance_Size);
			this.material.SetFloat("_Value2", this.Interferance_Speed);
			this.material.SetFloat("_Value3", this.Contrast);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060005BE RID: 1470 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x0001C028 File Offset: 0x0001A228
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000773 RID: 1907
	public Shader SCShader;

	// Token: 0x04000774 RID: 1908
	private float TimeX = 1f;

	// Token: 0x04000775 RID: 1909
	private Vector4 ScreenResolution;

	// Token: 0x04000776 RID: 1910
	private Material SCMaterial;

	// Token: 0x04000777 RID: 1911
	[Range(0f, 10f)]
	public float Interferance_Size = 1f;

	// Token: 0x04000778 RID: 1912
	[Range(0f, 10f)]
	public float Interferance_Speed = 0.5f;

	// Token: 0x04000779 RID: 1913
	[Range(0f, 10f)]
	public float Contrast = 1f;

	// Token: 0x0400077A RID: 1914
	[Range(0f, 10f)]
	private float Value4 = 1f;
}
